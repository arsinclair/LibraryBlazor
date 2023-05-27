window.userJsFunctions = {
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