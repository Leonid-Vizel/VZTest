using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.DataModels.Test.CorrectAnswers;

public class CorrectIntAnswer : CorrectAnswer
{
    [Required(ErrorMessage = "Укажите ответ на вопрос!")]
    public int Correct { get; set; }

    public CorrectIntAnswer() { }

    public CorrectIntAnswer(int correct)
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
        CorrectIntAnswer? objAnswer = obj as CorrectIntAnswer;
        if (objAnswer == null)
        {
            return false;
        }
        return objAnswer.Correct == Correct;
    }

}