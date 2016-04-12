

// Assign the page onload fucntion.
$(function () {
    pageonload();

    $("ul.PCollapse li>div").click(function () {
        if ($(this).next().css("display") == "none") {
            $(".divType").next().hide("normal");
            $(".divType").children(".mark_arrow").removeClass("expanded");
            $(".divType").children(".mark_arrow").addClass("collapsed");
            $(this).next().show("normal");
            $(this).children(".mark_arrow").removeClass("collapsed");
            $(this).children(".mark_arrow").addClass("expanded");
        }
    });




});

$('#DWTcontainer').hover(function () {
    $(document).bind('mousewheel DOMMouseScroll', function (event) {
        stopWheel(event);
    });
}, function () {
    $(document).unbind('mousewheel DOMMouseScroll');
});



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


        $("#myModal").modal('show');
        $("#RoleId").val(row.RoleId);
        $("#showRoleID").text(row.RoleId);
        $("#txtRoleName").val(row.RoleName);

        var prop = row.Active;

        $('#Active').prop('checked', prop);

    },

    'click .remove': function (e, value, row, index) {
        BootstrapDialog.show({
            message: 'Are you sure? You want to delete this record.',
            buttons: [{
                label: 'Ok',
                cssClass: 'btn-primary',
                action: function (dialogItself) {
                    $.ajax({
                        cache: false,
                        async: false,
                        type: "POST",
                        url: "http://localhost:55038/api/RolesAPI/DeleteRole/" + row.RoleId,
                        success: function (data) {
                            if (data == true) {
                                RefreshGrid();
                                dialogItself.close();
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
            }, {
                label: 'Close',
                cssClass: 'btn-danger',
                action: function (dialogItself) {
                    dialogItself.close();
                }
            }]
        });

    }
};

//function rowStyle(row, index) {
//    var classes = ['active', 'success', 'info', 'warning', 'danger'];

//    if (index % 2 === 0) {
//        return {
//            classes: classes[1]
//        };
//    }
//    return {};
//}

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
    $('#loadingmessage').show();
    if ($("#hiddenMassUpdatecheckAll").val() == "") {
        return false;
    }
    var roleNames = $("#hiddenMassUpdatecheckAll").val();
    var activtype = $("#ddlactivate").val();

    var flag = 0;
    if (activtype == "1") {
        flag = 2;
    }
    else {
        flag = 3;
    }
    if ($("#hiddenMassUpdatecheckAllsts").val() != "") {
        roleNames = "0";
        if (activtype == "1") {
            flag = 4;
        }
        else {
            flag = 5;
        }
    }

    $.ajax({
        type: "POST",
        url: "/Roles/Roles/MassUpdate",
        data: '{roleNames: "' + roleNames + '",ModifyBy: ' + flag + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            if (activtype == "1") {
                ShowMessage(BootstrapDialog.TYPE_SUCCESS, 'Records have been marked activated');

                RefreshGrid();

            }
            else {
                ShowMessage(BootstrapDialog.TYPE_SUCCESS, 'Records have been marked deactivated');

                RefreshGrid();

            }


            $("#hiddenMassUpdatecheckAll").val("");


        },
        failure: function (response) {
            alert(response.d);
        }
    });

}

function CheckUncheckAllFill(src) {
    //  alert(src);
    if (src == "allcheck") {
        $("#hiddenMassUpdatecheckAllsts").val("allcheck");
    }
    else {
        $("#hiddenMassUpdatecheckAllsts").val("Unallcheck");
    }
}


function checkRow(src) {
    var str = src.RoleId.toString();
    if ($("#hiddenMassUpdatecheckAll").val() == "") {
        $("#hiddenMassUpdatecheckAll").val(str + ",");
        // $("#hiddenMassUpdateUncheck").val($("#hiddenMassUpdateUncheck").val().replace(str.toString() + ",", " "));
    }
    else {
        str = $("#hiddenMassUpdatecheckAll").val() + "" + str + ","
        $("#hiddenMassUpdatecheckAll").val(str);
        //  $("#hiddenMassUpdateUncheck").val($("#hiddenMassUpdateUncheck").val().replace(str.toString() + ",", " "));
    }

}
function UncheckRow(src) {
    var str = src.RoleId.toString();
    if ($("#hiddenMassUpdatecheckAll").val() != "") {
        // $("#hiddenMassUpdatecheckAll").val(str + ",");
        $("#hiddenMassUpdateUncheck").val($("#hiddenMassUpdateUncheck").val().replace(str.toString() + ",", " "));
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

var data;
function ShowImagesInCanvas() {
    $("#loadingmessage").show();
    $.ajax({
        type: "POST",
        url:$URL._GetAllImagesByScannedRecordID,
        data: '{ScannedRecordID: ' + $("#HiddenEditMode").val() + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            if (data.length > 0) {
                //  alert("FillData");
                if (data[0].Type == "U") {
                    $("#User").val(data[0].AssociatedWith);
                    $("#User").addClass("ShowDDl");
                    $("#User").removeClass("HideDDl");
                    $("#Patient").addClass("HideDDl");
                    $('#RadioUser').attr('checked', true);
                    $('#RadioPatient').attr('checked', false);
                    $("#HiddenTypeID").val(data[0].AssociatedWith);
                    $("#HiddenType").val("U");
                }
                else {
                    $("#Patient").val(data[0].AssociatedWith);
                    $("#Patient").addClass("ShowDDl");
                    $("#Patient").removeClass("HideDDl");
                    $("#User").addClass("HideDDl");
                    $('#RadioUser').attr('checked', false);
                    $('#RadioPatient').attr('checked', true);
                    $("#HiddenTypeID").val(data[0].AssociatedWith);
                    $("#HiddenType").val("P");

                }
            }
            // $("#User").triggerHandler()
            LoadImagesInEditMode(data);
            //data = data;
            $("#loadingmessage").hide();
        },
        failure: function (response) {
            alert(response.d);
        }
    });
}
function pageonloadForView() {
    // initMessageBox(true);
}



function Addtitle() {
    $('.page-next').attr('Title', 'Next');
    $('.page-first').attr('Title', 'First');
    $('.page-last').attr('Title', 'Last');
    $('.page-pre').attr('Title', 'Previous');
}
function DateFormatter(value, row, index) {
    return [
     row.EffectiveDate = new Date(row.EffectiveDate).toLocaleDateString()
    ].join('');
}
// Edit and Delete Formattte links
function operateFormatter(value, row, index) {
    return [
        '<a id="edit" href="javascript:void(0)"  class="edit ml10"  title="View">',
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

        window.location = $URL._ScanHomeIndex + "?EditMode=" + row.ScannedRecordID;

    },

    'click .remove': function (e, value, row, index) {

        BootstrapDialog.show({
            title: 'Confirmation',
            message: 'Are you sure You want to delete this record?.',
            buttons: [{
                label: 'Yes',
                cssClass: 'btn-primary',
                action: function (dialogItself) {
                    $('#loadingmessage').show();

                    $.ajax({
                        type: "POST",
                        url: $URL._DeleteRecordsScan,
                        data: '{ScannedRecordID: "' + row.ScannedRecordID + '" }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            RefreshGrid();
                            $('#loadingmessage').hide();

                            dialogItself.close();
                            ShowMessage(BootstrapDialog.TYPE_SUCCESS, 'Your Record is successfully deleted.');
                            $("#myModal").modal('hide');

                        },
                        failure: function (response) {
                            alert(response.d);
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



function AddNew() {

    // $("#AddModal").modal('show');
    window.location = $URL._ScanHomeIndex;

}
//Mass Delete
function MassUpdate() {
    CheckUncheckAll();
}
function CheckUncheckAll() {

    if ($("#hiddenMassUpdatecheckAllsts").val() == "" && $("#hiddenMassUpdatecheckAll").val() == "") {
        BootstrapDialog.alert("Please select scanned record");
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
        flag = 2;
        msg = 'Are you sure you want to activate these records?';
    }
    else if (activtype == "2") {
        flag = 7;
        msg = 'Are you sure you want to delete these records?';
    }
    else {
        flag = 3;
        msg = 'Are you sure you want to deactivarte these records?';
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
                    url: $URL._MassDeleteScannedRecord,
                    data: '{ScannedRecordIDs: "' + roleNames + '",unScannedRecordIDs:"' + unCheckIds + '",active: ' + flag + '}',
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
                            //   location.reload();
                            RefreshGrid();
                        }
                        $("#hiddenMassUpdatecheckAllsts").val("");
                        $("#hiddenMassUpdatecheckAll").val("");
                        $("#hiddenMassUncheckIds").val("");
                        window.location.href = "";
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
    // alert(src);
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

    var str = src.ScannedRecordID.toString();
    if ($("#hiddenMassUncheckIds").val() != "") { $("#hiddenMassUncheckIds").val($("#hiddenMassUncheckIds").val().replace(str.toString() + ",", " ")); }

    if ($("#hiddenMassUpdatecheckAll").val() == "") {
        $("#hiddenMassUpdatecheckAll").val(str + ",");
        // $("#hiddenMassUpdateUncheck").val($("#hiddenMassUpdateUncheck").val().replace(str.toString() + ",", " "));
    }
    else {
        str = $("#hiddenMassUpdatecheckAll").val() + "" + str + ","
        $("#hiddenMassUpdatecheckAll").val(str);
        //  $("#hiddenMassUpdateUncheck").val($("#hiddenMassUpdateUncheck").val().replace(str.toString() + ",", " "));
    }

}
function UncheckRow(src) {

    var str = src.ScannedRecordID.toString();

    if ($("#hiddenMassUncheckIds").val() == "") { $("#hiddenMassUncheckIds").val(str + ","); }
    else {
        str = $("#hiddenMassUncheckIds").val() + "" + str + ","
        $("#hiddenMassUncheckIds").val(str);
    }

    if ($("#hiddenMassUpdatecheckAll").val() != "") {
        // $("#hiddenMassUpdatecheckAll").val(str + ",");
        $("#hiddenMassUpdatecheckAll").val($("#hiddenMassUpdatecheckAll").val().replace(str.toString() + ",", " "));
    }
}
function LoadSuccess() {
    $("#btnMassUpdate").show();
    $("#ddlactivate").show();


    $("#ddlactivate option:nth-child(1)").remove();
    //$("#ddlactivate option:nth-child(0)").remove();
    $("#ddlactivate option[value='0']").remove();

    $(".bootstrap-table").addClass("mrg");
    $('#loadingmessage').hide();
}


//List View
function showList(ScannedRecordID) {

    $.ajax({
        type: "POST",
        url: $URL._ScannedRecordDetailsByScannedRecordID,
        data: '{ScannedRecordID: ' + ScannedRecordID + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            LoadImagesInEditMode(data);

        },
        failure: function (response) {
            alert(response.d);
        }
    });
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

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 8)//back space
        return true;
    if (charCode < 45 || charCode > 57)//0-9
    {
        alert("Please Enter Only Numbers.");
        return false;
    }

    return true;

}



function pageonloadForView() {
    initMessageBox(true);
}


// Check if the control is fully loaded.
function Dynamsoft_OnReady() {
    // If the ErrorCode is 0, it means everything is fine for the control. It is fully loaded.
    DWObject = Dynamsoft.WebTwainEnv.GetWebTwain('dwtcontrolContainer');
    if (DWObject) {
        if (DWObject.ErrorCode == 0) {
            DWObject.LogLevel = 1;
            DWObject.IfAllowLocalCache = true;


            if (Dynamsoft.Lib.env.bWin)
                DWObject.MouseShape = false;

            var CurrentPathName, CurrentPath, strActionPage, strHTTPServer, downloadsource;
            if (location.port == '') {
                DWObject.HTTPPort = DynamLib.detect.ssl ? 443 : 80;
            } else {
                DWObject.HTTPPort = location.port;
            }

            DWObject.IfSSL = DynamLib.detect.ssl;

            CurrentPathName = unescape(location.pathname);	// get current PathName in plain ASCII
            CurrentPath = CurrentPathName.substring(0, CurrentPathName.lastIndexOf("/") + 1);
            strActionPage = CurrentPath + "online_demo_download.aspx"; //the ActionPage's file path
            strHTTPServer = location.hostname;
            downloadsource = strActionPage + "?iImageIndex=<%=strImageID%>&ImageName=<%=strImageName%>&ImageExtName=<%=strImageExtName%>";

            var sFun = function () {
                updatePageInfo();
            };
            var OnSuccess = function () {
                updatePageInfo();
            };

            var OnFailure = function (errorCode, errorString) {
                updatePageInfo();
            };

            //   DWObject.HTTPDownloadEx(strHTTPServer, downloadsource, <%=strImageFileType %>, OnSuccess, OnFailure);


            DWObject.RegisterEvent("OnPostTransfer", Dynamsoft_OnPostTransfer);
            DWObject.RegisterEvent("OnPostLoadfunction", Dynamsoft_OnPostLoadfunction);
            DWObject.RegisterEvent("OnPostAllTransfers", Dynamsoft_OnPostAllTransfers);
            DWObject.RegisterEvent("OnMouseClick", Dynamsoft_OnMouseClick);
            DWObject.RegisterEvent("OnImageAreaSelected", Dynamsoft_OnImageAreaSelected);
            DWObject.RegisterEvent("OnImageAreaDeSelected", Dynamsoft_OnImageAreaDeselected);
            DWObject.RegisterEvent("OnTopImageInTheViewChanged", Dynamsoft_OnTopImageInTheViewChanged);
        }
    }
}


$(function () {
    pageonloadForView();
});



var WebTWAIN;
var ua;
var em = "";
var seed;
var objEmessage;
window.onload = Pageonload;
//====================Page Onload  Start==================//

function Pageonload() {
    var imargin = (document.getElementById("dwtcontrolContainer").parentNode.offsetWidth - document.getElementById("dwtcontrolContainer").offsetWidth) / 2 + "px";
    document.getElementById("dwtcontrolContainer").style.marginLeft = imargin;
    document.getElementById("divMsg").style.display = "none";
    var strObjectFF = "";
    strObjectFF = " <embed style='display: inline' id='mainDynamicWebTWAINnotIE' type='Application/DynamicWebTwain-Plugin'";
    strObjectFF += " OnMouseClick='DynamicWebTwain_OnMouseClick'";
    strObjectFF += " OnMouseDoubleClick = 'DynamicWebTwain_OnMouseDoubleClick'";
    strObjectFF += " OnMouseRightClick = 'DynamicWebTwain_OnMouseRightClick'";
    strObjectFF += " OnTopImageInTheViewChanged = 'DynamicWebTwain_OnTopImageInTheViewChanged'";
    strObjectFF += " class='divcontrol' pluginspage='DynamicWebTWAIN/DynamicWebTwain.xpi'></embed>";
    var strObject = "";
    strObject = "   <param name='_cx' value='3784' />";
    strObject += "	<param name='_cy' value='4128' />";
    strObject += "	<param name='JpgQuality' value='80' />";
    strObject += "	<param name='Manufacturer' value='DynamSoft Corporation' />";
    strObject += "	<param name='ProductFamily' value='Dynamic Web TWAIN' />";
    strObject += "	<param name='ProductName' value='Dynamic Web TWAIN' />";
    strObject += "	<param name='VersionInfo' value='Dynamic Web TWAIN 6.3.1' />";
    strObject += "	<param name='TransferMode' value='0' />";
    strObject += "	<param name='BorderStyle' value='0' />";
    strObject += "	<param name='FTPUserName' value='' />";
    strObject += "	<param name='FTPPassword' value='' />";
    strObject += "	<param name='FTPPort' value='21' />";
    strObject += "	<param name='HTTPUserName' value='' />";
    strObject += "	<param name='HTTPPassword' value='' />";
    strObject += "	<param name='HTTPPort' value='80' />";
    strObject += "	<param name='ProxyServer' value='' />";
    strObject += "	<param name='IfDisableSourceAfterAcquire' value='0' />";
    strObject += "	<param name='IfShowUI' value='-1' />";
    strObject += "	<param name='IfModalUI' value='-1' />";
    strObject += "	<param name='IfTiffMultiPage' value='0' />";
    strObject += "	<param name='IfThrowException' value='0' />";
    strObject += "	<param name='MaxImagesInBuffer' value='1' />";
    strObject += "	<param name='TIFFCompressionType' value='0' />";
    strObject += "	<param name='IfFitWindow' value='-1' />";
    strObject += "	<param name='IfSSL' value='0' />";
    strObject += "	</object>";
    if (ExplorerType() == "IE" && navigator.userAgent.indexOf("Win64") != -1 && navigator.userAgent.indexOf("x64") != -1) {
        strObject = "<object id='mainDynamicWebTwainIE' codebase='DynamicWebTWAIN/DynamicWebTWAINx64.cab#version=6,3,1' class='divcontrol' " + "classid='clsid:FFC6F181-A5CF-4ec4-A441-093D7134FBF2' viewastext> " + strObject;
        var objDivx64 = document.getElementById("maindivIEx64");
        objDivx64.style.display = "inline";
        objDivx64.innerHTML = strObject;
        var obj = document.getElementById("maindivPlugin");
        obj.style.display = "none";
        WebTWAIN = document.getElementById("mainDynamicWebTwainIE");
    }
    else if (ExplorerType() == "IE" && (navigator.userAgent.indexOf("Win64") == -1 || navigator.userAgent.indexOf("x64") == -1)) {
        strObject = "<object id='mainDynamicWebTwainIE' codebase='DynamicWebTWAIN/DynamicWebTWAIN.cab#version=6,3,1' class='divcontrol' " + "classid='clsid:FFC6F181-A5CF-4ec4-A441-093D7134FBF2' viewastext> " + strObject;
        var objDivx86 = document.getElementById("maindivIEx86");
        objDivx86.style.display = "inline";
        objDivx86.innerHTML = strObject;
        var obj = document.getElementById("maindivPlugin");
        obj.style.display = "none";
        WebTWAIN = document.getElementById("mainDynamicWebTwainIE");
    }
    else {
        var objDivFF = document.getElementById("mainControlInstalled");
        objDivFF.innerHTML = strObjectFF;
        var obj = document.getElementById("maindivIE");
        obj.style.display = "none";
        var obj = document.getElementById("maindivPlugin");
        obj.style.display = "inline";
        WebTWAIN = document.getElementById("mainDynamicWebTWAINnotIE");
    }
    seed = setInterval(ControlDetect, 500);
}

function ExplorerType() {
    ua = (navigator.userAgent.toLowerCase());
    if (ua.indexOf("msie") != -1) {
        return "IE";
    }
    else {
        return "notIE";
    }
}

function pause() {
    clearInterval(seed);
}

function ControlDetect() {
    if (WebTWAIN.ErrorCode == 0) {
        pause();
        document.getElementById("PreviewMode").options.length = 0;
        document.getElementById("PreviewMode").options.add(new Option("1X1", 0));
        document.getElementById("PreviewMode").options.add(new Option("2X2", 1));
        document.getElementById("PreviewMode").options.add(new Option("3X3", 2));
        document.getElementById("PreviewMode").options.add(new Option("4X4", 3));
        document.getElementById("PreviewMode").options.add(new Option("5X5", 4));
        document.getElementById("PreviewMode").selectedIndex = 0;

        objEmessage = document.getElementById("emessage");

        objEmessage.ondblclick = function () {
            em = "";
            this.innerHTML = "";
        }
    }
    else {
        if (ua.match(/chrome\/([\d.]+)/) || ua.match(/opera.([\d.]+)/) || ua.match(/version\/([\d.]+).*safari/)) {
            document.getElementById("mainControlNotInstalled").style.display = "inline";
            document.getElementById("mainControlInstalled").style.display = "none";
        }
    }
    if (ExplorerType() == "IE") {
        WebTWAIN.attachEvent('OnMouseClick', DynamicWebTwain_OnMouseClick);
        WebTWAIN.attachEvent('OnMouseDoubleClick', DynamicWebTwain_OnMouseDoubleClick);
        WebTWAIN.attachEvent('OnMouseRightClick', DynamicWebTwain_OnMouseRightClick);
        WebTWAIN.attachEvent('OnTopImageInTheViewChanged', DynamicWebTwain_OnTopImageInTheViewChanged);
    }
    setTimeout(function () {
        WebTWAIN.MaxImagesInBuffer = 4096;
        WebTWAIN.MouseShape = true;
        var CurrentPathName = unescape(location.pathname);	// get current PathName in plain ASCII
        var CurrentPath = CurrentPathName.substring(0, CurrentPathName.lastIndexOf("/") + 1);
        var strActionPage = CurrentPath + "online_demo_download.aspx"; //the ActionPage's file path

        strHTTPServer = location.hostname;
        WebTWAIN.HTTPPort = location.port==""?80:location.port;
        var downloadsource = strActionPage + "?iImageIndex=<%=strImageID%>&ImageName=<%=strImageName%>&ImageExtName=<%=strImageExtName%>";
        WebTWAIN.HTTPDownloadEx(strHTTPServer, downloadsource, strImageFileType);
        if (CheckErrorString()) {
            UpdatePageInfo();
        }
    }, 500);
}
function CheckIfImagesInBuffer() {
    if (WebTWAIN.HowManyImagesInBuffer == 0) {
        em = em + "There is no image in buffer.<br />";
        objEmessage.innerHTML = em;
        return false;
    }
    else {
        return true;
    }
}
function CheckErrorString() {
    if (WebTWAIN.ErrorCode == 0) {
        em = em + "<b>" + WebTWAIN.ErrorString + "</b><br />";
        objEmessage.innerHTML = em;
        return true;
    }
    else {
        document.getElementById("divMsg").style.display = "block";
        setTimeout(function () {
            document.getElementById("divMsg").style.display = "none";
        }, 5000);
        if (WebTWAIN.ErrorCode == -2003) {
            var ErrorMessageWin = window.open("", "ErrorMessage", "height=500,width=750,top=0,left=0,toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no");
            ErrorMessageWin.document.writeln(WebTWAIN.HTTPPostResponseString);
        }
        em = em + "<b>" + WebTWAIN.ErrorString + "</b><br />";
        objEmessage.innerHTML = em;
        return false;
    }
}
function UpdatePageInfo() {
    document.getElementById("TotalImage").value = WebTWAIN.HowManyImagesInBuffer;
    document.getElementById("CurrentImage").value = WebTWAIN.CurrentImageIndexInBuffer + 1;
}
//====================Preview Group Start====================//
function btnFirstImage_onclick() {
    if (!CheckIfImagesInBuffer()) {
        return;
    }
    WebTWAIN.CurrentImageIndexInBuffer = 0;
    UpdatePageInfo();
    if (CheckErrorString()) {
        return;
    }
}
function btnPreImage_onclick() {
    if (!CheckIfImagesInBuffer()) {
        return;
    }
    else if (WebTWAIN.CurrentImageIndexInBuffer == 0) {
        return;
    }
    WebTWAIN.CurrentImageIndexInBuffer = WebTWAIN.CurrentImageIndexInBuffer - 1;
    UpdatePageInfo();
    if (CheckErrorString()) {
        return;
    }
}
function btnNextImage_onclick() {
    if (!CheckIfImagesInBuffer()) {
        return;
    }
    else if (WebTWAIN.CurrentImageIndexInBuffer == WebTWAIN.HowManyImagesInBuffer - 1) {
        return;
    }
    WebTWAIN.CurrentImageIndexInBuffer = WebTWAIN.CurrentImageIndexInBuffer + 1;
    UpdatePageInfo();
    if (CheckErrorString()) {
        return;
    }
}
function btnLastImage_onclick() {
    if (!CheckIfImagesInBuffer()) {
        return;
    }
    WebTWAIN.CurrentImageIndexInBuffer = WebTWAIN.HowManyImagesInBuffer - 1;
    UpdatePageInfo();
    if (CheckErrorString()) {
        return;
    }
}
function btnRemoveCurrentImage_onclick() {
    if (!CheckIfImagesInBuffer()) {
        return;
    }
    WebTWAIN.RemoveImage(WebTWAIN.CurrentImageIndexInBuffer);
    if (WebTWAIN.HowManyImagesInBuffer == 0) {
        document.getElementById("TotalImage").value = WebTWAIN.HowManyImagesInBuffer;
        document.getElementById("CurrentImage").value = "";
        return;
    }
    else {
        UpdatePageInfo();
    }
    if (CheckErrorString()) {
        return;
    }
}
function btnRemoveAllImages_onclick() {
    if (!CheckIfImagesInBuffer()) {
        return;
    }
    WebTWAIN.RemoveAllImages();
    document.getElementById("TotalImage").value = "0";
    document.getElementById("CurrentImage").value = "";
    if (CheckErrorString()) {
        return;
    }
}
function slPreviewMode() {
    WebTWAIN.SetViewMode(parseInt(document.getElementById("PreviewMode").selectedIndex + 1), parseInt(document.getElementById("PreviewMode").selectedIndex + 1));
    if (CheckErrorString()) {
        return;
    }
}
//====================Preview Group End====================//
var imageindex;
var imageindex2;
function DynamicWebTwain_OnMouseClick(index) {
    imageindex = index;
    document.getElementById("CurrentImage").value = index + 1;
    if (CheckErrorString()) {
        return;
    }
}

function DynamicWebTwain_OnMouseRightClick(index2) {
    if (!CheckIfImagesInBuffer()) {
        return;
    }
    var StrextraInfo;
    StrextraInfo = "Mouse X coordinate: " + WebTWAIN.MouseX + " Mouse Y coordinate: " + WebTWAIN.MouseY;
    document.getElementById("extraInfo").innerHTML = StrextraInfo;
    setTimeout(function () {
        document.getElementById("extraInfo").innerHTML = "";
    }, 6000);
    imageindex2 = index2;
    if (imageindex != imageindex2) {
        WebTWAIN.SwitchImage(imageindex, imageindex2);
    }
    if (CheckErrorString()) {
        return;
    }
}

function DynamicWebTwain_OnMouseDoubleClick() {
    var StrextraInfo;
    StrextraInfo = "Image Width: " + WebTWAIN.GetImageWidth(WebTWAIN.CurrentImageIndexInBuffer) +
    " Image Height: " + WebTWAIN.GetImageHeight(WebTWAIN.CurrentImageIndexInBuffer) +
    " Image Bit Depth: " + WebTWAIN.GetImageBitDepth(WebTWAIN.CurrentImageIndexInBuffer);
    document.getElementById("extraInfo").innerHTML = StrextraInfo;
    setTimeout(function () {
        document.getElementById("extraInfo").innerHTML = "";
    }, 6000);
    if (CheckErrorString()) {
        return;
    }
}

function DynamicWebTwain_OnTopImageInTheViewChanged(index) {
    document.getElementById("CurrentImage").value = index + 1;
    if (CheckErrorString()) {
        return;
    }
}
