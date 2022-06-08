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
            if (attempt == null)
            {
                return View(null); //NotFound
            }
            if (!attempt.UserId.Equals(userManager.GetUserId(User)))
            {
                return View(null); //Forbidden
            }
            if (attempt.Active)
            {
                return RedirectToAction("Results", new { Id = attempt.Id });
            }
            Test? test = unitOfWork.GetTestById(attempt.TestId, false);
            if (test == null)
            {
                return View(null); //NotFound
            }
            AttemptModel attemptModel = new AttemptModel();
            attemptModel.Attempt = attempt;
            attemptModel.Test = test;
            return View(attemptModel); //Ok
        }

        #endregion

        #region Results

        public IActionResult Results(int id)
        {
            return null;
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
            if (foundTest.UserId.Equals(userId) && foundTest.PasswordHash != null && !foundTest.PasswordHash.Equals(passwordHash))
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
            Attempt attempt = new Attempt();
            attempt.UserId = userId;
            attempt.TestId = id;
            attempt.TimeStarted = DateTime.Now;
            attempt.Active = true;
            await unitOfWork.AttemptRepository.AddAsync(attempt);
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
            if (foundTest.PasswordHash == null)
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
        public IActionResult SaveAttemptAnswer(int attemptId, int questionId, object value)
        {
            return null;
        }
        #endregion
    }
}
