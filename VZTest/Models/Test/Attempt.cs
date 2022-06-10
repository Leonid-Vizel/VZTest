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
        public List<Answer> Answers { get; set; }
        [NotMapped]
        public double Balls { get; set; }
        [NotMapped]
        public double MaxBalls { get; set; }
        [NotMapped]
        public int CorrectAnswers { get; set; }
    }
}
