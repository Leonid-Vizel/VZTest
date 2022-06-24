function PublicToggle(id) {
    var verificationValue = document.getElementsByName('__RequestVerificationToken')[0].getAttribute('value');
    var public = document.getElementById("Public-" + id) == null;
    if (public) {
        swal({
            title: "Вы действительно хотите сделать этот тест приватным?",
            text: "Тест не будет отображаться в публичных списках, но его можно будет найти по ID, если он открытый!",
            icon: "warning",
            buttons: ["Отмена", "Применить"]
        })
            .then((willDelete) => {
                if (willDelete) {
                    $.ajax({
                        type: "POST",
                        url: "/Test/PublicToggle/",
                        data: { id: id, isPublic: public, __RequestVerificationToken: verificationValue },
                        error: function (error) {
                            ProcessErrors(error);
                        },
                        success: function () {
                            var element = document.getElementById("Private-" + id);
                            element.setAttribute('class', 'btn btn-success');
                            element.innerHTML = "<i class=\"bi bi-door-open\"></i> Опубликовать";
                            element.setAttribute('id', "Public-" + id);
                        }
                    });
                }
            });
    }
    else {
        swal({
            title: "Вы действительно хотите сделать этот тест публичным?",
            text: "Этот тест будет показан в открытых списках!",
            icon: "warning",
            buttons: ["Отмена", "Применить"]
        })
            .then((willDelete) => {
                if (willDelete) {
                    $.ajax({
                        type: "POST",
                        url: "/Test/PublicToggle/",
                        data: { id: id, isPublic: public, __RequestVerificationToken: verificationValue },
                        error: function (error) {
                            ProcessErrors(error);
                        },
                        success: function () {
                            var element = document.getElementById("Public-" + id);
                            element.setAttribute('class', 'btn btn-warning');
                            element.innerHTML = "<i class=\"bi bi-door-closed\"></i> Приватизировать";
                            element.setAttribute('id', "Private-" + id);
                        }
                    });
                }
            });
    }
}