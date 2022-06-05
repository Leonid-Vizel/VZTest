using VZTest.Models.Test;

namespace VZTest.Repository.IRepository
{
    public interface IAttemptRepository : IRepository<Attempt>
    {
        void Update(Attempt value);
        Task SaveAsync();
    }
}
