using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.DataModels.Test.CorrectAnswers;

public class CorrectDateAnswer : CorrectAnswer
{
    [Required(ErrorMessage = "Укажите ответ на вопрос!")]
    public DateTime Correct { get; set; }

    public CorrectDateAnswer() { }

    public CorrectDateAnswer(DateTime correct)
    {
        Correct = correct;
    }

    public override string ToString() => Correct.ToString("dd.MM.yyyy");

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        CorrectDateAnswer? objAnswer = obj as CorrectDateAnswer;
        if (objAnswer == null)
        {
            return false;
        }
        return objAnswer.Correct == Correct;
    }
}