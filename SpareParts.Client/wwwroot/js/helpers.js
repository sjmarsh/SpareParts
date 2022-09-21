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

export async function openBlob(data) {    

    // blazor passes byte arrays to js as a Uint8Array
    // ref: https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/6.0/byte-array-interop

    const theBlob = new Blob([data.buffer], {type: 'application/pdf'});

    let anchor = document.createElement("a");
    document.body.appendChild(anchor);

    let objectUrl = window.URL.createObjectURL(theBlob);
    anchor.href = objectUrl;
    anchor.target = '_blank';
    //anchor.download = 'report.pdf';
    anchor.click();

    window.URL.revokeObjectURL(objectUrl);
}