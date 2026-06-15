/* ════════════════════════════════════════════════════════════════════════
   HRMS Notify  —  Toast + Delete-Confirm system  v1.0

   Public API (window globals):
     hrmsNotify.success(msg)     — green toast
     hrmsNotify.error(msg)       — red toast
     hrmsNotify.warning(msg)     — orange toast
     hrmsNotify.info(msg)        — blue toast

     hrmsConfirm.delete({ onConfirm: function(done){...} })
       Opens the HRMS delete-confirmation modal.
       onConfirm receives a done() callback — call it when your AJAX
       completes (success OR error) to close the modal and reset the button.

   Legacy shims (backward-compatible — existing pages keep working):
     round_success_noti(msg)
     round_error_noti(msg)
     round_warning_noti(msg)

   Deduplication:
     Same type + message within 800 ms is silently dropped — prevents
     the double-toast bug that can occur when both a manual call and an
     auto-wired callback fire for the same operation.
   ════════════════════════════════════════════════════════════════════════ */
(function (w) {
    'use strict';

    /* ══════════════════════════════════════════════════════════════════════
       TOAST SYSTEM
    ══════════════════════════════════════════════════════════════════════ */

    var _wrap  = null;   /* lazy-created container element */
    var _dedup = {};     /* dedup store: "type\0msg" → timestamp */

    function _container() {
        if (!_wrap || !document.body.contains(_wrap)) {
            _wrap = document.getElementById('hrms-toast-wrap');
            if (!_wrap) {
                _wrap = document.createElement('div');
                _wrap.id = 'hrms-toast-wrap';
                _wrap.className = 'hrms-toast-wrap';
                document.body.appendChild(_wrap);
            }
        }
        return _wrap;
    }

    var SVG = {
        success: '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" viewBox="0 0 16 16"><path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/></svg>',
        error:   '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" viewBox="0 0 16 16"><path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM5.354 4.646a.5.5 0 1 0-.708.708L7.293 8l-2.647 2.646a.5.5 0 0 0 .708.708L8 8.707l2.646 2.647a.5.5 0 0 0 .708-.708L8.707 8l2.647-2.646a.5.5 0 0 0-.708-.708L8 7.293z"/></svg>',
        warning: '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" viewBox="0 0 16 16"><path d="M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z"/></svg>',
        info:    '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" viewBox="0 0 16 16"><path d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16zm.93-9.412-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2z"/></svg>',
        close:   '<svg xmlns="http://www.w3.org/2000/svg" width="13" height="13" fill="currentColor" viewBox="0 0 16 16"><path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"/></svg>'
    };

    var TITLES = { success: 'Success', error: 'Error', warning: 'Warning', info: 'Information' };

    function _esc(s) {
        return String(s || '').replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;').replace(/"/g,'&quot;');
    }

    function _showToast(type, msg, duration) {
        /* ── Dedup: drop identical type+msg within 800 ms ── */
        var key = type + '\x00' + (msg || '');
        if (_dedup[key] && Date.now() - _dedup[key] < 800) return;
        _dedup[key] = Date.now();

        duration = duration || 4000;

        var el = document.createElement('div');
        el.className = 'hrms-toast hrms-toast-' + type;
        el.setAttribute('role', 'alert');
        el.innerHTML =
            '<div class="hrms-toast-icon-col">' +
                '<span class="hrms-toast-icon-dot">' + (SVG[type] || '') + '</span>' +
            '</div>' +
            '<div class="hrms-toast-body">' +
                '<p class="hrms-toast-title">' + (TITLES[type] || type) + '</p>' +
                '<p class="hrms-toast-msg">'   + _esc(msg) + '</p>' +
            '</div>' +
            '<button type="button" class="hrms-toast-close" aria-label="Close">' + SVG.close + '</button>' +
            '<span class="hrms-toast-bar" style="animation-duration:' + duration + 'ms"></span>';

        _container().appendChild(el);

        /* Trigger enter animation after paint */
        requestAnimationFrame(function () {
            requestAnimationFrame(function () { el.classList.add('hrms-toast-in'); });
        });

        var timer = setTimeout(function () { _dismiss(el); }, duration);

        el.querySelector('.hrms-toast-close').addEventListener('click', function () {
            clearTimeout(timer);
            _dismiss(el);
        });
    }

    function _dismiss(el) {
        el.classList.remove('hrms-toast-in');
        el.classList.add('hrms-toast-out');
        setTimeout(function () { if (el.parentNode) el.parentNode.removeChild(el); }, 360);
    }

    /* ══════════════════════════════════════════════════════════════════════
       DELETE CONFIRM MODAL
    ══════════════════════════════════════════════════════════════════════ */

    var CONFIRM_ID   = 'hrmsDeleteConfirmModal';
    var _cModal      = null;   /* Bootstrap Modal instance */
    var _cEl         = null;   /* DOM element */

    var TRASH_ICON =
        '<svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" fill="currentColor" viewBox="0 0 16 16">' +
        '<path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2' +
        'a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7' +
        'a.5.5 0 0 1 .5-.5M8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5m3 .5v7a.5.5 0 0 1-1 0v-7' +
        'a.5.5 0 0 1 1 0"/></svg>';

    function _buildModal() {
        if (document.getElementById(CONFIRM_ID)) return;
        var div = document.createElement('div');
        div.innerHTML =
            '<div class="modal fade" id="' + CONFIRM_ID + '" tabindex="-1" aria-hidden="true" ' +
                 'aria-labelledby="hrmsConfirmLbl" data-bs-backdrop="static" data-bs-keyboard="false">' +
                '<div class="modal-dialog modal-dialog-centered hrms-confirm-dialog">' +
                    '<div class="modal-content hrms-confirm-content">' +

                        '<div class="hrms-confirm-icon-wrap">' +
                            '<div class="hrms-confirm-icon-circle">' + TRASH_ICON + '</div>' +
                        '</div>' +

                        '<div class="hrms-confirm-body">' +
                            '<h5 class="hrms-confirm-title" id="hrmsConfirmLbl">Delete Record</h5>' +
                            '<p class="hrms-confirm-desc">Are you sure you want to delete this record?</p>' +
                            '<p class="hrms-confirm-sub">This action cannot be undone.</p>' +
                        '</div>' +

                        '<div class="hrms-confirm-footer">' +
                            '<button type="button" class="btn btn-secondary btn-sm" id="hrmsConfirmNo">No</button>' +
                            '<button type="button" ' +
                                    'class="btn btn-danger btn-sm d-inline-flex align-items-center gap-1" ' +
                                    'id="hrmsConfirmYes">' +
                                '<span class="btn-spinner-icon" id="hrmsConfirmSpinner"></span>' +
                                '<span class="btn-label-text">Yes, Delete</span>' +
                            '</button>' +
                        '</div>' +

                    '</div>' +
                '</div>' +
            '</div>';
        document.body.appendChild(div.firstElementChild);
    }

    function _getModal() {
        _buildModal();
        _cEl = document.getElementById(CONFIRM_ID);
        if (!_cModal && w.bootstrap && bootstrap.Modal) {
            _cModal = new bootstrap.Modal(_cEl, { backdrop: 'static', keyboard: false });

            /* Elevate the backdrop that Bootstrap injects for THIS modal.
               We cannot use a CSS selector like .modal-backdrop:last-of-type
               because it would also elevate every other modal's backdrop,
               putting it above that modal and blocking all clicks.
               JS approach: tag the backdrop immediately after show fires. */
            _cEl.addEventListener('show.bs.modal', function () {
                /* Bootstrap appends the backdrop synchronously before 'show' fires */
                setTimeout(function () {
                    var bds = document.querySelectorAll('.modal-backdrop');
                    /* Tag the last backdrop (the one Bootstrap just created) */
                    var last = bds[bds.length - 1];
                    if (last && !last.dataset.hrmsOwned) {
                        last.dataset.hrmsOwned = 'confirm';
                        last.style.zIndex = '1000149';
                    }
                }, 0);
            });

            /* Reset backdrop z-index when confirm modal closes */
            _cEl.addEventListener('hidden.bs.modal', function () {
                var owned = document.querySelector('.modal-backdrop[data-hrms-owned="confirm"]');
                if (owned) {
                    owned.removeAttribute('data-hrms-owned');
                    owned.style.zIndex = '';
                }
            });
        }
        return _cEl;
    }

    function _resetYesBtn() {
        var btn = document.getElementById('hrmsConfirmYes');
        if (!btn) return;
        btn.disabled = false;
        var sp = document.getElementById('hrmsConfirmSpinner');
        if (sp) sp.style.display = 'none';
        var lbl = btn.querySelector('.btn-label-text');
        if (lbl) lbl.textContent = 'Yes, Delete';
    }

    function _confirmDelete(opts) {
        var el = _getModal();
        _resetYesBtn();

        /* Replace buttons with fresh clones to remove any prior listeners */
        function _fresh(id) {
            var old = document.getElementById(id);
            var neu = old.cloneNode(true);
            old.parentNode.replaceChild(neu, old);
            return neu;
        }
        var noBtn  = _fresh('hrmsConfirmNo');
        var yesBtn = _fresh('hrmsConfirmYes');

        noBtn.addEventListener('click', function () {
            if (_cModal) _cModal.hide();
        });

        yesBtn.addEventListener('click', function () {
            /* Disable + spinner */
            yesBtn.disabled = true;
            var sp = yesBtn.querySelector('.btn-spinner-icon');
            if (sp) sp.style.display = 'inline-block';
            var lbl = yesBtn.querySelector('.btn-label-text');
            if (lbl) lbl.textContent = 'Deleting...';

            /* done() — page AJAX handler calls this when complete */
            function done() {
                if (_cModal) _cModal.hide();
                /* hidden.bs.modal will call _resetYesBtn */
            }

            if (typeof opts.onConfirm === 'function') opts.onConfirm(done);
            else done();
        });

        /* Reset button state after modal fully hides */
        el.removeEventListener('hidden.bs.modal', _resetYesBtn);
        el.addEventListener('hidden.bs.modal', _resetYesBtn, { once: true });

        if (_cModal) _cModal.show();
    }

    /* ══════════════════════════════════════════════════════════════════════
       PUBLIC API
    ══════════════════════════════════════════════════════════════════════ */
    w.hrmsNotify = {
        success: function (msg) { _showToast('success', msg || 'Record saved successfully.'); },
        error:   function (msg) { _showToast('error',   msg || 'Something went wrong. Please try again.'); },
        warning: function (msg) { _showToast('warning', msg || 'Please fill all required fields.'); },
        info:    function (msg) { _showToast('info',    msg || ''); }
    };

    w.hrmsConfirm = {
        delete: function (opts) { _confirmDelete(opts || {}); }
    };

    /* ══════════════════════════════════════════════════════════════════════
       LEGACY SHIMS — keep existing round_*_noti() calls working
       These override the Lobibox wrappers from notification-custom-script.js
       (loaded earlier).  Same signature, same global name, just routes
       through our new toast renderer instead of Lobibox.
    ══════════════════════════════════════════════════════════════════════ */
    w.round_success_noti = function (msg) { _showToast('success', msg || 'Record saved successfully.'); };
    w.round_error_noti   = function (msg) { _showToast('error',   msg || 'Something went wrong. Please try again.'); };
    w.round_warning_noti = function (msg) { _showToast('warning', msg || 'Please fill all required fields.'); };

})(window);
