using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult List()
        {
            return View(unitOfWork.GetPublicTests());
        }

        public async Task<IActionResult> MyTests()
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View(null);
            }
            return View(await unitOfWork.GetUserTestsStatistics(userManager.GetUserId(User)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Open(int id)
        {
            Test? foundTest = unitOfWork.TestRepository.FirstOrDefault(x => x.Id == id);
            if (foundTest == null)
            {
                return RedirectToAction("MyTests");
            }
            foundTest.Opened = true;
            unitOfWork.TestRepository.Update(foundTest);
            await unitOfWork.Save();
            return RedirectToAction("MyTests");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Close(int id)
        {
            Test? foundTest = unitOfWork.TestRepository.FirstOrDefault(x => x.Id == id);
            if (foundTest == null)
            {
                return RedirectToAction("MyTests");
            }
            foundTest.Opened = false;
            unitOfWork.TestRepository.Update(foundTest);
            await unitOfWork.Save();
            return RedirectToAction("MyTests");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Test? foundTest = unitOfWork.TestRepository.FirstOrDefault(x => x.Id == id);
            if (foundTest == null)
            {
                return RedirectToAction("MyTests");
            }
            unitOfWork.TestRepository.Remove(foundTest);
            await unitOfWork.Save();
            return RedirectToAction("MyTests");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(int id)
        {
            return View();
        }
    }
}
