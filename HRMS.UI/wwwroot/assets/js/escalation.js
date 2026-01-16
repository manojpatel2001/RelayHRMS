
$(document).ready(async function () {
    var notiEsc = localStorage.getItem('EscalationSummary');
    if (notiEsc == 'true') {
        debugger;
        await fetchUpcomingEscalaionProbation();
        
    }
});


async function fetchUpcomingEscalaionProbation() {
    try {

        $.ajax({
            type: "GET",
            url: BaseUrlLayout + '/ApprovalMasterAPI/GetEscalationDueList/' + localStorage.getItem("EmployeeId"),
            contentType: 'application/json',
            
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem("authToken")
            },
            success: function (data) {
                debugger;
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
                        paging: { pageSize: 10 },
                        pager: {
                            showPageSizeSelector: true,
                            allowedPageSizes: [10, 25, 50, 100]
                        },
                        headerFilter: { visible: false },
                        filterRow: { visible: false, applyFilter: "auto" },
                        allowColumnResizing: false,
                        groupPanel: { visible: false },
                        searchPanel: {
                            visible: false,
                            width: 200,
                            placeholder: "Search..."
                        },
                        scrolling: {
                            mode: "standard",
                            useNative: true,
                            showScrollbar: "always"
                        },
                        allowColumnReordering: false,
                        columnFixing: { enabled: false }
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


