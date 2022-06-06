using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VZTest.Models.Test.Answers
{
    public class CheckAnswer : Answer
    {
        [NotMapped]
        public int[] Data
        {
            get
            {
                string[] tab = InternalData.Split(',');
                return new int[] { int.Parse(tab[0]), int.Parse(tab[1]) };
            }
            set
            {
                InternalData = string.Format("{0},{1}", value[0], value[1]);
            }
        }

        [Required(ErrorMessage = "Укажите ответ на данный вопрос")]
        public string InternalData { get; set; }
    }
}
