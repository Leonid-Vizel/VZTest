namespace VZTest.Models
{
    public class TestCreateModel
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int MaxAttempts { get; set; }
        public bool Shuffle { get; set; }
        public string? Password { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public List<QuestionBlueprint> Questions { get; set; }

        public bool Validate()
        {
            if (Title.Length == 0 || MaxAttempts <= 0 || Questions.Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}
