function declineCompanyStatus() {
    var hdnId = $('#hdnId').val();
    
    $.ajax({
        type: "POST",
        url: lsDeclineCompanyStatus,
        content: "application/json; charset=utf-8",
        async: false,
        data: {
            fiCompanyId: hdnId
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
function approveCompany() {
    debugger;
    var hdnId = $('#hdnId').val();
    $.ajax({
        type: "POST",
        url: lsApproveCompany,
        content: "application/json; charset=utf-8",
        async: false,
        data: {
            fiCompanyId: hdnId
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
            alert(jqXHR);
            alert(textStatus);
            alert(errorThrown);
            return false;
        }
    });
}
