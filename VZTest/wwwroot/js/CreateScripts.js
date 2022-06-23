function AddQuestion() {
    var questionCount = document.getElementsByName('QuestionContainer').length;
    var div = document.createElement('div');
    div.setAttribute('class', 'mt-3');
    div.setAttribute('name', 'QuestionContainer');
    div.setAttribute('id', questionCount);
    var elementAfter = document.getElementById('addQuestionBtn');
    div.innerHTML = document.getElementById('QuestionBlock').innerHTML.replace(/{questionId}/g, questionCount).replace(/{questionIdPlus}/g, questionCount + 1);
    elementAfter.insertAdjacentElement('beforebegin', div);
    div.scrollIntoView();
}

function DeleteQuestion(questionId) {
    var questionDiv = document.getElementById(questionId);
    if (questionDiv == null) {
        return;
    }
    questionDiv.remove();
    ReindexQuestions();
}

function ReindexQuestions() {
    var allQuestions = document.getElementsByName('QuestionContainer');
    for (var i = 0; i < allQuestions.length; i++) {
        var question = allQuestions[i];
        ReindexQuestion(question.id, i);
        question.setAttribute('id', i);
    }
}

function ReindexQuestion(oldId, newId) {
    if (oldId == newId) {
        return;
    }
    var header = document.getElementById('QuestionHeader-' + oldId);
    header.innerHTML = 'Вопрос #' + (newId + 1);
    header.setAttribute('id', 'QuestionHeader-' + newId);
    var input = document.getElementById('Questions[' + oldId + '].Title');
    input.setAttribute('id', 'Questions[' + newId + '].Title');
    var select = document.getElementById('Questions[' + oldId + '].Type');
    select.setAttribute('onchange', 'OnSelectAnswer(' + newId + ');');
    select.setAttribute('id', 'Questions[' + newId + '].Type');
    var deleteBtn = document.getElementById('btn-delete-' + oldId);
    deleteBtn.setAttribute('onclick', 'DeleteQuestion(' + newId + ');');
    deleteBtn.setAttribute('id', 'btn-delete-' + newId);
    var balls = document.getElementById('Questions[' + oldId + '].Balls');
    balls.setAttribute('id', 'Questions[' + newId + '].Balls');
    var correctLabel = document.getElementById('CorrectLabel-' + oldId);
    if (correctLabel != null) {
        correctLabel.setAttribute('id', 'CorrectLabel-' + newId);
    }
    var correctInput = document.getElementById('Questions[' + oldId + '].Correct');
    while (correctInput != null) {
        correctInput.setAttribute('id', 'Questions[' + newId + '].Correct');
        correctInput.setAttribute('name', 'Questions[' + newId + '].Correct');
        correctInput = document.getElementById('Questions[' + oldId + '].Correct');
    }
    var creator = document.getElementById('Creator-' + oldId);
    if (creator != null) {
        creator.setAttribute('onclick', 'AddRadioOption(' + newId + ');');
        creator.setAttribute('id', 'Creator-' + newId);
    }
    ReindexOptions(oldId, newId);
}

function AddRadioOption(questionId) {
    var optionCount = document.getElementsByName('option-' + questionId).length;
    var optionBlock = document.getElementById('RadioOptionBlock').innerHTML.replace(/{questionId}/g, questionId).replace(/{optionId}/g, optionCount);
    var creatorButton = document.getElementById('Creator-' + questionId);
    var optionDiv = document.createElement('div');
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
    var optionCount = document.getElementsByName('option-' + questionId).length;
    var optionBlock = document.getElementById('CheckOptionBlock').innerHTML.replace(/{questionId}/g, questionId).replace(/{optionId}/g, optionCount);
    var creatorButton = document.getElementById('Creator-' + questionId);
    var optionDiv = document.createElement('div');
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
    var optionDiv = document.getElementById('option-' + questionId + '-' + optionId);
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

function ReindexOptions(oldQuestionId, newQuestionId) {
    var options = document.getElementsByName('option-' + oldQuestionId);
    for (var i = 0; i < options.length; i++) {
        var option = options[i];
        var splitArray = option.id.split('-');
        var oldId = splitArray[splitArray.length - 1];
        ReindexOption(oldQuestionId, newQuestionId, oldId, i);
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
    titleInput.setAttribute('value', newId);
    var deleteBtn = document.getElementById('delete-question-' + questionId + '-option-' + oldId);
    deleteBtn.setAttribute('id', 'delete-question-' + questionId + '-option-' + newId);
    deleteBtn.setAttribute('onclick', 'DeleteOption(' + questionId + ',' + newId + ')');
}

function ReindexOption(oldQuestionId, newQuestionId, oldId, newId) {
    var titleInput = document.getElementById('Questions[' + oldQuestionId + '].Options[' + oldId + '].Text');
    titleInput.setAttribute('id', 'Questions[' + newQuestionId + '].Options[' + newId + '].Text');
    titleInput.setAttribute('value', newId);
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
    if (correctLabel == null) {
        correctLabel = document.createElement('h5');
        correctLabel.setAttribute('id', 'CorrectLabel-' + questionId);
        correctLabel.innerHTML = 'Баллы за ответ:';
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
    if (correctInput == null) {
        return;
    }
    correctInput.remove();
    if (correctLabel == null) {
        return;
    }
    correctLabel.remove();
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
    var ballsInput = document.getElementById('Questions[' + questionId + '].Balls');
    ballsInput.insertAdjacentElement('afterend', button);
}

function DeleteCreator(questionId) {
    var creator = document.getElementById('Creator-' + questionId);
    if (creator == null) {
        return;
    }
    creator.remove();
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
    var ballsInput = document.getElementById('Questions[' + questionId + '].Balls');
    ballsInput.insertAdjacentElement('afterend', button);
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
            correctInput.removeAttribute('onkeypress');
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
            correctInput.removeAttribute('onkeypress');
            correctInput.setAttribute('type', 'number');
            break;
        case '4':
            DeleteOptions(questionId);
            DeleteCreator(questionId);
            correctInput = CheckCorrectInputCreated(questionId);
            correctInput.value = null;
            correctInput.setAttribute('onkeypress', 'KeyPress(event);');
            correctInput.setAttribute('type', 'text');
            break;
        case '5':
            DeleteOptions(questionId);
            DeleteCreator(questionId);
            correctInput = CheckCorrectInputCreated(questionId);
            correctInput.value = null;
            correctInput.removeAttribute('onkeypress');
            correctInput.setAttribute('type', 'date');
            break;
    }
}

function CheckAndSend() {
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
    dictionary["Title"] = titleElement.value;
    dictionary["Description"] = descriptionElement.value;
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
    for (var i = 0; i < questions.length; i++) {
        var questionId = questions[i].id;
        console.log(questions);
        var questionTitleElement = document.getElementById('Questions[' + questionId + '].Title');
        if (questionTitleElement == null || questionTitleElement.value == '') {
            console.log("ERR TITLE");
            return;
        }
        dictionary['Questions[' + questionId + '].Title'] = questionTitleElement.value;

        var questionTypeElement = document.getElementById('Questions[' + questionId + '].Type');
        if (questionTypeElement == null || questionTypeElement.value == '') {
            console.log("ERR TYPE");
            return;
        }
        dictionary['Questions[' + questionId + '].Type'] = questionTypeElement.value;

        var questionBallsElement = document.getElementById('Questions[' + questionId + '].Balls');
        if (questionBallsElement == null || questionBallsElement.value == '') {
            console.log("ERR BALLS");
            return;
        }
        dictionary['Questions[' + questionId + '].Balls'] = questionBallsElement.value;

        switch (questionTypeElement.value) {
            case '0':
            case '3':
            case '4':
            case '5':
                var questionCorrectElement = document.getElementById('Questions[' + questionId + '].Correct');
                if (questionCorrectElement == null || questionCorrectElement.value == '') {
                    console.log("ERR CORRECT");
                    return;
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
                        console.log("ERR RADIO");
                        return;
                    }
                    dictionary['Questions[' + questionId + '].Options[' + j + ']'] = optionTitleElement.value;
                }
                var radioResult = GetRadioValue('Questions[' + questionId + '].Correct');
                dictionary['Questions[' + questionId + '].Correct'] = radioResult;
                break;
            case '2':
                var checkArray = document.getElementsByName('option-' + questionId);
                for (var j = 0; j < checkArray.length; j++) {
                    var idSplit = checkArray[j].id.split('-');
                    var optionId = idSplit[idSplit.length - 1];
                    var optionTitleElement = document.getElementById('Questions[' + questionId + '].Options[' + optionId + '].Text');
                    if (optionTitleElement == null || optionTitleElement.value == '') {
                        console.log("ERR CHECK");
                        return;
                    }
                    dictionary['Questions[' + questionId + '].Options[' + j + ']'] = optionTitleElement.value;
                }
                var checkResult = GetCheckValue('Questions[' + questionId + '].Correct');
                dictionary['Questions[' + questionId + '].Correct'] = checkResult;
                break;
            default:
                //ERROR
                return;
                break;
        }
        console.log(questions);
    }
    dictionary['__RequestVerificationToken'] = verificationValue;
    $.ajax({
        type: "POST",
        url: "/Test/Create/",
        data: dictionary,
        error: function (error) {
            switch (error.status) {
                case 403:
                    swal("Недоступно!", "У Вас нет доступа к этому действию!", "error");
                    break;
                case 401:
                    swal("Авторизуйтесь!", "Для того, чтобы отметить тест надо войти в аккаунт.", "error");
                    break;
                default:
                    swal("Ошибка!", "Отправка данных не удалась, попробуйте позже!", "error");
                    break;
            }
        },
        success: function(data) {
            window.location.replace("/Test/Preview/" + data);
        }
    });
}