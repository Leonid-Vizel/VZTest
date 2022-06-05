using VZTest.Data;
using VZTest.Models.Test;
using VZTest.Repository.IRepository;

namespace VZTest.Repository.Repository
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly ApplicationDbContext db;

        public QuestionRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Update(Question value)
        {
            db.Questions.Update(value);
        }
    }
}
