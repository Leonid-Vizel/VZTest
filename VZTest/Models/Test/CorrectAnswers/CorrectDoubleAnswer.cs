using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.Test.CorrectAnswers
{
    public class CorrectDoubleAnswer : CorrectAnswer
    {
        [Required(ErrorMessage = "Укажите ответ на вопрос!")]
        public double Correct;
    }
}
