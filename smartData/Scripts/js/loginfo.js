
function Addtitle() {
    $('.page-next').attr('Title', 'Next');
    $('.page-first').attr('Title', 'First');
    $('.page-last').attr('Title', 'Last');
    $('.page-pre').attr('Title', 'Previous');
}
function DateFormatter(value, row, index) {
    return [
     row.ModifiedDate = new Date(row.ModifiedDate).toLocaleDateString()
    ].join('');
}
function rowStyle(row, index) {
    var classes = ['active', 'success', 'info', 'warning', 'danger'];

    if (index % 2 === 0) {
        return {
            classes: classes[1]
        };
    }
    return {};
}
var $table = $("#Loggrid");
var chk = false;
function RefreshGrid() {
    document.getElementById("btnFilter").click();
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

function LoadSuccess() {
    //$("#btnMassUpdate").show();
    //$("#ddlactivate").show();
}