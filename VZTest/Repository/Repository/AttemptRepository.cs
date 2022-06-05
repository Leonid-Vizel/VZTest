using VZTest.Data;
using VZTest.Models.Test;
using VZTest.Repository.IRepository;

namespace VZTest.Repository.Repository
{
    public class AttemptRepository : Repository<Attempt>, IAttemptRepository
    {
        private readonly ApplicationDbContext db;

        public AttemptRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Update(Attempt value)
        {
            db.Attempts.Update(value);
        }
    }
}
