using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.Test.Answers
{
    public class TextAnswer : Answer
    {
        [Required(ErrorMessage = "Укажите ответ на данный вопрос")]
        public string Answer { get; set; }
    }
}
