using VZTest.Models;

namespace VZTest.Repository.IRepository
{
    public interface IUserStarRepository : IRepository<UserStar>
    {
        void Update(UserStar value);
        Task SaveAsync();
    }
}
