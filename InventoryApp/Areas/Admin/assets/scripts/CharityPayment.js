$(document).ready(function () {
    setSortingArrow();
    $('#txtSearch').focus();
});
function userpay(e) {

    if (e.which == 13) {
        e.preventDefault();
        $(this).blur();
        $('#btnSearch').focus().click();
    }
}

function charitypay() {
    var stSearch = "";
    var stToDate = "";
    if ($('#txtSearch').val() != "") {
        stSearch = $('#txtSearch').val();
    }
    refeshList("load");
}
function statusSearch(val) {
    $('#hdnStatus').val(val);
    refeshList("load");
}
function refeshList(foId, pageIndex) {
    var stSortField = "";
    var lsStatus = parseInt($('#hdnStatus').val());
    var lsSearch = $('#txtSearch').val().trim();
    var lsOriginalSearch = $('#txtSearch').val().trim();
    var lsSearch = lsSearch.replace(/'/g, "''");
    lsSearch = encodeURIComponent(lsSearch);
    if (foId == "aHrefNext") {
        $('#hdnPageIndex').val(pageIndex);
    }
    else if (foId == "aHrefPrev") {
        $('#hdnPageIndex').val(pageIndex);
    }
    else if (foId == "load") {
        pageIndex = 1;
    }
    else if (foId == "Id ASC" || foId == "Id DESC" || foId == "Charity ASC" || foId == "Charity DESC" || foId == "Campaign ASC" || foId == "Campaign DESC" || foId == "Date ASC" || foId == "Date DESC") {
        pageIndex = parseInt($('#hdnPageIndex').val());
    }
    else if (foId == "update")
    {
        pageIndex = parseInt($('#hdnPageIndex').val());
    }
    
    $.ajax({
        type: "POST",
        url: lsgetCharityPaymentList,
        content: "application/json; charset=utf-8",
        dataType: "html",
        data: {
            inPageIndex: pageIndex,
            inPageSize: 10,
            stSortColumn: $('#hdnOrder').val(),
            stSearch: lsSearch,
            STATUS: lsStatus
        },
        success: function (lodata) {

            if (lodata != null) {
                var divSOReleased = $('#charityPaymentList');
                divSOReleased.html('');
                divSOReleased.html(lodata);
                $('#hdnPageIndex').val(pageIndex);
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
function getOrderbyCharityPaymentList(foOrderedField, tdID) {
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
    $('#modelPaymentConfirm').modal('show');
}
function cancelUpdate() {
    var chkStatus = $("#chkbox" + RowId);

    if (chkStatus.is(':checked')) {
        $(chkStatus).prop('checked', false);
    }
    else {
        $(chkStatus).prop('checked', true);
    }
}
function checkUnits() {
    $('#modelPaymentConfirm').modal('hide');

    var value = $('#txt' + RowId).val().trim();
    value = value.replace("$ ", "");
    if (value == "")
    {
        value = 0;
    }
    var pageIndex = 0;
    var stSortField = "";
    var lsStatus = parseInt($('#hdnStatus').val());
    var lsSearch = $('#txtSearch').val().trim();
    var lsOriginalSearch = $('#txtSearch').val().trim();
    var lsSearch = lsSearch.replace(/'/g, "''");
    lsSearch = encodeURIComponent(lsSearch);
    pageIndex = parseInt($('#hdnPageIndex').val());
    $.ajax({
        type: "POST",
        url: lsPayCharityPayment,
        content: "application/json; charset=utf-8",
        dataType: "html",
        data: {
            ID: RowId,
            fsVal: value,
            inPageIndex: pageIndex,
            inPageSize: 10,
            stSortColumn: $('#hdnOrder').val(),
            stSearch: lsSearch,
            STATUS: lsStatus
        },
        success: function (lodata) {
            if (lodata != null) {
                var divSOReleased = $('#charityPaymentList');
                divSOReleased.html('');
                divSOReleased.html(lodata);
                $('#hdnPageIndex').val(pageIndex);
                $('.dashboard-menu').height('100%');
                $('.dashboard-menu').height($(document).height());
                setSortingArrow();
                $('#txtSearch').val(lsOriginalSearch);
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
function payPayment(fiId) {
    var value = $('#txt' + fiId).val().trim();
    value = value.replace("$ ", "");
    if (value == "") {
        value = 0;
    }
    var pageIndex = 0;
    var stSortField = "";
    var lsStatus = parseInt($('#hdnStatus').val());
    var lsSearch = $('#txtSearch').val().trim();
    var lsOriginalSearch = $('#txtSearch').val().trim();
    var lsSearch = lsSearch.replace(/'/g, "''");
    lsSearch = encodeURIComponent(lsSearch);
    pageIndex = parseInt($('#hdnPageIndex').val());
    $.ajax({
        type: "POST",
        url: lsCharityPayment,
        content: "application/json; charset=utf-8",
        dataType: "html",
        data: {
            ID: fiId,
            fsVal: value,
            inPageIndex: pageIndex,
            inPageSize: 10,
            stSortColumn: $('#hdnOrder').val(),
            stSearch: lsSearch,
            STATUS: lsStatus
        },
        success: function (lodata) {
            if (lodata != null) {
                var divSOReleased = $('#charityPaymentList');
                divSOReleased.html('');
                divSOReleased.html(lodata);
                $('#hdnPageIndex').val(pageIndex);
                $('.dashboard-menu').height('100%');
                $('.dashboard-menu').height($(document).height());
                setSortingArrow();
                $('#txtSearch').val(lsOriginalSearch);
                $('#txtSearch').focus();
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
