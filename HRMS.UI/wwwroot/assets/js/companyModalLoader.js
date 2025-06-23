


// Public function to load modal partial and company data
function openCompanyModal(partialUrl, apiUrl, callback) {
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

            loadCompanyDetails(apiUrl, callback);
            bindCompanyModalEvents(callback);
        },
        error: function () {
            console.error('❌ Failed to load company modal partial from: ' + partialUrl);
            if (typeof callback === 'function') callback(null);
        }
    });
}

// Load company data and populate boxes
function loadCompanyDetails(apiUrl, callback) {
    if (!apiUrl || typeof apiUrl !== 'string') {
        console.warn("Invalid API base URL.");
        if (typeof callback === 'function') callback(null);
        return;
    }

    $('#companyLoader').show();
    $('.company-box-container').hide();

    $.ajax({
        url: apiUrl + '/CompanyDetailsAPI/GetAllCompanyDetailsList',
        type: 'GET',
        success: function (response) {
            $('#companyLoader').hide();
            $('.company-box-container').show();

            if (response.isSuccess) {
                const container = $('.company-box-container');
                container.empty();

                $.each(response.data, function (index, company) {
                    const box = $(`
                        <div class="company-box"  data-company='${JSON.stringify(company)}'>
                            ${company.companyName}
                        </div>
                    `);
                    container.append(box);
                });

                // Handle selection
                $('.company-box').off('click').on('click', function () {
                    const companyData = $(this).data('company');
                    console.log("Company clicked:", companyData);
                    $('#companyModal').fadeOut();
                    if (typeof callback === 'function') callback(companyData);
                });
            } else {
                console.warn("Company list loaded but returned unsuccessful response.");
                if (typeof callback === 'function') callback(null);
            }
        },
        error: function (err) {
            console.error('❌ Error loading company details:', err);
            $('#companyLoader').hide();
            $('.company-box-container').show();
            if (typeof callback === 'function') callback(null);
        }
    });
}

// Handle close button and prevent accidental modal close
function bindCompanyModalEvents(callback) {
    $('.close-company').off('click').on('click', function () {
        console.log("Cancel clicked");
        $('#companyModal').fadeOut();
        if (typeof callback === 'function') callback(null);
    });

    // Prevent closing modal by clicking backdrop
    $('#companyModal').off('click').on('click', function (e) {
        if ($(e.target).closest('.modal-content').length === 0) {
            e.stopPropagation();
        }
    });
}
