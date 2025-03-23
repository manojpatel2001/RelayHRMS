var MsgTimeoutInterval = 20000;
function CheckAll(selectid, hck, callOnChange) {

    //Setting default value
    callOnChange = typeof callOnChange != 'undefined' ? callOnChange : false;

    isChk = hck.checked;
    //Updated by Nimesh 30-May-2015 (faster way to check all checked boxes using JQuery)
    setTimeout(function () {
        var table = $(hck).closest('table');
        checkboxes = findCheckBoxes(selectid, table);
        if (checkboxes.length == 1 || checkboxes.length == 0) {
            table = $(table).parent().closest('table')
            checkboxes = findChkTable(hck.id, selectid, table);
        }
        if (!callOnChange) {
            $(checkboxes).prop('checked', isChk);
        }
        else {
            $(checkboxes).prop('checked', isChk).change();
        }
        var totalSelectedItems = isChk ? checkboxes.length : 0;
        displayCounter(table, totalSelectedItems)
    }, 0);
    /*Commented by Nimesh
    for (j = 0; j < document.forms[0].length; j++) {
    var ocheckbox = document.forms[0][j];

    if (ocheckbox.type == 'checkbox') {

    var id = ocheckbox.id.substr(ocheckbox.id.lastIndexOf('_') + 1, selectid.length + 1);
    hck.checked = isChk;
    if (id == selectid && ocheckbox.disabled == false) {

    ocheckbox.checked = !hck.checked;
    ocheckbox.click()
    }
    hck.checked = isChk;
    }
    }*/
}
function findCheckBoxes(selectid, table) {
    //return $(table).find('td input:checkbox:not(:hidden):not(:disabled)[id*=' + selectid + ']'); -- Comment hidden by Nilesh patel on 06082019 -- For new Design 
    return $(table).find('td input:checkbox:not(:disabled)[id*=' + selectid + ']');
}
function findChkTable(chkID, selectid, table) {
    if ($(table).find('td input:checkbox').length > 1) {
        return findCheckBoxes(selectid, table);
    }
    else {
        if ($(table).find('table').length > 0) {
            tables = $(table).find('table');
            chks = null;
            $(tables).each(function (index) {
                if (!chks)
                    chks = findChkTable(chkID, selectid, this)
            });
            if (!chks)
                return chks;
        }
        return null;
    }
}
function ApplyStyle(me, selctedclass, clas, checkBoxHeaderId, No_items, btnids) { var td = me.parentNode; if (td == null) return; var tr = td.parentNode; if (me.checked) { tr.className = selctedclass } else { tr.className = clas } checked = 0; for (i = 0; i < document.forms[0].length; i++) { var o = document.forms[0][i]; if (o.type == 'checkbox') { var id = o.id.substr(o.id.lastIndexOf('_') + 1, o.id.length + 1); if (id == me.id.substr(o.id.lastIndexOf('_') + 1, o.id.length + 1)) { if (o.checked) { checked++ } } } } if (checked == No_items) document.getElementById(checkBoxHeaderId).checked = true; else document.getElementById(checkBoxHeaderId).checked = false }

function displayCounter(table, totalCount) {
    var b = [],
        parent = table.parent().closest('table');
    while (parent.length > 0) {
        b = parent.find('b:contains(Records found)');
        if (b.length == 0)
            b = parent.find('b[selected-counter]');
        parent = parent.parent().closest('table');
        if (b.length > 0)
            break;
    }
    if (b.length == 0)
        return;


    b.attr('selected-counter', 'true');
    var outOf = b.text().trim().split(' ')[0];
    if (b.attr('data-out-of'))
        outOf = b.attr('data-out-of');
    b.attr('data-out-of', outOf)

    var span = b.find('span.selected-item-counter');
    if (span.length == 0) {
        b.empty();
        span = $('<span>')
            .addClass('selected-item-counter')
            .appendTo(b);
    }
    if (parseInt(outOf) > 1)
        span.html(outOf + ' Records &nbsp;&nbsp;-&nbsp;&nbsp;' + totalCount.toString() + ' Selected.');
    else
        span.html(outOf + ' Record &nbsp;&nbsp;-&nbsp;&nbsp;' + totalCount.toString() + ' Selected.');
}

(function () { //(function ($) {
    function initDisplaySelectedCount() {
        var checkAll = $('input:checkbox[onchange*=CheckAll]');
        checkAll.each(function (index) {
            var targetId = getTargetID($(this)),
                parent = $(this).closest('table');
            while (parent.length > 0) {
                parent = parent.parent().closest('table');
                if (parent.find('input:checkbox[id*=' + targetId + ']').length > 0)
                    break;
            }

            var selectedCount = $(parent).find('input[id*=' + targetId + ']:checked').length;
            displayCounter(parent, selectedCount);

            parent.find('input:checkbox[id*=' + targetId + ']').each(function (index) {
                $(this).bind('change', function (e) {
                    var idpart = $(this).attr('id').substring($(this).attr('id').lastIndexOf('_') + 1, $(this).attr('id').length);
                    var selectedCount = $(this).closest('table').find('input[id*=' + idpart + ']:checked').length;
                    displayCounter($(this).closest('table'), selectedCount);
                });
            });
        });
    }
    function getTargetID(chk) {
        var parts = chk.attr('onchange').split(/[\s(,/']+/);
        return parts.length > 1 ? parts[1] : '';
    }

    try {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (evt, args) {
            initDisplaySelectedCount();
        });
    } catch (e) {
        console.log(e.message);
    }
    initDisplaySelectedCount();
})(window.jQuery_2_1_4);


function CheckAll_New(selectid, hck) {
    var bln = hck.checked;
    var c = document.getElementsByTagName("table");
    for (var i = 0; i < c.length; i++) {
        if (c[i].id.indexOf("tblrp") > -1) {
            var hidd = c[i].getElementsByTagName("input");
            for (var j = 0; j < hidd.length; j++) {

                if (hidd[j].type == "checkbox" && hidd[j].id.indexOf("chkgvselect") > -1 || hidd[j].id.indexOf("chkselect") > -1) {

                    if (bln == true) {

                        hidd[j].checked = true;
                    }
                    else {
                        hidd[j].checked = false;
                    }
                }
            }
        }
    }
}

function getParameterByName(name) {
    name = name.toLowerCase();
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search.toLowerCase());
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}


function setMsgTimeoutInterval(timout) {
    MsgTimeoutInterval = timout;
}
var msgIconIsLoaded = false;
//showMsg update by chetan 26-10-16
function showMsg(id, icon, message) {
    //timeout = typeof timeout == 'undefined' ? 10000 : timeout;
    //if (timeout == 0)
    //    timeout =
    //    if ($('#' + id).length > 0){
    //        $('#' + id).hide();
    //        message = message  || $('#' + id).text();
    //        if (message && message.length > 0){
    //            showAlert("", message, "sucess");
    //        }
    //    }    
    //    
    //    return;

    $lbl = $('#' + id);
    if ($lbl.length > 0)
        $lbl.hide();
    setTimeout(function () {
        var timeout = MsgTimeoutInterval,

            parent = $lbl.parent(),
            div_Id = 'div_' + id,
            imgID = 'dynamic_msg_icon_' + id;

        if (typeof (message) != 'undefined' && message != '')
            $lbl.text(message);

        if ($lbl.text() == '')
            return;



        var $div = $('#' + div_Id);
        $img = $('#' + imgID);

        if ($('#' + div_Id).length == 0) {
            $lbl.hide();
            $div = $('<div id="' + div_Id + '" ></div>')

            if ($img.length == 0) {

                $img = $('<img id="' + imgID + '" src="' + url + '" style="width:20px;height:20px;position:relative;" />');
                $img.css('left', -5)
                //              $img.css('top', 5);
                $img.css('top', 0); //for new design ess
                $div.width($lbl.width() + 100);
                $lbl.remove()
                $div.hide();

                $(parent).append($div);
                $div.append($img);
                $div.append($lbl);

            }
        } else {
            $div.hide();
        }
        $div.removeClass('err').removeClass('info');

        var url = window.location.href.toLowerCase().substring(0, window.location.href.toLowerCase().lastIndexOf('.aspx'))
        url = url.substring(0, url.lastIndexOf('/'));
        var tempUrl = url.toUpperCase() + '/';   //Change by Jaina 04-07-2017
        if (tempUrl.indexOf('/ADMIN_ASSOCIATES/') > -1 || tempUrl.indexOf('/REPORTS/') > -1 || tempUrl.indexOf('/HRMS/') > -1)   //Change by Jaina 04-07-2017 (IF In domain set hrms. , it not split properly) Image not found.)
            url = '..';

        if (icon == 'error') {
            url += '/image_new/dynamic_err_icon.png';
            $lbl.removeClass('awards_green');
            $lbl.addClass('awards_red');
        }
        else {
            url += '/image_new/dynamic_info_icon.png';
            $lbl.removeClass('awards_red');
            $lbl.addClass('awards_green');
        }

        $img.attr('src', url);

        $div.show();
        $img.show();

        $img.load(function (e) {
            $div.addClass('alert_msg');
            if ($(this).attr('src').indexOf('_err_') > -1)
                $div.addClass('err');
            else
                $div.addClass('info');
            $lbl.show();
            msgIconIsLoaded = true;
        });
        if (msgIconIsLoaded) {
            $div.addClass('alert_msg');
            if (icon == 'error')
                $div.addClass('err');
            else
                $div.addClass('info');
            $lbl.show();
        }
        //    $(window).bind('scroll', function() {
        //        if ($(window).scrollTop() > 50) {
        //            $div.css('position', 'fixed');
        //            $div.css('top', '0px');
        //        } else {
        //            $div.css('position', '');
        //            $div.css('top', '');
        //        }
        //    });

        var position = cumulativeOffset($div[0]);

        if (position.top < $(window).scrollTop()) {
            window.setTimeout(function () {
                $('body,html').animate({
                    scrollTop: 0
                }, 500);
            }, 800);
        } else if (position.top > ($(window).scrollTop() + $(window).height())) {
            window.setTimeout(function () {
                $('body,html').animate({
                    scrollTop: position.top
                }, 500);
            }, 800);
        }

        setTimeout(function () {
            clearTimeout();
        }, timeout);

        function clearTimeout(quick) {
            var step = quick ? 1.0 : 0.1;
            var opacity = parseFloat($div.css('opacity'));
            var handler = setInterval(function () {
                if (opacity <= 0) {
                    window.clearInterval(handler);
                    $div.hide();
                    $div.removeClass('alert_hide');
                    $div.css('opacity', '');
                    return false;
                }
                opacity -= step;
                $div.css('opacity', opacity);
            }, 200);
            $div.addClass('alert_hide');
            window.clearInterval(parentWatch);
        }


        var parentWatch = setInterval(function () {
            var parent = $div.closest('div');
            if (!parent.is(':visible')) {
                clearTimeout(true);
                return false;
            }
        }, 500);

    }, 500);
}
var cumulativeOffset = function (element) {
    var top = 0, left = 0;
    do {
        top += element.offsetTop || 0;
        left += element.offsetLeft || 0;
        element = element.offsetParent;
    } while (element);

    return {
        top: top,
        left: left
    };
};

function showAlert(title, message, icon, okCallback) {
    //    swal.fire({
    //                'title' : title,
    //                'text' : message,
    //                'type' : icon
    //                }).then(result => {
    //                    if (okCallback)
    //                        okCallback(result);
    //                }); 
    alert(message);
}

/*function showAlert(title, message, icon){
    
}*/

//document.onmousedown = disableclick;
//status = "Right Click Disabled";
//function disableclick(event) {
//    if (event.button == 2) {
//        alert(status);
//        return false;
//    }
//}

var buttonPosition = [];
function freezButtons(startupInterval) {
    startupInterval = typeof (startupInterval) == 'undefined' ? 100 : startupInterval;
    setTimeout(function () {
        if (buttonPosition.length == 0) {
            $('.frozenButton').css('position', '');
            $('.frozenButton').each(function (index) {
                var position = cumulativeOffset(this);
                buttonPosition.push({ 'id': this.id, 'top': position.top, 'left': position.left, 'flag': false });
            });
        }



        var $divBottom = $('#frozenButtonPanel');
        if ($divBottom.length == 0)
            $divBottom = $('<div id="frozenButtonPanel" class="frozenButtonPanel"></div>');

        $($('body')[0]).append($divBottom);

        var hasVisibleControl = false;
        setInterval(function () {
            if ($('.frozenButton:visible').length > 0) {
                $(buttonPosition).each(function (index) {
                    if (!this.flag) {
                        setFreez();
                        return false;
                    }
                });
                hasVisibleControl = true;
            } else {
                hasVisibleControl = false;
                $divBottom.hide();
            }
        }, 200);

        $(window).bind('scroll', function () {




            if (hasVisibleControl) {
                setFreez();




            }



        });

        $(document).find('body').bind('resize', function () {
            if (hasVisibleControl)
                setFreez();
        });

        function setFreez() {

            var scrollBottom = ($(window).scrollTop() + $(window).height()) - 40;

            $(buttonPosition).each(function (index) {
                var $button = $('#' + this.id);
                if ($button.is(':visible')) {

                    $button.css('z-index', 999);

                    if (this.top == 0 && $button.css('position') != 'fixed') {
                        var position = cumulativeOffset($button[0]);
                        buttonPosition[index].top = position.top;
                        buttonPosition[index].left = position.left;
                    }
                    var buttonOffsetTop = this.top;
                    // alert(" buttonOffsetTop  " + buttonOffsetTop + " scrollBottom  " + scrollBottom + " scrollTop " + $(window).scrollTop() + " height " +$(window).height());
                    //if (scrollBottom < buttonOffsetTop) commented binal due to issue and not working in new design and added new if condition
                    if (buttonOffsetTop > $(window).scrollTop() + $(window).height()) {
                        if (!$divBottom.is(':visible'))
                            $divBottom.show();
                        $button.css('position', 'fixed');
                        $button.css('bottom', '8px');
                        $button.css('left', this.left.toString() + 'px');
                        buttonPosition[index].flag = true;

                    } else {
                        $button.css('position', '');
                        $button.css('top', '');
                        $button.css('left', '');
                        $divBottom.hide();
                        buttonPosition[index].flag = false;
                    }
                } else {
                    $button.css('position', '');
                    $button.css('top', '');
                    $button.css('left', '');
                    buttonPosition[index].flag = false;
                }
            });


        }

    }, startupInterval);
}


function isDate(date) {
    try {
        var comp = date.split('/');
        var d = parseInt(comp[0], 10);
        var m = parseInt(comp[1], 10);
        var y = parseInt(comp[2], 10);
        var date = new Date(y, m - 1, d);
        if (date.getFullYear() == y && date.getMonth() + 1 == m && date.getDate() == d) {
            return true;
        } else {
            return false;
        }
    }
    catch (e) {
        console.log("RepeaterJS>>isDate: String cannot be converted from " + date + " to Date.");
        return false;
    }
}

function toDate(strDate, format) {
    format = format || "dd/mm/yyyy";
    var strTime = '',
        hrs = 0,
        min = 0;
    if (strDate.indexOf(':') > 0) {
        if (strDate.indexOf(' ') > 0) {
            strTime = strDate.split(' ')[1];
            strDate = strDate.split(' ')[0];
        } else {
            strTime = strDate;
            strDate = '';
        }
        if (!strTime && !strDate)
            return null;
        if ((strTime == '__:__' || strTime.split(':').length < 2) && !strDate)
            return null;
        try {
            var dtTime = new Date(2000, 0, 1, parseInt(strTime.split(':')[0]), parseInt(strTime.split(':')[1]));
            hrs = dtTime.getHours();
            min = dtTime.getMinutes();
        }
        catch (e) {
            console.log(e.toString());
        }
    }
    var date = new Date();
    date = new Date(date.getFullYear(), date.getMonth(), date.getDay(), hrs, min, 0);

    var sep = strDate.replace(/[0-9]/g, "")[0];
    var dateArr = strDate.split(sep);

    var d = parseInt(dateArr[0], 10);
    var m = parseInt(dateArr[1], 10);
    var y = parseInt(dateArr[2], 10);
    date = new Date(y, m - 1, d, date.getHours(), date.getMinutes(), 0);

    if (date.getFullYear() == y && date.getMonth() + 1 == m && date.getDate() == d) {
        return date;
    }
    return null;
}


function formatDate(date, format) {
    if (date == null)
        return "";

    var sep = format.replace(/[a-z0-9]|\s+|\r?\n|\r/gmi, "")[0];
    var dateArr = format.split(sep);

    var arrMMM = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var arrMMMM = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

    var strDate = '';
    var tmpPart = '';
    var part = ''
    for (i = 0; i < 3; i++) {
        part = dateArr[i].toLowerCase();
        switch (part) {
            case 'd': case 'dd':
                tmpPart = ("0" + date.getDate());
                strDate += tmpPart.substring(tmpPart.length - part.length);
                break;
            case 'm': case 'mm':
                tmpPart = ("0" + (date.getMonth() + 1).toString());
                strDate += tmpPart.substring(tmpPart.length - part.length);
                break;
            case 'mmm':
                strDate += arrMMM[date.getMonth() - 1]
                break;
            case 'mmmm':
                strDate += arrMMMM[date.getMonth() - 1]
                break;
            case 'y': case 'yy': case 'yyy':
                tmpPart = ("0000" + (date.getYear() % 1000).toString());
                strDate += tmpPart.substring(tmpPart.length - part.length);
                break;
            case 'yyyy':
                strDate += (date.getYear() + 1900).toString();
                break;
        }
        if (i < 2)
            strDate += sep;
    }

    return strDate;
}


function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > milliseconds) {
            break;
        }
    }
}

/*Common Method*/
String.prototype.toDate = function (format) {
    format = format || "dd/MM/yyy";
    var dateTime = this.trim();
    var sep = dateTime.replace(/[0-9]/g, "")[0];
    if (dateTime.split(sep).length != 3)
        return null;


    var valid = true;
    var onlyTime = '';
    var onlyDate = dateTime.substring(0, 10);
    if (dateTime.indexOf(' ') > 0) {
        onlyTime = dateTime.substring(11).trim().toUpperCase();
    }
    var dateData = onlyDate.split(sep);
    var formatData = format.split(sep),
        formatPart = '';
    var day = 0,
        month = 0,
        year = 0;

    for (var x = 0; x < dateData.length; x++) {
        formatPart = formatData[x].toLowerCase();
        if (formatPart.indexOf('d') > -1)
            day = parseInt(dateData[x]);
        else if (formatPart.indexOf('m') > -1)
            month = parseInt(dateData[x]);
        else if (formatPart.indexOf('y') > -1)
            year = parseInt(dateData[x]);
    }

    var flag = "";
    if (isNaN(month) || isNaN(day) || isNaN(year))
        return null;
    if (month < 1 || month > 12)
        return null;
    else if (day < 1 || day > 31)
        return null;
    else if ((month == 4 || month == 6 || month == 9 || month == 11) && day == 31)
        return null;
    else if ((month == 2 && year % 4 > 0 && day > 28) || (month == 2 && year % 4 == 0 && day > 29) || year < 1900 || year > 2100) {
        return null;
    }

    if (onlyTime.indexOf(':') > 0) {
        var timeData = onlyTime.split(' ')[0].split(':');
        var maxHrs = 23;

        var isAM = true;
        if (onlyTime.indexOf('AM') > 0
            || onlyTime.indexOf('PM') > 0) {
            maxHrs = 12;
            if (timeData.length > 1 && onlyTime.indexOf('PM') > 0 && parseInt(timeData[0]) < 12) {
                timeData[0] = parseInt(timeData[0]) + 12;
                maxHrs = 23;
            } else if (timeData.length > 1 && onlyTime.indexOf('AM') > 0 && parseInt(timeData[0]) == 12) {
                timeData[0] = 0;
                maxHrs = 23;
            }
        }

        var hour = (timeData.length > 1) ? parseInt(timeData[0]) : 0;
        var min = (timeData.length > 1) ? parseInt(timeData[1]) : 0;
        var sec = (timeData.length > 2) ? parseInt(timeData[2]) : 0;

        if (hour < 0 || hour > maxHrs)
            return null;
        else if (min < 0 || min > 59)
            return null;
        else if (sec < 0 || sec > 59)
            return null;
    }
    var returnDate = new Date(year, month - 1, day);
    if (window.browserType == 'Firefox')
        returnDate = new Date(year, month - 1, day, 0, (new Date()).getTimezoneOffset() * -1, 0, 0);
    if (onlyTime.length > 0) {
        returnDate.setTime(returnDate.getTime() + (1000 * 60 * 60 * hour));
        returnDate.setTime(returnDate.getTime() + (1000 * 60 * min));
        returnDate.setTime(returnDate.getTime() + (1000 * sec));
    }

    return returnDate;
}




/*Remove This function later*/
function changeLanguage() {
}


//add by chetan 120517
function validateRepeaterSelection(table_id, msg_id) {
    var count = $('#' + table_id).find('[id*=chkgvselect]:checked').length;
    if (count == 0)
        showMsg(msg_id, 'error', 'Select at-least one record.');
    return count > 0;
}

function setEventHandlerForOtherBrowser() {
    //    var ua = navigator.userAgent.toLowerCase();
    //    if (ua.indexOf('safari') != -1) {
    //        if (ua.indexOf('chrome') > -1) {
    //            //for chrome
    //        } else {
    //            //for safari
    //            $('input:checkbox[name]').each(function (index) {
    //                if ($(this).closest('span').attr('onchange') && !$(this).attr('onchange')) {
    //                    $(this).attr('onchange', $(this).closest('span').attr('onchange'));
    //                    $(this).closest('span').attr('onchange', '');
    //                }
    //            });
    //        }
    //    }
    switch (window.browserType) {
        case 'chrome':
            //for chrome
            break;
        case 'safari':
            $('input:checkbox[name]').each(function (index) {
                if ($(this).closest('span').attr('onchange') && !$(this).attr('onchange')) {
                    $(this).attr('onchange', $(this).closest('span').attr('onchange'));
                    $(this).closest('span').attr('onchange', '');
                }
            });
            break;
        case 'IE':
            //for Internet Explorer
            break;
        default:
            //for Firefox
            break;
    }
}


//var ua = navigator.userAgent.toLowerCase();
function BrowserDetection() {
    //Check if browser is IE
    if (navigator.userAgent.search("MSIE") > -1) {
        window.browserType = 'IE'
    }
    //Check if browser is EDGE
    else if (navigator.userAgent.search("Edge") > -1) {
        // insert conditional Chrome code here
        window.browserType = 'Edge'
    }
    //Check if browser is Chrome
    else if (navigator.userAgent.search("Chrome") > -1) {
        // insert conditional Chrome code here
        window.browserType = 'Chrome'
    }
    //Check if browser is Firefox 
    else if (navigator.userAgent.search("Firefox") > -1) {
        // insert conditional Firefox Code here
        window.browserType = 'Firefox'
    }
    //Check if browser is Safari
    else if (navigator.userAgent.search("Safari") > -1) {
        // insert conditional Safari code here
        window.browserType = 'Safari'
    }
    //Check if browser is Opera
    else if (navigator.userAgent.search("Opera") > -1) {
        // insert conditional Opera code here
        window.browserType = 'Opera'
    } else {	//Else Internet Explorer :D
        window.browserType = 'IE'
    }
}
BrowserDetection();




alertMsg = function (msg) {
    div = document.getElementById('divWindowAlert');
    if (!div) {
        var div = document.createElement('div');
        div.id = 'divWindowAlert';
        div.className = 'window-alert';
        document.body.appendChild(div);

        var content = document.createElement('p');
        content.innerHTML = msg;
        div.appendChild(content);

        var footer = document.createElement('footer');
        div.appendChild(footer);

        var button = document.createElement('button');
        footer.appendChild(button)
        button.innerHTML = "OK"
        button.onclick = function (e) {
            div = document.getElementById('divWindowAlert');
            div.style.display = 'none'
        }
    }
    var msgContent = div.getElementsByTagName('p');
    msgContent.innerHTML = msg;

    div.style.left = ((document.body.offsetWidth - div.offsetWidth) / 2).toString() + 'px';

    if (div.style.display != 'block')
        div.style.display = 'block';
    div.style.opacity = 0;
    div.style.width = div.offsetWidth.toString() + 'px';
    div.style.left = ((document.body.offsetWidth - div.offsetWidth) / 2).toString() + 'px';
    div.style.opacity = 1;
}

function maxLengthTextArea() {
    if ($('textarea[max-length]').length > 0) {
        $('textarea[max-length]').on('keydown', function (e) {
            var mLen = $(this).attr('max-length');
            if (null == mLen)
                mLen = length;

            var maxLength = parseInt(mLen);
            if (!checkSpecialKeys(e)) {
                if ((this).value.length > maxLength - 1) {
                    return false;
                }
            }
        });
    }
    function checkSpecialKeys(e) {
        if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
            return false;
        else
            return true;
    }
}

$(function () {
    maxLengthTextArea();
    //attachExportButton();
});
try {
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        maxLengthTextArea();
        //attachExportButton();
    });
    window.onpageshow = function () {
        maxLengthTextArea();
        //attachExportButton();
    }
}
catch (e) {
}



/*Error Alert*/
function alertException(message) {
    setTimeout(function () {
        var divs = $('div.error-alert-main'); //addedd in body binal new version
        //var divs = $('div.error-alert');
        //        var div = $('<div>')
        //                    .addClass('error-alert')
        //                    .appendTo($('body'));
        //        var span = $('<span>')
        //                        .appendTo(div)
        //                        .text(message);
        //    
        //        var close = $('<span>')
        //                        .css({
        //                            'padding': '5px',
        //                            'position': 'absolute',
        //                            'right' : '5px',
        //                            'top' : '5px',
        //                            'cursor' : 'pointer'
        //                        })
        //                        .text('X')
        //                        .appendTo(div)
        //                        .click(function(){
        //                            $(this).closest('div.error-alert').remove();
        //                        });;


        //var div = $('<div>')
        //  .addClass('error-alert card red')
        // .css({
        //  'float': 'left',
        // 'position': 'fixed',
        //   'left' : '60px',
        // 'width' : '95.7%',
        // 'cursor' : 'pointer'
        // })
        //  .appendTo(divs);


        //var innerdiv= $('<div>')
        //  .addClass('card-content white-text')
        // .appendTo(div);

        //  var innerdivp=$('<p>')
        //  .appendTo(innerdiv)
        //  .text(message);

        // var close = $('<button>')
        //    .addClass('close white-text')
        //     .attr('data-dismiss','alert')
        //     .attr('aria-label','Close')
        //     .appendTo(div)
        //   .click(function(){
        //  $(this).closest('div.error-alert').remove();
        //   });
        //   var spanx = $('<span>')
        //     .attr('aria-hidden','true')
        // .text('X')
        //  .appendTo(close);

        //  var iBottom = (div.height() + 26) * -1; //26 = 12 Top Padding + 12 Bottom Padding + 1 Top Border + 1 Bottom Border
        //  div.css('bottom' , iBottom.toString() + 'px');


        //   var totalHeight = 0;
        //for(var i =0; i<divs.length; i++)
        //totalHeight += ($(divs[i]).height() + 36);        


        //  var intervalID = setInterval(function(){
        //  iBottom += 10;
        // div.css('bottom', iBottom.toString() + 'px');       
        // if (iBottom >= totalHeight)
        // clearInterval(intervalID);
        //   }, 20);

        setTimeout(function () {
            // div.remove();
        }, 30000);
    }, 1000);
}





/**********************************************Export Grid Into Excel**************************************************/
function export2Excel(container) {
    try {
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");

        var mainTable = $(container).closest('table');
        var gridTable = mainTable.find('.headtable').closest('table');
        if (gridTable.length == 0)
            return;

        copyTable = $(gridTable[0].outerHTML);
        copyTable.find('td.col-hidden,th.col-hidden').removeClass('col-hidden');

        var htmlTable = copyTable[0].outerHTML

        exportToExcel(htmlTable);
    } catch (e) {
        console.log(e.toString());
    }
}
function exportToExcel(html) {
    //getting values of current time for generating the file name
    var dt = new Date();
    var day = dt.getDate();
    var month = dt.getMonth() + 1;
    var year = dt.getFullYear();
    var hour = dt.getHours();
    var mins = dt.getMinutes();
    var postfix = day + "." + month + "." + year + "_" + hour + "." + mins;
    //creating a temporary HTML link element (they support setting file names)
    var a = document.createElement('a');
    //getting data from our div that contains the HTML table
    var data_type = 'data:application/vnd.ms-excel';
    a.href = data_type + ', ' + encodeURIComponent(html);
    //setting the file name
    a.download = 'Exported_' + postfix + '.xls';
    //triggering the function
    a.click();
    //just in case, prevent default behaviour
    e.preventDefault();
}

function attachExportButton() {
    $('input[id*=txtpage]').each(function (index) {
        var td = $(this).closest('td');
        if (td.find('img#imgExport').length > 0)
            return;
        var html = "<div style='float: left; padding-right: 3px; cursor:pointer;' align='right' onclick='export2Excel(this); return false;'>" +
            "<img src='image_new/excel.png' id='imgExport' title='Export to Excel File'>" +
            "</div>";
        td.prepend(html);
    });
}