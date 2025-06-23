
$('#btnLogout').click(function () {
    var url = uiBaseUrlLayout +'/AuthManage/Logout';
    
        $.ajax({
            url: uiBaseUrlLayout +'/AuthManage/Logout', // Update this to match your API route
            type: 'POST',
            //headers: {
            //    'Authorization': 'Bearer ' + localStorage.getItem('token') // if you're using JWT auth
            //},
            success: function (response) {
              // Redirect to login or home
                window.location.href = uiBaseUrlLayout+'/AuthManage/Login';
            },
            error: function (xhr) {
                alert("Logout failed!");
            }
        });
});


$(document).ready(function () {

    debugger
    const savedCompany = localStorage.getItem('selectedCompany');
    var company = JSON.parse(savedCompany);
    $('.companyNameLayout').text(company.companyName);

    document.querySelectorAll('.custom-tooltip').forEach((el) => {
        el.setAttribute('data-tooltip', el.textContent);
    });


    if (!savedCompany) {
        // Simulate click on logout button
        const logoutBtn = document.getElementById('btnLogout');
        if (logoutBtn) {
            logoutBtn.click();
        } else {
            // Fallback if button not present
            $.ajax({
                url: uiBaseUrlLayout + '/AuthManage/Logout', // Update this to match your API route
                type: 'POST',
                //headers: {
                //    'Authorization': 'Bearer ' + localStorage.getItem('token') // if you're using JWT auth
                //},
                success: function (response) {
                    // Redirect to login or home
                    window.location.href = uiBaseUrlLayout + '/AuthManage/Login';
                },
                error: function (xhr) {
                    alert("Logout failed!");
                }
            });
        }
    } else {
        const companyDetails = JSON.parse(savedCompany);

    }

    $('#companyDrop').on('click', function () {
        openCompanyModal(uiBaseUrlLayout, BaseUrlLayout, function (selectedCompany) {
            if (selectedCompany) {
                console.log("Selected company:", selectedCompany);
                localStorage.removeItem('selectedCompany');
                localStorage.setItem('selectedCompany', JSON.stringify(selectedCompany));
                location.reload();
            } else {
                console.log(" No company selected or user canceled.");
            }
        });
    });
});


