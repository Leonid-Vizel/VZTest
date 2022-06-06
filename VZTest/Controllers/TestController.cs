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

        public TestController(IUnitOfWork unitOfWork, SignInManager<IdentityUser> signInManager)
        {
            this.unitOfWork = unitOfWork;
            this.signInManager = signInManager;
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

        public IActionResult Attempt()
        {
            return View();
        }

        public IActionResult Preview()
        {
            return View();
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(int id)
        {
            return View();
        }
    }
}
