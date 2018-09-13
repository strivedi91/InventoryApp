function sendGiftcard() {
    var hdnId = $('#hdnId').val();
    var hdnGiftId = $('#hdnGiftId').val();
    $.ajax({
        type: "POST",
        url: lsSendGiftcard,
        content: "application/json; charset=utf-8",
        async: false,
        data: {
            Id: hdnId,
            GiftId: hdnGiftId
        },
        success: function (lodata) {
            if (lodata != null) {
                debugger;
                var divSOReleased = $('#divDetails');

                divSOReleased.html('');
                divSOReleased.html(lodata);
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