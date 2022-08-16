using VZTest.Models.DataModels.Test;

namespace VZTest.Models.ViewModels.Test
{
    public class QuestionModel : Question
    {
        public List<Option> Options { get; set; }
        public CorrectAnswer CorrectAnswer { get; set; }

        public QuestionModel(Question question, List<Option> options, CorrectAnswer answer)
        {
            Id = question.Id;
            TestId = question.TestId;
            Title = question.Title;
            Type = question.Type;
            ImageUrl = question.ImageUrl;
            Balls = question.Balls;
            Options = options;
            CorrectAnswer = answer;
        }

        public QuestionModel() { }

        public QuestionBlueprint ToBlueprint()
        {
            QuestionBlueprint blueprint = new QuestionBlueprint()
            {
                Id = Id,
                Title = Title,
                Type = Type,
                ImageUrl = ImageUrl,
                Balls = Balls,
                Options = Options.Select(x => x.Title).ToList(),
                OptionIds = Options.Select(x => x.Id).ToList(),
            };
            if (Type == Enumerations.Test.QuestionType.Check)
            {
                blueprint.Correct = CorrectAnswer.ToString().Replace("-",",");
            }
            else
            {
                blueprint.Correct = CorrectAnswer.ToString();
            }

            return blueprint;
        }
    }
}
