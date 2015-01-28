$(function () {
    var currentId;
    var currentName;
    //add 弹框
    $("#AddCategoryBtn").bind("click", function () {
        $("#txtAddCategoryName", $("#Close_addbox")).val("");
        adminjs.openwinbox('#Close_addbox');
    });

    //edit 弹框
    $(".EditCategoryBtn").bind("click", function () {
        currentId = $(this).closest("tr").attr("Categoryid");
        currentName = $(this).attr("CategoryName");
        $("#txtEditCategoryName", $("#Close_editbox")).val(currentName);
        adminjs.openwinbox('#Close_editbox');

    });

    //del 弹框
    $(".DeleteCategoryBtn").bind("click", function () {
        currentId = $(this).closest("tr").attr("Categoryid");
        adminjs.openwinbox('#Close_delbox');
    });

    //del请求
    $("#btnDel").click(function () {
        var paramaters = { "id": currentId };
        var url = "/SupplierDishCategory/Delete";
        $.post("/SupplierDishCategory/Delete", paramaters, function (data) {
            if (data) {
                window.location.href = window.location.href;
            }
        });
    });

    //编辑请求
    $("#btnEdit").click(function () {
        currentName = $('#txtEditCategoryName').val();
        var paramaters = { "id": currentId, "CategoryName": currentName };
        webExpress.utility.ajax.request
        (
        $.post("/SupplierDishCategory/SaveEdit", paramaters, function (data) {
            if (data) {
                window.location.href = window.location.href;
            }
        }));
    });

    //Add请求
    $("#btnAdd").click(function () {
        currentName = $('#txtAddCategoryName').val();
        normal("txtAddCategoryName", 2);
        var paramaters = { "CategoryName": currentName };
        $.post("/SupplierDishCategory/Add", paramaters, function (data) {
            if (data) {
                window.location.href = window.location.href;
            }
        });
    });
});