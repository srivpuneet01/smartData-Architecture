var form;
$(function () {
    form = $('#FormID');

})

function mycustomsubmit(e) {
    $('#loadingmessage').show();
    // if (form.valid())
    {
            
        if ($(".rname #ModuleName").val().trim() == "") {
            $(".rname #ModuleName").focus();
            BootstrapDialog.alert("Please Enter Module");
            $('#loadingmessage').hide();
            return false;
        }
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: $URL._CreateModule,
            data: form.serialize(),
        success: function (data) {
            if (data.ResponseStatus) {
                $("#AddModal").modal('hide');
                ShowMessage(BootstrapDialog.TYPE_SUCCESS, data.ResponseText);
                RefreshGrid();
                $('#loadingmessage').hide();
                $("#ModuleName").val("");
            }
            else {
                ShowMessage(BootstrapDialog.TYPE_SUCCESS, data.ResponseText);
                $('#loadingmessage').hide();
            }
        //    @*if (data == true) {
        //        $("#spanError").html("");
        //        window.location = '@UserListingURL()'
        //    }
        //else {
        //        ShowMessage(BootstrapDialog.TYPE_DANGER, 'Error occured.');
        //}*@
        },
    error: function (request, error) {
        if (request.statusText == "CustomMessage") {
            $("#spanError").html(request.responseText)
        }
    },
    headers: {
        //'RequestVerificationToken': '@TokenHeaderValue()'
        'RequestVerificationToken': token

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

var form;
$(document).ready(function () {

    form = $('#FormID');

    $("#FormID").on("submit", function (event) {
        alert("fm");
        if ($("#rname #RoleName").val() == "")
        {
            alert("hello");
            $("#rname #RoleName").focus();
            return;
        }
        mycustomsubmit(event);
    });
});

//function mycustomsubmit(e) {
//    //debugger;

//    if (form.valid()) {
//        $.ajax({
//            cache: false,
//            async: true,
//            type: "POST",
//            url: $URL._CreateRoles,
//            data: form.serialize(),
//        success: function (data) {

//            if (data == true) {
//                $("#spanError").html("");
//                window.location = '@UserListingURL()'
//            }
//            else {
//                ShowMessage(BootstrapDialog.TYPE_DANGER, 'Error occured.');
//            }
//        },
//        error: function (request, error) {
//            if (request.statusText == "CustomMessage") {
//                $("#spanError").html(request.responseText)
//            }
//        },
//        headers: {
//            //'RequestVerificationToken': '@TokenHeaderValue()'
//            'RequestVerificationToken': token
//        }
//    });
//};           
//}
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
