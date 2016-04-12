$("input[type=text]").first().focus();
var form;
$(document).ready(function () {
    // $("#RoleName").first().focus();
    form = $('#FormID');

    $("#FormID").on("submit", function (event) {
        alert("fm");
        if ($("#rname #RoleName").val() == "")
        {
            //alert("hello");
            $("#rname #RoleName").focus();
            return;
        }
        mycustomsubmit(event);
    });
});

function mycustomsubmit(e) {
    //debugger;

    if (form.valid()) {
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: '@Url.Action("Create", "Roles", "Roles")',
            data: form.serialize(),
            success: function (data) {
                if (data == true) {
                    $("#spanError").html("");
                    window.location = '@UserListingURL()'
                }
                else {
                    ShowMessage(BootstrapDialog.TYPE_DANGER, 'Error occured.');
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

function ShowMessage(type, message) {
    $messageData = $("<span>Information</span>");
    BootstrapDialog.show({
        title: $messageData,
        type: type,
        message: message,
        closable: true,
        closeByBackdrop: false,
        closeByKeyboard: false,
        buttons: [{
            label: 'Ok',
            action: function (dialogItself) {
                      
                dialogItself.close();
                       
                       
            }
        }]
    });
}