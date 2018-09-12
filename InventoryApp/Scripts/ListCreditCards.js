
BindTable('load');

function BindTable(foId) {
    $.ajax({
        type: "POST",
        url: '/billing/getCardList',
        content: "application/json; charset=utf-8",
        dataType: "html",
        data: {
        },
        success: function (lodata) {
            if (lodata != null) {
                var divCardList = $('#divCardList');
                divCardList.html('');
                divCardList.html(lodata);
                if ($('#viewbagvalue').val() == "" || $('#viewbagvalue').val() == null)
                { $('#divCreditCardNotFound').show(); }
                else
                { $('#divCreditCardNotFound').hide(); }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (errorThrown == "abort") {
                return;
            }
            else {
                alert(errorThrown);
            }
        }
    });
}

function formValidate() {
    var form = $("#frmAddEditCard");
    if (isValidFrom()) {
        form.submit();
        $('#showhidetrigger').removeAttr('disabled');
    }
    return true;
}
function isValidFrom() {
    var IsValid = true;
    if ($('#cardno').val().trim() == '') {
        $("#msgStCard").css("display", "block");
        IsValid = false;
    }
    else {
        $("#msgStCard").css("display", "none");
    }

    if ($('#cardno').val().trim() != '') {
        if (parseInt($('#cardno').val().trim().length) < 12) {

            $("#msgStCardLength").css("display", "block");
            IsValid = false;
        }
        else {
            $("#msgStCardLength").css("display", "none");
        }
    }

    if ($('#cardholder').val().trim() == '') {
        $("#msgStCardHolder").css("display", "block");
        IsValid = false;

    }
    else {
        $("#msgStCardHolder").css("display", "none");
    }

    if ($('#cardCVV').val().trim() == '') {
        $("#msgStCVV").css("display", "block");
        IsValid = false;

    }
    else {
        $("#msgStCVV").css("display", "none");
    }

    if ($('#cardCVV').val().trim() != '') {
        if (parseInt($('#cardCVV').val().trim().length) < 3) {

            $("#msgStCVVLength").css("display", "block");
            IsValid = false;
        }
        else {
            $("#msgStCVVLength").css("display", "none");
        }
    }

    if ($('#expdateMonth').val() == null || $('#expdateYear').val() == null) {
        $("#msgStExpDate").css("display", "block");
        IsValid = false;

    }
    else {
        $("#msgStExpDate").css("display", "none");
    }

    //var IsValidDateAndMonth = true;

    //if (parseInt($('#expdateMonth').val().trim()) > 12 || parseInt($('#expdateMonth').val().trim()) < 1) {
    //    $("#msgStExpMonth").css("display", "block");
    //    IsValid = false;
    //    IsValidDateAndMonth = false;
    //}
    //else {
    //    $("#msgStExpMonth").css("display", "none");
    //}

    //if ($('#expdateYear').val().trim() != '') {
    //    if (parseInt($('#expdateYear').val().trim().length) != 4) {
    //        $("#msgStExpYear").css("display", "block");
    //        IsValid = false;
    //        IsValidDateAndMonth = false;
    //    }
    //    else {
    //        $("#msgStExpYear").css("display", "none");
    //    }
    //}
    //if ($('#expdateMonth').val().trim() != '' && $('#expdateYear').val().trim() != '' && IsValidDateAndMonth == true) {
    //    if (IsExpired($('#expdateMonth').val().trim(), $('#expdateYear').val().trim())) {
    //        $("#msgStExpiredDate").css("display", "block");
    //        IsValid = false;
    //    }
    //    else {
    //        $("#msgStExpiredDate").css("display", "none");
    //    }
    //}
    //else {
    //    $("#msgStExpiredDate").css("display", "none");
    //}



    if ($('#address').val().trim() == '') {
        $("#msgStaddress").css("display", "block");
        IsValid = false;

    }
    else {
        $("#msgStaddress").css("display", "none");
    }

    if ($('#city').val().trim() == '') {
        $("#msgStcity").css("display", "block");
        IsValid = false;

    }
    else {
        $("#msgStcity").css("display", "none");
    }

    if ($('#state').val().trim() == '') {
        $("#msgStstate").css("display", "block");
        IsValid = false;

    }
    else {
        $("#msgStstate").css("display", "none");
    }


    if ($('#zip').val().trim() == '') {
        $("#msgStzip").css("display", "block");
        IsValid = false;

    }
    else {

        if (!isValidUSZip($('#zip').val().trim())) {
            $("#msgStzip").css("display", "block");
            $("#msgStzip").html("Invalid zip.");
            IsValid = false;
        }
        else {
            $("#msgStzip").css("display", "none");
        }
    }
    return IsValid;
}
function isValidUSZip(sZip) {
    return /^[0-9]*$/.test(sZip);
}


function IsExpired(month, year) {
    var currentDate = new Date(),
        currentYear = currentDate.getFullYear();

    if (currentYear > year) {
        return true;
    } else if (currentYear == year
            && (month - 1) < currentDate.getMonth()) {
        // Months are zero-indexed, Jan = 0, Feb = 1...
        return true;
    } else {
        return false;
    }
}

function clearForms() {
    $('#cardholder').val("");
    $('#cardno').val("");
    $('#cardCVV').val("");
    $('#expdateMonth').val("");
    $('#expdateYear').val("");
    $('#address').val("");
    $('#city').val("");
    $('#state').val("");
    $('#zip').val("");
    $('#hdnToken').val("");
}
function paynow()
{
    var tokenID = $('#CreditCardNumber').val();

    var flag = true;

    if (tokenID == '' || tokenID == 0) {
        $('#ReqCreditCardNumber').css("display", "");
        flag = false;
    }
    else { $('#ReqCreditCardNumber').css("display", "none"); }

    if (flag == false) {
        return false;
    }

    if (!document.getElementById('CreditCardNumber')) {
        alert('Please add new credit card.');
        return false;
    }
        
    $.ajax({
        type: "POST",
        url: '/billing/paynow',
        content: "application/json; charset=utf-8",
        dataType: "html",
        data: {
            Token: tokenID
        },
        success: function (lodata) {
            if (lodata != null) {
                window.location.href = 'ManageBilling';
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (errorThrown == "abort") {
                return;
            }
            else {
                alert(errorThrown);
            }
        }
    });
    return true;
}
$(document).ready(function () {
    
    $("#cardno").on('paste', function (e) {
        var $this = $(this);
        setTimeout(function () {
            $this.val($this.val().replace(/[^0-9]/g, ''));
        }, 5);
    });
    $("#zip").keypress(function isNumber(evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;


    });

    $("#zip").on('paste', function (e) {
        var $this = $(this);
        setTimeout(function () {
            $this.val($this.val().replace(/[^0-9]/g, ''));
        }, 5);
    });

    $("#cardno").on('paste', function (e) {
        var $this = $(this);
        setTimeout(function () {
            $this.val($this.val().replace(/[^0-9]/g, ''));
        }, 5);
    });

    $("#expdateMonth").on('paste', function (e) {
        var $this = $(this);
        setTimeout(function () {
            $this.val($this.val().replace(/[^0-9]/g, ''));
        }, 5);
    });

    $("#expdateYear").on('paste', function (e) {
        var $this = $(this);
        setTimeout(function () {
            $this.val($this.val().replace(/[^0-9]/g, ''));
        }, 5);
    });

    $("#cardCVV").on('paste', function (e) {
        var $this = $(this);
        setTimeout(function () {
            $this.val($this.val().replace(/[^0-9]/g, ''));
        }, 5);
    });

    (function () {
        var $,
          __indexOf = [].indexOf || function (item) { for (var i = 0, l = this.length; i < l; i++) { if (i in this && this[i] === item) return i; } return -1; };

        $ = jQuery;

        $.fn.validateCreditCard = function (callback, options) {
            var bind, card, card_type, card_types, get_card_type, is_valid_length, is_valid_luhn, normalize, validate, validate_number, _i, _len, _ref;
            card_types = [
              {
                  name: 'amex',
                  pattern: /^3[47]/,
                  valid_length: [15]
              }, {
                  name: 'diners_club_carte_blanche',
                  pattern: /^30[0-5]/,
                  valid_length: [14]
              }, {
                  name: 'diners_club_international',
                  pattern: /^36/,
                  valid_length: [14]
              }, {
                  name: 'jcb',
                  pattern: /^35(2[89]|[3-8][0-9])/,
                  valid_length: [16]
              }, {
                  name: 'laser',
                  pattern: /^(6304|670[69]|6771)/,
                  valid_length: [16, 17, 18, 19]
              }, {
                  name: 'visa_electron',
                  pattern: /^(4026|417500|4508|4844|491(3|7))/,
                  valid_length: [16]
              }, {
                  name: 'visa',
                  pattern: /^4/,
                  valid_length: [16]
              }, {
                  name: 'mastercard',
                  pattern: /^5[1-5]/,
                  valid_length: [16]
              }, {
                  name: 'maestro',
                  pattern: /^(5018|5020|5038|6304|6759|676[1-3])/,
                  valid_length: [12, 13, 14, 15, 16, 17, 18, 19]
              }, {
                  name: 'discover',
                  pattern: /^(6011|622(12[6-9]|1[3-9][0-9]|[2-8][0-9]{2}|9[0-1][0-9]|92[0-5]|64[4-9])|65)/,
                  valid_length: [16]
              }
            ];
            bind = false;
            if (callback) {
                if (typeof callback === 'object') {
                    options = callback;
                    bind = false;
                    callback = null;
                } else if (typeof callback === 'function') {
                    bind = true;
                }
            }
            if (options == null) {
                options = {};
            }
            if (options.accept == null) {
                options.accept = (function () {
                    var _i, _len, _results;
                    _results = [];
                    for (_i = 0, _len = card_types.length; _i < _len; _i++) {
                        card = card_types[_i];
                        _results.push(card.name);
                    }
                    return _results;
                })();
            }
            _ref = options.accept;
            for (_i = 0, _len = _ref.length; _i < _len; _i++) {
                card_type = _ref[_i];
                if (__indexOf.call((function () {
                  var _j, _len1, _results;
                  _results = [];
                  for (_j = 0, _len1 = card_types.length; _j < _len1; _j++) {
                    card = card_types[_j];
                    _results.push(card.name);
                }
                  return _results;
                })(), card_type) < 0) {
                    throw "Credit card type '" + card_type + "' is not supported";
                }
            }
            get_card_type = function (number) {
                var _j, _len1, _ref1;
                _ref1 = (function () {
                    var _k, _len1, _ref1, _results;
                    _results = [];
                    for (_k = 0, _len1 = card_types.length; _k < _len1; _k++) {
                        card = card_types[_k];
                        if (_ref1 = card.name, __indexOf.call(options.accept, _ref1) >= 0) {
                            _results.push(card);
                        }
                    }
                    return _results;
                })();
                for (_j = 0, _len1 = _ref1.length; _j < _len1; _j++) {
                    card_type = _ref1[_j];
                    if (number.match(card_type.pattern)) {
                        return card_type;
                    }
                }
                return null;
            };
            is_valid_luhn = function (number) {
                var digit, n, sum, _j, _len1, _ref1;
                sum = 0;
                _ref1 = number.split('').reverse();
                for (n = _j = 0, _len1 = _ref1.length; _j < _len1; n = ++_j) {
                    digit = _ref1[n];
                    digit = +digit;
                    if (n % 2) {
                        digit *= 2;
                        if (digit < 10) {
                            sum += digit;
                        } else {
                            sum += digit - 9;
                        }
                    } else {
                        sum += digit;
                    }
                }
                return sum % 10 === 0;
            };
            is_valid_length = function (number, card_type) {
                var _ref1;
                return _ref1 = number.length, __indexOf.call(card_type.valid_length, _ref1) >= 0;
            };
            validate_number = (function (_this) {
                return function (number) {
                    var length_valid, luhn_valid;
                    card_type = get_card_type(number);
                    luhn_valid = false;
                    length_valid = false;
                    if (card_type != null) {
                        luhn_valid = is_valid_luhn(number);
                        length_valid = is_valid_length(number, card_type);
                    }
                    return {
                        card_type: card_type,
                        valid: luhn_valid && length_valid,
                        luhn_valid: luhn_valid,
                        length_valid: length_valid
                    };
                };
            })(this);
            validate = (function (_this) {
                return function () {
                    var number;
                    number = normalize($(_this).val());
                    return validate_number(number);
                };
            })(this);
            normalize = function (number) {
                return number.replace(/[ -]/g, '');
            };
            if (!bind) {
                return validate();
            }
            this.on('input.jccv', (function (_this) {
                return function () {
                    $(_this).off('keyup.jccv');
                    return callback.call(_this, validate());
                };
            })(this));
            this.on('keyup.jccv', (function (_this) {
                return function () {
                    return callback.call(_this, validate());
                };
            })(this));
            callback.call(this, validate());
            return this;
        };

    }).call(this);

    $('#cardno').on('input', function () {
        
        if ($("#hdnImageUrl").val().length > 0) {

            $('#cardno').css({ 'background-image': 'url(' + "../../images/images.png" + ')', 'background-repeat': 'no-repeat' });
            $('#cardno').css('background-repeat', 'no-repeat');
            $('#cardno').css('background-size', ' 120px 361px, 120px 361px');
            $('#cardno').css('background-position', '2px -121px, 260px -61px');
            $('#cardno').css('padding-left', '54px');
            $('#cardno').css('width', '100%');
            $('#cardno').css('height', '34px');
            $('#cardno').css('border', '1px solid #ccc;');
            $("#hdnImageUrl").val("");
        }
    });

    //if (jQuery('#cardno').keyup()) {
    //    debugger
    //    if ($("#hdnImageUrl").val().length == 0) {
            
    //        $('#cardno').css({ 'background-image': 'url(' + "../../images/images.png" + ')', 'background-repeat': 'no-repeat' });
    //        $('#cardno').css('background-repeat', 'no-repeat');
    //        $('#cardno').css('background-size', ' 120px 361px, 120px 361px');
    //        $('#cardno').css('background-position', '2px -121px, 260px -61px');
    //        $('#cardno').css('padding-left', '54px');
    //        $('#cardno').css('width', '100%');
    //        $('#cardno').css('height', '34px');
    //        $('#cardno').css('border', '1px solid #ccc;');
            
    //    }
    //}

    if ($("#hdnImageUrl").val().length > 0) {
        
        $('#cardno').removeClass();
        $('#cardno').css('background-image', "'" + $("#hdnImageUrl").val() + "'");
        $('#cardno').css({ 'background-image': 'url(' + $("#hdnImageUrl").val() + ')', 'background-repeat': 'no-repeat' });
        $('#cardno').css('background-repeat', 'no-repeat');
        $('#cardno').css('background-size', '42px');
        $('#cardno').css('background-position', 'initial');
        $('#cardno').css('padding-left', '54px');
        $('#cardno').css('width', '100%');
        $('#cardno').css('height', '32px');
        $('#cardno').css('border', '1px solid #ccc;');
        $('#cardno').css('background-position-y', 'center');
        
    }
    

    //$('#cardno').on('change', function (e) {
        
    //    if ($("#hdnImageUrl").val().length > 0)
    //    { 
    //    $('#cardno').css({ 'background-image': 'url(' + "../../images/images.png" + ')', 'background-repeat': 'no-repeat' });
    //    $('#cardno').css('background-repeat', 'no-repeat');
    //    $('#cardno').css('background-size', ' 120px 361px, 120px 361px');
    //    $('#cardno').css('background-position', '2px -121px, 260px -61px');
    //    $('#cardno').css('padding-left', '54px');
    //    $('#cardno').css('width', '100%');
    //    $('#cardno').css('height', '34px');
    //    $('#cardno').css('border', '1px solid #ccc;');
    //    $("#hdnImageUrl").val("");
    //    }
        
    //});

    //$("#cardno").change(function () {
        
    //    $("#hdnImageUrl").val("");
    //});

   

});

function saveAutoPay() {

    var chkAuto = $("#IsAutoRefillOK");
    var isAutoRefil = chkAuto.is(':checked');

    $.ajax({
        type: "POST",
        url: "/Billing/SaveAutoRefillSettings",
        content: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        processing: true,
        data: {
            IsAutoRefillOK: isAutoRefil
        },
        success: function (lodata) {
            if (lodata != null) {
                if (lodata == 1) {
                    $("#divSucMsg").show();
                }
                if (lodata == -1) {
                    $("#divErrMsg").show();
                    
                }
            }
        }
    });
}
function hidediv(lsDiv)
{
    $("#" + lsDiv).hide();
}


$(function () {
    $('#cardno').validateCreditCard(function (result) {
       

        if (result.card_type == null) {
            $('#cardno').removeClass();
        }
        else {
            $('#cardno').addClass(result.card_type.name);
        }

        if (!result.valid) {
            $('#cardno').removeClass("valid");
        }
        else {
            $('#cardno').addClass("valid");
        }

    });
});