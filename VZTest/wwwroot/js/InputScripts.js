function GetRadioValue(groupName) {
    var found = document.querySelector('input[name="' + groupName + '"]:checked');
    if (found != null) {
        return found.value;
    }
    else {
        return "";
    }
}

function GetInputValue(inputId) {
    return document.getElementById(inputId).value;
}

function GetCheckValue(groupName) {
    var result = "";
    var elements = document.getElementsByName(groupName);
    for (var i = 0; i < elements.length; i++) {
        if (elements[i].checked) {
            result += elements[i].value + ",";
        }
    }
    return result.slice(0, -1);
}