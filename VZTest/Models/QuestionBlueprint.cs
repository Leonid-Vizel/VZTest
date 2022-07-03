using VZTest.Models.Test;
using VZTest.Models.Test.CorrectAnswers;

namespace VZTest.Models
{
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

        public Question? ToQuestion(bool transferId)
        {
            if (Correct == null || Correct.Length == 0)
            {
                return null;
            }
            if (OptionIds != null && OptionIds.Count != 0 && OptionIds.Count != Options.Count)
            {
                return null;
            }
            Question question = new Question();
            if (transferId)
            {
                question.Id = Id;
            }
            question.Title = Title;
            question.Type = Type;
            if (Uri.IsWellFormedUriString(ImageUrl, UriKind.RelativeOrAbsolute))
            {
                question.ImageUrl = ImageUrl;
            }
            question.Balls = Balls;
            question.Options = new List<Option>();
            if (question.Type == QuestionType.Check || question.Type == QuestionType.Radio)
            {
                for (int i = 0; i < Options.Count; i++)
                {
                    Option option = new Option();
                    option.QuestionId = Id;
                    option.Title = Options[i];
                    if (OptionIds != null)
                    {
                        option.Id = OptionIds[i];
                    }
                    question.Options.Add(option);
                }
            }
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
                    if (!int.TryParse(Correct, out int intResult))
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
            if (transferId)
            {
                question.CorrectAnswer.QuestionId = question.Id;
            }
            return question;
        }
    }
}
