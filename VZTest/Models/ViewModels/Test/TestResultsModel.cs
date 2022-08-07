using VZTest.Models.DataModels.Test;

namespace VZTest.Models.ViewModels.Test;

public class TestResultsModel
{
    public DataModels.Test.Test Test { get; set; }
    public IEnumerable<Attempt> Attempts { get; set; }
    public bool NotFound { get; set; }
    public bool Forbidden { get; set; }
    public double MaxBalls { get; set; }
}