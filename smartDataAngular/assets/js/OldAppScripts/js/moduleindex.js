
function Addtitle() {
    $('.page-next').attr('Title', 'Next');
    $('.page-first').attr('Title', 'First');
    $('.page-last').attr('Title', 'Last');
    $('.page-pre').attr('Title', 'Previous');
}


// Edit and Delete Formatte links
function operateFormatter(value, row, index) {
    return [
        '<a id="edit"  class="edit ml10" href="javascript:void(0)" title="Edit">',
            '<i class="glyphicon glyphicon-edit"></i>',
        '</a>&nbsp;&nbsp;',
        '<a id="delete" class="remove ml10" href="javascript:void(0)" title="Remove">',
            '<i class="glyphicon glyphicon-remove"></i>',
        '</a>'
    ].join('');
}

// Link Events Edit and Delete
window.operateEvents = {       
    'click .edit': function (e, value, row, index) {
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: "/Users/Users/IsUserPermitedtoDelete",
            success: function (data) {                        
                if(data==true)
                {  
                    $("#myModal").modal('show');
                    $("#ModuleID").val(row.ModuleID);
                    $("#txtModuleName").val(row.ModuleName);
                }
                else {
                    ShowMessage(BootstrapDialog.TYPE_DANGER, 'Sorry, only Super Admin can edit this user!');
                    RefreshGrid();
                }
            },
            error: function (request) {
            },
        })
    },

    //

    'click .chkItem': function (e, value, row, index) {
        alert(row.RoleId);

        var prop = row.Active;

        $('#Active').prop('checked', prop);

    },

    'click .remove': function (e, value, row, index) {
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: "/Users/Users/IsUserPermitedtoDelete",
            success: function (data) {                        
                if(data==true)
                {  
                    BootstrapDialog.show({title: 'Confirmation',
                        message: 'Are you sure you want to delete this record?',
                        buttons: [{
                            label: 'Yes',
                            cssClass: 'btn-primary',
                            action: function (dialogItself) {
                                $.ajax({
                                    cache: false,
                                    async: false,
                                    type: "POST",
                                    url: $("#HiddenHostName").val()+ "/api/ModuleAPI/DeleteModule/" + row.ModuleID,
                                    success: function (data) {
                                        if (data == true) {
                                            RefreshGrid();
                                            dialogItself.close();
                                        }
                                        else {
                                            dialogItself.close();
                                            ShowMessage(BootstrapDialog.TYPE_DANGER, 'You do not have permissions to delete this module.');
                                        }
                                    },
                                    error: function (request, error) {
                                        if (request.statusText == "CustomMessage") {
                                            $("#spanError").html(request.responseText)
                                        }
                                    },
                                    headers: {
                                        'RequestVerificationToken': token
                                    }
                                });
                            }
                        }, {
                            label: 'No',
                            cssClass: 'btn-danger',
                            action: function (dialogItself) {
                                dialogItself.close();
                            }
                        }]
                    });
                }
                else {
                    ShowMessage(BootstrapDialog.TYPE_DANGER, 'Sorry, only Super Admin can delete this user!');
                    RefreshGrid();
                }
            },
            error: function (request) {
            },
        })
    }
};

function rowStyle(row, index) {
    var classes = ['active', 'success', 'info', 'warning', 'danger'];

    if (index % 2 === 0) {
        return {
            classes: classes[1]
        };
    }
    return {};
}

function RefreshGrid() {
    $('#grid').bootstrapTable('refresh', { silent: true });
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

function selectval(src) {

    Selectcheckbox(src, $(src).parent().next().text());
}

function stateFormatter(value, row, index) {
    alert(row.RoleId);
    if (index === 2) {
        return {
            disabled: true
        };
    }
    if (index === 5) {
        return {
            disabled: true,
            checked: true
        }
    }
    return value;
}
   
function LoadSuccess() {
    $(".bootstrap-table").addClass("mrg");
    $('#loadingmessage').hide();
}

function AddNew()
{
    $("#AddModal").modal('show');
    $(".rname #ModuleName").val("");
}

