using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.DataModels.Test.CorrectAnswers;

public class CorrectDoubleAnswer : CorrectAnswer
{
    [Required(ErrorMessage = "Укажите ответ на вопрос!")]
    public double Correct { get; set; }

    public CorrectDoubleAnswer() { }

    public CorrectDoubleAnswer(double correct)
    {
        Correct = correct;
    }

    public override string ToString() => Correct.ToString();

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        CorrectDoubleAnswer? objAnswer = obj as CorrectDoubleAnswer;
        if (objAnswer == null)
        {
            return false;
        }
        return objAnswer.Correct == Correct;
    }
}