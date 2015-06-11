var adminjs = new adminglass(); //实例化后台类
$(document).ready(function () {
    $("#btnAdd").on('click', function () {
        $.ajax({
            type: 'POST',
            url: "/DataManager/edit",
            data: {},
            success: function (data) {
                $('#divshow').html(data);
                adminjs.openwinbox('#AddDivShow');
            }
        });
    });
    window.location.hash = '';
});

//添加  确认按钮
$("#btnAddSure").live('click', function () {
    var _Name = $("#_Name").val();
    var _Executetime = $('#_Executetime').val();
    var _ReceiveEmail = $('#_ReceiveEmail').val();
    var _SqlText = $('#_SqlText').val();
    var _IsEnable = $('#_IsEnable').val();
    if (_Name == "" || _Executetime == "" || _ReceiveEmail == "" || _SqlText == "") {
        alert("参数不能为空!");
        return;
    }
    var pars = {
        "Name": _Name, "Executetime": _Executetime, "ReceiveEmail": _ReceiveEmail, "SqlText": _SqlText,
        "IsEnable": _IsEnable
    };
    var url = "/DataManager/Add";
    $.ajax({
        type: 'POST',
        url: url,
        data: pars,
        success: function (data) {
            if (data.Result == "success") {
                alert("添加成功!");
                window.location.href = "/DataManager/index";
            } else {
                alert("添加失败!");
            }
        }
    });
});

//编辑  确认按钮
$("#btnEditSure").live('click', function () {
    var _Name = $("#_Name").val();
    var _Executetime = $('#_Executetime').val();
    var _ReceiveEmail = $('#_ReceiveEmail').val();
    var _SqlText = $('#_SqlText').val();
    var _IsEnable = $('#_IsEnable').val();
    var _Id = $('#_Id').val();
    if (_Name == "" || _Executetime == "" || _ReceiveEmail == "" || _SqlText == "") {
        alert("参数不能为空!");
        return;
    }
    var pars = {
        "Name": _Name, "Executetime": _Executetime, "ReceiveEmail": _ReceiveEmail, "SqlText": _SqlText,
        "IsEnable": _IsEnable,"Id":_Id
    };
    var url = "/DataManager/DoEdit";
    $.ajax({
        type: 'POST',
        url: url,
        data: pars,
        success: function (data) {
            if (data.Result == "success") {
                alert("编辑成功!");
                window.location.href = "/DataManager/index";
            } else {
                alert("编辑失败!");
            }
        }
    });
});

//关闭弹层
$('#btnCanel').live("click",function () {
    adminjs.closewinbox('.add-openbox');
    return false;
});

//删除事件  
function deleteModel(id) {
    if (confirm("确认删除吗？")) {
        var url = "/DataManager/delete?Id="+id;
        $.ajax({
            type: 'POST',
            url: url,
            data: {},
            success: function (data) {
                if (data.Result == "success") {
                    alert("删除成功!");
                    window.location.href = "/DataManager/index";
                } else {
                    alert("删除失败!");
                }
            }
        });
    }
}

//删除事件  
function Edit(id) {
    $.ajax({
        type: 'POST',
        url: "/DataManager/edit?id=" + id,
        data: {},
        success: function (data) {
            $('#divshow').html(data);
            adminjs.openwinbox('#AddDivShow');
        }

    });

}