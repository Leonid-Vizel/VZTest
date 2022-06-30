using VZTest.Models.Test;
using VZTest.Models.Test.CorrectAnswers;

namespace VZTest.Models
{
    public class TestCreateModel
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int MaxAttempts { get; set; }
        public bool Shuffle { get; set; }
        public string? Password { get; set; }
        public List<QuestionBlueprint> Questions { get; set; }
    }

    public class QuestionBlueprint
    {
        public int Id { get; set; } //Only if converted from Question
        public string Title { get; set; }
        public QuestionType Type { get; set; }
        public string? ImageUrl { get; set; }
        public double Balls { get; set; }
        public List<int> OptionIds { get; set; }
        public List<string> Options { get; set; }
        public string Correct { get; set; }

        public Question? ToQuestion()
        {
            if (Correct.Length == 0)
            {
                return null;
            }
            Question question = new Question();
            question.Title = Title;
            question.Type = Type;
            question.ImageUrl = ImageUrl;
            question.Balls = Balls;
            question.Options = new List<Option>();
            Options?.ForEach(x => question.Options.Add(new Option(x)));
            if ((question.Type == QuestionType.Radio || question.Type == QuestionType.Check) && question.Options.Count == 0)
            {
                return null;
            }
            switch (question.Type)
            {
                case QuestionType.Text:
                    question.CorrectAnswer = new CorrectTextAnswer(Correct);
                    break;
                case QuestionType.Int:
                    if (!int.TryParse(Correct,out int intResult))
                    {
                        return null;
                    }
                    question.CorrectAnswer = new CorrectIntAnswer(intResult);
                    break;
                case QuestionType.Double:
                    if (!double.TryParse(Correct, out double doubleResult))
                    {
                        return null;
                    }
                    question.CorrectAnswer = new CorrectDoubleAnswer(doubleResult);
                    break;
                case QuestionType.Date:
                    if (!DateTime.TryParse(Correct, out DateTime dateResult))
                    {
                        return null;
                    }
                    question.CorrectAnswer = new CorrectDateAnswer(dateResult);
                    break;
                case QuestionType.Check:
                    string[] splitIds = Correct.Split(',');
                    if (splitIds.Length == 0)
                    {
                        return null;
                    }
                    for (int i = 0; i < splitIds.Length; i++)
                    {
                        if (!int.TryParse(splitIds[i], out int checkId) || Options.Count <= checkId)
                        {
                            return null;
                        }
                    }
                    question.CorrectAnswer = new CorrectCheckAnswer(Correct);
                    break;
                case QuestionType.Radio:
                    if (!int.TryParse(Correct, out int radioResult) || Options.Count <= radioResult)
                    {
                        return null;
                    }
                    question.CorrectAnswer = new CorrectIntAnswer(radioResult);
                    break;
            }
            return question;
        }
    }
}
