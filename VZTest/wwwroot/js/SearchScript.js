function Search() {
    let verificationValue = document.getElementsByName('__RequestVerificationToken')[0].getAttribute('value');
    let id = document.getElementById('SearchId').value;
    $.ajax({
        type: "POST",
        url: "/Test/SearchNoPassword/",
        data: { id: id, __RequestVerificationToken: verificationValue },
        error: function (error) {
            if (error.status == 404) {
                swal("Тест не найден!", "Попробуйте другой номер или свяжитесь с автором теста.", "error");
            }
            else if (error.status == 401) {
                swal("Авторизуйтесь!", "Войдите в свой ааканут перед тем, как проходить тесты.", "error");
            }
        },
        success: function (data) {
            if (data == 'Redirect') {
                window.location.replace("/Test/Preview/" + id);
            }
            else if (data == 'Closed') {
                swal("Тест закрыт!", "Тест закрыт для общего доступа, только автор может его видеть. Свяжитесь с автором.", "error");
            }
            else if (data == 'NeedPassword') {
                swal({
                    text: 'Введите пароль для данного теста',
                    content: {
                        element: "input",
                        attributes: {
                            placeholder: "Пароль",
                            type: "password",
                        },
                    },
                    button: {
                        text: "Открыть",
                        closeModal: false
                    }
                })
                    .then(password => {
                        $.ajax({
                            type: "POST",
                            url: "/Test/SearchPassword/",
                            data: { id: id, password: password, __RequestVerificationToken: verificationValue },
                            error: function (error) {
                                swal.stopLoading();
                                swal.close();
                                HandleError(error);
                            },
                            success: function (data) {
                                swal.stopLoading();
                                swal.close();
                                if (data == 'WrongPassword') {
                                    swal("Неверный пароль!", "Попробуйте другой пароль.", "error");
                                }
                                else if (data == 'RedirectOnlyId') {
                                    window.location.replace("/Test/Preview/" + id);
                                }
                                else if (data.startsWith('RedirectHash')) {
                                    window.location.replace("/Test/Preview/" + id + "?passwordHash=" + data.replace('RedirectHash:', ''));
                                }
                                else {
                                    swal("Ошибка!", "Ошибка поиска попробуйте позже", "error");
                                }
                            }
                        });
                    });
            }
            else {
                swal("Ошибка!", "Ошибка поиска попробуйте позже", "error");
            }
        }
    });
}