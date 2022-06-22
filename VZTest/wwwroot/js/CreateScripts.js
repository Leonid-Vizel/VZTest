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
    while (options.length > 0) {
        options.setAttribute('name', 'option-' + newQuestionId);
    }
}

function ReindexOption(questionId, oldId, newId) {
    var titleInput = document.getElementById('Questions[' + questionId + '].Options[' + oldId + '].Text');
    titleInput.setAttribute('id', 'Questions[' + questionId + '].Options[' + newId + '].Text');
    var deleteBtn = document.getElementById('delete-question-' + questionId + '-option-' + oldId);
    deleteBtn.setAttribute('id', 'delete-question-' + questionId + '-option-' + newId);
    deleteBtn.setAttribute('onclick', 'DeleteOption(' + questionId + ',' + newId + ')');
}

function ReindexOption(oldQuestionId, newQuestionId, oldId, newId) {
    var titleInput = document.getElementById('Questions[' + oldQuestionId + '].Options[' + oldId + '].Text');
    titleInput.setAttribute('id', 'Questions[' + newQuestionId + '].Options[' + newId + '].Text');
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

function CheckCorrectInputCreated(id) {
    var correctInput = document.getElementById('Questions[' + id + '].Correct');
    var correctLabel = document.getElementById('CorrectLabel-' + id);
    var ballsInput = document.getElementById('Questions[' + id + '].Balls');
    if (correctInput == null) {
        correctInput = document.createElement('input');
        correctInput.setAttribute('class', 'form-control');
        correctInput.setAttribute('placeholder', 'Ответ');
        correctInput.setAttribute('id', 'Questions[' + id + '].Correct');
        if (correctLabel != null) {
            correctLabel.insertAdjacentElement('afterend', correctInput);
        }
        else {
            ballsInput.insertAdjacentElement('afterend', correctInput);
        }
    }
    if (correctLabel == null) {
        correctLabel = document.createElement('h5');
        correctLabel.setAttribute('id', 'CorrectLabel-' + id);
        correctLabel.innerHTML = 'Баллы за ответ:';
        ballsInput.insertAdjacentElement('afterend', correctLabel);
    }
    return correctInput;
}

function DeleteCorrectInput(id) {
    var correctInput = document.getElementById('Questions[' + id + '].Correct');
    var correctLabel = document.getElementById('CorrectLabel-' + id);
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

            }
            break;
        case 'CheckToRadio':
            for (var i = 0; i < elementArray.length; i++) {

            }
            break;
    }
}

function OnSelectAnswer(id) {
    var correctInput;
    var element = document.getElementById('Questions[' + id + '].Type');
    if (element == null) {
        return;
    }
    switch (element.value) {
        case '0':
            DeleteOptions(id);
            DeleteCreator(id);
            correctInput = CheckCorrectInputCreated(id);
            correctInput.value = null;
            correctInput.removeAttribute('onkeypress');
            correctInput.setAttribute('type', 'text');
            break;
        case '1':
            DeleteCorrectInput(id);
            AddRadioCreator(id);
            break;
        case '2':
            DeleteCorrectInput(id);
            AddCheckCreator(id);
            break;
        case '3':
            DeleteOptions(id);
            DeleteCreator(id);
            correctInput = CheckCorrectInputCreated(id);
            correctInput.value = null;
            correctInput.removeAttribute('onkeypress');
            correctInput.setAttribute('type', 'number');
            break;
        case '4':
            DeleteOptions(id);
            DeleteCreator(id);
            correctInput = CheckCorrectInputCreated(id);
            correctInput.value = null;
            correctInput.setAttribute('onkeypress', 'KeyPress(event);');
            correctInput.setAttribute('type', 'text');
            break;
        case '5':
            DeleteOptions(id);
            DeleteCreator(id);
            correctInput = CheckCorrectInputCreated(id);
            correctInput.value = null;
            correctInput.removeAttribute('onkeypress');
            correctInput.setAttribute('type', 'date');
            break;
    }
}