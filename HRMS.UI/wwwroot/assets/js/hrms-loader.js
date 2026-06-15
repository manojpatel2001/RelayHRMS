/* ════════════════════════════════════════════════════════════════════════
   HRMS Loader Manager  —  WMS dot-spinner, centralized  v2.0

   Public API (window globals):
     showLoader()          increment counter, show WMS spinner if needed
     hideLoader()          decrement counter, hide when counter reaches 0
     forceHideLoader()     reset counter to 0, always hide (use in catch blocks)
     showPageLoader()      alias → showLoader
     hidePageLoader()      alias → hideLoader

   jQuery AJAX auto-wiring:
     Every $.ajax / $.get / $.post call automatically triggers show/hide.
     To skip the loader for a background/polling request add the option:
       $.ajax({ ..., _silentLoader: true })

   Old-loader auto-transform:
     On DOMContentLoaded scans for any div[id*="loader"] that contains the
     old loder.png image and replaces it with the WMS dot-spinner.
     A MutationObserver proxy is attached so existing page JS that calls
     $('#grid-loader').css('display','flex') / .hide() still works—
     those actions are intercepted and redirected to the global counter.

   Page navigation:
     Shows loader on same-origin anchor clicks.

   Fade:
     The main #loader fades in/out via opacity transition. A generation
     counter ensures rapid show→hide→show sequences never get stuck.
   ════════════════════════════════════════════════════════════════════════ */
(function (w) {
    'use strict';

    /* ── Counter state ───────────────────────────────────────────── */
    var _busy        = 0;
    var _silentBusy  = 0;
    var _safetyTimer = null;
    var _hideGen     = 0;       // generation counter for fade-out race guard
    var SAFETY_MS    = 30000;

    /* ── DOM element ─────────────────────────────────────────────── */
    function el() { return document.getElementById('loader'); }

    /* ── Raw show / hide with opacity fade ───────────────────────── */
    function _show() {
        clearTimeout(_safetyTimer);
        _hideGen++;                      // cancel any pending fade-out timeout
        var e = el();
        if (e) {
            e.style.display = 'flex';
            /* Double rAF: first one waits for display change to apply,
               second waits for the repaint before starting the transition */
            requestAnimationFrame(function () {
                requestAnimationFrame(function () {
                    e.style.opacity = '1';
                });
            });
        }
        _safetyTimer = setTimeout(_forceHide, SAFETY_MS);
    }

    function _hide() {
        clearTimeout(_safetyTimer);
        var e = el();
        if (!e) return;
        e.style.opacity = '0';
        var gen = _hideGen;
        setTimeout(function () {
            if (_hideGen === gen) {     // no new _show() was called meanwhile
                e.style.display = 'none';
            }
        }, 240);                        // match CSS transition-duration
    }

    function _forceHide() {
        _busy       = 0;
        _silentBusy = 0;
        _hideGen++;
        clearTimeout(_safetyTimer);
        var e = el();
        if (e) { e.style.opacity = '0'; e.style.display = 'none'; }
    }

    /* ── Counter-aware public API ────────────────────────────────── */
    function show() {
        _busy++;
        if (_busy === 1) _show();
    }

    function hide() {
        _busy = Math.max(0, _busy - 1);
        if (_busy === 0) _hide();
    }

    /* ── jQuery AJAX integration ─────────────────────────────────── */
    function wireAjax() {
        var $ = w.jQuery;
        if (!$ || !$.ajax) return;

        $(document).on('ajaxSend', function (evt, xhr, settings) {
            if (settings._silentLoader) {
                _silentBusy++;
                xhr._hrmssilent = true;
            } else {
                show();
            }
        });

        $(document).on('ajaxComplete', function (evt, xhr) {
            if (xhr._hrmssilent) {
                _silentBusy = Math.max(0, _silentBusy - 1);
            } else {
                hide();
            }
        });
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', wireAjax, { once: true });
    } else {
        wireAjax();
    }

    /* ── Page navigation: show on same-origin anchor clicks ──────── */
    document.addEventListener('click', function (e) {
        var a = e.target;
        while (a && a.tagName !== 'A') a = a.parentElement;
        if (!a || a.tagName !== 'A') return;

        var href = (a.getAttribute('href') || '').trim();
        if (!href || href === '#') return;
        if (/^(javascript|mailto|tel):/i.test(href)) return;
        if (a.target === '_blank') return;
        if (a.dataset.bsToggle || a.dataset.bsDismiss ||
            a.dataset.toggle   || a.dataset.dismiss) return;
        if ('noLoader' in (a.dataset || {})) return;

        try {
            var url = new URL(href, w.location.origin);
            if (url.origin !== w.location.origin) return;
            if (url.pathname === w.location.pathname && !url.search && url.hash) return;
        } catch (_) { return; }

        show();
    }, true);

    /* ── Safety nets ─────────────────────────────────────────────── */
    w.addEventListener('load',     _forceHide);
    w.addEventListener('pageshow', _forceHide);

    /* ════════════════════════════════════════════════════════════════
       OLD-LOADER AUTO-TRANSFORM
       Finds every div whose id contains "loader" AND contains the old
       loder.png <img>, replaces its content with the WMS dot-spinner,
       and wires a MutationObserver proxy so existing page JS that
       calls $('#grid-loader').css('display','flex') / .hide() is
       silently redirected to showLoader() / hideLoader() — without
       touching a single page file.
    ════════════════════════════════════════════════════════════════ */
    var DOT_HTML =
        '<div class="wg-dot-spinner">' +
        '<div></div><div></div><div></div><div></div>' +
        '<div></div><div></div><div></div><div></div>' +
        '<div></div><div></div><div></div><div></div>' +
        '</div>' +
        '<p class="hrms-loader-text">Please wait' +
        '<span class="hrms-loader-dots"></span></p>';

    function isOldLoader(el) {
        /* Must have old image OR be a known grid-loader pattern */
        if (el.querySelector('img[src*="loder"]') ||
            el.querySelector('.grid-logo-spinner') ||
            el.classList.contains('grid-loader')) return true;
        /* Heuristic: fixed/absolute, large, centred, transparent bg */
        var s = el.style;
        return s.position === 'fixed' &&
               (s.width === '100vw' || s.width === '100%') &&
               s.zIndex >= 9000;
    }

    function attachProxy(el) {
        /* _isSelfHiding flag prevents the observer's own style.display='none'
           from triggering hideLoader() a second time. */
        var _isSelfHiding = false;

        new MutationObserver(function (muts) {
            muts.forEach(function (m) {
                if (m.attributeName !== 'style') return;

                if (_isSelfHiding) { _isSelfHiding = false; return; }

                var disp = el.style.display;
                var shown = disp !== '' && disp !== 'none';

                if (shown) {
                    /* Page code is trying to show this old loader →
                       hide it immediately and show the global WMS loader */
                    _isSelfHiding = true;
                    el.style.display = 'none';
                    show();
                } else {
                    /* Page code is hiding the old loader → hide global */
                    hide();
                }
            });
        }).observe(el, { attributes: true, attributeFilter: ['style'] });
    }

    function transformOldLoaders() {
        var candidates = document.querySelectorAll(
            'div[id*="loader"], div[class*="grid-loader"]'
        );

        candidates.forEach(function (el) {
            /* Skip the main WMS #loader and already-converted elements */
            if (el.id === 'loader' ||
                el.classList.contains('wg-page-loader') ||
                el.classList.contains('hrms-ol-done')) return;

            if (!isOldLoader(el)) return;

            /* Replace content with WMS dot-spinner */
            el.innerHTML = DOT_HTML;
            el.classList.add('hrms-ol-done');    /* mark as converted */

            /* Ensure it has the right base flex layout when shown */
            el.style.flexDirection = 'column';
            el.style.alignItems    = 'center';
            el.style.justifyContent = 'center';
            el.style.gap           = '14px';

            /* Wire show/hide proxy */
            attachProxy(el);
        });
    }

    /* Run after DOM is parsed so all loader elements exist */
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', transformOldLoaders, { once: true });
    } else {
        transformOldLoaders();
    }

    /* ════════════════════════════════════════════════════════════════
       MODAL TELEPORT  (global fix — runs on every page)

       Root cause: app-shell.css sets `.main-area { position: fixed }`
       which creates an isolated CSS stacking context.  Any Bootstrap
       modal defined inside @RenderBody() lives inside that stacking
       context, so its z-index is LOCAL to .main-area.  Bootstrap's
       backdrop is injected as a direct <body> child in the ROOT
       stacking context — the backdrop always wins, blocking clicks.

       Fix: move every .modal element to be a DIRECT child of <body>
       so it shares the root stacking context with the backdrop.
       z-index 1000100 (modal) then correctly beats 1000099 (backdrop).

       Timing: this script tag is at the bottom of <body>, so the DOM
       is already fully parsed when it runs — readyState is 'interactive'
       or 'complete', never 'loading'.  teleportModals() is called
       synchronously, completing before any $(document).ready() fires.
    ════════════════════════════════════════════════════════════════ */
    function teleportModals() {
        document.querySelectorAll('.modal').forEach(function (modal) {
            if (modal.parentNode !== document.body) {
                document.body.appendChild(modal);
            }
        });
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', teleportModals, { once: true });
    } else {
        teleportModals();   /* DOM already parsed — run immediately */
    }

    /* ── Expose globals ──────────────────────────────────────────── */
    w.showLoader      = show;
    w.hideLoader      = hide;
    w.forceHideLoader = _forceHide;
    w.showPageLoader  = show;
    w.hidePageLoader  = hide;

})(window);
