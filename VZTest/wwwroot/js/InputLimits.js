function OnIntInput(event) {
    let element = event.srcElement;
    element.value = element.value.replace(/\D/g, '');
}

function OnDoubleInput(event) {
    let element = event.srcElement;
    element.value = element.value.replace(/[^\.\,\d]/g, '');
}