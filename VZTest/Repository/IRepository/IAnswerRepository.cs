using VZTest.Models.Test;
using VZTest.Models.Test.Answers;

namespace VZTest.Repository.IRepository
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        IEnumerable<DateAnswer> GetDateAnswers();
        IEnumerable<IntAnswer> GetIntAnswers();
        IEnumerable<DoubleAnswer> GetDoubleAnswers();
        IEnumerable<TextAnswer> GetTextAnswers();
        IEnumerable<RadioAnswer> GetRadioAnswers();
        IEnumerable<CheckAnswer> GetCheckAnswers();

        IEnumerable<DateAnswerOptional> GetDateAnswersOptional();
        IEnumerable<IntAnswerOptional> GetIntAnswersOptional();
        IEnumerable<DoubleAnswerOptional> GetDoubleAnswersOptional();
        IEnumerable<TextAnswerOptional> GetTextAnswersOptional();
        IEnumerable<RadioAnswerOptional> GetRadioAnswersOptional();
        IEnumerable<CheckAnswerOptional> GetCheckAnswersOptional();

        void Update(Answer value);
        Task SaveAsync();
    }
}
