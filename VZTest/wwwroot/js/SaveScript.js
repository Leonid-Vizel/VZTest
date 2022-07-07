let lastSave = false;
let ajaxArray = new Array();

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
    let verificationValue = document.getElementsByName('__RequestVerificationToken')[0].getAttribute('value');
    let ajax = $.ajax({
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

function SaveAll() {
    let elements = document.getElementsByName('save-button');
    for (let i = 0; i < elements.length; i++) {
        elements[i].click();
    }
    if (lastSave) {
        $.when.apply($, ajaxArray).done(function () {
            document.getElementById('main-button').click();
        });
    }
}