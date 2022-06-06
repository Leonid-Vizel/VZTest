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
        public ICorrectAnswerRepository CorrectAnswerRepository { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            this.db = db;
            AnswerRepository = new AnswerRepository(db);
            AttemptRepository = new AttemptRepository(db);
            OptionRepository = new OptionRepository(db);
            QuestionRepository = new QuestionRepository(db);
            TestRepository = new TestRepository(db);
            CorrectAnswerRepository = new CorrectAnswerRepository(db);
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

        public async Task<IEnumerable<TestStatistics>> GetUserTestsStatistics(string userId)
        {
            IEnumerable<Test> userTests = TestRepository.GetWhere(x => !string.IsNullOrEmpty(x.UserId) && x.UserId.Equals(userId)).ToList();
            List<TestStatistics> userTestStatistics = new List<TestStatistics>();
            foreach(Test test in userTests)
            {
                userTestStatistics.Add(await GetTestStatistics(test));
            }
            return userTestStatistics;
        }

        public IEnumerable<Test> GetPublicTests()
            => TestRepository.GetWhere(x => x.Public).ToList();

        public Test? GetTestById(int testId, bool loadAnswers)
        {
            Test? test = TestRepository.FirstOrDefault(x => x.Id == testId);
            if (test == null)
            {
                return null;
            }
            test.Questions = GetTestQuestions(test.Id, loadAnswers);
            return test;
        }

        public IEnumerable<Question> GetTestQuestions(int testId, bool loadAnswers)
        {
            IEnumerable<Question> questions = QuestionRepository.GetWhere(x => x.Id == testId).ToList();
            if (loadAnswers)
            {
                foreach (Question question in questions)
                {
                    question.Options = GetQuestionOptions(question.Id);
                    question.CorrectAnswer = GetQuestionCorrectAnswer(question.Id);
                }
            }
            else
            {
                foreach (Question question in questions)
                {
                    question.Options = GetQuestionOptions(question.Id);
                }
            }
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

        public CorrectAnswer? GetQuestionCorrectAnswer(int questionId)
            => CorrectAnswerRepository.FirstOrDefault(x => x.Id == questionId);

        public async Task<int> GetTestAttemptsCount(int testId)
            => await AttemptRepository.CountAsync(x => x.Id == testId);

        public async Task<int> GetTestQuestionCount(int testId)
            => await QuestionRepository.CountAsync(x => x.Id == testId);

        public async Task<TestStatistics?> GetTestStatistics(int testId)
        {
            Test? test = TestRepository.FirstOrDefault(x => x.Id == testId);
            if (test == null)
            {
                return null;
            }
            return new TestStatistics()
            {
                Test = test,
                QuestionCount = await GetTestQuestionCount(testId),
                AttemptsCount = await GetTestAttemptsCount(testId)
            };
        }

        private async Task<TestStatistics> GetTestStatistics(Test test)
        {
            return new TestStatistics()
            {
                Test = test,
                QuestionCount = await GetTestQuestionCount(test.Id),
                AttemptsCount = await GetTestAttemptsCount(test.Id)
            };
        }
    }
}
