using System.ComponentModel.DataAnnotations.Schema;

namespace VZTest.Models.Test.Answers
{
    public class RadioAnswerOptional : Answer
    {
        public int? OptionId { get; set; }
        [ForeignKey("Option")]
        public Option? Option { get; set; }
    }
}
