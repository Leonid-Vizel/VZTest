using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VZTest.Models.Test.CorrectAnswers
{
    public class CorrectCheckAnswer : CorrectAnswer
    {
        [NotMapped]
        public int[] Correct
        {
            get
            {
                if (CheckAnswerString == null)
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
                if (value.Length == 0)
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
    }
}
