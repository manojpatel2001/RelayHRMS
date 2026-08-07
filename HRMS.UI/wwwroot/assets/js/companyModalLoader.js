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
            $('#companyModal').fadeIn();
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
    $('#companyLoader').show();

    // Get the container element
    const container = $('.company-box-container');
    container.hide().empty(); // Clear existing content

    // Check if CompanyList exists and has data
    if (!CompanyList || CompanyList.length === 0) {
        $('#companyLoader').hide();
        container.show();
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

function renderCompanyBoxes(CompanyList, callback, container) {
    $.each(CompanyList, function (index, company) {
        const box = $(`
            <div class="company-box">
                ${company.CompanyName || 'Unknown Company'}
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
        $('#companyModal').fadeOut();
        if (typeof callback === 'function') {
            callback(companyData);
        }
    });

    $('#companyLoader').hide();
    container.show();
}

// Handle close button and prevent accidental modal close
function bindCompanyModalEvents(callback) {
    // Handle close button
    $('.close-company').off('click').on('click', function () {
        $('#companyModal').fadeOut();
        if (typeof callback === 'function') callback(null);
    });

    // Prevent closing modal by clicking backdrop
    $('#companyModal').off('click').on('click', function (e) {
        // Only close if clicking outside modal content
        if ($(e.target).is('#companyModal')) {
            $('#companyModal').fadeOut();
            if (typeof callback === 'function') callback(null);
        }
    });
}