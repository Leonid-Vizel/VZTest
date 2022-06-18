using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VZTest.Models.Test;

namespace VZTest.Models
{
    public class TestCreateModel
    {
        [Required(ErrorMessage = "Укажите название теста!")]
        [DisplayName("Название")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Укажите описание теста!")]
        [DisplayName("Описание")]
        public string? Description { get; set; }
        public string? ImageName { get; set; }
        public IFormFile? ImageFile { get; set; }
        [Range(1,100,ErrorMessage = "Максимальное количество попыток должно быть в пределах от 1 до 100")]
        [Required(ErrorMessage = "Максимальное количество попыток должно быть указано")]
        [RegularExpression(@"[0-9]*$", ErrorMessage = "Some message if value entered isn't number")]
        [DisplayName("Максимальное количество попыток")]
        public int MaxAttempts { get; set; }
        [DisplayName("Перемешивать порядок вопросов")]
        public bool Shuffle { get; set; }
        [DisplayName("Пароль")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public List<Question> Questions { get; set; }
    }
}
