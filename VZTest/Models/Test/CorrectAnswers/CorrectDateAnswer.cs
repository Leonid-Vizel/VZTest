using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.Test.CorrectAnswers
{
    public class CorrectDateAnswer : CorrectAnswer
    {
        [Required(ErrorMessage = "Укажите ответ на вопрос!")]
        public DateTime Correct { get; set; }

        public CorrectDateAnswer() { }

        public CorrectDateAnswer(DateTime correct)
        {
            Correct = correct;
        }
    }
}
