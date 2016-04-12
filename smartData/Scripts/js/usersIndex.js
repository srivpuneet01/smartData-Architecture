var form;
var Param = {};
var token;
var saveid = 0;
$(document).ready(function () {
    $("#HiddenHostName").val(window.location.origin);
    $("#hiddenMassUpdatecheckAll").val("");
    $("#hiddenMassUpdatecheckAllsts").val("");
    setTimeout(function () {
        $("#btnMassUpdate").show();
        $("#ddlactivate").show();
    }, 2500);

    var headers = {};

    // Set Custom filters for searching : Model Property name and search TxtBox Id names must be same
          
    form = $('#EditForm');
          
    if (saveid == 0) {
    }
    var reqUrl = $("#HiddenHostName").val() + '/api/UsersAPI/GetUsers';
           
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
                  title: 'SelectAll',
                  type: 'checkbox',
                  checkbox: true,
                  value: '',
                  clickToSelect: true,
                  align: 'left',
              },
            {
                field: 'UserId',
                title: 'User ID',
                type: 'search',
                visible: false,
                sortable: true

            },
       {
           field: 'FirstName',
           title: 'First Name',
           type: 'search',
           sortable: true,
           formatter: function (value, row, index) {
               if (row.FirstName != "" && row.FirstName != null) {
                   return '<a id="edit"  class="edit ml10" href="javascript:ShowEditPopup(' + row.UserId + ')" title="Edit">' + row.FirstName + '</a>'
               }
                       
           }
       }, {
           field: 'LastName',
           title: 'Last Name',
           type: 'search',

           sortable: true,
                  
       }, {
           field: 'Email',
           title: 'Email',
           type: 'search',

           sortable: true,
           formatter: function (value, row, index) {
               if (row.FirstName != "" && row.FirstName != null) {
                   return '<a id="email" href="mailto:' + row.Email + '" title="Email">' + row.Email + '</a>'
               }

           }
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