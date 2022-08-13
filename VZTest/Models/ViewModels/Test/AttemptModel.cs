using VZTest.Models.DataModels.Test;

namespace VZTest.Models.ViewModels.Test
{
    public class AttemptModel : Attempt
    {
        public List<Answer> Answers { get; set; }
        public double Balls => Answers.Sum(x => x.Balls);
        public string[] QuestionSequence
        {
            get => Sequence.Split('-');
            set => Sequence = string.Join('-', QuestionSequence);
        }

        public AttemptModel(Attempt attempt, List<Answer> answers)
        {
            Id = attempt.Id;
            TestId = attempt.TestId;
            Active = attempt.Active;
            TimeStarted = attempt.TimeStarted;
            UserId = attempt.UserId;
            CorrectAnswers = attempt.CorrectAnswers;
            Sequence = attempt.Sequence;
            Answers = answers;
        }
    }
}
