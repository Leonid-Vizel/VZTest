using VZTest.Data;
using VZTest.Models.Test;
using VZTest.Repository.IRepository;

namespace VZTest.Repository.Repository
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        private readonly ApplicationDbContext db;

        public AnswerRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Update(Answer value)
        {
            db.Answers.Update(value);
        }
    }
}
