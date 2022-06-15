function SetClipboard(content) {
    navigator.clipboard.writeText(content);
    document.getElementById('copyIcon').setAttribute('class', 'bi bi-check2');
    var interval = setInterval(function () { SetIconBack() }, 2500);
}

function SetIconBack() {
    document.getElementById('copyIcon').setAttribute('class', 'bi bi-clipboard-check');
}