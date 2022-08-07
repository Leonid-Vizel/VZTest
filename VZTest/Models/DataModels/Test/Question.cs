using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VZTest.Models.Enumerations.Test;
using VZTest.Models.ViewModels.Test;

namespace VZTest.Models.DataModels.Test;

public class Question
{
    [Key]
    public int Id { get; set; }
    public int TestId { get; set; }
    [Required(ErrorMessage = "")]
    public string Title { get; set; }
    public QuestionType Type { get; set; }
    public string? ImageUrl { get; set; }
    public double Balls { get; set; }
    [NotMapped]
    public List<Option> Options { get; set; }
    [NotMapped]
    public CorrectAnswer? CorrectAnswer { get; set; }

    public QuestionBlueprint ToBlueprint()
    {
        return new QuestionBlueprint()
        {
            Id = Id,
            Title = Title,
            Type = Type,
            ImageUrl = ImageUrl,
            Balls = Balls,
            Options = Options.Select(x => x.Title).ToList(),
            OptionIds = Options.Select(x => x.Id).ToList(),
            Correct = CorrectAnswer?.ToString()
        };
    }
}