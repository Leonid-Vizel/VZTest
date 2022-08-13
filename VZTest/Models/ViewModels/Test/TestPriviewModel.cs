using VZTest.Models.DataModels.Test;

namespace VZTest.Models.ViewModels.Test;

public class TestPreviewModel
{
    public DataModels.Test.Test Test { get; set; } = null!;
    public int TotalAttempts { get; set; }
    public int StarsCount { get; set; }
    public bool NotFound { get; set; }
    public bool Forbidden { get; set; }
    public bool Closed { get; set; }
    public bool BeforeStart { get; set; }
    public bool AfterEnd { get; set; }
    public bool Liked { get; set; }
    public double MaxBalls { get; set; }
    public List<AttemptModel> UserAttempts { get; set; } = null!;

    public bool Errored => NotFound || Forbidden || Closed || BeforeStart || AfterEnd;
}