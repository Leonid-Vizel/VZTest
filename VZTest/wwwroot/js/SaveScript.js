var lastSave = false;
var ajaxArray = new Array();

function ProcessErrors(error) {
    switch (error.status) {
        case 403:
            swal("Недоступно!", "У Вас нет доступа к этому действию!", "error");
            break;
        case 404:
            swal("Элемент не найден!", "Перезагрузите страницу и попробуйте снова.", "error");
            break;
        case 401:
            swal("Авторизуйтесь!", "Для того, чтобы пройти тест надо войти в аккаунт.", "error");
            break;
        default:
            swal("Неизвестная ошибка " + error.status, "Обратитесь к службу поддержки.", "error");
            break;
    }
}

function OnSaveClick(attemptId, questionId, value) {
    if (value == null) {
        value = "";
    }
    var verificationValue = document.getElementsByName('__RequestVerificationToken')[0].getAttribute('value');
    var ajax = $.ajax({
        type: "POST",
        url: "/Test/SaveAttemptAnswer/",
        data: { attemptId: attemptId, questionId: questionId, value: value, __RequestVerificationToken: verificationValue },
        error: function (error) {
            ProcessErrors(error);
        }
    });
    if (lastSave) {
        ajaxArray.push(ajax);
    }
}

function GetRadioValue(groupName) {
    var found = document.querySelector('input[name="' + groupName + '"]:checked');
    if (found != null) {
        return found.value;
    }
    else {
        return "";
    }
}

function GetInputValue(inputId) {
    return document.getElementById(inputId).value;
}

function GetCheckValue(groupName) {
    var result = "";
    var elements = document.getElementsByName(groupName);
    for (var i = 0; i < elements.length; i++) {
        if (elements[i].checked) {
            result += elements[i].value + ",";
        }
    }
    return result.slice(0, -1);
}

function SaveAll() {
    var elements = document.getElementsByName('save-button');
    for (var i = 0; i < elements.length; i++) {
        elements[i].click();
    }
    if (lastSave) {
        $.when.apply($, ajaxArray).done(function () {
            document.getElementById('main-button').click();
        });
    }
}