
$(document).ready(async function () {
    var notiEsc = localStorage.getItem('EscalationSummary');
    if (notiEsc == 'true' && localStorage.getItem("EmployeeId")=='14' ) {
        await fetchUpcomingEscalaionProbation();

    }
});
var companyDetailsESc = JSON.parse(localStorage.getItem('selectedCompany'));
var CompanyIdEsc = companyDetailsESc.CompanyId;

var escalationGrid = null;
var escalationRows = [];

$("#btnExportEscalation").click(function () {
    downloadPendingProbationGridExcel();
})

$("#btnfetchUpcomingEscalaionProbation").click(async function () {
    if (localStorage.getItem("EmployeeId") === '14') {
        await fetchUpcomingEscalaionProbation();
    } else {
        showToast("You are not authorized to access this report.", "error");
    }
});



async function fetchUpcomingEscalaionProbation() {
    try {

        $.ajax({
            type: "GET",
            url: BaseUrlLayout + '/ApprovalMasterAPI/GetEscalationDueList/' + CompanyIdEsc,
            contentType: 'application/json',

            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem("authToken")
            },
            success: function (data) {
                if (data.isSuccess) {
                    escalationRows = data.data || [];
                    $('#escalatioModel').modal('show');
                    renderEscalationGrid(escalationRows);
                    localStorage.removeItem('EscalationSummary');
                }
                else
                {
                    $('#escalatioModel').modal('hide');
                }


            },
            error: function (xhr, status, error) {
                console.error("AJAX Error:", status, error);

            }
        });
    } catch (e) {
        console.error("Error in escalation:", e);

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
            height: '50vh',
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

