// Global variables to track data loading status
let compOffLoaded = false;
let upcomingProbationLoaded = false;
let pendingProbationLoaded = false;
let loanLoaded = false;
let escalationLoaded = false;

// Global variables to track if tabs have data
let hasCompOffData = false;
let hasUpcomingProbationData = false;
let hasPendingProbationData = false;
let hasLoanData = false;
let hasEscalationData = false;

// Guards checkAllDataLoaded()'s tab-selection/modal.show() logic from running
// more than once. It's called once per fetch's completion (4 total), and
// without this guard every one of them that completes AFTER the point all 4
// flags are already true would redundantly re-run that logic and re-invoke
// modal.show() on a fresh bootstrap.Modal instance — which is what showed up
// as multiple stacked popups after login.
let modalDecisionMade = false;

// Show loading indicators for all tabs initially
function showLoadingIndicators() {
    $('#compOffLoader, #upcomingProbationLoader, #pendingProbationLoader').show();
    $('#compOffLoading, #upcomingProbationLoading, #pendingProbationLoading').show();
    $('#compOffContent, #upcomingProbationContent, #pendingProbationContent').hide();
}

// Hide loading indicators and show content for a specific tab
function hideLoadingIndicators(tabId) {
    switch (tabId) {
        case 'compOff':
            $('#compOffLoader').hide();
            $('#compOffLoading').hide();
            $('#compOffContent').show();
            break;
        case 'upcomingProbation':
            $('#upcomingProbationLoader').hide();
            $('#upcomingProbationLoading').hide();
            $('#upcomingProbationContent').show();
            break;
        case 'pendingProbation':
            $('#pendingProbationLoader').hide();
            $('#pendingProbationLoading').hide();
            $('#pendingProbationContent').show();
            break;
        case 'loan':
            $('#loanLoader').hide();
            $('#loanLoading').hide();
            $('#loanContent').show();
            break;
        case 'escalation':
            $('#escalationLoader').hide();
            $('#escalationLoading').hide();
            $('#escalationContent').show();
            break;
    }
}

// Check if all data is loaded and set active tab
function checkAllDataLoaded() {
    // Wait until all APIs finish
    if (!(compOffLoaded && upcomingProbationLoaded && pendingProbationLoaded && loanLoaded && escalationLoaded)) {
        return;
    }

    // Only decide the active tab / show the modal once — see the comment on
    // modalDecisionMade's declaration above.
    if (modalDecisionMade) {
        return;
    }
    modalDecisionMade = true;

    // Selectors below are plain ID/class lookups rather than scoped to
    // .leave-balance-modal-container — once shown, Bootstrap relocates
    // #leaveBalanceModal to be a direct child of <body>, so anything still
    // scoped to that container silently matches zero elements from that
    // point on (all these IDs are unique page-wide anyway).

    // ----------------------------
    // Hide tabs without data
    // ----------------------------
    if (!hasCompOffData) {
        $('#compOffTab').closest('li').hide();
        $('#compOff').removeClass('active show');
    }

    if (!hasUpcomingProbationData) {
        $('#upCommingProbationTab').closest('li').hide();
        $('#upCommingProbation').removeClass('active show');
    }

    if (!hasPendingProbationData) {
        $('#pendingProbationTab').closest('li').hide();
        $('#pendingProbation').removeClass('active show');
    }

    if (!hasLoanData) {
        $('#loanTab').closest('li').hide();
        $('#loan').removeClass('active show');
    }

    if (!hasEscalationData) {
        $('#escalationTab').closest('li').hide();
        $('#escalation').removeClass('active show');
    }

    // ----------------------------
    // Decide active tab (PRIORITY)
    // ----------------------------
    let activeTabId = null;
    let activePaneId = null;

    if (hasCompOffData) {
        activeTabId = '#compOffTab';
        activePaneId = '#compOff';
    }
    else if (hasUpcomingProbationData) {
        activeTabId = '#upCommingProbationTab';
        activePaneId = '#upCommingProbation';
    }
    else if (hasPendingProbationData) {
        activeTabId = '#pendingProbationTab';
        activePaneId = '#pendingProbation';
    }
    else if (hasLoanData) {
        activeTabId = '#loanTab';
        activePaneId = '#loan';
    }
    else if (hasEscalationData) {
        activeTabId = '#escalationTab';
        activePaneId = '#escalation';
    }

    // ----------------------------
    // No data in all tabs → do NOT open modal
    // ----------------------------
    if (!activeTabId) {
        $('#noRecordsMessage').show();
        return;
    }

    // ----------------------------
    // Activate selected tab
    // ----------------------------
    $('#reportTabs button').removeClass('active');
    $('.tab-pane').removeClass('active show');

    $(activeTabId).addClass('active');
    $(activePaneId).addClass('active show');

    $('#noRecordsMessage').hide();

    // ----------------------------
    // Show modal
    // ----------------------------
    const modalElement = document.getElementById('leaveBalanceModal');
    const modal = new bootstrap.Modal(modalElement);

    modal.show();
    localStorage.setItem('NotificationSummary', 'false');
}

// Fetch Comp-Off Leave Data
async function fetchCompOffLeave() {
    try {
        $('#compOffLoader').show();

        const response = await $.ajax({
            url: BaseUrlLayout + '/NotificationRemainderAPI/GetRemainingCompOffLeave/' + localStorage.getItem('EmployeeId'),
            method: 'GET'
        });

        if (response.data && response.data.length > 0) {
            hasCompOffData = true;
            renderLeaveRecords(response.data, 'compOff');
            $("#compOffTotal").text(getTotalBalance(response.data));
        } else {
            hasCompOffData = false;
            $('#compOffRecordsContainer').empty();
            $('#compOffRecordsContainer').append('<div class="text-center py-3">No comp-off records found</div>');
        }

        compOffLoaded = true;
        hideLoadingIndicators('compOff');
        checkAllDataLoaded();
    } catch (error) {
        console.error('Error fetching Comp-Off data:', error);
        hasCompOffData = false;
        compOffLoaded = true;
        hideLoadingIndicators('compOff');
        $('#compOffRecordsContainer').empty();
        $('#compOffRecordsContainer').append('<div class="text-center py-3 text-danger">Error loading comp-off records</div>');
        checkAllDataLoaded();
    }
}

// Fetch Pending Probation Data
async function fetchPendingProbation() {
    try {
        $('#pendingProbationLoader').show();

        return $.ajax({
            type: "POST",
            url: BaseUrlLayout + '/ApprovalManagementAPI/GetPendingApprovalRequests',
            contentType: 'application/json',
            data: JSON.stringify({
                ApproverEmployeeId: parseInt(localStorage.getItem("EmployeeId")),
                StatusId: 1
            }),
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem("authToken")
            },
            success: function (data) {
                if (data.isSuccess && data.data && data.data.length > 0) {
                    hasPendingProbationData = true;
                    $("#pendingProbationTotal").text(data.data.length);
                    setupPendingProbationGrid(data.data);
                } else {
                    hasPendingProbationData = false;
                    $("#pendingProbationRecordsContainer").empty();
                    $("#pendingProbationRecordsContainer").append('<div class="text-center py-3">No pending probation records found</div>');
                }

                pendingProbationLoaded = true;
                hideLoadingIndicators('pendingProbation');
                checkAllDataLoaded();
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error:", status, error);
                hasPendingProbationData = false;
                pendingProbationLoaded = true;
                hideLoadingIndicators('pendingProbation');
                $("#pendingProbationRecordsContainer").empty();
                $("#pendingProbationRecordsContainer").append('<div class="text-center py-3 text-danger">Error loading pending probation records</div>');
                checkAllDataLoaded();
            }
        });
    } catch (e) {
        console.error("Error in fetchPendingProbation:", e);
        hasPendingProbationData = false;
        pendingProbationLoaded = true;
        hideLoadingIndicators('pendingProbation');
        $("#pendingProbationRecordsContainer").empty();
        $("#pendingProbationRecordsContainer").append('<div class="text-center py-3 text-danger">Error loading pending probation records</div>');
        checkAllDataLoaded();
    }
}

// Fetch Upcoming Probation Data
async function fetchUpcomingProbation() {
    try {
        $('#upcomingProbationLoader').show();

        return $.ajax({
            type: "POST",
            url: BaseUrlLayout + '/ApprovalManagementAPI/GetUpcomingProbationDetails',
            contentType: 'application/json',
            data: JSON.stringify({
                EmployeeId: parseInt(localStorage.getItem("EmployeeId"))
            }),
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem("authToken")
            },
            success: function (data) {
                if (data.isSuccess && data.data && data.data.length > 0) {
                    hasUpcomingProbationData = true;
                    $("#upcomingProbationTotal").text(data.data[0].TotalCount || data.data.length);
                    setupUpcomingProbationGrid(data.data);
                } else {
                    hasUpcomingProbationData = false;
                    $("#upCommingProbationRecordsContainer").empty();
                    $("#upCommingProbationRecordsContainer").append('<div class="text-center py-3">No upcoming probation records found</div>');
                }

                upcomingProbationLoaded = true;
                hideLoadingIndicators('upcomingProbation');
                checkAllDataLoaded();
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error:", status, error);
                hasUpcomingProbationData = false;
                upcomingProbationLoaded = true;
                hideLoadingIndicators('upcomingProbation');
                $("#upCommingProbationRecordsContainer").empty();
                $("#upCommingProbationRecordsContainer").append('<div class="text-center py-3 text-danger">Error loading upcoming probation records</div>');
                checkAllDataLoaded();
            }
        });
    } catch (e) {
        console.error("Error in fetchUpcomingProbation:", e);
        hasUpcomingProbationData = false;
        upcomingProbationLoaded = true;
        hideLoadingIndicators('upcomingProbation');
        $("#upCommingProbationRecordsContainer").empty();
        $("#upCommingProbationRecordsContainer").append('<div class="text-center py-3 text-danger">Error loading upcoming probation records</div>');
        checkAllDataLoaded();
    }
}


// Fetch Loan Data
async function fetchLoanData() {
    try {
        $('#loanLoader').show();
        const requestUrl = BaseUrlLayout + '/LoanApplicationAPI/GetPendingLoanApprovalRequests';
        const requestData = {
            ApproverEmployeeId: parseInt(localStorage.getItem("EmployeeId")),
            ApproverMatserid: 2,
            StatusId: 7
        };

        return $.ajax({
            type: "GET",
            url: requestUrl,
            data: requestData,
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem("authToken")
            },
            success: function (response) {
         

                if (response.isSuccess && response.data) {
                    // Convert single object to array if needed
                    let loanData = Array.isArray(response.data) ? response.data : [response.data];

                    if (loanData.length > 0) {
                        hasLoanData = true;
                        $("#loanTotal").text(loanData.length);

                        loanLoaded = true;
                        hideLoadingIndicators('loan');

                        // Call grid setup after showing content
                        setTimeout(() => {
                            setupLoanGrid(loanData);
                        }, 200);

                        checkAllDataLoaded();
                    } else {
                        hasLoanData = false;
                        loanLoaded = true;
                        hideLoadingIndicators('loan');
                        $("#loanRecordsContainer").empty();
                        $("#loanRecordsContainer").append(
                            '<div class="text-center py-3">No loan records found</div>'
                        );
                        checkAllDataLoaded();
                    }
                } else {
                    hasLoanData = false;
                    loanLoaded = true;
                    hideLoadingIndicators('loan');
                    $("#loanRecordsContainer").empty();
                    $("#loanRecordsContainer").append(
                        '<div class="text-center py-3">No loan records found</div>'
                    );
                    checkAllDataLoaded();
                }
            },
            error: function (xhr, status, error) {
                console.error("Loan API Error:", status, error);
                hasLoanData = false;
                loanLoaded = true;
                hideLoadingIndicators('loan');
                $("#loanRecordsContainer").empty();
                $("#loanRecordsContainer").append(
                    '<div class="text-center py-3 text-danger">Error loading loan records</div>'
                );
                checkAllDataLoaded();
            }
        });
    } catch (e) {
        console.error("Loan Fetch Exception:", e);
        hasLoanData = false;
        loanLoaded = true;
        hideLoadingIndicators('loan');
        $("#loanRecordsContainer").empty();
        $("#loanRecordsContainer").append(
            '<div class="text-center py-3 text-danger">Error loading loan records</div>'
        );
        checkAllDataLoaded();
    }
}

function setupLoanGrid(data) {
    console.log("Setting up loan grid with data:", data);

    const container = $("#loanRecordsContainer");

    if (container.length === 0) {
        console.error("Loan container not found!");
        return;
    }

    // Check if container is visible
    if (!container.is(':visible')) {
        console.warn("Container not visible, waiting...");
        setTimeout(() => setupLoanGrid(data), 300);
        return;
    }

    // Clear container completely
    container.empty();

    try {
        // Initialize fresh grid
        const gridInstance = container.dxDataGrid({
            dataSource: data,
            keyExpr: 'approvalRequestId', // Important: unique key
            columns: [
                {
                    dataField: 'requesterName',
                    caption: 'Requester',
                    alignment: 'left',
                    width: 200
                },
                {
                    dataField: 'requestTitle',
                    caption: 'Request Title',
                    alignment: 'left',
                    width: 250
                },
                {
                    dataField: 'levelNo',
                    caption: 'Level',
                    alignment: 'center',
                    width: 80
                },
                {
                    dataField: 'levelStatus',
                    caption: 'Status',
                    alignment: 'center',
                    width: 100
                },
                {
                    dataField: 'assignedOn',
                    caption: 'Assigned On',
                    alignment: 'center',
                    width: 120,
                    dataType: 'date',
                    format: 'dd-MM-yyyy'
                },
                {
                    dataField: 'escalationDueOn',
                    caption: 'Escalation Due',
                    alignment: 'center',
                    width: 150,
                    dataType: 'date',
                    format: 'dd-MM-yyyy'
                }
            ],
            showBorders: true,
            showRowLines: true,
            showColumnLines: true,
            rowAlternationEnabled: true,
            hoverStateEnabled: true,
            columnAutoWidth: false,
            wordWrapEnabled: false,
            paging: {
                enabled: true,
                pageSize: 10
            },
            pager: {
                visible: true,
                showPageSizeSelector: true,
                allowedPageSizes: [10, 25, 50, 100],
                showInfo: true,
                showNavigationButtons: true
            },
            headerFilter: {
                visible: false
            },
            filterRow: {
                visible: false
            },
            searchPanel: {
                visible: false
            },
            groupPanel: {
                visible: false
            },
            scrolling: {
                mode: "standard"
            },
            loadPanel: {
                enabled: true
            },
            noDataText: "No loan records available",
            onContentReady: function (e) {
                const rowCount = e.component.totalCount();
                console.log("Loan grid ready - Total rows:", rowCount);

                if (rowCount === 0) {
                    console.warn("Grid initialized but no rows displayed!");
                }
            },
            onInitialized: function (e) {
                console.log("Grid initialized");
            }
        }).dxDataGrid("instance");

        // Force refresh
        gridInstance.refresh();

        console.log("Loan grid setup complete");

    } catch (error) {
        console.error("Error initializing loan grid:", error);
        container.empty();
        container.append(
            '<div class="text-center py-3 text-danger">Error displaying loan records: ' + error.message + '</div>'
        );
    }
}

function setupLoanGrid(data) {
    $("#loanRecordsContainer").dxDataGrid({
        dataSource: data || [],
        height: 360,
        columns: [
            { dataField: 'requesterName', caption: 'Requester', alignment: 'left', minWidth: 200 },
            { dataField: 'levelNo', caption: 'Level', alignment: 'left', minWidth: 120 },
            { dataField: 'levelStatus', caption: 'Status', alignment: 'left', minWidth: 120 },
            {
                dataField: 'assignedOn',
                caption: 'Assigned On',
                alignment: 'center',
                minWidth: 150,
                dataType: 'date',
                format: 'dd-MM-yyyy'
            },
            {
                dataField: 'escalationDueOn',
                caption: 'Escalation Due',
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
}

// Setup Pending Probation Grid
function setupPendingProbationGrid(data) {
    $("#pendingProbationRecordsContainer").dxDataGrid({
        dataSource: data || [],
        height: 360,
        columns: [
            { dataField: 'RequesterName', caption: 'Requester', alignment: 'left', minWidth: 200 },
            { dataField: 'CurrentLevelSequence', caption: 'Level', alignment: 'left', minWidth: 120 },
            { dataField: 'LevelStatus', caption: 'Status', alignment: 'left', minWidth: 120 },
            {
                dataField: 'AssignedOn',
                caption: 'Assigned On',
                alignment: 'center',
                minWidth: 150,
                dataType: 'date',
                format: 'dd-MM-yyyy'
            },
            {
                dataField: 'EscalationDueOn',
                caption: 'Escalation Due',
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
}

function setupUpcomingProbationGrid(data) {
    $("#upCommingProbationRecordsContainer").dxDataGrid({
        dataSource: data || [],
        height: 360,
        columns: [
            { dataField: 'EmployeeName', caption: 'Employee Name', alignment: 'left', minWidth: 250 },
            { dataField: 'DaysRemaining', caption: 'Days Remaining', alignment: 'center', minWidth: 120 },
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
}

// Render leave records for a specific tab
function renderLeaveRecords(data, tabId) {
    if (!data) return;

    if (data.length > 0) {
        $('#employeeName').text(data[0].Name);
        $('#employeeCode').text(data[0].EmployeeCode);
    }

    const containerId = `${tabId}RecordsContainer`;
    $(`#${containerId}`).empty();

    if (data.length === 0) {
        $(`#${containerId}`).append('<div class="text-center py-3">No comp-off records found</div>');
        return;
    }

    $.each(data, function (index, leave) {
        const recordHtml = `
            <div class="leave-card">
                <div class="d-flex justify-content-between align-items-center mb-2">
                    <span class="record-badge">#${index + 1}</span>
                    <div class="balance-badge">
                        <p class="balance-number">${leave.RemainingBalance}</p>
                        <p class="balance-text">day${leave.RemainingBalance !== 1 ? 's' : ''}</p>
                    </div>
                </div>
                <div class="row g-2">
                    <div class="col-6">
                        <div class="info-box info-box-blue">
                            <div class="info-label text-primary">
                                <i class='bx bx-calendar-plus' style="font-size: 0.95rem;"></i>
                                <span>Earned Days</span>
                            </div>
                            <p class="info-value text-primary">${leave.EarnedDays}</p>
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="info-box info-box-amber">
                            <div class="info-label text-warning">
                                <i class='bx bx-calendar-check' style="font-size: 0.9rem;"></i>
                                <span>Remaining Days</span>
                            </div>
                            <p class="info-value text-warning">${leave.RemainingDays}</p>
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="info-box info-box-purple">
                            <div class="info-label text-purple">
                                <i class='bx bx-calendar-event' style="font-size: 0.9rem;"></i>
                                <span>Approved Date</span>
                            </div>
                            <p class="info-value text-purple small">${formatDate(leave.ApprovedDate)}</p>
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="info-box info-box-rose">
                            <div class="info-label text-danger">
                                <i class='bx bx-time-five' style="font-size: 0.9rem;"></i>
                                <span>Valid Till</span>
                            </div>
                            <p class="info-value text-danger small">${formatDate(leave.ValidTill)}</p>
                        </div>
                    </div>
                </div>
            </div>
        `;
        $(`#${containerId}`).append(recordHtml);
    });
}

// Format date function
function formatDate(dateString) {
    if (!dateString) return '';
    const date = new Date(dateString);
    const options = { year: 'numeric', month: 'short', day: 'numeric' };
    return date.toLocaleDateString('en-US', options);
}

// Calculate total balance
function getTotalBalance(data) {
    return data.reduce((sum, item) => sum + item.RemainingBalance, 0);
}

// Initialize everything when document is ready
$(document).ready(async function () {

    var noti = localStorage.getItem('NotificationSummary');
    if (noti == 'true') {
        await fetchCompOffLeave();
        await fetchPendingProbation();
        await fetchUpcomingProbation();
        await  fetchLoanData();

        // escalation.js (and the #escalation tab) is only loaded on the Admin
        // layout — on pages that don't include it, mark it loaded-with-no-data
        // immediately so checkAllDataLoaded()'s gate doesn't wait forever.
        if (typeof fetchUpcomingEscalaionProbation === 'function') {
            await fetchUpcomingEscalaionProbation();
        } else {
            escalationLoaded = true;
            hasEscalationData = false;
            checkAllDataLoaded();
        }
    }
});