$.ajaxSetup({
    beforeSend: function () {
        if ($("#loadingbar").length === 0) {
            $("body").append("<div id='loadingbar'></div>")
            $("#loadingbar").addClass("waiting").append($("<dt/><dd/>"));
            $("#loadingbar").width((50 + Math.random() * 30) + "%");
        }
    },
    complete: function () {
        $("#loadingbar").width("101%").delay(200).fadeOut(400, function () {
            $(this).remove();
        });
    }
});

function DisplayMessage(lsMessage) {
    $("#divSuccess").show();
    $("#divSuccess").removeClass("hide");
    $("#divSuccess").alert();
    $("#spanMessage").html(lsMessage);
    window.scrollTo(1, 1);
    $("#divSuccess").fadeTo(3000, 500).slideUp(500, function () {
        $("#divSuccess").hide();
        $("#divSuccess").addClass("hide");
        // $("#divSuccess").alert('close');
        $("#spanMessage").html("");
    });
}

function refreshCreditBalance() {
    $.ajax({
        type: "GET",
        url: "/Billing/getLatestCreditBalance",
        content: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        processing: true,
        success: function (lodata) {
            if (lodata != null) {
                if (lodata >= 0) {
                    $("#currentCreditBalance").html(lodata);
                }
            }
        }
    });
}

/**
Purpics Site Javascript
**/

function isNumericValue(e) {
    if ($.inArray(e.keyCode, [17, 46, 8, 9, 27, 13, 116]) !== -1 || (e.keyCode == 65 && e.ctrlKey === true) || (e.keyCode >= 35 && e.keyCode <= 39)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || e.keyCode == 110 || e.keyCode == 190 || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
}

function preventSpace(e) {
    if (e.keyCode == 32)
        e.preventDefault();
}

function isDecimalValue(e) {
    if ($.inArray(e.keyCode, [17, 46, 8, 9, 27, 13, 110, 190, 116]) !== -1 || (e.keyCode == 65 && e.ctrlKey === true) || (e.keyCode >= 35 && e.keyCode <= 39)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
}

// Allow Only Alphbates
function isFirstNameValue(e) {
    if ($.inArray(e.keyCode, [8, 9, 36, 35, 37, 39, 46]) !== -1 || (e.keyCode >= 65 && e.keyCode <= 90)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.keyCode > 90) || (e.keyCode < 65)) {
        e.preventDefault();
    }
}

// Allow Only Alphbates + white space + '
function isLastNameValue(e) {
    if ($.inArray(e.keyCode, [8, 9, 32, 222, 36, 35, 37, 39, 46]) !== -1 || (e.keyCode >= 65 && e.keyCode <= 90)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.keyCode > 90) || (e.keyCode < 65)) {
        e.preventDefault();
    }
}

// Allow Only Alphbates + 0-9 + _ + .
function isUserNameValue(e) {
    if ($.inArray(e.keyCode, [190, 9, 173, 110, 46, 8, 95, 36, 35, 37, 39]) !== -1 || (e.keyCode >= 65 && e.keyCode <= 90) || (e.keyCode >= 48 && e.keyCode <= 57)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.keyCode > 90) || (e.keyCode < 65)) {
        e.preventDefault();
    }
}

function IsNumeric(input) {
    var RE = /^-{0,1}\d+$/;
    return (RE.test(input));
}

function getDateTimeAmPmFormat(date) {

    var lsMonth = date.getMonth() + 1;
    var lsDay = date.getDate();
    var lsYear = date.getFullYear();
    var lsHours = date.getHours();
    var lsMinutes = date.getMinutes();
    var lsAmPm = lsHours >= 12 ? 'pm' : 'am';
    lsHours = lsHours % 12;
    lsHours = lsHours ? lsHours : 12; // the hour '0' should be '12'
    minutes = lsMinutes < 10 ? '0' + lsMinutes : lsMinutes;
    var lsDateTime = lsMonth + '/' + lsDay + '/' + lsYear + ' ' + lsHours + ':' + lsMinutes + ' ' + lsAmPm;
    return lsDateTime;
}

function getDateFormat(date) {

    var lsMonth = date.getMonth() + 1;
    var lsDay = date.getDate();
    var lsYear = date.getFullYear();
    var lsDate = lsMonth + '/' + lsDay + '/' + lsYear;
    return lsDate;
}

function getTimeAmPmFormat(date) {
    var lsHours = date.getHours();
    var lsMinutes = date.getMinutes();
    var lsAmPm = lsHours >= 12 ? 'pm' : 'am';
    lsHours = lsHours % 12;
    lsHours = lsHours ? lsHours : 12; // the hour '0' should be '12'
    minutes = lsMinutes < 10 ? '0' + lsMinutes : lsMinutes;
    var lsDateTime = lsHours + ':' + lsMinutes + ' ' + lsAmPm;
    return lsDateTime;
}

function getTimeDiff(fdtFromDate, fdtToDate) {
    var timeStart = fdtFromDate.getTime();
    var timeEnd = fdtToDate.getTime();
    var hourDiff = timeEnd - timeStart; //in ms
    var secDiff = hourDiff / 1000; //in s
    var minDiff = hourDiff / 60 / 1000; //in minutes
    var hDiff = hourDiff / 3600 / 1000; //in hours  

    var seconds = Math.floor((timeEnd - timeStart) / 1000);
    var minutes = Math.floor(seconds / 60);
    var hours = Math.floor(minutes / 60);
    var days = Math.floor(hours / 24);

    hours = hours - (days * 24);
    minutes = minutes - (days * 24 * 60) - (hours * 60);

    var humanReadable = {};
    humanReadable.days = days.toFixed(0);
    humanReadable.hours = hours.toFixed(0);
    humanReadable.minutes = minutes.toFixed(0);
    return humanReadable; //{hours: 0, minutes: 30}
}

function getLastSynce(dtLastSyncedDevice) {
    var laTime = getTimeDiff(new Date(parseInt(dtLastSyncedDevice.substr(6))), new Date());
    var lsTime = 'Last synced ';
    if (laTime.days > 0) {
        if (laTime.days > 1 && laTime.hours > 1 && laTime.minutes > 1)
            lsTime += laTime.days + ' days, ' + laTime.hours + ' hours and ' + laTime.minutes + ' minutes ago';
        else if (laTime.days > 1 && laTime.hours <= 1 && laTime.minutes > 1)
            lsTime += laTime.days + ' days, ' + laTime.hours + ' hour and ' + laTime.minutes + ' minutes ago';
        else if (laTime.days > 1 && laTime.hours > 1 && laTime.minutes <= 1)
            lsTime += laTime.days + ' days, ' + laTime.hours + ' hours and ' + laTime.minutes + ' minutes ago';
        else if (laTime.days <= 1 && laTime.hours > 1 && laTime.minutes > 1)
            lsTime += laTime.days + ' day, ' + laTime.hours + ' hours and ' + laTime.minutes + ' minutes ago';
        else if (laTime.days > 1 && laTime.hours <= 1 && laTime.minutes <= 1)
            lsTime += laTime.days + ' days, ' + laTime.hours + ' hour and ' + laTime.minutes + ' minute ago';
        else if (laTime.days <= 1 && laTime.hours > 1 && laTime.minutes <= 1)
            lsTime += laTime.days + ' day, ' + laTime.hours + ' hours and ' + laTime.minutes + ' minute ago';
        else if (laTime.days <= 1 && laTime.hours <= 1 && laTime.minutes > 1)
            lsTime += laTime.days + ' day, ' + laTime.hours + ' hour and ' + laTime.minutes + ' minutes ago';
    }
    else if (laTime.days <= 0 && laTime.hours > 0) {
        if (laTime.hours > 1 && laTime.minutes > 1)
            lsTime += laTime.hours + ' hours and ' + laTime.minutes + ' minutes ago';
        else if (laTime.hours > 1 && laTime.minutes <= 1)
            lsTime += laTime.hours + ' hours and' + laTime.minutes + ' minute ago';
        else if (laTime.hours <= 1 && laTime.minutes > 1)
            lsTime += laTime.hours + ' hour and' + laTime.minutes + ' minutes ago';
        else if (laTime.hours <= 1 && laTime.minutes <= 1)
            lsTime += laTime.hours + ' hour and' + laTime.minutes + ' minute ago';
    }
    else {
        lsTime += laTime.minutes + ' minutes ago';
    }

    return lsTime;
}


//Visa card
function validVisaCardNumber(fsText) {
    var cardno = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;
    if (fsText.match(cardno)) {
        return true;
    }
    else {
        return false;
    }
}

function validAmeExprCardNumber(inputtxt) {
    var cardno = /^(?:3[47][0-9]{13})$/;
    if (inputtxt.value.match(cardno)) {
        return true;
    }
    else {
        return false;
    }
}

function validMasterCardNumber(inputtxt) {
    var cardno = /^(?:5[1-5][0-9]{14})$/;
    if (inputtxt.value.match(cardno)) {
        return true;
    }
    else {
        return false;
    }
}

function validDiscoverCardNumber(inputtxt) {
    var cardno = /^(?:6(?:011|5[0-9][0-9])[0-9]{12})$/;
    if (inputtxt.value.match(cardno)) {
        return true;
    }
    else {
        return false;
    }
}

function validDinersClubCardNumber(inputtxt) {
    var cardno = /^(?:3(?:0[0-5]|[68][0-9])[0-9]{11})$/;
    if (inputtxt.value.match(cardno)) {
        return true;
    }
    else {
        return false;
    }
}

function validJCBCardNumber(inputtxt) {
    var cardno = /^(?:(?:2131|1800|35\d{3})\d{11})$/;
    if (inputtxt.value.match(cardno)) {
        return true;
    }
    else {
        return false;
    }
}

//get city state name from lat & lng
function getCityStateName(fsLat, fsLng) {
    var lsReturn = '';
    if (fsLat != '' && fsLng != '' && fsLat != null && fsLng != null) {
        $.ajax({
            type: "GET",
            url: "https://maps.google.com/maps/api/geocode/json?latlng=" + fsLat + "," + fsLng + "&sensor=true",
            async: false,
            dataType: "json"
        })
        .error(function () {
            return lsReturn;
        })
        .success(function (loResult) {
            if (loResult.status == "OK") {
                for (var liCounter = 0; liCounter < loResult.results[0].address_components.length; liCounter++) {
                    if (loResult.results[0].address_components[liCounter].types[0] == "locality") {
                        lsReturn += loResult.results[0].address_components[liCounter].short_name + ',';
                    }

                    if (loResult.results[0].address_components[liCounter].types[0] == "administrative_area_level_1") {
                        lsReturn += loResult.results[0].address_components[liCounter].short_name;
                    }

                    if (loResult.results[0].address_components[liCounter].types[0] == "postal_code") {
                        lsReturn += ' ' + loResult.results[0].address_components[liCounter].short_name;
                    }
                }
                return lsReturn;
            }
            else
                lsReturn = '';
        });
    }
    return lsReturn;
}

//get city state name from lat & lng
function getVerifyAddress(fsAddress) {
    var lsReturn = '';
    if (fsAddress != '' && fsAddress != null) {
        $.ajax({
            type: "GET",
            url: "https://maps.google.com/maps/api/geocode/json?address=" + fsAddress + "&sensor=true",
            async: false,
            dataType: "json"
        })
        .error(function () {
            return lsReturn;
        })
        .success(function (loResult) {
            if (loResult.status == "OK") {
                for (var liCounter = 0; liCounter < loResult.results[0].address_components.length; liCounter++) {
                    if (loResult.results[0].address_components[liCounter].types[0] == "postal_code") {
                        lsReturn += ' ' + loResult.results[0].address_components[liCounter].short_name;
                    }
                }
                return lsReturn;
            }
            else
                lsReturn = '';
        });
    }
    return lsReturn;
}

// check string lenght
function isValidStringLength(lsString, liStringLength) {
    var lbIsVaildString = true;
    if (lsString.length >= liStringLength)
        lbIsVaildString = false;

    return lbIsVaildString;
}

function checkRegulerExpr(fsValue, fsRegExp) {
    var lbIsVaild = true;

    var lsValue = fsValue;
    var lsRegExp = new RegExp(fsRegExp);

    if (!lsRegExp.test(lsValue)) {
        lbIsVaild = false;
    }
    return lbIsVaild;
}

function getCurrentRegion() {
    var lsReturn = '';
    $.ajax({
        type: "GET",
        url: "https://freegeoip.net/json/",
        async: false,
        dataType: "json"
    })
    .error(function () {
        return lsReturn;
    })
    .success(function (loResult) {
        if (loResult != null) {
            if (loResult.country_code != '' && loResult.country_code != null)
                lsReturn = loResult.country_code;
        }
    });
    return lsReturn;
}


Number.prototype.formatMoney = function (c, d, t) {
    var n = this,
        c = isNaN(c = Math.abs(c)) ? 2 : c,
        d = d == undefined ? "." : d,
        t = t == undefined ? "," : t,
        s = n < 0 ? "-" : "",
        i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};


function getFileExtenstion(fsFilename) {
    return (/[.]/.exec(fsFilename)) ? /[^.]+$/.exec(fsFilename) : undefined;
}

// getUrlVars()["me"];
function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

// getParameterByName('prodId');
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}
function cancelForm() {
    if ($("#loadingbar").length === 0) {
        $("body").append("<div id='loadingbar'></div>")
        $("#divLoader").css("display", "block");
        $("#loadingbar").addClass("waiting").append($("<dt/><dd/>"));
        $("#loadingbar").width((50 + Math.random() * 30) + "%");
    }
}

$("input[type=button]").bind('click', function (e) {
    $(this).prop('disabled', true);
});

$("a").bind('click', function (e) {
    // Commented for Logout drodown menu not working
    //$(this).prop('disabled', true);
});

function GetPager(recCount, pageIndex)
{
    if (typeof recCount === "undefined") {
        recCount = 10;
    }

    if (typeof pageIndex === "undefined") {
        pageIndex = 1;
    }

    $.ajax({
        type: "POST",
        url: "/Pager/getPagerData",
        content: "application/json; charset=utf-8",
        dataType: "html",
        async: true,
        processing: true,
        data: {
            RecCount: recCount ,
            PageIndex: pageIndex ,
        },
        success: function (lodata) {
            if (lodata != null) {
                $("#divPager").html(lodata);
            }
        }
    });
}

function validate_double(theField, len, precision) {
    numVal = theField.value.replace(/[^\d|\.]/g, '').split('.');
    if (numVal.length > 1) {
        numVal[0] = numVal[0].substr(0, len);
        numVal[1] = numVal[1].substr(0, precision);
        numVal.length = 2;
        theField.value = numVal.join('.');
    } else {
        theField.value = numVal[0];
    };
}