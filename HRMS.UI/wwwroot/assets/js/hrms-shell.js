/* ════════════════════════════════════════════════════════════════════════
   HRMS Shell — WMS-style sidebar / shell wiring
   Handles: desktop sidebar collapse, mobile sidebar, group accordion,
   active-link detection, scroll-to-top, user-name sync.
   No dependency on WMS_MENU_DATA or wms-app.js.
   ════════════════════════════════════════════════════════════════════════ */
(function (w) {
    'use strict';

    var LS_KEY = 'hrms_sb_collapsed';

    // ── Desktop sidebar collapse toggle ─────────────────────────────────
    function sbToggleCollapse() {
        var sidebar = document.getElementById('sidebar');
        var icon    = document.getElementById('sb-toggle-icon');
        if (!sidebar) return;
        var collapsed = sidebar.classList.toggle('sb-collapsed');
        document.body.classList.toggle('sb-collapsed-body', collapsed);
        if (icon) icon.className = collapsed ? 'bx bx-chevrons-right' : 'bx bx-chevrons-left';
        try { localStorage.setItem(LS_KEY, collapsed ? '1' : '0'); } catch (_) {}
    }

    // ── Mobile sidebar open / close ──────────────────────────────────────
    function hrmsToggleMobSidebar() {
        var sidebar  = document.getElementById('sidebar');
        var overlay  = document.getElementById('sidebar-overlay');
        if (!sidebar) return;
        var open = sidebar.classList.toggle('mob-open');
        document.body.classList.toggle('sb-mob-open', open);
        if (overlay) overlay.classList.toggle('open', open);
    }

    function closeMobSidebar() {
        var sidebar = document.getElementById('sidebar');
        var overlay = document.getElementById('sidebar-overlay');
        if (sidebar) { sidebar.classList.remove('mob-open'); }
        document.body.classList.remove('sb-mob-open');
        if (overlay) overlay.classList.remove('open');
    }

    // ── Sidebar group accordion ──────────────────────────────────────────
    // Called with the .sb-group element (not the header).
    function sbToggleGroup(groupEl) {
        if (!groupEl) return;
        groupEl.classList.toggle('collapsed');
    }

    // ── Restore collapsed state from localStorage ────────────────────────
    function restoreSidebarState() {
        try {
            if (localStorage.getItem(LS_KEY) === '1') {
                var sidebar = document.getElementById('sidebar');
                var icon    = document.getElementById('sb-toggle-icon');
                if (sidebar) {
                    sidebar.classList.add('sb-collapsed');
                    document.body.classList.add('sb-collapsed-body');
                }
                if (icon) icon.className = 'bx bx-chevrons-right';
            }
        } catch (_) {}
    }

    // ── Active sidebar link detection ────────────────────────────────────
    function markActiveSidebarLink() {
        var path    = w.location.pathname.toLowerCase().replace(/\/$/, '');
        var links   = document.querySelectorAll('#sidebar-link a[href]');
        var best    = null;
        var bestLen = 0;

        links.forEach(function (a) {
            var raw = (a.getAttribute('href') || '').trim();
            if (!raw || raw === '#' || raw.indexOf('javascript') === 0) return;

            var hPath;
            try {
                hPath = new URL(raw, w.location.origin).pathname.toLowerCase().replace(/\/$/, '');
            } catch (_) { return; }

            if (!hPath || hPath === '/') return;

            // Exact match OR current path starts with this link's path
            // (e.g. /adminpanel/jobmaster/state matches /adminpanel/jobmaster/state)
            if ((path === hPath || path.startsWith(hPath + '/')) && hPath.length > bestLen) {
                best    = a;
                bestLen = hPath.length;
            }
        });

        if (best) {
            best.classList.add('is-active');

            // Walk up and remove 'collapsed' from EVERY ancestor .sb-group,
            // not just the nearest one — this fixes nested sub-groups where
            // closest() would stop at the inner group and leave the outer
            // parent group still collapsed.
            var node = best.parentElement;
            while (node && node.id !== 'sidebar-link') {
                if (node.classList && node.classList.contains('sb-group')) {
                    node.classList.remove('collapsed');
                }
                node = node.parentElement;
            }
        }
    }

    // ── Active module tab detection ──────────────────────────────────────
    function markActiveModuleTab() {
        var path  = w.location.pathname.toLowerCase();
        var tabs  = document.querySelectorAll('.as-tab-nav-item[href]');
        var best  = null;
        var bestLen = 0;

        tabs.forEach(function (tab) {
            var href = (tab.getAttribute('href') || '').toLowerCase();
            try {
                var u = new URL(href, w.location.origin);
                href  = u.pathname.toLowerCase();
            } catch (_) {}
            // Match by longest-prefix of the path
            var parts = href.split('/').filter(Boolean);
            if (parts.length >= 2) {
                var seg = '/' + parts.slice(-2).join('/');
                if (path.includes(seg) && seg.length > bestLen) {
                    best    = tab;
                    bestLen = seg.length;
                }
            }
        });

        if (best) {
            tabs.forEach(function (t) { t.classList.remove('active'); t.removeAttribute('aria-selected'); });
            best.classList.add('active');
            best.setAttribute('aria-selected', 'true');
        }
    }

    // ── Sync user name to the slim top-nav "Logged in as …" span ────────
    function syncTopNavUserName() {
        try {
            var name = localStorage.getItem('userName')
                    || localStorage.getItem('fullName')
                    || localStorage.getItem('username')
                    || '';
            if (name) {
                var el = document.getElementById('as-userName');
                if (el) el.textContent = name;
                var dn = document.getElementById('dropdown-name');
                if (dn) dn.textContent = name;
            }
        } catch (_) {}
    }

    // ── Scroll-to-top button ─────────────────────────────────────────────
    function initScrollToTop() {
        var btn = document.getElementById('scroll-to-top');
        if (!btn) return;
        var sc = document.getElementById('main-area') || w;

        function tick() {
            var y = (sc === w) ? w.scrollY : sc.scrollTop;
            if (y > 300) {
                btn.style.display = 'flex';
                setTimeout(function () {
                    btn.style.opacity = '1';
                    btn.style.pointerEvents = 'auto';
                    btn.style.transform = 'translateY(0)';
                }, 10);
            } else {
                btn.style.opacity       = '0';
                btn.style.pointerEvents = 'none';
                btn.style.transform     = 'translateY(8px)';
                setTimeout(function () {
                    if (btn.style.opacity === '0') btn.style.display = 'none';
                }, 270);
            }
        }

        btn.style.transform = 'translateY(8px)';
        sc.addEventListener('scroll', tick, { passive: true });

        btn.onclick = function () {
            if (sc === w) w.scrollTo({ top: 0, behavior: 'smooth' });
            else sc.scrollTo({ top: 0, behavior: 'smooth' });
        };
        btn.addEventListener('mouseenter', function () {
            btn.style.transform = 'translateY(-3px) scale(1.08)';
            btn.style.boxShadow = '0 6px 20px rgba(45,122,181,.55)';
        });
        btn.addEventListener('mouseleave', function () {
            btn.style.transform = 'translateY(0) scale(1)';
            btn.style.boxShadow = '0 4px 14px rgba(45,122,181,.40)';
        });
    }

    // ── Custom modal: ESC key close ──────────────────────────────────────
    // Only targets custom modals (no .fade class) that are currently visible.
    // Bootstrap modals (.modal.fade) manage their own keyboard handling.
    function initModalKeyboard() {
        document.addEventListener('keydown', function (e) {
            if (e.key !== 'Escape') return;
            document.querySelectorAll('.modal:not(.fade)').forEach(function (m) {
                if (m.style && m.style.display && m.style.display !== 'none') {
                    m.style.display = 'none';
                }
            });
        });
    }

    // ── Custom modal: click-outside (backdrop) close ─────────────────────
    // A click directly on the .modal backdrop element (not on its content)
    // dismisses it. Checking e.target === the modal avoids closing when
    // clicking on child elements inside the modal content.
    function initModalBackdropClose() {
        document.addEventListener('click', function (e) {
            var target = e.target;
            if (!target || !target.classList) return;
            if (!target.classList.contains('modal')) return;   // must be the backdrop
            if (target.classList.contains('fade')) return;     // skip Bootstrap modals
            if (target.style && target.style.display !== 'none') {
                target.style.display = 'none';
            }
        });
    }

    // ── Notification bell toggle for HRMS notification system ───────────
    // The HRMS notification-remainder.js uses #bellIcon / #notificationDropdown.
    // This just wires the as-notif-dot count badge to the existing HRMS badge.
    // ── Sync company name from slim nav to project header ────────────
    function syncCompanyNameToHeader() {
        var src = document.querySelector('.as-top-nav .companyNameLayout .custom-tooltip');
        var dst = document.getElementById('hrms-company-header');
        if (!src || !dst) return;

        var sync = function () {
            var t = (src.textContent || '').trim();
            if (t) dst.textContent = t;
        };

        sync();
        new MutationObserver(sync).observe(src, { childList: true, characterData: true, subtree: true });
    }

    function syncNotifBadge() {
        var hrmsCount = document.getElementById('notificationCount');
        var wmsDot    = document.getElementById('notif-dot-topbar');
        if (!hrmsCount || !wmsDot) return;

        var observer = new MutationObserver(function () {
            var n = parseInt(hrmsCount.textContent, 10) || 0;
            wmsDot.textContent = n;
            if (n > 0) {
                wmsDot.classList.add('active');
                var bellBtn = document.getElementById('notif-bell-btn');
                if (bellBtn) bellBtn.classList.add('has-unread');
            } else {
                wmsDot.classList.remove('active');
                var bellBtn2 = document.getElementById('notif-bell-btn');
                if (bellBtn2) bellBtn2.classList.remove('has-unread');
            }
        });
        observer.observe(hrmsCount, { childList: true, characterData: true, subtree: true });
    }

    // ── Boot ─────────────────────────────────────────────────────────────
    function boot() {
        restoreSidebarState();
        markActiveSidebarLink();
        markActiveModuleTab();
        syncTopNavUserName();
        initScrollToTop();
        initModalKeyboard();
        initModalBackdropClose();
        syncNotifBadge();
        syncCompanyNameToHeader();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', boot, { once: true });
    } else {
        setTimeout(boot, 0);
    }

    // Expose globals used by inline onclick handlers in layout HTML
    w.sbToggleCollapse      = sbToggleCollapse;
    w.hrmsToggleMobSidebar  = hrmsToggleMobSidebar;
    w.closeMobSidebar       = closeMobSidebar;
    w.sbToggleGroup         = sbToggleGroup;

})(window);

/* ════════════════════════════════════════════════════════════════════════
   Page-loader helpers  — keep id="loader" for backwards compat
   (permissions.js, layouts all reference document.getElementById("loader"))
   The element is now a .wg-page-loader instead of .loader-container.
   ════════════════════════════════════════════════════════════════════════ */
(function () {
    function getLoader() { return document.getElementById('loader'); }

    window.showPageLoader = function () {
        var el = getLoader();
        if (el) el.style.display = 'flex';
    };

    window.hidePageLoader = function () {
        var el = getLoader();
        if (el) el.style.display = 'none';
    };

    /* Safety net for Bootstrap welcome modal: ensure backdrop is always
       removed on modal hide, even if Bootstrap's own cleanup stalls. */
    document.addEventListener('DOMContentLoaded', function () {
        var welcomeModal = document.getElementById('leaveBalanceModal');
        if (!welcomeModal) return;

        welcomeModal.addEventListener('hidden.bs.modal', function () {
            /* Remove any lingering Bootstrap backdrop */
            document.querySelectorAll('.modal-backdrop').forEach(function (bd) {
                bd.remove();
            });
            document.body.classList.remove('modal-open');
            document.body.style.removeProperty('overflow');
            document.body.style.removeProperty('padding-right');
        });
    });
})();
