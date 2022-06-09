using VZTest.Models.Test;

namespace VZTest.Repository.IRepository
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        void Update(Answer value);
        Task SaveAsync();
    }
}
