const queryString1 = window.location.search;
// Create a URLSearchParams object
const urlParams1 = new URLSearchParams(queryString1);
let Emp_Id = urlParams1.get('EmployeeId');

$.ajax({
	type: "GET",
	url: BaseUrlLayout + "/EmployeeMasterAPI/GetEmployeeById/"+Emp_Id,
	success: function (data) {
		console.log("Data received:", data);
		debugger
		if (data.isSuccess)
		{
			setEmployeeLoginDetails(data.data)
		}
		else {
			
		}
	},
	error: function (xhr, status, error) {
		console.error("AJAX Error: " + status + " - " + error);
	}
})

function setEmployeeLoginDetails(data) {
	debugger;
	if (data.employeeProfileUrl != null) {
		$('.user-img').attr('src', data.employeeProfileUrl);
	}
	else {
		$('.user-img').attr('src', BaseDomainUrl+'/default-image/avatar-2.png');
	}
	$(".user-name").text(data.fullName);
	//$(".designattion").text(data.userPrivilege);
}