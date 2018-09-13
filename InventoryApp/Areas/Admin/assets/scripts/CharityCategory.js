
$(document).ready(function () {
    setSortingArrowCategory();
    $('#txtSearch').focus();
});
function categorySearch(e) {
    if (e.which == 13) {
        e.preventDefault();
        $(this).blur();
        $('#btnSearch').focus().click();
    }
}

function searchCategory() {
    var stSearch = "";
    var stToDate = "";
    if ($('#txtSearch').val() != "") {
        stSearch = $('#txtSearch').val();
    }
    refeshCategory(stSearch);
}
function refeshCategory(foId, pageIndex) {

    var stSortField = "";
    var lsSearch = $('#txtSearch').val().trim();
    var lsOriginalSearch = $('#txtSearch').val().trim();
    lsSearch = encodeURIComponent(lsSearch);

    var lsSearch = lsSearch.replace(/'/g, "''");

    if (foId == "load") {
        pageIndex = 1;
    }
    else if (foId == "Name ASC" || foId == "Name DESC") {
        pageIndex = parseInt($('#hdnPageIndex').val());
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
                var divcategory = $('#divcategory');
                divcategory.html('');
                divcategory.html(lodata);
                $('#hdnPageIndex').val(pageIndex);
                setSortingArrowCategory();
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
function getOrderbyList(foOrderedField, tdID) {
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
    refeshCategory(foFieldwithOrder);
}

function setSortingArrowCategory() {

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
        $('#sortName').removeClass("sorting");
        $('#sortName').addClass("sorting_asc");
        $('#hdnOrder').val("Name ASC");
    }
}

function DeleteUser(fiUserId) {
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
                refeshCategory("delete");
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
