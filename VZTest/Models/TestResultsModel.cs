using VZTest.Models.Test;

namespace VZTest.Models
{
    public class TestResultsModel
    {
        public Test.Test Test { get; set; }
        public IEnumerable<Attempt> Attempts { get; set; }
        public bool NotFound { get; set; }
        public bool Forbidden { get; set; }
    }
}
