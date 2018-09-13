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
    if ($('#txtFromDate').val() != "") {
        stSearch = $('#txtFromDate').val();
    }
    refeshList("load");
}
function companySearch(val) {
    $('#hdnSelectedCompany').val(val);
    refeshList("load");
}

function refeshList(foId, pageIndex) {
    
    var lsSelectCompany = parseInt($('#hdnSelectedCompany').val());
    var stSortField = "";

    if (foId == "load") {
        pageIndex = 1;
    }
    else if (foId == "Id ASC" || foId == "Id DESC" || foId == "Company ASC" || foId == "Company DESC" || foId == "Likes ASC" || foId == "Likes DESC" || foId == "PackageLikes ASC" || foId == "PackageLikes DESC" || foId == "CampaignStartDate ASC" || foId == "CampaignStartDate DESC" || foId == "CampaignEndDate ASC" || foId == "CampaignEndDate DESC") {
        pageIndex = parseInt($('#hdnPageIndex').val());
    }
    var lsFromdate = $('#txtFromDate').val().trim();
    var lsTodate = $('#txtToDate').val().trim();
    $.ajax({
        type: "POST",
        url: lsgetCompanyList,
        content: "application/json; charset=utf-8",
        dataType: "html",
        data: {
            inPageIndex: pageIndex,
            inPageSize: 10,
            stSortColumn: $('#hdnOrder').val(),
            selectedCompanyID: lsSelectCompany,
             lsFromDate: lsFromdate,
            lsToDate: lsTodate
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

function clearDate(fsTextBoxId) {
    $('#' + fsTextBoxId).val('');
}
$(function () {

    $('.datepicker').datepicker({
        format: 'mm/dd/yyyy',
        endDate: '+0d',
        autoclose: true
    });
});
