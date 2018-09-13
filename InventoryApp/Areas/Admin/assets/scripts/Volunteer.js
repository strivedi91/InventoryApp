$(document).ready(function () {
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
function refeshList(foId, pageIndex) {
    var stSortField = "";
    var lsSearch = $('#txtSearch').val().trim();
    var lsOriginalSearch = $('#txtSearch').val().trim();
    var lsSearch = lsSearch.replace(/'/g, "''");
   
    if (foId == "load") {
        pageIndex = 1;
    }
    else if (foId == "Id ASC" || foId == "Id DESC" || foId == "Name ASC" || foId == "Name DESC" || foId == "FollowerCount ASC" || foId == "FollowerCount DESC") {
        pageIndex = parseInt($('#hdnPageIndex').val());
    }
    
    $.ajax({
        type: "POST",
        url: lsgetVolunteersList,
        content: "application/json; charset=utf-8",
        dataType: "html",
        data: {
            inPageIndex: pageIndex,
            inPageSize: 10,
            stSortColumn: $('#hdnOrder').val(),
            stSearch: lsSearch,
            stSelectedSOs: $("#hdnSelectedSOs").val()
        },
        success: function (lodata) {
            if (lodata != null) {
                var divSOReleased = $('#companylist');
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
function getOrderbyCompanyList(foOrderedField, tdID) {
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
