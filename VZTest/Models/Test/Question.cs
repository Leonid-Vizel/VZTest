using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VZTest.Models.Test
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public int TestId { get; set; }
        [Required(ErrorMessage = "")]
        public string Title { get; set; }
        public QuestionType Type { get; set; }
        public string? ImageName { get; set; }
        public int Number { get; set; }
        public double Balls { get; set; }
        [NotMapped]
        public IEnumerable<Option> Options { get; set; }
        public CorrectAnswer? CorrectAnswer { get; set; }
    }
    public enum QuestionType
    {
        Text,
        Radio,
        Int,
        Double,
        Date
    }
}
