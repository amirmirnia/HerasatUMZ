// wwwroot/js/webcam.js

window.startWebcam = async (videoElementId) => {
    const video = document.getElementById(videoElementId);
    if (!video) return false;

    try {
        const stream = await navigator.mediaDevices.getUserMedia({ video: true });
        video.srcObject = stream;
        video.play();
        return true;
    } catch (err) {
        console.error("Error accessing webcam: ", err);
        return false;
    }
};

window.stopWebcam = (videoElementId) => {
    const video = document.getElementById(videoElementId);
    if (video && video.srcObject) {
        const tracks = video.srcObject.getTracks();
        tracks.forEach(track => track.stop());
        video.srcObject = null;
    }
};

window.takePhoto = (videoElementId, canvasElementId) => {
    const video = document.getElementById(videoElementId);
    const canvas = document.getElementById(canvasElementId);
    if (!video || !canvas || video.videoWidth === 0) return null;

    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    const context = canvas.getContext('2d');
    context.drawImage(video, 0, 0, canvas.width, canvas.height);

    return canvas.toDataURL('image/jpeg', 0.8); // کیفیت 80%
};