using VZTest.Data;
using VZTest.Models.Test;
using VZTest.Models.Test.Answers;
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

        public IEnumerable<DateAnswer> GetDateAnswers()
            => db.DateAnswers;

        public IEnumerable<DateAnswerOptional> GetDateAnswersOptional()
            => db.DateAnswersOptional;

        public IEnumerable<DoubleAnswer> GetDoubleAnswers()
            => db.DoubleAnswers;

        public IEnumerable<DoubleAnswerOptional> GetDoubleAnswersOptional()
            => db.DoubleAnswersOptional;

        public IEnumerable<IntAnswer> GetIntAnswers()
            => db.IntAnswers;

        public IEnumerable<IntAnswerOptional> GetIntAnswersOptional()
            => db.IntAnswersOptional;

        public IEnumerable<RadioAnswer> GetRadioAnswers()
            => db.RadioAnswers;

        public IEnumerable<RadioAnswerOptional> GetRadioAnswersOptional()
            => db.RadioAnswersOptional;

        public IEnumerable<TextAnswer> GetTextAnswers()
            => db.TextAnswers;

        public IEnumerable<TextAnswerOptional> GetTextAnswersOptional()
            => db.TextAnswersOptional;

        public async Task SaveAsync()
            => await db.SaveChangesAsync();

        public void Update(Answer value)
            => db.Answers.Update(value);
    }
}
