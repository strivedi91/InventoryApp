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

function userPointsSearch(e) {

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

function clearsearch() {
    $('#txtSearch').val("");
    refeshList("load");
}

function refeshList(foId, pageIndex) {

    var stSortField = "";
    var lsSearch = $('#txtSearch').val().trim();
    var lsPointSearch = $('#txtPointSearch').val().trim();
    var lsOriginalSearch = $('#txtSearch').val().trim();
    var lsSearch = lsSearch.replace(/'/g, "''");
    if (foId == "load") {
        pageIndex = 1;
    }
    else if (foId == "Id ASC" || foId == "Id DESC" || foId == "Name ASC" || foId == "Name DESC" || foId == "CreditPoints ASC" || foId == "CreditPoints DESC" || foId == "GiftCardPreferenceName ASC" || foId == "GiftCardPreferenceName DESC") {
        pageIndex = parseInt($('#hdnVolunteerPageIndex').val());
    }
    $.ajax({
        type: "POST",
        url: lsgetVolunteerUserList,
        content: "application/json; charset=utf-8",
        dataType: "html",
        data: {
            inPageIndex: pageIndex,
            inPageSize: 10,
            stSortColumn: $('#hdnVolunteerOrder').val(),
            stSearch: lsSearch,
            stPointSearch: lsPointSearch
        },
        success: function (lodata) {

            if (lodata != null) {
                var divSOReleased = $('#divVolunteersList');
                divSOReleased.html('');
                divSOReleased.html(lodata);
                $('#hdnVolunteerPageIndex').val(pageIndex);

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
    $('#hdnVolunteerSortingOnColumn').val(tdID);

    if ($('#hdnVolunteerOrder').val() == "") {
        $('#hdnVolunteerOrder').val(foOrderedField + " ASC");
        foFieldwithOrder = foOrderedField + " ASC";
        $('#hdnVolunteerOrder').val(foFieldwithOrder);
        $('#hdnVolunteerSortingDirection').val("ASC");

    }
    else {
        if ($('#hdnVolunteerOrder').val() == (foOrderedField + " ASC")) {
            foFieldwithOrder = foOrderedField + " DESC";
            $('#hdnVolunteerOrder').val(foFieldwithOrder); $('#hdnVolunteerSortingDirection').val("DESC");
        }
        else {
            foFieldwithOrder = foOrderedField + " ASC";
            $('#hdnVolunteerOrder').val(foFieldwithOrder);
            $('#hdnVolunteerSortingDirection').val("ASC");
        }
    }
    refeshList(foFieldwithOrder);
}

function setSortingArrow() {

    var sortTD = $('#hdnVolunteerSortingOnColumn').val();
    var sortDirection = $('#hdnVolunteerSortingDirection').val();

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
        $('#sortName').removeClass("sorting");
        $('#sortName').addClass("sorting_asc");
        $('#hdnVolunteerOrder').val("Id ASC");
    }
}