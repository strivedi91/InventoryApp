$(document).ready(function () {
    setSortingArrow();
    $('#txtSearch').focus();
});
function userSearch(e) {
    if (e.which === 13) {
        e.preventDefault();
        $(this).blur();
        $('#btnSearch').focus().click();
    }
}

$('#ddlFilterCategory').change(function () {
    refeshList();
});

function search() {
    var stSearch = "";
    var stToDate = "";
    if ($('#txtSearch').val() !== "") {
        stSearch = $('#txtSearch').val();
    }
    refeshList(stSearch);
}
function clearFilter() {

    $('#txtSearch').val('');
    refeshList("load");
}
function refeshList(foId,pageIndex) {

    var stSortField = "";
    var lsSearch = $('#txtSearch').val().trim();
    var lsOriginalSearch = $('#txtSearch').val().trim();
    lsSearch = encodeURIComponent(lsSearch);

    lsSearch = lsSearch.replace(/'/g, "''");
    if (foId === "load") {
        pageIndex = 1;
    }
    else if (foId === "Name ASC" || foId === "Name DESC" || foId === "ID ASC" || foId === "ID DESC" || foId === "Type ASC" || foId === "Type DESC" || foId === "Brand ASC" || foId === "Brand DESC" || foId === "Price ASC" || foId === "Price DESC" || foId === "Quantity ASC" || foId === "Quantity DESC" || foId === "CategoryId ASC" || foId === "CategoryId DESC") {
        pageIndex = parseInt($('#hdnPageIndex').val());
    }
    
    $.ajax({
        type: "POST",
        url: lsSearchCouponsUrl,
        content: "application/json; charset=utf-8",
        dataType: "html",
        data: {
            inPageIndex: pageIndex,
            inPageSize: 10,
            stSortColumn: $('#hdnOrder').val(),
            stSearch: lsSearch
        },
        success: function (lodata) {
            if (lodata !== null) {
                var divToPutHTML = $('#divCouponlist');
                divToPutHTML.html('');
                divToPutHTML.html(lodata);
                $('#hdnPageIndex').val(pageIndex);
                setSortingArrow();
                $('#txtSearch').val(lsOriginalSearch);
                $('#txtSearch').focus();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $('#txtSearch').val(lsOriginalSearch);
            $('#txtSearch').focus();
            if (errorThrown === "abort") {
                return;
            }
            else {
                alert(errorThrown);
            }
        }
    });
}

function getOrderedList(foOrderedField, tdID) {
    var foFieldwithOrder = "";
    $('#hdnSortingOnColumn').val(tdID);

    if ($('#hdnOrder').val() === "") {
        $('#hdnOrder').val(foOrderedField + " ASC");
        foFieldwithOrder = foOrderedField + " ASC";
        $('#hdnOrder').val(foFieldwithOrder);
        $('#hdnSortingDirection').val("ASC");

    }
    else {
        if ($('#hdnOrder').val() === (foOrderedField + " ASC")) {
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

    if (sortTD !== "") {
        $('#' + sortTD).removeClass("sorting");
        if (sortDirection.toUpperCase() === "ASC") {
            $('#' + sortTD).addClass("sorting_asc");
        }
        else if (sortDirection.toUpperCase() === "DESC") {
            $('#' + sortTD).addClass("sorting_desc");
        }
    }
    else {
        $('#sortName').removeClass("sorting");
        $('#sortName').addClass("sorting_asc");
        $('#hdnOrder').val("Name ASC");
    }
}

function DeleteCategory(fiId) {
        $.ajax({
            type: "POST",
            url: lsDeleteCouponUrl,
            content: "application/json; charset=utf-8",
            dataType: "json",
            data: {
                ID: fiId
            },
            success: function (lodata) {
                if (lodata !== null) {
                    refeshList("delete");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (errorThrown === "abort") {
                    return;
                }
                else {
                    alert(errorThrown);
                }
            }
        });
}