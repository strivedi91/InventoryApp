$(document).ready(function () {
    setSortingArrow();
    $('#txtSearch').focus();
});

function userSearch(e) {
    if (e.which == 13) {
        e.preventDefault();
        $(this).blur();
        $('#btnSearch').focus().click();
    }
}

function search() {
    var stSearch = "";
    var stToDate = "";
    if ($('#txtSearch').val() != "") {
        stSearch = $('#txtSearch').val();
    }
    refeshList(stSearch);
}
function statusSearch(val) {
    $('#hdnStatus').val(val);
    refeshList("load");
}

function refeshList(foId, pageIndex) {

    var lsStatus = parseInt($('#hdnStatus').val());
    var stSortField = "";
    var lsSearch = $('#txtSearch').val().trim();
    var lsOriginalSearch = $('#txtSearch').val().trim();
    var lsSearch = lsSearch.replace(/'/g, "''");
    lsSearch = encodeURIComponent(lsSearch);

    if (foId == "load") {
        pageIndex = 1;
    }
    else if (foId == "Id ASC" || foId == "Id DESC" || foId == "LegalCampaignName ASC" || foId == "LegalCampaignName DESC" || foId == "Company ASC" || foId == "Company DESC" || foId == "Charity ASC" || foId == "Charity DESC" || foId == "StudentOrganization ASC" || foId == "StudentOrganization DESC") {
        pageIndex = parseInt($('#hdnPageIndex').val());
    }

    $.ajax({
        type: "POST",
        url: lsgetCompanyList,
        content: "application/json; charset=utf-8",
        dataType: "html",
        data: {
            inPageIndex: pageIndex,
            inPageSize: 10,
            stSortColumn: $('#hdnOrder').val(),
            stSearch: lsSearch,
            CampaignStatusID: lsStatus
        },
        success: function (lodata) {
            if (lodata != null) {
                var divSOReleased = $('#companylist');
                divSOReleased.html('');
                divSOReleased.html(lodata);
                $('#hdnPageIndex').val(pageIndex);
                //SetPaging();
                $('.dashboard-menu').height('100%');
                $('.dashboard-menu').height($(document).height());

                setSortingArrow();
                $('#txtSearch').val(lsOriginalSearch);
                $('#txtSearch').focus();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $('#txtSearch').val(lsOriginalSearch);
            $('#txtSearch').focus();
            if (errorThrown == "abort") {
                return;
            }
            else {
                alert(errorThrown);
            }
        }
    });
}
function getOrderbyCampaignList(foOrderedField, tdID) {
    var foFieldwithOrder = "";
    $('#hdnSortingOnColumn').val(tdID);

    if ($('#hdnOrder').val() == "") {
        $('#hdnOrder').val(foOrderedField + " ASC");
        foFieldwithOrder = foOrderedField + " ASC";
        $('#hdnOrder').val(foFieldwithOrder);
        $('#hdnSortingDirection').val("ASC");

    }
    else {
        if ($('#hdnOrder').val() == (foOrderedField + " ASC")) {
            foFieldwithOrder = foOrderedField + " DESC";
            $('#hdnOrder').val(foFieldwithOrder);
            $('#hdnSortingDirection').val("DESC");
        }
        else {
            foFieldwithOrder = foOrderedField + " ASC";
            $('#hdnOrder').val(foFieldwithOrder);
            $('#hdnSortingDirection').val("ASC");
        }
    }
    refeshList(foFieldwithOrder);
}
function setSortingArrow() {

    var sortTD = $('#hdnSortingOnColumn').val();
    var sortDirection = $('#hdnSortingDirection').val();

    if (sortTD != "") {
        $('#' + sortTD).removeClass("sorting");
        if (sortDirection.toUpperCase() == "ASC") {
            $('#' + sortTD).addClass("sorting_asc");
        }
        else if (sortDirection.toUpperCase() == "DESC") {
            $('#' + sortTD).addClass("sorting_desc");
        }
    }
    else {
        $('#sortId').removeClass("sorting");
        $('#sortId').addClass("sorting_asc");
        $('#hdnOrder').val("Id ASC");
    }
}


var RowId;
function confirmStatusChange(id) {
    RowId = id;
    var chkIsActive = $("#IsFeaturedCampaign_" + RowId);
    var status = chkIsActive.is(':checked');

    if (chkIsActive.is(':checked') == false) {
        $('#modelChangeStatus').modal('hide');
        $('#modelChangeStatusUnFeatured').modal('show');
    }
    else {
        $('#modelChangeStatusUnFeatured').modal('hide');
        $('#modelChangeStatus').modal('show');
    }

}
function changeStatus() {
    var chkIsActive = $("#IsFeaturedCampaign_" + RowId);
    $('#modelChangeStatus').modal('hide');
    $('#modelChangeStatusUnFeatured').modal('hide');
    $.ajax({
        type: "POST",
        url: changeFeaturedCampaign,
        content: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        processing: true,
        data: {
            Id: RowId,
            isFeaturedCampaign: chkIsActive.is(':checked')
        },
        success: function (lodata) {
            if (lodata != null) {
                if (lodata == 1) {
                    DisplayMessage("Status changed successfully.");
                }
            }
        }
    });
    return false;
}

function promoteCampaign(campaignId) {
    $('#txtNotificationMessage').val('');
    $('#txtNotificationMessage').removeClass("input-validation-error");
    $('#notificationMessageRequired').css('display', 'none');
    $('#hdnPromoteCampaignId').val(campaignId);
    $('#modelPromoteCampaign').modal('show');
}

function sendNotification() {
    if ($('#txtNotificationMessage').val().trim() == '') {
        $('#txtNotificationMessage').addClass("input-validation-error");
        $('#notificationMessageRequired').css('display', 'block');
        return;
    }
    else {
        $('#txtNotificationMessage').removeClass("input-validation-error");
        $('#notificationMessageRequired').css('display', 'none');
    }

    $.ajax({
        type: "POST",
        url: lsSendNotification,
        content: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        processing: true,
        data: {
            campaignId: $('#hdnPromoteCampaignId').val(),
            notificationMessage: $('#txtNotificationMessage').val()
        },
        success: function (lodata) {
            if (lodata != null) {
                if (lodata.status == "success") {
                    DisplayMessage(lodata.message);
                }
                else {
                    alert(lodata.message);
                }
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
    $('#modelPromoteCampaign').modal('hide');
    return false;
}

function cancelNotification() {
    $('#modelPromoteCampaign').modal('hide');
}
