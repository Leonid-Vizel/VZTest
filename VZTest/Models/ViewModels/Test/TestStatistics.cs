namespace VZTest.Models.ViewModels.Test;

public class TestStatistics
{
    public DataModels.Test.Test Test { get; set; }
    public int AttemptsCount { get; set; }
    public int QuestionCount { get; set; }
    public int StarsCount { get; set; }
    public bool CurrectUserStarred { get; set; }
}