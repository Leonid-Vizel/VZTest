using VZTest.Models.Test;

namespace VZTest.Models
{
    public class AttemptModel
    {
        public Test.Test Test { get; set; }
        public Attempt Attempt { get; set; }

        public void AlignQuestions()
        {
            List<Question> alignedQuestions = new List<Question>();
            IEnumerable<int> questionIds = Attempt.QuestionSequence.Select(x => int.Parse(x));
            foreach (int questionId in questionIds)
            {
                Question? question = Test.Questions.FirstOrDefault(x => x.Id == questionId);
                if (question != null)
                {
                    alignedQuestions.Add(question);
                }
            }
            Test.Questions = alignedQuestions;
        }

        public bool NotFound { get; set; }
        public bool Forbidden { get; set; }
        public double MaxBalls { get; set; }
    }
}
