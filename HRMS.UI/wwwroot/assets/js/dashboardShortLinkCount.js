

const DashboardApiService = (function () {
    // Private variables
    const Token = localStorage.getItem("authToken");
    const companyDetails = JSON.parse(localStorage.getItem('selectedCompany'));
    const CompanyId = companyDetails?.CompanyId;
    let baseUrl = '';

    // Initialize base URL
    function init(url) {
        baseUrl = url;
    }

    // Generic API call function
    function apiCall(options) {
        const defaults = {
            type: 'GET',
            headers: {
                'Authorization': 'Bearer ' + Token,
                'Content-Type': 'application/json'
            },
            success: null,
            error: null
        };

        const settings = { ...defaults, ...options };

        return $.ajax(settings);
    }

    // Leave Application Count
    function getLeaveApplicationCount(callback) {
        const payload = {
            SearchType: '',
            SearchFor: '',
            BranchId: '0',
            CompId: CompanyId
        };

        apiCall({
            url: `${baseUrl}/api/LeaveApplication/GetLeaveApplicationsforApproveAdmin`,
            type: 'POST',
            data: JSON.stringify(payload),
            success: function (data) {
                const count = (data.isSuccess && data.data) ? data.data.length : 0;
                if (callback) callback(count, null);
            },
            error: function (xhr, status, error) {
                console.error("Error loading leave application count:", error);
                if (callback) callback(0, error);
            }
        });
    }

    // Comp Off Application Count
    function getCompOffApplicationCount(callback) {
        const payload = {
            SearchType: null,
            SearchFor: null,
            CompId: CompanyId,
            BranchId: null,
            ApplicationStatus: 'Pending'
        };

        apiCall({
            url: `${baseUrl}/api/CompOffAPI/GetCompOffDetailsReportForAdmin`,
            type: 'POST',
            data: JSON.stringify(payload),
            success: function (data) {
                const count = (data.isSuccess && data.data) ? data.data.length : 0;
                if (callback) callback(count, null);
            },
            error: function (xhr, status, error) {
                console.error("Error loading comp off application count:", error);
                if (callback) callback(0, error);
            }
        });
    }

    // Leave Cancellation Count (example - add your actual API endpoint)
    function getLeaveCancellationCount(callback) {
        // Add your API call here
        apiCall({
            url: `${baseUrl}/api/YourEndpoint/GetLeaveCancellation`,
            success: function (data) {
                const count = (data.isSuccess && data.data) ? data.data.length : 0;
                if (callback) callback(count, null);
            },
            error: function (xhr, status, error) {
                console.error("Error loading leave cancellation count:", error);
                if (callback) callback(0, error);
            }
        });
    }

    // Pre Comp Off Count
    function getPreCompOffCount(callback) {
        // Add your API call here
        if (callback) callback(0, null); // Placeholder
    }

    // Loan Application Count
    function getLoanApplicationCount(callback) {
        // Add your API call here
        if (callback) callback(0, null); // Placeholder
    }

    // Claim Application Count
    function getClaimApplicationCount(callback) {
        // Add your API call here
        if (callback) callback(0, null); // Placeholder
    }

    // Attendance Regularization Count
    function getAttendanceRegularizationCount(callback) {
        // Add your API call here
        if (callback) callback(0, null); // Placeholder
    }

    // Load all dashboard counts at once
    function loadAllCounts(callbacks) {
        getLeaveApplicationCount(callbacks.leaveApplication);
        getCompOffApplicationCount(callbacks.compOff);
        getLeaveCancellationCount(callbacks.leaveCancellation);
        getPreCompOffCount(callbacks.preCompOff);
        getLoanApplicationCount(callbacks.loan);
        getClaimApplicationCount(callbacks.claim);
        getAttendanceRegularizationCount(callbacks.attendanceReg);
    }

    // Public API
    return {
        init: init,
        getLeaveApplicationCount: getLeaveApplicationCount,
        getCompOffApplicationCount: getCompOffApplicationCount,
        getLeaveCancellationCount: getLeaveCancellationCount,
        getPreCompOffCount: getPreCompOffCount,
        getLoanApplicationCount: getLoanApplicationCount,
        getClaimApplicationCount: getClaimApplicationCount,
        getAttendanceRegularizationCount: getAttendanceRegularizationCount,
        loadAllCounts: loadAllCounts
    };
})();