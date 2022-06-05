using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.Test.Answers : Answer
{
    public class IntAnswer
    {
        [Required(ErrorMessage = "Укажите ответ на данный вопрос")]
        public int Answer { get; set; }
    }
}
