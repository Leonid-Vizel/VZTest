using VZTest.Data;
using VZTest.Models.Test;
using VZTest.Repository.IRepository;

namespace VZTest.Repository.Repository
{
    public class TestRepository : Repository<Test>, ITestRepository
    {
        private readonly ApplicationDbContext db;

        public TestRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Update(Test value)
        {
            db.Tests.Update(value);
        }
    }
}
