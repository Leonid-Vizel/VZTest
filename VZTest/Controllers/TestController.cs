using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VZTest.Data.IRepository;
using VZTest.Models.DataModels.Test;
using VZTest.Models.Enumerations.Test;
using VZTest.Models.ViewModels.Test;
using X.PagedList;

namespace VZTest.Controllers
{
    public class TestController : Controller
    {
        private const int pageSize = 5;
        private IUnitOfWork unitOfWork;
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;

        public TestController(IUnitOfWork unitOfWork, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestCreateModel model)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
            Test? createdTest = await unitOfWork.CreateTest(model, userManager.GetUserId(User));
            if (createdTest == null)
            {
                return BadRequest();
            }
            return Content(createdTest.Id.ToString());
        }

        #endregion

        #region Edit

        public IActionResult Edit(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View();
            }
            return View(unitOfWork.GetTestEditModelById(id, userManager.GetUserId(User)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TestEditModel model)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
            Test? foundTest = unitOfWork.GetTestById(model.Id);
            if (foundTest == null)
            {
                return StatusCode(404);
            }
            if (!foundTest.UserId.Equals(userManager.GetUserId(User)))
            {
                return StatusCode(403);
            }
            TestModel? editedModel = await unitOfWork.EditTest(model);
            if (model == null)
            {
                return BadRequest();
            }
            return Content(model.Id.ToString());
        }

        #endregion

        #region Attempt
        public IActionResult Attempt(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View();
            }
            return View(unitOfWork.GetAttemptViewModelById(id, userManager.GetUserId(User)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Attempt")]
        public async Task<IActionResult> AttemptPost(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View();
            }
            AttemptViewModel model = unitOfWork.GetAttemptViewModelById(id, userManager.GetUserId(User));
            if (model.NotFound || model.Forbidden)
            {
                return View(model);
            }
            if (!model.Attempt.Active)
            {
                return RedirectToAction("Result", new { Id = id });
            }
            await unitOfWork.CheckAttempt(model.Attempt);
            return RedirectToAction("Result", new { Id = id });
        }
        #endregion

        #region Result
        public IActionResult Result(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View();
            }
            return View(unitOfWork.GetResultViewModelById(id, userManager.GetUserId(User)));
        }
        #endregion

        #region Results
        public IActionResult Results(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View();
            }
            return View(unitOfWork.GetTestResultsModel(id, userManager.GetUserId(User)));
        }
        #endregion

        #region Preview
        public async Task<IActionResult> Preview(int id, string passwordHash = "")
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View();
            }
            return View(await unitOfWork.GetTestPreviewModel(id, userManager.GetUserId(User), passwordHash));
        }

        [HttpPost]
        [ActionName("Preview")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PreviewPost(int id, string passwordHash = "")
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View();
            }
            string userId = userManager.GetUserId(User);
            TestPreviewModel model = await unitOfWork.GetTestPreviewModel(id, userId, passwordHash);
            if (model.Errored)
            {
                return View(model);
            }
            Attempt? attempt = await unitOfWork.CreateAttempt(model.Test, userId);
            if (attempt == null)
            {
                return RedirectToAction("Preview", new { id = id });
            }
            await unitOfWork.SaveAsync();
            return RedirectToAction("Attempt", new { id = attempt.Id });
        }
        #endregion

        #region Search
        public IActionResult Search()
        {
            return View();
        }
        #endregion

        #region List
        public async Task<IActionResult> List(int page = 1)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View();
            }
            if (page < 1)
            {
                page = 1;
            }
            IEnumerable<TestStatistics> tests = await unitOfWork.GetPublicTestStatistics(userManager.GetUserId(User));
            return View(await tests.ToPagedListAsync(page, pageSize));
        }
        #endregion

        #region MyTests
        public async Task<IActionResult> MyTests(int page = 1)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View();
            }
            if (page < 1)
            {
                page = 1;
            }
            IEnumerable<TestStatistics> tests = await unitOfWork.GetUserTestStatistics(userManager.GetUserId(User));
            return View(await tests.ToPagedListAsync(page, pageSize));
        }
        #endregion

        #region Ajax Methods
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StarToggle(int testId)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401); //unauthorised
            }
            Test? foundTest = unitOfWork.GetTestById(testId);
            if (foundTest == null)
            {
                return StatusCode(404); //not found
            }
            string userId = userManager.GetUserId(User);
            if (foundTest.UserId.Equals(userId))
            {
                return StatusCode(403); //forbidden
            }
            await unitOfWork.ToggleStarAsync(testId, userId);
            return Content((await unitOfWork.GetTestStarCount(testId)).ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OpenToggle(int id, bool opened)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401); //unauthorised
            }
            Test? foundTest = unitOfWork.GetTestById(id);
            if (foundTest == null)
            {
                return StatusCode(404); //not found
            }
            if (!foundTest.UserId.Equals(userManager.GetUserId(User)))
            {
                return StatusCode(403); //forbidden
            }
            foundTest.Opened = !opened;
            unitOfWork.TestRepository.Update(foundTest);
            await unitOfWork.SaveAsync();
            return Ok(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublicToggle(int id, bool isPublic)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401); //unauthorised
            }
            Test? foundTest = unitOfWork.GetTestById(id);
            if (foundTest == null)
            {
                return StatusCode(404); //not found
            }
            if (!foundTest.UserId.Equals(userManager.GetUserId(User)))
            {
                return StatusCode(403); //forbidden
            }
            foundTest.Public = !isPublic;
            unitOfWork.TestRepository.Update(foundTest);
            await unitOfWork.SaveAsync();
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
            return StatusCode(await unitOfWork.RemoveTest(id, userManager.GetUserId(User)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchNoPassword(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
            Test? foundTest = unitOfWork.GetTestById(id);
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
            Test? foundTest = unitOfWork.GetTestById(id);
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
        public async Task<IActionResult> UpdateAttemptAnswer(int attemptId, int questionId, string value)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
            return StatusCode(await unitOfWork.UpdateAttemptAnswer(attemptId,questionId,userManager.GetUserId(User), value));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckActiveAttempts(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
            Test? foundTest = unitOfWork.GetTestById(id);
            if (foundTest == null)
            {
                return StatusCode(404);
            }
            if (await unitOfWork.CheckAnyActiveTestAttempts(id, userManager.GetUserId(User)))
            {
                return Content("Active");
            }
            return Content("Redirect");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAttempt(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
            return StatusCode(await unitOfWork.RemoveAttempt(id, userManager.GetUserId(User)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetTestStatus(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
            Test? testAttempt = unitOfWork.GetTestById(id);
            if (testAttempt == null)
            {
                return StatusCode(404);
            }
            return Content(testAttempt.Opened.ToString());
        }
        #endregion
    }
}