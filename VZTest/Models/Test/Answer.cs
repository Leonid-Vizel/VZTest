using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VZTest.Models.Test
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public int AttemptId { get; set; }
        public int QuestionId { get; set; }
        #region Check
        [NotMapped]
        public int[] CheckAnswer
        {
            get
            {
                string[] tab = CheckAnswerString.Split(',');
                return new int[] { int.Parse(tab[0]), int.Parse(tab[1]) };
            }
            set
            {
                CheckAnswerString = string.Format("{0},{1}", value[0], value[1]);
            }
        }
        public string? CheckAnswerString { get; set; }
        #endregion
        public DateTime? DateAnswer { get; set; }
        public double? DoubleAnswer { get; set; }
        public string? TextAnswer { get; set; }
        public int? RadioAnswer { get; set; }
        public int? IntAnswer { get; set; }
    }
}
