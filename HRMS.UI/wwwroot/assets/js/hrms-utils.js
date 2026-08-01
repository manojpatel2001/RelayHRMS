/* ════════════════════════════════════════════════════════════════════════
   HRMS Utils  —  Global UI Utilities  v1.0

   Loaded in all 3 layouts after hrms-grid-config.js / before @RenderSection.
   Provides:
   • Auto-upgrade of old card-header + inline-button patterns on DOM ready
   • Bootstrap 5 jQuery $.fn.modal bridge (fixes pages that load jQuery after Bootstrap)
   • Global setBtnLoading / setBtnNormal helpers
   • HRMSGrid.create()  — standard grid factory
   • HRMSUtils.setRowCount() — row-count span helper
   ════════════════════════════════════════════════════════════════════════ */
(function (w) {
    'use strict';

    /* ── Bootstrap 5 jQuery $.fn.modal bridge ─────────────────────────────
       Bootstrap 5 wires $.fn.modal at load time.  Pages that reload jQuery
       after Bootstrap lose the plugin.  We re-attach it so that the old
       pattern  $("#myModal").modal('show')  always works.                  */
    function _wireModalBridge() {
        if (!w.$ || !w.bootstrap) return;
        if ($.fn.modal) return;                   // already wired by Bootstrap
        $.fn.modal = function (config, relatedTarget) {
            return this.each(function () {
                var el   = this;
                var inst = bootstrap.Modal.getInstance(el);
                if (typeof config === 'string') {
                    if (!inst) inst = new bootstrap.Modal(el);
                    if (typeof inst[config] === 'function') inst[config](relatedTarget);
                } else {
                    new bootstrap.Modal(el, config || {}).show();
                }
            });
        };
    }

    /* ── Card-header auto-upgrade ─────────────────────────────────────────
       Old pages:  class="card-header bg-transparent ml-0 py-0"
       Bootstrap's .bg-transparent carries  background-color:transparent !important
       which beats our card-header background:#fff rule even though we use !important
       (Bootstrap inline wins when the utility is more specific).
       Removing those classes in JS lets hrms-premium.css take full effect.  */
    function _upgradeCardHeaders() {
        document.querySelectorAll('.card-header').forEach(function (h) {
            h.classList.remove('bg-transparent', 'py-0', 'ml-0', 'px-0', 'pl-0', 'pr-0');

            /* Mark titles so CSS can target .hrms-card-title typography */
            var t = h.querySelector('h1, h2, h3, h4, h5, h6');
            if (t && !t.classList.contains('hrms-card-title')) {
                t.classList.add('hrms-card-title');
                t.classList.remove('pt-2');          /* remove old top-pad */
            }
        });
    }

    /* ── Add-button auto-upgrade ──────────────────────────────────────────
       Old pages:  class="btn mr-1 rounded-1" style="background-color:#2395c6;color:white;"
       Inline style has specificity 1000 and beats class !important.
       We strip the inline style and ensure the button has btn-primary btn-sm. */
    function _upgradeHeaderButtons() {
        document.querySelectorAll('.card-header .btn').forEach(function (btn) {
            var s = (btn.getAttribute('style') || '');
            /* Only touch buttons with an inline background — leave others alone */
            if (s.includes('background-color') || s.includes('background:')) {
                btn.removeAttribute('style');
            }
            /* Add btn-primary only if no colour variant is present */
            var hasColor = ['btn-primary','btn-secondary','btn-danger',
                            'btn-success','btn-warning','btn-info','btn-light',
                            'btn-dark','btn-outline-primary','btn-outline-secondary']
                           .some(function (c) { return btn.classList.contains(c); });
            if (!hasColor) btn.classList.add('btn-primary');

            /* Ensure btn-sm unless a size is already set */
            if (!btn.classList.contains('btn-sm') && !btn.classList.contains('btn-lg') &&
                !btn.classList.contains('btn-xs')) {
                btn.classList.add('btn-sm');
            }

            /* Remove leftover Bootstrap 4 spacing utilities if needed */
            btn.classList.remove('mr-1', 'ml-1', 'rounded-1');
        });
    }

    /* ── Also upgrade modal footer buttons with inline styles ─────────── */
    function _upgradeModalButtons() {
        document.querySelectorAll('.modal-footer .btn, .btn-heading-title .btn').forEach(function (btn) {
            var s = (btn.getAttribute('style') || '');
            if (s.includes('background-color') || s.includes('background:')) {
                btn.removeAttribute('style');
                var hasColor = ['btn-primary','btn-secondary','btn-danger',
                                'btn-success','btn-warning','btn-info']
                               .some(function (c) { return btn.classList.contains(c); });
                if (!hasColor) btn.classList.add('btn-primary');
            }
        });
    }

    /* ── Button loading helpers ───────────────────────────────────────────
       Usage:
         setBtnLoading('#btnSave', 'Saving...');
         setBtnNormal('#btnSave', 'Save');
       Also accepts HTMLElement or jQuery object.                            */
    function setBtnLoading(btnRef, loadingText) {
        var btn = _resolve(btnRef);
        if (!btn) return;
        btn.disabled = true;
        var spinner = btn.querySelector('.btn-spinner-icon');
        var label   = btn.querySelector('.btn-label-text');
        if (spinner) spinner.style.display = 'inline-block';
        if (label && loadingText) label.textContent = loadingText;
        if (!spinner) {
            if (!btn.dataset.origText) btn.dataset.origText = btn.textContent.trim();
            if (loadingText) btn.textContent = loadingText;
        }
    }

    function setBtnNormal(btnRef, normalText) {
        var btn = _resolve(btnRef);
        if (!btn) return;
        btn.disabled = false;
        var spinner = btn.querySelector('.btn-spinner-icon');
        var label   = btn.querySelector('.btn-label-text');
        if (spinner) spinner.style.display = 'none';
        if (label && normalText) label.textContent = normalText;
        if (!spinner) {
            btn.textContent = normalText || btn.dataset.origText || '';
            delete btn.dataset.origText;
        }
    }

    function _resolve(ref) {
        if (!ref) return null;
        if (typeof ref === 'string') {
            /* Support '#id', 'id', or CSS selector */
            return document.querySelector(ref) || document.getElementById(ref.replace(/^#/, ''));
        }
        if (ref instanceof HTMLElement) return ref;
        if (w.$ && ref instanceof $) return ref[0] || null;
        return null;
    }

    /* ── Standard grid factory ────────────────────────────────────────────
       Usage:
         HRMSGrid.create('gridContainer', data, [
           { dataField:'name', caption:'Name' },
           {
             type:'buttons', caption:'Actions', width:80,
             fixed:true, fixedPosition:'right',
             buttons:[
               { hint:'Edit', icon:'edit', cssClass:'hr-grid-btn hr-grid-btn-edit',
                 onClick: function(e){ editRow(e.row.data); } },
               { hint:'Delete', icon:'trash', cssClass:'hr-grid-btn hr-grid-btn-delete',
                 onClick: function(e){ deleteRow(e.row.data); } }
             ]
           }
         ]);                                                                 */
    function _gridCreate(containerId, dataSource, columns, extraOptions) {
        if (!w.$) return null;
        var $c = $('#' + containerId);
        if (!$c.length) { console.warn('HRMSGrid.create: #' + containerId + ' not found'); return null; }
        /* Safely dispose any existing instance */
        if ($.data($c[0], 'dxDataGrid')) {
            try { $c.dxDataGrid('dispose'); } catch (ignore) {}
        }
        var opts = $.extend(true, { dataSource: dataSource, columns: columns }, extraOptions || {});
        return $c.dxDataGrid(opts).dxDataGrid('instance');
    }

    /* ── Row-count helper ─────────────────────────────────────────────────
       Usage:  HRMSUtils.setRowCount('rowCountRole', 42);                   */
    function _setRowCount(idOrEl, count) {
        var el = typeof idOrEl === 'string' ? document.getElementById(idOrEl) : idOrEl;
        if (!el) return;
        el.textContent = (count === 0)
            ? 'No records found'
            : count + ' Record' + (count === 1 ? '' : 's');
    }

    /* ── DOM-ready init ───────────────────────────────────────────────── */
    function _init() {
        _wireModalBridge();
        _upgradeCardHeaders();
        _upgradeHeaderButtons();
        _upgradeModalButtons();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', _init);
    } else {
        _init();
    }

    /* ── Public API ───────────────────────────────────────────────────── */
    w.setBtnLoading = setBtnLoading;
    w.setBtnNormal  = setBtnNormal;
    w.HRMSGrid      = { create: _gridCreate };
    w.HRMSUtils     = { setRowCount: _setRowCount };

})(window);

/* ════════════════════════════════════════════════════════════════════════
   WMS-style Toast  (showToast) + Confirm Dialog (wmsConfirm)
   Copied from WMS project — used by all migrated WMSGrid pages.
   ════════════════════════════════════════════════════════════════════════ */
function showToast(msg, type, duration) {
    document.querySelectorAll('.wms-toast').forEach(function (t) { t.remove(); });
    var colorMap   = { success:'#166534',green:'#166534', warn:'#92400E',warning:'#92400E',amber:'#92400E', error:'#991B1B',red:'#991B1B', info:'#1565C0',blue:'#1565C0' };
    var bgMap      = { success:'#DCFCE7',green:'#DCFCE7', warn:'#FEF3C7',warning:'#FEF3C7',amber:'#FEF3C7', error:'#FEE2E2',red:'#FEE2E2', info:'#DBEAFE',blue:'#DBEAFE' };
    var borderMap  = { success:'#86EFAC',green:'#86EFAC', warn:'#FCD34D',warning:'#FCD34D',amber:'#FCD34D', error:'#FCA5A5',red:'#FCA5A5', info:'#93C5FD',blue:'#93C5FD' };
    var iconMap    = { success:'<i class="bx bx-check-circle"></i>', green:'<i class="bx bx-check-circle"></i>', warn:'<i class="bx bx-error"></i>', warning:'<i class="bx bx-error"></i>', amber:'<i class="bx bx-error"></i>', error:'<i class="bx bx-x-circle"></i>', red:'<i class="bx bx-x-circle"></i>', info:'<i class="bx bx-info-circle"></i>', blue:'<i class="bx bx-info-circle"></i>' };
    var t = type || 'info';
    var color = colorMap[t] || colorMap.info; var bg = bgMap[t] || bgMap.info; var border = borderMap[t] || borderMap.info;
    if (!document.getElementById('wms-toast-style')) {
        var s = document.createElement('style'); s.id = 'wms-toast-style';
        s.textContent = '@keyframes wmsSlideIn{from{transform:translateX(110%);opacity:0}to{transform:none;opacity:1}}@keyframes wmsSlideOut{from{transform:none;opacity:1}to{transform:translateX(110%);opacity:0}}.wms-toast{position:fixed;top:64px;right:24px;z-index:99999;display:flex;align-items:center;gap:10px;padding:11px 14px;border-radius:10px;font-size:13px;font-weight:600;font-family:inherit;box-shadow:0 8px 32px rgba(0,0,0,.18);animation:wmsSlideIn .3s cubic-bezier(.22,1,.36,1);min-width:260px;max-width:420px;border-left:4px solid;}.wms-toast-icon{font-size:18px;flex-shrink:0;line-height:1;display:flex;align-items:center;}.wms-toast-msg{flex:1;line-height:1.4}.wms-toast-close{flex-shrink:0;background:none;border:none;cursor:pointer;font-size:16px;line-height:1;display:flex;align-items:center;opacity:0.6;padding:0;transition:opacity .15s;}.wms-toast-close:hover{opacity:1;}';
        document.head.appendChild(s);
    }
    var cleanMsg = String(msg || '').replace(/[\u{1F300}-\u{1FAFF}\u{2600}-\u{26FF}\u{2700}-\u{27BF}✅⚠️❌ℹ️]/gu, '').replace(/\s{2,}/g, ' ').trim();
    var toast = document.createElement('div');
    toast.className = 'wms-toast';
    toast.style.cssText = 'background:' + bg + ';color:' + color + ';border-color:' + border + ';';
    toast.innerHTML = '<span class="wms-toast-icon" style="color:' + color + '">' + (iconMap[t] || iconMap.info) + '</span><span class="wms-toast-msg">' + cleanMsg + '</span><button class="wms-toast-close" style="color:' + color + '" onclick="this.closest(\'.wms-toast\').remove()"><i class=\'bx bx-x\'></i></button>';
    document.body.appendChild(toast);
    setTimeout(function () {
        if (toast.parentNode) { toast.style.animation = 'wmsSlideOut .3s ease forwards'; setTimeout(function () { if (toast.parentNode) toast.remove(); }, 280); }
    }, duration || 3000);
}

/* ════════════════════════════════════════════════════════════════════════
   WMS-style Field Validation Helpers
   Works with two form patterns:
     1. WMS pattern:  <div class="fg [fg-req]"><label>...</label><input></div>
     2. HRMS legacy: <input id="txtX"> + <span id="spnX" style="display:none">
   ════════════════════════════════════════════════════════════════════════ */

function fieldError(fieldIdOrEl, msg) {
    var el = typeof fieldIdOrEl === 'string' ? document.getElementById(fieldIdOrEl) : fieldIdOrEl;
    if (!el) return;
    el.classList.add('is-invalid');
    var fg = el.closest ? el.closest('.fg') : null;
    if (!fg) { var p = el.parentNode; while (p && !p.classList.contains('fg')) p = p.parentNode; fg = p; }
    if (fg) {
        var existing = fg.querySelector('.field-error-msg');
        if (existing) existing.remove();
        var errEl = document.createElement('div');
        errEl.className = 'field-error-msg';
        errEl.textContent = msg;
        fg.appendChild(errEl);
    }
}

function fieldClear(fieldIdOrEl) {
    var el = typeof fieldIdOrEl === 'string' ? document.getElementById(fieldIdOrEl) : fieldIdOrEl;
    if (!el) return;
    el.classList.remove('is-invalid');
    var fg = el.closest ? el.closest('.fg') : null;
    if (!fg) { var p = el.parentNode; while (p && !p.classList.contains('fg')) p = p.parentNode; fg = p; }
    if (fg) { var e = fg.querySelector('.field-error-msg'); if (e) e.remove(); }
}

function fieldClearAll(containerIdOrEl) {
    var c = typeof containerIdOrEl === 'string' ? document.getElementById(containerIdOrEl) : containerIdOrEl;
    if (!c) c = document.body;
    c.querySelectorAll('.is-invalid').forEach(function (el) {
        el.classList.remove('is-invalid');
        var fg = el.closest ? el.closest('.fg') : null;
        if (fg) { var e = fg.querySelector('.field-error-msg'); if (e) e.remove(); }
    });
}

function fieldValidateAll(checks) {
    var hasError = false;
    var firstEl = null;
    checks.forEach(function (c) {
        var el = typeof c.id === 'string' ? document.getElementById(c.id) : c.id;
        if (!el) return;
        var isErr = (c.condition !== undefined) ? c.condition : (!c.val && c.val !== 0);
        if (isErr) {
            fieldError(el, c.msg || 'This field is required.');
            if (!firstEl) firstEl = el;
            hasError = true;
        } else {
            fieldClear(el);
        }
    });
    if (firstEl) firstEl.focus();
    return !hasError;
}

/* ── CSS for fieldValidateAll (injected once) ──────────────────────────── */
(function () {
    if (document.getElementById('hrms-fv-style')) return;
    var s = document.createElement('style');
    s.id = 'hrms-fv-style';
    s.textContent = [
        '.is-invalid{border-color:#C62828!important;box-shadow:0 0 0 3px rgba(198,40,40,.12)!important;background:#FFF8F8!important;}',
        '.field-error-msg{display:flex;align-items:center;gap:4px;font-size:11.5px;font-weight:600;color:#C62828;margin-top:3px;}',
        '.field-error-msg::before{content:"⚠ ";}',
        '@keyframes fvIn{from{opacity:0;transform:translateY(-3px)}to{opacity:1;transform:none}}'
    ].join('');
    document.head.appendChild(s);
})();

/* ════════════════════════════════════════════════════════════════════════
   WMSConfirm  —  Promise-based confirm dialog (delete / warn / info)
   Usage:
     WMSConfirm.delete('Role "Admin"', async () => { await doDelete(); });
     WMSConfirm.warn({ title:'...', message:'...', okText:'...' }).then(ok => ...);
   ════════════════════════════════════════════════════════════════════════ */
var WMSConfirm = (function () {
    var _active = null;

    var _types = {
        danger:  { bar: '#C62828', icon: 'bx bx-trash',         okCls: 'wms-confirm-btn-danger',   okDefault: 'Yes, Delete' },
        warning: { bar: '#E65100', icon: 'bx bx-error',          okCls: 'wms-confirm-btn-warning',  okDefault: 'Yes, Proceed' },
        info:    { bar: '#1565C0', icon: 'bx bx-info-circle',    okCls: 'wms-confirm-btn-info',     okDefault: 'OK' },
        success: { bar: '#2E7D32', icon: 'bx bx-check-circle',   okCls: 'wms-confirm-btn-success',  okDefault: 'OK' }
    };

    function _injectCSS() {
        if (document.getElementById('wms-confirm-style')) return;
        var s = document.createElement('style');
        s.id = 'wms-confirm-style';
        s.textContent = [
            '@keyframes wmsConfirmIn{from{opacity:0}to{opacity:1}}',
            '@keyframes wmsConfirmPop{from{transform:scale(.92) translateY(10px);opacity:0}to{transform:none;opacity:1}}',
            '@keyframes wmsConfirmShake{0%,100%{transform:none}20%,60%{transform:translateX(-6px)}40%,80%{transform:translateX(6px)}}',
            '.wms-confirm-overlay{position:fixed;inset:0;z-index:99990;background:rgba(0,0,0,.48);display:flex;align-items:center;justify-content:center;animation:wmsConfirmIn .18s ease;}',
            '.wms-confirm-box{background:#fff;border-radius:14px;max-width:420px;width:90%;box-shadow:0 24px 64px rgba(0,0,0,.22);animation:wmsConfirmPop .22s cubic-bezier(.22,1,.36,1);overflow:hidden;position:relative;}',
            '.wms-confirm-bar{height:5px;}',
            '.wms-confirm-close{position:absolute;top:10px;right:12px;background:none;border:none;font-size:20px;color:#94A3B8;cursor:pointer;line-height:1;padding:2px 6px;border-radius:5px;transition:color .12s,background .12s;}',
            '.wms-confirm-close:hover{color:#1e3a5f;background:#F1F5F9;}',
            '.wms-confirm-body{display:flex;align-items:flex-start;gap:14px;padding:22px 24px 16px;}',
            '.wms-confirm-icon{width:42px;height:42px;border-radius:50%;display:flex;align-items:center;justify-content:center;flex-shrink:0;font-size:22px;}',
            '.wms-confirm-icon-danger{background:#FFEBEE;color:#C62828;}',
            '.wms-confirm-icon-warning{background:#FFF3E0;color:#E65100;}',
            '.wms-confirm-icon-info{background:#E3F2FD;color:#1565C0;}',
            '.wms-confirm-icon-success{background:#E8F5E9;color:#2E7D32;}',
            '.wms-confirm-title{font-size:15px;font-weight:700;color:#0F172A;margin-bottom:6px;}',
            '.wms-confirm-msg{font-size:13px;color:#475569;line-height:1.55;}',
            '.wms-confirm-divider{border:none;border-top:1px solid #F1F5F9;margin:0;}',
            '.wms-confirm-footer{display:flex;justify-content:flex-end;gap:8px;padding:14px 20px;}',
            '.wms-confirm-btn{display:inline-flex;align-items:center;gap:6px;padding:8px 18px;border-radius:8px;font-size:13px;font-weight:600;cursor:pointer;border:1.5px solid transparent;font-family:inherit;transition:background .15s;}',
            '.wms-confirm-btn-cancel{background:#F8FAFC;color:#475569;border-color:#CBD5E1;}.wms-confirm-btn-cancel:hover{background:#F1F5F9;}',
            '.wms-confirm-btn-danger{background:#C62828;color:#fff;border-color:#B71C1C;}.wms-confirm-btn-danger:hover{background:#B71C1C;}',
            '.wms-confirm-btn-warning{background:#E65100;color:#fff;border-color:#BF360C;}.wms-confirm-btn-warning:hover{background:#BF360C;}',
            '.wms-confirm-btn-info{background:#1565C0;color:#fff;border-color:#0D47A1;}.wms-confirm-btn-info:hover{background:#0D47A1;}',
            '.wms-confirm-btn-success{background:#2E7D32;color:#fff;border-color:#1B5E20;}.wms-confirm-btn-success:hover{background:#1B5E20;}',
            '.wms-confirm-btn.loading{opacity:.7;pointer-events:none;}'
        ].join('');
        document.head.appendChild(s);
    }

    function _show(opts) {
        _injectCSS();
        opts = opts || {};
        var type    = opts.type || 'danger';
        var cfg     = _types[type] || _types.danger;
        var title   = opts.title || 'Are you sure?';
        var msg     = opts.message || '';
        var okText  = opts.okText || cfg.okDefault;
        var okIcon  = opts.okIcon || cfg.icon;
        var canText = opts.cancelText !== undefined ? opts.cancelText : 'Cancel';

        if (_active) { _active.remove(); _active = null; }

        return new Promise(function (resolve) {
            var overlay = document.createElement('div');
            overlay.className = 'wms-confirm-overlay';
            overlay.innerHTML = [
                '<div class="wms-confirm-box" id="_wcb">',
                '  <div class="wms-confirm-bar" style="background:' + cfg.bar + '"></div>',
                '  <button class="wms-confirm-close" id="_wcc">&#x2715;</button>',
                '  <div class="wms-confirm-body">',
                '    <div class="wms-confirm-icon wms-confirm-icon-' + type + '"><i class="' + cfg.icon + '"></i></div>',
                '    <div><div class="wms-confirm-title">' + title + '</div><div class="wms-confirm-msg">' + msg + '</div></div>',
                '  </div>',
                '  <hr class="wms-confirm-divider">',
                '  <div class="wms-confirm-footer">',
                canText ? '    <button class="wms-confirm-btn wms-confirm-btn-cancel" id="_wcn"><i class="bx bx-x"></i>' + canText + '</button>' : '',
                '    <button class="wms-confirm-btn ' + cfg.okCls + '" id="_wco"><i class="' + okIcon + '"></i>' + okText + '</button>',
                '  </div>',
                '</div>'
            ].join('');
            document.body.appendChild(overlay);
            _active = overlay;

            var box    = overlay.querySelector('#_wcb');
            var okBtn  = overlay.querySelector('#_wco');
            var canBtn = overlay.querySelector('#_wcn');
            var closeX = overlay.querySelector('#_wcc');

            async function doConfirm() {
                if (okBtn.classList.contains('loading')) return;
                if (typeof opts.onConfirm === 'function') {
                    okBtn.classList.add('loading');
                    okBtn.innerHTML = '<i class="bx bx-loader-alt bx-spin"></i> Working…';
                    try { await Promise.resolve(opts.onConfirm()); } catch (e) {}
                    okBtn.classList.remove('loading');
                }
                overlay.remove(); _active = null;
                resolve(true);
            }

            function doCancel() {
                if (type === 'danger' && box) {
                    box.style.animation = 'wmsConfirmShake .35s ease';
                    setTimeout(function () { box.style.animation = ''; }, 350);
                }
                overlay.remove(); _active = null;
                resolve(false);
            }

            okBtn.addEventListener('click', doConfirm);
            if (canBtn)  canBtn.addEventListener('click', doCancel);
            if (closeX) closeX.addEventListener('click', doCancel);
            overlay.addEventListener('click', function (e) { if (e.target === overlay) doCancel(); });
            document.addEventListener('keydown', function _kh(e) {
                if (e.key === 'Enter')  { document.removeEventListener('keydown', _kh); doConfirm(); }
                if (e.key === 'Escape') { document.removeEventListener('keydown', _kh); doCancel(); }
            });
        });
    }

    return {
        show:    _show,
        delete:  function (label, onConfirm) {
            return _show({
                type:      'danger',
                title:     'Delete Confirmation',
                message:   'Are you sure you want to delete <strong>' + (label || 'this record') + '</strong>?<br><span style="font-size:12px;color:#EF5350">This action cannot be undone.</span>',
                okText:    'Yes, Delete',
                okIcon:    'bx bx-trash',
                onConfirm: onConfirm
            });
        },
        warn:    function (opts) { return _show(Object.assign({ type: 'warning' }, typeof opts === 'string' ? { message: opts } : opts)); },
        info:    function (opts) { return _show(Object.assign({ type: 'info' }, typeof opts === 'string' ? { message: opts } : opts)); },
        success: function (opts) { return _show(Object.assign({ type: 'success' }, typeof opts === 'string' ? { message: opts } : opts)); }
    };
})();

function wmsConfirm(msg, onYes, options) {
    options = options || {};
    var title = options.title || 'Confirm Delete'; var confirmText = options.confirmText || 'Delete'; var cancelText = options.cancelText || 'Cancel';
    var headerColor = options.headerColor || '#C62828'; var confirmColor = options.confirmColor || '#C62828'; var confirmIcon = options.confirmIcon || 'bx-trash';
    var overlay = document.createElement('div');
    overlay.style.cssText = 'position:fixed;inset:0;z-index:99998;background:rgba(0,0,0,0.45);display:flex;align-items:center;justify-content:center;animation:wmsDialogIn .18s ease;';
    if (!document.getElementById('wms-dialog-style')) {
        var ds = document.createElement('style'); ds.id = 'wms-dialog-style';
        ds.textContent = '@keyframes wmsDialogIn{from{opacity:0}to{opacity:1}}@keyframes wmsDialogPop{from{transform:scale(.93);opacity:0}to{transform:none;opacity:1}}';
        document.head.appendChild(ds);
    }
    overlay.innerHTML = '<div style="background:#fff;border-radius:14px;padding:0;max-width:420px;width:90%;box-shadow:0 20px 60px rgba(0,0,0,.22);animation:wmsDialogPop .2s cubic-bezier(.22,1,.36,1);overflow:hidden"><div style="background:' + headerColor + ';padding:16px 24px;border-radius:14px 14px 0 0"><div style="font-size:15px;font-weight:700;color:#fff;letter-spacing:0.8px;text-transform:uppercase">' + title + '</div></div><div style="padding:22px 24px 20px"><div style="font-size:13px;color:#444;line-height:1.6;margin-bottom:22px">' + msg + '</div><div style="display:flex;gap:10px;justify-content:flex-end"><button id="wms-conf-no" style="padding:8px 20px;border:1.5px solid #D0D8DC;border-radius:7px;background:#fff;font-size:13px;font-weight:600;cursor:pointer;color:#555"><i class="bx bx-x"></i> ' + cancelText + '</button><button id="wms-conf-yes" style="padding:8px 22px;border:none;border-radius:7px;background:' + confirmColor + ';font-size:13px;font-weight:700;cursor:pointer;color:#fff"><i class="bx ' + confirmIcon + '"></i> ' + confirmText + '</button></div></div></div>';
    document.body.appendChild(overlay);
    overlay.querySelector('#wms-conf-no').onclick = function () { overlay.remove(); };
    overlay.querySelector('#wms-conf-yes').onclick = function () { overlay.remove(); onYes(); };
    overlay.addEventListener('click', function (e) { if (e.target === overlay) overlay.remove(); });
}
