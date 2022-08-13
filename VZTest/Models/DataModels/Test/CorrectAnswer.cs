using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.DataModels.Test;

public class CorrectAnswer
{
    [Key]
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string? TextAnswer { get; set; } = null!;
    public double? DoubleAnswer { get; set; }
    public int? IntAnswer { get; set; }
    public DateTime? DateAnswer { get; set; }

    public CorrectAnswer()
    {
        TextAnswer = null;
        DoubleAnswer = null;
        IntAnswer = null;
        DateAnswer = null;
    }

    public CorrectAnswer(string? answer)
    {
        TextAnswer = answer;
    }

    public CorrectAnswer(double? answer)
    {
        DoubleAnswer = answer;
    }

    public CorrectAnswer(int? answer)
    {
        IntAnswer = answer;
    }

    public CorrectAnswer(DateTime? answer)
    {
        DateAnswer = answer;
    }

    public void Flush()
    {
        TextAnswer = null;
        DoubleAnswer = null;
        IntAnswer = null;
        DateAnswer = null;
    }

    public void Update(CorrectAnswer source)
    {
        Flush();
        if (source.TextAnswer != null)
        {
            TextAnswer = source.TextAnswer;
        }
        else if (source.IntAnswer != null)
        {
            IntAnswer = source.IntAnswer;
        }
        else if (source.DoubleAnswer != null)
        {
            DoubleAnswer = source.DoubleAnswer;
        }
        else if (source.DateAnswer != null)
        {
            DateAnswer = source.DateAnswer;
        }
    }

    public override string ToString()
    {
        if (TextAnswer != null)
        {
            return TextAnswer;
        }
        else if (IntAnswer != null)
        {
            return IntAnswer.Value.ToString();
        }
        else if (DoubleAnswer != null)
        {
            return DoubleAnswer.Value.ToString();
        }
        else if (DateAnswer != null)
        {
            return DateAnswer.Value.ToString();
        }
        else
        {
            return string.Empty;
        }
    }
}