var str = "";
$(document).ready(function () {

    // 1st replace first column header text with checkbox   display: inline-block;

    if ($("#HiddenSuperAdmin").val().toString().toLocaleLowerCase() == ("True").toLocaleLowerCase()) {
        $("#SuperAdmin").css("display", "block");
    }
    else {
        $("#SuperAdmin").css("display", "none");
    }

    $("#checkableGrid th").each(function () {
        if ($.trim($(this).text().toString().toLowerCase()) === "{checkall}") {
            $(this).text('');
            $("<input/>", { type: "checkbox", id: "cbSelectAll", value: "" }).appendTo($(this));
            $(this).append("<span></span>");
        }
    });

    //2nd click event for header checkbox for select /deselect all
    $("#cbSelectAll").on("click", function () {
        // alert("SelectAll");
        var ischecked = this.checked;
        $('#checkableGrid').find("input:checkbox").each(function () {
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

            }
        });

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