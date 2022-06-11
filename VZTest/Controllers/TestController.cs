using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VZTest.Models;
using VZTest.Models.Test;
using VZTest.Repository.IRepository;

namespace VZTest.Controllers
{
    public class TestController : Controller
    {
        private IUnitOfWork unitOfWork;
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;

        public TestController(IUnitOfWork unitOfWork, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        #region Attempt
        public IActionResult Attempt(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View(null); //Authorize
            }
            Attempt? attempt = unitOfWork.GetAttemptWithAnswers(id);
            AttemptModel attemptModel = new AttemptModel();
            if (attempt == null)
            {
                attemptModel.NotFound = true;
                return View(attemptModel);
            }
            if (!attempt.UserId.Equals(userManager.GetUserId(User)))
            {
                attemptModel.Forbidden = true;
                return View(attemptModel);
            }
            if (!attempt.Active)
            {
                return RedirectToAction("Results", new { Id = attempt.Id });
            }
            Test? test = unitOfWork.GetTestById(attempt.TestId, false);
            if (test == null)
            {
                attemptModel.NotFound = true;
                return View(attemptModel);
            }
            attemptModel.Attempt = attempt;
            attemptModel.Test = test;
            return View(attemptModel); //Ok
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Attempt(int id, AttemptModel model)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View(null); //Authorize
            }
            Attempt? attempt = unitOfWork.AttemptRepository.FirstOrDefault(x => x.Id == id);
            if (attempt == null)
            {
                AttemptModel attemptModel = new AttemptModel();
                attemptModel.NotFound = true;
                return View(attemptModel);
            }
            if (!attempt.UserId.Equals(userManager.GetUserId(User)))
            {
                AttemptModel attemptModel = new AttemptModel();
                attemptModel.Forbidden = true;
                return View(attemptModel);
            }
            if (!attempt.Active)
            {
                return RedirectToAction("Results", new { Id = attempt.Id });
            }
            Test? test = unitOfWork.GetTestById(attempt.TestId, true);
            if (test == null)
            {
                AttemptModel attemptModel = new AttemptModel();
                attemptModel.NotFound = true;
                return View(attemptModel);
            }
            if (test.Questions.Count != model.Attempt.Answers.Count)
            {
                return View(null); //Error
            }
            List<Answer> AttemptAnswers = unitOfWork.GetAttemptAnswers(id).ToList();
            for (int i = 0; i < model.Attempt.Answers.Count; i++)
            {
                Answer? foundAnswer = AttemptAnswers.FirstOrDefault(x => x.QuestionId == test.Questions[i].Id);
                if (foundAnswer != null)
                {
                    foundAnswer.CheckAnswer = model.Attempt.Answers[i].CheckAnswer;
                    foundAnswer.DateAnswer = model.Attempt.Answers[i].DateAnswer;
                    foundAnswer.IntAnswer = model.Attempt.Answers[i].IntAnswer;
                    foundAnswer.DoubleAnswer = model.Attempt.Answers[i].DoubleAnswer;
                    foundAnswer.RadioAnswer = model.Attempt.Answers[i].RadioAnswer;
                    foundAnswer.TextAnswer = model.Attempt.Answers[i].TextAnswer;
                    unitOfWork.AnswerRepository.Update(foundAnswer);
                }
            }
            attempt.Active = false;
            unitOfWork.AttemptRepository.Update(attempt);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Results", new { Id = attempt.Id });
        }
        #endregion

        #region Results

        public IActionResult Results(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View(null); //Authorize
            }
            string userId = userManager.GetUserId(User);
            AttemptModel attemptModel = new AttemptModel();
            Attempt? foundAttempt = unitOfWork.GetCheckedAttempt(id);
            if (foundAttempt == null)
            {
                attemptModel.NotFound = true;
                return View(attemptModel);
            }
            Test? foundTest = unitOfWork.GetTestById(foundAttempt.TestId, true);
            if (foundTest == null)
            {
                attemptModel.NotFound = true;
                return View(attemptModel);
            }
            if (!foundAttempt.UserId.Equals(userId))
            {
                if (!foundTest.UserId.Equals(userId))
                {
                    attemptModel.Forbidden = true;
                    return View(attemptModel);
                }
            }
            if (foundAttempt.Active && foundAttempt.UserId.Equals(userId))
            {
                return RedirectToAction("Attempt", new { Id = foundAttempt.Id });
            }
            attemptModel.Attempt = foundAttempt;
            attemptModel.Test = foundTest;
            return View(attemptModel); //Ok
        }

        #endregion

        #region Preview
        public async Task<IActionResult> Preview(int id, string passwordHash = "")
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View(null);
            }
            string userId = userManager.GetUserId(User);
            TestPriviewModel model = new TestPriviewModel();
            Test? foundTest = unitOfWork.TestRepository.FirstOrDefault(x => x.Id == id);
            if (foundTest == null)
            {
                model.NotFound = true;
                return View(model);
            }
            if (!foundTest.UserId.Equals(userId) && foundTest.PasswordHash != null && !foundTest.PasswordHash.Equals(passwordHash))
            {
                model.Forbidden = true;
                return View(model);
            }
            model.Liked = unitOfWork.CheckUserLiked(id, userId);
            model.StarsCount = await unitOfWork.GetTestStarsCount(id);
            model.Test = foundTest;
            model.TotalAttempts = await unitOfWork.GetTestAttemptsCount(id);
            model.UserAttempts = unitOfWork.GetUserTestAttempt(id, userId);
            return View(model);
        }

        [HttpPost]
        [ActionName("Preview")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PreviewPost(int id, string passwordHash = "")
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View(null);
            }
            string userId = userManager.GetUserId(User);
            TestStatistics? foundTest = await unitOfWork.GetTestStatistics(id, userId);
            if (foundTest == null)
            {
                TestPriviewModel model = new TestPriviewModel();
                model.NotFound = true;
                return View(model);
            }
            if (!foundTest.Test.UserId.Equals(userId) && foundTest.Test.PasswordHash != null && !foundTest.Test.PasswordHash.Equals(passwordHash))
            {
                TestPriviewModel model = new TestPriviewModel();
                model.Forbidden = true;
                return View(model);
            }
            IEnumerable<Attempt> userAttempts = unitOfWork.GetUserTestAttempt(id, userId);
            if (foundTest.Test.MaxAttempts <= userAttempts.Count())
            {
                return RedirectToAction("Preview");
            }

            Attempt attempt = new Attempt()
            {
                UserId = userId,
                TestId = id,
                TimeStarted = DateTime.Now,
                Active = true
            };
            await unitOfWork.AttemptRepository.AddAsync(attempt);
            await unitOfWork.SaveAsync();
            List<Answer> answers = new List<Answer>();
            foreach (Question question in unitOfWork.GetTestQuestions(id, false))
            {
                Answer answer = new Answer();
                answer.QuestionId = question.Id;
                answer.AttemptId = attempt.Id;
                answers.Add(answer);
            }
            await unitOfWork.AnswerRepository.AddRangeAsync(answers);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Attempt", new { id = attempt.Id });
        }
        #endregion

        public IActionResult Search()
        {
            return View();
        }

        public async Task<IActionResult> List()
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View(null);
            }
            return View(await unitOfWork.GetPublicTestsStatistics(userManager.GetUserId(User)));
        }

        public async Task<IActionResult> MyTests()
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View(null);
            }
            return View(await unitOfWork.GetUserTestsStatistics(userManager.GetUserId(User)));
        }

        #region Ajax Methods
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StarToggle(int id, bool starred)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401); //unauthorised
            }
            Test? foundTest = unitOfWork.TestRepository.FirstOrDefault(x => x.Id == id);
            if (foundTest == null)
            {
                return StatusCode(404); //not found
            }
            if (foundTest.UserId.Equals(userManager.GetUserId(User)))
            {
                return StatusCode(403); //forbidden
            }
            if (!starred && unitOfWork.CheckUserLiked(id, userManager.GetUserId(User)))
            {
                return StatusCode(403); //forbidden
            }
            if (starred)
            {
                if (!unitOfWork.RemoveUserStar(id, userManager.GetUserId(User)))
                {
                    return StatusCode(404); //not found
                }
            }
            else
            {
                await unitOfWork.UserStarRepository.AddAsync(new UserStar() { TestId = id, UserId = userManager.GetUserId(User) });
            }
            await unitOfWork.SaveAsync();
            return Content(JsonConvert.SerializeObject(await unitOfWork.GetTestStarsCount(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OpenToggle(int id, bool opened)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401); //unauthorised
            }
            Test? foundTest = unitOfWork.TestRepository.FirstOrDefault(x => x.Id == id);
            if (foundTest == null)
            {
                return StatusCode(404); //not found
            }
            if (foundTest.UserId.Equals(userManager.GetUserId(User)))
            {
                foundTest.Opened = !opened;
                unitOfWork.TestRepository.Update(foundTest);
                await unitOfWork.SaveAsync();
                return Ok();
            }
            return StatusCode(403); //forbidden
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
            Test? foundTest = unitOfWork.TestRepository.FirstOrDefault(x => x.Id == id);
            if (foundTest == null)
            {
                return StatusCode(404);
            }
            if (foundTest.UserId.Equals(userManager.GetUserId(User)))
            {
                unitOfWork.RemoveTest(id);
                await unitOfWork.SaveAsync();
                return Ok();
            }
            return StatusCode(403);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchNoPassword(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
            Test? foundTest = unitOfWork.TestRepository.FirstOrDefault(x => x.Id == id);
            if (foundTest == null)
            {
                return StatusCode(404);
            }
            if (!foundTest.Opened)
            {
                return Content("Closed");
            }
            if (foundTest.PasswordHash == null || foundTest.UserId.Equals(userManager.GetUserId(User)))
            {
                return Content("Redirect");
            }
            return Content("NeedPassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchPassword(int id, string password)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
            Test? foundTest = unitOfWork.TestRepository.FirstOrDefault(x => x.Id == id);
            if (foundTest == null)
            {
                return StatusCode(404);
            }
            if (!foundTest.Opened)
            {
                return Content("Closed");
            }
            if (foundTest.PasswordHash == null)
            {
                return Content("RedirectOnlyId");
            }
            string hash = Hasher.HashPassword(password);
            if (foundTest.PasswordHash.Equals(hash))
            {
                return Content($"RedirectHash:{hash}");
            }
            return Content("WrongPassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAttemptAnswer(int attemptId, int questionId, object value)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
            Answer? foundAnswer = unitOfWork.AnswerRepository.FirstOrDefault(x => x.AttemptId == attemptId && x.QuestionId == questionId);
            Question? foundQuestion = unitOfWork.QuestionRepository.FirstOrDefault(x => x.Id == questionId);
            if (foundAnswer == null || foundQuestion == null)
            {
                return StatusCode(404);
            }
            if (value == null)
            {
                foundAnswer.DoubleAnswer = null;
                foundAnswer.TextAnswer = null;
                foundAnswer.IntAnswer = null;
                foundAnswer.RadioAnswer = null;
                foundAnswer.CheckAnswer = null;
                foundAnswer.DateAnswer = null;
            }
            else
            {
                switch (foundQuestion.Type)
                {
                    case QuestionType.Text:
                        break;
                    case QuestionType.Int:
                        break;
                    case QuestionType.Double:
                        break;
                    case QuestionType.Date:
                        break;
                    case QuestionType.Radio:
                        break;
                    case QuestionType.Check:
                        break;
                }
            }
            unitOfWork.AnswerRepository.Update(foundAnswer);
            await unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckActiveAttemps(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
            Test? foundTest = unitOfWork.TestRepository.FirstOrDefault(x => x.Id == id);
            if (foundTest == null)
            {
                return StatusCode(404);
            }
            List<Attempt> attemps = unitOfWork.GetUserTestAttempt(id,userManager.GetUserId(User)).ToList();
            if (attemps.Any(x=>x.Active))
            {
                return Content("Active");
            }
            return Content("Redirect");
        }
        #endregion
    }
}
