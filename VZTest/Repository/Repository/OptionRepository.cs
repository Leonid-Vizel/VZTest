using VZTest.Data;
using VZTest.Models.Test;
using VZTest.Repository.IRepository;

namespace VZTest.Repository.Repository
{
    public class OptionRepository : Repository<Option>, IOptionRepository
    {
        private readonly ApplicationDbContext db;

        public OptionRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Update(Option value)
        {
            db.Options.Update(value);
        }
    }
}
