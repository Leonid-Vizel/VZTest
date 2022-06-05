using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VZTest.Models.Test
{
    public class Attempt
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Test")]
        public int TestId { get; set; }
        public Test Test { get; set; }
        public DateTime TimeStarted { get; set; }
        public string UserId { get; set; }
        [NotMapped]
        public List<Answer> Answers { get; set; }
    }
}
