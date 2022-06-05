using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.Test.Answers
{
    public class IntAnswer : Answer
    {
        [Required(ErrorMessage = "Укажите ответ на данный вопрос")]
        public int Answer { get; set; }
    }
}
