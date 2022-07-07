function OnIntInput(event) {
    var element = event.srcElement;
    element.value = element.value.replace(/\D/g, '');
}

function OnDoubleInput(event) {
    var element = event.srcElement;
    element.value = element.value.replace(/[^\.\,\d]/g, '');
}