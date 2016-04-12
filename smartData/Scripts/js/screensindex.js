var form;

$(document).ready(function () {
    $("#HiddenHostName").val(window.location.origin);

    $("#ScreenName").keyup(function () {
        var count = $(this).val().length;
        var text = $(this).val();
        if(count>50)
        {
            var text1 = text.substring(0, 50);
            $(this).val(text1);
        }

    });

        
    $("#txtActionName").keyup(function () {
        var count = $(this).val().length;
        var text = $(this).val();
        if (count > 50) {
            var text1 = text.substring(0, 50);
            $(this).val(text1);
        }

    });

    
    var headers = {};
    // Set Custom filters for searching : Model Property name and search TxtBox Id names must be same
    var Param = {};
    $("#btnFilter").click(function () {
        var screenname = $("#screenName").val();
        var active = $("#Active").val();
        Param.clickBtn = true;
        form = $('#EditForm');
        if (screenname != "") {
            Param.screenName = screenname;
        }
        else {
            Param.screenName = "";
        }
            
        $('#grid').bootstrapTable('filterBy');
        Param.clickBtn = false;
    });
    $("#EditForm").on("submit", function (event) {
        if ($("#txtscreenName").val().trim() == "") {
            BootstrapDialog.alert("Please Enter Screen Name");
            $('#loadingmessage').hide();
            return false;
        }
        mycustomsubmit(event);
    });
    form = $('#EditForm');
    function mycustomsubmit(e) {
        $('#loadingmessage').show();

        if ($("#ModuleId").val().trim() == "Select Module") {
            $("#ModuleId").focus();
            BootstrapDialog.alert("Please Select Module Name");
            $('#loadingmessage').hide();
            return false;
        }

        e.preventDefault();
        $.ajax({
            cache: false,
            async: true,
            type: "POST",
            url: $("#HiddenHostName").val() + "/api/screenAPI/Editscreen",
            data: $('#EditForm').serialize(),//form.serialize(),
            success: function (data) {
                RefreshGrid();
                if(data == 'true'){
                    ShowMessage(BootstrapDialog.TYPE_SUCCESS, 'Your Record is successfully updated.');
                }
                else {
                    ShowMessage(BootstrapDialog.TYPE_SUCCESS, 'Sorry Duplicate Records exists.');
                }
                $("#myModal").modal('hide');
                $('#loadingmessage').hide();
                    
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

    var reqUrl = $("#HiddenHostName").val() + '/api/screenAPI/GetScreens';
        
    $('#grid').bootstrapTable({
        headers: headers,
        method: 'post',
        url: reqUrl,
        cache: false,
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
            field: 'ScreenName',
            title: 'Screen',
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
        
});