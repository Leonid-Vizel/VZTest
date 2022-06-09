using VZTest.Models.Test;
using VZTest.Models.Test.CorrectAnswers;

namespace VZTest.Repository.IRepository
{
    public interface ICorrectAnswerRepository : IRepository<CorrectAnswer>
    {
        void Update(CorrectAnswer value);
        Task SaveAsync();
    }
}
