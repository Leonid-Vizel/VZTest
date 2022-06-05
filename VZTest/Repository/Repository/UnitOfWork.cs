using VZTest.Data;
using VZTest.Models.Test;
using VZTest.Repository.IRepository;

namespace VZTest.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db;

        public IAnswerRepository AnswerRepository { get; set; }
        public IAttemptRepository AttemptRepository { get; set; }
        public IOptionRepository OptionRepository { get; set; }
        public IQuestionRepository QuestionRepository { get; set; }
        public ITestRepository TestRepository { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            this.db = db;
            AnswerRepository = new AnswerRepository(db);
            AttemptRepository = new AttemptRepository(db);
            OptionRepository = new OptionRepository(db);
            QuestionRepository = new QuestionRepository(db);
            TestRepository = new TestRepository(db);
        }

        public async Task Save()
            => await db.SaveChangesAsync();

        public IEnumerable<Attempt> GetUserAttempts(string userId)
        {
            IEnumerable<Attempt> userAttempts =
                AttemptRepository.GetWhere(x => !string.IsNullOrEmpty(x.UserId) && x.UserId.Equals(userId));
            foreach (Attempt attempt in userAttempts)
            {
                attempt.Answers = GetAttemptAnswers(attempt.Id);
            }
            return userAttempts;
        }

        public IEnumerable<Test> GetUserTests(string userId)
        {
            IEnumerable<Test> userTests =
                TestRepository.GetWhere(x => !string.IsNullOrEmpty(x.UserId) && x.UserId.Equals(userId)).ToList();
            foreach (Test test in userTests)
            {
                test.Questions = GetTestQuestions(test.Id);
            }
            return userTests;
        }

        public IEnumerable<Question> GetTestQuestions(int testId)
        {
            IEnumerable<Question> questions = QuestionRepository.GetWhere(x => x.Id == testId).ToList();
            foreach (Question question in questions)
            {
                question.Options = GetQuestionOptions(question.Id);
            }
            return questions;
        }

        public IEnumerable<Option> GetQuestionOptions(int questionId)
            => OptionRepository.GetWhere(x => x.Id == questionId);

        public IEnumerable<Answer> GetAttemptAnswers(int attemptId)
            => AnswerRepository.GetWhere(x => x.Id == attemptId);

        public async Task AddTest(Test value)
        {
            await TestRepository.AddAsync(value);
            await TestRepository.SaveAsync();
            foreach (Question question in value.Questions)
            {
                question.TestId = value.Id;
            }
            await QuestionRepository.AddRangeAsync(value.Questions);
            await QuestionRepository.SaveAsync();
            foreach (Question question in value.Questions.Where(x => x.Options != null))
            {
                foreach (Option option in question.Options)
                {
                    option.QuestionId = question.Id;
                }
                await OptionRepository.AddRangeAsync(question.Options);
            }
        }
    }
}
