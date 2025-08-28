function initTrimInputHandler() {
    const selector = '[data-trim-input], [data-integer-only], [data-integer-with-space], [data-decimal], [data-letters-only], [data-uppercase-only]';

    // Trim spaces on blur
    $(document).on('blur', selector, function () {
        if (this.value !== undefined) {
            this.value = this.value.trim();
        } else {
            $(this).text($(this).text().trim());
        }
    });

    // Input filtering
    $(document).on('input', selector, function () {
        const $el = $(this);
        let val = this.value !== undefined ? this.value : $el.text();

        if ($el.is('[data-integer-only]')) {
            val = val.replace(/\D/g, '');
        }
        else if ($el.is('[data-integer-with-space]')) {
            val = val.replace(/[^0-9\s]/g, '');
        }
        else if ($el.is('[data-decimal]')) {
            val = val.replace(/[^0-9.]/g, '');
            const parts = val.split('.');
            if (parts.length > 2) {
                val = parts.shift() + '.' + parts.join('');
            }
        }
        else if ($el.is('[data-letters-only]')) {
            val = val.replace(/[^a-zA-Z\s]/g, '');
        }
        else if ($el.is('[data-uppercase-only]')) {
            // Allow only uppercase letters and spaces, convert to uppercase
            val = val.replace(/[^A-Za-z\s]/g, '').toUpperCase();
        }

        // Set value or text accordingly
        if (this.value !== undefined) {
            this.value = val;
        } else {
            $el.text(val);
        }
    });

    // Prevent leading space
    $(document).on('keydown', selector, function (e) {
        const $el = $(this);
        const val = this.value !== undefined ? this.value : $el.text();
        const cursorPos = this.selectionStart || 0;

        if (e.key === " " && cursorPos === 0) {
            e.preventDefault();
        }
    });
}

$(document).ready(function () {
    initTrimInputHandler();
});
