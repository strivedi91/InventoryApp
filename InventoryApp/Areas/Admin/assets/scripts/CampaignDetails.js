function declineCampaignStatus() {
    var hdnId = $('#hdnId').val();

    $.ajax({
        type: "POST",
        url: lsDeclineCampaign,
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
                //$("#updateportal").modal('show');
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
function approveCampaign() {

    var hdnId = $('#hdnId').val();
    $.ajax({
        type: "POST",
        url: lsUpdateCampaign,
        content: "application/json; charset=utf-8",
        async: false,
        data: {
            Id: hdnId
        },
        success: function (lodata) {
            if (lodata != null) {
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
function refeshList(foId, pageIndex) {

    $.ajax({
        type: "POST",
        url: lsGetVolunteers,
        content: "application/json; charset=utf-8",
        dataType: "html",
        data: {
            inPageIndex: pageIndex,
            inPageSize: 10,
            Id: $('#hdnId').val()
        },
        success: function (lodata) {
            if (lodata != null) {
                var divSOReleased = $('#campaignVolunteerlist');
                divSOReleased.html('');
                divSOReleased.html(lodata);
                $('#hdnVolPageIndex').val(pageIndex);
                $('.dashboard-menu').height('100%');
                $('.dashboard-menu').height($(document).height());
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