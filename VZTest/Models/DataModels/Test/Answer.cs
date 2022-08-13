using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.DataModels.Test;

public class Answer
{
    [Key]
    public int Id { get; set; }
    public int AttemptId { get; set; }
    public int QuestionId { get; set; }
    public DateTime? DateAnswer { get; set; }
    public double? DoubleAnswer { get; set; }
    public string? TextAnswer { get; set; }
    public int? IntAnswer { get; set; }
    public double Balls { get; set; }

    public void Flush()
    {
        DateAnswer = null;
        DoubleAnswer = null;
        TextAnswer = null;
        IntAnswer = null;
    }
}