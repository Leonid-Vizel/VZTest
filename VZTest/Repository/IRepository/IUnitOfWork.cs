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

        IEnumerable<Attempt> GetUserAttempts(string userId, bool loadAnswers);
        IEnumerable<Attempt> GetTestAttempts(int testId, bool loadAnswers);
        Task<IEnumerable<TestStatistics>> GetUserTestsStatistics(string userId);
        IEnumerable<Test> GetPublicTests();
        IEnumerable<Question> GetTestQuestions(int testId, bool loadAnswers);
        IEnumerable<Option> GetQuestionOptions(int questionId);
        IEnumerable<Answer> GetAttemptAnswers(int attemptId);
        Task<int> GetTestAttemptsCount(int testId);
        Task<int> GetTestQuestionCount(int testId);
        Task<TestStatistics?> GetTestStatistics(int testId);

        CorrectAnswer? GetQuestionCorrectAnswer(int questionId);
        Test? GetTestById(int testId, bool loadAnswers);

        void RemoveTest(int testId);
        Task AddTest(Test value);
        Task SaveAsync();
    }
}
