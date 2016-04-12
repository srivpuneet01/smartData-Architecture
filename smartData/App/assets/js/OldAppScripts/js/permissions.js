var showpopup = "0";
var callbacks = $.Callbacks();
$(document).ready(function () {
    $("#HiddenAllpermission").val("");
    $("#HiddenExitspermission").val("");
    $("#HiddenDeletepermission").val("");
    $("#HiddenDeletepermission").val("");
    $("#RoleList").change(function () {
        //  BindGrid();
        if (($("#ModuleList :selected").text() == "Select Module")) {
            $("#ModuleList").focus();
            return;
        }
        if (($("#RoleList :selected").text() == "Select Role")) {
            $("#RoleList").focus();
            return;
        }
        //CheckBoxFill();
        BindGrid1();
    });
    $("#ModuleList").change(function () {        
        if (($("#ModuleList :selected").text() == "Select Module")) {
            $("#ModuleList").focus();
            return;
        }
        if (($("#RoleList :selected").text() == "Select Role")) {
            $("#RoleList").focus();
            return;
        }
        // CheckBoxFill();
        BindGrid1();
    });

    $('#FullPermission').click(function (event) {  //on click 
        if (this.checked) { // check select status
            $('.full').each(function () { //loop through each checkbox
                this.checked = true;  //select all checkboxes with class "full"      
                var str = this.id.toString();
                if ($("#HiddenAllpermission").val() == "") {
                    $("#HiddenAllpermission").val(str + "/");
                }
                else {
                    str = $("#HiddenAllpermission").val() + "" + str + "/"
                    $("#HiddenAllpermission").val(str);
                }
            });
        } else {
            $('.full').each(function () { //loop through each checkbox
                this.checked = false; //deselect all checkboxes with class "full"   
                var str = this.id.toString();
                $("#HiddenAllpermission").val($("#HiddenAllpermission").val().replace(str + "/", ""));
                if ($("#HiddenDeletepermission").val() == "") {
                    $("#HiddenDeletepermission").val(str + "/");
                }
                else {
                    str = $("#HiddenDeletepermission").val() + "" + str + "/"
                    $("#HiddenDeletepermission").val(str);
                }
            });
        }
               
    });
    $("#ButtonAdd").click(function () {        
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: $URL._IsUserPermitedtoDelete,
            success: function (data) {                        
                if(data==true)
                {  
                    if (($("#RoleList :selected").text() == "Select Role")) {
                        $("#RoleList").focus();
                        alert("Please select role");
                        return;
                    }
                    if (($("#ModuleList :selected").text() == "Select Module")) {
                        $("#ModuleList").focus();
                        alert("Please select module");
                        return;
                    }
                    if ($("#FullPermission").is(":checked")) {
                   
                        AddFullPermission();
                    }
                    else {
                        if ($("#HiddenDeletepermission").val() != "") {
                            DeletePermission();

                        }
                        DeleteFullPermission();
                        
                        alert('Module permission saved successfully!');
                    }
                }
                else {
                    alert('Sorry, you are not authorized user!');
                    return false;
                }
                $('#loadingmessage').hide();
            },
            error: function (request) {
            },
        })

    });
});

function BindGrid() {
    var ModuleID, RoleId;
    if (($("#ModuleList :selected").text() == "Select Module")) {
        ModuleID = "";
    }
    else
    {
        ModuleID = $("#ModuleList :selected").val();
    }
    if (($("#RoleList :selected").text() == "Select Role")) {
        RoleId = "";
    }
    else
    {
        RoleId=$("#RoleList :selected").val();
    }
    var path = $URL._GetAllScreenPermission;
    $.getJSON(path, { ModuleID: ModuleID, RoleId: RoleId }, function (data) {
        if (data != null) {
            var j = "";
            $('#ActionGrid').html("");
            var table = '<table id="Actiongrid" class="gridtable">';
            table += '<tr>';
            table += '<th data-field="ActionId">Screen Name</th>';
            table += '<th data-field="Actions">Actions</th>';
            table += '</tr>';
            $.each(data, function (i, item) {
                    
                if (i == 0) {
                    table += '<tr>';
                    j = item.ScreenName;
                    table += '<td class="visible"><input id="' + "hfv" + item.ScreenId + '" type="hidden" value="' + item.ScreenId + '" />' + item.ScreenName;
                    table += '</td>';
                    if (item.ActionId == 0) {
                        if (item.ActionName == "Custom") {

                        }
                        else {
                            table += '<td class="f2"><input id="' + item.ScreenId + "," + item.ActionId + "," + item.ActionName + '" class="' + item.ActionName + "" + item.ScreenId + "" + item.ActionId + '" type="checkbox" onclick="javascript:Selectcheckbox(this);" disabled="disabled" />' + item.ActionName
                        }
                    }
                    else {
                        table += '<td class="f2"><input id="' + item.ScreenId + "," + item.ActionId + "," + item.ActionName + '" class="' + item.ActionName + "" + item.ScreenId + "" + item.ActionId + '" type="checkbox" onclick="javascript:Selectcheckbox(this);"/>' + item.ActionName
                    }
                    table += '</td>';
                }
                else if (j.toString().toLocaleLowerCase() == item.ScreenName.toString().toLocaleLowerCase()) {
                         
                    if (item.ActionId == 0) {
                        if (item.ActionName == "Custom") {

                        }
                        else {
                            table += '<td class="f2"><input id="' + item.ScreenId + "," + item.ActionId + "," + item.ActionName + '" class="' + item.ActionName + "" + item.ScreenId + "" + item.ActionId + '" type="checkbox" onclick="javascript:Selectcheckbox(this);" disabled="disabled" />' + item.ActionName
                        }
                    }
                    else {
                        table += '<td class="f2"><input id="' + item.ScreenId + "," + item.ActionId + "," + item.ActionName + '" class="' + item.ActionName + "" + item.ScreenId + "" + item.ActionId + '" type="checkbox" onclick="javascript:Selectcheckbox(this);"/>' + item.ActionName
                    }
                    table += '</td>';
                }
                    
                else {
                    table += '</tr>';
                    j = item.ScreenName;
                    table += '<tr>';
                    table += '<td class="visible"><input id="' + "hfv" + item.ScreenId + '" type="hidden" value="' + item.ScreenId + '" />' + item.ScreenName;
                    table += '</td>';
                    if (item.ActionId == 0) {
                        if (item.ActionName == "Custom") {

                        }
                        else {
                            table += '<td class="f2"><input id="' + item.ScreenId + "," + item.ActionId + "," + item.ActionName + '" class="' + item.ActionName + "" + item.ScreenId + "" + item.ActionId + '" type="checkbox" onclick="javascript:Selectcheckbox(this);" disabled="disabled" />' + item.ActionName
                        }
                    }
                    else {
                        table += '<td class="f2"><input id="' + item.ScreenId + "," + item.ActionId + "," + item.ActionName + '" class="' + item.ActionName + "" + item.ScreenId + "" + item.ActionId + '" type="checkbox" onclick="javascript:Selectcheckbox(this);"/>' + item.ActionName
                    }
                    table += '</td>';
                }
            })
            table += '</tr>';
            table += '</table>';
            $('#ActionGrid').append(table);
        }
        else {
            document.write("Not Found");
        }
    });

}

function Selectcheckbox(Ctr) {
    showpopup = "0";
          
    var str = Ctr.id.toString();
    if ($(Ctr).is(':checked')) {
              
        if ($("#HiddenAllpermission").val() == "") {
            $("#HiddenAllpermission").val(str + "/");
        }
        else {
            str = $("#HiddenAllpermission").val() + "" + str + "/"
            $("#HiddenAllpermission").val(str);
        }
                
    }
    else {
        $("#HiddenAllpermission").val($("#HiddenAllpermission").val().replace(str + "/", ""));
        if ($("#HiddenDeletepermission").val() == "") {
            $("#HiddenDeletepermission").val(str + "/");
        }
        else {
            str = $("#HiddenDeletepermission").val() + "" + str + "/"
            $("#HiddenDeletepermission").val(str);
        }
                
    }
    $('#FullPermission').attr('checked', false);
          
}

function AddScreenPermission() {
    if ($("#HiddenAllpermission").val() == "")
    {
        ShowMessage(BootstrapDialog.TYPE_SUCCESS, 'Your Permission is successfully Added.');
        $('#loadingmessage').hide();
        return;
    }
    var RoleId = $("#RoleList :selected").val();
    var ModuleID = $("#ModuleList :selected").val();
    var str = $("#HiddenAllpermission").val();
            
    $.ajax({
        type: "POST",
        url: $URL._AddScreenPermission,
        data: '{str: "' + str + '",RoleId: ' + RoleId + ',ModuleID: ' + ModuleID + ' }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
                  
            if (data == "1")
            {
                $('#loadingmessage').hide();
                ShowMessage(BootstrapDialog.TYPE_SUCCESS, 'Your Permission is successfully Added.');

            }
            $("#HiddenAllpermission").val("");
        },
        failure: function (response) {
            alert(response.d);
        }
    });
}

function DeleteScreenPermission(str) {
            
    $('#loadingmessage').show();
           
    var RoleId = $("#RoleList :selected").val();
    var ModuleID = $("#ModuleList :selected").val();
    $.ajax({
        type: "POST",
        url: $URL._DeleteScreenPermission,
        data: '{str: "' + str + '",RoleId: ' + RoleId + ',ModuleID: ' + ModuleID + ' }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#HiddenDeletepermission").val("");

            showpopup = "1";
                 
        },
        failure: function (response) {
            alert(response.d);
        }
    });
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

function CheckBoxFill() {
    $('input[type=checkbox]').attr('checked', false);
    var exitsIDs = "";
    var RoleId = $("#RoleList :selected").val();
    var ModuleID = $("#ModuleList :selected").val();
    $.ajax({
        type: "Post",
        url: $URL._GetPermissionList,
        data: '{RoleId: ' + parseInt(RoleId) + ',ModuleID: ' + parseInt(ModuleID) + ' }',
        //   data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {

                $.each(data, function (i, item) {
                    var ctr = item.ActionName + "" + item.ScreenId + "" + item.ActionId;

                    var id = "." + ctr.toString().trim();
                    if ($("#HiddenExitspermission").val() == "") {
                        $("#HiddenExitspermission").val(exitsIDs + "/");
                    }
                    else {
                        str = $("#HiddenExitspermission").val() + "" + exitsIDs + "/"
                        $("#HiddenExitspermission").val(str);
                    }

                    $(id).prop("checked", true)

                });
            }
            else {
                BindGrid();
            }
        },
        failure: function (response) {
            alert(response.d);
        }
    });

}

function DeletePermission() {
    DeleteScreenPermission($("#HiddenDeletepermission").val());
    $("#HiddenDeletepermission").val("");
}

function BindGrid1() {
    
    $('#loadingmessage').show();
    $('input[type=checkbox]').attr('checked', false);
    var exitsIDs = "";
    var RoleId = $("#RoleList :selected").val();
    var ModuleID = $("#ModuleList :selected").val();
    $.ajax({
        type: "Post",
        url: $URL._GetAllScreenPermission,
        data: '{RoleId: ' + parseInt(RoleId) + ',ModuleID: ' + parseInt(ModuleID) + ' }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {
                $("#ButtonAdd").addClass("delShow");
                var j = "";
                $('#ActionGrid').html("");
                var table = '<table id="Actiongrid" class="gridtable">';
                table += '<tr>';
                table += '<th data-field="ActionId">Screen Name</th>';
                table += '<th data-field="Actions">Actions</th>';
                table += '</tr>';
                $.each(data, function (i, item) {

                    if (i == 0) {
                        table += '<tr style="border-style: solid; border-width: 1px;">';
                        j = item.ScreenName;
                        table += '<td class="visible"><input id="' + "hfv" + item.ScreenId + '" type="hidden" value="' + item.ScreenId + '" />' + item.ScreenName;
                        table += '</td>';
                        if (item.ActionId == 0) {
                            if (item.ActionName == "Custom") {

                            }
                            else {
                                table += '<td class="f2"><input id="' + item.ScreenId + "," + item.ActionId + "," + item.ActionName + '" class="' + item.ActionName + "" + item.ScreenId + "" + item.ActionId + '" type="checkbox" onclick="javascript:Selectcheckbox(this);" disabled="disabled" />' + item.ActionName
                            }
                        }
                        else {
                            table += '<td class="f2"><input id="' + item.ScreenId + "," + item.ActionId + "," + item.ActionName + '" class="full ' + item.ActionName + "" + item.ScreenId + "" + item.ActionId + '" type="checkbox" onclick="javascript:Selectcheckbox(this);"/>' + item.ActionName
                        }
                        table += '</td>';
                    }
                    else if (j.toString().toLocaleLowerCase() == item.ScreenName.toString().toLocaleLowerCase()) {

                        if (item.ActionId == 0) {
                            if (item.ActionName == "Custom") {

                            }
                            else {
                                table += '<td class="f2"><input id="' + item.ScreenId + "," + item.ActionId + "," + item.ActionName + '" class="' + item.ActionName + "" + item.ScreenId + "" + item.ActionId + '" type="checkbox" onclick="javascript:Selectcheckbox(this);" disabled="disabled" />' + item.ActionName
                            }
                        }
                        else {
                            table += '<td class="f2"><input id="' + item.ScreenId + "," + item.ActionId + "," + item.ActionName + '" class="full ' + item.ActionName + "" + item.ScreenId + "" + item.ActionId + '" type="checkbox" onclick="javascript:Selectcheckbox(this);"/>' + item.ActionName
                        }
                        table += '</td>';
                    }
                    else {
                        table += '</tr>';
                        j = item.ScreenName;
                        table += '<tr style="border-style: solid; border-width: 1px;">';
                        table += '<td class="visible"><input id="' + "hfv" + item.ScreenId + '" type="hidden" value="' + item.ScreenId + '" />' + item.ScreenName;
                        table += '</td>';
                        if (item.ActionId == 0) {
                            if (item.ActionName == "Custom") {

                            }
                            else {
                                table += '<td class="f2"><input id="' + item.ScreenId + "," + item.ActionId + "," + item.ActionName + '" class="' + item.ActionName + "" + item.ScreenId + "" + item.ActionId + '" type="checkbox" onclick="javascript:Selectcheckbox(this);" disabled="disabled" />' + item.ActionName
                            }
                        }
                        else {
                            table += '<td class="f2"><input id="' + item.ScreenId + "," + item.ActionId + "," + item.ActionName + '" class="full ' + item.ActionName + "" + item.ScreenId + "" + item.ActionId + '" type="checkbox" onclick="javascript:Selectcheckbox(this);"/>' + item.ActionName
                        }
                        table += '</td>';
                    }
                })
                table += '</tr>';
                table += '</table>';
                $('#ActionGrid').append(table);
                $('#loadingmessage').hide();
            }
            else {
                $('#ActionGrid').html("");
                var table = '<table id="Actiongrid" class="gridtable">';
                table += '<tr>';
                table += '<td>No Record Found!';
                table += '</td>';
                table += '</tr>';
                table += '</table>';
                $('#ActionGrid').append(table);
                $('#loadingmessage').hide();
            }
            GetModulePermission();
        },
        failure: function (response) {
            alert(response.d);
        }
    });

}

function AddFullPermission() {
    var RoleId = $("#RoleList :selected").val();
    var ModuleID = $("#ModuleList :selected").val();
    var FullPermission = false;
    if ($("#FullPermission").is(':checked')) {
        FullPermission = true;
    }
    $('#loadingmessage').show();
    $.ajax({
        type: "POST",
        url: $URL._CreateFullPermission,
        data: '{FullPermission: "' + FullPermission + '",RoleId: ' + RoleId + ',ModuleID: ' + ModuleID + ' }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $('#loadingmessage').hide();
                  
            ShowMessage(BootstrapDialog.TYPE_SUCCESS, data.ResponseText);

            $("#HiddenAllpermission").val("");
        },
        failure: function (response) {
            alert(response.d);
        }
    });

}

function GetModulePermission() {
    var RoleId = $("#RoleList :selected").val();
    var ModuleID = $("#ModuleList :selected").val();
    var count = 0;
    $("#HiddenAllpermission").val("");
    $('#loadingmessage').show();
    $.ajax({
        type: "POST",
        url: $URL._GetModulePermission,
        data: '{RoleId: ' + RoleId + ',ModuleID: ' + ModuleID + ' }',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            count = (data.length);
                 
            if (data.length > 0) {
                if (data[0].FullPermission.toString() == "true") {

                    $(".full").each(function () { //loop through each checkbox
                        this.checked = true;  //select all checkboxes with class "full" 
                        var str = this.id.toString();
                        if ($("#HiddenAllpermission").val() == "") {
                            $("#HiddenAllpermission").val(str + "/");
                        }
                        else {
                            str = $("#HiddenAllpermission").val() + "" + str + "/"
                            $("#HiddenAllpermission").val(str);
                        }
                    });
                    $('#FullPermission').prop('checked', true);
                }
                else {

                }
            }
        },
        failure: function (response) {
            alert(response.d);
                   
        }
    });
    if(count==0)
    {
        CheckBoxFill();
        $('#loadingmessage').hide();
    }
}

function DeleteFullPermission() {
    var RoleId = $("#RoleList :selected").val();
    var ModuleID = $("#ModuleList :selected").val();
    var FullPermission = false;
           
    $('#loadingmessage').show();
    $.ajax({
        type: "POST",
        url: $URL._DeleteModulePermission,
        data: '{ModuleID: ' + ModuleID + ',RoleId: ' + RoleId + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
                 
            AddScreenPermission();
                    
            $("#HiddenAllpermission").val("");
                 
        },
        failure: function (response) {
            alert(response.d);
        }
    });

}