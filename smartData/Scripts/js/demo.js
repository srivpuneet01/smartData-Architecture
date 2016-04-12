$(document).ready(function () {

    $.ajax({
        type: "post",
        url: $URL._GetAccessPermissionList,
        data: '{Screen: "Demo" }',
        //  data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {

                $.each(data, function (i, item) {
                    //if(item.ActionType=="1")
                    //{

                    //}
                    if (item.IsAllowPermission == "1") {
                        $(".isAllow").addClass("showButton");
                    }
                    if (item.IsAllowPermission == "0") {
                        $(".isAllow").addClass("hideButton");
                    }

                    if (item.ActionType == "2") {
                        $("#ButtonAdd").addClass("showButton");
                    }
                    if (item.ActionType == "3") {
                        $("#ButtonEdit").addClass("showButton");
                    }
                    if (item.ActionType == "4") {
                        $("#ButtonDelete").addClass("showButton");
                    }
                    if (item.ActionType == "5") {
                        if (item.ActionName == "Export") {
                            $("#ButtonExport").addClass("showButton");
                        }
                        if (item.ActionName == "Print") {

                            $("#ButtonPrint").addClass("showButton");
                        }
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

    $("#ButtonAdd").click(function () {
        $.ajax({
            type: "Post",
            url: $URL._AddDemo,
            //data: '{RoleId: ' + parseInt(RoleId) + ',ModuleID: ' + parseInt(ModuleID) + ' }',
            data: '{Screen: "new Screen112" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.length > 0) {

                    alert(data);

                }
                else {

                }
            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });
});