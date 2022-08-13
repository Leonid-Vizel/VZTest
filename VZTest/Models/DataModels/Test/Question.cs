using System.ComponentModel.DataAnnotations;
using VZTest.Models.Enumerations.Test;

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
}