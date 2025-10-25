async function GetCompOffLeave() {
    try {
        const response = await new Promise((resolve, reject) => {
            $.ajax({
                url: BaseUrlLayout + '/NotificationRemainderAPI/GetRemainingCompOffLeave/'+ localStorage.getItem('EmployeeId'),
                method: 'GET',
                success: function (response) {
                    resolve(response);
                },
                error: function (error) {
                    reject(error);
                }
            });
        });
        if (response.isSuccess) {
            showLeaveModal(response.data)
        }
        
    } catch (error) {
        console.error('Error fetching data:', error);
    }
}


// Format date function
function formatDate(dateString) {
    const date = new Date(dateString);
    const options = { year: 'numeric', month: 'short', day: 'numeric' };
    return date.toLocaleDateString('en-US', options);
}

// Calculate total balance
function getTotalBalance(data) {
    return data.reduce((sum, item) => sum + item.RemainingBalance, 0);
}

// Render leave records using jQuery
function renderLeaveRecords(data) {
    if (data == null)
        return;

    // Set employee info
    if (data.length > 0) {
        $('#employeeName').text(data[0].Name);
        $('#employeeCode').text(data[0].EmployeeCode);
        $('#totalBalance').text(getTotalBalance(data));
    }

    // Clear container
    $('#leaveRecordsContainer').empty();

    // Render each record
    $.each(data, function (index, leave) {
        const recordHtml = `
             <div class="leave-card">
                 <!-- Header -->
                 <div class="d-flex justify-content-between align-items-center mb-2">
                     <span class="record-badge">#${index + 1}</span>
                     <div class="balance-badge">
                         <p class="balance-number">${leave.RemainingBalance}</p>
                         <p class="balance-text">day${leave.RemainingBalance !== 1 ? 's' : ''}</p>
                     </div>
                 </div>

                 <!-- Info Grid -->
                 <div class="row g-2">
                     <div class="col-6">
                         <div class="info-box info-box-blue">
                             <div class="info-label text-primary">
                                 <i class='bx bx-calendar-plus' style="font-size: 0.9rem;"></i>
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

        $('#leaveRecordsContainer').append(recordHtml);
    });
}

// Show modal function
function showLeaveModal(data) {
    renderLeaveRecords(data);
    const modal = new bootstrap.Modal(document.getElementById('leaveBalanceModal'));
    modal.show();
    sessionStorage.setItem('leaveModalShown', 'true');
}

$(document).ready(function () {
    if (localStorage.getItem('compOffLeaveShown')) {
        GetCompOffLeave();
    }
    localStorage.removeItem('compOffLeaveShown');
});


// Add custom CSS for purple color
$('<style>.text-purple { color: #7c3aed !important; }</style>').appendTo('head');