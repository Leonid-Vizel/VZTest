using VZTest.Data;
using VZTest.Models.Test;
using VZTest.Models.Test.CorrectAnswers;
using VZTest.Repository.IRepository;

namespace VZTest.Repository.Repository
{
    public class CorrectAnswerRepository : Repository<CorrectAnswer>, ICorrectAnswerRepository
    {
        private readonly ApplicationDbContext db;

        public CorrectAnswerRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task SaveAsync()
            => await db.SaveChangesAsync();

        public void Update(CorrectAnswer value)
            => db.CorrectAnswers.Update(value);
    }
}
