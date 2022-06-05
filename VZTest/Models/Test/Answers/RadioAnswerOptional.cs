using System.ComponentModel.DataAnnotations.Schema;

namespace VZTest.Models.Test.Answers
{
    public class RadioAnswerOptional : Answer
    {
        [ForeignKey("Option")]
        public int? OptionId { get; set; }
        public Option? Option { get; set; }
    }
}
