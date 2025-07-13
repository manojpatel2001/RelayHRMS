
var token = localStorage.getItem("authToken");
var decodedToken = "";
if (token) {
    $(".body-hiden-wrapper").show();
    var decodedToken = jwt_decode(token);
    console.log("decodeToken", decodedToken)
    var permissions = decodedToken.Permission;

    /*Super Admin */
    /*Change company Option*/
    if (permissions.includes("all-admin")) {
        //$("#dropdownESSSwitch").show();
        //$("#dropdownAdminSwitch").show();
        $("#drpManageRoleAndPermission").show();
        $("#drpCompanyInformationMenu").show();

    }
    

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
else {
    $(".body-hiden-wrapper").hide();
    window.location.href = uiBaseUrlLayout + '/AuthManage/Login';
}
