using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VZTest.Models.Test
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Укажите название теста")]
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "Укажите максимальное количество попыток")]
        public int MaxAttempts { get; set; }
        public bool Opened { get; set; }
        public bool Public { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool Shuffle { get; set; }
        [NotMapped]
        public List<Question> Questions { get; set; }

        public TestEditModel ToEditModel()
        {
            return new TestEditModel()
            {
                Title = Title,
                Description = Description,
                ImageUrl = ImageUrl,
                MaxAttempts = MaxAttempts,
                Shuffle = Shuffle,
                Questions = Questions.Select(x => x.ToBlueprint()).ToList()
            };
        }
    }
}
