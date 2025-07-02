
let Emp_Id = localStorage.getItem("EmployeeId");

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