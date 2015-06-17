var adminjs = new adminglass(); //实例化后台类
$(document).ready(function () {
    $('#txtPubDateStart').datepicker();
    $('#txtPubDateEnd').datepicker();
    window.location.hash = '';
 
});


//取消发布
function Canel(id) {
    if (confirm("您确认取消发布吗？")) {
        $.post("/messagemanager/CanelMessage?id=" + id, function (data) {
            if (data == true) {
                alert("取消发布成功！");
                $("#btnSearch").click();
            } else {
                alert("取消发布失败，请刷新列表查看当前消息状态是否允许取消！");
            }
        });
    }
}

//详情
function detail(id) {
    $.ajax({
        type: 'POST',
        url: "/messagemanager/detail?messageId="+id,
        data: {},
        success: function (data) {
            $('#divshow').html(data);
            adminjs.openwinbox('#AddDivShow');
        }
    });
}


//关闭弹层
$('#btnCanel').live("click", function () {
    adminjs.closewinbox('.add-openbox');
    return false;
});