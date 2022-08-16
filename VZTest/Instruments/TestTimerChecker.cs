using System.Timers;
using VZTest.Models.DataModels.Test;
using VZTest.Repository.IRepository;

namespace VZTest.Instruments
{
    public class TestTimerChecker
    {
        private System.Timers.Timer timer;
        private IUnitOfWork unitOfWork;
        public TestTimerChecker(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            timer = new System.Timers.Timer(20000);
            timer.AutoReset = true;
            timer.Elapsed += CheckTestsEnded;
            timer.Start();
        }
        private async void CheckTestsEnded(object? obj, ElapsedEventArgs e)
        {
            if (unitOfWork == null)
            {
                Console.WriteLine("Checking Tests Cancelled");
                return;
            }
            Console.WriteLine("Started Checking Tests");
            //Немного сложное условие, поэтому оставлю разъяснение:
            //Беру все активные вопросы, у которых обязательно есть время окончания и оно в пределах 4 часов до текущено времени и
            //у которого время начала либо null либо больше текущего времени
            //Из этого всего достаю только Id и привожу в List, чтобы не откладывать исполнение LINQ запроса
            DateTime checkTime = DateTime.Now; //записываю в переменную, так как время может поменяться во время операции
            List<int> testIds = unitOfWork.TestRepository.GetWhere(x => x.Opened && x.EndTime != null).ToList() //Делаю так как полносью эта проверка не может быть автоматически преобразована в SQL
                .Where(x => (x.StartTime == null || DateTime.Compare(checkTime, x.StartTime.Value) > 0) &&
                DateTime.Compare(checkTime, x.EndTime.Value) > 0 && (checkTime - x.EndTime.Value).TotalHours <= 4)
                .Select(x => x.Id).ToList();
            int attemptUpdateAmount = 0;
            foreach (int testId in testIds)
            {
                IEnumerable<Attempt> attempts = unitOfWork.GetTestAttempts(testId, true).Where(x => x.Active);
                attemptUpdateAmount += attempts.Count(); // Не делаю ToList так как Count() и так вызывает принудительное выполнение
                foreach (Attempt attempt in attempts)
                {
                    unitOfWork.CheckAttempt(attempt);
                    unitOfWork.UpdateAttempt(attempt);
                }
            }
            Console.WriteLine($"Всего обновлено попыток: {attemptUpdateAmount}");
            if (attemptUpdateAmount > 0)
            {
                await unitOfWork.SaveAsync();
            }
        }
    }
}
