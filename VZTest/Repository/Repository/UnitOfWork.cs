using VZTest.Data;
using VZTest.Models;
using VZTest.Models.Test;
using VZTest.Models.Test.CorrectAnswers;
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

        #region TestStatistics
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
        #endregion

        #region Attemps
        public IEnumerable<Attempt> GetUserTestAttempts(int testId, string userId)
        {
            return AttemptRepository.GetWhere(x => x.TestId == testId && x.UserId.Equals(userId));
        }

        public IEnumerable<Attempt> GetUserAttempts(string userId, bool loadAnswers)
        {
            IEnumerable<Attempt> userAttempts =
                AttemptRepository.GetWhere(x => !string.IsNullOrEmpty(x.UserId) && x.UserId.Equals(userId));
            if (loadAnswers)
            {
                foreach (Attempt attempt in userAttempts)
                {
                    attempt.Answers = GetAttemptAnswers(attempt.Id).ToList();
                }
            }
            return userAttempts;
        }

        public Attempt? GetAttemptWithAnswers(int attemptId)
        {
            Attempt? attempt = AttemptRepository.FirstOrDefault(x => x.Id == attemptId);
            if (attempt == null)
            {
                return null;
            }
            attempt.Answers = GetAttemptAnswers(attemptId).ToList();
            return attempt;
        }

        public IEnumerable<Attempt> GetTestAttempts(int testId, bool loadAnswers)
        {
            IEnumerable<Attempt> testAttempts = AttemptRepository.GetWhere(x => x.TestId == testId);
            if (loadAnswers)
            {
                foreach (Attempt attempt in testAttempts)
                {
                    attempt.Answers = GetAttemptAnswers(attempt.Id).ToList();
                }
            }
            return testAttempts;
        }

        public async Task<int> GetTestAttemptsCount(int testId)
        {
            return await AttemptRepository.CountAsync(x => x.TestId == testId);
        }

        public IEnumerable<Attempt> GetUserTestCheckedAttempts(int testId, string userId)
        {
            IEnumerable<Attempt> attempts = AttemptRepository.GetWhere(x => x.TestId == testId && x.UserId.Equals(userId)).ToList();
            foreach(Attempt attempt in attempts)
            {
                attempt.Answers = GetAttemptAnswers(attempt.Id).ToList();
                CheckAttempt(attempt);
            }
            return attempts;
        }

        private void CheckAttempt(Attempt attempt)
        {
            attempt.MaxBalls = GetTestTotalBalls(attempt.TestId);
            foreach (Answer answer in attempt.Answers)
            {
                double balls = CheckAnswerCorrect(answer);
                attempt.Balls += balls;
                answer.Correct = CheckAnswerCorrect(answer) > 0;
                if (answer.Correct)
                {
                    attempt.CorrectAnswers++;
                }
            }
        }

        public Attempt? GetCheckedAttempt(int attemptId)
        {
            Attempt? attempt = GetAttemptWithAnswers(attemptId);
            if (attempt == null)
            {
                return null;
            }
            attempt.MaxBalls = GetTestTotalBalls(attempt.TestId);
            foreach (Answer answer in attempt.Answers)
            {
                double balls = CheckAnswerCorrect(answer);
                attempt.Balls += balls;
                answer.Correct = CheckAnswerCorrect(answer) > 0;
                if (answer.Correct)
                {
                    attempt.CorrectAnswers++;
                }
            }
            return attempt;
        }
        #endregion

        #region Questions
        public IEnumerable<Question> GetTestQuestions(int testId, bool loadAnswers)
        {
            IEnumerable<Question> questions = QuestionRepository.GetWhere(x => x.TestId == testId).ToList();
            if (loadAnswers)
            {
                foreach (Question question in questions)
                {
                    question.Options = GetQuestionOptions(question.Id).ToList();
                    question.CorrectAnswer = GetQuestionCorrectAnswer(question.Id);
                }
            }
            else
            {
                foreach (Question question in questions)
                {
                    question.Options = GetQuestionOptions(question.Id).ToList();
                }
            }
            foreach (Question question in questions)
            {
                question.Options = GetQuestionOptions(question.Id).ToList();
            }
            return questions;
        }

        public async Task<int> GetTestQuestionCount(int testId)
        {
            return await QuestionRepository.CountAsync(x => x.TestId == testId);
        }

        public double GetTestTotalBalls(int testId)
        {
            return QuestionRepository.GetWhere(x => x.TestId == testId).Sum(x => x.Balls);
        }
        #endregion

        #region UserStars
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
        {
            return UserStarRepository.GetWhere(x => x.TestId == testId);
        }

        public async Task<int> GetTestStarsCount(int testId)
        {
            return await UserStarRepository.CountAsync(x => x.TestId == testId);
        }
        #endregion

        #region Tests
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

        public Test? GetTestById(int testId, bool loadAnswers)
        {
            Test? test = TestRepository.FirstOrDefault(x => x.Id == testId);
            if (test == null)
            {
                return null;
            }
            test.Questions = GetTestQuestions(test.Id, loadAnswers).OrderBy(x => x.Number).ToList();
            return test;
        }
        #endregion

        #region Options
        public IEnumerable<Option> GetQuestionOptions(int questionId)
        {
            return OptionRepository.GetWhere(x => x.QuestionId == questionId);
        }
        #endregion

        #region Answers
        public IEnumerable<Answer> GetAttemptAnswers(int attemptId)
        {
            return AnswerRepository.GetWhere(x => x.AttemptId == attemptId);
        }

        private double CheckAnswerCorrect(Answer answer)
        {
            Question? question = QuestionRepository.FirstOrDefault(x => x.Id == answer.QuestionId);
            CorrectAnswer? correctAnswer = CorrectAnswerRepository.FirstOrDefault(x => x.QuestionId == answer.QuestionId);
            if (question == null || correctAnswer == null)
            {
                return 0;
            }
            switch (question.Type)
            {
                case QuestionType.Text:
                    CorrectTextAnswer? correctTextAnswer = correctAnswer as CorrectTextAnswer;
                    if (correctTextAnswer == null)
                    {
                        return 0;
                    }
                    if (answer.TextAnswer == null)
                    {
                        return 0;
                    }
                    if (answer.TextAnswer.Equals(correctTextAnswer.Correct))
                    {
                        return question.Balls;
                    }
                    else
                    {
                        return 0;
                    }
                case QuestionType.Date:
                    CorrectDateAnswer? correctDateAnswer = correctAnswer as CorrectDateAnswer;
                    if (correctDateAnswer == null)
                    {
                        return 0;
                    }
                    if (answer.DateAnswer == null)
                    {
                        return 0;
                    }
                    if ((answer.DateAnswer.Value.Year == correctDateAnswer.Correct.Year) &&
                    (answer.DateAnswer.Value.Month == correctDateAnswer.Correct.Month) &&
                    (answer.DateAnswer.Value.Day == correctDateAnswer.Correct.Day))
                    {
                        return question.Balls;
                    }
                    else
                    {
                        return 0;
                    }
                case QuestionType.Int:
                    CorrectIntAnswer? correctIntAnswer = correctAnswer as CorrectIntAnswer;
                    if (correctIntAnswer == null)
                    {
                        return 0;
                    }
                    if (answer.IntAnswer == null)
                    {
                        return 0;
                    }
                    if (answer.IntAnswer == correctIntAnswer.Correct)
                    {
                        return question.Balls;
                    }
                    else
                    {
                        return 0;
                    }
                case QuestionType.Double:
                    CorrectDoubleAnswer? correctDoubleAnswer = correctAnswer as CorrectDoubleAnswer;
                    if (correctDoubleAnswer == null)
                    {
                        return 0;
                    }
                    if (answer.DoubleAnswer == null)
                    {
                        return 0;
                    }
                    if (answer.DoubleAnswer == correctDoubleAnswer.Correct)
                    {
                        return question.Balls;
                    }
                    else
                    {
                        return 0;
                    }
                case QuestionType.Radio:
                    CorrectIntAnswer? correctRadioAnswer = correctAnswer as CorrectIntAnswer;
                    if (correctRadioAnswer == null)
                    {
                        return 0;
                    }
                    if (answer.RadioAnswer == null)
                    {
                        return 0;
                    }
                    if (answer.RadioAnswer == correctRadioAnswer.Correct)
                    {
                        return question.Balls;
                    }
                    else
                    {
                        return 0;
                    }
                case QuestionType.Check:
                    return 0;
                default:
                    return 0;
            }
        }
        #endregion

        #region CorrectAnswer
        public CorrectAnswer? GetQuestionCorrectAnswer(int questionId)
        {
            return CorrectAnswerRepository.FirstOrDefault(x => x.QuestionId == questionId);
        }
        #endregion

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
