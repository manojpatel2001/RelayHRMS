
(function () {
    const tokenExist = localStorage.getItem("authToken");

    if (!tokenExist) {
        localStorage.clear();
        window.location.href = uiBaseUrlLayout+"/AuthManage/Login";
    }
    
})();

$(document).ready(async function () {
    var CompnayList = [];
    const tokenLogout = localStorage.getItem("authToken");
    let decodedLogout = "";
    let logoutEmail = "";
    let logoutPassword = "";
    async function callRefreshTokenAPI() {
        try {
            var response = await fetch(BaseUrlLayout + '/AuthenticationAPI/Login', {
                method: 'POST',
                headers: {
                    'Authorization': 'Bearer ' + tokenLogout,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ Email: logoutEmail, Password: logoutPassword })
            })
            const refressData = await response.json();

            if (refressData.isSuccess) {
                localStorage.removeItem('authToken');
                localStorage.setItem("authToken", refressData.data.token);
            }
            
        } catch (error) {
            console.error('Fetch error:', error);
        }
    }

    
    function checkTokenExpiry() {
        const token = localStorage.getItem("authToken");
        if (!token) return;

        const decoded = jwt_decode(token);
        const exp = decoded.exp * 1000; // Convert to milliseconds
        const now = Date.now();
        const timeLeft = exp - now;

        if (timeLeft <= 5 * 60 * 1000 && timeLeft > 0) {
            callRefreshTokenAPI(); // Your refresh token logic
        }
    }

    // Run every minute
    setInterval(checkTokenExpiry, 60 * 1000);

        
        if (tokenLogout)
        {
            $(".body-hiden-wrapper").show();
            decodedLogout = jwt_decode(tokenLogout);
            CompnayList = JSON.parse(decodedLogout.Company);
           
            if (!JSON.parse(localStorage.getItem("EmployeeId"))) {
                $(".body-hiden-wrapper").hide();
                window.location.href = uiBaseUrlLayout + '/AuthManage/Login';
            }




            const savedCompany = localStorage.getItem('selectedCompany');
            var company = JSON.parse(savedCompany);
            var FullName = decodedLogout.FullName;
            var Designation = decodedLogout.Designation;
            var ProfileUrl = decodedLogout.ProfileUrl;
            logoutEmail = decodedLogout.email;
            logoutPassword = decodedLogout.Password;
           
            $('.companyNameLayout').text(company.CompanyName);

            if (ProfileUrl != "") {
                $('.user-img').attr('src', ProfileUrl);
            }
            else {
                $('.user-img').attr('src', BaseDomainUrl + '/default-image/avatar-2.png');
            }
            $(".user-name").text(FullName);
            $(".designation").text(Designation);


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
                        localStorage.removeItem('selectedCompany');
                        localStorage.setItem('selectedCompany', JSON.stringify(selectedCompany));
                        location.reload();
                    } else {
                    }
                });
            });

        }
        else {
            $(".body-hiden-wrapper").hide();
            localStorage.clear();
            window.location.href = uiBaseUrlLayout + '/AuthManage/Login';
        }
        $('#btnLogout').click(function () {
            localStorage.clear();
            window.location.href = uiBaseUrlLayout+'/AuthManage/Login';
            
        });


     
    $(document).ready(async function () {

        $.ajax({
            url: uiBaseUrlLayout + '/AdminPanel/PartialView/LoadSessionTimmerModal',
            type: 'GET',
            success: function (html) {
                $('#sessionTimmerContainer').html(html);
                
            },
            error: function () {
                
            }
        });
    });

       
        
});

