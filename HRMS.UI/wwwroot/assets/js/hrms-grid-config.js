/* ════════════════════════════════════════════════════════════════════════
   HRMS Grid Config  —  Global DevExtreme DataGrid defaults  v1.0

   Loaded after dx.all.js and before @RenderSection("Scripts"), so all
   grids on every page inherit these settings.  Individual pages can still
   override any option in their own dxDataGrid({...}) call.
   ════════════════════════════════════════════════════════════════════════ */
(function (w) {
    'use strict';

    if (!w.DevExpress || !DevExpress.ui || !DevExpress.ui.dxDataGrid) return;

    DevExpress.ui.dxDataGrid.defaultOptions({
        options: {
            /* ── Layout ── */
            rowAlternationEnabled: true,
            showBorders:           true,
            wordWrapEnabled:       false,
            allowColumnResizing:   true,
            columnsAutoWidth:      false,   /* correct DX option name — "columns" not "column" */
            columnFixing:          { enabled: true },

            /* ── Paging ── */
            paging: { pageSize: 10 },
            pager: {
                showPageSizeSelector:  true,
                allowedPageSizes:      [10, 25, 50, 100],
                showInfo:              true,
                infoText:              'Showing {0}–{1} of {2} Records',
                showNavigationButtons: true
            },

            /* ── Filter / search ── */
            headerFilter: { visible: true },
            filterRow:    { visible: true, applyFilter: 'auto' },
            groupPanel:   { visible: true },
            searchPanel:  { visible: true, width: 220, placeholder: 'Search…' },

            /* ── Scrolling — native thumb scrollbar via CSS ── */
            scrolling: {
                mode:            'standard',
                useNative:       false,
                scrollByContent: true,
                scrollByThumb:   true
            }
        }
    });

})(window);
