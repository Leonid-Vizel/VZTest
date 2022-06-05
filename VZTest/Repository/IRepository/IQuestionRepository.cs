using VZTest.Models.Test;

namespace VZTest.Repository.IRepository
{
    public interface IQuestionRepository : IRepository<Question>
    {
        void Update(Question value);
        Task SaveAsync();
    }
}
