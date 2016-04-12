var str = "";

$(document).ready(function () {
    // 1st replace first column header text with checkbox
    ($("#HiddenDeleteIds").val(""));
    ($("#RoleIDs").val(""));
    ($(".form-group #RoleIDs").val());
    $("#HiddenUserId").val("");
    $("#checkableGrid th").each(function () {
        if ($.trim($(this).text().toString().toLowerCase()) === "{checkall}") {
            $(this).text('');
            $("<input/>", { type: "checkbox", id: "cbSelectAll", value: "" }).appendTo($(this));
            $(this).append("<span>Roles</span>");
        }
    });

    $(".op #cbSelectAll").on("click", function () {
        var ischecked = this.checked;
        str = "";
        $("#RoleIDs").val("");
        $('.op #checkableGrid').find("input:checkbox").each(function () {
            this.checked = ischecked;
            if (ischecked) {
                str = (this.value.toString());
                if ($("#RoleIDs").val() == "") {
                    $("#RoleIDs").val(str + ",");
                }
                else {
                    str = $("#RoleIDs").val() + str + ",";
                    $("#RoleIDs").val(str);
                }
            }
            else {
                $("#RoleIDs").val("");
                str = (this.value.toString());
                if ($(".op #HiddenDeleteIds").val() == "") {
                    $(".op #HiddenDeleteIds").val(str + ",");
                }
                else {
                    str = $(".op #HiddenDeleteIds").val() + str + ",";
                    $(".op #HiddenDeleteIds").val(str);
                }
            }
        });
        $(".form-group #RoleIDs").val($("#RoleIDs").val());
    });

    //3rd click event for checkbox of each row
    $("input[name='ids']").click(function () {

        var totalRows = $("#checkableGrid td :checkbox").length;
        var checked = $("#checkableGrid td :checkbox:checked").length;
        if (checked == totalRows) {
            $("#checkableGrid").find("input:checkbox").each(function () {
                this.checked = true;
            });
        }
        else {
            $("#cbSelectAll").removeAttr("checked");
        }
    });

});