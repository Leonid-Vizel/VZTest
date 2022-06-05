using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VZTest.Models.Test
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Title { get; set; }
        public QuestionType Type { get; set; }
        public string? ImageName { get; set; }
        public int Number { get; set; }
        [NotMapped]
        public IEnumerable<Option> Options { get; set; }
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
