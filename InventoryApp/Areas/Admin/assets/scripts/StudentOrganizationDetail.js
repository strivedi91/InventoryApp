function declineStudentOrganizationStatus() {
    var hdnId = $('#hdnId').val();

    $.ajax({
        type: "POST",
        url: lsDeclineStudentOrganizationStatus,
        content: "application/json; charset=utf-8",
        async: false,
        data: {
            Id: hdnId
        },
        success: function (lodata) {
            if (lodata != null) {
                debugger;
                var divSODetails = $('#divSODetails');
                divSODetails.html('');
                divSODetails.html(lodata);
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
function approveStudentOrganization() {
    var hdnId = $('#hdnId').val();
    $.ajax({
        type: "POST",
        url: lsUpdateStudentOrganization,
        content: "application/json; charset=utf-8",
        async: false,
        data: {
            Id: hdnId
        },
        success: function (lodata) {
            if (lodata != null) {
                var divSODetails = $('#divSODetails');
                divSODetails.html('');
                divSODetails.html(lodata);
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
