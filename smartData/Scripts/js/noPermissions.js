$(document).ready(function () {
    BootstrapDialog.show({
        message: 'You are not authorized to view this page.',
        buttons: [{
            label: 'Ok',
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                var url = $("#RedirectTo").val();
                location.href = url;
            }
        }]
    });
});