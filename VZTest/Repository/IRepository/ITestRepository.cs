using VZTest.Models.Test;

namespace VZTest.Repository.IRepository
{
    public interface ITestRepository : IRepository<Test>
    {
        void Update(Test value);
        Task SaveAsync();
    }
}
