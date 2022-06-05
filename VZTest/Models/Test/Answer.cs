using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VZTest.Models.Test
{
    public abstract class Answer
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Attempt")]
        public int AttemptId { get; set; }
        public Attempt Attempt { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
