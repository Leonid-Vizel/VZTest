using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VZTest.Models.Test
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Test")]
        public int TestId { get; set; }
        public Test Test { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string? ImageName { get; set; }
    }
}
