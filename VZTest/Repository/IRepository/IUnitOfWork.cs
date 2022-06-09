using VZTest.Models;
using VZTest.Models.Test;

namespace VZTest.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IAnswerRepository AnswerRepository { get; set; }
        IAttemptRepository AttemptRepository { get; set; }
        IOptionRepository OptionRepository { get; set; }
        IQuestionRepository QuestionRepository { get; set; }
        ITestRepository TestRepository { get; set; }
        ICorrectAnswerRepository CorrectAnswerRepository { get; set; }
        IUserStarRepository UserStarRepository { get; set; }

        #region Attempts
        IEnumerable<Attempt> GetUserAttempts(string userId, bool loadAnswers);
        IEnumerable<Attempt> GetTestAttempts(int testId, bool loadAnswers);
        IEnumerable<Attempt> GetUserTestAttempt(int testId, string userId);
        Attempt? GetAttemptWithAnswers(int attemptId);
        Task<int> GetTestAttemptsCount(int testId);
        Attempt? GetCheckedAttempt(int attemptId);
        #endregion

        #region TestStatistics
        Task<IEnumerable<TestStatistics>> GetUserTestsStatistics(string userId);
        Task<IEnumerable<TestStatistics>> GetPublicTestsStatistics(string userId);
        Task<TestStatistics?> GetTestStatistics(int testId, string userId);
        #endregion

        #region Questions
        IEnumerable<Question> GetTestQuestions(int testId, bool loadAnswers);
        Task<int> GetTestQuestionCount(int testId);
        #endregion

        #region UserStars
        IEnumerable<UserStar> GetTestStars(int testId);
        bool RemoveUserStar(int testId, string userId);
        bool CheckUserLiked(int testId, string userId);
        Task<int> GetTestStarsCount(int testId);
        #endregion

        #region Tests
        Test? GetTestById(int testId, bool loadAnswers);
        void RemoveTest(int testId);
        Task AddTest(Test value);
        #endregion

        #region Options
        IEnumerable<Option> GetQuestionOptions(int questionId);
        #endregion

        #region Answers
        IEnumerable<Answer> GetAttemptAnswers(int attemptId);
        #endregion

        #region CorrectAnswer
        CorrectAnswer? GetQuestionCorrectAnswer(int questionId);
        #endregion

        Task SaveAsync();
    }
}
