/* ════════════════════════════════════════════════════════════════════════
   App-shell wiring for WMS Spectrum
     · User dropdown toggle (My Account ▾)
     · Search-pages live-suggestions
     · Jump-to-page select
     · Module tabs (top-level menu groups)
   Page list is derived from the WMS sidebar (#sidebar-link) so it always
   matches the user's permission-filtered menu.
   ════════════════════════════════════════════════════════════════════════ */
(function (w) {
    'use strict';

    // ── User dropdown ───────────────────────────────────────────────────
    function asToggleUserDd() {
        var dd = document.getElementById('as-user-dd');
        if (!dd) return;
        dd.classList.toggle('open');
    }
    function asCloseUserDd() {
        var dd = document.getElementById('as-user-dd');
        if (dd) dd.classList.remove('open');
    }
    document.addEventListener('click', function (e) {
        var wrap = document.getElementById('as-user-dd-wrap');
        if (!wrap) return;
        if (!wrap.contains(e.target)) asCloseUserDd();
    });

    // ── Sidebar pages discovery ─────────────────────────────────────────
    // The WMS sidebar is built dynamically by menu.js — pages appear there
    // as anchors with href / data-page attributes. We reuse those so the
    // search-pages list and jump-to-page select stay automatically in sync
    // with the user's allowed menu (no separate hard-coded list to maintain).
    function asGetPagesFromSidebar() {
        var sidebar = document.getElementById('sidebar-link');
        if (!sidebar) return [];
        var pages = [];
        var anchors = sidebar.querySelectorAll('a[href]:not([href="#"]), a[data-page]');
        anchors.forEach(function (a) {
            var label = (a.textContent || '').trim().replace(/\s+/g, ' ');
            var url = a.getAttribute('href') || '';
            if (!label || !url || url === '#') return;

            // Walk up to find the closest group/category title for grouping.
            var group = 'Menu';
            var grpEl = a.closest('[data-menu-group], .sb-group, .menu-group');
            if (grpEl) {
                var gh = grpEl.querySelector('.sb-group-name, .menu-group-name, [data-group-name]');
                if (gh) group = (gh.textContent || '').trim();
                else if (grpEl.dataset.menuGroup) group = grpEl.dataset.menuGroup;
            }

            // De-dup (same href appearing twice from collapsed/expanded states).
            if (pages.find(function (p) { return p.url === url && p.label === label; })) return;
            pages.push({ label: label, url: url, group: group });
        });
        return pages;
    }

    // ── Search pages (live dropdown under the input) ───────────────────
    function asInitQuickSearch() {
        var input = document.getElementById('as-globalSearch');
        var dd    = document.getElementById('as-globalSearch-dd');
        if (!input || !dd) return;
        var activeIdx = -1;

        function highlight(idx) {
            activeIdx = idx;
            dd.querySelectorAll('.as-qs-item').forEach(function (el, i) {
                el.classList.toggle('_active', i === idx);
            });
        }

        function render(q) {
            if (!q) { dd.classList.remove('_open'); activeIdx = -1; return; }
            var pages = asGetPagesFromSidebar();
            var ql = q.toLowerCase();
            var hits = pages.filter(function (p) {
                return p.label.toLowerCase().indexOf(ql) !== -1
                    || p.group.toLowerCase().indexOf(ql) !== -1;
            }).slice(0, 8);

            if (!hits.length) {
                dd.innerHTML =
                    '<div class="as-qs-item" style="cursor:default;color:#94A3B8">' +
                    '<i class="bx bx-info-circle"></i><span>No matching pages</span></div>';
                dd.classList.add('_open');
                activeIdx = -1;
                return;
            }

            dd.innerHTML = hits.map(function (p, i) {
                return '<a class="as-qs-item" href="' + escapeAttr(p.url) + '" data-i="' + i + '">' +
                       '<i class="bx bx-chevron-right"></i>' +
                       '<span>' + escapeHtml(p.label) + '</span>' +
                       (p.group ? '<span style="margin-left:auto;font-size:10.5px;color:#94A3B8">' + escapeHtml(p.group) + '</span>' : '') +
                       '</a>';
            }).join('');
            Array.prototype.forEach.call(dd.querySelectorAll('.as-qs-item'), function (el) {
                el.addEventListener('mouseenter', function () { highlight(parseInt(el.dataset.i, 10)); });
            });
            dd.classList.add('_open');
            activeIdx = -1;
        }

        input.addEventListener('input', function () { render(input.value.trim()); });
        input.addEventListener('focus', function () { if (input.value.trim()) render(input.value.trim()); });
        input.addEventListener('keydown', function (e) {
            var items = dd.querySelectorAll('.as-qs-item[href]');
            if (e.key === 'ArrowDown') { e.preventDefault(); highlight(Math.min(activeIdx + 1, items.length - 1)); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); highlight(Math.max(activeIdx - 1, 0)); }
            else if (e.key === 'Enter') {
                e.preventDefault();
                if (activeIdx >= 0 && items[activeIdx]) { window.location.href = items[activeIdx].href; }
                else if (items[0]) { window.location.href = items[0].href; }
            } else if (e.key === 'Escape') {
                dd.classList.remove('_open');
                input.blur();
            }
        });

        document.addEventListener('click', function (e) {
            if (!e.target.closest('.as-quick-search-wrap')) dd.classList.remove('_open');
        });
    }

    // ── Jump to page (grouped <select>) ─────────────────────────────────
    function asInitJumpToPage() {
        var sel = document.getElementById('as-jumpToPage');
        if (!sel) return;

        function rebuild() {
            var pages = asGetPagesFromSidebar();
            // Reset
            sel.innerHTML = '<option value="">Jump to a page…</option>';
            if (!pages.length) return;

            // Group pages by their .group property
            var groups = {};
            pages.forEach(function (p) {
                if (!groups[p.group]) groups[p.group] = [];
                groups[p.group].push(p);
            });

            Object.keys(groups).forEach(function (g) {
                var grp = document.createElement('optgroup');
                grp.label = g;
                groups[g].forEach(function (p) {
                    var opt = document.createElement('option');
                    opt.value = p.url;
                    opt.textContent = p.label;
                    grp.appendChild(opt);
                });
                sel.appendChild(grp);
            });
        }

        rebuild();
        sel.addEventListener('change', function () {
            if (sel.value) {
                window.location.href = sel.value;
                sel.selectedIndex = 0;
            }
        });

        // Re-build whenever the sidebar changes (menu.js populates it asynchronously).
        var sidebar = document.getElementById('sidebar-link');
        if (sidebar && typeof MutationObserver === 'function') {
            var mo = new MutationObserver(function () { rebuild(); });
            mo.observe(sidebar, { childList: true, subtree: true });
        }
    }

    // ── User name + change-password / logout hooks ─────────────────────
    // Reuse WMS's existing globals — these functions already exist in
    // wms-app.js. We just re-bind onclick to keep the existing flow.
    function asWireUserMenu() {
        // Display the logged-in user's name.
        try {
            var name = sessionStorage.getItem('userName')
                    || sessionStorage.getItem('fullName')
                    || sessionStorage.getItem('username')
                    || '';
            if (name) {
                var el = document.getElementById('as-userName');
                if (el) el.textContent = name;
            }
        } catch (_) {}

        var btnPwd = document.getElementById('as-btn-change-pwd');
        if (btnPwd) btnPwd.addEventListener('click', function (e) {
            e.preventDefault();
            asCloseUserDd();
            if (typeof w.openChangePwd === 'function') w.openChangePwd();
            else if (typeof w.changePassword === 'function') w.changePassword();
        });

        var btnLogout = document.getElementById('as-btn-logout');
        if (btnLogout) btnLogout.addEventListener('click', function (e) {
            e.preventDefault();
            asCloseUserDd();
            // Always show the confirmation modal first.  openLogout() pops the
            // confirmation; the Yes button on that modal calls confirmLogout()
            // which actually logs the user out.  Earlier this jumped straight
            // to confirmLogout(), which logged the user out without asking.
            if (typeof w.openLogout === 'function') {
                w.openLogout();
                return;
            }
            // Fallbacks if the legacy modal helpers aren't on the page (e.g.
            // a screen that hasn't pulled wms-app.js).
            var goAhead = (typeof w.WMSConfirm !== 'undefined' && w.WMSConfirm.show)
                ? w.WMSConfirm.show({
                    type:    'warning',
                    title:   'Sign Out',
                    message: 'Are you sure you want to sign out?',
                    okText:  'Yes, Sign Out',
                    okIcon:  'bx bx-log-out'
                })
                : Promise.resolve(window.confirm('Are you sure you want to sign out?'));
            Promise.resolve(goAhead).then(function (ok) {
                if (!ok) return;
                if (typeof w.confirmLogout === 'function') { w.confirmLogout(); return; }
                if (typeof w.logout === 'function')        { w.logout(); return; }
                sessionStorage.clear();
                window.location.href = '/account/login';
            });
        });
    }

    function escapeHtml(s) {
        return String(s == null ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function escapeAttr(s) { return escapeHtml(s); }

    // ── Module tabs (top-level menu groups as horizontal tabs) ──────────
    // Reads WMS_MENU_DATA (populated by menu.js after the user-permissions
    // API responds) and renders each top-level group as a clickable tab.
    // The tab whose group contains the current page becomes "active".
    function asGetCurrentControllerAction() {
        var parts = window.location.pathname.replace(/^\/+|\/+$/g, '').split('/');
        return {
            controller: (parts[0] || '').toLowerCase(),
            action:     (parts[1] || '').toLowerCase()
        };
    }

    function asIsGroupActive(menu, cur) {
        if (menu.isDirectLink) {
            return cur.controller === (menu.controllerName || '').toLowerCase()
                && cur.action     === (menu.actionName     || '').toLowerCase();
        }
        return (menu.children || []).some(function (c) {
            return c.controllerName !== 'JS'
                && cur.controller === (c.controllerName || '').toLowerCase();
        });
    }

    function asFirstChildUrl(menu) {
        if (menu.isDirectLink) {
            var c = (menu.controllerName || '').toLowerCase();
            var a = (menu.actionName     || '').toLowerCase();
            return c && a ? ('/' + c + '/' + a) : '#';
        }
        var children = (menu.children || []).filter(function (c) {
            return c.actionType !== 'function' && c.controllerName && c.actionName;
        });
        if (!children.length) return '#';
        children.sort(function (a, b) { return (a.sortOrder ?? 99) - (b.sortOrder ?? 99); });
        var first = children[0];
        return '/' + (first.controllerName || '').toLowerCase() + '/' + (first.actionName || '').toLowerCase();
    }

    function buildModuleTabs() {
        var nav = document.getElementById('as-tab-nav');
        if (!nav) return;
        if (!w.WMS_MENU_DATA || !Array.isArray(w.WMS_MENU_DATA.menuTypes)) return;

        var sidebarMenus = (w.WMS_MENU_DATA.menuTypes.find(function (t) {
            return t.parentMenuTypeId === 1;
        }) || {}).parents || [];

        if (!sidebarMenus.length) { nav.innerHTML = ''; return; }

        var sorted = sidebarMenus.slice().sort(function (a, b) {
            return (a.sortOrder ?? 99) - (b.sortOrder ?? 99);
        });
        var cur = asGetCurrentControllerAction();

        nav.innerHTML = sorted.map(function (menu) {
            var label = menu.parentMenuName || '';
            if (!label) return '';
            var url = asFirstChildUrl(menu);
            var isActive = asIsGroupActive(menu, cur);
            var iconHtml = menu.parentIcon
                ? '<i class="' + menu.parentIcon + '"' + (menu.parentIconColor ? ' style="color:' + menu.parentIconColor + '"' : '') + '></i>'
                : '';
            return '<a class="as-tab-nav-item' + (isActive ? ' active' : '') + '"'
                 + ' href="' + url + '"'
                 + ' data-tab-key="' + escapeAttr(label) + '"'
                 + ' role="tab" aria-selected="' + (isActive ? 'true' : 'false') + '"'
                 + ' title="' + escapeAttr(label) + '">'
                 + iconHtml
                 + '<span>' + escapeHtml(label) + '</span>'
                 + '</a>';
        }).join('');
    }

    // The sidebar (and so WMS_MENU_DATA) is populated AFTER menu.js's
    // initMenus() async fetch resolves. Watch for sidebar updates and
    // (re-)build the tabs each time.
    function asInitModuleTabsAutoRefresh() {
        var sidebar = document.getElementById('sidebar-link');
        if (!sidebar) return;
        // Try once immediately in case data is already loaded.
        buildModuleTabs();
        if (typeof MutationObserver === 'function') {
            var mo = new MutationObserver(function () { buildModuleTabs(); });
            mo.observe(sidebar, { childList: true, subtree: false });
        }
    }

    // ── WMS modal stacking fix ───────────────────────────────────────────
    // .main-area is `position:fixed` (see app-shell.css) with no explicit
    // z-index, so it establishes its own stacking context at the implicit
    // "auto" level. Any .wms-modal-overlay left nested inside it — however
    // high its own z-index (9400) — gets compared against siblings like
    // .as-tab-nav/.as-project-header/.as-top-nav (z-index 230-250) at the
    // .main-area level, not its own, and loses: the modal's header renders
    // BEHIND those fixed bars instead of above them. Moving each overlay to
    // be a direct child of <body> (a "portal", same trick used by most
    // component libraries) puts it in the top-level stacking context where
    // its z-index is finally compared directly against the shell's — and
    // wins. Pure DOM reparenting; no visual/layout change otherwise, and
    // existing `document.getElementById('xxxModal')` / event listeners
    // everywhere keep working exactly as before.
    function asRelocateWmsModal(el) {
        if (el && el.parentElement !== document.body) {
            document.body.appendChild(el);
        }
    }
    function asRelocateWmsModals(root) {
        (root || document).querySelectorAll('.wms-modal-overlay').forEach(asRelocateWmsModal);
    }
    function asInitWmsModalRelocation() {
        asRelocateWmsModals();
        if (typeof MutationObserver !== 'function') return;
        // Tab partials / AJAX content can add more .wms-modal-overlay nodes
        // after initial load — catch those too.
        new MutationObserver(function (mutations) {
            mutations.forEach(function (m) {
                m.addedNodes.forEach(function (node) {
                    if (node.nodeType !== 1) return;
                    if (node.classList && node.classList.contains('wms-modal-overlay')) {
                        asRelocateWmsModal(node);
                    } else if (node.querySelectorAll) {
                        asRelocateWmsModals(node);
                    }
                });
            });
        }).observe(document.body, { childList: true, subtree: true });
    }

    // ── Boot ────────────────────────────────────────────────────────────
    function boot() {
        asWireUserMenu();
        asInitQuickSearch();
        asInitJumpToPage();
        asInitModuleTabsAutoRefresh();
        asInitWmsModalRelocation();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', boot, { once: true });
    } else {
        setTimeout(boot, 0);
    }

    // Expose for debugging + so menu.js can re-trigger tab rebuild explicitly.
    w.AppShell = {
        getPagesFromSidebar: asGetPagesFromSidebar,
        toggleUserDd:        asToggleUserDd,
        closeUserDd:         asCloseUserDd,
        buildModuleTabs:     buildModuleTabs
    };
})(window);
