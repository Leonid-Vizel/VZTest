function countDown() {
    let nowDate = new Date();
    let element = document.getElementById('timer');
    let achiveDate = Date.parse(element.getAttribute('data-time'));
    let result = (achiveDate - nowDate) + 1000;
    if (result < 0) {
        if (element != null) {
            element.remove();
        }
        let form = document.getElementById('form');
        form.removeAttribute('hidden');
        let elementLabel = document.getElementById('timer-label');
        if (elementLabel != null) {
            elementLabel.remove();
        }
        let recommendElement = document.getElementById('recommend-label');
        if (recommendElement != null) {
            recommendElement.remove();
        }
        let topLabel = document.getElementById('top-label');
        if (topLabel != null) {
            topLabel.innerHTML = 'Время пришло!';
            let label = document.createElement('h5');
            label.setAttribute('class', 'text-center');
            label.innerHTML = 'Чтобы пройти тест просто нажмите на эту кнопку';
            topLabel.insertAdjacentElement("afterend", label);
        }
        return;
    }
    let seconds = Math.floor((result / 1000) % 60);
    let minutes = Math.floor((result / 1000 / 60) % 60);
    let hours = Math.floor((result / 1000 / 60 / 60) % 24);
    let days = Math.floor(result / 1000 / 60 / 60 / 24);
    if (seconds < 10) seconds = '0' + seconds;
    if (minutes < 10) minutes = '0' + minutes;
    if (hours < 10) hours = '0' + hours;
    element.innerHTML = days + ':' + hours + ':' + minutes + ':' + seconds;
    setTimeout(countDown, 1000);
}
window.onload = function () {
    countDown();
}