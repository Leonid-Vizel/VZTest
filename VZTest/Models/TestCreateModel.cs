using VZTest.Models.Test;

namespace VZTest.Models
{
    public class TestCreateModel
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageName { get; set; }
        public IFormFile? ImageFile { get; set; }
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
