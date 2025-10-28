function initTrimInputHandler() {
    const selector = '[data-trim-input], [data-integer-only], [data-integer-with-space], [data-decimal], [data-letters-only], [data-uppercase-only],[data-decimal-two]';

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
            // Handle leading decimal point (e.g., ".5" → "0.5")
            if (val.startsWith('.')) {
                val = '0' + val;
            }

            const parts = val.split('.');

            if (parts.length > 2) {
                val = parts[0] + '.' + parts.slice(1).join('').substring(0, 2);
            }
            else if (parts.length === 2) {
                val = parts[0] + '.' + parts[1].substring(0, 2);
            }

           
        }
        else if ($el.is('[data-decimal-two]')) {
            debugger
             val = $el.val();
            const maxDigits = 2; // Enforce 2 digits before and after decimal

            val = val.replace(/[^0-9.]/g, '');

            if (val.startsWith('.')) {
                val = '0' + val;
            }

            const parts = val.split('.');

            if (parts[0].length > maxDigits) {
                parts[0] = parts[0].substring(0, maxDigits);
            }

            if (parts.length > 1) {
                parts[1] = parts[1].substring(0, maxDigits);
                val = parts.join('.');
            } else {
                val = parts[0];
            }

            $el.val(val);
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
