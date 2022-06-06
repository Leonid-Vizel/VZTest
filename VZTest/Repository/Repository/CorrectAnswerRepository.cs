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

        public IEnumerable<CorrectDateAnswer> GetDateAnswers()
            => db.CorrectDateAnswers;

        public IEnumerable<CorrectDoubleAnswer> GetDoubleAnswers()
            => db.CorrectDoubleAnswers;

        public IEnumerable<CorrectIntAnswer> GetIntAnswers()
            => db.CorrectIntAnswers;

        public IEnumerable<CorrectTextAnswer> GetTextAnswers()
            => db.CorrectTextAnswers;

        public async Task SaveAsync()
            => await db.SaveChangesAsync();

        public void Update(CorrectAnswer value)
            => db.CorrectAnswers.Update(value);
    }
}
