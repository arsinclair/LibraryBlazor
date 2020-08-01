window.userJsFunctions = {
    scrollToBottom: function (element) {
        var objDiv = document.getElementById(element);
        if (objDiv != null && objDiv.scrollTop) {
            objDiv.scrollTop = objDiv.scrollHeight;
        }
    }
};