var form;
var token;
$(document).ready(function () {
    $("#hiddenMassUpdatecheckAll").val("");
    $("#hiddenMassUpdatecheckAllsts").val("");
    $("#HiddenHostName").val(window.location.origin);
    $("#btnAdd").click(function () {
        $("#AddModal").modal('show');
    });

    // Set Custom filters for searching : Model Property name and search TxtBox Id names must be same
    var Param = {};


    form = $('#EditForm');

    // Antiforgery Token
    token = '@TokenHeaderValue()';
    var headers = {};
    headers["RequestVerificationToken"] = token;
    var reqUrl = $("#HiddenHostName").val() + '/api/RolesAPI/GetUsers';
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
                  field: '',
                  title: '',
                  type: 'checkbox',
                  checkbox: true,
                  value: '',
                  clickToSelect: true,
                  align: 'left',
              },

        {
            field: 'RoleName',
            title: 'Role',
            type: 'search',
            sortable: true,
        },
            {
                field: 'ActiveDisplay',
                title: 'Active',
                visible: true,
                sortable: false,
                align: 'left',
                clickToSelect: false,
                disabled: true
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