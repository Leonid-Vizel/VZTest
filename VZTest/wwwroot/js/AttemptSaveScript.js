let flag = true;

function AskEnd(e) {
    if (!flag) {
        return;
    }
    e.preventDefault();
    swal({
        title: "Вы уверены?",
        text: "После завершения попытки вы больше не сможете редактировать свои ответы!",
        icon: "warning",
        buttons: true
    })
        .then((end) => {
            if (end) {
                flag = false;
                lastSave = true;
                document.getElementById('save-all-button').click();
            }
        });
}