
$(document).ready(async function () {
    var notiEsc = localStorage.getItem('EscalationSummary');
    if (notiEsc == 'true' && localStorage.getItem("EmployeeId")=='14' ) {
        await fetchUpcomingEscalaionProbation();
        
    }
});
var companyDetailsESc = JSON.parse(localStorage.getItem('selectedCompany'));
var CompanyIdEsc = companyDetailsESc.CompanyId;

$("#btnExportEscalation").click(function () {
    downloadPendingProbationGridExcel();
})

$("#btnfetchUpcomingEscalaionProbation").click(async function () {
    if (localStorage.getItem("EmployeeId") === '14') {
        await fetchUpcomingEscalaionProbation();
    } else {
        round_error_noti("You are not authorized to access this report.");
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
                    $('#escalatioModel').modal('show');
                    $("#upEscalationContainer").dxDataGrid({
                        dataSource: data.data || [],
                        columns: [
                            { dataField: 'EmployeeName', caption: 'Employee Name', alignment: 'left', minWidth: 220 },
                            { dataField: 'ApproverName', caption: 'Approver Name', alignment: 'left', minWidth: 220 },
                            { dataField: 'StatusName', caption: 'Status', alignment: 'left', minWidth: 120 },
                            { dataField: 'LevelNo', caption: 'Level No', alignment: 'left', minWidth: 70 },
                            { dataField: 'RemainingDaysString', caption: 'Days Remaining', alignment: 'center', minWidth: 120 },
                            {
                                dataField: 'EscalationDueOn',
                                caption: 'Escalation Date',
                                alignment: 'center',
                                minWidth: 150,
                                dataType: 'date',
                                format: 'dd-MM-yyyy'
                            },
                            {
                                dataField: 'ProbationEndDate',
                                caption: 'Probation End Date',
                                alignment: 'center',
                                minWidth: 150,
                                dataType: 'date',
                                format: 'dd-MM-yyyy'
                            }
                        ],
                        columnsAutoWidth: true,
                        wordWrapEnabled: false,
                        rowAlternationEnabled: true,
                        showBorders: true,
                        height: 300,
                        paging: { enabled: false },
                        headerFilter: { visible: false },
                        filterRow: { visible: false, applyFilter: "auto" },
                        allowColumnResizing: false,
                        groupPanel: { visible: false },
                        searchPanel: {
                            visible: true,
                            width: 200,
                            placeholder: "Search..."
                        },
                        scrolling: {
                            mode: "standard",
                            useNative: false,
                            scrollByContent: true,
                            scrollByThumb: true,
                            showScrollbar: "always"
                        },
                        allowColumnReordering: false,
                        columnFixing: { enabled: false },
                        onContentReady: function (e) {
                            $("#upEscalationRecord").html("Total Records: " + e.component.totalCount());
                        }
                    });
                    localStorage.removeItem('EscalationSummary');
                }
                else
                {
                    $('#escalatioModel').hide();
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


function downloadPendingProbationGridExcel() {

    const grid = $("#upEscalationContainer").dxDataGrid("instance");

    if (!grid) {
        round_error_noti("Grid not loaded");
        return;
    }

    if (grid.totalCount() === 0) {
        round_error_noti("No data available to export");
        return;
    }


    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet("Pending Probation Data");

    DevExpress.excelExporter.exportDataGrid({
        component: grid,
        worksheet: worksheet,
        autoFilterEnabled: true

    })
        .then(() => {
            return workbook.xlsx.writeBuffer();
        })
        .then((buffer) => {
            saveAs(
                new Blob([buffer], { type: "application/octet-stream" }),
                `PendingProbation.xlsx`
            );
        })
        .catch((error) => {
            console.error("Excel export failed:", error);
            round_error_noti("Failed to export Excel");
        });
}


