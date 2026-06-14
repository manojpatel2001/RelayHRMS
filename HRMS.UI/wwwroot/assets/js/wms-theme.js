/* ════════════════════════════════════════════════════════════════════════
   WMS THEME TOGGLE (Light / Dark)
   ─────────────────────────────────────────────────────────────────────────
   · Default mode: light.
   · Preference persists in localStorage under TWO keys so the user keeps
     their setting across sessions AND each user on the same machine has
     their own choice:
        - `wms_theme`              global fallback ('light' | 'dark')
        - `wms_theme_<userId>`     per-user override (set when user logs in)
   · Apply order on boot:
        1. per-user key (if userId is in sessionStorage)
        2. global key
        3. default 'light'
   · The toggle button is inserted into the My Account dropdown row in
     the top nav (#as-theme-toggle is the slot).  A floating toggle is
     also bound to any element with [data-wms-theme-toggle].

   Loaded VERY EARLY in the head so the theme is applied before paint,
   avoiding the brief flash of light-mode pixels on a dark-mode reload.
   ════════════════════════════════════════════════════════════════════════ */
(function (w, d) {
    'use strict';

    var STORAGE_GLOBAL = 'wms_theme';            // only used pre-login (login screen)
    var STORAGE_USER   = function (userId) { return 'wms_theme_' + userId; };

    // Read preference — strictly per-user once logged in.  This prevents
    // User A's choice from leaking to User B on a shared device.
    //
    //   · Authenticated  → read `wms_theme_<userId>` only.  If not set yet,
    //                      DEFAULT TO LIGHT for that user (matches the
    //                      "each user starts fresh" expectation).
    //   · Pre-auth (login page) → fall back to the global key so the login
    //                      screen remembers your last visible theme.
    function readPref() {
        try {
            var uid = (w.sessionStorage && w.sessionStorage.getItem('userId')) || '';
            if (uid) {
                var perUser = w.localStorage.getItem(STORAGE_USER(uid));
                return (perUser === 'dark') ? 'dark' : 'light';
            }
            var g = w.localStorage.getItem(STORAGE_GLOBAL);
            return (g === 'dark') ? 'dark' : 'light';
        } catch (_) { }
        return 'light';
    }

    // Write preference — only writes to the key that matters for the
    // current auth state.  Authenticated users write ONLY to their own
    // per-user key, so the toggle never affects another user.
    function writePref(mode) {
        try {
            var uid = (w.sessionStorage && w.sessionStorage.getItem('userId')) || '';
            if (uid) {
                w.localStorage.setItem(STORAGE_USER(uid), mode);
            } else {
                w.localStorage.setItem(STORAGE_GLOBAL, mode);
            }
        } catch (_) { }
    }

    function apply(mode) {
        d.documentElement.setAttribute('data-theme', mode);
        d.documentElement.style.colorScheme = mode === 'dark' ? 'dark' : 'light';
        // Notify listeners (e.g. charts can re-render with dark palette).
        try { w.dispatchEvent(new CustomEvent('wms-theme-change', { detail: { theme: mode } })); } catch (_) { }
        updateButtonState();
    }

    function toggle() {
        var current = d.documentElement.getAttribute('data-theme') || 'light';
        var next = current === 'dark' ? 'light' : 'dark';
        writePref(next);
        apply(next);
        return next;
    }

    function updateButtonState() {
        var mode = d.documentElement.getAttribute('data-theme') || 'light';
        var label = mode === 'dark' ? '☾ Dark' : '☀ Light';
        // Update every theme toggle button on the page (slot + any data-attr opt-ins).
        var btns = d.querySelectorAll('.wms-theme-toggle, [data-wms-theme-toggle]');
        for (var i = 0; i < btns.length; i++) {
            var b = btns[i];
            b.setAttribute('aria-label', mode === 'dark' ? 'Switch to light mode' : 'Switch to dark mode');
            b.setAttribute('title',      mode === 'dark' ? 'Switch to light mode' : 'Switch to dark mode');
            // Only re-render the inner label when the button uses our default markup.
            if (b.dataset.wmsAuto !== 'no') {
                var icon = mode === 'dark' ? 'bx bx-sun' : 'bx bx-moon';
                b.innerHTML = '<i class="' + icon + '"></i>' + label;
            }
        }
    }

    function ensureSlotButton() {
        // If the layout has an explicit slot, fill it in.  Otherwise insert
        // the toggle into the right side of the top nav, before the "My Account"
        // dropdown.
        var slot = d.getElementById('as-theme-toggle');
        if (slot && !slot.querySelector('button')) {
            var btn = d.createElement('button');
            btn.type = 'button';
            btn.className = 'wms-theme-toggle';
            btn.addEventListener('click', toggle);
            slot.appendChild(btn);
        } else if (!slot) {
            // Fallback — append before My Account.
            var anchor = d.getElementById('as-myAccountBtn');
            if (anchor && anchor.parentNode) {
                if (!anchor.parentNode.previousElementSibling ||
                    !anchor.parentNode.previousElementSibling.classList ||
                    !anchor.parentNode.previousElementSibling.classList.contains('wms-theme-toggle')) {
                    var btn2 = d.createElement('button');
                    btn2.type = 'button';
                    btn2.className = 'wms-theme-toggle';
                    btn2.style.marginRight = '8px';
                    btn2.addEventListener('click', toggle);
                    anchor.parentNode.parentNode.insertBefore(btn2, anchor.parentNode);
                }
            }
        }
        // Any element opted-in via attribute also wires up.
        var attrBtns = d.querySelectorAll('[data-wms-theme-toggle]');
        for (var i = 0; i < attrBtns.length; i++) {
            var b = attrBtns[i];
            if (b.dataset.wmsBound) continue;
            b.dataset.wmsBound = '1';
            b.addEventListener('click', toggle);
        }
        updateButtonState();
    }

    // Apply the saved theme as early as possible (before first paint when
    // the script is referenced in <head>).
    apply(readPref());

    // After DOM is ready, place / wire the toggle buttons.
    if (d.readyState === 'loading') {
        d.addEventListener('DOMContentLoaded', ensureSlotButton);
    } else {
        ensureSlotButton();
    }

    // Re-apply when sessionStorage.userId becomes available (login completes).
    // We poll briefly because Login flow sets userId asynchronously.
    var attempts = 0;
    var lastUid = '';
    var iv = setInterval(function () {
        attempts++;
        var uid = (w.sessionStorage && w.sessionStorage.getItem('userId')) || '';
        if (uid && uid !== lastUid) {
            lastUid = uid;
            apply(readPref());
        }
        if (attempts > 60) clearInterval(iv);   // stop after ~30s
    }, 500);

    // Expose helpers for inline onclick handlers or other scripts.
    w.WMSTheme = {
        toggle:  toggle,
        apply:   apply,
        get:     function () { return d.documentElement.getAttribute('data-theme') || 'light'; }
    };
})(window, document);
