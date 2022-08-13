namespace VZTest.Models.ViewModels.Test
{
    public class ResultViewModel : AttemptViewModel
    {
        public double MaxBalls { get; set; }

        public ResultViewModel(AttemptViewModel model)
        {
            Test = model.Test;
            Attempt = model.Attempt;
            NotFound = model.NotFound;
            Forbidden = model.Forbidden;
        }
    }
}
