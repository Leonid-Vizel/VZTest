using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VZTest.Models.Test.Answers
{
    public class RadioAnswer : Answer
    {
        [Required(ErrorMessage = "Укажите ответ на данный вопрос")]
        public int OptionId { get; set; }
    }
}
