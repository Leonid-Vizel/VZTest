function PreEditCheck(testId) {
    let verificationValue = document.getElementsByName('__RequestVerificationToken')[0].getAttribute('value');
    $.ajax({
        type: "POST",
        url: "/Test/GetTestStatus/" + testId,
        data: { id: testId, __RequestVerificationToken: verificationValue },
        error: function (error) {
            switch (error.status) {
                case 404:
                    swal("Элемент не найден!", "Перезагрузите страницу и попробуйте снова.", "error");
                    break;
                case 401:
                    swal("Авторизуйтесь!", "Для того, чтобы реактировать тест надо войти в аккаунт.", "error");
                    break;
                default:
                    swal("Неизвестная ошибка " + error.status, "Обратитесь к службу поддержки.", "error");
                    break;
            }
        },
        success: function (data) {
            if (data == 'True') {
                swal("Тест открыт!", "Для редактирования - закройте тест!", "error");
            }
            else {
                window.location.replace('/Test/Edit/' + testId);
            }
        }
    });
}