/********************************** forgot password scripts start */
function updateSuccess(data) {
    //alert(data);
    $('#loadingmessage').show();
    if (data == 'Current password is not correct.') {
        $('#divSpanError').show();

        $('#loadingmessage').hide();
    }
    else if (data == "Current password and new password must be different") {
        $('#divSpanError').show();

        $('#loadingmessage').hide();
    }
    else if (data == "Password changed.") {

        $('.validation-summary-errors').addClass('validation-summary-valid');
        $('.validation-summary-valid').removeClass('validation-summary-errors');
        $('#divSpanError').attr('style', 'display:none;');
        $('#divSuccess').show();

        $('#spanSuccess ').attr('style', 'color:green;');
        $('#spanSuccess').html("Your password changed successfully.");

        clearFileds();
        //window.location.reload(true);
        $('#loadingmessage').hide();
    }
}

function hideCustomErr() {
    //alert('enter');
    // $('.validation-summary-errors').hide();
    $('#divSpanError').attr('style', 'display:none;');
    $('#divSuccess').attr('style', 'display:none;');
    //$('#loadingmessage').show();

    return true;
}

function clearFileds() {
    $('#NewPswd').val("");
    $('#OldPswd').val("");
    $('#ConfirmPswd').val("");
}
/********************************** forgot password scripts end */

/********************************** register scripts start */
$(function () {
    $("input[type=text]").first().focus();
    // alert("hi");
});
/********************************** register scripts end */