var form;
$(document).ready(function () {
    form = $('#FormID');
    $("#EditForm").unbind().on("submit", function (event) {

        if ($("#txtRoleName").val().trim() == "") {
            BootstrapDialog.alert("Please Enter Role Name");
            $('#loadingmessage').hide();
            return false;
        }

        $('#loadingmessage').show();
       
        mycustomEditRolesubmit(event);
    });
});
    
function mycustomsubmit(e) {
    $('#loadingmessage').show();
    {
            
        if ($(".rname #RoleName").val().trim() == "") {
            $(".rname #RoleName").focus();
            BootstrapDialog.alert("Please Enter Role");
            $('#loadingmessage').hide();
            return false;
        }

        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: $URL._CreateRoles,
            data: form.serialize(),
           success: function (data) {
            if (data.ResponseStatus) {

                $("#AddModal").modal('hide');
                ShowMessage(BootstrapDialog.TYPE_SUCCESS, data.ResponseText);
                RefreshGrid();
                $('#loadingmessage').hide();
                $("#RoleName").val("");
                    
            }
            else {
                ShowMessage(BootstrapDialog.TYPE_SUCCESS, data.ResponseText);
                $("#AddModal #RoleName").val("");
                $('#loadingmessage').hide();
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


function mycustomsubmit(e) {
    e.preventDefault();
    if (form.valid()) {
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: $URL._CreateRoles,
            data: form.serialize(),
            success: function (data) {
                if (data.ResponseStatus) {
                    $("#AddModal").modal('hide');
                    ShowMessage(BootstrapDialog.TYPE_SUCCESS, data.ResponseText);
                    RefreshGrid();
                    $('#loadingmessage').hide();
                    $("#RoleName").val("");
                }
                else {                    
                    ShowMessage(BootstrapDialog.TYPE_SUCCESS, data.ResponseText);
                    $('#loadingmessage').hide();
                }
                //$("#spanError").html("");
                //window.location = $URL._RolesIndex
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
}


function Addtitle() {
    $('.page-next').attr('Title', 'Next');
    $('.page-first').attr('Title', 'First');
    $('.page-last').attr('Title', 'Last');
    $('.page-pre').attr('Title', 'Previous');
}

// Edit and Delete Formattte links
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
            url: $URL._IsUserPermitedtoDelete,
            success: function (data) {
                if (data == true) {
                    $("#myModal").modal('show');
                    $("#RoleId").val(row.RoleId);
                    $("#showRoleID").text(row.RoleId);
                    $("#txtRoleName").val(row.RoleName);

                    var prop = row.Active;

                    $('#Active').prop('checked', prop);
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

    'click .remove': function (e, value, row, index) {
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: $URL._IsUserPermitedtoDelete,
            success: function (data) {                        
                if(data==true)
                {  
                    BootstrapDialog.show({
                        title: 'Confirmation',
                        message: 'Are you sure You want to delete this record?.',
                        buttons: [{
                            label: 'Yes',
                            cssClass: 'btn-primary',
                            action: function (dialogItself) {
                                $.ajax({
                                    cache: false,
                                    async: false,
                                    type: "POST",
                                    url: $("#HiddenHostName").val()+ "/api/RolesAPI/DeleteRole/" + row.RoleId,
                                    success: function (data) {
                                        if (data == true) {
                                            RefreshGrid();
                                            dialogItself.close();
                                        }
                                        else {
                                            dialogItself.close();
                                            ShowMessage(BootstrapDialog.TYPE_DANGER, 'You do not have permissions to delete this role.');
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

function MassUpdate() {
    CheckUncheckAll();
}
function CheckUncheckAll() {


    if ($("#hiddenMassUpdatecheckAllsts").val() == "" && $("#hiddenMassUpdatecheckAll").val() == "") {
        BootstrapDialog.alert("Please select roles");
        return false;
    }
     
    var roleNames = $("#hiddenMassUpdatecheckAll").val();
    var unCheckIds = '';
    if ($("#hiddenMassUpdatecheckAllsts").val() == "allcheck" && $("#hiddenMassUpdatecheckAll").val() == "0" && $("#hiddenMassUpdatecheckAll").val() != "") {
        unCheckIds = $("#hiddenMassUncheckIds").val();
    } else { unCheckIds = '0'; }

    var activtype = $("#ddlactivate").val();
    var flag = 0;
    var msg = '';
    if (activtype == "1") {
        flag = 2; msg = 'Are you sure you want to activate these records?';
    } else if (activtype == "2") {
        flag = 7; msg = 'Are you sure you want to delete these records?';
    }
    else {
        flag = 3; msg = 'Are you sure you want to deactivarte these records?';
    }
    if ($("#hiddenMassUpdatecheckAllsts").val() == "allcheck") {
        roleNames = "0";
        if (unCheckIds == "0" || unCheckIds == "") {
            if (activtype == "1") { flag = 4; msg = 'Are you sure you want to activate these records?'; }
            else if (activtype == "2") { flag = 6; msg = 'Are you sure you want to delete these records?'; }
            else { flag = 5; msg = 'Are you sure you want to deactivarte these records?'; }
        }
    }

    BootstrapDialog.show({
        title: 'Confirmation',
        message: msg,
        buttons: [{
            label: 'Yes',
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                $('#loadingmessage').show();
                $.ajax({
                    type: "POST",
                    url: $URL._MassUpdateRole,
                    data: '{roleNames: "' + roleNames + '",UnCheckIds:"' + unCheckIds + '",ModifyBy: ' + flag + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {

                        if (activtype == "1") {
                            ShowMessage(BootstrapDialog.TYPE_SUCCESS, 'Records have been marked activated');
                            RefreshGrid();

                        }
                        else if (activtype == "2") {
                            ShowMessage(BootstrapDialog.TYPE_SUCCESS, 'Records have been deleted successfully.');
                            RefreshGrid();
                        }
                        else {
                            ShowMessage(BootstrapDialog.TYPE_SUCCESS, 'Records have been marked deactivated');
                            RefreshGrid();
                        }
                        window.location.href = "";
                        $("#hiddenMassUpdatecheckAllsts").val("");
                        $("#hiddenMassUpdatecheckAll").val("");
                        $("#hiddenMassUncheckIds").val("");
                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });
                dialogItself.close();
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

function CheckUncheckAllFill(src) {
    //  alert(src);
    if (src == "allcheck") {
        $("#hiddenMassUpdatecheckAllsts").val("allcheck");
        $("#hiddenMassUpdatecheckAll").val("0");
    }
    else {
        $("#hiddenMassUpdatecheckAllsts").val("");
        $("#hiddenMassUpdatecheckAll").val("");
    }
}


function checkRow(src) {
    var str = src.RoleId.toString();

    if ($("#hiddenMassUncheckIds").val() != "") { $("#hiddenMassUncheckIds").val($("#hiddenMassUncheckIds").val().replace(str.toString() + ",", " ")); }

    if ($("#hiddenMassUpdatecheckAll").val() == "") {
        $("#hiddenMassUpdatecheckAll").val(str + ",");
    }
    else {
        str = $("#hiddenMassUpdatecheckAll").val() + "" + str + ","
        $("#hiddenMassUpdatecheckAll").val(str);
    }

}
function UncheckRow(src) {
    var str = src.RoleId.toString();

    if ($("#hiddenMassUncheckIds").val() == "") { $("#hiddenMassUncheckIds").val(str + ","); }
    else {
        str = $("#hiddenMassUncheckIds").val() + "" + str + ","
        $("#hiddenMassUncheckIds").val(str);
    }

    if ($("#hiddenMassUpdatecheckAll").val() != "") {
        $("#hiddenMassUpdatecheckAll").val($("#hiddenMassUpdatecheckAll").val().replace(str.toString() + ",", " "));
    }
}

function AddNew() {
    saveid = 1;
    $("#AddModal").modal('show');
    $('FormID').focus_first();
}

function LoadSuccess() {
    $("#btnMassUpdate").show();
    $("#ddlactivate").show();
    $(".bootstrap-table").addClass("mrg");
    $('#loadingmessage').hide();
}

// Focus first element
$.fn.focus_first = function () {
    var elem = $('input:visible', this).get(0);
    var select = $('select:visible', this).get(0);
    if (select && elem) {
        if (select.offsetTop < elem.offsetTop) {
            elem = select;
        }
    }
    var textarea = $('textarea:visible', this).get(0);
    if (textarea && elem) {
        if (textarea.offsetTop < elem.offsetTop) {
            elem = textarea;
        }
    }

    if (elem) {
        elem.focus();
    }
    return this;
}

$("#btnFilter").click(function () {
    var rolename = $("#txtRoleSearch").val();
    var active = $("#Active").val();

    Param.clickBtn = true;
    form = $('#EditForm');
    if (rolename != "") {
        Param.RoleName = rolename;
    }
    else {
        Param.RoleName = "";
    }
    if (Active != "") {
        Param.Active = active;
    }
    else {
        Param.Active = "";
    }
    $('#grid').bootstrapTable('filterBy');
    Param.clickBtn = false;
});

function mycustomEditRolesubmit(e) {

    e.preventDefault();
    $.ajax({
        cache: false,
        async: true,
        type: "POST",
        url: $("#HiddenHostName").val() + "/api/RolesAPI/EditRole",
        data: $('#EditForm').serialize(),//form.serialize(),
        success: function (data) {

            RefreshGrid();
            ShowMessage(BootstrapDialog.TYPE_SUCCESS, 'Your Record is successfully updated.');
            $("#myModal").modal('hide');
            $('#loadingmessage').hide();

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