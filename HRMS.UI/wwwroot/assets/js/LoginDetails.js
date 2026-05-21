
let Emp_Id = localStorage.getItem("EmployeeId");

$.ajax({
	type: "GET",
	url: BaseUrlLayout + "/EmployeeMasterAPI/GetEmployeeById/"+Emp_Id,
	success: function (data) {
		if (data.isSuccess)
		{
			setEmployeeLoginDetails(data.data)
		}
		else {
			
		}
	},
	error: function (xhr, status, error) {
	}
})

function setEmployeeLoginDetails(data) {
	if (data.employeeProfileUrl != null) {
		$('.profileImage').attr('src', data.employeeProfileUrl);
	}
	else {
		$('.profileImage').attr('src', BaseDomainUrl+'/default-image/avatar-2.png');
	}
	$(".user-name").text(data.fullName);
	//$(".designattion").text(data.userPrivilege);
	
}  

async function callRefreshTokenAPI() {
    try {
        var response = await fetch(BaseUrlLayout + '/AuthenticationAPI/Login', {
            method: 'POST',
            headers: {
                'Authorization': 'Bearer ' + tokenLogout,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ Email: logoutEmail, Password: logoutPassword })
        });
        const refressData = await response.json();

        if (refressData.isSuccess) {
            // ✅ Save original LoginHistoryID before replacing token
            const originalLoginHistoryID = localStorage.getItem('LoginHistoryID');

            localStorage.removeItem('authToken');
            localStorage.setItem("authToken", refressData.data.token);

            // ✅ Restore original ID — do NOT overwrite with new refresh ID
            if (originalLoginHistoryID) {
                localStorage.setItem('LoginHistoryID', originalLoginHistoryID);
            }
        }

    } catch (error) {
        console.error('Fetch error:', error);
    }
}