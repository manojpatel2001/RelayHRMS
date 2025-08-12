

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
                console.log(dataPermission);
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
            //$("#AddCompanyDetails").show();
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

        }


    }
    else {
        $(".body-hiden-wrapper").hide();
        localStorage.clear();
        window.location.href = uiBaseUrlLayout + '/AuthManage/Login';
    }


})();