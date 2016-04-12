var form;
$(function () {
    form = $('#FormID');

});

function mycustomsubmit(e) {
    if ($(".addS #ScreenName").val() == "") {
        //alert("test1");
        $(".addS #ScreenName").focus();
        return;
    }

    if ($(".addS #ScreenName").val().trim() == "") {
        //  alert("hello");
        $(".addS #ScreenName").focus();
        BootstrapDialog.alert("Please Enter Screen Name");
        $('#loadingmessage').hide();
        return false;
    }

    e.preventDefault();
    //  if (form.valid())
    {
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: '@Url.Action("Create", "screen", "screen")',
            data: form.serialize(),
            success: function (data) {
                if (data.ResponseStatus) {
                    $("#AddModal").modal('hide');
                    ShowMessage(BootstrapDialog.TYPE_SUCCESS, data.ResponseText);
                    RefreshGrid();
                }
                else {
                    ShowMessage(BootstrapDialog.TYPE_SUCCESS, data.ResponseText);
                    $("#ScreenName").val('');

                }

            },
            error: function (request, error) {
                if (request.statusText == "CustomMessage") {
                    $("#spanError").html(request.responseText)
                }
            },
            headers: {
                'RequestVerificationToken': '@TokenHeaderValue()'
            }
        });
    }
}