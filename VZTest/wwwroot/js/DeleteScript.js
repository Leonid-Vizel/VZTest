function Delete(deleteId) {
    var verificationValue = document.getElementsByName('__RequestVerificationToken')[0].getAttribute('value');
    swal({
        title: "Вы действительно хотите удалить этот тест?",
        text: "После удаления теста воостановить его будет невозможно!",
        icon: "warning",
        buttons: ["Отмена", "Удалить"],
        dangerMode: true
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    type: "POST",
                    url: "/Test/Delete/",
                    data: { id: deleteId, __RequestVerificationToken: verificationValue },
                    error: function (error) {
                        ProcessErrors(error);
                    },
                    success: function () {
                        var element = document.getElementById("test-container-" + deleteId);
                        element.remove();
                        element = document.getElementById("test-separator-" + deleteId);
                        element.remove();
                        var testContainersCount = document.getElementsByName("test-container").length;
                        if (testContainersCount == 0) {
                            element = document.getElementById('top-separator');
                            if (element != null) {
                                var h5 = document.createElement('h5');
                                h5.innerHTML = 'Вы пока не создали ни одного теста.';
                                element.insertAdjacentElement('afterend', h5);
                            }
                        }
                    }
                });
            }
        });
}