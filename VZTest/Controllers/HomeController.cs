using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VZTest.Models;
using VZTest.Models.Test;
using VZTest.Models.Test.CorrectAnswers;
using VZTest.Repository.IRepository;

namespace VZTest.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            Question question1 = new Question();
            question1.Title = "Введите 123";
            question1.Number = 0;
            question1.Balls = 1;
            question1.Type = QuestionType.Int;
            CorrectIntAnswer answer1 = new CorrectIntAnswer();
            answer1.Correct = 123;
            question1.CorrectAnswer = answer1;

            Question question2 = new Question();
            question2.Title = "Введите ZOV";
            question2.Number = 1;
            question2.Balls = 1;
            question2.Type = QuestionType.Text;
            CorrectTextAnswer answer2 = new CorrectTextAnswer();
            answer2.Correct = "ZOV";
            question2.CorrectAnswer = answer2;

            Question question3 = new Question();
            question3.Title = "Введите 01.05.2020";
            question3.Number = 2;
            question3.Balls = 1;
            question3.Type = QuestionType.Date;
            CorrectDateAnswer answer3 = new CorrectDateAnswer();
            answer3.Correct = new DateTime(2020,05,01);
            question3.CorrectAnswer = answer3;

            Question question4 = new Question();
            question4.Title = "Введите 24.24";
            question4.Number = 2;
            question4.Balls = 1.5;
            question4.Type = QuestionType.Double;
            CorrectDoubleAnswer answer4 = new CorrectDoubleAnswer();
            answer4.Correct = 24.24;
            question4.CorrectAnswer = answer4;

            Question question5 = new Question();
            question5.Title = "Выберите АБОБА";
            question5.Number = 2;
            question5.Balls = 1.5;
            question5.Type = QuestionType.Radio;
            Option option1 = new Option();
            option1.Title = "АБИБА";
            Option option2 = new Option();
            option2.Title = "АБОБА";
            Option option3 = new Option();
            option3.Title = "АПЧИХБА";
            question5.Options = new List<Option>() { option1, option2, option3 };

            Test test = new Test();
            test.Title = "Второй тест";
            test.CreatedTime = DateTime.Now;
            test.UserId = "";
            test.Description = "Описание крутого опроса";
            test.Opened = true;
            test.Public = true;
            test.MaxAttempts = 3;
            test.Questions = new List<Question>() { question1, question2, question3, question4, question5 };
            //await unitOfWork.AddTest(test);
            //await unitOfWork.SaveAsync();
            //CorrectIntAnswer answer5 = new CorrectIntAnswer();
            //answer5.QuestionId = question5.Id;
            //answer5.Correct = option2.Id;
            //await unitOfWork.CorrectAnswerRepository.AddAsync(answer5);
            //await unitOfWork.SaveAsync();
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}