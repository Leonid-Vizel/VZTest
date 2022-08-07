using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.DataModels.Test.CorrectAnswers;

public class CorrectTextAnswer : CorrectAnswer
{
    [Required(ErrorMessage = "Укажите ответ на вопрос!")]
    public string Correct { get; set; }

    public CorrectTextAnswer() { }

    public CorrectTextAnswer(string correct)
    {
        Correct = correct;
    }

    public override string ToString() => Correct;

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        CorrectTextAnswer? objAnswer = obj as CorrectTextAnswer;
        if (objAnswer == null)
        {
            return false;
        }
        return objAnswer.Correct.Equals(Correct);
    }
}