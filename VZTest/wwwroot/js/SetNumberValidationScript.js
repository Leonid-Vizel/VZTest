function SetNumberMessage() {
    jQuery.extend(jQuery.validator.messages, {
        number: "Значение должно быть числом!"
    });
}

window.onload = SetNumberMessage;