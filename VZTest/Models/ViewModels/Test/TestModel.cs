namespace VZTest.Models.ViewModels.Test
{
    public class TestModel : DataModels.Test.Test
    {
        public List<QuestionModel> Questions { get; set; }

        public TestModel(DataModels.Test.Test test, List<QuestionModel> questions) : base()
        {
            Id = test.Id;
            Title = test.Title;
            Description = test.Description;
            ImageUrl = test.ImageUrl;
            UserId = test.UserId;
            MaxAttempts = test.MaxAttempts;
            Opened = test.Opened;
            Public = test.Public;
            PasswordHash = test.PasswordHash;
            CreatedTime = test.CreatedTime;
            EditedTime = test.EditedTime;
            StartTime = test.StartTime;
            EndTime = test.EndTime;
            Shuffle = test.Shuffle;
            Questions = questions;
        }

        public TestEditModel ToEditModel(int id)
        {
            return new TestEditModel()
            {
                Id = id,
                Title = Title,
                Description = Description,
                ImageUrl = ImageUrl,
                StartTime = StartTime,
                EndTime = EndTime,
                MaxAttempts = MaxAttempts,
                Shuffle = Shuffle,
                Questions = Questions.Select(x => x.ToBlueprint()).ToList()
            };
        }
    }
}
