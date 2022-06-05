using VZTest.Models.Test;

namespace VZTest.Repository.IRepository
{
    public interface IOptionRepository : IRepository<Option>
    {
        void Update(Option value);
        Task SaveAsync();
    }
}
