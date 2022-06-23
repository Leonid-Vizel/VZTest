using System.ComponentModel.DataAnnotations;

namespace VZTest.Models.Test
{
    public class Option
    {
        [Key]
        public int Id { get; set; }
        public int QuestionId { get; set; }
        [Required(ErrorMessage = "Укажите наименование опции")]
        public string Title { get; set; }
    }
}
