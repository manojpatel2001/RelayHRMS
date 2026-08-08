// Escalation is now a tab inside the single "Welcome Back" notification
// modal (compoffleavemodal.js owns that modal's lifecycle/tab-switching),
// not a separate standalone popup. Its own document-ready trigger — gated
// by `localStorage.getItem("EmployeeId")=='14'` — was leftover test/debug
// code hardcoded to one specific employee, which is what caused this to
// only ever appear as an extra popup for that one account. The fetch is now
// called from compoffleavemodal.js's own sequence alongside the other tabs,
// driven by real data (hasEscalationData) instead of a hardcoded ID.

var companyDetailsESc = JSON.parse(localStorage.getItem('selectedCompany'));
var CompanyIdEsc = companyDetailsESc.CompanyId;

var escalationGrid = null;
var escalationRows = [];

$("#btnExportEscalation").click(function () {
    downloadPendingProbationGridExcel();
})

async function fetchUpcomingEscalaionProbation() {
    try {
        $('#escalationLoader').show();

        return $.ajax({
            type: "GET",
            url: BaseUrlLayout + '/ApprovalMasterAPI/GetEscalationDueList/' + CompanyIdEsc,
            contentType: 'application/json',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem("authToken")
            },
            success: function (data) {
                if (data.isSuccess && data.data && data.data.length > 0) {
                    hasEscalationData = true;
                    escalationRows = data.data;
                    $('#escalationTotal').text(escalationRows.length);
                    renderEscalationGrid(escalationRows);
                } else {
                    hasEscalationData = false;
                    $('#upEscalationContainer').empty();
                    $('#upEscalationContainer').append('<div class="text-center py-3">No escalation records found</div>');
                }

                escalationLoaded = true;
                hideLoadingIndicators('escalation');
                checkAllDataLoaded();
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error:", status, error);
                hasEscalationData = false;
                escalationLoaded = true;
                hideLoadingIndicators('escalation');
                $('#upEscalationContainer').empty();
                $('#upEscalationContainer').append('<div class="text-center py-3 text-danger">Error loading escalation records</div>');
                checkAllDataLoaded();
            }
        });
    } catch (e) {
        console.error("Error in escalation:", e);
        hasEscalationData = false;
        escalationLoaded = true;
        hideLoadingIndicators('escalation');
        $('#upEscalationContainer').empty();
        $('#upEscalationContainer').append('<div class="text-center py-3 text-danger">Error loading escalation records</div>');
        checkAllDataLoaded();
    }
}

function escFmtDate(v) {
    if (!v) return '-';
    var d = new Date(v);
    if (isNaN(d.getTime())) return '-';
    return String(d.getDate()).padStart(2, '0') + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + d.getFullYear();
}

function renderEscalationGrid(rows) {
    if (!escalationGrid) {
        escalationGrid = WMSGrid.create({
            container: 'upEscalationContainer',
            title: 'Pending Probation',
            icon: 'bx-time-five',
            perPage: 10,
            height: '360px',
            data: rows,
            features: { export: false },
            columns: [
                { field: 'EmployeeName', header: 'Employee Name', width: '200px' },
                { field: 'ApproverName', header: 'Approver Name', width: '200px' },
                { field: 'StatusName', header: 'Status', width: '120px' },
                { field: 'LevelNo', header: 'Level No', width: '90px', align: 'center' },
                { field: 'RemainingDaysString', header: 'Days Remaining', width: '140px', align: 'center' },
                { field: 'EscalationDueOn', header: 'Escalation Date', width: '150px', align: 'center', render: function (v) { return escFmtDate(v); } },
                { field: 'ProbationEndDate', header: 'Probation End Date', width: '160px', align: 'center', render: function (v) { return escFmtDate(v); } }
            ]
        });
    } else {
        escalationGrid.setData(rows);
        escalationGrid.hideLoader();
    }
}


function downloadPendingProbationGridExcel() {

    if (!escalationRows.length) {
        showToast("No data available to export", "error");
        return;
    }

    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet("Pending Probation Data");

    worksheet.columns = [
        { header: 'Employee Name', key: 'EmployeeName', width: 26 },
        { header: 'Approver Name', key: 'ApproverName', width: 26 },
        { header: 'Status', key: 'StatusName', width: 16 },
        { header: 'Level No', key: 'LevelNo', width: 12 },
        { header: 'Days Remaining', key: 'RemainingDaysString', width: 16 },
        { header: 'Escalation Date', key: 'EscalationDueOn', width: 18 },
        { header: 'Probation End Date', key: 'ProbationEndDate', width: 18 }
    ];

    escalationRows.forEach(function (row) {
        worksheet.addRow({
            EmployeeName: row.EmployeeName,
            ApproverName: row.ApproverName,
            StatusName: row.StatusName,
            LevelNo: row.LevelNo,
            RemainingDaysString: row.RemainingDaysString,
            EscalationDueOn: escFmtDate(row.EscalationDueOn),
            ProbationEndDate: escFmtDate(row.ProbationEndDate)
        });
    });

    workbook.xlsx.writeBuffer()
        .then((buffer) => {
            saveAs(
                new Blob([buffer], { type: "application/octet-stream" }),
                `PendingProbation.xlsx`
            );
        })
        .catch((error) => {
            console.error("Excel export failed:", error);
            showToast("Failed to export Excel", "error");
        });
}
