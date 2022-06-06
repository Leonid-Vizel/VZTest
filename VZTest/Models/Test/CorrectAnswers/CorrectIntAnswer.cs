using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.Test.CorrectAnswers
{
    public class CorrectIntAnswer : CorrectAnswer
    {
        [Required(ErrorMessage = "Укажите ответ на вопрос!")]
        public int Correct { get; set; }
    }
}
