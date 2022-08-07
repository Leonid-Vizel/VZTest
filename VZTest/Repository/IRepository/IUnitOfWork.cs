using VZTest.Models;
using VZTest.Models.DataModels.Test;
using VZTest.Models.ViewModels.Test;

namespace VZTest.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IRepository<Answer> AnswerRepository { get; set; }
        IRepository<Attempt> AttemptRepository { get; set; }
        IRepository<Option> OptionRepository { get; set; }
        IRepository<Question> QuestionRepository { get; set; }
        IRepository<Test> TestRepository { get; set; }
        IRepository<CorrectAnswer> CorrectAnswerRepository { get; set; }
        IRepository<UserStar> UserStarRepository { get; set; }

        #region Attempts
        IEnumerable<Attempt> GetUserAttempts(string userId, bool loadAnswers);
        IEnumerable<Attempt> GetTestAttempts(int testId, bool loadAnswers);
        IEnumerable<Attempt> GetUserTestAttempts(int testId, string userId);
        Attempt? GetAttemptWithAnswers(int attemptId);
        Attempt? GetAttemptMainInfo(int attemptId);
        Task<int> GetTestAttemptsCount(int testId);
        Task<Attempt?> CreateAttempt(Test test, string userId);
        Task AddAttemptAsync(Attempt attempt);
        void CheckAttempt(Attempt attempt);
        void RemoveAttempt(int attemptId);
        void RemoveAttempt(Attempt attempt);
        void UpdateAttempt(Attempt attempt);
        #endregion

        #region TestStatistics
        Task<IEnumerable<TestStatistics>> GetUserTestsStatistics(string userId);
        Task<IEnumerable<TestStatistics>> GetPublicTestsStatistics(string userId);
        Task<TestStatistics?> GetTestStatistics(int testId, string userId);
        #endregion

        #region Questions
        IEnumerable<Question> GetTestQuestions(int testId, bool loadAnswers);
        Task<int> GetTestQuestionCount(int testId);
        Question? GetQuestion(int questionId, bool loadOptions);
        Task AddQuestionAsync(Question question);
        void RemoveQuestion(Question question);
        void UpdateQuestion(Question question);
        #endregion

        #region UserStars
        IEnumerable<UserStar> GetTestStars(int testId);
        bool RemoveUserStar(int testId, string userId);
        bool CheckUserLiked(int testId, string userId);
        Task<int> GetTestStarsCount(int testId);
        Task AddUserStarAsync(UserStar star);
        #endregion

        #region Tests
        Test? GetTestById(int testId, bool loadAnswers);
        Test? GetTestMainInfo(int testId);
        Task AddTest(Test value);
        double GetTestTotalBalls(int testId);
        void FillTest(Test test, bool loadAnswers);
        void RemoveTest(int testId);
        void UpdateTest(Test test);
        #endregion

        #region Options
        List<Option> GetQuestionOptions(int questionId);
        bool OptionExists(int questionId, int optionId);
        Task AddOptionAsync(Option option);
        void RemoveOption(Option option);
        void UpdateOption(Option option);
        #endregion

        #region Answers
        Answer? GetAnswer(int attemptId, int questionId);
        Task AddAnswerRangeAsync(IEnumerable<Answer> answers);
        void GetAttemptAnswers(Attempt attempt);
        void UpdateAnswer(Answer answer);
        #endregion

        #region CorrectAnswer
        CorrectAnswer? GetQuestionCorrectAnswer(int questionId);
        Task AddCorrectAnswerAsync(CorrectAnswer answer);
        void RemoveCorrectAnswer(CorrectAnswer answer);
        #endregion

        Task SaveAsync();
    }
}
