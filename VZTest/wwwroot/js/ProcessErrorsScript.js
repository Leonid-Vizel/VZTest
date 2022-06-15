function ProcessErrors(error) {
    switch (error.status) {
        case 403:
            swal("Недоступно!", "У Вас нет доступа к этому действию!", "error");
            break;
        case 404:
            swal("Элемент не найден!", "Перезагрузите страницу и попробуйте снова.", "error");
            break;
        case 401:
            swal("Авторизуйтесь!", "Для того, чтобы отметить тест надо войти в аккаунт.", "error");
            break;
        default:
            swal("Неизвестная ошибка " + error.status, "Обратитесь к службу поддержки.", "error");
            break;
    }
}