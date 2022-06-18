function SetNumberMessage() {
    console.log('Am I a joke to you?');
    jQuery.extend(jQuery.validator.messages, {
        number: "Значение должно быть числом!"
    });
}

window.onload = SetNumberMessage;