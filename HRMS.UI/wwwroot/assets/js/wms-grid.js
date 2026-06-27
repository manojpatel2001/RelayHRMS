; (function (global) {
    'use strict';

    var _cssInjected = false;

    function injectCSS() {
        if (_cssInjected) return;
        _cssInjected = true;
        var style = document.createElement('style');
        style.id = 'wms-grid-styles';
        var css = [
            ':root {',
            '  --wg-primary: #2F6F8E;',
            '  --wg-primary-d: #235A75;',
            '  --wg-surface: #FFFFFF;',
            '  --wg-border: #D0D8DC;',
            '  --wg-border2: #B8C4CC;',
            '  --wg-hdr-bg: #DDE6ED;',
            '  --wg-row-even: #F5F8FA;',
            '  --wg-row-hover: #EAF2F8;',
            '  --wg-text: #1A1A1A;',
            '  --wg-muted: #778899;',
            '  --wg-green: #2E7D32; --wg-green-bg: #E8F5E9;',
            '  --wg-red: #C62828; --wg-red-bg: #FFEBEE;',
            '  --wg-amber: #E65100; --wg-amber-bg: #FFF3E0;',
            '  --wg-teal: #00695C; --wg-teal-bg: #E0F2F1;',
            '  --wg-blue: #1565C0; --wg-blue-bg: #E3F2FD;',
            '}',
            '.wms-grid-wrap { border:1px solid var(--wg-border); border-radius:8px; overflow:hidden; background:var(--wg-surface); box-shadow:0 2px 8px rgba(0,0,0,0.07); display:flex; flex-direction:column; font-family:"Inter",system-ui,sans-serif; font-size:13px; }',
            '.wms-grid-toolbar { display:flex; align-items:center; gap:0; padding:9px 14px; background:#EEF2F5; border-bottom:1px solid var(--wg-border); min-height:46px; flex-wrap:nowrap; }',
            '.wms-grid-title { font-size:13px; font-weight:700; color:#1A4558; display:flex; align-items:center; gap:7px; }',
            '.wms-grid-count-pill { font-size:10px; font-weight:700; background:#DDE9F0; color:#235A75; border-radius:20px; padding:2px 8px; }',
            '.wms-grid-toolbar-right { margin-left:auto; display:flex; align-items:center; gap:6px; flex-shrink:0; }',
            '.wms-grid-search-wrap { position:relative; display:flex; align-items:center; transition:width .2s; }',
            '.wms-grid-search-wrap i.bx-search { position:absolute; left:9px; top:50%; transform:translateY(-50%); color:#94A3B8; font-size:15px; pointer-events:none; z-index:1; }',
            '.wms-grid-search-input { width:200px; padding:6px 30px 6px 32px; border:1.5px solid #CBD5E1; border-radius:7px; font-size:12.5px; font-family:inherit; color:var(--wg-text); background:#fff; outline:none; transition:border-color .15s, box-shadow .15s; }',
            '.wms-grid-search-input:focus { border-color:var(--wg-primary); box-shadow:0 0 0 3px rgba(47,111,142,0.13); width:240px; }',
            '.wms-grid-search-input::placeholder { color:#b0bcc4; }',
            '.wms-grid-search-reset { position:absolute; right:7px; top:50%; transform:translateY(-50%); display:none; align-items:center; justify-content:center; width:18px; height:18px; background:#E2E8F0; border:none; border-radius:50%; cursor:pointer; color:#64748B; font-size:12px; padding:0; transition:background .12s; }',
            '.wms-grid-search-reset:hover { background:#FECACA; color:#DC2626; }',
            '.wms-grid-search-wrap.has-query .wms-grid-search-reset { display:flex; }',
            '.wms-grid-search-wrap.has-query .wms-grid-search-input { border-color:var(--wg-primary); }',
            '.wg-btn { display:inline-flex; align-items:center; gap:5px; padding:5px 11px; border-radius:6px; font-size:11.5px; font-weight:600; cursor:pointer; border:1px solid transparent; font-family:inherit; transition:background .15s; white-space:nowrap; line-height:1; }',
            '.wg-btn-primary { background:var(--wg-primary); color:#fff; border-color:var(--wg-primary-d); }',
            '.wg-btn-primary:hover { background:var(--wg-primary-d); }',
            '.wg-btn-outline { background:#F0F5F8; color:var(--wg-primary); border-color:var(--wg-border2); }',
            '.wg-btn-outline:hover { background:#DDE9F0; }',
            '.wg-btn-ghost { background:transparent; color:#64748B; border-color:#CBD5E1; }',
            '.wg-btn-ghost:hover { background:#F0F5F8; color:var(--wg-primary); }',
            /* ── FIX 3: touch scroll — tbl-wrap keeps overflow-x:auto for native touch,
                  cursor:grab only on non-touch via media query.
               ── FIX PAGINATION: cap the table-body height so the pagination bar is
                  always visible below the grid without requiring page-level scrolling.
                  calc(100vh - 420px) accounts for topbar(106px) + card-header(50px) +
                  grid-toolbar(46px) + pagination-bar(48px) + paddings + tab-nav(56px)
                  ≈ 420px total shell overhead.  overflow-y:auto lets rows scroll
                  inside the cap; sticky thead sticks inside the scroll container.  */
            '.wms-grid-tbl-wrap { overflow-x:auto; overflow-y:auto; -webkit-overflow-scrolling:touch; flex:1; scroll-behavior:smooth; scrollbar-width:thin; scrollbar-color:#b0c4d0 #f1f5f9; touch-action:pan-x pan-y; max-height:calc(100vh - 420px); min-height:120px; }',
            '.wms-grid-tbl-wrap::-webkit-scrollbar { width:6px; height:6px; }',
            '.wms-grid-tbl-wrap::-webkit-scrollbar-track { background:#f1f5f9; }',
            '.wms-grid-tbl-wrap::-webkit-scrollbar-thumb { background:#b0c4d0; border-radius:3px; }',
            '@media (hover:hover) { .wms-grid-tbl-wrap { cursor:grab; } .wms-grid-tbl-wrap.grabbing { cursor:grabbing !important; user-select:none; } }',
            '.wg-swipe-wrap { position:relative; }',
            '.wg-scroll-hint { position:absolute; top:50%; transform:translateY(-50%); z-index:10; width:28px; height:44px; display:flex; align-items:center; justify-content:center; background:rgba(47,111,142,0.13); border-radius:6px; cursor:pointer; transition:opacity .2s, background .15s; pointer-events:none; opacity:0; }',
            '.wg-scroll-hint i { font-size:18px; color:#2F6F8E; }',
            '.wg-scroll-hint.left { left:4px; }',
            '.wg-scroll-hint.right { right:4px; }',
            '.wg-scroll-hint.visible { opacity:1; pointer-events:all; }',
            '.wg-scroll-hint:hover { background:rgba(47,111,142,0.25); }',
            '.wg-swipe-bar { height:3px; background:#E8ECF0; border-radius:2px; margin:0; overflow:hidden; }',
            '.wg-swipe-bar-inner { height:100%; background:linear-gradient(90deg,#2F6F8E,#88C8E0); border-radius:2px; transition:width .1s, margin-left .1s; width:100%; }',
            '.wms-grid-tbl-wrap.fixed-height { max-height:none; }',
            /* ── Full grid borders: use border-collapse:separate so sticky cells keep their borders ── */
            '.wms-grid-tbl-wrap table { width:100%; min-width:max-content; border-collapse:separate; border-spacing:0; font-size:12.5px; }',
            /* Header cells — right border draws column dividers; bottom border is the heavy header underline */
            '.wms-grid-tbl-wrap thead th { background:var(--wg-hdr-bg); padding:8px 12px; font-size:11px; font-weight:700; color:#3B5068; text-align:left; border-top:1px solid var(--wg-border2); border-bottom:2px solid var(--wg-border2); border-right:1px solid var(--wg-border2); white-space:nowrap; position:sticky; top:0; z-index:2; user-select:none; }',
            '.wms-grid-tbl-wrap thead th:first-child { border-left:none; }',
            '.wms-grid-tbl-wrap thead th:last-child  { border-right:none; }',
            '.wms-grid-tbl-wrap thead th.wg-sortable { cursor:pointer; }',
            '.wms-grid-tbl-wrap thead th.wg-sortable:hover { background:#CED8E5; }',
            '.wg-sort-arrow { display:inline-block; margin-left:4px; opacity:0.35; font-style:normal; font-size:10px; }',
            'th.wg-sort-asc .wg-sort-arrow { opacity:1; color:var(--wg-primary); }',
            'th.wg-sort-asc .wg-sort-arrow::before { content:"\\e9a2"; font-family:boxicons; font-size:13px; }',
            'th.wg-sort-desc .wg-sort-arrow { opacity:1; color:var(--wg-primary); }',
            'th.wg-sort-desc .wg-sort-arrow::before { content:"\\e9a3"; font-family:boxicons; font-size:13px; }',
            '.wg-sort-arrow { font-size:13px; margin-left:3px; opacity:0.35; vertical-align:middle; }',
            /* Body cells — right border = column line; bottom border = row line */
            '.wms-grid-tbl-wrap tbody td { padding:9px 12px; border-bottom:1px solid #D8E2E8; border-right:1px solid #D8E2E8; vertical-align:middle; color:var(--wg-text); }',
            '.wms-grid-tbl-wrap tbody td:first-child { border-left:none; }',
            '.wms-grid-tbl-wrap tbody td:last-child  { border-right:none; }',
            '.wms-grid-tbl-wrap tbody tr:nth-child(even) td { background:var(--wg-row-even); }',
            '.wms-grid-tbl-wrap tbody tr:hover td { background:var(--wg-row-hover) !important; }',
            '.wms-grid-tbl-wrap tbody tr:last-child td { border-bottom:none; }',
            '.wms-grid-tbl-wrap tbody tr.wg-clickable { cursor:pointer; }',
            '.wg-empty-row td { text-align:center; padding:36px 20px; color:#94A3B8; font-size:13px; }',
            /* ── FIX 3: Sticky Action column ── */
            '.wms-grid-tbl-wrap thead th.wg-action-col { position:sticky; right:0; z-index:4; background:var(--wg-hdr-bg); box-shadow:-3px 0 8px rgba(0,0,0,0.10); border-left:1px solid var(--wg-border2); border-right:none; }',
            '.wms-grid-tbl-wrap tbody td.wg-action-col { position:sticky; right:0; z-index:1; background:var(--wg-surface); box-shadow:-3px 0 8px rgba(0,0,0,0.07); border-left:1px solid #D8E2E8; border-right:none; }',
            '.wms-grid-tbl-wrap tbody tr:nth-child(even) td.wg-action-col { background:var(--wg-row-even); }',
            '.wms-grid-tbl-wrap tbody tr:hover td.wg-action-col { background:var(--wg-row-hover) !important; }',
            /* ── end sticky action col ── */
            '.wms-grid-pagination { display:flex; align-items:center; justify-content:space-between; padding:10px 16px; border-top:1px solid var(--wg-border); background:#FAFBFC; flex-wrap:wrap; gap:8px; }',
            '.wg-pg-info { font-size:11.5px; color:#64748B; font-weight:500; }',
            '.wg-pg-info strong { color:#1A4558; }',
            '.wg-pg-btns { display:flex; align-items:center; gap:3px; }',
            '.wg-pg-btn { min-width:30px; height:28px; padding:0 8px; border:1px solid #E2E8F0; background:#fff; border-radius:6px; font-size:11.5px; font-weight:600; color:#475569; cursor:pointer; font-family:inherit; transition:background .12s; display:flex; align-items:center; justify-content:center; }',
            '.wg-pg-btn:hover:not(:disabled) { background:#EEF6FA; border-color:#88C8E0; color:#1A4558; }',
            '.wg-pg-btn.active { background:var(--wg-primary); border-color:var(--wg-primary); color:#fff; }',
            '.wg-pg-btn:disabled { opacity:0.35; cursor:not-allowed; }',
            '.wg-pg-sep { font-size:11px; color:#94A3B8; padding:0 2px; }',
            '.wg-per-page { display:flex; align-items:center; gap:5px; font-size:11.5px; color:#64748B; }',
            '.wg-per-page select { border:1px solid #E2E8F0; border-radius:6px; padding:3px 6px; font-size:11.5px; color:#475569; background:#fff; cursor:pointer; font-family:inherit; }',
            '.wg-match-chip { font-size:10.5px; color:var(--wg-primary); font-weight:700; white-space:nowrap; padding:3px 9px; background:#DDE9F0; border-radius:20px; border:1px solid #B8D2E0; }',
            '.wg-pill { display:inline-flex; align-items:center; padding:2px 9px; border-radius:3px; font-size:10.5px; font-weight:700; white-space:nowrap; }',
            '.wg-pill-green { background:var(--wg-green-bg); color:var(--wg-green); border:1px solid #A5D6A7; }',
            '.wg-pill-red   { background:var(--wg-red-bg); color:var(--wg-red); border:1px solid #EF9A9A; }',
            '.wg-pill-amber { background:var(--wg-amber-bg); color:var(--wg-amber); border:1px solid #FFCC80; }',
            '.wg-pill-teal  { background:var(--wg-teal-bg); color:var(--wg-teal); border:1px solid #80CBC4; }',
            '.wg-pill-blue  { background:var(--wg-blue-bg); color:var(--wg-blue); border:1px solid #90CAF9; }',
            '.wg-pill-pu    { background:#DDE9F0; color:#235A75; }',
            '.wg-col-filter-wrap { position:relative; display:flex; align-items:center; }',
            '.wg-col-filter-wrap .wg-col-filter-input { padding-right:24px; width:100%; box-sizing:border-box; }',
            '.wg-col-filter-clear { position:absolute; right:5px; top:50%; transform:translateY(-50%); display:none; align-items:center; justify-content:center; width:16px; height:16px; background:#E2E8F0; border:none; border-radius:50%; cursor:pointer; color:#64748B; font-size:11px; padding:0; transition:background .12s; }',
            '.wg-col-filter-clear:hover { background:#FECACA; color:#DC2626; }',
            '.wg-col-filter-wrap.has-value .wg-col-filter-clear { display:flex; }',
            '.wg-col-filter-wrap.has-value .wg-col-filter-input { border-color:var(--wg-primary); }',
            '.wg-col-filter-input { width:100%; padding:3px 7px; border:1px solid #CBD5E1; border-radius:5px; font-size:11px; font-family:inherit; color:var(--wg-text); background:#fff; outline:none; transition:border-color .12s; }',
            '.wg-col-filter-input:focus { border-color:var(--wg-primary); }',
            '.wg-col-filter-input::placeholder { color:#b0bcc4; font-size:10.5px; }',
            '#wg-row-tooltip { position:fixed; background:#1A2B3C; color:#E2EAF0; font-size:11px; font-weight:500; padding:5px 10px; border-radius:6px; pointer-events:none; opacity:0; transition:opacity .12s; z-index:99999; box-shadow:0 4px 14px rgba(0,0,0,0.4); white-space:nowrap; max-width:340px; overflow:hidden; text-overflow:ellipsis; font-family:inherit; }',
            '@media (max-width:600px) { .wms-grid-search-wrap { width:100%; } .wms-grid-toolbar-right { width:100%; } .wms-grid-pagination { flex-direction:column; align-items:flex-start; } }',
            '.wg-btn-refresh.loading i{animation:wgspin 0.7s linear infinite;}@keyframes wgspin{from{transform:rotate(0deg);}to{transform:rotate(360deg);}}',
            '.wg-grid-loader{position:absolute;inset:0;background:rgba(255,255,255,.65);display:flex;align-items:center;justify-content:center;z-index:999}.wg-dot-spinner{position:relative;width:40px;height:40px}.wg-dot-spinner div{transform-origin:20px 20px;animation:wg-dot-spin 1.2s linear infinite}.wg-dot-spinner div:after{content:" ";display:block;position:absolute;top:2px;left:18px;width:4px;height:8px;border-radius:20%;background:#2F6F8E}.wg-dot-spinner div:nth-child(1){transform:rotate(0deg);animation-delay:-1.1s}.wg-dot-spinner div:nth-child(2){transform:rotate(30deg);animation-delay:-1s}.wg-dot-spinner div:nth-child(3){transform:rotate(60deg);animation-delay:-.9s}.wg-dot-spinner div:nth-child(4){transform:rotate(90deg);animation-delay:-.8s}.wg-dot-spinner div:nth-child(5){transform:rotate(120deg);animation-delay:-.7s}.wg-dot-spinner div:nth-child(6){transform:rotate(150deg);animation-delay:-.6s}.wg-dot-spinner div:nth-child(7){transform:rotate(180deg);animation-delay:-.5s}.wg-dot-spinner div:nth-child(8){transform:rotate(210deg);animation-delay:-.4s}.wg-dot-spinner div:nth-child(9){transform:rotate(240deg);animation-delay:-.3s}.wg-dot-spinner div:nth-child(10){transform:rotate(270deg);animation-delay:-.2s}.wg-dot-spinner div:nth-child(11){transform:rotate(300deg);animation-delay:-.1s}.wg-dot-spinner div:nth-child(12){transform:rotate(330deg);animation-delay:0s}@keyframes wg-dot-spin{0%{opacity:1}100%{opacity:0}}'
        ].join('\n');
        style.textContent = css;
        document.head.appendChild(style);
    }

    var _rowTip = null;
    function getRowTip() {
        if (_rowTip) return _rowTip;
        _rowTip = document.createElement('div');
        _rowTip.id = 'wg-row-tooltip';
        if (document.body) {
            document.body.appendChild(_rowTip);
        } else {
            document.addEventListener('DOMContentLoaded', function () {
                document.body.appendChild(_rowTip);
            });
        }
        return _rowTip;
    }

    function esc(str) {
        if (str == null) return '';
        return String(str)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    }

    function uid(prefix) {
        return (prefix || 'wg') + '_' + Math.random().toString(36).slice(2, 8);
    }

    // ─── DEFAULT FEATURES ──────────────────────────────────────────────────────
    var DEFAULT_FEATURES = {
        toolbar: true,
        title: true,
        search: true,
        // Column-filter row starts collapsed everywhere. The toolbar's
        // filter-toggle button stays available so users can flip it on
        // when they actually need to filter.
        columnFilters: false,
        sorting: true,
        pagination: true,
        perPage: true,
        paginationInfo: true,
        export: true,
        refresh: true,
        filterToggle: true,
        rowTooltip: true,
        scrollHints: true,
        scrollBar: true,
    };

    function mergeFeatures(userFeatures) {
        var f = {};
        for (var k in DEFAULT_FEATURES) f[k] = DEFAULT_FEATURES[k];
        if (userFeatures && typeof userFeatures === 'object') {
            for (var k2 in userFeatures) f[k2] = !!userFeatures[k2];
        }
        return f;
    }

    // ─── MAIN FACTORY ──────────────────────────────────────────────────────────
    function createGrid(opts) {
        injectCSS();

        var container = typeof opts.container === 'string'
            ? document.getElementById(opts.container)
            : opts.container;
        if (!container) { console.error('[WMSGrid] container not found:', opts.container); return null; }

        var id = uid('wmsg');
        var columns = opts.columns || [];
        var title = opts.title || '';
        var data = (opts.data || []).slice();
        var perPage = opts.perPage || 10;
        var emptyText = opts.emptyText || 'No records found';
        var exportName = opts.exportName || '';
        var height = opts.height || 'auto';
        var extraBtns = opts.extraButtons || [];
        var onRowClick = opts.onRowClick || null;
        var serverSide = opts.serverSide || false;
        var onPageChange = opts.onPageChange || null;
        var onSearch = opts.onSearch || null;
        var onSort = opts.onSort || null;

        var features = mergeFeatures(opts.features);

        var state = {
            page: 1,
            perPage: perPage,
            sortCol: -1,
            sortDir: 'asc',
            query: '',
            colFilters: columns.map(function () { return ''; })
        };

        // ── Build DOM skeleton ─────────────────────────────────────────────────
        container.innerHTML = '';

        var wrap = document.createElement('div');
        wrap.className = 'wms-grid-wrap';
        wrap.id = id + '_wrap';
        container.appendChild(wrap);

        var toolbar = document.createElement('div');
        toolbar.className = 'wms-grid-toolbar';
        toolbar.id = id + '_toolbar';
        wrap.appendChild(toolbar);

        var swipeWrap = document.createElement('div');
        swipeWrap.className = 'wg-swipe-wrap';
        wrap.appendChild(swipeWrap);

        var hintLeft = document.createElement('div');
        hintLeft.className = 'wg-scroll-hint left';
        hintLeft.id = id + '_hintL';
        hintLeft.innerHTML = '<i class="bx bx-chevron-left"></i>';
        swipeWrap.appendChild(hintLeft);

        var hintRight = document.createElement('div');
        hintRight.className = 'wg-scroll-hint right';
        hintRight.id = id + '_hintR';
        hintRight.innerHTML = '<i class="bx bx-chevron-right"></i>';
        swipeWrap.appendChild(hintRight);

        var tblWrap = document.createElement('div');
        tblWrap.className = 'wms-grid-tbl-wrap' + (height !== 'auto' ? ' fixed-height' : '');
        if (height !== 'auto') tblWrap.style.maxHeight = height;
        swipeWrap.appendChild(tblWrap);

        // ══════════════════════════════════════════════════════════════════════
        //  FIX 2 — Desktop mouse drag-to-scroll  (touch-action:pan-x pan-y in
        //  CSS handles native finger scroll; mouse drag handled below)
        // ══════════════════════════════════════════════════════════════════════
        (function () {
            var dragging = false;
            var startX = 0;
            var scrollL = 0;

            tblWrap.addEventListener('mousedown', function (e) {
                if (e.button !== 0) return;
                var tag = e.target.tagName;
                if (tag === 'INPUT' || tag === 'BUTTON' || tag === 'SELECT' ||
                    tag === 'A' || tag === 'TEXTAREA' || e.target.isContentEditable) return;
                dragging = true;
                startX = e.pageX - tblWrap.offsetLeft;
                scrollL = tblWrap.scrollLeft;
                tblWrap.classList.add('grabbing');
                e.preventDefault();
            });

            document.addEventListener('mousemove', function (e) {
                if (!dragging) return;
                e.preventDefault();
                var x = e.pageX - tblWrap.offsetLeft;
                var walk = x - startX;
                tblWrap.scrollLeft = scrollL - walk;
            }, { passive: false });

            document.addEventListener('mouseup', function () {
                if (!dragging) return;
                dragging = false;
                tblWrap.classList.remove('grabbing');
            });

            document.addEventListener('mouseleave', function () {
                if (!dragging) return;
                dragging = false;
                tblWrap.classList.remove('grabbing');
            });

            // ── FIX 2b: explicit touch scroll (belt-and-suspenders for browsers
            //    that block native overflow scroll inside sticky-column tables) ──
            var touchStartX = 0;
            var touchStartY = 0;
            var touchScrollL = 0;
            var touchScrollT = 0;
            var touchAxis = null; // 'h' | 'v' | null

            tblWrap.addEventListener('touchstart', function (e) {
                if (e.touches.length !== 1) return;
                touchStartX = e.touches[0].clientX;
                touchStartY = e.touches[0].clientY;
                touchScrollL = tblWrap.scrollLeft;
                touchScrollT = tblWrap.scrollTop;
                touchAxis = null;
            }, { passive: true });

            tblWrap.addEventListener('touchmove', function (e) {
                if (e.touches.length !== 1) return;
                var dx = touchStartX - e.touches[0].clientX;
                var dy = touchStartY - e.touches[0].clientY;

                // Determine dominant axis on first significant move
                if (!touchAxis) {
                    if (Math.abs(dx) > 4 || Math.abs(dy) > 4) {
                        touchAxis = Math.abs(dx) >= Math.abs(dy) ? 'h' : 'v';
                    } else {
                        return;
                    }
                }

                if (touchAxis === 'h') {
                    // Horizontal scroll — let the element handle it but nudge manually
                    // to help browsers that throttle momentum on sticky tables
                    tblWrap.scrollLeft = touchScrollL + dx;
                    e.preventDefault(); // prevent page scroll while swiping table
                }
                // Vertical axis: let browser handle natively (do not preventDefault)
            }, { passive: false });

            tblWrap.addEventListener('touchend', function () {
                touchAxis = null;
            }, { passive: true });
        })();
        // ══════════════════════════════════════════════════════════════════════

        var swipeBar = document.createElement('div');
        swipeBar.className = 'wg-swipe-bar';
        swipeBar.id = id + '_swipeBar';
        var swipeBarInner = document.createElement('div');
        swipeBarInner.className = 'wg-swipe-bar-inner';
        swipeBar.appendChild(swipeBarInner);
        swipeWrap.appendChild(swipeBar);

        var table = document.createElement('table');
        tblWrap.appendChild(table);

        var thead = document.createElement('thead');
        table.appendChild(thead);

        var theadRow = document.createElement('tr');
        thead.appendChild(theadRow);

        var colFilterRow = document.createElement('tr');
        colFilterRow.id = id + '_colfilterrow';
        thead.appendChild(colFilterRow);

        var tbody = document.createElement('tbody');
        tbody.id = id + '_tbody';
        table.appendChild(tbody);

        var pgBar = document.createElement('div');
        pgBar.className = 'wms-grid-pagination';
        pgBar.id = id + '_pgbar';
        wrap.appendChild(pgBar);

        var loader = document.createElement('div');
        loader.className = 'wg-grid-loader';
        loader.innerHTML = '<div class="wg-dot-spinner">' + '<div></div>'.repeat(12) + '</div>';
        loader.style.display = 'none';
        wrap.style.position = 'relative';
        wrap.appendChild(loader);

        applyAllFeatures();

        function show(el) { if (el) el.style.display = ''; }
        function hide(el) { if (el) el.style.display = 'none'; }
        function vis(el, flag) { if (el) el.style.display = flag ? '' : 'none'; }

        function applyAllFeatures() {
            vis(toolbar, features.toolbar);
            vis(hintLeft, features.scrollHints);
            vis(hintRight, features.scrollHints);
            vis(swipeBar, features.scrollBar);
            vis(pgBar, features.pagination);
            applyColFilterVisibility();
        }

        function applyColFilterVisibility() {
            var row = document.getElementById(id + '_colfilterrow');
            if (row) vis(row, features.columnFilters);
        }

        // ─────────────────────────────────────────────────────────────────────
        // TOOLBAR
        // ─────────────────────────────────────────────────────────────────────

        function renderToolbar() {
            toolbar.innerHTML = '';

            if (title && features.title) {
                var titleEl = document.createElement('div');
                titleEl.className = 'wms-grid-title';
                titleEl.id = id + '_titleEl';
                var iconHtml = opts.icon ? '<i class="bx ' + opts.icon + '"></i> ' : '';
                titleEl.innerHTML = iconHtml + esc(title) + ' <span class="wms-grid-count-pill" id="' + id + '_count">0 entries</span>';
                toolbar.appendChild(titleEl);
            }

            var right = document.createElement('div');
            right.className = 'wms-grid-toolbar-right';
            right.id = id + '_toolbarRight';
            toolbar.appendChild(right);

            var srchWrap = document.createElement('div');
            srchWrap.className = 'wms-grid-search-wrap';
            srchWrap.id = id + '_srchWrap';

            // ══════════════════════════════════════════════════════════════════
            //  FIX 1 — Prevent browser autofill on the search box.
            //
            //  Browsers ignore autocomplete="off" for search-like inputs, so we
            //  use three complementary tricks:
            //    1. autocomplete="new-password"  — browsers treat it as a new
            //       credential field and skip credential autofill.
            //    2. A unique random `name` attribute — prevents the browser from
            //       matching the field to any saved form value.
            //    3. A visually-hidden honeypot <input type="text"> placed first
            //       in the wrapper — browsers fill the first text input they find
            //       (the honeypot) and leave the real one alone.
            // ══════════════════════════════════════════════════════════════════
            var honeypotName = 'wg_hp_' + Math.random().toString(36).slice(2, 10);
            var srchName = 'wg_s_' + Math.random().toString(36).slice(2, 10);

            srchWrap.innerHTML =
                // honeypot — absorbs any autofill attempt
                '<input type="text" name="' + honeypotName + '" tabindex="-1" aria-hidden="true"' +
                '  style="position:absolute;width:1px;height:1px;opacity:0;pointer-events:none;left:-9999px">' +
                // search icon
                '<i class="bx bx-search" style="position:absolute;left:9px;top:50%;transform:translateY(-50%);' +
                'color:#94A3B8;font-size:15px;pointer-events:none;z-index:1;"></i>' +
                // real search input
                '<input class="wms-grid-search-input"' +
                '  type="text"' +
                '  autocomplete="new-password"' +
                '  autocorrect="off"' +
                '  autocapitalize="off"' +
                '  spellcheck="false"' +
                '  name="' + srchName + '"' +
                '  id="' + id + '_srch"' +
                '  placeholder="' + esc(opts.searchPlaceholder || 'Search...') + '"' +
                '  value="' + esc(state.query) + '">' +
                // clear button
                '<button class="wms-grid-search-reset" id="' + id + '_srchReset" title="Clear">' +
                '  <i class="bx bx-x" style="font-size:13px;line-height:1;"></i>' +
                '</button>';

            vis(srchWrap, features.search);
            right.appendChild(srchWrap);

            var matchChip = document.createElement('span');
            matchChip.id = id + '_matchChip';
            matchChip.className = 'wg-match-chip';
            matchChip.style.display = 'none';
            right.appendChild(matchChip);

            extraBtns.forEach(function (btn) {
                var b = document.createElement('button');
                b.className = 'wg-btn ' + (btn.cls || 'wg-btn-outline');
                b.id = btn.id ? btn.id : '';
                var iconPart = btn.icon ? '<i class="bx ' + btn.icon + '" style="font-size:13px;"></i> ' : '';
                b.innerHTML = iconPart + esc(btn.label || '');
                b.addEventListener('click', function (e) { if (btn.onClick) btn.onClick(e, data); });
                right.appendChild(b);
            });

            if (exportName) {
                var expBtn = document.createElement('button');
                expBtn.className = 'wg-btn wg-btn-outline';
                expBtn.id = id + '_exportBtn';
                expBtn.title = 'Export to Excel';
                expBtn.innerHTML = '<i class="bx bx-table" style="font-size:14px;"></i> Export Excel';
                expBtn.addEventListener('click', doExport);
                vis(expBtn, features.export);
                right.appendChild(expBtn);
            }

            var cfBtn = document.createElement('button');
            cfBtn.className = 'wg-btn wg-btn-ghost';
            cfBtn.id = id + '_cfBtn';
            cfBtn.title = 'Toggle column filters';
            cfBtn.innerHTML = '<i class="bx bx-filter-alt" style="font-size:14px;"></i>';
            cfBtn.addEventListener('click', toggleColFilters);
            vis(cfBtn, features.filterToggle);
            right.appendChild(cfBtn);

            var refreshBtn = document.createElement('button');
            refreshBtn.className = 'wg-btn wg-btn-ghost';
            refreshBtn.id = id + '_refreshBtn';
            refreshBtn.title = 'Refresh';
            refreshBtn.innerHTML = '<i class="bx bx-refresh" style="font-size:14px;"></i>';
            refreshBtn.addEventListener('click', function () {
                if (opts.serverSide && typeof onPageChange === 'function') {
                    state.page = 1; state.query = ''; state.sortCol = -1;
                    state.sortDir = 'asc';
                    state.colFilters = columns.map(function () { return ''; });
                    var srchInp = document.getElementById(id + '_srch');
                    if (srchInp) srchInp.value = '';
                    var inputs = document.querySelectorAll('#' + id + '_colfilterrow input');
                    inputs.forEach(function (inp) { inp.value = ''; });
                    onPageChange(1, state.perPage, '', null, 'asc', []);
                } else {
                    state.page = 1; state.query = ''; state.sortCol = -1;
                    state.sortDir = 'asc';
                    state.colFilters = columns.map(function () { return ''; });
                    renderHeaders(); render();
                }
            });
            vis(refreshBtn, features.refresh);
            right.appendChild(refreshBtn);

            // ── Search events ──────────────────────────────────────────────────
            var srchInp = document.getElementById(id + '_srch');
            var srchReset = document.getElementById(id + '_srchReset');
            var _srchDebTimer = null;

            if (srchInp) {
                srchInp.addEventListener('input', function () {
                    if (srchInp.value.length < 2) return;

                    srchWrap.classList.toggle('has-query', srchInp.value.length > 0);
                    clearTimeout(_srchDebTimer);

                    if (srchInp.value.length === 0) {
                        state.query = ''; state.page = 1;
                        srchWrap.classList.remove('has-query');
                        if (onSearch) onSearch('', state.colFilters);
                        else render();
                        return;
                    }

                    _srchDebTimer = setTimeout(function () {
                        state.query = srchInp.value.toLowerCase().trim();
                        state.page = 1;
                        if (onSearch) onSearch(state.query, state.colFilters);
                        else render();
                    }, 1000);
                });

                srchInp.addEventListener('keydown', function (e) {
                    if (srchInp.value.length < 2) return;

                    if (e.key === 'Enter') {
                        clearTimeout(_srchDebTimer);
                        if (srchInp.value.length > 0 && srchInp.value.length < 2) return;
                        state.query = srchInp.value.toLowerCase().trim();
                        state.page = 1;
                        srchWrap.classList.toggle('has-query', state.query.length > 0);
                        if (onSearch) onSearch(state.query, state.colFilters);
                        else render();
                    } else if (e.key === 'Escape') {
                        clearTimeout(_srchDebTimer);
                        srchInp.value = ''; state.query = ''; state.page = 1;
                        srchWrap.classList.remove('has-query');
                        if (onSearch) onSearch('', state.colFilters);
                        else render();
                    }
                });
            }

            if (srchReset) {
                srchReset.addEventListener('click', function () {
                    clearTimeout(_srchDebTimer);
                    if (srchInp) srchInp.value = '';
                    state.query = ''; state.page = 1;
                    srchWrap.classList.remove('has-query');
                    if (onSearch) onSearch('', state.colFilters);
                    else render();
                });
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // HEADERS
        // ─────────────────────────────────────────────────────────────────────

        function renderHeaders() {
            theadRow.innerHTML = '';
            columns.forEach(function (col, i) {
                var th = document.createElement('th');
                var sortable = features.sorting && col.sortable !== false && col.field;
                if (sortable) th.className = 'wg-sortable';

                // ── FIX 3: mark sticky action column header ──
                if (col.stickyRight) th.classList.add('wg-action-col');

                var arrow = sortable ? '<i class="wg-sort-arrow bx bx-sort-alt-2" style="font-size:13px;margin-left:3px;opacity:0.4;"></i>' : '';
                th.innerHTML = esc(col.header || '') + arrow;
                if (sortable && i === state.sortCol) {
                    th.classList.add('wg-sort-' + state.sortDir);
                }
                if (sortable) {
                    (function (idx) {
                        th.addEventListener('click', function () {
                            if (state.sortCol === idx) {
                                state.sortDir = state.sortDir === 'asc' ? 'desc' : 'asc';
                            } else {
                                state.sortCol = idx; state.sortDir = 'asc';
                            }
                            if (onSort) {
                                onSort(columns[idx].field, state.sortDir, state.query, state.colFilters);
                            }
                            renderHeaders();
                            if (!onSort) render();
                        });
                    })(i);
                }
                if (col.width) th.style.minWidth = col.width;
                theadRow.appendChild(th);
            });

            // ── Column filter row ─────────────────────────────────────────────
            colFilterRow.innerHTML = '';
            columns.forEach(function (col, i) {
                var td = document.createElement('td');
                td.style.cssText = 'padding:5px 8px; background:#F8FAFC; border-bottom:1px solid #E2E8F0;';

                // ── FIX 3: sticky for filter cell of action column ──
                if (col.stickyRight) {
                    td.classList.add('wg-action-col');
                    td.style.background = '#F8FAFC';
                }

                if (col.field) {
                    var filterWrap = document.createElement('div');
                    filterWrap.className = 'wg-col-filter-wrap';
                    if (state.colFilters[i]) filterWrap.classList.add('has-value');

                    var inp = document.createElement('input');
                    inp.type = 'text';
                    inp.className = 'wg-col-filter-input';
                    inp.placeholder = 'Filter ' + (col.header || '') + '...';
                    inp.value = state.colFilters[i] || '';

                    var clrBtn = document.createElement('button');
                    clrBtn.type = 'button';
                    clrBtn.className = 'wg-col-filter-clear';
                    clrBtn.title = 'Clear';
                    clrBtn.innerHTML = '<i class="bx bx-x" style="font-size:11px;line-height:1;"></i>';

                    (function (idx, wrap, clearBtn, input) {
                        var _debTimer = null;

                        function applyFilter() {
                            state.colFilters[idx] = input.value.toLowerCase().trim();
                            state.page = 1;
                            wrap.classList.toggle('has-value', input.value.length > 0);
                            if (onSearch) onSearch(state.query, state.colFilters);
                            else render();
                        }

                        input.addEventListener('input', function () {
                            if (input.value.length < 2) return;

                            wrap.classList.toggle('has-value', input.value.length > 0);
                            clearTimeout(_debTimer);
                            _debTimer = setTimeout(applyFilter, 1000);
                        });

                        input.addEventListener('keydown', function (e) {
                            if (input.value.length < 2) return;

                            if (e.key === 'Enter') {
                                clearTimeout(_debTimer);
                                applyFilter();
                            } else if (e.key === 'Escape') {
                                clearTimeout(_debTimer);
                                input.value = '';
                                state.colFilters[idx] = '';
                                state.page = 1;
                                wrap.classList.remove('has-value');
                                if (onSearch) onSearch(state.query, state.colFilters);
                                else render();
                            }
                        });

                        clearBtn.addEventListener('click', function () {
                            clearTimeout(_debTimer);
                            input.value = '';
                            state.colFilters[idx] = '';
                            state.page = 1;
                            wrap.classList.remove('has-value');
                            if (onSearch) onSearch(state.query, state.colFilters);
                            else render();
                        });
                    })(i, filterWrap, clrBtn, inp);

                    filterWrap.appendChild(inp);
                    filterWrap.appendChild(clrBtn);
                    td.appendChild(filterWrap);
                }
                colFilterRow.appendChild(td);
            });

            applyColFilterVisibility();
        }

        function toggleColFilters() {
            var row = document.getElementById(id + '_colfilterrow');
            if (row) row.style.display = (row.style.display === 'none') ? '' : 'none';
        }

        // ─────────────────────────────────────────────────────────────────────
        // BODY
        // ─────────────────────────────────────────────────────────────────────

        function renderBody(pageData) {
            tbody.innerHTML = '';
            if (!pageData.length) {
                var emptyRow = document.createElement('tr');
                emptyRow.className = 'wg-empty-row';
                var emptyTd = document.createElement('td');
                emptyTd.colSpan = columns.length;
                emptyTd.textContent = emptyText;
                emptyRow.appendChild(emptyTd);
                tbody.appendChild(emptyRow);
                return;
            }
            var tip = features.rowTooltip ? getRowTip() : null;
            pageData.forEach(function (row) {
                var tr = document.createElement('tr');
                if (onRowClick) {
                    tr.className = 'wg-clickable';
                    tr.addEventListener('click', function (e) {
                        if (e.target.closest && (e.target.closest('button') || e.target.closest('a'))) return;
                        onRowClick(row, tr);
                    });
                }
                columns.forEach(function (col) {
                    var td = document.createElement('td');
                    var rawVal = col.field ? row[col.field] : '';
                    if (typeof col.render === 'function') {
                        td.innerHTML = col.render(rawVal, row);
                    } else {
                        td.textContent = rawVal != null ? rawVal : '';
                    }
                    if (col.align) td.style.textAlign = col.align;
                    if (col.mono) td.style.fontFamily = '"JetBrains Mono", monospace';
                    if (col.bold) td.style.fontWeight = '700';
                    if (col.nowrap) td.style.whiteSpace = 'nowrap';
                    if (col.width) td.style.minWidth = col.width;

                    // ── FIX 3: sticky action column body cell ──
                    if (col.stickyRight) td.classList.add('wg-action-col');

                    // Column-specific tooltip: show this cell's exact value on hover
                    if (tip && col.field && rawVal != null && rawVal !== '') {
                        var tipText = String(rawVal);
                        td.addEventListener('mouseover', function (e) {
                            e.stopPropagation();
                            tip.textContent = tipText;
                            tip.style.opacity = '1';
                            moveTip(e);
                        });
                        td.addEventListener('mousemove', moveTip);
                        td.addEventListener('mouseout', function () { tip.style.opacity = '0'; });
                    }

                    tr.appendChild(td);
                });
                tbody.appendChild(tr);
            });
        }

        function moveTip(e) {
            var tip = getRowTip();
            var x = e.clientX + 14, y = e.clientY - 32;
            if (x + 350 > window.innerWidth) x = e.clientX - 360;
            if (y < 8) y = e.clientY + 16;
            tip.style.left = x + 'px';
            tip.style.top = y + 'px';
        }

        // ─────────────────────────────────────────────────────────────────────
        // PAGINATION
        // ─────────────────────────────────────────────────────────────────────

        function setPaginationInfo(totalRecords, currentPage, pageSize) {
            if (currentPage) state.page = currentPage;
            if (pageSize) state.perPage = pageSize;
            renderPagination(totalRecords, totalRecords);
        }

        function renderPagination(total, filtered, fromRow, toRow) {
            var countEl0 = document.getElementById(id + '_count');
            if (countEl0) countEl0.textContent = total + ' entries';
            if (!features.pagination) { pgBar.innerHTML = ''; vis(pgBar, false); return; }
            vis(pgBar, true);

            var perPg = state.perPage;
            var page = state.page;
            var maxPg = Math.max(1, Math.ceil(filtered / perPg));
            var from = filtered === 0 ? 0 : (fromRow || Math.min((page - 1) * perPg + 1, filtered));
            var to = filtered === 0 ? 0 : (toRow || Math.min(page * perPg, filtered));

            var countEl = document.getElementById(id + '_count');
            if (countEl) countEl.textContent = total + ' entries';

            var matchEl = document.getElementById(id + '_matchChip');
            if (matchEl) {
                var hasFilter = state.query || state.colFilters.some(function (f) { return !!f; });
                if (hasFilter && filtered < total) {
                    matchEl.textContent = filtered + ' / ' + total + ' match';
                    matchEl.style.display = '';
                } else {
                    matchEl.style.display = 'none';
                }
            }

            if (filtered === 0) { pgBar.innerHTML = ''; return; }
            pgBar.innerHTML = '';

            if (features.paginationInfo) {
                var info = document.createElement('div');
                info.className = 'wg-pg-info';
                if (filtered === total) {
                    info.innerHTML = 'Showing <strong>' + from + '-' + to + '</strong> of <strong>' + filtered + '</strong> entries';
                } else {
                    info.innerHTML = 'Showing <strong>' + from + '-' + to + '</strong> of <strong>' + filtered + '</strong> (filtered from <strong>' + total + '</strong>)';
                }
                pgBar.appendChild(info);
            }

            var btnsWrap = document.createElement('div');
            btnsWrap.className = 'wg-pg-btns';
            function makeBtn(label, targetPage, active, disabled) {
                var b = document.createElement('button');
                b.className = 'wg-pg-btn' + (active ? ' active' : '');
                b.disabled = disabled || false;
                b.innerHTML = label;
                if (!disabled) {
                    (function (tp) {
                        b.addEventListener('click', function () {
                            state.page = tp;
                            if (onPageChange) onPageChange(tp, state.perPage, state.query, null, state.sortDir, state.colFilters);
                            else render();
                        });
                    })(targetPage);
                }
                return b;
            }
            btnsWrap.appendChild(makeBtn('<i class=\'bx bx-chevron-left\' style=\'font-size:14px;\'></i>', page - 1, false, page <= 1));
            var pages = [];
            if (maxPg <= 7) {
                for (var p = 1; p <= maxPg; p++) pages.push(p);
            } else {
                pages.push(1);
                if (page > 3) pages.push('...');
                for (var p2 = Math.max(2, page - 1); p2 <= Math.min(maxPg - 1, page + 1); p2++) pages.push(p2);
                if (page < maxPg - 2) pages.push('...');
                pages.push(maxPg);
            }
            pages.forEach(function (p) {
                if (p === '...') {
                    var sep = document.createElement('span');
                    sep.className = 'wg-pg-sep'; sep.textContent = '...';
                    btnsWrap.appendChild(sep);
                } else {
                    btnsWrap.appendChild(makeBtn(p, p, p === page, false));
                }
            });
            btnsWrap.appendChild(makeBtn('<i class=\'bx bx-chevron-right\' style=\'font-size:14px;\'></i>', page + 1, false, page >= maxPg));
            pgBar.appendChild(btnsWrap);

            if (features.perPage) {
                var perPgWrap = document.createElement('div');
                perPgWrap.className = 'wg-per-page';
                perPgWrap.appendChild(document.createTextNode('Rows: '));
                var sel = document.createElement('select');
                sel.className = 'wms-no-dd';
                [10, 25, 50, 100].forEach(function (n) {
                    var op = document.createElement('option');
                    op.value = n; op.textContent = n;
                    if (n === perPg) op.selected = true;
                    sel.appendChild(op);
                });
                sel.addEventListener('change', function () {
                    state.perPage = parseInt(this.value) || 10;
                    state.page = 1;
                    if (onPageChange) onPageChange(1, state.perPage, state.query, null, state.sortDir, state.colFilters);
                    else render();
                });
                perPgWrap.appendChild(sel);
                pgBar.appendChild(perPgWrap);
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // CLIENT-SIDE FILTER + SORT + PAGINATE
        // ─────────────────────────────────────────────────────────────────────

        function getFilteredSorted() {
            var q = state.query;
            var cf = state.colFilters;
            var fields = columns.map(function (c) { return c.field || null; });

            var filtered = data.filter(function (row) {
                if (q) {
                    var found = false;
                    for (var fi = 0; fi < fields.length; fi++) {
                        if (fields[fi] && String(row[fields[fi]] || '').toLowerCase().indexOf(q) !== -1) { found = true; break; }
                    }
                    if (!found) return false;
                }
                for (var ci = 0; ci < cf.length; ci++) {
                    if (cf[ci] && fields[ci] && String(row[fields[ci]] || '').toLowerCase().indexOf(cf[ci]) === -1) return false;
                }
                return true;
            });

            if (features.sorting && state.sortCol >= 0 && columns[state.sortCol] && columns[state.sortCol].field) {
                var sf = columns[state.sortCol].field;
                var dir = state.sortDir === 'asc' ? 1 : -1;
                filtered.sort(function (a, b) {
                    var av = a[sf] != null ? a[sf] : '';
                    var bv = b[sf] != null ? b[sf] : '';
                    if (typeof av === 'number' && typeof bv === 'number') return (av - bv) * dir;
                    return String(av).localeCompare(String(bv)) * dir;
                });
            }
            return filtered;
        }

        // ─────────────────────────────────────────────────────────────────────
        // EXPORT
        // ─────────────────────────────────────────────────────────────────────

        function doExport() {
            var headers = columns.filter(function (c) { return c.field; }).map(function (c) { return c.header || c.field; });
            var fields = columns.filter(function (c) { return c.field; }).map(function (c) { return c.field; });
            var rows = data.map(function (row) {
                return fields.map(function (f) { return row[f] != null ? row[f] : ''; });
            });
            if (typeof XLSX !== 'undefined') {
                var wsData = [headers].concat(rows);
                var ws = XLSX.utils.aoa_to_sheet(wsData);
                var wb = XLSX.utils.book_new();
                XLSX.utils.book_append_sheet(wb, ws, exportName.substring(0, 31));
                XLSX.writeFile(wb, exportName + '_' + new Date().toISOString().split('T')[0] + '.xlsx');
                return;
            }
            var csv = [headers.join(',')]
                .concat(rows.map(function (r) {
                    return r.map(function (v) { return '"' + String(v).replace(/"/g, '""') + '"'; }).join(',');
                })).join('\n');
            var blob = new Blob([csv], { type: 'text/csv' });
            var a = document.createElement('a');
            a.href = URL.createObjectURL(blob);
            a.download = (exportName || 'export') + '.csv';
            a.click();
        }

        // ─────────────────────────────────────────────────────────────────────
        // RENDER (client-side)
        // ─────────────────────────────────────────────────────────────────────

        function render() {
            var filtered = getFilteredSorted();
            var total = data.length;
            var perPg = state.perPage;
            var page = state.page;
            var maxPg = Math.max(1, Math.ceil(filtered.length / perPg));
            if (page > maxPg) { state.page = maxPg; page = maxPg; }
            var from = (page - 1) * perPg;
            var pageData = filtered.slice(from, from + perPg);
            renderBody(pageData);
            renderPagination(total, filtered.length, from + 1, Math.min(from + perPg, filtered.length));
        }

        renderToolbar();
        renderHeaders();
        render();
        applyAllFeatures();

        // ─────────────────────────────────────────────────────────────────────
        // PUBLIC API
        // ─────────────────────────────────────────────────────────────────────

        var api = {
            setData: function (newData) {
                data = (newData || []).slice(); state.page = 1; render(); return this;
            },
            getData: function () { return data; },
            refresh: function () { render(); return this; },
            addRow: function (row) { data.unshift(row); state.page = 1; render(); return this; },
            updateRow: function (field, value, newRow) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i][field] === value) { data[i] = newRow; break; }
                }
                render(); return this;
            },
            deleteRow: function (field, value) {
                data = data.filter(function (r) { return r[field] !== value; });
                state.page = 1; render(); return this;
            },
            search: function (query) {
                state.query = (query || '').toLowerCase().trim();
                state.page = 1;
                var inp = document.getElementById(id + '_srch');
                if (inp) inp.value = query || '';
                render(); return this;
            },
            reset: function () {
                state.query = ''; state.page = 1; state.sortCol = -1; state.sortDir = 'asc';
                state.colFilters = columns.map(function () { return ''; });
                var inp = document.getElementById(id + '_srch');
                if (inp) inp.value = '';
                renderHeaders(); render(); return this;
            },
            getFilteredData: function () { return getFilteredSorted(); },
            getPageSize: function () { return state.perPage; },
            getSearchTerm: function () { return state.query; },
            getPage: function () { return state.page; },
            getColFilters: function () { return state.colFilters; },
            getColumns: function () { return columns; },
            getFeatures: function () { return features; },
            showLoader: function () { loader.style.display = 'flex'; },
            hideLoader: function () { loader.style.display = 'none'; },
            setPaginationInfo: setPaginationInfo,
            el: wrap,
            id: id,

            setFeature: function (featureName, enabled) {
                features[featureName] = !!enabled;
                _applyFeature(featureName, !!enabled);
                return this;
            },
            toggleFeature: function (featureName) {
                return this.setFeature(featureName, !features[featureName]);
            },
            setFeatures: function (featureMap) {
                for (var k in featureMap) {
                    if (Object.prototype.hasOwnProperty.call(featureMap, k)) {
                        features[k] = !!featureMap[k];
                        _applyFeature(k, !!featureMap[k]);
                    }
                }
                return this;
            }
        };

        function _applyFeature(name, enabled) {
            switch (name) {
                case 'toolbar': vis(toolbar, enabled); break;
                case 'title': vis(document.getElementById(id + '_titleEl'), enabled); break;
                case 'search': vis(document.getElementById(id + '_srchWrap'), enabled); break;
                case 'columnFilters': applyColFilterVisibility(); break;
                case 'sorting': renderHeaders(); break;
                case 'pagination': render(); break;
                case 'paginationInfo': render(); break;
                case 'perPage': render(); break;
                case 'export': vis(document.getElementById(id + '_exportBtn'), enabled); break;
                case 'refresh': vis(document.getElementById(id + '_refreshBtn'), enabled); break;
                case 'filterToggle': vis(document.getElementById(id + '_cfBtn'), enabled); break;
                case 'rowTooltip': render(); break;
                case 'scrollHints':
                    vis(document.getElementById(id + '_hintL'), enabled);
                    vis(document.getElementById(id + '_hintR'), enabled);
                    break;
                case 'scrollBar': vis(document.getElementById(id + '_swipeBar'), enabled); break;
                default: console.warn('[WMSGrid] Unknown feature:', name);
            }
        }

        return api;
    }

    // ═══════════════════════════════════════════════════════════════════════
    //  DYNAMIC COLUMN ENGINE
    // ═══════════════════════════════════════════════════════════════════════

    var DYN_PATTERNS = [
        { test: /^(isactive|isenabled|isdeleted|isverified)$/i, type: 'bool-pill' },
        { test: /^ishandlingcharged$/i, type: 'bool-yesno' },
        { test: /(id|code|no)$/i, type: 'mono' },
        { test: /(date|time|createdon|modifiedon|_at)$/i, type: 'date' },
        { test: /(amount|price|salary|total|value|cost|rate)$/i, type: 'currency' },
        { test: /(percent|pct|ratio)$/i, type: 'percent' },
        { test: /email/i, type: 'email' },
        { test: /(phone|mobile|contact)/i, type: 'phone' },
        { test: /(description|remark|note|comment|details)/i, type: 'fallback' },
    ];

    var DYN_RENDERERS = {
        'bool-pill': function (v) {
            if (v === true || v === 1 || v === 'Active' || v === 'Yes')
                return '<span class="wg-pill wg-pill-green">Active</span>';
            if (v === false || v === 0 || v === 'Inactive' || v === 'No')
                return '<span class="wg-pill wg-pill-amber">Inactive</span>';
            return '<span class="wg-pill wg-pill-pu">' + (v || '—') + '</span>';
        },
        'bool-yesno': function (v) {
            if (v === true || v === 1 || v === 'true' || v === 'Yes')
                return '<span class="wg-pill wg-pill-green">Yes</span>';
            return '<span class="wg-pill wg-pill-red">No</span>';
        },
        'mono': function (v) {
            if (v == null || v === '') return '<span style="color:#CBD5E1">—</span>';
            return '<span style="font-family:monospace;color:#2F6F8E;font-weight:700">' + v + '</span>';
        },
        'date': function (v) {
            if (!v) return '<span style="color:#CBD5E1">—</span>';
            try {
                var d = new Date(v); if (isNaN(d)) return v;
                return '<span style="white-space:nowrap;font-size:12px">' +
                    d.toLocaleDateString('en-IN', { day: '2-digit', month: 'short', year: 'numeric' }) + '</span>';
            } catch (e) { return v; }
        },
        'currency': function (v) {
            if (v == null || v === '') return '<span style="color:#CBD5E1">—</span>';
            var n = parseFloat(v); if (isNaN(n)) return v;
            return '<span style="font-family:monospace">&#8377;' +
                n.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) + '</span>';
        },
        'percent': function (v) {
            if (v == null || v === '') return '<span style="color:#CBD5E1">—</span>';
            return '<span>' + parseFloat(v).toFixed(2) + '%</span>';
        },
        'email': function (v) {
            if (!v) return '<span style="color:#CBD5E1">—</span>';
            return '<a href="mailto:' + v + '" style="color:#2F6F8E;text-decoration:none">' + v + '</a>';
        },
        'phone': function (v) {
            if (!v) return '<span style="color:#CBD5E1">—</span>';
            return '<a href="tel:' + v + '" style="color:#2F6F8E;text-decoration:none">' + v + '</a>';
        },
        'fallback': function (v) {
            return v || '<span style="color:#CBD5E1">No description</span>';
        },
        'default': function (v) {
            return (v != null && v !== '') ? v : '<span style="color:#CBD5E1">—</span>';
        }
    };

    function dynDetectType(field) {
        for (var i = 0; i < DYN_PATTERNS.length; i++) {
            if (DYN_PATTERNS[i].test.test(field)) return DYN_PATTERNS[i].type;
        }
        return 'default';
    }

    function dynHumanize(f) {
        return f.replace(/([A-Z])/g, ' $1').replace(/^./, function (c) { return c.toUpperCase(); }).trim();
    }

    function dynGuessWidth(f, type) {
        if (type === 'mono' || /^.*(id|code|no)$/i.test(f)) return '70px';
        if (type === 'bool-pill' || /status|active/i.test(f)) return '100px';
        if (type === 'date') return '130px';
        if (type === 'currency' || type === 'percent') return '120px';
        if (type === 'email') return '200px';
        if (type === 'fallback') return '220px';
        if (/name/i.test(f)) return '180px';
        return '150px';
    }

    function buildDynamicColumns(result, opts) {
        opts = opts || {};
        var hidden = opts.hiddenFields || [];
        var ovr = opts.overrides || {};
        var srcMeta = result.columns || [];
        var firstRow = (result.data && result.data[0]) ? result.data[0] : {};

        var fields = srcMeta.length > 0
            ? srcMeta.filter(function (m) { return hidden.indexOf(m.field || m.Field) === -1; })
            : Object.keys(firstRow)
                .filter(function (k) { return hidden.indexOf(k) === -1; })
                .map(function (k) { return { field: k }; });

        var cols = fields.map(function (meta) {
            var f = meta.field || meta.Field || '';
            var type = meta.type || meta.Type || dynDetectType(f);
            var renderer = DYN_RENDERERS[type] || DYN_RENDERERS['default'];
            var col = {
                field: f,
                header: meta.header || meta.Header || meta.displayName || dynHumanize(f),
                sortable: meta.sortable !== false && meta.Sortable !== false,
                width: meta.width || meta.Width || dynGuessWidth(f, type),
                render: (function (r) { return function (v) { return r(v); }; })(renderer)
            };
            if (meta.bold || meta.Bold) col.bold = true;
            if (meta.nowrap || meta.Nowrap) col.nowrap = true;
            if (meta.align || meta.Align) col.align = meta.align || meta.Align;

            if (ovr[f]) {
                var o = ovr[f];
                for (var k in o) { if (k !== 'renderType') col[k] = o[k]; }
                if (o.renderType && DYN_RENDERERS[o.renderType]) {
                    col.render = (function (rt) { return function (v) { return DYN_RENDERERS[rt](v); }; })(o.renderType);
                }
            }
            return col;
        });

        // ── FIX 3: action column always gets stickyRight:true ──
        if (opts.actionColumn) {
            cols.push(Object.assign(
                { header: 'Actions', sortable: false, nowrap: true, width: '180px', stickyRight: true },
                opts.actionColumn
            ));
        }
        return cols;
    }

    function dynGetSignature(result) {
        if (result.columns && result.columns.length)
            return result.columns.map(function (c) { return c.field || c.Field; }).join(',');
        if (result.data && result.data.length)
            return Object.keys(result.data[0]).join(',');
        return '';
    }

    var _origCreate = createGrid;
    function createGridWithDynamic(opts) {
        var isDynamic = !!opts.dynamic;
        var dynOpts = isDynamic ? {
            hiddenFields: opts.hiddenFields || [],
            overrides: opts.overrides || {},
            actionColumn: opts.actionColumn || null
        } : null;

        if (isDynamic && (!opts.columns || opts.columns.length === 0)) {
            opts = Object.assign({}, opts, { columns: [] });
        }

        var grid = _origCreate(opts);
        if (!grid || !isDynamic) return grid;

        var _colSig = '';

        grid.loadResult = function (result) {
            var sig = dynGetSignature(result);
            if (sig !== _colSig) {
                _colSig = sig;
                var newCols = buildDynamicColumns(result, dynOpts);
                var existing = grid.getColumns();
                existing.length = 0;
                newCols.forEach(function (c) { existing.push(c); });
                grid.reset();
            }
            grid.setData(result.data || []);
            if (result.totalCount !== undefined) {
                grid.setPaginationInfo(result.totalCount, result.page, result.pageSize);
            }
            return grid;
        };

        return grid;
    }

    global.WMSGrid = {
        create: createGridWithDynamic,
        injectCSS: injectCSS,
        version: '3.1',
        DEFAULT_FEATURES: DEFAULT_FEATURES,
        dynRenderers: DYN_RENDERERS,
        dynDetectType: dynDetectType,
        dynHumanize: dynHumanize,
    };

})(window);