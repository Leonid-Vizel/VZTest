using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VZTest.Models.Test
{
    public class Attempt
    {
        [Key]
        public int Id { get; set; }
        public int TestId { get; set; }
        public bool Active { get; set; }
        public DateTime TimeStarted { get; set; }
        public string UserId { get; set; }
        [NotMapped]
        public IEnumerable<Answer> Answers { get; set; }
    }
}
