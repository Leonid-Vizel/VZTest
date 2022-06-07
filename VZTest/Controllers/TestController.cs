using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(object inProgress)
        {
            return View();
        }

        public IActionResult Recreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recreate(int Id)
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Test updateTest)
        {
            return View();
        }

        public IActionResult Attempt(int id)
        {
            return View(unitOfWork.GetTestById(id, false));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Attempt(object inProgress)
        {
            return View();
        }

        public IActionResult Preview(int id)
        {
            return View(unitOfWork.GetTestById(id,false));
        }

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
        public async Task<IActionResult> StarToggle(int id)
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
            if (!string.IsNullOrEmpty(foundTest.UserId) && foundTest.UserId.Equals(userManager.GetUserId(User)))
            {
                return StatusCode(403); //forbidden
            }
            await unitOfWork.UserStarRepository.AddAsync(new UserStar() { TestId = id, UserId = userManager.GetUserId(User)});
            return Ok();
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
            if (!string.IsNullOrEmpty(foundTest.UserId) && foundTest.UserId.Equals(userManager.GetUserId(User)))
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
            if (!string.IsNullOrEmpty(foundTest.UserId) && foundTest.UserId.Equals(userManager.GetUserId(User)))
            {
                unitOfWork.RemoveTest(id);
                await unitOfWork.SaveAsync();
                return Ok();
            }
            return StatusCode(403);
        }

        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(int id)
        {
            return View();
        }
    }
}
