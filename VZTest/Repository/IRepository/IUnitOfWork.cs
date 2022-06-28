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
        IEnumerable<Attempt> GetUserTestAttempts(int testId, string userId);
        Attempt? GetAttemptWithAnswers(int attemptId);
        Task<int> GetTestAttemptsCount(int testId);
        void CheckAttempt(Attempt attempt);
        void RemoveAttempt(int attemptId);
        void RemoveAttempt(Attempt attempt);
        Attempt? GetAttemptMainInfo(int attemptId);
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
        #endregion

        #region UserStars
        IEnumerable<UserStar> GetTestStars(int testId);
        bool RemoveUserStar(int testId, string userId);
        bool CheckUserLiked(int testId, string userId);
        Task<int> GetTestStarsCount(int testId);
        #endregion

        #region Tests
        Test? GetTestById(int testId, bool loadAnswers);
        void FillTest(Test test, bool loadAnswers);
        Test? GetTestMainInfo(int testId);
        double GetTestTotalBalls(int testId);
        void RemoveTest(int testId);
        Task AddTest(Test value);
        #endregion

        #region Options
        List<Option> GetQuestionOptions(int questionId);
        bool OptionExists(int questionId, int optionId);
        #endregion

        #region Answers
        void GetAttemptAnswers(Attempt attempt);
        Answer? GetAnswer(int attemptId, int questionId);
        #endregion

        #region CorrectAnswer
        CorrectAnswer? GetQuestionCorrectAnswer(int questionId);
        #endregion

        Task SaveAsync();
    }
}
