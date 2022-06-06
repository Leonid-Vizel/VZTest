using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VZTest.Data;
using VZTest.Models;
using VZTest.Models.Test;
using VZTest.Models.Test.Answers;
using VZTest.Models.Test.CorrectAnswers;
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
            Question question1 = new Question();
            question1.Title = "Введите 123";
            question1.Number = 0;
            question1.Balls = 1;
            question1.Type = QuestionType.Int;
            CorrectIntAnswer answer1 = new CorrectIntAnswer();
            answer1.Correct = 123;
            question1.CorrectAnswer = answer1;

            Question question2 = new Question();
            question2.Title = "Введите ZOV";
            question2.Number = 1;
            question2.Balls = 1;
            question2.Type = QuestionType.Text;
            CorrectTextAnswer answer2 = new CorrectTextAnswer();
            answer2.Correct = "ZOV";
            question2.CorrectAnswer = answer2;

            Question question3 = new Question();
            question3.Title = "Введите 01.05.2020";
            question3.Number = 2;
            question3.Balls = 1;
            question3.Type = QuestionType.Date;
            CorrectDateAnswer answer3 = new CorrectDateAnswer();
            answer3.Correct = new DateTime(2020,05,01);
            question3.CorrectAnswer = answer3;

            Test test = new Test();
            test.Title = "Первый тест в системе!";
            test.CreatedTime = DateTime.Now;
            test.UserId = "43e44405-cdcf-437b-95dc-e3d2a8c2cc1d";
            test.Description = "Описание крутого опроса";
            test.Opened = false;
            test.MaxAttempts = 1;
            test.Questions = new List<Question>() { question1, question2, question3 };
            //await unitOfWork.AddTest(test);
            //await unitOfWork.SaveAsync();
            return View();
        }

        public IActionResult About()
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