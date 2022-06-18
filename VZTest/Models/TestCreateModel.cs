using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VZTest.Models.Test;

namespace VZTest.Models
{
    public class TestCreateModel
    {
        [Required(ErrorMessage = "Добавьте название теста")]
        [DisplayName("Название теста")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Добавьте описание теста")]
        [DisplayName("Описание теста")]
        public string? Description { get; set; }
        public string? ImageName { get; set; }
        [Required(ErrorMessage = "Добавьте изображение для теста")]
        public IFormFile? ImageFile { get; set; }
        [Required(ErrorMessage = "Максимальное количество попыток")]
        public int MaxAttempts { get; set; }
        [DisplayName("Перемешивать")]
        public bool Shuffle { get; set; }
        [DisplayName("Пароль")]
        public string? Password { get; set; }
        public List<Question> Questions { get; set; }
    }
}
