using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.DataModels.Test;

public class Attempt
{
    [Key]
    public int Id { get; set; }
    public int TestId { get; set; }
    public bool Active { get; set; }
    public DateTime TimeStarted { get; set; }
    public string UserId { get; set; } = null!;
    public int CorrectAnswers { get; set; }
    public string Sequence { get; set; } = null!;
}