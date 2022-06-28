function OnInput(event) {
    var element = event.srcElement;
    element.value = element.value.replace(/\D/g, '');
}