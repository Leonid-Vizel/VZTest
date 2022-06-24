using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;
using VZTest.Instruments;
using VZTest.Models;
using VZTest.Models.Test;
using VZTest.Models.Test.CorrectAnswers;
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
            Test test = new Test();
            test.Title = model.Title;
            test.Description = model.Description;
            test.CreatedTime = DateTime.Now;
            test.UserId = userManager.GetUserId(User);
            test.Opened = false;
            test.Public = false;
            test.MaxAttempts = model.MaxAttempts;
            test.Questions = new List<Question>();
            if (model.Password != null)
            {
                test.PasswordHash = Hasher.HashPassword(model.Password);
            }
            foreach (QuestionBlueprint blueptint in model.Questions)
            {
                Question? question = blueptint.ToQuestion();
                if (question != null)
                {
                    test.Questions.Add(question);
                }
            }
            await unitOfWork.AddTest(test);
            await unitOfWork.SaveAsync();

            foreach (Question Question in test.Questions.Where(x => x.Type == QuestionType.Radio))
            {
                CorrectIntAnswer answer = Question.CorrectAnswer as CorrectIntAnswer;
                Question.CorrectAnswer = new CorrectIntAnswer(Question.Options[answer.Correct].Id);
                Question.CorrectAnswer.QuestionId = Question.Id;
                await unitOfWork.CorrectAnswerRepository.AddAsync(Question.CorrectAnswer);
            }
            foreach (Question Question in test.Questions.Where(x => x.Type == QuestionType.Check))
            {
                CorrectCheckAnswer answer = Question.CorrectAnswer as CorrectCheckAnswer;
                int[] indexes = answer.Correct;
                int[] answers = new int[indexes.Length];
                for (int i = 0; i < indexes.Length; i++)
                {
                    answers[i] = Question.Options[indexes[i]].Id;
                }
                Question.CorrectAnswer = new CorrectCheckAnswer(answers);
                Question.CorrectAnswer.QuestionId = Question.Id;
                await unitOfWork.CorrectAnswerRepository.AddAsync(Question.CorrectAnswer);
            }
            await unitOfWork.SaveAsync();
            return Content(test.Id.ToString());
        }

        #endregion

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
                return RedirectToAction("Result", new { Id = attempt.Id });
            }
            Test? test = unitOfWork.GetTestById(attempt.TestId, false);
            if (test == null)
            {
                attemptModel.NotFound = true;
                return View(attemptModel);
            }
            attemptModel.Attempt = attempt;
            attemptModel.Test = test;
            attemptModel.AlignQuestions();
            return View(attemptModel); //Ok
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Attempt")]
        public async Task<IActionResult> AttemptPost(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View(null); //Authorize
            }
            Attempt? attempt = unitOfWork.GetAttemptWithAnswers(id);
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
                return RedirectToAction("Result", new { Id = attempt.Id });
            }
            Test? test = unitOfWork.GetTestById(attempt.TestId, true);
            if (test == null)
            {
                AttemptModel attemptModel = new AttemptModel();
                attemptModel.NotFound = true;
                return View(attemptModel);
            }
            attempt.Active = false;
            unitOfWork.CheckAttempt(attempt);
            unitOfWork.AttemptRepository.Update(attempt);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Result", new { Id = attempt.Id });
        }
        #endregion

        #region Result

        public IActionResult Result(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View(null); //Authorize
            }
            string userId = userManager.GetUserId(User);
            AttemptModel attemptModel = new AttemptModel();
            Attempt? foundAttempt = unitOfWork.GetAttemptWithAnswers(id);
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
            attemptModel.AlignQuestions();
            return View(attemptModel); //Ok
        }

        #endregion

        #region Results

        public IActionResult Results(int id)
        {
            TestResultsModel model = new TestResultsModel();
            if (!signInManager.IsSignedIn(User))
            {
                return View(null); //Authorize
            }
            string userId = userManager.GetUserId(User);
            Test? foundTest = unitOfWork.TestRepository.FirstOrDefault(x => x.Id == id);
            if (foundTest == null)
            {
                model.NotFound = true;
                return View(model);
            }
            if (!foundTest.UserId.Equals(userId))
            {
                model.Forbidden = true;
                return View(model);
            }
            model.Test = foundTest;
            model.Attempts = unitOfWork.GetTestAttempts(id, true);
            return View(model);
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
            model.UserAttempts = unitOfWork.GetUserTestAttempts(id, userId);
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
            IEnumerable<Attempt> userAttempts = unitOfWork.GetUserTestAttempts(id, userId);
            if (foundTest.Test.MaxAttempts <= userAttempts.Count())
            {
                return RedirectToAction("Preview");
            }
            List<Question> questions = unitOfWork.GetTestQuestions(id, false).ToList();
            if (foundTest.Test.Shuffle)
            {
                Shuffler.Shuffle(questions);
            }
            Attempt attempt = new Attempt()
            {
                UserId = userId,
                TestId = id,
                TimeStarted = DateTime.Now,
                Active = true,
                Sequence = string.Join('-', questions.Select(x => x.Id))
            };
            await unitOfWork.AttemptRepository.AddAsync(attempt);
            await unitOfWork.SaveAsync();
            List<Answer> answers = new List<Answer>();
            foreach (Question question in questions)
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
                return View(null);
            }
            if (page < 1)
            {
                page = 1;
            }
            IEnumerable<TestStatistics> tests = await unitOfWork.GetPublicTestsStatistics(userManager.GetUserId(User));
            int total = tests.Count();
            if (total % 6 > 0)
            {
                total = total / 6 + 1;
            }
            else
            {
                total = total / 6;
            }
            ViewBag.TotalPages = total;
            return View(tests.ToPagedList(page, 6));
        }
        #endregion

        #region MyTests
        public async Task<IActionResult> MyTests(int page = 1)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View(null);
            }
            if (page < 1)
            {
                page = 1;
            }
            IEnumerable<TestStatistics> tests = await unitOfWork.GetUserTestsStatistics(userManager.GetUserId(User));
            int total = tests.Count();
            if (total % 6 > 0)
            {
                total = total / 6 + 1;
            }
            else
            {
                total = total / 6;
            }
            ViewBag.TotalPages = total;
            return View(tests.ToPagedList(page, 6));
        }
        #endregion

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
        public async Task<IActionResult> PublicToggle(int id, bool isPublic)
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
                foundTest.Public = !isPublic;
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
        public async Task<IActionResult> SaveAttemptAnswer(int attemptId, int questionId, string value)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
            Attempt? attempt = unitOfWork.AttemptRepository.FirstOrDefault(x => x.Id == attemptId);
            Answer? foundAnswer = unitOfWork.AnswerRepository.FirstOrDefault(x => x.AttemptId == attemptId && x.QuestionId == questionId);
            Question? foundQuestion = unitOfWork.QuestionRepository.FirstOrDefault(x => x.Id == questionId);
            if (foundAnswer == null || foundQuestion == null || attempt == null)
            {
                return StatusCode(404);
            }
            if (!attempt.Active || !attempt.UserId.Equals(userManager.GetUserId(User)))
            {
                return StatusCode(403);
            }
            if (value == null || value == "")
            {
                foundAnswer.DoubleAnswer = null;
                foundAnswer.TextAnswer = "";
                foundAnswer.IntAnswer = null;
                foundAnswer.RadioAnswer = null;
                foundAnswer.CheckAnswers = new int[0];
                foundAnswer.DateAnswer = null;
            }
            else
            {
                switch (foundQuestion.Type)
                {
                    case QuestionType.Text:
                        foundAnswer.TextAnswer = value;
                        break;
                    case QuestionType.Int:
                        if (!int.TryParse(value, out int intResult))
                        {
                            return StatusCode(400);
                        }
                        foundAnswer.IntAnswer = intResult;
                        break;
                    case QuestionType.Double:
                        if (!double.TryParse(value, out double doubleResult))
                        {
                            return StatusCode(400);
                        }
                        foundAnswer.DoubleAnswer = doubleResult;
                        break;
                    case QuestionType.Date:
                        if (!DateTime.TryParse(value, out DateTime dateResult))
                        {
                            return StatusCode(400);
                        }
                        foundAnswer.DateAnswer = dateResult;
                        break;
                    case QuestionType.Radio:
                        if (!int.TryParse(value, out int radioResult))
                        {
                            return StatusCode(400);
                        }
                        Option? foundOption = unitOfWork.OptionRepository.FirstOrDefault(x => x.Id == radioResult && x.QuestionId == questionId);
                        if (foundOption == null)
                        {
                            return StatusCode(404);
                        }
                        foundAnswer.RadioAnswer = radioResult;
                        break;
                    case QuestionType.Check:
                        string[] splitted = value.Split(',');
                        int[] optionIds = new int[splitted.Length];
                        for (int i = 0; i < splitted.Length; i++)
                        {
                            if (!int.TryParse(splitted[i], out optionIds[i]))
                            {
                                return StatusCode(400);
                            }
                            Option? checkOption = unitOfWork.OptionRepository.FirstOrDefault(x => x.Id == optionIds[i] && x.QuestionId == questionId);
                            if (checkOption == null)
                            {
                                return StatusCode(404);
                            }
                        }
                        foundAnswer.CheckAnswers = optionIds;
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
            List<Attempt> attemps = unitOfWork.GetUserTestAttempts(id, userManager.GetUserId(User)).ToList();
            if (attemps.Any(x => x.Active))
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
            Attempt? foundAttempt = unitOfWork.AttemptRepository.FirstOrDefault(x => x.Id == id);
            if (foundAttempt == null)
            {
                return StatusCode(404);
            }
            Test? foundTest = unitOfWork.TestRepository.FirstOrDefault(x => x.Id == foundAttempt.TestId);
            if (foundTest == null)
            {
                return StatusCode(404);
            }
            if (!foundTest.UserId.Equals(userManager.GetUserId(User)))
            {
                return StatusCode(403);
            }
            unitOfWork.RemoveAttempt(foundAttempt);
            await unitOfWork.SaveAsync();
            return Ok();
        }
        #endregion
    }
}
