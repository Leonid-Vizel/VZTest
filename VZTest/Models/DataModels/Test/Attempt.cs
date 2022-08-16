using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VZTest.Models.DataModels.Test;

public class Attempt
{
    [Key]
    public int Id { get; set; }
    public int TestId { get; set; }
    public bool Active { get; set; }
    public DateTime TimeStarted { get; set; }
    public string UserId { get; set; }
    public int CorrectAnswers { get; set; }
    public string Sequence
    {
        get => string.Join('-', QuestionSequence);
        set => QuestionSequence = value.Split('-');
    }
    [NotMapped]
    public List<Answer> Answers { get; set; }
    [NotMapped]
    public double Balls => Answers.Sum(x => x.Balls);
    [NotMapped]
    public string[] QuestionSequence { get; set; }

    public Attempt()
    {
        QuestionSequence = new string[0];
    }
}