namespace VZTest.Models.ViewModels.Test
{
    public class AttemptViewModel
    {
        public TestModel Test { get; set; }
        public AttemptModel Attempt { get; set; }

        public void AlignQuestions()
        {
            List<QuestionModel> alignedQuestions = new List<QuestionModel>();
            IEnumerable<int> questionIds = Attempt.QuestionSequence.Select(x => int.Parse(x));
            foreach (int questionId in questionIds)
            {
                QuestionModel? question = Test.Questions.FirstOrDefault(x => x.Id == questionId);
                if (question != null)
                {
                    alignedQuestions.Add(question);
                }
            }
            Test.Questions = alignedQuestions;
        }

        public bool NotFound { get; set; }
        public bool Forbidden { get; set; }
    }
}
