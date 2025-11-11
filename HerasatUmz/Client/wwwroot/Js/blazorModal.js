
window.blazorModal = {
    setupEsc: function (dotNetHelper) {
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') {
                dotNetHelper.invokeMethodAsync('CloseFromJs');
            }
        });
    }
};