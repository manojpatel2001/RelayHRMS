var CompnayList = [];
var token = localStorage.getItem("authToken");
var decoded = "";
if (token) {
    $(".body-hiden-wrapper").show();
    var decoded = jwt_decode(token);
    CompnayList = JSON.parse(decoded.Company);
    if (decoded.RoleSlug == "ess")
    {
        if (!JSON.parse(localStorage.getItem("EmployeeId"))) {
            $(".body-hiden-wrapper").hide();
            window.location.href = uiBaseUrlLayout + '/AuthManage/Login';
        }
    }

}
else {
    $(".body-hiden-wrapper").hide();
    window.location.href = uiBaseUrlLayout + '/AuthManage/Login';
}
$('#btnLogout').click(function () {
    localStorage.removeItem("authToken");
    window.location.href = uiBaseUrlLayout+'/AuthManage/Login';
            
});


$(document).ready(function () {

    const savedCompany = localStorage.getItem('selectedCompany');
    var company = JSON.parse(savedCompany);
    var FullName = decoded.FullName;
    var Designation = decoded.Designation;
    var ProfileUrl = decoded.ProfileUrl;
    $('.companyNameLayout').text(company.CompanyName);

    if (ProfileUrl!="") {
        $('.user-img').attr('src', ProfileUrl);
    }
    else {
        $('.user-img').attr('src', BaseDomainUrl + '/default-image/avatar-2.png');
    }
    $(".user-name").text(FullName);
    $(".designattion").text(Designation);


    document.querySelectorAll('.custom-tooltip').forEach((el) => {
        el.setAttribute('data-tooltip', el.textContent);
    });


    if (!savedCompany) {
        // Simulate click on logout button
        localStorage.removeItem("authToken");
        window.location.href = uiBaseUrlLayout + '/AuthManage/Login';
       
    } else {
        const companyDetails = JSON.parse(savedCompany);

    }

    $('#companyDrop').on('click', function () {
        openCompanyModal(uiBaseUrlLayout, BaseUrlLayout, CompnayList, function (selectedCompany) {
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


