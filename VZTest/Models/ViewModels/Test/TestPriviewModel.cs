using VZTest.Models.DataModels.Test;

namespace VZTest.Models.ViewModels.Test;

public class TestPriviewModel
{
    public DataModels.Test.Test Test { get; set; }
    public int TotalAttempts { get; set; }
    public int StarsCount { get; set; }
    public bool NotFound { get; set; }
    public bool Forbidden { get; set; }
    public bool Closed { get; set; }
    public bool BeforeStart { get; set; }
    public bool AfterEnd { get; set; }
    public bool Liked { get; set; }
    public double MaxBalls { get; set; }
    public IEnumerable<Attempt> UserAttempts { get; set; }
}