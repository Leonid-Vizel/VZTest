using System.ComponentModel.DataAnnotations;
using VZTest.Models.Test;

namespace VZTest.Models
{
    public class TestCreateModel
    {
        [Required(ErrorMessage = "Укажите название теста!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Укажите описание теста!")]
        public string? Description { get; set; }
        public string? ImageName { get; set; }
        public IFormFile? ImageFile { get; set; }
        [Range(1,100,ErrorMessage = "Максимальное количество попыток должно быть в пределах от 1 до 100")]
        [Required(ErrorMessage = "Максимальное количество попыток должно быть указано")]
        [RegularExpression(@"[0-9]*$", ErrorMessage = "Some message if value entered isn't number")]
        public int MaxAttempts { get; set; }
        public bool Shuffle { get; set; }
        public string? Password { get; set; }
        public List<QuestionBlueprint> Questions { get; set; }
    }

    public class QuestionBlueprint
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string Title { get; set; }
        public QuestionType Type { get; set; }
        public string? ImageName { get; set; }
        public double Balls { get; set; }
        public List<Option> Options { get; set; }
        public string Correct { get; set; }
    }
}
