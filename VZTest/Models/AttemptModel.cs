namespace VZTest.Models
{
    public class AttemptModel
    {
        public Test.Test Test { get; set; }
        public Test.Attempt Attempt { get; set; }

        public bool NotFound { get; set; }
        public bool Forbidden { get; set; }
    }
}
