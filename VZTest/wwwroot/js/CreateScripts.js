function AddQuestion() {
    let questionCount = document.getElementsByName('QuestionContainer').length;
    let div = document.createElement('div');
    div.setAttribute('class', 'mt-3');
    div.setAttribute('name', 'QuestionContainer');
    div.setAttribute('id', questionCount);
    let elementAfter = document.getElementById('addQuestionBtn');
    div.innerHTML = document.getElementById('QuestionBlock').innerHTML.replace(/{questionId}/g, questionCount).replace(/{questionIdPlus}/g, questionCount + 1);
    elementAfter.insertAdjacentElement('beforebegin', div);
    div.scrollIntoView();
}

function DeleteQuestion(questionId) {
    let questionDiv = document.getElementById(questionId);
    if (questionDiv == null) {
        return;
    }
    questionDiv.remove();
    ReindexQuestions();
}

function ReindexQuestions() {
    let allQuestions = document.getElementsByName('QuestionContainer');
    for (let i = 0; i < allQuestions.length; i++) {
        let question = allQuestions[i];
        ReindexQuestion(question.id, i);
        question.setAttribute('id', i);
    }
}

function ReindexQuestion(oldId, newId) {
    if (oldId == newId) {
        return;
    }
    let header = document.getElementById('QuestionHeader-' + oldId);
    header.innerHTML = 'Вопрос #' + (newId + 1);
    header.setAttribute('id', 'QuestionHeader-' + newId);
    let input = document.getElementById('Questions[' + oldId + '].Title');
    input.setAttribute('id', 'Questions[' + newId + '].Title');
    let select = document.getElementById('Questions[' + oldId + '].Type');
    select.setAttribute('onchange', 'OnSelectAnswer(' + newId + ');');
    select.setAttribute('id', 'Questions[' + newId + '].Type');
    let deleteBtn = document.getElementById('btn-delete-' + oldId);
    deleteBtn.setAttribute('onclick', 'DeleteQuestion(' + newId + ');');
    deleteBtn.setAttribute('id', 'btn-delete-' + newId);
    let questionErrorSpan = document.getElementById('Question-' + oldId + '-Errors');
    questionErrorSpan.setAttribute('id', 'Question-' + newId + '-Errors');
    let image = document.getElementById('Questions[' + oldId + '].ImageUrl');
    image.setAttribute('id', 'Questions[' + newId + '].ImageUrl');
    let imageErrorSpan = document.getElementById('Image-' + oldId + '-Errors');
    imageErrorSpan.setAttribute('id', 'Image-' + newId + '-Errors');
    let balls = document.getElementById('Questions[' + oldId + '].Balls');
    balls.setAttribute('id', 'Questions[' + newId + '].Balls');
    let ballsErrorSpan = document.getElementById('Balls-' + oldId + '-Errors');
    ballsErrorSpan.setAttribute('id', 'Balls-' + newId + '-Errors');
    let correctLabel = document.getElementById('CorrectLabel-' + oldId);
    if (correctLabel != null) {
        correctLabel.setAttribute('id', 'CorrectLabel-' + newId);
    }
    let correctInput = document.getElementById('Questions[' + oldId + '].Correct');
    if (correctInput != null && correctInput.getAttribute('name') == null) {
        correctInput.setAttribute('id', 'Questions[' + newId + '].Correct');
        correctInput = document.getElementById('Questions[' + oldId + '].Correct');
        let correctErrorSpan = document.getElementById('Correct-' + oldId + '-Errors');
        correctErrorSpan.setAttribute('id', 'Correct-' + newId + '-Errors');
    }
    var creator = document.getElementById('Creator-' + oldId);
    if (creator != null) {
        creator.setAttribute('onclick', 'AddRadioOption(' + newId + ');');
        creator.setAttribute('id', 'Creator-' + newId);
    }
    ReindexOptionsWithQuestion(oldId, newId);
}

function AddRadioOption(questionId) {
    let optionCount = document.getElementsByName('option-' + questionId).length;
    let optionBlock = document.getElementById('RadioOptionBlock').innerHTML.replace(/{questionId}/g, questionId).replace(/{optionId}/g, optionCount);
    let creatorButton = document.getElementById('Creator-' + questionId);
    let optionDiv = document.createElement('div');
    if (optionBlock == null || creatorButton == null || optionDiv == null) {
        return;
    }
    optionDiv.setAttribute('id', 'option-' + questionId + '-' + optionCount);
    optionDiv.setAttribute('name', 'option-' + questionId);
    optionDiv.setAttribute('class', 'input-group mt-2 ml-3');
    optionDiv.innerHTML = optionBlock;
    creatorButton.insertAdjacentElement('BeforeBegin', optionDiv);
    optionDiv.scrollIntoView();
}

function AddCheckOption(questionId) {
    let optionCount = document.getElementsByName('option-' + questionId).length;
    let optionBlock = document.getElementById('CheckOptionBlock').innerHTML.replace(/{questionId}/g, questionId).replace(/{optionId}/g, optionCount);
    let creatorButton = document.getElementById('Creator-' + questionId);
    let optionDiv = document.createElement('div');
    if (optionBlock == null || creatorButton == null || optionDiv == null) {
        return;
    }
    optionDiv.setAttribute('id', 'option-' + questionId + '-' + optionCount);
    optionDiv.setAttribute('name', 'option-' + questionId);
    optionDiv.setAttribute('class', 'input-group mt-2 ml-3 ');
    optionDiv.innerHTML = optionBlock;
    creatorButton.insertAdjacentElement('BeforeBegin', optionDiv);
    optionDiv.scrollIntoView();
}

function DeleteOption(questionId, optionId) {
    let optionDiv = document.getElementById('option-' + questionId + '-' + optionId);
    if (optionDiv == null) {
        return;
    }
    optionDiv.remove();
    ReindexOptions(questionId);
}

function ReindexOptions(questionId) {
    var options = document.getElementsByName('option-' + questionId);
    for (var i = 0; i < options.length; i++) {
        var option = options[i];
        var splitArray = option.id.split('-');
        var oldId = splitArray[splitArray.length - 1];
        ReindexOption(questionId, oldId, i);
        option.setAttribute('id', 'option-' + questionId + '-' + i);
    }
}

function ReindexOptionsWithQuestion(oldQuestionId, newQuestionId) {
    var options = document.getElementsByName('option-' + oldQuestionId);
    for (var i = 0; i < options.length; i++) {
        var option = options[i];
        var splitArray = option.id.split('-');
        var oldId = splitArray[splitArray.length - 1];
        ReindexOptionWithQuestion(oldQuestionId, newQuestionId, oldId, i);
        option.setAttribute('id', 'option-' + newQuestionId + '-' + i);
    }
    if (oldQuestionId != newQuestionId) {
        while (options.length > 0) {
            options[0].setAttribute('name', 'option-' + newQuestionId);
        }
    }
}

function ReindexOption(questionId, oldId, newId) {
    if (oldId == newId) {
        return;
    }
    var titleInput = document.getElementById('Questions[' + questionId + '].Options[' + oldId + '].Text');
    titleInput.setAttribute('id', 'Questions[' + questionId + '].Options[' + newId + '].Text');
    var correctInput = document.getElementById('Option-' + questionId + '-' + oldId + '-Correct');
    correctInput.setAttribute('id', 'Option-' + questionId + '-' + newId + '-Correct');
    correctInput.setAttribute('value', newId);
    var deleteBtn = document.getElementById('delete-question-' + questionId + '-option-' + oldId);
    deleteBtn.setAttribute('id', 'delete-question-' + questionId + '-option-' + newId);
    deleteBtn.setAttribute('onclick', 'DeleteOption(' + questionId + ',' + newId + ')');
}

function ReindexOptionWithQuestion(oldQuestionId, newQuestionId, oldId, newId) {
    var titleInput = document.getElementById('Questions[' + oldQuestionId + '].Options[' + oldId + '].Text');
    titleInput.setAttribute('id', 'Questions[' + newQuestionId + '].Options[' + newId + '].Text');
    var correctInput = document.getElementById('Option-' + oldQuestionId + '-' + oldId + '-Correct');
    correctInput.setAttribute('id', 'Option-' + newQuestionId + '-' + newId + '-Correct');
    correctInput.setAttribute('name', 'Questions[' + newQuestionId + '].Correct');
    correctInput.setAttribute('value', newId);
    var deleteBtn = document.getElementById('delete-question-' + oldQuestionId + '-option-' + oldId);
    deleteBtn.setAttribute('id', 'delete-question-' + newQuestionId + '-option-' + newId);
    deleteBtn.setAttribute('onclick', 'DeleteOption(' + newQuestionId + ',' + newId + ')');
}

function DeleteOptions(questionId) {
    var elements = document.getElementsByName('option-' + questionId);
    while (elements.length > 0) {
        elements[0].remove();
    }
}

function CheckCorrectInputCreated(questionId) {
    var correctInput = document.getElementById('Questions[' + questionId + '].Correct');
    var correctLabel = document.getElementById('CorrectLabel-' + questionId);
    var correctErrorSpan = document.getElementById('Correct-' + questionId + '-Errors');
    var ballsInput = document.getElementById('Questions[' + questionId + '].Balls');
    if (correctInput == null) {
        correctInput = document.createElement('input');
        correctInput.setAttribute('class', 'form-control');
        correctInput.setAttribute('placeholder', 'Ответ');
        correctInput.setAttribute('id', 'Questions[' + questionId + '].Correct');
        if (correctLabel != null) {
            correctLabel.insertAdjacentElement('afterend', correctInput);
        }
        else {
            ballsInput.insertAdjacentElement('afterend', correctInput);
        }
    }
    if (correctErrorSpan == null) {
        correctErrorSpan = document.createElement('span');
        correctErrorSpan.setAttribute('class', 'text-danger');
        correctErrorSpan.setAttribute('id', 'Correct-' + questionId + '-Errors');
        correctInput.insertAdjacentElement('afterend', correctErrorSpan);
    }
    if (correctLabel == null) {
        correctLabel = document.createElement('h5');
        correctLabel.setAttribute('id', 'CorrectLabel-' + questionId);
        correctLabel.innerHTML = 'Правильный ответ:';
        ballsInput.insertAdjacentElement('afterend', correctLabel);
    }
    return correctInput;
}

function DeleteCorrectInput(questionId) {
    var correctArray = document.getElementsByName('Questions[' + questionId + '].Correct');
    if (correctArray.length > 0) {
        return;
    }
    var correctInput = document.getElementById('Questions[' + questionId + '].Correct');
    var correctLabel = document.getElementById('CorrectLabel-' + questionId);
    var correctErrorSpan = document.getElementById('Correct-' + questionId + '-Errors');
    if (correctInput != null) {
        correctInput.remove();
    }
    if (correctLabel != null) {
        correctLabel.remove();
    }
    if (correctErrorSpan != null) {
        correctErrorSpan.remove();
    }
}

function AddRadioCreator(questionId) {
    var creator = document.getElementById('Creator-' + questionId);
    if (creator != null) {
        creator.setAttribute('onclick', 'AddRadioOption(' + questionId + ');');
        return;
    }
    var button = document.createElement('a');
    button.setAttribute('id', 'Creator-' + questionId);
    button.setAttribute('class', 'btn btn-outline-info mt-2');
    button.setAttribute('onclick', 'AddRadioOption(' + questionId + ');');
    button.innerHTML = "<i class=\"bi bi-plus-circle\"></i> Добавить опцию";
    var ballsErrorSpan = document.getElementById('Balls-' + questionId + '-Errors');
    ballsErrorSpan.insertAdjacentElement('afterend', button);
}

function AddCheckCreator(questionId) {
    var creator = document.getElementById('Creator-' + questionId);
    if (creator != null) {
        creator.setAttribute('onclick', 'AddCheckOption(' + questionId + ');');
        return;
    }
    var button = document.createElement('a');
    button.setAttribute('id', 'Creator-' + questionId);
    button.setAttribute('class', 'btn btn-outline-info mt-2');
    button.setAttribute('onclick', 'AddCheckOption(' + questionId + ');');
    button.innerHTML = "<i class=\"bi bi-plus-circle\"></i> Добавить опцию";
    var ballsErrorSpan = document.getElementById('Balls-' + questionId + '-Errors');
    ballsErrorSpan.insertAdjacentElement('afterend', button);
}

function DeleteCreator(questionId) {
    var creator = document.getElementById('Creator-' + questionId);
    if (creator == null) {
        return;
    }
    creator.remove();
}

function TransformCorrects(questionId, string) {
    var elementArray = document.getElementsByName('Questions[' + questionId + '].Correct');
    switch (string) {
        case 'RadioToCheck':
            for (var i = 0; i < elementArray.length; i++) {
                elementArray[i].setAttribute('type', 'checkbox');
            }
            break;
        case 'CheckToRadio':
            for (var i = 0; i < elementArray.length; i++) {
                elementArray[i].setAttribute('type', 'radio');
            }
            break;
    }
}

function CheckUrl(urlString) {
    let url;

    try {
        url = new URL(urlString);
    } catch (_) {
        return false;
    }

    return url.protocol === "http:" || url.protocol === "https:";
}

function CheckTime(timeLower, timeHigher) {
    let lowerDateTime = new Date(timeLower).getDate();
    let higherDateTime = new Date(timeHigher).getDate();
    return lowerDateTime < higherDateTime;
}

function OnSelectAnswer(questionId) {
    var correctInput;
    var element = document.getElementById('Questions[' + questionId + '].Type');
    if (element == null) {
        return;
    }
    switch (element.value) {
        case '0':
            DeleteOptions(questionId);
            DeleteCreator(questionId);
            correctInput = CheckCorrectInputCreated(questionId);
            correctInput.value = null;
            correctInput.removeAttribute('oninput');
            correctInput.setAttribute('type', 'text');
            break;
        case '1':
            DeleteCorrectInput(questionId);
            TransformCorrects(questionId, "CheckToRadio");
            AddRadioCreator(questionId);
            break;
        case '2':
            DeleteCorrectInput(questionId);
            TransformCorrects(questionId, "RadioToCheck");
            AddCheckCreator(questionId);
            break;
        case '3':
            DeleteOptions(questionId);
            DeleteCreator(questionId);
            correctInput = CheckCorrectInputCreated(questionId);
            correctInput.value = null;
            correctInput.setAttribute('oninput', "OnInput(event);");
            correctInput.setAttribute('type', 'text');
            break;
        case '4':
            DeleteOptions(questionId);
            DeleteCreator(questionId);
            correctInput = CheckCorrectInputCreated(questionId);
            correctInput.value = null;
            correctInput.removeAttribute('oninput');
            correctInput.setAttribute('type', 'number');
            break;
        case '5':
            DeleteOptions(questionId);
            DeleteCreator(questionId);
            correctInput = CheckCorrectInputCreated(questionId);
            correctInput.value = null;
            correctInput.removeAttribute('oninput');
            correctInput.setAttribute('type', 'date');
            break;
    }
}

function CheckAndSend(method) {
    var verificationValue = document.getElementsByName('__RequestVerificationToken')[0].getAttribute('value');
    //Title
    var titleElement = document.getElementById('Title');
    var titleError = document.getElementById('Title-Errors');
    if (titleElement == null || titleElement.value == '') {
        titleError.innerHTML = "Укажите название теста!";
        return;
    }
    else {
        titleError.innerHTML = "";
    }
    //Description
    var descriptionElement = document.getElementById('Description');
    var descriptionError = document.getElementById('Description-Errors');
    if (descriptionElement == null || descriptionElement.value == '') {
        descriptionError.innerHTML = "Укажите описание теста!";
        return;
    }
    else {
        descriptionError.innerHTML = "";
    }

    //MaxAttempts
    var attemptsElement = document.getElementById('MaxAttempts');
    var attemptsError = document.getElementById('MaxAttempts-Errors');
    if (attemptsElement == null || attemptsElement.value == '') {
        attemptsError.innerHTML = "Укажите максимальное количество попыток для теста!";
        return;
    }
    else if (attemptsElement.value < 1 || attemptsElement.value > 100) {
        attemptsError.innerHTML = "Максимальное количество попыток должно быть в пределах от 1 до 100!";
        return;
    }
    else {
        descriptionError.innerHTML = "";
    }
    //Password
    var passwordElement = document.getElementById('Password');
    //Shuffle
    var shuffleElement = document.getElementById('Shuffle');
    //Starting To Fill Dictionary
    var dictionary = new Object();

    var testIdElement = document.getElementById('test-id');
    if (testIdElement != null) {
        dictionary["Id"] = testIdElement.innerHTML;
    }
    dictionary["Title"] = titleElement.value;
    dictionary["Description"] = descriptionElement.value;

    var imageElement = document.getElementById('ImageUrl');
    var imageError = document.getElementById('ImageUrl-Errors');
    if (imageElement != null && !CheckUrl(imageElement.value)) {
        imageError.innerHTML = "Укажите действительную ссылку!";
        return;
    }
    else if (imageElement != null) {
        dictionary["ImageUrl"] = imageElement.value;
    }
    else {
        imageError.innerHTML = "";
    }

    var startTimeElement = document.getElementById('StartTime');
    if (startTimeElement != null) {
        dictionary['StartTime'] = startTimeElement.value;
    }

    var endTimeElement = document.getElementById('EndTime');
    var endTimeError = document.getElementById('EndTime-Errors');
    if (endTimeElement != null) {
        if (startTimeElement != null && !CheckTime(startTimeElement.value, endTimeElement.value)) {
            endTimeError.innerHTML = 'Дата и время окончания теста должны быть позже даты и времени его начала!';
            return;
        }
        else {
            endTimeError.innerHTML = '';
        }
        dictionary['EndTime'] = endTimeElement.value;
    }
    else {
        endTimeError.innerHTML = '';
    }

    if (passwordElement == null || passwordElement.value == '') {
        dictionary['Password'] = '';
    }
    else {
        dictionary['Password'] = passwordElement.value;
    }
    dictionary["MaxAttempts"] = attemptsElement.value;
    if (shuffleElement == null) {
        dictionary['Shuffle'] = '';
    }
    else {
        dictionary['Shuffle'] = shuffleElement.checked;
    }

    var questions = document.getElementsByName('QuestionContainer');
    if (questions.length == 0) {
        attemptsError.innerHTML = "В тесте должен быть как минимум 1 вопрос!";
        return;
    }
    else {
        attemptsError.innerHTML = "";
    }
    for (var i = 0; i < questions.length; i++) {
        var questionId = questions[i].id;
        var questionError = document.getElementById('Question-' + questionId + '-Errors');
        var questionTitleElement = document.getElementById('Questions[' + questionId + '].Title');
        if (questionTitleElement == null || questionTitleElement.value == '') {
            questionError.innerHTML = "Укажите название вопроса!";
            return;
        }
        else {
            questionError.innerHTML = "";
        }
        dictionary['Questions[' + questionId + '].Title'] = questionTitleElement.value;

        if (questions[i].getAttribute('data-id') != null) {
            dictionary['Questions[' + questionId + '].Id'] = questions[i].getAttribute('data-id');
        }
        else {
            dictionary['Questions[' + questionId + '].Id'] = 0;
        }

        var questionTypeElement = document.getElementById('Questions[' + questionId + '].Type');
        if (questionTypeElement == null || questionTypeElement.value == '') {
            questionError.innerHTML = "Укажите тип вопроса!";
            return;
        }
        else {
            questionError.innerHTML = "";
        }
        dictionary['Questions[' + questionId + '].Type'] = questionTypeElement.value;

        var questionImageElement = document.getElementById('Questions[' + questionId + '].ImageUrl');
        var questionImageError = document.getElementById('Image-' + questionId + '-Errors');
        if (questionImageElement != null && !CheckUrl(questionImageElement.value)) {
            questionImageError.innerHTML = "Укажите действительную ссылку!";
            return;
        }
        else if (questionImageElement != null) {
            dictionary['Questions[' + questionId + '].ImageUrl'] = questionImageElement.value;
        }
        else {
            questionImageError.innerHTML = "";
        }

        var questionBallsElement = document.getElementById('Questions[' + questionId + '].Balls');
        var questionBallsError = document.getElementById('Balls-' + questionId + '-Errors');
        if (questionBallsElement == null || questionBallsElement.value == '') {
            questionBallsError.innerHTML = "Укажите баллы за правильный ответ!";
            return;
        }
        else {
            questionBallsError.innerHTML = "";
        }
        dictionary['Questions[' + questionId + '].Balls'] = questionBallsElement.value;

        switch (questionTypeElement.value) {
            case '0':
            case '3':
            case '4':
            case '5':
                var questionCorrectError = document.getElementById('Correct-' + questionId + '-Errors');
                var questionCorrectElement = document.getElementById('Questions[' + questionId + '].Correct');
                if (questionCorrectElement == null || questionCorrectElement.value == '') {
                    questionCorrectError.innerHTML = "Укажите правильный ответ!";
                    return;
                }
                else {
                    questionCorrectError.innerHTML = "";
                }
                dictionary['Questions[' + questionId + '].Correct'] = questionCorrectElement.value;
                break;
            case '1':
                var radioArray = document.getElementsByName('option-' + questionId);
                for (var j = 0; j < radioArray.length; j++) {
                    var idSplit = radioArray[j].id.split('-');
                    var optionId = idSplit[idSplit.length - 1];
                    var optionTitleElement = document.getElementById('Questions[' + questionId + '].Options[' + optionId + '].Text');
                    if (optionTitleElement == null || optionTitleElement.value == '') {
                        questionError.innerHTML = "Укажите названия всех опций!";
                        return;
                    }
                    else {
                        questionError.innerHTML = "";
                    }
                    dictionary['Questions[' + questionId + '].Options[' + j + ']'] = optionTitleElement.value;
                    if (radioArray[j].getAttribute('data-id') != null) {
                        dictionary['Questions[' + questionId + '].OptionIds[' + j + ']'] = radioArray[j].getAttribute('data-id');
                    }
                    else {
                        dictionary['Questions[' + questionId + '].OptionIds[' + j + ']'] = 0;
                    }
                }
                var radioResult = GetRadioValue('Questions[' + questionId + '].Correct');
                if (radioResult == '') {
                    questionError.innerHTML = "Укажите правильный ответ!";
                    return;
                }
                else {
                    questionError.innerHTML = "";
                }
                dictionary['Questions[' + questionId + '].Correct'] = radioResult;
                break;
            case '2':
                var checkArray = document.getElementsByName('option-' + questionId);
                for (var j = 0; j < checkArray.length; j++) {
                    var idSplit = checkArray[j].id.split('-');
                    var optionId = idSplit[idSplit.length - 1];
                    var optionTitleElement = document.getElementById('Questions[' + questionId + '].Options[' + optionId + '].Text');
                    if (optionTitleElement == null || optionTitleElement.value == '') {
                        questionError.innerHTML = "Укажите названия всех опций!";
                        return;
                    }
                    else {
                        questionError.innerHTML = "";
                    }
                    dictionary['Questions[' + questionId + '].Options[' + j + ']'] = optionTitleElement.value;
                    if (checkArray[j].getAttribute('data-id') != null) {
                        dictionary['Questions[' + questionId + '].OptionIds[' + j + ']'] = checkArray[j].getAttribute('data-id');
                    }
                    else {
                        dictionary['Questions[' + questionId + '].OptionIds[' + j + ']'] = 0;
                    }
                }
                var checkResult = GetCheckValue('Questions[' + questionId + '].Correct');
                if (checkResult == '') {
                    questionError.innerHTML = "Укажите правильный ответ!";
                    return;
                }
                else {
                    questionError.innerHTML = "";
                }
                dictionary['Questions[' + questionId + '].Correct'] = checkResult;
                break;
            default:
                questionError.innerHTML = "Неизвестный тип вопроса!";
                return;
                break;
        }
    }
    dictionary['__RequestVerificationToken'] = verificationValue;
    $.ajax({
        type: "POST",
        url: "/Test/" + method + "/",
        data: dictionary,
        error: function (error) {
            switch (error.status) {
                case 401:
                    swal("Авторизуйтесь!", "Для того, чтобы отметить тест надо войти в аккаунт.", "error");
                    break;
                default:
                    swal("Ошибка!", "Отправка данных не удалась, попробуйте позже!", "error");
                    break;
            }
        },
        success: function (data) {
            window.location.replace("/Test/Preview/" + data);
        }
    });
}