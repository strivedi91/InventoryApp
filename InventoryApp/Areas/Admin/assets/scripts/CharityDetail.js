function declineCharityStatus() {
    var hdnId = $('#hdnId').val();

    $.ajax({
        type: "POST",
        url: lsDeclineCharityStatus,
        content: "application/json; charset=utf-8",
        async: false,
        data: {
            Id: hdnId
        },
        success: function (lodata) {
            if (lodata != null) {
                debugger;
                var divSOReleased = $('#divDetails');

                divSOReleased.html('');
                divSOReleased.html(lodata);
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
function approveCharity() {
    debugger;
    var hdnId = $('#hdnId').val();
    $.ajax({
        type: "POST",
        url: lsUpdateCharity,
        content: "application/json; charset=utf-8",
        async: false,
        data: {
            Id: hdnId
        },
        success: function (lodata) {
            if (lodata != null) {
                debugger;
                var divSOReleased = $('#divDetails');

                divSOReleased.html('');
                divSOReleased.html(lodata);
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

function approveCharityWithout() {
    debugger;
    var hdnId = $('#hdnId').val();
    $.ajax({
        type: "POST",
        url: lsUpdateCharityWithoutPaymet,
        content: "application/json; charset=utf-8",
        async: false,
        data: {
            Id: hdnId
        },
        success: function (lodata) {
            if (lodata != null) {
                debugger;
                var divSOReleased = $('#divDetails');

                divSOReleased.html('');
                divSOReleased.html(lodata);
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
