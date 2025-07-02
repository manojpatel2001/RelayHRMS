// Public function to load modal partial and company data
function openCompanyModal(partialUrl, apiUrl, CompanyList, callback) {
    if (!partialUrl || typeof partialUrl !== 'string') {
        console.warn("Invalid partial view base URL.");
        if (typeof callback === 'function') callback(null);
        return;
    }
    $.ajax({
        url: partialUrl + '/PartialView/LoadCompanyModal',
        type: 'GET',
        success: function (html) {
            $('#companyModalContainer').html(html);
            $('#companyModal').fadeIn();
            loadCompanyDetails(CompanyList, callback);
            bindCompanyModalEvents(callback);
        },
        error: function () {
            console.error('❌ Failed to load company modal partial from: ' + partialUrl);
            if (typeof callback === 'function') callback(null);
        }
    });
}

// Load company data and populate boxes
function loadCompanyDetails(CompanyList, callback) {
    debugger
    $('#companyLoader').show();

    // Get the container element
    const container = $('.company-box-container');
    container.hide().empty(); // Clear existing content

    // Check if CompanyList exists and has data
    if (!CompanyList || CompanyList.length === 0) {
        console.warn("No company data provided");
        $('#companyLoader').hide();
        container.show();
        return;
    }

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
        debugger
        const companyData = $(this).data('company');
        console.log("Company selected:", companyData);

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
        console.log("Cancel clicked");
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