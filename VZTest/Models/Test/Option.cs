using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VZTest.Models.Test
{
    public class Option
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        [Required(ErrorMessage = "Укажите наименование опции")]
        public string Title { get; set; }
        public bool Correct { get; set; }
    }
}
