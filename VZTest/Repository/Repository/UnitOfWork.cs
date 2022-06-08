using VZTest.Data;
using VZTest.Models;
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
        public IUserStarRepository UserStarRepository { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            this.db = db;
            AnswerRepository = new AnswerRepository(db);
            AttemptRepository = new AttemptRepository(db);
            OptionRepository = new OptionRepository(db);
            QuestionRepository = new QuestionRepository(db);
            TestRepository = new TestRepository(db);
            CorrectAnswerRepository = new CorrectAnswerRepository(db);
            UserStarRepository = new UserStarRepository(db);
        }

        public async Task SaveAsync()
            => await db.SaveChangesAsync();

        public bool RemoveUserStar(int testId, string userId)
        {
            UserStar? star = UserStarRepository.FirstOrDefault(x => x.TestId == testId && x.UserId.Equals(userId));
            if (star == null)
            {
                return false;
            }
            UserStarRepository.Remove(star);
            return true;
        }

        public bool CheckUserLiked(int testId, string userId)
        {
            UserStar? star = UserStarRepository.FirstOrDefault(x => x.TestId == testId && x.UserId.Equals(userId));
            if (star == null)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<UserStar> GetTestStars(int testId)
            => UserStarRepository.GetWhere(x => x.TestId == testId);

        public async Task<int> GetTestStarsCount(int testId)
            => await UserStarRepository.CountAsync(x => x.TestId == testId);

        public IEnumerable<Attempt> GetUserAttempts(string userId, bool loadAnswers)
        {
            IEnumerable<Attempt> userAttempts =
                AttemptRepository.GetWhere(x => !string.IsNullOrEmpty(x.UserId) && x.UserId.Equals(userId));
            if (loadAnswers)
            {
                foreach (Attempt attempt in userAttempts)
                {
                    attempt.Answers = GetAttemptAnswers(attempt.Id);
                }
            }
            return userAttempts;
        }

        public async Task<IEnumerable<TestStatistics>> GetUserTestsStatistics(string userId)
        {
            IEnumerable<Test> userTests = TestRepository.GetWhere(x => x.UserId.Equals(userId)).ToList();
            List<TestStatistics> userTestStatistics = new List<TestStatistics>();
            foreach (Test test in userTests)
            {
                userTestStatistics.Add(await GetTestStatistics(test, userId));
            }
            return userTestStatistics;
        }

        public async Task<IEnumerable<TestStatistics>> GetPublicTestsStatistics(string userId)
        {
            IEnumerable<Test> foundTests = TestRepository.GetWhere(x => x.Public && x.Opened).ToList();
            List<TestStatistics> testStatistics = new List<TestStatistics>();
            foreach (Test test in foundTests)
            {
                testStatistics.Add(await GetTestStatistics(test, userId));
            }
            return testStatistics;
        }

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
            IEnumerable<Question> questions = QuestionRepository.GetWhere(x => x.TestId == testId).ToList();
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

        public IEnumerable<Attempt> GetTestAttempts(int testId, bool loadAnswers)
        {
            IEnumerable<Attempt> testAttempts = AttemptRepository.GetWhere(x => x.TestId == testId);
            if (loadAnswers)
            {
                foreach (Attempt attempt in testAttempts)
                {
                    attempt.Answers = GetAttemptAnswers(attempt.Id);
                }
            }
            return testAttempts;
        }

        public IEnumerable<Option> GetQuestionOptions(int questionId)
            => OptionRepository.GetWhere(x => x.QuestionId == questionId);

        public IEnumerable<Answer> GetAttemptAnswers(int attemptId)
            => AnswerRepository.GetWhere(x => x.AttemptId == attemptId);

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
                if (question.CorrectAnswer != null)
                {
                    await CorrectAnswerRepository.AddAsync(question.CorrectAnswer);
                }
                await OptionRepository.AddRangeAsync(question.Options);
            }
        }

        public void RemoveTest(int testId)
        {
            Test? test = GetTestById(testId, true);
            if (test == null)
            {
                return;
            }
            TestRepository.Remove(test);
            foreach (Question question in test.Questions)
            {
                QuestionRepository.Remove(question);
                CorrectAnswerRepository.Remove(question.CorrectAnswer);
                foreach (Option option in question.Options)
                {
                    OptionRepository.Remove(option);
                }
            }
            IEnumerable<Attempt> attempts = GetTestAttempts(test.Id, true);
            foreach (Attempt attempt in attempts)
            {
                AttemptRepository.Remove(attempt);
                foreach (Answer answer in attempt.Answers)
                {
                    AnswerRepository.Remove(answer);
                }
            }

            IEnumerable<UserStar> stars = GetTestStars(test.Id);
            foreach (UserStar userStar in stars)
            {
                UserStarRepository.Remove(userStar);
            }
        }

        public CorrectAnswer? GetQuestionCorrectAnswer(int questionId)
            => CorrectAnswerRepository.FirstOrDefault(x => x.QuestionId == questionId);

        public async Task<int> GetTestAttemptsCount(int testId)
            => await AttemptRepository.CountAsync(x => x.TestId == testId);

        public async Task<int> GetTestQuestionCount(int testId)
            => await QuestionRepository.CountAsync(x => x.TestId == testId);

        public async Task<TestStatistics?> GetTestStatistics(int testId, string userId)
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
                AttemptsCount = await GetTestAttemptsCount(testId),
                StarsCount = await GetTestStarsCount(testId),
                CurrectUserStarred = CheckUserLiked(testId, userId)
            };
        }

        private async Task<TestStatistics> GetTestStatistics(Test test, string userId)
        {
            return new TestStatistics()
            {
                Test = test,
                QuestionCount = await GetTestQuestionCount(test.Id),
                AttemptsCount = await GetTestAttemptsCount(test.Id),
                StarsCount = await GetTestStarsCount(test.Id),
                CurrectUserStarred = CheckUserLiked(test.Id, userId)
            };
        }

        public IEnumerable<Attempt> GetUserTestAttempt(int testId, string userId)
            => AttemptRepository.GetWhere(x=>x.TestId == testId && x.UserId.Equals(userId));
    }
}
