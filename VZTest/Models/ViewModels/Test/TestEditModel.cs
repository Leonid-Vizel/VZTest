namespace VZTest.Models.ViewModels.Test;

public class TestEditModel : TestCreateModel
{
    public int Id { get; set; }
    public bool Forbidden { get; set; }
    public bool NotFound { get; set; }
    public bool TestOpened { get; set; }
}