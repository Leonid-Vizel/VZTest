using VZTest.Models.DataModels.Test;
using VZTest.Models.ViewModels.Test;

namespace VZTest.Data.IRepository
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

        #region Test
        Test? GetTestById(int id);
        TestModel? GetTestModelById(int id);
        TestModel? GetTestModelFromTest(Test test);
        double GetTestTotalBalls(int id);
        Task<TestPreviewModel> GetTestPreviewModel(int id, string userId, string? hash);
        TestResultsModel GetTestResultsModel(int id, string userId);
        TestEditModel GetTestEditModelById(int id, string userId);
        Task<TestStatistics?> GetTestStatistics(int id, string userId);
        Task<List<TestStatistics>> GetPublicTestStatistics(string userId);
        Task<List<TestStatistics>> GetUserTestStatistics(string userId);
        Task<Test?> CreateTest(TestCreateModel model, string userId);
        Task<TestModel?> EditTest(TestEditModel model);
        Task<int> RemoveTest(int id, string userId);
        #endregion

        #region Question
        Question? GetQuestionById(int id);
        QuestionModel? GetQuestionModelById(int id);
        List<QuestionModel> GetTestQuestionModels(int testId);
        Task<int> GetTestQuestionCount(int testId);
        List<int> GetTestQuestionIds(int testId);
        QuestionModel? GetQuestionModelFromBlueprint(QuestionBlueprint blueprint, bool transferId = false);
        #endregion

        #region Option
        List<Option> GetQuestionOptions(int questionId);
        Task<bool> OptionExists(int questionId, int optionId);
        #endregion

        #region Attempt
        Attempt? GetAttemptById(int id);
        AttemptModel GetAttemptModel(Attempt attempt);
        AttemptModel? GetAttemptModelById(int id);
        Task<int> GetTestTotalAttemptsCount(int testId);
        List<AttemptModel> GetTestUserAttemptModels(int testId, string userId);
        List<Attempt> GetUserTotalAttempts(string userId);
        List<Attempt> GetTestTotalAttempts(int testId);
        List<AttemptModel> GetTestTotalAttemptModels(int testId);
        List<AttemptModel> GetTestActiveAttemptModels(int testId);
        AttemptViewModel GetAttemptViewModelById(int id, string userId);
        Task<int> GetTestUserAttemptsCount(int testId, string userId);
        Task<Attempt?> CreateAttempt(Test test, string userId);
        Task CheckAttempt(AttemptModel model, bool save = true);
        Task<int> RemoveAttempt(int id, string userId);
        Task<bool> CheckAnyActiveTestAttempts(int testId, string userId);
        #endregion

        #region Answer
        Answer? GetAnswerById(int id);
        Answer? GetAnswerByAttemptAndQuestion(int attemptId, int questionId);
        List<Answer> GetAttemptAnswers(int attemptId);
        double CheckAnswer(Answer answer);
        Task<int> UpdateAttemptAnswer(int attemptId, int questionId, string userId, string value);
        #endregion

        #region Stars
        bool CheckTestStarred(int testId, string userId);
        Task<int> GetTestStarCount(int testId);
        UserStar? GetUserStar(int testId, string userId);
        Task ToggleStarAsync(int testId, string userId);
        #endregion

        #region CorrectAnswer
        CorrectAnswer? GetQuestionCorrectAnswer(int questionId);
        #endregion

        Task SaveAsync();
    }
}
