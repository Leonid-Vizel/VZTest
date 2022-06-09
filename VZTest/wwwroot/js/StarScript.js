function StarToggle(id) {
    var verificationValue = document.getElementsByName('__RequestVerificationToken')[0].getAttribute('value');
    var starred = document.getElementById("star-toggle-" + id) == null;
    $.ajax({
        type: "POST",
        url: "/Test/StarToggle/",
        data: { id: id, starred: starred, __RequestVerificationToken: verificationValue },
        error: function (error) {
            switch (error.status) {
                case 403:
                    swal("Не доступно!", "Пользователь не может отметить свой же тест!", "error");
                    break;
                case 404:
                    swal("Элемент не найден!", "Попробуйте снова.", "error");
                    break;
                case 401:
                    swal("Авторизуйтесь!", "Для того, чтобы отметить тест надо войти в аккаунт.", "error");
                    break;
                default:
                    swal("Неизвестная ошибка " + error.status, "Обратитесь к службу поддержки.", "error");
                    break;
            }
        },
        success: function (data) {
            if (starred) {
                var element = document.getElementById("unstar-toggle-" + id);
                element.innerHTML = "<small>" + data + " <i class=\"bi bi-star\"></i></small>";
                element.setAttribute('id', "star-toggle-" + id);
            }
            else {
                var element = document.getElementById("star-toggle-" + id);
                element.innerHTML = "<small>" + data + " <i class=\"bi bi-star-fill\"></i></small>";
                element.setAttribute('id', "unstar-toggle-" + id);
            }
        }
    });
}