$(function () {
    //Donut Chart
    var donut = new Morris.Donut({
        element: 'sales-chart',
        resize: true,
        colors: ["#3c8dbc", "#f56954", "#00a65a"],
        data: [
          { label: "Download Sales", value: 12 },
          { label: "In-Store Sales", value: 30 },
          { label: "Mail-Order Sales", value: 20 }
        ],
        hideHover: 'auto'
    });
    $.ajax({
        url: "/api/UsersAPI/GetUsersForCharts",
        type: 'GET',
        dataType: 'json',
        async: false,
        success: function (data) {
            /* Morris.js Charts */
            // Sales chart
            var area = new Morris.Area({
                element: 'revenue-chart',
                resize: true,
                data: data.userdata,
                xkey: 'period',
                ykeys: ['users'],
                labels: ['Users'],
                lineColors: ['#3c8dbc'],
                hideHover: 'auto'
            });
            var data1 = [
              { y: '2014', a: 50, b: 90 },
              { y: '2015', a: 65, b: 75 },
              { y: '2016', a: 50, b: 50 },
              { y: '2017', a: 75, b: 60 },
              { y: '2018', a: 80, b: 65 },
              { y: '2019', a: 90, b: 70 },
              { y: '2020', a: 100, b: 75 },
              { y: '2021', a: 115, b: 75 },
              { y: '2022', a: 120, b: 85 },
              { y: '2023', a: 145, b: 85 },
              { y: '2024', a: 160, b: 95 }
            ];
            var bar = new Morris.Bar({
                element: 'bar-chart',
                data: data1,
                xkey: 'y',
                ykeys: ['a', 'b'],
                labels: ['Total Income', 'Total Outcome'],
                fillOpacity: 0.6,
                hideHover: 'auto',
                behaveLikeLine: true,
                resize: true,
                pointFillColors: ['#ffffff'],
                pointStrokeColors: ['black'],
                lineColors: ['gray', 'red']
            });
        },
        error: function (statusText) {
            var error = JSON.parse(statusText.responseText)
            console.log(error.exceptionMessage);
        }
    });

    //Fix for charts under tabs
    $('.box ul.nav a').on('shown.bs.tab', function () {
        area.redraw();
        bar.redraw();
        donut.redraw();
    });
});
