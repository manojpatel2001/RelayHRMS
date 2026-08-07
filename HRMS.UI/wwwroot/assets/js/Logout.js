(function () {
    const tokenExist = localStorage.getItem("authToken");
    if (!tokenExist) {
        localStorage.clear();
        window.location.href = uiBaseUrlLayout + "/AuthManage/Login";
    }
})();

$(document).ready(async function () {
    var CompnayList = [];
    const tokenLogout = localStorage.getItem("authToken");
    let decodedLogout = "";
    let logoutEmail = "";
    let logoutPassword = "";

    // ✅ FIX 1 — Login ke time LoginHistoryID save karo
    if (tokenLogout) {
        try {
            const decodedTemp = jwt_decode(tokenLogout);
            // Sirf pehli baar save karo (agar pehle se nahi hai)
            if (!localStorage.getItem('LoginHistoryID')) {
                localStorage.setItem('LoginHistoryID', decodedTemp.LoginHistoryID);
                console.log('LoginHistoryID saved:', decodedTemp.LoginHistoryID);
            }
        } catch (e) {
            console.error('Token decode error on init:', e);
        }
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
                // ✅ FIX 2 — Refresh ke time original ID bachao
                const originalLoginHistoryID = localStorage.getItem('LoginHistoryID');

                localStorage.removeItem('authToken');
                localStorage.setItem("authToken", refressData.data.token);

                // ✅ Original ID wapas rakho — overwrite mat karo
                if (originalLoginHistoryID) {
                    localStorage.setItem('LoginHistoryID', originalLoginHistoryID);
                    console.log('LoginHistoryID preserved after refresh:', originalLoginHistoryID);
                }
            }
        } catch (error) {
            console.error('Fetch error:', error);
        }
    }

    function checkTokenExpiry() {
        const token = localStorage.getItem("authToken");
        if (!token) return;

        const decoded = jwt_decode(token);
        const exp = decoded.exp * 1000;
        const now = Date.now();
        const timeLeft = exp - now;

        if (timeLeft <= 5 * 60 * 1000 && timeLeft > 0) {
            callRefreshTokenAPI();
        }
    }

    setInterval(checkTokenExpiry, 60 * 1000);

    if (tokenLogout) {
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
        $(".user-name").text(FullName);
        $(".designation").text(Designation);

        // `company` above is the JWT/localStorage snapshot from login time — if
        // the company was renamed since then, refresh the badge (and the cached
        // copy, so the next page load is already correct) with the live name.
        if (company && company.CompanyId) {
            $.ajax({
                url: BaseUrlLayout + '/CompanyDetailsAPI/GetByCompanyId/' + company.CompanyId,
                type: 'GET',
                success: function (res) {
                    if (res && res.isSuccess && res.data && res.data.companyName && res.data.companyName !== company.CompanyName) {
                        company.CompanyName = res.data.companyName;
                        $('.companyNameLayout').text(company.CompanyName);
                        localStorage.setItem('selectedCompany', JSON.stringify(company));
                    }
                }
            });
        }

        document.querySelectorAll('.custom-tooltip').forEach((el) => {
            el.setAttribute('data-tooltip', el.textContent);
        });

        if (!savedCompany) {
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
                }
            });
        });

    } else {
        $(".body-hiden-wrapper").hide();
        localStorage.clear();
        window.location.href = uiBaseUrlLayout + '/AuthManage/Login';
    }

    // ✅ FIX 3 — Logout button
    $('#btnLogout').click(async function () {
        try {
            const token = localStorage.getItem("authToken");

            // ✅ Pehle localStorage se lo
            let historyIdToSend = localStorage.getItem('LoginHistoryID');

            // ✅ Agar nahi mila to token se decode karo
            if (!historyIdToSend && token) {
                try {
                    const decoded = jwt_decode(token);
                    historyIdToSend = decoded.LoginHistoryID || '';
                } catch (e) {
                    console.error('Token decode error:', e);
                }
            }

            console.log('=== LOGOUT ===');
            console.log('LoginHistoryID sending:', historyIdToSend);

            await fetch(BaseUrlLayout + '/AuthenticationAPI/Logout', {
                method: 'POST',
                headers: {
                    'Authorization': 'Bearer ' + token,
                    'Content-Type': 'application/json',
                    'X-LoginHistoryID': historyIdToSend || ''
                }
            });

        } catch (error) {
            console.error('Logout error:', error);
        } finally {
            localStorage.clear();
            window.location.href = uiBaseUrlLayout + '/AuthManage/Login';
        }
    });

    $(document).ready(async function () {
        $.ajax({
            url: uiBaseUrlLayout + '/AdminPanel/PartialView/LoadSessionTimmerModal',
            type: 'GET',
            success: function (html) {
                $('#sessionTimmerContainer').html(html);
            },
            error: function () { }
        });
    });
});