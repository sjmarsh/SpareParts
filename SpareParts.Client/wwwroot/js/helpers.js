export function setFocus(elementId) {
    var el = document.getElementById(elementId);
    if (el) {
        el.focus();
    }
}

export function scrollIntoView(elementId) {
    var el = document.getElementById(elementId);
    if (el) {
        el.scrollIntoView();
    }
}

export function selectTab(elementId) {
    setFocus(elementId);
    scrollIntoView(elementId);
}