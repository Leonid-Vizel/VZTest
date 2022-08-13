using VZTest.Data.IRepository;
using VZTest.Instruments;
using VZTest.Models.DataModels.Test;
using VZTest.Models.Enumerations.Test;
using VZTest.Models.ViewModels.Test;

namespace VZTest.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db;

        public IRepository<Answer> AnswerRepository { get; set; }
        public IRepository<Attempt> AttemptRepository { get; set; }
        public IRepository<Option> OptionRepository { get; set; }
        public IRepository<Question> QuestionRepository { get; set; }
        public IRepository<Test> TestRepository { get; set; }
        public IRepository<CorrectAnswer> CorrectAnswerRepository { get; set; }
        public IRepository<UserStar> UserStarRepository { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            this.db = db;
            AnswerRepository = new Repository<Answer>(db);
            AttemptRepository = new Repository<Attempt>(db);
            OptionRepository = new Repository<Option>(db);
            QuestionRepository = new Repository<Question>(db);
            TestRepository = new Repository<Test>(db);
            CorrectAnswerRepository = new Repository<CorrectAnswer>(db);
            UserStarRepository = new Repository<UserStar>(db);
        }

        #region Test
        public Test? GetTestById(int id)
        {
            return TestRepository.FirstOrDefault(x => x.Id == id);
        }

        public TestModel? GetTestModelById(int id)
        {
            Test? foundTest = GetTestById(id);
            if (foundTest == null)
            {
                return null;
            }
            return new TestModel(foundTest, GetTestQuestionModels(id));
        }

        public TestModel? GetTestModelFromTest(Test test)
        {
            return new TestModel(test, GetTestQuestionModels(test.Id));
        }

        public double GetTestTotalBalls(int id)
        {
            return QuestionRepository.GetWhere(x => x.TestId == id).Sum(x => x.Balls);
        }

        public async Task<TestPreviewModel> GetTestPreviewModel(int id, string userId, string? hash)
        {
            TestPreviewModel model = new TestPreviewModel();
            Test? foundTest = GetTestById(id);
            if (foundTest == null)
            {
                model.NotFound = true;
                return model;
            }
            if (!foundTest.UserId.Equals(userId) && foundTest.PasswordHash != null && !foundTest.PasswordHash.Equals(hash))
            {
                model.Forbidden = true;
                return model;
            }
            model.Test = foundTest;
            model.BeforeStart = (foundTest.StartTime != null && DateTime.Compare(foundTest.StartTime.Value, DateTime.Now) > 0);
            model.AfterEnd = (foundTest.EndTime != null && DateTime.Compare(foundTest.EndTime.Value, DateTime.Now) < 0);
            model.Closed = !foundTest.Opened;
            model.TotalAttempts = await GetTestTotalAttemptsCount(id);
            model.Liked = CheckTestStarred(id, userId);
            model.UserAttempts = GetTestUserAttemptModels(id, userId);
            model.StarsCount = await GetTestStarCount(id);
            model.MaxBalls = GetTestTotalBalls(id);
            return model;
        }

        public TestResultsModel GetTestResultsModel(int id, string userId)
        {
            TestResultsModel model = new TestResultsModel();
            Test? foundTest = GetTestById(id);
            if (foundTest == null)
            {
                model.NotFound = true;
                return model;
            }
            if (!foundTest.UserId.Equals(userId))
            {
                model.Forbidden = true;
                return model;
            }
            model.Attempts = GetTestTotalAttemptModels(id);
            model.Test = foundTest;
            model.MaxBalls = GetTestTotalBalls(id);
            return model;
        }

        public async Task<TestStatistics?> GetTestStatistics(int id, string userId)
        {
            Test? foundTest = GetTestById(id);
            if (foundTest == null)
            {
                return null;
            }
            TestStatistics statistics = new TestStatistics();
            statistics.Test = foundTest;
            statistics.AttemptsCount = await GetTestTotalAttemptsCount(id);
            statistics.QuestionCount = await GetTestQuestionCount(id);
            statistics.StarsCount = await GetTestStarCount(id);
            statistics.CurrectUserStarred = CheckTestStarred(id, userId);
            return statistics;
        }

        public async Task<List<TestStatistics>> GetPublicTestStatistics(string userId)
        {
            List<int> testIds = TestRepository.GetWhere(x => x.Public && x.Opened).Select(x => x.Id).ToList();
            List<TestStatistics> statistics = new List<TestStatistics>();
            foreach (int testId in testIds)
            {
                TestStatistics? testStatistics = await GetTestStatistics(testId, userId);
                if (testStatistics != null)
                {
                    statistics.Add(testStatistics);
                }
            }
            return statistics;
        }

        public async Task<List<TestStatistics>> GetUserTestStatistics(string userId)
        {
            List<int> testIds = TestRepository.GetWhere(x => x.UserId.Equals(userId)).Select(x => x.Id).ToList();
            List<TestStatistics> statistics = new List<TestStatistics>();
            foreach (int testId in testIds)
            {
                TestStatistics? testStatistics = await GetTestStatistics(testId, userId);
                if (testStatistics != null)
                {
                    statistics.Add(testStatistics);
                }
            }
            return statistics;
        }

        public TestEditModel GetTestEditModelById(int id, string userId)
        {
            TestEditModel model = new TestEditModel();
            TestModel? foundTest = GetTestModelById(id);
            if (foundTest == null)
            {
                model.NotFound = true;
                return model;
            }
            if (!foundTest.UserId.Equals(userId))
            {
                model.Forbidden = true;
                return model;
            }
            model.Id = id;
            model.Title = foundTest.Title;
            model.Description = foundTest.Description;
            model.ImageUrl = foundTest.ImageUrl;
            model.MaxAttempts = foundTest.MaxAttempts;
            model.Shuffle = foundTest.Shuffle;
            model.StartTime = foundTest.StartTime;
            model.EndTime = foundTest.EndTime;
            model.Questions = foundTest.Questions.Select(x => x.ToBlueprint()).ToList();
            return model;
        }

        public async Task<Test?> CreateTest(TestCreateModel model, string userId)
        {
            #region Примитивная проверка валидности модели
            if (model.Questions.Count == 0 || model.Title.Length == 0 || model.MaxAttempts <= 0 || !model.Validate())
            {
                return null;
            }
            #endregion
            #region Создание и сохранение теста
            Test test = new Test();
            test.Title = model.Title;
            test.Description = model.Description;
            test.CreatedTime = DateTime.Now;
            test.UserId = userId;
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
            if (model.Password != null)
            {
                test.PasswordHash = Hasher.HashPassword(model.Password);
            }
            await TestRepository.AddAsync(test);
            await SaveAsync(); //Чтобы получить Id теста
            #endregion
            #region Трансформация и сохранение вопросов
            List<QuestionModel> questions = new List<QuestionModel>();
            foreach (QuestionBlueprint blueprint in model.Questions)
            {
                QuestionModel? transformedQuestion = GetQuestionModelFromBlueprint(blueprint);
                if (transformedQuestion != null)
                {
                    transformedQuestion.TestId = test.Id;
                    questions.Add(transformedQuestion);
                }
            }
            await QuestionRepository.AddRangeAsync(questions);
            await SaveAsync(); //Чтобы получить Id вопросов
            #endregion
            #region Создание и сохранение опций + ответов, кроме Radio и Check
            foreach (QuestionModel questionModel in questions)
            {
                foreach (Option option in questionModel.Options)
                {
                    option.QuestionId = questionModel.Id;
                }
                questionModel.CorrectAnswer.QuestionId = questionModel.Id;
                if (questionModel.Type != QuestionType.Radio && questionModel.Type != QuestionType.Check)
                {
                    await CorrectAnswerRepository.AddAsync(questionModel.CorrectAnswer);
                }
                await OptionRepository.AddRangeAsync(questionModel.Options);
            }
            await SaveAsync(); //Чтобы получить Id опций
            #endregion
            #region Сохранение ответов к Radio и Check
            int saveCount = 0;
            foreach (QuestionModel questionModel in questions.Where(x => x.Type == QuestionType.Radio))
            {
                CorrectAnswer answer = questionModel.CorrectAnswer;
                answer.IntAnswer = questionModel.Options[answer.IntAnswer.Value].Id;
                await CorrectAnswerRepository.AddAsync(answer);
                saveCount++;
            }
            foreach (QuestionModel questionModel in questions.Where(x => x.Type == QuestionType.Check))
            {
                CorrectAnswer answer = questionModel.CorrectAnswer;
                int[] indexes = ArrayTransformer.ToIntArray(answer.TextAnswer);
                int[] answers = new int[indexes.Length];
                for (int i = 0; i < indexes.Length; i++)
                {
                    answers[i] = questionModel.Options[indexes[i]].Id;
                }
                answer.TextAnswer = string.Join('-', answers);
                await CorrectAnswerRepository.AddAsync(answer);
                saveCount++;
            }
            if (saveCount > 0)
            {
                await SaveAsync();
            }
            #endregion
            return test;
        }

        public async Task<int> RemoveTest(int id, string userId)
        {
            TestModel? foundModel = GetTestModelById(id);
            if (foundModel == null)
            {
                return 404;
            }
            if (!foundModel.UserId.Equals(userId))
            {
                return 403;
            }
            foreach (QuestionModel questionModel in foundModel.Questions)
            {
                if (questionModel.Type == QuestionType.Radio || questionModel.Type == QuestionType.Check)
                {
                    OptionRepository.RemoveRange(questionModel.Options);
                }
                CorrectAnswerRepository.Remove(questionModel.CorrectAnswer);
            }
            QuestionRepository.RemoveRange(foundModel.Questions);
            TestRepository.Remove(foundModel);
            await SaveAsync();
            return 200;
        }

        public async Task<TestModel?> EditTest(TestEditModel model)
        {
            if (!model.Validate())
            {
                return null;
            }
            TestModel? foundTest = GetTestModelById(model.Id);
            if (foundTest == null)
            {
                return null;
            }
            #region Updating the test main info
            foundTest.Title = model.Title;
            foundTest.Description = model.Description;
            if (Uri.IsWellFormedUriString(model.ImageUrl, UriKind.RelativeOrAbsolute))
            {
                foundTest.ImageUrl = model.ImageUrl;
            }
            else if (model.ImageUrl == null || model.ImageUrl.Equals(""))
            {
                foundTest.ImageUrl = null;
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
            foundTest.EndTime = model.EndTime;
            foundTest.StartTime = model.StartTime;
            #endregion
            TestRepository.Update(foundTest);
            #region Убираю все попытки отредактировать вопросы из других тестов
            model.Questions = model.Questions.Where(x => x.Id == 0 || foundTest.Questions.Any(q => q.Id == x.Id)).ToList();
            #endregion
            #region Adding new questions
            List<QuestionModel> newQuestions = model.Questions.Where(x => x.Id == 0).Select(x => GetQuestionModelFromBlueprint(x,false)).Where(x => x != null).ToList();
            List<QuestionModel> newOptionQuestions = new List<QuestionModel>();
            foreach (QuestionModel? question in newQuestions)
            {
                question.TestId = model.Id;
                await QuestionRepository.AddAsync(question);
            }
            await SaveAsync();
            foreach (QuestionModel question in newQuestions)
            {
                switch (question.Type)
                {
                    case QuestionType.Text:
                    case QuestionType.Int:
                    case QuestionType.Double:
                    case QuestionType.Date:
                        question.CorrectAnswer.QuestionId = question.Id;
                        await CorrectAnswerRepository.AddAsync(question.CorrectAnswer);
                        break;
                    case QuestionType.Check:
                    case QuestionType.Radio:
                        newOptionQuestions.Add(question);
                        foreach (Option option in question.Options)
                        {
                            option.QuestionId = question.Id;
                            await OptionRepository.AddAsync(option);
                        }
                        break;
                }
            }
            await SaveAsync();

            foreach (QuestionModel newQuestion in newOptionQuestions)
            {
                if (newQuestion.Type == QuestionType.Radio)
                {
                    newQuestion.CorrectAnswer.IntAnswer = newQuestion.Options[newQuestion.CorrectAnswer.IntAnswer.Value].Id;
                }
                else
                {
                    int[] indexes = ArrayTransformer.ToIntArray(newQuestion.CorrectAnswer.TextAnswer);
                    int[] answers = new int[indexes.Length];
                    for (int i = 0; i < indexes.Length; i++)
                    {
                        answers[i] = newQuestion.Options[indexes[i]].Id;
                    }
                    Array.Sort(answers);
                    newQuestion.CorrectAnswer.TextAnswer = string.Join('-', answers);
                }

                newQuestion.CorrectAnswer.QuestionId = newQuestion.Id;
                await CorrectAnswerRepository.AddAsync(newQuestion.CorrectAnswer);
            }
            #endregion
            #region Deleting questions
            IEnumerable<int> Ids = model.Questions.Where(x => x.Id != 0).Select(x => x.Id);
            List<QuestionModel> questionsToDelete = foundTest.Questions.Where(x => !Ids.Any(y => y == x.Id)).ToList();
            foreach (QuestionModel question in questionsToDelete)
            {
                OptionRepository.RemoveRange(question.Options);
                CorrectAnswerRepository.Remove(question.CorrectAnswer);
                QuestionRepository.Remove(question);
                foundTest.Questions.Remove(question);
            }
            #endregion
            #region Updating the questions

            foreach (QuestionModel question in foundTest.Questions)
            {
                QuestionBlueprint? foundBlueprint = model.Questions.FirstOrDefault(x => x.Id == question.Id);
                if (foundBlueprint == null)
                {
                    continue;
                }
                QuestionModel? updQuestion = GetQuestionModelFromBlueprint(foundBlueprint, true);
                if (updQuestion == null)
                {
                    continue;
                }
                question.Title = updQuestion.Title;
                question.Balls = updQuestion.Balls;
                if (Uri.IsWellFormedUriString(updQuestion.ImageUrl, UriKind.RelativeOrAbsolute))
                {
                    question.ImageUrl = updQuestion.ImageUrl;
                }
                else if (updQuestion.ImageUrl == null || updQuestion.ImageUrl.Equals(""))
                {
                    question.ImageUrl = null;
                }

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
                                    question.CorrectAnswer.Update(updQuestion.CorrectAnswer);
                                    CorrectAnswerRepository.Update(question.CorrectAnswer);
                                }
                                break;
                            case QuestionType.Check:
                            case QuestionType.Radio:
                                foreach (Option option in updQuestion.Options)
                                {
                                    option.QuestionId = question.Id;
                                    question.Options.Add(option);
                                    await OptionRepository.AddAsync(option);
                                }
                                await SaveAsync();
                                question.CorrectAnswer.Flush();
                                if (updQuestion.Type == QuestionType.Radio)
                                {
                                    question.CorrectAnswer.IntAnswer = question.Options[updQuestion.CorrectAnswer.IntAnswer.Value].Id;
                                }
                                else
                                {
                                    int[] indexes = ArrayTransformer.ToIntArray(updQuestion.CorrectAnswer.TextAnswer);
                                    int[] answers = new int[indexes.Length];
                                    for (int i = 0; i < indexes.Length; i++)
                                    {
                                        answers[i] = question.Options[indexes[i]].Id;
                                    }
                                    Array.Sort(answers);
                                    question.CorrectAnswer.TextAnswer = string.Join('-', answers);
                                }
                                CorrectAnswerRepository.Update(question.CorrectAnswer);
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
                                OptionRepository.RemoveRange(question.Options);
                                question.CorrectAnswer.Update(updQuestion.CorrectAnswer);
                                CorrectAnswerRepository.Update(question.CorrectAnswer);
                                break;
                            case QuestionType.Check:
                            case QuestionType.Radio:
                                updQuestion.Options = updQuestion.Options.Where(x => x.Id == 0 || question.Options.Any(o => o.Id == x.Id)).ToList();
                                #region New Options
                                foreach (Option option in updQuestion.Options.Where(x => x.Id == 0))
                                {
                                    question.Options.Add(option);
                                    option.QuestionId = updQuestion.Id;
                                    await OptionRepository.AddAsync(option);
                                }
                                #endregion
                                #region Edited Options
                                foreach (Option option in updQuestion.Options.Where(x => x.Id != 0))
                                {
                                    Option? initialOption = question.Options.FirstOrDefault(x => x.Id == option.Id);
                                    if (initialOption == null)
                                    {
                                        continue;
                                    }
                                    initialOption.Title = option.Title;
                                    OptionRepository.Update(initialOption);
                                }
                                #endregion
                                #region Deleted Options
                                List<Option> optionsToDelete = question.Options.Where(x => !updQuestion.Options.Any(o => o.Id == x.Id)).ToList();
                                foreach (Option option in optionsToDelete)
                                {
                                    question.Options.Remove(option);
                                    OptionRepository.Remove(option);
                                }
                                await SaveAsync();
                                #endregion
                                #region Correct Reformatting
                                if (updQuestion.Type == QuestionType.Radio)
                                {
                                    question.CorrectAnswer.IntAnswer = question.Options[updQuestion.CorrectAnswer.IntAnswer.Value].Id;
                                }
                                else
                                {
                                    int[] indexes = ArrayTransformer.ToIntArray(updQuestion.CorrectAnswer.TextAnswer);
                                    int[] answers = new int[indexes.Length];
                                    for (int i = 0; i < indexes.Length; i++)
                                    {
                                        answers[i] = question.Options[indexes[i]].Id;
                                    }
                                    Array.Sort(answers);
                                    question.CorrectAnswer.TextAnswer = string.Join('-', answers);
                                }
                                #endregion
                                CorrectAnswerRepository.Update(question.CorrectAnswer);
                                break;
                        }
                        break;
                }
                question.Type = updQuestion.Type;
                QuestionRepository.Update(question);
            }
            await SaveAsync();
            #endregion
            return foundTest;
        }
        #endregion

        #region Question
        public Question? GetQuestionById(int id)
        {
            return QuestionRepository.FirstOrDefault(x => x.Id == id);
        }

        public QuestionModel? GetQuestionModelById(int id)
        {
            Question? foundQuestion = GetQuestionById(id);
            if (foundQuestion == null)
            {
                return null;
            }
            CorrectAnswer? foundAnswer = GetQuestionCorrectAnswer(id);
            if (foundAnswer == null)
            {
                return null;
            }
            return new QuestionModel(foundQuestion, GetQuestionOptions(id), foundAnswer);
        }

        public List<QuestionModel> GetTestQuestionModels(int testId)
        {
            List<int> testQuestions = GetTestQuestionIds(testId);
            List<QuestionModel> models = new List<QuestionModel>();
            foreach (int questionId in testQuestions)
            {
                QuestionModel? foundModel = GetQuestionModelById(questionId);
                if (foundModel != null)
                {
                    models.Add(foundModel);
                }
            }
            return models;
        }

        public async Task<int> GetTestQuestionCount(int testId)
        {
            return await QuestionRepository.CountAsync(x => x.TestId == testId);
        }

        public List<int> GetTestQuestionIds(int testId)
        {
            return QuestionRepository.GetWhere(x => x.TestId == testId).Select(x => x.Id).ToList();
        }

        public QuestionModel? GetQuestionModelFromBlueprint(QuestionBlueprint blueprint, bool transferId = false)
        {
            QuestionModel model = new QuestionModel();
            if (blueprint.Correct == null || blueprint.Correct.Length == 0)
            {
                return null;
            }
            if ((blueprint.OptionIds != null && blueprint.Options == null) || (blueprint.OptionIds == null && blueprint.Options != null)
            || (blueprint.OptionIds != null && blueprint.Options != null && blueprint.OptionIds.Count != blueprint.Options.Count))
            {
                return null;
            }
            if ((blueprint.Type == QuestionType.Radio || blueprint.Type == QuestionType.Check) && (blueprint.OptionIds == null || blueprint.OptionIds.Count == 0 || blueprint.Options == null || blueprint.Options.Count == 0))
            {
                return null;
            }
            QuestionModel question = new QuestionModel();
            if (transferId)
            {
                question.Id = blueprint.Id;
            }
            question.Title = blueprint.Title;
            question.Type = blueprint.Type;
            if (Uri.IsWellFormedUriString(blueprint.ImageUrl, UriKind.RelativeOrAbsolute))
            {
                question.ImageUrl = blueprint.ImageUrl;
            }
            question.Balls = blueprint.Balls;
            question.Options = new List<Option>();
            if (blueprint.Type == QuestionType.Check || blueprint.Type == QuestionType.Radio)
            {
                foreach (string optionName in blueprint.Options)
                {
                    Option option = new Option()
                    {
                        QuestionId = blueprint.Id,
                        Title = optionName
                    };
                    question.Options.Add(option);
                }
            }
            switch (question.Type)
            {
                case QuestionType.Text:
                    question.CorrectAnswer = new CorrectAnswer(blueprint.Correct);
                    break;
                case QuestionType.Int:
                    if (!int.TryParse(blueprint.Correct, out int intResult))
                    {
                        return null;
                    }
                    question.CorrectAnswer = new CorrectAnswer(intResult);
                    break;
                case QuestionType.Double:
                    if (!double.TryParse(blueprint.Correct, out double doubleResult))
                    {
                        return null;
                    }
                    question.CorrectAnswer = new CorrectAnswer(doubleResult);
                    break;
                case QuestionType.Date:
                    if (!DateTime.TryParse(blueprint.Correct, out DateTime dateResult))
                    {
                        return null;
                    }
                    question.CorrectAnswer = new CorrectAnswer(dateResult);
                    break;
                case QuestionType.Check:
                    string[] splitIds = blueprint.Correct.Split(',');
                    if (splitIds.Length == 0)
                    {
                        return null;
                    }
                    for (int i = 0; i < splitIds.Length; i++)
                    {
                        if (!int.TryParse(splitIds[i], out int checkId) || blueprint.Options.Count <= checkId || checkId < 0)
                        {
                            return null;
                        }
                    }
                    question.CorrectAnswer = new CorrectAnswer(blueprint.Correct);
                    break;
                case QuestionType.Radio:
                    if (!int.TryParse(blueprint.Correct, out int radioResult) || blueprint.Options.Count <= radioResult || radioResult < 0)
                    {
                        return null;
                    }
                    question.CorrectAnswer = new CorrectAnswer(radioResult);
                    break;
            }
            if (transferId)
            {
                question.CorrectAnswer.QuestionId = question.Id;
            }
            return question;
        }
        #endregion

        #region Option
        public List<Option> GetQuestionOptions(int questionId)
        {
            return OptionRepository.GetWhere(x => x.QuestionId == questionId).ToList();
        }

        public async Task<bool> OptionExists(int questionId, int optionId)
        {
            return await OptionRepository.CountAsync(x => x.Id == optionId && x.QuestionId == questionId) != 0;
        }
        #endregion

        #region Attempt
        public Attempt? GetAttemptById(int id)
        {
            return AttemptRepository.FirstOrDefault(x => x.Id == id);
        }

        public AttemptModel GetAttemptModel(Attempt attempt)
        {
            return new AttemptModel(attempt, GetAttemptAnswers(attempt.Id));
        }

        public AttemptModel? GetAttemptModelById(int id)
        {
            Attempt? foundAttempt = GetAttemptById(id);
            if (foundAttempt == null)
            {
                return null;
            }
            return new AttemptModel(foundAttempt, GetAttemptAnswers(id));
        }

        public async Task<int> GetTestTotalAttemptsCount(int testId)
        {
            return await AttemptRepository.CountAsync(x => x.TestId == testId);
        }

        public List<AttemptModel> GetTestUserAttemptModels(int testId, string userId)
        {
            return AttemptRepository.GetWhere(x => x.TestId == testId && x.UserId.Equals(userId)).Select(x => GetAttemptModel(x)).ToList();
        }

        public List<Attempt> GetUserTotalAttempts(string userId)
        {
            return AttemptRepository.GetWhere(x => x.UserId.Equals(userId)).ToList();
        }

        public List<Attempt> GetTestTotalAttempts(int testId)
        {
            return AttemptRepository.GetWhere(x => x.TestId == testId).ToList();
        }

        public List<AttemptModel> GetTestTotalAttemptModels(int testId)
        {
            return GetTestTotalAttempts(testId).Select(x => GetAttemptModel(x)).ToList();
        }

        public List<AttemptModel> GetTestActiveAttemptModels(int testId)
        {
            return GetTestTotalAttempts(testId).Where(x=>x.Active).Select(x => GetAttemptModel(x)).ToList();
        }

        public AttemptViewModel GetAttemptViewModelById(int id, string userId)
        {
            AttemptViewModel model = new AttemptViewModel();
            AttemptModel? foundAttempt = GetAttemptModelById(id);
            if (foundAttempt == null)
            {
                model.NotFound = true;
                return model;
            }
            TestModel? foundTest = GetTestModelById(foundAttempt.TestId);
            if (foundTest == null)
            {
                model.NotFound = true;
                return model;
            }
            if (!foundAttempt.UserId.Equals(userId) && !foundTest.UserId.Equals(userId))
            {
                model.Forbidden = true;
                return model;
            }
            model.Attempt = foundAttempt;
            model.Test = foundTest;
            model.AlignQuestions();
            return model;
        }

        public ResultViewModel GetResultViewModelById(int id, string userId)
        {
            ResultViewModel model = new ResultViewModel(GetAttemptViewModelById(id, userId));
            if (!model.Forbidden && !model.NotFound)
            {
                model.MaxBalls = GetTestTotalBalls(model.Test.Id);
            }
            return model;
        }

        public async Task<int> GetTestUserAttemptsCount(int testId, string userId)
        {
            return await AttemptRepository.CountAsync(x => x.TestId == testId && x.UserId.Equals(userId));
        }

        public async Task<Attempt?> CreateAttempt(Test test, string userId)
        {
            if (await GetTestUserAttemptsCount(test.Id, userId) >= test.MaxAttempts)
            {
                return null;
            }
            List<int> questionIds = GetTestQuestionIds(test.Id);
            if (test.Shuffle)
            {
                Shuffler.Shuffle(questionIds);
            }
            Attempt attempt = new Attempt()
            {
                UserId = userId,
                TestId = test.Id,
                TimeStarted = DateTime.Now,
                Active = true,
                Sequence = string.Join('-', questionIds)
            };
            await AttemptRepository.AddAsync(attempt);
            await SaveAsync();
            List<Answer> answers = new List<Answer>();
            questionIds.ForEach(x => answers.Add(new Answer()
            {
                QuestionId = x,
                AttemptId = attempt.Id
            }));
            await AnswerRepository.AddRangeAsync(answers);
            await SaveAsync();
            return attempt;
        }

        public async Task CheckAttempt(AttemptModel model, bool save = true)
        {
            if (model == null)
            {
                return;
            }
            foreach (Answer answer in model.Answers)
            {
                answer.Balls = CheckAnswer(answer);
            }
            model.Active = false;
            AnswerRepository.UpdateRange(model.Answers);
            AttemptRepository.Update(model);
            if (save)
            {
                await SaveAsync();
            }
        }

        public async Task<int> RemoveAttempt(int id, string userId)
        {
            AttemptModel? foundAttempt = GetAttemptModelById(id);
            if (foundAttempt == null)
            {
                return 404;
            }
            Test? foundTest = GetTestById(foundAttempt.TestId);
            if (foundTest == null)
            {
                return 404;
            }
            if (!foundTest.UserId.Equals(userId))
            {
                return 403;
            }
            AnswerRepository.RemoveRange(foundAttempt.Answers);
            AttemptRepository.Remove(foundAttempt);
            await SaveAsync();
            return 200;
        }

        public async Task<bool> CheckAnyActiveTestAttempts(int testId, string userId)
        {
            return await AttemptRepository.CountAsync(x => x.TestId == testId && x.UserId.Equals(userId) && x.Active) != 0;
        }
        #endregion

        #region Answer
        public Answer? GetAnswerById(int id)
        {
            return AnswerRepository.FirstOrDefault(x => x.Id == id);
        }

        public Answer? GetAnswerByAttemptAndQuestion(int attemptId, int questionId)
        {
            return AnswerRepository.FirstOrDefault(x => x.AttemptId == attemptId && x.QuestionId == questionId);
        }

        public List<Answer> GetAttemptAnswers(int attemptId)
        {
            return AnswerRepository.GetWhere(x => x.AttemptId == attemptId).ToList();
        }

        public double CheckAnswer(Answer answer)
        {
            Attempt? foundAttempt = GetAttemptById(answer.AttemptId);
            Question? foundQuestion = GetQuestionById(answer.AttemptId);
            if (foundAttempt == null || foundQuestion == null)
            {
                return 0;
            }
            CorrectAnswer? foundAnswer = GetQuestionCorrectAnswer(foundQuestion.Id);
            if (foundAnswer == null)
            {
                return 0;
            }
            switch (foundQuestion.Type)
            {
                case QuestionType.Text:
                    return (answer.TextAnswer != null && foundAnswer.TextAnswer != null && answer.TextAnswer.Equals(foundAnswer.TextAnswer)) ? foundQuestion.Balls : 0;
                case QuestionType.Double:
                    return (answer.DoubleAnswer != null && foundAnswer.DoubleAnswer != null && answer.DoubleAnswer == foundAnswer.DoubleAnswer) ? foundQuestion.Balls : 0;
                case QuestionType.Int:
                case QuestionType.Radio:
                    return (answer.IntAnswer != null && foundAnswer.IntAnswer != null && answer.IntAnswer == foundAnswer.IntAnswer) ? foundQuestion.Balls : 0;
                case QuestionType.Date:
                    return (answer.DateAnswer != null && foundAnswer.DateAnswer != null
                        && answer.DateAnswer.Value.Day == foundAnswer.DateAnswer.Value.Day
                        && answer.DateAnswer.Value.Month == foundAnswer.DateAnswer.Value.Month
                        && answer.DateAnswer.Value.Year == foundAnswer.DateAnswer.Value.Year) ? foundQuestion.Balls : 0;
                case QuestionType.Check:
                    if (answer.TextAnswer == null || foundAnswer.TextAnswer == null)
                    {
                        return 0;
                    }
                    if (answer.TextAnswer.Equals(foundAnswer.TextAnswer))
                    {
                        return foundQuestion.Balls;
                    }
                    int[] correctOptions = ArrayTransformer.ToIntArray(answer.TextAnswer);
                    int[] selectedOptions = ArrayTransformer.ToIntArray(foundAnswer.TextAnswer);
                    int countWrong = 0;
                    foreach (int correctOption in correctOptions)
                    {
                        if (!selectedOptions.Contains(correctOption))
                        {
                            countWrong++;
                        }
                    }
                    foreach (int selectedOption in selectedOptions)
                    {
                        if (!correctOptions.Contains(selectedOption))
                        {
                            countWrong++;
                        }
                    }
                    if (countWrong == 0)
                    {
                        return foundQuestion.Balls;
                    }
                    else if (countWrong == 1)
                    {
                        return (double)foundQuestion.Balls / 2;
                    }
                    else
                    {
                        return 0;
                    }
                default:
                    return 0;
            }
        }

        public async Task<int> UpdateAttemptAnswer(int attemptId, int questionId, string userId, string value)
        {
            AttemptModel? attemptModel = GetAttemptModelById(attemptId);
            if (attemptModel == null)
            {
                return 404;
            }
            if (!attemptModel.Active)
            {
                return 400;
            }
            if (!attemptModel.UserId.Equals(userId))
            {
                return 403;
            }
            QuestionModel? questionModel = GetQuestionModelById(questionId);
            if (questionModel == null)
            {
                return 404;
            }
            Answer? foundAnswer = AnswerRepository.FirstOrDefault(x => x.QuestionId == questionId && x.AttemptId == attemptId);
            if (foundAnswer == null)
            {
                return 404;
            }
            foundAnswer.Flush();
            switch(questionModel.Type)
            {
                case QuestionType.Text:
                    foundAnswer.TextAnswer = value;
                    break;
                case QuestionType.Double:
                    if (!double.TryParse(value, out double doubleResult))
                    {
                        return 400;
                    }
                    foundAnswer.DoubleAnswer = doubleResult;
                    break;
                case QuestionType.Int:
                    if (!int.TryParse(value, out int intResult))
                    {
                        return 400;
                    }
                    foundAnswer.IntAnswer = intResult;
                    break;
                case QuestionType.Date:
                    if (!DateTime.TryParse(value, out DateTime dateResult))
                    {
                        return 400;
                    }
                    foundAnswer.DateAnswer = dateResult;
                    break;
                case QuestionType.Radio:
                    if (!int.TryParse(value, out int radioResult))
                    {
                        return 400;
                    }
                    if (!await OptionExists(questionId, radioResult))
                    {
                        return 404;
                    }
                    foundAnswer.IntAnswer = radioResult;
                    break;
                case QuestionType.Check:
                    int[] answerArray = ArrayTransformer.ToIntArray(value);
                    foreach(int id in answerArray)
                    {
                        if (!await OptionExists(questionId, id))
                        {
                            return 404;
                        }
                    }
                    foundAnswer.TextAnswer = string.Join('-', answerArray);
                    break;
            }
            AnswerRepository.Update(foundAnswer);
            await SaveAsync();
            return 200;
        }
        #endregion

        #region Stars
        public bool CheckTestStarred(int testId, string userId)
        {
            return GetUserStar(testId, userId) != null;
        }

        public async Task<int> GetTestStarCount(int testId)
        {
            return await UserStarRepository.CountAsync(x => x.TestId == testId);
        }

        public UserStar? GetUserStar(int testId, string userId)
        {
            return UserStarRepository.FirstOrDefault(x => x.TestId == testId && x.UserId.Equals(userId));
        }

        public async Task ToggleStarAsync(int testId, string userId)
        {
            UserStar? foundStar = GetUserStar(testId, userId);
            if (foundStar == null)
            {
                await UserStarRepository.AddAsync(new UserStar() { TestId = testId, UserId = userId });
            }
            else
            {
               UserStarRepository .Remove(foundStar);
            }
            await SaveAsync();
        }
        #endregion

        #region CorrectAnswer
        public CorrectAnswer? GetQuestionCorrectAnswer(int questionId)
        {
            return CorrectAnswerRepository.FirstOrDefault(x => x.QuestionId == questionId);
        }
        #endregion

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
