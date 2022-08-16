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

        public CorrectCheckAnswer() { }

        public CorrectCheckAnswer(string correct)
        {
            CheckAnswerString = correct;
        }

        public CorrectCheckAnswer(int[] correct)
        {
            Correct = correct;
        }

        public override string ToString() => CheckAnswerString;

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            CorrectCheckAnswer? objAnswer = obj as CorrectCheckAnswer;
            if (objAnswer == null)
            {
                return false;
            }
            int[] array1 = objAnswer.Correct;
            int[] array2 = Correct;
            if (array1.Length != array2.Length)
            {
                return false;
            }
            Array.Sort(array1);
            Array.Sort(array2);
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
