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
            List<Test> foundTests = TestRepository.GetWhere(x => x.Public && x.Opened).ToList();
            List<TestStatistics> testStatistics = new List<TestStatistics>();
            foreach (Test test in foundTests)
            {
                testStatistics.Add(await GetTestStatistics(test, userId));
            }
            return testStatistics;
        }

        public async Task<TestStatistics?> GetTestStatistics(int testId, string userId)
        {
            Test? test = GetTestMainInfo(testId);
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
            List<Attempt> attempts = AttemptRepository.GetWhere(x => x.TestId == testId && x.UserId.Equals(userId)).ToList();
            foreach (Attempt attempt in attempts)
            {
                GetAttemptAnswers(attempt);
            }
            return attempts;
        }

        public IEnumerable<Attempt> GetUserAttempts(string userId, bool loadAnswers)
        {
            IEnumerable<Attempt> userAttempts =
                AttemptRepository.GetWhere(x => !string.IsNullOrEmpty(x.UserId) && x.UserId.Equals(userId));
            if (loadAnswers)
            {
                foreach (Attempt attempt in userAttempts)
                {
                    GetAttemptAnswers(attempt);
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
            GetAttemptAnswers(attempt);
            return attempt;
        }

        public IEnumerable<Attempt> GetTestAttempts(int testId, bool loadAnswers)
        {
            List<Attempt> testAttempts = AttemptRepository.GetWhere(x => x.TestId == testId).ToList();
            if (loadAnswers)
            {
                foreach (Attempt attempt in testAttempts)
                {
                    GetAttemptAnswers(attempt);
                }
            }
            return testAttempts;
        }

        public async Task<int> GetTestAttemptsCount(int testId)
        {
            return await AttemptRepository.CountAsync(x => x.TestId == testId);
        }

        public void CheckAttempt(Attempt attempt)
        {
            attempt.MaxBalls = GetTestTotalBalls(attempt.TestId);
            foreach (Answer answer in attempt.Answers)
            {
                answer.Balls = CheckAnswerCorrect(answer);
                if (answer.Balls > 0)
                {
                    attempt.CorrectAnswers++;
                }
            }
        }

        public void RemoveAttempt(int attemptId)
        {
            Attempt? attempt = GetAttemptWithAnswers(attemptId);
            if (attempt == null)
            {
                return;
            }
            foreach (Answer answer in attempt.Answers)
            {
                AnswerRepository.Remove(answer);
            }
            AttemptRepository.Remove(attempt);
        }

        public void RemoveAttempt(Attempt attempt)
        {
            if (attempt.Answers == null)
            {
                GetAttemptAnswers(attempt);
            }
            foreach (Answer answer in attempt.Answers)
            {
                AnswerRepository.Remove(answer);
            }
            AttemptRepository.Remove(attempt);
        }

        public Attempt? GetAttemptMainInfo(int attemptId)
        {
            return AttemptRepository.FirstOrDefault(x => x.Id == attemptId);
        }

        public async Task AddAttemptAsync(Attempt attempt)
        {
            await AttemptRepository.AddAsync(attempt);
        }

        public void UpdateAttempt(Attempt attempt)
        {
            AttemptRepository.Update(attempt);
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

        public async Task<int> GetTestQuestionCount(int testId)
        {
            return await QuestionRepository.CountAsync(x => x.TestId == testId);
        }

        public double GetTestTotalBalls(int testId)
        {
            return QuestionRepository.GetWhere(x => x.TestId == testId).Sum(x => x.Balls);
        }

        public Question? GetQuestion(int questionId, bool loadOptions)
        {
            if (loadOptions)
            {
                Question? question = QuestionRepository.FirstOrDefault(x => x.Id == questionId);
                if (question == null)
                {
                    return null;
                }
                question.Options = GetQuestionOptions(questionId);
                return question;
            }
            return QuestionRepository.FirstOrDefault(x=>x.Id == questionId);
        }

        public async Task AddQuestionAsync(Question question)
        {
            await QuestionRepository.AddAsync(question);
        }

        public void RemoveQuestion(Question question)
        {
            QuestionRepository.Remove(question);
            if (question.CorrectAnswer != null)
            {
                CorrectAnswerRepository.Remove(question.CorrectAnswer);
            }
            foreach (Option option in question.Options)
            {
                OptionRepository.Remove(option);
            }
            foreach(Answer answer in AnswerRepository.GetWhere(x=>x.QuestionId == question.Id))
            {
                AnswerRepository.Remove(answer);
            }
        }

        public void UpdateQuestion(Question question)
        {
            QuestionRepository.Update(question);
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

        public async Task AddUserStarAsync(UserStar star)
        {
            await UserStarRepository.AddAsync(star);
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
                if (question.CorrectAnswer != null && question.Type != QuestionType.Radio && question.Type != QuestionType.Check)
                {
                    question.CorrectAnswer.QuestionId = question.Id;
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
                if (question.CorrectAnswer != null)
                {
                    CorrectAnswerRepository.Remove(question.CorrectAnswer);
                }
                foreach (Option option in question.Options)
                {
                    OptionRepository.Remove(option);
                }
            }
            IEnumerable<Attempt> attempts = GetTestAttempts(test.Id, true);
            foreach (Attempt attempt in attempts)
            {
                RemoveAttempt(attempt);
            }

            IEnumerable<UserStar> stars = GetTestStars(test.Id);
            foreach (UserStar userStar in stars)
            {
                UserStarRepository.Remove(userStar);
            }
        }

        public Test? GetTestById(int testId, bool loadAnswers)
        {
            Test? test = GetTestMainInfo(testId);
            if (test == null)
            {
                return null;
            }
            test.Questions = GetTestQuestions(test.Id, loadAnswers).ToList();
            return test;
        }

        public void FillTest(Test test, bool loadAnswers)
        {
            if (test == null)
            {
                return;
            }
            test.Questions = GetTestQuestions(test.Id, loadAnswers).ToList();
        }

        public Test? GetTestMainInfo(int testId)
        {
            return TestRepository.FirstOrDefault(x => x.Id == testId);
        }

        public void UpdateTest(Test test)
        {
            TestRepository.Update(test);
        }
        #endregion

        #region Options
        public List<Option> GetQuestionOptions(int questionId)
        {
            return OptionRepository.GetWhere(x => x.QuestionId == questionId).ToList();
        }

        public bool OptionExists(int questionId, int optionId)
        {
            return OptionRepository.FirstOrDefault(x => x.Id == optionId && x.QuestionId == questionId) != null;
        }

        public async Task AddOptionAsync (Option option)
        {
            await OptionRepository.AddAsync(option);
        }
        #endregion

        #region Answers
        public void GetAttemptAnswers(Attempt attempt)
        {
            IEnumerable<int> questionIds = attempt.QuestionSequence.Select(x => int.Parse(x));
            attempt.Answers = new List<Answer>();
            foreach (int questionId in questionIds)
            {
                Answer? answer = AnswerRepository.FirstOrDefault(x => x.QuestionId == questionId && x.AttemptId == attempt.Id);
                if (answer != null)
                {
                    attempt.Answers.Add(answer);
                }
            }
        }

        public Answer? GetAnswer(int attemptId, int questionId)
        {
            return AnswerRepository.FirstOrDefault(x => x.AttemptId == attemptId && x.QuestionId == questionId);
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
                    CorrectCheckAnswer? correctCheckAnswer = correctAnswer as CorrectCheckAnswer;
                    if (correctCheckAnswer == null)
                    {
                        return 0;
                    }
                    if (correctCheckAnswer.Correct.Length != 0 && answer.CheckAnswers.Length == 0)
                    {
                        return 0;
                    }
                    int incorrectCount = 0;
                    foreach (int answerId in answer.CheckAnswers)
                    {
                        if (!correctCheckAnswer.Correct.Contains(answerId))
                        {
                            incorrectCount++;
                        }
                    }
                    foreach (int answerId in correctCheckAnswer.Correct)
                    {
                        if (!answer.CheckAnswers.Contains(answerId))
                        {
                            incorrectCount++;
                        }
                    }
                    if (incorrectCount == 0)
                    {
                        return question.Balls;
                    }
                    else if (incorrectCount == 1)
                    {
                        return question.Balls / 2;
                    }
                    else
                    {
                        return 0;
                    }
                default:
                    return 0;
            }
        }

        public async Task AddAnswerRangeAsync(IEnumerable<Answer> answers)
        {
            await AnswerRepository.AddRangeAsync(answers);
        }

        public void UpdateAnswer(Answer answer)
        {
            AnswerRepository.Update(answer);
        }
        #endregion

        #region CorrectAnswer
        public CorrectAnswer? GetQuestionCorrectAnswer(int questionId)
        {
            return CorrectAnswerRepository.FirstOrDefault(x => x.QuestionId == questionId);
        }

        public async Task AddCorrectAnswerAsync(CorrectAnswer answer)
        {
            await CorrectAnswerRepository.AddAsync(answer);
        }

        public void RemoveCorrectAnswer(CorrectAnswer answer)
        {
            CorrectAnswerRepository.Remove(answer);
        }
        #endregion

        #region Users

        #endregion

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
