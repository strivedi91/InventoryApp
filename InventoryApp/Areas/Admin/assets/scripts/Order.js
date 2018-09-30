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

function search() {
    var stSearch = "";
    if ($('#txtFromDate').val() !== "") {
        stSearch = $('#txtFromDate').val();
    }
    refeshList("load");
}

$('#ddlFilterCategory').change(function () {
    refeshList("load");
});

$('#ddlFilterSeller').change(function () {
    refeshList("load");
});

$('#ddlFilterStatus').change(function () {
    refeshList("load");
});

function clearFilter() {
    
    $('#txtFromDate').val('');
    $('#txtToDate').val('');
    $('#ddlFilterCategory').val('0');
    $('#ddlFilterSeller').val('');
    $('#ddlFilterStatus').val('');
    refeshList("load");
}

function refeshList(foId, pageIndex) {
    if (pageIndex === undefined)
        pageIndex = 0;
    
    var stSortField = "";
    var lsStatus = parseInt($('#hdnStatus').val());
    var lsSearch = "";//$('#txtSearch').val().trim();
    var lsOriginalSearch = "";//$('#txtSearch').val().trim();
    lsSearch = lsSearch.replace(/'/g, "''");
    lsSearch = encodeURIComponent(lsSearch);

    var liFilterCategory = $('#ddlFilterCategory').val();
    var liFilterSeller = $('#ddlFilterSeller').val();
    var lsFilterStatus = $('#ddlFilterStatus').val();
    var lsFromdate = $('#txtFromDate').val().trim();
    var lsTodate = $('#txtToDate').val().trim();

    if (foId === "aHrefNext") {
        $('#hdnPageIndex').val(pageIndex);
    }
    else if (foId === "aHrefPrev") {
        $('#hdnPageIndex').val(pageIndex);
    }
    else if (foId === "load") {
        pageIndex = 1;
    }
    else if (foId === "Id ASC" || foId === "Id DESC" || foId === "CreatedOn ASC" || foId === "CreatedOn DESC" || foId === "Discount ASC" || foId === "Discount DESC" || foId === "OrderStatus ASC" || foId === "OrderStatus DESC" || foId === "SubTotal ASC" || foId === "SubTotal DESC" || foId === "Total ASC" || foId === "Total DESC") {
        pageIndex = parseInt($('#hdnPageIndex').val());
    }
    
    $.ajax({
        type: "POST",
        url: lsgetOrderList,
        content: "application/json; charset=utf-8",
        dataType: "html",
        data: {
            inPageIndex: pageIndex,
            inPageSize: 10,
            stSortColumn: $('#hdnOrder').val(),
            stSearch: lsSearch,
            lsFromDate: lsFromdate,
            lsToDate: lsTodate,
            inFilterCategory: liFilterCategory,
            OrderStatus: lsFilterStatus,
            inFilterSeller: liFilterSeller
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
                //$('#txtSearch').val(lsOriginalSearch);
                //$('#txtSearch').focus();
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

function updateOrderStatus(fiOrderId, ele) {
    $.ajax({
        type: "GET",
        url: lsUpdateOrderStatusUrl,
        content: "application/json; charset=utf-8",
        dataType: "json",
        data: {
            fiOrderId: fiOrderId,
            fsOrderStatus: $(ele).val()
        },
        success: function (Result) {
            if (Result === true || Result === 'true') {
                $('#alertSuccessMsg').show();
                $('#alertErrorMsg').hide();
            }
            else {
                $('#alertSuccessMsg').hide();
                $('#alertErrorMsg').show();
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
        if (sortDirection != null) {
            if (sortDirection.toUpperCase() === "ASC") {
                $('#' + sortTD).addClass("sorting_asc");
            }
            else if (sortDirection.toUpperCase() === "DESC") {
                $('#' + sortTD).addClass("sorting_desc");
            }
        }
    }
    else {
        $('#sortId').removeClass("sorting");
        $('#sortId').addClass("sorting_asc");
        $('#hdnOrder').val("Id ASC");
    }
}
