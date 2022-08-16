using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
        public int[] CheckAnswers
        {
            get
            {
                if (string.IsNullOrEmpty(CheckAnswerString))
                {
                    return new int[0] { };
                }
                string[] tab = CheckAnswerString.Split(',');
                int[] result = new int[tab.Length];
                for (int i = 0; i < tab.Length; i++)
                {
                    result[i] = int.Parse(tab[i]);
                }
                return result;
            }
            set
            {
                if (value == null || value.Length == 0)
                {
                    CheckAnswerString = null;
                    return;
                }
                StringBuilder builder = new StringBuilder();
                foreach (int integer in value)
                {
                    builder.Append($"{integer},");
                }
                builder.Remove(builder.Length - 1, 1);
                CheckAnswerString = builder.ToString();
            }
        }
        public string? CheckAnswerString { get; set; }
        #endregion
        public DateTime? DateAnswer { get; set; }
        public double? DoubleAnswer { get; set; }
        public string? TextAnswer { get; set; }
        public int? RadioAnswer { get; set; }
        public int? IntAnswer { get; set; }
        public double Balls { get; set; }
    }
}
