$(document).ready(function () {
    SetPaging();
    setSortingArrow();
    $('#txtSearch').focus();
});
function userSearch(e) {

    if (e.which == 13) {
        debugger
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
function refeshList(foId) {
    var pageIndex = 0;
    var stSortField = "";
    var lsSearch = $('#txtSearch').val().trim();
    var lsOriginalSearch = $('#txtSearch').val().trim();
    var lsSearch = lsSearch.replace(/'/g, "''");
    if (foId == "aHrefNext") {
        pageIndex = parseInt($('#hdnPageIndex').val()) + 1;
    }
    else if (foId == "aHrefPrev") {
        pageIndex = parseInt($('#hdnPageIndex').val()) - 1;
    }
    else if (foId == "load") {
        pageIndex = 1;
    }
    else if (foId == "Name ASC" || foId == "Name DESC" || foId == "Value ASC" || foId == "Value DESC" ) {
        pageIndex = parseInt($('#hdnPageIndex').val());
    }
    else {
        pageIndex = 1;
    }
    $.ajax({
        type: "POST",
        url: lsgetUserList,
        content: "application/json; charset=utf-8",
        dataType: "html",
        data: {
            inPageIndex: pageIndex,
            inPageSize: 10,
            stSortColumn: $('#hdnOrder').val(),
            stSearch: lsSearch
        },
        success: function (lodata) {
            if (lodata != null) {
                var divSOReleased = $('#packagelist');
                divSOReleased.html('');
                divSOReleased.html(lodata);
                $('#hdnPageIndex').val(pageIndex);
                SetPaging();
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
function getOrderbyUserList(foOrderedField, tdID) {
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

function SetPaging() {

    var inPagesize = 10;

    $('#aHrefPrev').parent("li").attr('class', 'disabled');
    $('#aHrefPrev').removeAttr("onclick", "javascript:void(0);");

    pageIndex = $('#hdnPageIndex').val();
    totalRec = $('#hdnTotalRec').val();

    if (pageIndex == 1) {
        $('#aHrefPrev').parent("li").attr('class', 'disabled');
        $('#aHrefPrev').attr("onclick", "javascript:void(0);");
    }
    else {
        $('#aHrefPrev').parent("li").removeAttr('class', 'disabled');
        $('#aHrefPrev').attr("onclick", "refeshList(this.id)");
    }
    if (pageIndex * inPagesize >= totalRec) {
        $('#aHrefNext').parent("li").attr('class', 'disabled');
        $('#aHrefNext').attr("onclick", "javascript:void(0);");
    }
    if (inPagesize >= totalRec) {
        $('#Patientpaging').css('visibility', 'hidden');
    }
    else {
        $('#Patientpaging').css('visibility', 'visible');
    }
}

function setSortingArrow() {

    var sortTD = $('#hdnSortingOnColumn').val();
    var sortDirection = $('#hdnSortingDirection').val();

    if (sortTD != "") {
        $('#' + sortTD).removeClass("sorting");
        if (sortDirection == "ASC") {
            $('#' + sortTD).addClass("sorting_asc");
        }
        else if (sortDirection == "DESC") {
            $('#' + sortTD).addClass("sorting_desc");
        }
    }
    else {
        $('#sortName').removeClass("sorting");
        $('#sortName').addClass("sorting_asc");
        $('#hdnOrder').val("Name ASC");
    }
}

function DeleteUser(fiUserId) {
    if (confirm('Are you sure you want to delete this record?')) {

        $.ajax({
            type: "POST",
            url: deleteUserUrl,
            content: "application/json; charset=utf-8",
            dataType: "json",
            data: {
                ID: fiUserId
            },
            success: function (lodata) {
                if (lodata != null) {

                    refeshList("delete");
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
    return false;
}

$("#CampaignPer").keydown(function (e) {
    isDecimalValue(e);
});

$("#CampaignPointAwarded").keydown(function (e) {
    isDecimalValue(e);
});

$("#RedemptionPoints").keydown(function (e) {
    isDecimalValue(e);
});

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

var form = $("#frmUpdateAppSettings");
function formValidate() {
    debugger
    form.validate();
    if (form.valid()) {
        form.submit();
    }
    return true;
}

