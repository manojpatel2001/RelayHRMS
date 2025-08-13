//[
//    "add-esscomp-offapplication",
//    "add-esscomp-offapproval",
//    "add-essemployeedirectory",
//    "add-essemployeeinoutrecord",
//    "add-essitdeclarationform",
//    "add-essleaveapplication",
//    "add-essleaveapprovalpage",
//    "add-essmyinout",
//    "add-essmyprofile",
//    "add-essprecomp-offapplication",
//    "add-essprecomp-offapproval",
//    "add-essshiftchange",
//    "add-essticketclose",
//    "add-essticketopen",
//    "delete-esscomp-offapplication",
//    "delete-esscomp-offapproval",
//    "delete-essemployeedirectory",
//    "delete-essemployeeinoutrecord",
//    "delete-essitdeclarationform",
//    "delete-essleaveapplication",
//    "delete-essleaveapprovalpage",
//    "delete-essmyinout",
//    "delete-essmyprofile",
//    "delete-essprecomp-offapplication",
//    "delete-essprecomp-offapproval",
//    "delete-essshiftchange",
//    "delete-essticketclose",
//    "delete-essticketopen",
//    "edit-esscomp-offapplication",
//    "edit-esscomp-offapproval",
//    "edit-essemployeedirectory",
//    "edit-essemployeeinoutrecord",
//    "edit-essitdeclarationform",
//    "edit-essleaveapplication",
//    "edit-essleaveapprovalpage",
//    "edit-essmyinout",
//    "edit-essmyprofile",
//    "edit-essprecomp-offapplication",
//    "edit-essprecomp-offapproval",
//    "edit-essshiftchange",
//    "edit-essticketclose",
//    "edit-essticketopen",
//    "view-esschangepassword",
//    "view-esscomp-offapplication",
//    "view-esscomp-offapproval",
//    "view-esscomp-offmenu",
//    "view-essemployeedirectory",
//    "view-essemployeeinoutrecord",
//    "view-essitdeclarationform",
//    "view-essleaveapplication",
//    "view-essleaveapprovalmenu",
//    "view-essleaveapprovalpage",
//    "view-essleavemenu",
//    "view-essmemberdetails",
//    "view-essmembershift",
//    "view-essmyinout",
//    "view-essmyreports",
//    "view-essmyteammenu",
//    "view-essprecomp-offapplication",
//    "view-essprecomp-offapproval",
//    "view-essprobationselfrating",
//    "view-esssalarydetailsmenu",
//    "view-essshiftchange",
//    "view-essswitch",
//    "view-essticketclose",
//    "view-essticketopen",
//    "view-essticketrequest",
//    "view-myreport",
//    "view-rolepermissionmenu"
//]

(async function () {
    async function loadUserPermision() {
       
        try {
            const response = await fetch(BaseUrlLayout + '/RolePermissionAPI/GetAllUserAndRolePermissionList/' + UserId, {
                method: 'GET',
                headers: {
                    'Authorization': 'Bearer ' + localStorage.getItem("authToken"),
                    'Content-Type': 'application/json'
                }
            });

            const dataPermission = await response.json();

            if (dataPermission.isSuccess) {
                /*console.log(dataPermission);*/
                permissions = dataPermission.data;
            }
            else {
                permissions = [];
            }
        } catch (error) {
            console.error('Fetch error:', error);
        }
    }

    let tokenPermission = localStorage.getItem("authToken");
    var decodedToken = "";
    let UserId = 0;
    let permissions = [];
  
    if (tokenPermission) {
        $(".body-hiden-wrapper").show();
        const decodedToken = jwt_decode(tokenPermission);
        const roleSlug = decodedToken.RoleSlug;
         UserId = parseInt(decodedToken.Id);
        if (roleSlug == "super-admin")
        {
            //$("#dropdownESSSwitch").show();
            //$("#dropdownAdminSwitch").show();
            $("#drpManageRoleAndPermission").show();
            $("#drpCompanyInformationMenu").show();
            $("#AddCompanyDetails").show();
            $("#companyInfo").show();
        }

        if (roleSlug == "ess" || roleSlug == "admin")
        {
            
            await loadUserPermision();
            /*Ess switch*/
            if (permissions.includes("view-essswitch")) {
                $("#dropdownESSSwitch").show();
            }
            if (permissions.includes("block-essswitch")) {
                $("#dropdownESSSwitch").hide();
            }

            /*Admin switch*/
            if (permissions.includes("view-adminswitch")) {
                $("#dropdownAdminSwitch").show();
            }
            if (permissions.includes("block-adminswitch")) {
                $("#dropdownAdminSwitch").hide();
            }


            // EmployeeLayout Permission

            if (permissions.includes("view-essmemberdetails"))
            {
                $(".essSalaryDetailsMenu").show();
            }
            if (permissions.includes("block-essmemberdetails"))
            {
                $(".essSalaryDetailsMenu").hide();
            }

            if (permissions.includes("view-essitdeclarationform")) {
                $(".essItDeclarationForm").show();
            }
            if (permissions.includes("block-view-essitdeclarationform")) {
                $(".essItDeclarationForm").hide();
            }




        }


    }
    else {
        $(".body-hiden-wrapper").hide();
        localStorage.clear();
        window.location.href = uiBaseUrlLayout + '/AuthManage/Login';
    }


})();