using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VZTest.Data;
using VZTest.Models;
using VZTest.Models.Test;
using VZTest.Models.Test.Answers;
using VZTest.Repository.IRepository;

namespace VZTest.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            //Option option = new Option()
            //{
            //    Correct = true,
            //    Title = "ABOBA",
            //};
            //Question question1 = new Question()
            //{
            //    Title = "123",
            //    Type = QuestionType.Int,
            //    ImageName = "123"
            //};
            //Question question2 = new Question()
            //{
            //    Title = "321",
            //    Type = QuestionType.Radio,
            //    ImageName = "321",
            //    Options = new List<Option>() { option }
            //};
            //Test test1 = new Test()
            //{
            //    Title = "123",
            //    Description = "312",
            //    CreatedTime = DateTime.Now,
            //    ImageName = "123",
            //    Opened = true,
            //    UserId = "123",
            //    Questions = new List<Question>() { question1, question2 }
            //};
            //await unitOfWork.AddTest(test1);
            //await unitOfWork.Save();
            var a = unitOfWork.GetUserTests("123", false).ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}