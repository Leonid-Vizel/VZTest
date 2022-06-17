function DeleteAttempt(attemptId) {
    var verificationValue = document.getElementsByName('__RequestVerificationToken')[0].getAttribute('value');
    swal({
        title: "Вы действительно хотите удалить эту попытку?",
        text: "После удаления попытки воостановить её будет невозможно!",
        icon: "warning",
        buttons: ["Отмена", "Удалить"],
        dangerMode: true
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    type: "POST",
                    url: "/Test/DeleteAttempt/",
                    data: { id: attemptId, __RequestVerificationToken: verificationValue },
                    error: function (error) {
                        ProcessErrors(error);
                    },
                    success: function () {
                        var element = document.getElementById("circle-" + attemptId);
                        element.remove();
                        element = document.getElementById("separator-" + attemptId);
                        element.remove();
                        var separatorCount = document.getElementsByName("separator").length;
                        if (separatorCount == 0) {
                            element = document.getElementById('top-separator');
                            if (element != null) {
                                var h5 = document.createElement('h5');
                                h5.innerHTML = 'Здесь пока ничего нет, но надеемся, что скоро появится ;)';
                                element.insertAdjacentElement('afterend', h5);
                            }
                        }
                    }
                });
            }
        });
}