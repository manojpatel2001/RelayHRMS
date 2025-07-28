
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
		$('.user-img').attr('src', data.employeeProfileUrl);
	}
	else {
		$('.user-img').attr('src', BaseDomainUrl+'/default-image/avatar-2.png');
	}
	$(".user-name").text(data.fullName);
	//$(".designattion").text(data.userPrivilege);
}