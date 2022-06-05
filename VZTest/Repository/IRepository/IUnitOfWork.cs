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

        IEnumerable<Attempt> GetUserAttempts(string userId);
        IEnumerable<Test> GetUserTests(string userId);
        IEnumerable<Question> GetTestQuestions(int testId);
        IEnumerable<Option> GetQuestionOptions(int questionId);
        IEnumerable<Answer> GetAttemptAnswers(int attemptId);

        Task AddTest(Test value);

        Task Save();
    }
}
