using VZTest.Data;
using VZTest.Models;
using VZTest.Repository.IRepository;

namespace VZTest.Repository.Repository
{
    public class UserStarRepository : Repository<UserStar>, IUserStarRepository
    {
        private readonly ApplicationDbContext db;

        public UserStarRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Update(UserStar value)
        {
            db.UserStars.Update(value);
        }
    }
}