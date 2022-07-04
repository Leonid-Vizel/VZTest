using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            if (model.Questions.Count == 0 || model.Title.Length == 0 || model.MaxAttempts == 0)
            {
                return BadRequest();
            }
            if (!model.Validate())
            {
                return BadRequest();
            }
            Test test = new Test();
            test.Title = model.Title;
            test.Description = model.Description;
            test.CreatedTime = DateTime.Now;
            test.UserId = userManager.GetUserId(User);
            if (Uri.IsWellFormedUriString(model.ImageUrl, UriKind.RelativeOrAbsolute))
            {
                test.ImageUrl = model.ImageUrl;
            }
            test.Opened = false;
            test.Public = false;
            test.MaxAttempts = model.MaxAttempts;
            test.StartTime = model.StartTime;
            if (model.StartTime != null && model.EndTime != null && DateTime.Compare(model.StartTime.Value, model.EndTime.Value) < 0)
            {
                test.EndTime = model.EndTime;
            }
            test.Questions = new List<Question>();
            if (model.Password != null)
            {
                test.PasswordHash = Hasher.HashPassword(model.Password);
            }
            foreach (QuestionBlueprint blueptint in model.Questions)
            {
                Question? question = blueptint.ToQuestion(false);
                if (question != null)
                {
                    test.Questions.Add(question);
                }
            }
            if (test.Questions.Count == 0)
            {
                return BadRequest();
            }
            await unitOfWork.AddTest(test);
            await unitOfWork.SaveAsync();

            foreach (Question Question in test.Questions.Where(x => x.Type == QuestionType.Radio))
            {
                CorrectIntAnswer answer = Question.CorrectAnswer as CorrectIntAnswer;
                Question.CorrectAnswer = new CorrectIntAnswer(Question.Options[answer.Correct].Id);
                Question.CorrectAnswer.QuestionId = Question.Id;
                await unitOfWork.AddCorrectAnswerAsync(Question.CorrectAnswer);
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
                await unitOfWork.AddCorrectAnswerAsync(Question.CorrectAnswer);
            }
            await unitOfWork.SaveAsync();
            return Content(test.Id.ToString());
        }

        #endregion

        #region Edit

        public IActionResult Edit(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View(null);
            }
            Test? foundTest = unitOfWork.GetTestMainInfo(id);
            if (foundTest == null)
            {
                TestEditModel model = new TestEditModel();
                model.NotFound = true;
                return View(model);
            }
            if (!foundTest.UserId.Equals(userManager.GetUserId(User)))
            {
                TestEditModel model = new TestEditModel();
                model.Forbidden = true;
                return View(model);
            }
            if (foundTest.Opened)
            {
                TestEditModel model = new TestEditModel();
                model.Id = id;
                model.TestOpened = true;
                return View(model);
            }
            unitOfWork.FillTest(foundTest, true);
            return View(foundTest.ToEditModel(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TestEditModel model)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
            Test? foundTest = unitOfWork.GetTestMainInfo(model.Id);
            if (foundTest == null)
            {
                return StatusCode(404);
            }
            if (!foundTest.UserId.Equals(userManager.GetUserId(User)))
            {
                return StatusCode(403);
            }

            if (!model.Validate())
            {
                return BadRequest();
            }

            unitOfWork.FillTest(foundTest, true);

            #region Updating the test main info
            foundTest.Title = model.Title;
            foundTest.Description = model.Description;
            if (Uri.IsWellFormedUriString(model.ImageUrl, UriKind.RelativeOrAbsolute))
            {
                foundTest.ImageUrl = model.ImageUrl;
            }
            foundTest.MaxAttempts = model.MaxAttempts;
            if (model.Password == null)
            {
                foundTest.PasswordHash = null;
            }
            else
            {
                foundTest.PasswordHash = Hasher.HashPassword(model.Password);
            }
            foundTest.Shuffle = model.Shuffle;
            foundTest.EditedTime = DateTime.Now;
            #endregion

            unitOfWork.UpdateTest(foundTest);

            #region Cleaning the model
            model.Questions = model.Questions.Where(x => x.Id == 0 || foundTest.Questions.Any(q => q.Id == x.Id)).ToList();
            #endregion

            #region Adding new questions
            List<Question> newQuestions = model.Questions.Where(x => x.Id == 0).Select(x => x.ToQuestion(false)).Where(x => x != null).ToList();
            List<Question> newOptionQuestions = new List<Question>();
            foreach (Question? question in newQuestions)
            {
                question.TestId = model.Id;
                await unitOfWork.AddQuestionAsync(question);
            }
            await unitOfWork.SaveAsync();
            foreach (Question question in newQuestions)
            {
                switch (question.Type)
                {
                    case QuestionType.Text:
                    case QuestionType.Int:
                    case QuestionType.Double:
                    case QuestionType.Date:
                        question.CorrectAnswer.QuestionId = question.Id;
                        await unitOfWork.AddCorrectAnswerAsync(question.CorrectAnswer);
                        break;
                    case QuestionType.Check:
                    case QuestionType.Radio:
                        newOptionQuestions.Add(question);
                        foreach (Option option in question.Options)
                        {
                            option.QuestionId = question.Id;
                            await unitOfWork.AddOptionAsync(option);
                        }
                        break;
                }
            }
            await unitOfWork.SaveAsync();

            foreach (Question newQuestion in newOptionQuestions)
            {
                if (newQuestion.Type == QuestionType.Radio)
                {
                    CorrectIntAnswer answer = newQuestion.CorrectAnswer as CorrectIntAnswer;
                    newQuestion.CorrectAnswer = new CorrectIntAnswer(newQuestion.Options[answer.Correct].Id);
                }
                else
                {
                    CorrectCheckAnswer answer = newQuestion.CorrectAnswer as CorrectCheckAnswer;
                    int[] indexes = answer.Correct;
                    int[] answers = new int[indexes.Length];
                    for (int i = 0; i < indexes.Length; i++)
                    {
                        answers[i] = newQuestion.Options[indexes[i]].Id;
                    }
                    newQuestion.CorrectAnswer = new CorrectCheckAnswer(answers);
                }

                newQuestion.CorrectAnswer.QuestionId = newQuestion.Id;
                await unitOfWork.AddCorrectAnswerAsync(newQuestion.CorrectAnswer);
            }
            #endregion

            #region Deleting questions
            IEnumerable<int> Ids = model.Questions.Where(x => x.Id != 0).Select(x => x.Id);
            List<Question> questionsToDelete = foundTest.Questions.Where(x => !Ids.Any(y => y == x.Id)).ToList();
            foreach (Question question in questionsToDelete)
            {
                unitOfWork.RemoveQuestion(question);
                foundTest.Questions.Remove(question);
            }
            #endregion

            #region Updating the questions

            foreach (Question question in foundTest.Questions)
            {
                QuestionBlueprint? foundBlueprint = model.Questions.FirstOrDefault(x => x.Id == question.Id);
                if (foundBlueprint == null)
                {
                    continue;
                }
                Question? updQuestion = foundBlueprint.ToQuestion(true);
                if (updQuestion == null)
                {
                    continue;
                }
                question.Title = updQuestion.Title;
                question.Balls = updQuestion.Balls;
                question.ImageUrl = updQuestion.ImageUrl;

                switch (question.Type)
                {
                    case QuestionType.Text:
                    case QuestionType.Int:
                    case QuestionType.Double:
                    case QuestionType.Date:
                        switch (updQuestion.Type)
                        {
                            case QuestionType.Text:
                            case QuestionType.Int:
                            case QuestionType.Double:
                            case QuestionType.Date:
                                if (question.Type != updQuestion.Type || !updQuestion.CorrectAnswer.Equals(question.CorrectAnswer))
                                {
                                    if (question.CorrectAnswer != null)
                                    {
                                        unitOfWork.RemoveCorrectAnswer(question.CorrectAnswer);
                                    }
                                    await unitOfWork.AddCorrectAnswerAsync(updQuestion.CorrectAnswer);
                                }
                                break;
                            case QuestionType.Check:
                            case QuestionType.Radio:
                                foreach (Option option in updQuestion.Options)
                                {
                                    option.QuestionId = question.Id;
                                    question.Options.Add(option);
                                    await unitOfWork.AddOptionAsync(option);
                                }
                                await unitOfWork.SaveAsync();
                                if (updQuestion.Type == QuestionType.Radio)
                                {
                                    CorrectIntAnswer answer = updQuestion.CorrectAnswer as CorrectIntAnswer;
                                    updQuestion.CorrectAnswer = new CorrectIntAnswer(question.Options[answer.Correct].Id);
                                }
                                else
                                {
                                    CorrectCheckAnswer answer = updQuestion.CorrectAnswer as CorrectCheckAnswer;
                                    int[] indexes = answer.Correct;
                                    int[] answers = new int[indexes.Length];
                                    for (int i = 0; i < indexes.Length; i++)
                                    {
                                        answers[i] = question.Options[indexes[i]].Id;
                                    }
                                    updQuestion.CorrectAnswer = new CorrectCheckAnswer(answers);
                                }
                                updQuestion.CorrectAnswer.QuestionId = question.Id;
                                if (question.CorrectAnswer != null)
                                {
                                    unitOfWork.RemoveCorrectAnswer(question.CorrectAnswer);
                                }
                                await unitOfWork.AddCorrectAnswerAsync(updQuestion.CorrectAnswer);
                                break;
                        }
                        break;
                    case QuestionType.Check:
                    case QuestionType.Radio:
                        switch (updQuestion.Type)
                        {
                            case QuestionType.Text:
                            case QuestionType.Int:
                            case QuestionType.Double:
                            case QuestionType.Date:
                                foreach (Option option in question.Options)
                                {
                                    unitOfWork.RemoveOption(option);
                                }
                                if (question.CorrectAnswer != null)
                                {
                                    unitOfWork.RemoveCorrectAnswer(question.CorrectAnswer);
                                }
                                await unitOfWork.AddCorrectAnswerAsync(updQuestion.CorrectAnswer);
                                break;
                            case QuestionType.Check:
                            case QuestionType.Radio:
                                updQuestion.Options = updQuestion.Options.Where(x => x.Id == 0 || question.Options.Any(o => o.Id == x.Id)).ToList();
                                //New
                                foreach (Option option in updQuestion.Options.Where(x => x.Id == 0))
                                {
                                    question.Options.Add(option);
                                    option.QuestionId = updQuestion.Id;
                                    await unitOfWork.AddOptionAsync(option);
                                }
                                //Edit
                                foreach (Option option in updQuestion.Options.Where(x => x.Id != 0))
                                {
                                    Option? initialOption = question.Options.FirstOrDefault(x => x.Id == option.Id);
                                    if (initialOption == null)
                                    {
                                        continue;
                                    }
                                    initialOption.Title = option.Title;
                                    unitOfWork.UpdateOption(initialOption);
                                }
                                //Delete
                                List<Option> optionsToDelete = question.Options.Where(x => !updQuestion.Options.Any(o => o.Id == x.Id)).ToList();
                                foreach (Option option in optionsToDelete)
                                {
                                    question.Options.Remove(option);
                                    unitOfWork.RemoveOption(option);
                                }
                                await unitOfWork.SaveAsync();
                                #region Correct Reformatting
                                if (question.Type == QuestionType.Radio)
                                {
                                    CorrectIntAnswer answer = updQuestion.CorrectAnswer as CorrectIntAnswer;
                                    updQuestion.CorrectAnswer = new CorrectIntAnswer(question.Options[answer.Correct].Id);
                                }
                                else
                                {
                                    CorrectCheckAnswer answer = updQuestion.CorrectAnswer as CorrectCheckAnswer;
                                    int[] indexes = answer.Correct;
                                    int[] answers = new int[indexes.Length];
                                    for (int i = 0; i < indexes.Length; i++)
                                    {
                                        answers[i] = question.Options[indexes[i]].Id;
                                    }
                                    updQuestion.CorrectAnswer = new CorrectCheckAnswer(answers);
                                }
                                #endregion
                                if (question.Type != updQuestion.Type || !updQuestion.CorrectAnswer.Equals(question.CorrectAnswer))
                                {
                                    if (question.CorrectAnswer != null)
                                    {
                                        unitOfWork.RemoveCorrectAnswer(question.CorrectAnswer);
                                    }
                                    updQuestion.CorrectAnswer.QuestionId = question.Id;
                                    await unitOfWork.AddCorrectAnswerAsync(updQuestion.CorrectAnswer);
                                }
                                break;
                        }
                        break;
                }
                question.Type = updQuestion.Type;
                unitOfWork.UpdateQuestion(question);
            }

            #endregion

            await unitOfWork.SaveAsync();

            return Content(model.Id.ToString());
        }

        #endregion

        #region Attempt
        public IActionResult Attempt(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View(null);
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
            unitOfWork.UpdateAttempt(attempt);
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
            Test? foundTest = unitOfWork.GetTestMainInfo(id);
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
            model.Attempts = unitOfWork.GetTestAttempts(id, true).Where(x=>!x.Active);
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
            Test? foundTest = unitOfWork.GetTestMainInfo(id);
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
            await unitOfWork.AddAttemptAsync(attempt);
            await unitOfWork.SaveAsync();
            List<Answer> answers = new List<Answer>();
            foreach (Question question in questions)
            {
                Answer answer = new Answer();
                answer.QuestionId = question.Id;
                answer.AttemptId = attempt.Id;
                answers.Add(answer);
            }
            await unitOfWork.AddAnswerRangeAsync(answers);
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
            if (total % pageSize > 0)
            {
                total = total / pageSize + 1;
            }
            else
            {
                total = total / pageSize;
            }
            ViewBag.TotalPages = total;
            return View(await tests.ToPagedListAsync(page, pageSize));
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
            if (total % pageSize > 0)
            {
                total = total / pageSize + 1;
            }
            else
            {
                total = total / pageSize;
            }
            ViewBag.TotalPages = total;
            return View(await tests.ToPagedListAsync(page, pageSize));
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
            Test? foundTest = unitOfWork.GetTestMainInfo(id);
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
                await unitOfWork.AddUserStarAsync(new UserStar() { TestId = id, UserId = userManager.GetUserId(User) });
            }
            await unitOfWork.SaveAsync();
            return Content((await unitOfWork.GetTestStarsCount(id)).ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OpenToggle(int id, bool opened)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401); //unauthorised
            }
            Test? foundTest = unitOfWork.GetTestMainInfo(id);
            if (foundTest == null)
            {
                return StatusCode(404); //not found
            }
            if (foundTest.UserId.Equals(userManager.GetUserId(User)))
            {
                foundTest.Opened = !opened;
                unitOfWork.UpdateTest(foundTest);
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
            Test? foundTest = unitOfWork.GetTestMainInfo(id);
            if (foundTest == null)
            {
                return StatusCode(404); //not found
            }
            if (foundTest.UserId.Equals(userManager.GetUserId(User)))
            {
                foundTest.Public = !isPublic;
                unitOfWork.UpdateTest(foundTest);
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
            Test? foundTest = unitOfWork.GetTestMainInfo(id);
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
            Test? foundTest = unitOfWork.GetTestMainInfo(id);
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
            Test? foundTest = unitOfWork.GetTestMainInfo(id);
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
            Attempt? attempt = unitOfWork.GetAttemptMainInfo(attemptId);
            Answer? foundAnswer = unitOfWork.GetAnswer(attemptId, questionId);
            Question? foundQuestion = unitOfWork.GetQuestion(questionId, false);
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
                        if (!unitOfWork.OptionExists(questionId, radioResult))
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
                            if (!unitOfWork.OptionExists(questionId, optionIds[i]))
                            {
                                return StatusCode(404);
                            }
                        }
                        foundAnswer.CheckAnswers = optionIds;
                        break;
                }
            }
            unitOfWork.UpdateAnswer(foundAnswer);
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
            Test? foundTest = unitOfWork.GetTestMainInfo(id);
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
            Attempt? foundAttempt = unitOfWork.GetAttemptMainInfo(id);
            if (foundAttempt == null)
            {
                return StatusCode(404);
            }
            Test? foundTest = unitOfWork.GetTestMainInfo(foundAttempt.TestId);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetTestStatus(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
            Test? testAttempt = unitOfWork.GetTestMainInfo(id);
            if (testAttempt == null)
            {
                return StatusCode(404);
            }
            return Content(testAttempt.Opened.ToString());
        }
        #endregion
    }
}
