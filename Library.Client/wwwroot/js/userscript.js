window.userJsFunctions = {
    scrollToBottom: function (element) {
        var objDiv = document.getElementById(element);
        if (objDiv != null && objDiv.scrollTop) {
            objDiv.scrollTop = objDiv.scrollHeight;
        }
    },
    populateEntityAttributes: function (mappings) {
        if (mappings && Object.keys(mappings).length > 0) {
            for (const fieldName of Object.keys(mappings)) {
                const field = document.querySelector(`input[id="${fieldName}_control" i]`);
                if (field) {
                    field.value = mappings[fieldName];
                    field.dispatchEvent(new Event('change', { 'bubbles': true }));
                }
            }
        }
    }
};