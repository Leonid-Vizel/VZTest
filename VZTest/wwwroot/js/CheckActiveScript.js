let flag = true;

function CheckActive(e, id) {
    if (!flag) {
        return;
    }
    e.preventDefault();
    let verificationValue = document.getElementsByName('__RequestVerificationToken')[0].getAttribute('value');
    $.ajax({
        type: "POST",
        url: "/Test/CheckActiveAttempts/",
        data: { id: id, __RequestVerificationToken: verificationValue },
        error: function (error) {
            switch (error.status) {
                case 401:
                    swal("Авторизуйтесь!", "Для того, чтобы пройти тес надо авторизоваться", "error");
                    break;
                case 404:
                    swal("Элемент не найден!", "Попробуйте снова.", "error");
                    break;
            }
        },
        success: function (data) {
            switch (data) {
                case 'Active':
                    swal("Ошибка!", "У Вас уже есть активные попытки в этом тесте!", "error");
                    break;
                case 'Redirect':
                    flag = false;
                    document.getElementById('main-button').click();
                    break;
            }
        }
    });
}