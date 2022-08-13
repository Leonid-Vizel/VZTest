using VZTest.Models.DataModels.Test;
using VZTest.Models.Enumerations.Test;

namespace VZTest.Models.ViewModels.Test;

public class QuestionBlueprint
{
    public int Id { get; set; }
    public string Title { get; set; }
    public QuestionType Type { get; set; }
    public string? ImageUrl { get; set; }
    public double Balls { get; set; }
    public List<int> OptionIds { get; set; }
    public List<string> Options { get; set; }
    public string Correct { get; set; }
}
