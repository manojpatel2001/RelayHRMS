/* ══════════════════════════════════════════════════════════════════════════════
   WMS SEARCHABLE DROPDOWN ENGINE — extracted from WMS_UI wms-app.js
   (Single + Multi select, lines ~9361-9956 of the source file)

   Auto-init scans the whole document for every plain <select>, matching
   WMS's own behavior. DevExtreme widgets (dxSelectBox/dxTagBox) and the
   legacy searchableSelect.js widget are both built on <div>/<input> markup,
   never on a real <select>, so they never match this scan and cannot
   collide with it. Add the `wms-no-dd` class to a <select> to opt it out.
   Manual `WmsSingleDropdown(el)` / `WmsMultiDropdown(el)` calls still work
   on any element regardless of scope.
   ══════════════════════════════════════════════════════════════════════════════ */

// Single Searchable Dropdown
(function () {
    'use strict';

    function debounce(func, wait) {
        let timeout;
        return function () {
            const context = this, args = arguments;
            clearTimeout(timeout);
            timeout = setTimeout(() => func.apply(context, args), wait);
        };
    }

    function fire(sel) {
        sel.dispatchEvent(new Event('change', { bubbles: true }));
    }

    function getOpts(sel) {
        // NOTE: do not fall back to o.text when o.value is falsy. A <select>'s
        // native .value getter already returns the option's text content when
        // no value attribute is present (per spec), so the fallback is only
        // ever reached for options with an explicit value="" (a deliberate
        // blank/placeholder sentinel) -- and clobbering that to the option's
        // own label text breaks the placeholder-skip check in renderOpts(),
        // making the placeholder render as a normal, clickable-looking row
        // whose click silently fails to match any real <option> (no visible
        // change, perceived as the dropdown "not letting you select").
        return Array.from(sel.options).map(function (o) {
            return { value: o.value, text: o.text, selected: o.selected };
        });
    }

    function closeAll() {
        document.querySelectorAll('.wms-dd-panel.open, .wms-ms-panel.open').forEach(function (p) {
            p.classList.remove('open');
            if (p._trigger) p._trigger.classList.remove('open');
        });
    }

    // Bootstrap's modal FocusTrap forces focus back inside the modal whenever
    // focus lands on an element the modal doesn't `.contains()`. Our panel is
    // normally appended to <body> (so it can escape modal-body's overflow
    // clipping), but that puts it *outside* the modal's DOM subtree, so the
    // trap immediately steals focus back from the search input on open —
    // breaking typing, Escape-to-close, and making the dropdown look like it
    // randomly fails to open. Appending the panel inside the modal element
    // itself (still `position: fixed`, so overflow/clipping is unaffected)
    // keeps it inside the trap's containment check while staying visually
    // and positionally identical.
    function panelHost(trigger) {
        return trigger.closest('.modal') || document.body;
    }

    function posPanel(panel, trigger) {
        if (!panel || !trigger || !document.body.contains(panel)) {
            console.warn("Panel or trigger not found or not in DOM.");
            return;
        }
        try {
            const r = trigger.getBoundingClientRect();
            const pw = Math.max(r.width, 220);
            const spaceBelow = window.innerHeight - r.bottom - 8;
            const spaceAbove = r.top - 8;
            if (spaceBelow >= 140 || spaceBelow >= spaceAbove) {
                panel.style.top = (r.bottom + 3) + 'px';
                panel.style.bottom = 'auto';
            } else {
                panel.style.bottom = (window.innerHeight - r.top + 3) + 'px';
                panel.style.top = 'auto';
            }
            panel.style.left = r.left + 'px';
            panel.style.width = pw + 'px';
            panel.style.maxHeight = Math.min(300, Math.max(spaceBelow, spaceAbove) - 8) + 'px';
        } catch (e) {
            console.error("Positioning error:", e);
        }
    }

    document.addEventListener('click', function (e) {
        if (!e.target.closest('.wms-dd-wrap') && !e.target.closest('.wms-dd-panel') &&
            !e.target.closest('.wms-ms-wrap') && !e.target.closest('.wms-ms-panel')) {
            closeAll();
        }
    });

    window.addEventListener('scroll', debounce(function () {
        const op = document.querySelector('.wms-dd-panel.open, .wms-ms-panel.open');
        if (op && op._trigger && document.body.contains(op)) {
            posPanel(op, op._trigger);
        }
    }, 50), true);

    window.addEventListener('resize', debounce(function () {
        const op = document.querySelector('.wms-dd-panel.open, .wms-ms-panel.open');
        if (op && op._trigger && document.body.contains(op)) {
            posPanel(op, op._trigger);
        }
    }, 50), true);

    function buildSingle(sel) {
        if (sel._wmsBuilt) return;
        sel._wmsBuilt = true;

        let opts = getOpts(sel);
        let val = sel.value || (opts[0] && opts[0].value) || '';

        const wrap = document.createElement('div');
        wrap.className = 'wms-dd-wrap';
        if (sel.style.width) wrap.style.width = sel.style.width;

        const trigger = document.createElement('button');
        trigger.type = 'button';
        trigger.className = 'wms-dd-trigger';
        const valSpan = document.createElement('span');
        valSpan.className = 'wms-dd-val';
        const chev = document.createElement('span');
        chev.className = 'wms-dd-chevron';
        trigger.appendChild(valSpan);
        trigger.appendChild(chev);

        const panel = document.createElement('div');
        panel.className = 'wms-dd-panel';
        panel._trigger = trigger;

        const sr = document.createElement('div');
        sr.className = 'wms-dd-search-row';
        const sico = document.createElement('span');
        sico.className = 'wms-dd-search-ico';
        sico.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="#94A3B8" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round"><circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/></svg>';
        const si = document.createElement('input');
        si.type = 'text';
        si.className = 'wms-dd-search';
        si.placeholder = 'Search…';
        const sreset = document.createElement('button');
        sreset.type = 'button';
        sreset.className = 'wms-dd-sreset';
        sreset.innerHTML = '&#x2715;';
        sreset.title = 'Clear search';
        sr.appendChild(sico);
        sr.appendChild(si);
        sr.appendChild(sreset);
        panel.appendChild(sr);

        const ol = document.createElement('div');
        ol.className = 'wms-dd-opts';
        panel.appendChild(ol);

        function updateLabel() {
            trigger.innerHTML = '';
            const found = opts.find(o => o.value === val);
            if (!found || !val) {
                const ph = document.createElement('span');
                ph.className = 'wms-dd-ph';
                ph.textContent = opts[0] ? opts[0].text : 'Select…';
                trigger.appendChild(ph);
            } else {
                const span = document.createElement('span');
                span.className = 'wms-dd-val';
                span.textContent = found.text;
                trigger.appendChild(span);
            }
            trigger.appendChild(chev);
        }

        function renderOpts(q) {
            q = (q || '').toLowerCase();
            ol.innerHTML = '';
            let vis = 0;
            opts.forEach((o, i) => {
                if (i === 0 && o.value === '') return;
                if (q && o.text.toLowerCase().indexOf(q) === -1) return;
                vis++;
                const item = document.createElement('div');
                item.className = 'wms-dd-opt' + (o.value === val ? ' selected' : '');
                const txtEl = document.createElement('span');
                txtEl.className = 'wms-dd-opt-txt';
                txtEl.textContent = o.text;
                item.appendChild(txtEl);
                item.addEventListener('click', (e) => {
                    e.stopPropagation();
                    val = o.value;
                    sel.value = val;
                    updateLabel();
                    renderOpts(si.value);
                    fire(sel);
                    panel.classList.remove('open');
                    trigger.classList.remove('open');
                });
                ol.appendChild(item);
            });
            if (!vis) ol.innerHTML = '<div class="wms-dd-empty">No results</div>';
        }

        updateLabel();
        renderOpts('');

        si.addEventListener('input', function () {
            renderOpts(this.value);
            sr.classList.toggle('has-query', this.value.length > 0);
        });
        sreset.addEventListener('click', (e) => {
            e.stopPropagation();
            si.value = '';
            renderOpts('');
            sr.classList.remove('has-query');
            si.focus();
        });
        si.addEventListener('click', (e) => e.stopPropagation());

        si.addEventListener('keydown', (e) => {
            const items = ol.querySelectorAll('.wms-dd-opt');
            const foc = ol.querySelector('.wms-dd-opt.focused');
            let idx = foc ? Array.from(items).indexOf(foc) : -1;
            if (e.key === 'ArrowDown') {
                e.preventDefault();
                if (foc) foc.classList.remove('focused');
                idx = Math.min(idx + 1, items.length - 1);
                if (items[idx]) {
                    items[idx].classList.add('focused');
                    items[idx].scrollIntoView({ block: 'nearest' });
                }
            } else if (e.key === 'ArrowUp') {
                e.preventDefault();
                if (foc) foc.classList.remove('focused');
                idx = Math.max(idx - 1, 0);
                if (items[idx]) {
                    items[idx].classList.add('focused');
                    items[idx].scrollIntoView({ block: 'nearest' });
                }
            } else if (e.key === 'Enter') {
                if (foc) foc.click();
            } else if (e.key === 'Escape') {
                panel.classList.remove('open');
                trigger.classList.remove('open');
            }
        });

        trigger.addEventListener('click', (e) => {
            e.stopPropagation();
            const was = panel.classList.contains('open');
            closeAll();
            if (!was) {
                posPanel(panel, trigger);
                panel.classList.add('open');
                trigger.classList.add('open');
                setTimeout(() => si.focus(), 50);
            }
        });

        function syncFromSelect() {
            const newOpts = getOpts(sel);
            val = sel.value || (newOpts[0] && newOpts[0].value) || '';
            opts = newOpts;
            updateLabel();
            renderOpts(si.value);
        }

        const observer = new MutationObserver(syncFromSelect);
        observer.observe(sel, { childList: true, subtree: true });

        // Pages pre-fill edit forms via $(sel).val(x) or option.selected = true,
        // neither of which mutates the DOM tree, so the MutationObserver above
        // never fires for them. Poll the computed value instead of touching
        // every page's prefill code.
        const valuePoll = setInterval(() => {
            if (sel.value !== val) syncFromSelect();
        }, 250);

        sel._wmsSetValue = function (newVal) {
            val = (newVal != null) ? String(newVal) : '';
            sel.value = val;
            updateLabel();
            renderOpts('');
        };

        sel.classList.add('wms-replaced');
        sel.parentNode.insertBefore(wrap, sel);
        wrap.appendChild(sel);
        wrap.appendChild(trigger);
        panelHost(trigger).appendChild(panel);

        sel._wmsPanel = panel;

        sel._wmsDestroy = function () {
            clearInterval(valuePoll);
            if (sel._wmsPanel && sel._wmsPanel.parentNode) {
                sel._wmsPanel.parentNode.removeChild(sel._wmsPanel);
            }
            const wrp = sel.parentNode;
            if (wrp && wrp.classList && wrp.classList.contains('wms-dd-wrap')) {
                wrp.parentNode.insertBefore(sel, wrp);
                wrp.parentNode.removeChild(wrp);
            }
            sel.classList.remove('wms-replaced');
            sel._wmsBuilt    = false;
            sel._wmsSetValue = null;
            sel._wmsPanel    = null;
            sel._wmsDestroy  = null;
        };

        if (sel._wmsOnchange) sel.onchange = sel._wmsOnchange;
        if (sel._wmsOninput)  sel.oninput  = sel._wmsOninput;
    }

    function tryBuildSingle(sel) {
        if (sel._wmsBuilt || !sel.options) return;
        if (sel.onchange) sel._wmsOnchange = sel.onchange;
        if (sel.oninput)  sel._wmsOninput  = sel.oninput;
        buildSingle(sel);
    }

    function init() {
        document.querySelectorAll('select:not([multiple]):not(.wms-replaced):not(.wms-no-dd)').forEach(tryBuildSingle);
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => setTimeout(init, 100));
    } else {
        setTimeout(init, 100);
    }

    window.WmsSingleDropdown = buildSingle;

    const observer = new MutationObserver(mutations => {
        mutations.forEach(mutation => {
            mutation.addedNodes.forEach(node => {
                if (node.nodeType !== 1) return;
                if (node.matches && node.matches('select:not([multiple]):not(.wms-replaced):not(.wms-no-dd)')) {
                    tryBuildSingle(node);
                }
                if (!node.querySelectorAll) return;
                node.querySelectorAll('select:not([multiple]):not(.wms-replaced):not(.wms-no-dd)').forEach(tryBuildSingle);
            });
        });
    });
    observer.observe(document.body, { childList: true, subtree: true });
})();

// Multi-Select Searchable Dropdown
(function () {
    'use strict';

    function fire(sel) {
        sel.dispatchEvent(new Event('change', { bubbles: true }));
    }

    function getOpts(sel) {
        // NOTE: do not fall back to o.text when o.value is falsy. A <select>'s
        // native .value getter already returns the option's text content when
        // no value attribute is present (per spec), so the fallback is only
        // ever reached for options with an explicit value="" (a deliberate
        // blank/placeholder sentinel) -- and clobbering that to the option's
        // own label text breaks the placeholder-skip check in renderOpts(),
        // making the placeholder render as a normal, clickable-looking row
        // whose click silently fails to match any real <option> (no visible
        // change, perceived as the dropdown "not letting you select").
        return Array.from(sel.options).map(function (o) {
            return { value: o.value, text: o.text, selected: o.selected };
        });
    }

    // See the single-select engine's panelHost() above for why this matters:
    // appending the panel inside the modal (rather than always to <body>)
    // keeps it inside Bootstrap's FocusTrap containment check.
    function panelHost(trigger) {
        return trigger.closest('.modal') || document.body;
    }

    function buildMulti(sel) {
        if (sel._wmsBuilt) return;
        sel._wmsBuilt = true;

        let opts = getOpts(sel);
        let vals = Array.from(sel.selectedOptions).map(o => o.value);

        const wrap = document.createElement('div');
        wrap.className = 'wms-ms-wrap';
        if (sel.style.width) wrap.style.width = sel.style.width;

        const trigger = document.createElement('button');
        trigger.type = 'button';
        trigger.className = 'wms-ms-trigger';
        const chev = document.createElement('span');
        chev.className = 'wms-ms-chevron';
        trigger.appendChild(chev);

        const panel = document.createElement('div');
        panel.className = 'wms-ms-panel';
        panel._trigger = trigger;

        const sr = document.createElement('div');
        sr.className = 'wms-ms-search-row';
        const sico = document.createElement('span');
        sico.className = 'wms-ms-search-ico';
        sico.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="#94A3B8" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round"><circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/></svg>';
        const si = document.createElement('input');
        si.type = 'text';
        si.className = 'wms-ms-search';
        si.placeholder = 'Search…';
        const sreset = document.createElement('button');
        sreset.type = 'button';
        sreset.className = 'wms-ms-sreset';
        sreset.innerHTML = '&#x2715;';
        sreset.title = 'Clear search';
        sr.appendChild(sico);
        sr.appendChild(si);
        sr.appendChild(sreset);
        panel.appendChild(sr);

        const ol = document.createElement('div');
        ol.className = 'wms-ms-opts';
        panel.appendChild(ol);

        function updateLabel() {
            trigger.innerHTML = '';
            const selected = opts.filter(o => vals.includes(o.value));
            if (selected.length === 0) {
                const ph = document.createElement('span');
                ph.className = 'wms-ms-ph';
                ph.textContent = 'Select…';
                trigger.appendChild(ph);
            } else {
                selected.forEach(o => {
                    const tag = document.createElement('span');
                    tag.className = 'wms-ms-tag';
                    tag.innerHTML = o.text + ' <span class="wms-ms-tag-remove" data-value="' + o.value + '">×</span>';
                    trigger.appendChild(tag);
                });
            }
            trigger.appendChild(chev);
        }

        function renderOpts(q) {
            q = (q || '').toLowerCase();
            ol.innerHTML = '';
            let vis = 0;
            opts.forEach(o => {
                if (q && o.text.toLowerCase().indexOf(q) === -1) return;
                vis++;
                const item = document.createElement('div');
                item.className = 'wms-ms-opt' + (vals.includes(o.value) ? ' selected' : '');
                const txtEl = document.createElement('span');
                txtEl.className = 'wms-ms-opt-txt';
                txtEl.textContent = o.text;
                item.appendChild(txtEl);
                item.addEventListener('click', e => {
                    e.stopPropagation();
                    const idx = vals.indexOf(o.value);
                    if (idx === -1) {
                        vals.push(o.value);
                    } else {
                        vals.splice(idx, 1);
                    }
                    Array.from(sel.options).forEach(opt => {
                        opt.selected = vals.includes(opt.value);
                    });
                    updateLabel();
                    renderOpts(si.value);
                    fire(sel);
                });
                ol.appendChild(item);
            });
            if (!vis) ol.innerHTML = '<div class="wms-ms-empty">No results</div>';
        }

        updateLabel();
        renderOpts('');

        si.addEventListener('input', function () {
            renderOpts(this.value);
            sr.classList.toggle('has-query', this.value.length > 0);
        });
        sreset.addEventListener('click', e => {
            e.stopPropagation();
            si.value = '';
            renderOpts('');
            sr.classList.remove('has-query');
            si.focus();
        });
        si.addEventListener('click', e => e.stopPropagation());

        si.addEventListener('keydown', e => {
            const items = ol.querySelectorAll('.wms-ms-opt');
            const foc = ol.querySelector('.wms-ms-opt.focused');
            let idx = foc ? Array.from(items).indexOf(foc) : -1;
            if (e.key === 'ArrowDown') {
                e.preventDefault();
                if (foc) foc.classList.remove('focused');
                idx = Math.min(idx + 1, items.length - 1);
                if (items[idx]) {
                    items[idx].classList.add('focused');
                    items[idx].scrollIntoView({ block: 'nearest' });
                }
            } else if (e.key === 'ArrowUp') {
                e.preventDefault();
                if (foc) foc.classList.remove('focused');
                idx = Math.max(idx - 1, 0);
                if (items[idx]) {
                    items[idx].classList.add('focused');
                    items[idx].scrollIntoView({ block: 'nearest' });
                }
            } else if (e.key === 'Enter') {
                if (foc) foc.click();
            } else if (e.key === 'Escape') {
                panel.classList.remove('open');
                trigger.classList.remove('open');
            }
        });

        trigger.addEventListener('click', e => {
            e.stopPropagation();
            const was = panel.classList.contains('open');
            document.querySelectorAll('.wms-dd-panel.open, .wms-ms-panel.open').forEach(function (p) {
                p.classList.remove('open');
                if (p._trigger) p._trigger.classList.remove('open');
            });
            if (!was) {
                const r = trigger.getBoundingClientRect();
                panel.style.left = r.left + 'px';
                panel.style.top = (r.bottom + 3) + 'px';
                panel.style.width = Math.max(r.width, 220) + 'px';
                panel.classList.add('open');
                trigger.classList.add('open');
                setTimeout(() => si.focus(), 50);
            }
        });

        trigger.addEventListener('click', e => {
            if (e.target.classList.contains('wms-ms-tag-remove')) {
                e.stopPropagation();
                vals = vals.filter(v => v !== e.target.dataset.value);
                Array.from(sel.options).forEach(opt => {
                    opt.selected = vals.includes(opt.value);
                });
                updateLabel();
                renderOpts(si.value);
                fire(sel);
            }
        });

        function syncFromSelect() {
            opts = getOpts(sel);
            vals = Array.from(sel.selectedOptions).map(o => o.value);
            updateLabel();
            renderOpts(si.value);
        }

        const observer = new MutationObserver(syncFromSelect);
        observer.observe(sel, { childList: true, subtree: true, attributes: true });

        // Pages pre-fill via option.selected = true in a loop, which doesn't
        // mutate the DOM tree, so the observer above can miss it. Poll the
        // computed selection instead of touching every page's prefill code.
        setInterval(() => {
            const current = Array.from(sel.selectedOptions).map(o => o.value).join('');
            if (current !== vals.join('')) syncFromSelect();
        }, 250);

        sel.classList.add('wms-ms-replaced');
        sel.parentNode.insertBefore(wrap, sel);
        wrap.appendChild(sel);
        wrap.appendChild(trigger);
        panelHost(trigger).appendChild(panel);

        if (sel.dataset.onchange) {
            sel.onchange = new Function(sel.dataset.onchange);
        }
    }

    function tryBuildMulti(sel) {
        if (sel._wmsBuilt || !sel.options) return;
        if (sel.onchange) sel.dataset.onchange = sel.onchange.toString().replace(/function\s*\([^)]*\)\s*\{|\}/g, '');
        buildMulti(sel);
    }

    function init() {
        document.querySelectorAll('select[multiple]:not(.wms-ms-replaced):not(.wms-no-dd)').forEach(tryBuildMulti);
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => setTimeout(init, 100));
    } else {
        setTimeout(init, 100);
    }

    window.WmsMultiDropdown = buildMulti;

    const observer = new MutationObserver(mutations => {
        mutations.forEach(mutation => {
            mutation.addedNodes.forEach(node => {
                if (node.nodeType !== 1) return;
                if (node.matches && node.matches('select[multiple]:not(.wms-ms-replaced):not(.wms-no-dd)')) {
                    tryBuildMulti(node);
                }
                if (!node.querySelectorAll) return;
                node.querySelectorAll('select[multiple]:not(.wms-ms-replaced):not(.wms-no-dd)').forEach(tryBuildMulti);
            });
        });
    });
    observer.observe(document.body, { childList: true, subtree: true });
})();
