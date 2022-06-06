using VZTest.Models.Test;
using VZTest.Models.Test.CorrectAnswers;

namespace VZTest.Repository.IRepository
{
    public interface ICorrectAnswerRepository : IRepository<CorrectAnswer>
    {
        void Update(CorrectAnswer value);
        Task SaveAsync();
        IEnumerable<CorrectDateAnswer> GetDateAnswers();
        IEnumerable<CorrectIntAnswer> GetIntAnswers();
        IEnumerable<CorrectDoubleAnswer> GetDoubleAnswers();
        IEnumerable<CorrectTextAnswer> GetTextAnswers();
    }
}
