

(function (jQuery, window, document) {
    var $ = jQuery; // Use jQuery alias for $
    var bStarted = false;
    $(function () {
        appStart();
        initDropDown();
    });
    try {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (evt, args) {
            appStart();
            initDropDown();
        });
    } catch (e) {
        console.log(e.toString());
    }
    function appStart() {
        if (bStarted)
            return;


        $('li.drop-down-menu a').hover(function (e) {
            $('select').each(function (e) {
                if ($(this).is(':focus'))
                    $(this).blur();
            });
        });
        bStarted = true;
    }
    try {
        appStart();
    } catch (e) {
    }


    /*DropDown Control*/
    function initDropDown() {
        var dropdown_array = []
        function controlBindControl(dropdown) {
            dropdown
                .removeClass('input')
                .removeClass('input_ddl')
                .removeClass('active-control');
            if (dropdown.width() > 100 && !dropdown.attr('data-width-reset')) {
                switch (window.browserType) {
                    case 'safari':
                        dropdown.width(dropdown.width() + 27);
                        break;
                    default:
                        dropdown.width(dropdown.width() + 12);
                        break;
                }
                dropdown.attr('data-width-reset', true);
            }
            dropdown_array.push(dropdown);
            //            dropdown.change(function (e) {
            //                $(this).select2().select2('val', $(this).val());
            //            });
            //            dropdown.select2({
            //                minimumResultsForSearch: 20
            //            });
            //dropdown.select2().select2('val', dropdown.val());

        }
        var timeoutID = -1;
        $('select.select2').each(function () {
            //if ($(this).find('option').length > 50) {
            controlBindControl($(this));
            //} else {
            $(this).bind("DOMSubtreeModified", function (e) {
                var ddl = $(this);
                if (ddl.find('option').length > 50 && timeoutID == -1) {
                    timeoutID = setTimeout(function () {
                        controlBindControl(ddl);
                    }, 600);
                }

            });
            //}
        });

        setInterval(function () {

        }, 500);
    }
})(window.jQuery, window, document);