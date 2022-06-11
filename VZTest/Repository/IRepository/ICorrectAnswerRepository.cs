using VZTest.Models.Test;

namespace VZTest.Repository.IRepository
{
    public interface ICorrectAnswerRepository : IRepository<CorrectAnswer>
    {
        void Update(CorrectAnswer value);
        Task SaveAsync();
    }
}
