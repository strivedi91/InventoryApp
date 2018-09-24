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
function refeshList(foId,pageIndex) {

    var stSortField = "";
    var lsSearch = $('#txtSearch').val().trim();
    var liFilterCategory = $('#ddlFilterCategory').val();
    var lsOriginalSearch = $('#txtSearch').val().trim();
    var lsSearchQuantity = $('#txtSearchQuantity').val().trim();
    lsSearch = encodeURIComponent(lsSearch);

    lsSearch = lsSearch.replace(/'/g, "''");
    if (foId === "load") {
        pageIndex = 1;
    }
    else if (foId === "Name ASC" || foId === "Name DESC" || foId === "ID ASC" || foId === "ID DESC") {
        pageIndex = parseInt($('#hdnPageIndex').val());
    }
    
    $.ajax({
        type: "POST",
        url: lsSearchProductsUrl,
        content: "application/json; charset=utf-8",
        dataType: "html",
        data: {
            inPageIndex: pageIndex,
            inPageSize: 10,
            stSortColumn: $('#hdnOrder').val(),
            stSearch: lsSearch,
            stSearchQuantity: lsSearchQuantity,
            inFilterCategory: liFilterCategory,
        },
        success: function (lodata) {
            if (lodata !== null) {
                var divToPutHTML = $('#divProductlist');
                divToPutHTML.html('');
                divToPutHTML.html(lodata);
                $('#hdnPageIndex').val(pageIndex);
                //SetPaging();
                setSortingArrow();
                $('#txtSearch').val(lsOriginalSearch);
                $('#ddlFilterCategory').val(liFilterCategory);
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
function getOrderbyCategoryList(foOrderedField, tdID) {
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

    if (sortTD != "") {
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

function DeleteCategory(fiCategoryId) {
    
        $.ajax({
            type: "POST",
            url: lsDeleteProductUrl,
            content: "application/json; charset=utf-8",
            dataType: "json",
            data: {
                ID: fiCategoryId
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
function jsInteger(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode == 46)
        return false;
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;

    return true;
}