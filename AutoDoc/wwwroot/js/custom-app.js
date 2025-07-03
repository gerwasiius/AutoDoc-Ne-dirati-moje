window.blazorDownloadFile = (fileName, contentType, byteArray) => {
    const blob = new Blob([new Uint8Array(byteArray)], { type: contentType });
    const url = URL.createObjectURL(blob);
    const anchor = document.createElement("a");
    anchor.href = url;
    anchor.download = fileName;
    anchor.click();
    URL.revokeObjectURL(url);
};