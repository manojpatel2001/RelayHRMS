
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
