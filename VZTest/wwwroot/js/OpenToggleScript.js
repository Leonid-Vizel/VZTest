function OpenToggle(id) {
    var verificationValue = document.getElementsByName('__RequestVerificationToken')[0].getAttribute('value');
    var open = document.getElementById("Open-" + id) == null;
    if (open) {
        swal({
            title: "Вы действительно хотите закрыть этот тест?",
            text: "Прохождение этого теста другими пользователями будет невозможно, однако сам тест удалён не будет!",
            icon: "warning",
            buttons: ["Отмена", "Закрыть"]
        })
            .then((willDelete) => {
                if (willDelete) {
                    $.ajax({
                        type: "POST",
                        url: "/Test/OpenToggle/",
                        data: { id: id, opened: open, __RequestVerificationToken: verificationValue },
                        error: function (error) {
                            ProcessErrors(error);
                        },
                        success: function () {
                            var element = document.getElementById("Close-" + id);
                            element.setAttribute('class', 'btn btn-success');
                            element.innerHTML = "<i class=\"bi bi-door-open\"></i> Открыть";
                            element.setAttribute('id', "Open-" + id);
                        }
                    });
                }
            });
    }
    else {
        swal({
            title: "Вы действительно хотите открыть этот тест?",
            text: "После открытия другие пользователи смогут проходить Ваш тест!",
            icon: "warning",
            buttons: ["Отмена", "Открыть"]
        })
            .then((willDelete) => {
                if (willDelete) {
                    $.ajax({
                        type: "POST",
                        url: "/Test/OpenToggle/",
                        data: { id: id, opened: open, __RequestVerificationToken: verificationValue },
                        error: function (error) {
                            ProcessErrors(error);
                        },
                        success: function () {
                            var element = document.getElementById("Open-" + id);
                            element.setAttribute('class', 'btn btn-warning');
                            element.innerHTML = "<i class=\"bi bi-door-closed\"></i> Закрыть";
                            element.setAttribute('id', "Close-" + id);
                        }
                    });
                }
            });
    }
}