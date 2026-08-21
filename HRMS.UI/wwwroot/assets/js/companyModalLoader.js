// Public function to load modal partial and company data
function openCompanyModal(partialUrl, apiUrl, CompanyList, callback) {
    if (!partialUrl || typeof partialUrl !== 'string') {
        if (typeof callback === 'function') callback(null);
        return;
    }
    $.ajax({
        url: partialUrl + '/AdminPanel/PartialView/LoadCompanyModal',
        type: 'GET',
        success: function (html) {
            $('#companyModalContainer').html(html);
            // Plain flex, not .fadeIn() — jQuery's fade helpers default to
            // display:block, which would break the centered flex layout.
            // The fade/pop animation itself is already handled by the
            // company-modal.css keyframes, which replay automatically
            // whenever display flips from none to flex.
            $('#companyModal').css('display', 'flex');
            loadCompanyDetails(CompanyList, callback, apiUrl);
            bindCompanyModalEvents(callback);
        },
        error: function () {
            if (typeof callback === 'function') callback(null);
        }
    });
}

// Load company data and populate boxes
function loadCompanyDetails(CompanyList, callback, apiUrl) {
    $('#companyLoader').css('display', 'flex');

    // Get the container element
    const container = $('.company-box-container');
    container.css('display', 'none').empty(); // Clear existing content

    // Check if CompanyList exists and has data
    if (!CompanyList || CompanyList.length === 0) {
        $('#companyLoader').css('display', 'none');
        container.html('<div class="company-box-empty"><i class="bx bx-buildings" style="font-size:28px;display:block;margin-bottom:8px;"></i>No companies found for this account.</div>').css('display', 'flex');
        return;
    }

    // CompanyList here is the JWT's embedded snapshot from login time — if a
    // company was renamed since then, this box would otherwise keep showing
    // the old name until the user logs out and back in. Refresh just the
    // display name live before rendering; which companies/IDs appear is still
    // entirely determined by the JWT snapshot, so permission scoping is
    // untouched — only the label can change.
    var nameRequests = apiUrl
        ? $.map(CompanyList, function (company) {
            return $.ajax({ url: apiUrl + '/CompanyDetailsAPI/GetByCompanyId/' + company.CompanyId, type: 'GET' })
                .then(function (res) {
                    if (res && res.isSuccess && res.data && res.data.companyName) {
                        company.CompanyName = res.data.companyName;
                    }
                })
                .catch(function () { /* keep the JWT-snapshot name on failure */ });
        })
        : [];

    $.when.apply($, nameRequests).always(function () {
        renderCompanyBoxes(CompanyList, callback, container);
    });
}

// A small fixed palette (not random) so the same company always gets the
// same avatar color across renders/logins, cycled by name hash.
var _companyAvatarColors = ['#3E6A8C', '#2E7D6B', '#8A5A44', '#5B5FA6', '#B0793A', '#4B7F52'];

function _companyAvatarColor(name) {
    var hash = 0;
    for (var i = 0; i < (name || '').length; i++) { hash = (hash * 31 + name.charCodeAt(i)) >>> 0; }
    return _companyAvatarColors[hash % _companyAvatarColors.length];
}

function renderCompanyBoxes(CompanyList, callback, container) {
    $.each(CompanyList, function (index, company) {
        var name = company.CompanyName || 'Unknown Company';
        var initial = name.trim().charAt(0).toUpperCase() || '?';
        var box = $(`
            <div class="company-box" title="${name}">
                <span class="company-box-avatar" style="background:${_companyAvatarColor(name)}">${initial}</span>
                <span class="company-box-name">${name}</span>
                <i class="bx bx-chevron-right company-box-arrow"></i>
            </div>
        `);

        // Store company data using jQuery's data method
        box.data('company', company);
        container.append(box);
    });

    // Bind click events for company selection
    $('.company-box').off('click').on('click', function () {
        const companyData = $(this).data('company');

        // Hide modal and execute callback
        $('#companyModal').css('display', 'none');
        if (typeof callback === 'function') {
            callback(companyData);
        }
    });

    $('#companyLoader').css('display', 'none');
    container.css('display', 'flex');
}

// Handle close button and prevent accidental modal close
function bindCompanyModalEvents(callback) {
    // Handle close button
    $('.close-company').off('click').on('click', function () {
        $('#companyModal').css('display', 'none');
        if (typeof callback === 'function') callback(null);
    });

    // Prevent closing modal by clicking backdrop
    $('#companyModal').off('click').on('click', function (e) {
        // Only close if clicking outside modal content
        if ($(e.target).is('#companyModal')) {
            $('#companyModal').css('display', 'none');
            if (typeof callback === 'function') callback(null);
        }
    });
}