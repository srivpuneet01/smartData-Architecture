var form;
var Param = {};
var $table = $("#grid");
$("#btnFilter").click(function () {
    var fullname = $("#FullName").val();
    var socialsecurityno = $("#SocialSecurityNo").val();
    var homeaddress = $("#HomeAddress").val();
    var phone = $("#Phone").val();

    Param.clickBtn = true;
    form = $('#EditForm');
    if (fullname != "") {
        Param.FullName = fullname;
    }
    else {
        Param.FullName = "";
    }
    if (socialsecurityno != "") {
        Param.SocialSecurityNo = socialsecurityno;
    }
    else {
        Param.SocialSecurityNo = "";
    }
    if (homeaddress != "") {
        Param.HomeAddress = homeaddress;
    }
    else {
        Param.HomeAddress = "";
    }
    if (phone != "") {
        Param.Phone = phone;
    }
    else {
        Param.Phone = "";
    }
    $table.bootstrapTable('refresh');
    Param.clickBtn = false;
});

$("#EditForm").on("submit", function (event) {
    mycustomsubmit(event);
});

$("#btnUpdate").on("click", function (event) {
    UpdatePatient(event);
});

function UpdatePatient(e) {
    $('#loadingmessage').show();
    e.preventDefault();
    form = $('#FormID');
    $.ajax({
        cache: false,
        async: true,
        type: "POST",
        url: $("#HiddenHostName").val() + "/Patients/Patients/EditPatient",
        data: form.serialize(),
        success: function (data) {
            ShowMessage(BootstrapDialog.TYPE_SUCCESS, 'Your Record is successfully updated.');
            $('#loadingmessage').hide();
            $("#myModal").modal('hide');
            RefreshGrid();

        },
        error: function (request, error) {
            if (request.statusText == "CustomMessage") {
                $("#spanError").html(request.responseText)
                $('#loadingmessage').hide();
            }
        },
        headers: headers,

    });
    return false;
}

// Antiforgery Token
var token = '@TokenHeaderValue()';
var headers = {};
headers["RequestVerificationToken"] = token;
var reqUrl = $("#HiddenHostName").val() + '/api/PatientsAPI/GetPatients';

$('#grid').bootstrapTable({
    headers: headers,
    method: 'post',
    url: reqUrl,
    cache: true,
    height: 500,
    classes: 'table table-hover',
    customParams: Param,
    striped: true,
    pageNumber: 1,
    pagination: true,
    pageSize: 10,
    pageList: [5, 10, 20, 30],
    search: false,
    showColumns: true,
    showRefresh: true,
    sidePagination: 'server',
    minimumCountColumns: 2,
    showHeader: true,
    showFilter: false,
    smartDisplay: true,
    clickToSelect: true,
    rowStyle: rowStyle,
    toolbar: '#custom-toolbar',
    columns: [

            {
                field: 'FullName',
                title: 'Name',
                checkbox: false,
                type: 'search',
                sortable: true,
            }, {
                field: 'DateOfBirth',
                title: 'Date of Birth',
                type: 'search',
                formatter: DateFormatter,
                sortable: true,
            }, {
                field: 'SocialSecurityNo',
                title: 'Social Security No.',
                type: 'search',

                sortable: true,
            },
    {
        field: 'HomeAddress',
        title: 'Home Address',
        type: 'search',

        sortable: true,
    },
    {
        field: 'Phone',
        title: 'Phone No.',
        type: 'search',

        sortable: true,
    },
    {
        field: 'operate',
        title: 'Actions',


        clickToSelect: false,
        formatter: operateFormatter,
        events: operateEvents
    }],
    onSubmit: function () {
        var data = $('#filter-bar').bootstrapTableFilter('getData');
        console.log(data);
    },
    onLoadSuccess: function () {
        Addtitle();
    },
    onPageChange: function () {
        Addtitle();
    }
});

$("#FirstName").on("keydown", function (e) {
    return e.which !== 32;
});
$("#LastName").on("keydown", function (e) {
    return e.which !== 32;
});
$("#MiddleName").on("keydown", function (e) {
    return e.which !== 32;
});

function Addtitle() {
    $('.page-next').attr('Title', 'Next');
    $('.page-first').attr('Title', 'First');
    $('.page-last').attr('Title', 'Last');
    $('.page-pre').attr('Title', 'Previous');
}
function DateFormatter(value, row, index) {
    return [
     row.DateOfBirth = new Date(row.DateOfBirth).toLocaleDateString()
    ].join('');
}

// Edit and Delete Formattte links
function operateFormatter(value, row, index) {
    return [
        '<a id="edit"  class="edit ml10 isAllowEdit" href="javascript:void(0)" title="Edit">',
            '<i class="glyphicon glyphicon-edit"></i>',
        '</a>&nbsp;&nbsp;',
        '<a id="delete" class="remove ml10 isAllowDelete" href="javascript:void(0)" title="Remove">',
            '<i class="glyphicon glyphicon-remove"></i>',
        '</a>'
    ].join('');
}

// Link Events Edit and Delete
window.operateEvents = {
    'click .edit': function (e, value, row, index) {
        $('#loadingmessage').show();
        $.ajax({

            url: $("#HiddenHostName").val() + "/Patients/Patients/Edit",
            data: row,
            type: "GET",
            success: function (data) {
                $("#modalid").html(data);
                $("#myModal").modal('show');
                $('#loadingmessage').hide();
            },
            error: function () {
                $('#loadingmessage').hide();
            }
            ,
            headers: {
                'RequestVerificationToken': '@TokenHeaderValue()'
            }
        });
    },
    'click .remove': function (e, value, row, index) {
        BootstrapDialog.show({
            title: 'Confirmation',
            message: 'Are you sure you want to delete this record?',
            buttons: [{
                label: 'Yes',
                cssClass: 'btn-primary',
                action: function (dialogItself) {
                    $.ajax({
                        cache: false,
                        async: false,
                        type: "POST",
                        url: $("#HiddenHostName").val() + "/api/PatientsAPI/DeletePatient/" + row.PatientId,
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

var $table = $("#grid");
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
function AddNew() {
    //  window.location.href = "/Patients/Patients/Create";
    //  $("hdtext").text("Add Role");
    // $("#AddModal").modal('show');
    //window.location.href = "/Patients/Patients/Create";
    $("#AddModal").modal('show');
}

//Close Popup
function  ClosePopup()
{
    $("#AddModal").modal('hide');
}
//End
function LoadSuccess() {
    //$("#btnMassUpdate").show();
    //$("#ddlactivate").show();
    CheckPermission();
}

function CheckPermission()
{
    $.ajax({
        type: "post",
        url: $URL._GetAccessPermissionList,
        data: '{Screen: "Patients" }',
        //  data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {

                $.each(data, function (i, item) {
                    //if(item.ActionType=="1")
                    //{

                    //}
                         
                    if (item.IsAllowPermission == "0") {
                        $(".btnAdd").addClass("hideButton");
                    }

                    if (item.ActionType == "2") {
                        $(".btnAdd").addClass("showButton");
                    }
                    if (item.ActionType == "3") {
                        //setTimeout(function () {
                        //    //  $(".isAllowEdit").removeClass("hideButton");
                        //    $(".isAllowEdit").addClass("showButton");
                        //}, 3500);
                        $(".isAllowEdit").addClass("showButton");
                    }

                    if (item.ActionType == "4") {
                        //setTimeout(function () {
                        //    //   $("#btnMassUpdate").addClass("hidden");
                        //    // $(".isAllowEdit").removeClass("hideButton");
                        //    $(".isAllowDelete").addClass("showButton");
                        //}, 3500);
                        $(".isAllowDelete").addClass("showButton");
                    }

                    if (item.ActionType == "5") {
                        if (item.ActionName == "Export") {
                            $("#ButtonExport").addClass("showButton");
                        }
                        if (item.ActionName == "Print") {

                            $("#ButtonPrint").addClass("showButton");
                        }
                    }
                    if (item.IsAllowPermission == "1") {
                        $(".isAllow").addClass("showButton");
                        $(".isAllowEdit").addClass("showButton");
                        $(".btnAdd").addClass("showButton");
                        $(".isAllowDelete").addClass("showButton");
                    }

                });
            }
            else {

            }
        },
        failure: function (response) {
            alert(response.d);
        }
    });
}