
const userIdNotification = localStorage.getItem("EmployeeId");
const tokenNotification = localStorage.getItem("authToken");
const notificationConnection = new signalR.HubConnectionBuilder()
    .withUrl(`${BaseDomainUrl}/notificationRemainderHub?userId=${userIdNotification}`, {
        accessTokenFactory: () => tokenNotification
    })
  //  .configureLogging(signalR.LogLevel.Information) // Detailed logging
    .withAutomaticReconnect()
    .build();

// Function to start connection
async function startConnection() {
    try {
        await notificationConnection.start();
    } catch (err) {
        // Retry after 5 seconds
        setTimeout(startConnection, 5000);
    }
}

//// Reconnecting event
//notificationConnection.onreconnecting((error) => {
//    console.warn(`[${new Date().toLocaleTimeString()}] ⚠️ Connection lost. Reconnecting...`, error);
//});

//// Reconnected event
//notificationConnection.onreconnected((connectionId) => {
//    console.log(`[${new Date().toLocaleTimeString()}] 🔄 Reconnected successfully. New ConnectionId:`, connectionId);
//});

// Connection closed
notificationConnection.onclose((error) => {
    //console.error(`[${new Date().toLocaleTimeString()}] ❌ Connection closed.`, error);
    // Optional: automatic retry after 5 seconds
    setTimeout(startConnection, 5000);
});

// Example: Receive messages
notificationConnection.on("ReceiveNotificationRemainder", (notification) => {
    playNotificationSound();
    //console.log(` New message:`, notification);
    showNotificationToast(notification);
    loadAllUnreadNotification();
});

// Start the connection
startConnection();



//function incrementNotificationCount() {
//    let badgeCount = parseInt($("#notificationCount").text());
//    badgeCount = badgeCount+1;
//    $("#notificationCount").text(badgeCount);
//}




function playNotificationSound() {
    var audio = $("#notificationSound")[0];
    audio.currentTime = 0;
    audio.play().catch(function (error) {
        console.error("Error playing notification sound:", error);
    });
}





$(document).ready(async function () {
    await loadAllUnreadNotification();
    // Initialize toast container if it doesn't exist
    if ($('#toast-container').length === 0) {
        $('body').append('<div id="toast-container" class="toast-container"></div>');
    }
});
let duration = 15000;
function showNotificationToast(notification) {
    var toastHtml = `
        <div class="success-toast">
            <div class="toast-icon">
                <i class="bx bx-check text-white fs-6"></i>
            </div>
            <div class="toast-content">
                <div class="toast-title">${notification.notificationType}</div>
                <div class="toast-message">${notification.notificationMessage}</div>
            </div>
            <button class="toast-close">
                <i class="bx bx-x fs-4"></i>
            </button>
            <div class="toast-progress">
                <div class="toast-progress-bar"></div>
            </div>
        </div>
    `;
    var $toast = $(toastHtml);
    var autoCloseTimer;
    var isPaused = false;

    $('#toast-container').append($toast);

    // Animation sequence: right -> left -> center
    setTimeout(function () {
        $toast.addClass('slide-in');
    }, 100);
    setTimeout(function () {
        $toast.removeClass('slide-in').addClass('show');
    }, 300);

  
    // Pause on hover/touch
    // Pause on hover/touch
    $toast.on({
        mouseenter: function () {
            isPaused = true;
            $toast.addClass('paused');
            clearTimeout(autoCloseTimer);
        },
        mouseleave: function () {
            isPaused = false;
            $toast.removeClass('paused');
            autoCloseTimer = setTimeout(function () {
                closeToast($toast);
            }, duration);
        },
        touchstart: function (e) {
            // Don't prevent default if clicking close button
            if (!$(e.target).closest('.toast-close').length) {
                isPaused = true;
                $toast.addClass('paused');
                clearTimeout(autoCloseTimer);
                e.preventDefault();
            }
        },
        touchend: function (e) {
            // Don't prevent default if clicking close button
            if (!$(e.target).closest('.toast-close').length) {
                isPaused = false;
                $toast.removeClass('paused');
                autoCloseTimer = setTimeout(function () {
                    closeToast($toast);
                }, duration);
                e.preventDefault();
            }
        }
    });

 

    // Initial auto close timer
    autoCloseTimer = setTimeout(function () {
        if (!isPaused) {
            closeToast($toast);
        }
    }, duration);

    return $toast;
}

function closeToast($toast) {
    $toast.removeClass('show paused').addClass('hide');
    setTimeout(function () {
        $toast.remove();
    }, 400);
}




async function loadAllUnreadNotification() {
    try {
        const response = await fetch(BaseUrlLayout + '/NotificationRemainderAPI/GetAllNotificationByUserId/' + parseInt(userIdNotification), {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + tokenNotification,
                'Content-Type': 'application/json' // Important for JSON payloads
            }
        });


        const notificationData = await response.json();

        if (notificationData.isSuccess) {

            var notificationDetails = notificationData.data;
            updateNotificationList(notificationDetails)
        }
        else {

        }
    } catch (error) {
        console.log('Fetch error:', error);
    }

}



$(document).ready(function () {
    // Initialize with some sample notifications
    
    // Toggle dropdown on bell click
    $('#bellIcon').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
        toggleNotificationDropdown();
    });

    // Close dropdown when clicking outside
    $(document).on('click', function (e) {
        if (!$(e.target).closest('#bellIcon').length) {
            closeNotificationDropdown();
        }
    });
});

function toggleNotificationDropdown() {
    $('#notificationDropdown').toggleClass('show');
}

function closeNotificationDropdown() {
    $('#notificationDropdown').removeClass('show');
}




async function redirectPage(pageUrl,notificationType)
{
    if (notificationType == "Attendance Application" || notificationType == "Leave Application" || notificationType == "Leave Approval"
        || notificationType == "CompOff Application" || notificationType == "CompOff Approval" || notificationType == "Attendance Approval" ||
        notificationType == "Ticket FollowUp" || notificationType == "Ticket Application" || notificationType == "Ticket Response")
    {
        await readNotification(notificationType);
    }
    window.location.href = pageUrl;
}

async function readNotification(notificationType)
{
    try {
        const response = await fetch(BaseUrlLayout + '/NotificationRemainderAPI/ReadNotificationRemainder', {
            method: 'POST',
            headers: {
                'Authorization': 'Bearer ' + tokenNotification,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                UserId: userIdNotification,
                notificationType: notificationType
            })
        });


        const notificationData = await response.json();
       
       
    } catch (error) {
        console.log('Fetch error:', error);
    }
}
function updateNotificationList(notificationDetails) {
    var notificationsList = notificationDetails.notifications;
    if (notificationsList.length === 0) {
        $list.html(`
                    <div class="empty-state">
                        <i class="bx bx-bell-off"></i>
                        <h6>No notifications</h6>
                        <p class="mb-0">All clear!</p>
                    </div>
                `);

        $("#notificationCount").text(0);
        return;
    }
    $("#notificationCount").text(notificationDetails.notificationCount);
    const notificationsHtml = notificationsList.map(notification => {
        let href = '#';
        switch (notification.notificationType) {
            case 'Leave Application':
                href = '/EmployeePanel/Leave/LeaveApproval';
                break;
            case 'Leave Approval':
                href = '/EmployeePanel/Leave/LeaveApplication';
                break;
            case 'CompOff Application':
                href = '/EmployeePanel/CompOffApplication/CompOffApproval';
                break;
            case 'CompOff Approval':
                href = '/EmployeePanel/CompOffApplication/Index';
                break;
            case 'Attendance Application':
                href = '/EmployeePanel/Leave/AttendanceRegularizationApproval';
                break;
            case 'Attendance Approval':
                href = '/EmployeePanel/Leave/Attendance';
                break;
            case 'Ticket Application':
                href = '/EmployeePanel/TicketRequest/TicketApplication';
                break;
            case 'Ticket FollowUp':
                href = '/EmployeePanel/TicketRequest/TicketApplication';
                break;
            case 'Ticket Response':
                href = '/EmployeePanel/TicketRequest/TicketRequest';
                break;
            case 'Probation Over':
                href = '/EmployeePanel/ManageProbation/Probation';
                break;

            default:
                href = '#'; // Default fallback
        }
        
        return `
                    <div class="notification-item" onclick='redirectPage("${uiBaseUrlLayout}${href}", "${notification.notificationType}")'>
                    <span class="notification-item-count">${notification.notificationTypeCount}</span>
                        <div class="notification-content">
                            <div class="notification-title">${notification.notificationType}</div>
                        </div>
                        
                    </div>
                `;
    }).join('');

    $("#notificationList").html(notificationsHtml);

}










