$(function () {
    var id = 0;
    var status = 0;
    var isCloses = "";
    var sTime = "";
    var eTime = "";
    var supName = "";
    var UserName = "";
    //重置密码
    $(document).on('click', '.res', function () {
        adminjs.openwinbox('#Reset_qxbox');
        supName = $(this).attr("SupplierName");
        id = $(this).attr("SupplierId");
        UserName = $(this).attr("UserName");
        $("#SupName").val("您确定将  "+supName+" 的密码重置为123456吗？");
        return false;
    })
    $("#btnReset").click(function () {
        webExpress.utility.ajax.request("/Supplier/ResetPassWord", { id: id, UserName: UserName }, function (data) {
            adminjs.closewinbox('.add-openbox');
            if (data) {
                alert("重置密码成功");
                refresh(0);
            }
        })
    })
    //查看订单详情
    //$(document).on('click', "#linkView", function (orderId, businessName) {
    //    $("#orderNo").val(orderId).attr('disabled', 'disabled');
    //    adminjs.openwinbox('#FinishOrderShow');
    //    return false;
    //});
    $("#btnClose").click(function () {
        webExpress.utility.ajax.request("/Supplier/CloseSupplier", { id: id, status: status }, function (data) {
            adminjs.closewinbox('.add-openbox');
            if (data) {
                refresh(0);
            }
        })
    })
    //打烊设置
    $(document).on('click', ".dy", function () {
        adminjs.openwinbox('#DaYang_qxbox');
        id = $(this).attr("SupplierId");
        sTime = $(this).attr("BeginTime");
        eTime = $(this).attr("EenTime");
        if (sTime != "" && eTime != "") {
            $("#startdate").val(sTime.toString("yyyy-MM-dd"));
            $("#enddate").val(eTime.toString("yyyy-MM-dd"));
            isCloses = "update";
        } else {
            isCloses = "insert"
        }
    })
    $("#btnDaYang").click(function () {
        var startTime = $("#startdate").val();
        var endTime = $("#enddate").val();
        if (startTime == "" || endTime == "") {
            alert("请输入开始或结束日期");
            return false;
        }
        if (startTime < endTime) {
            alert("结束日期不能小于开始日期");
            return false;
        }
        webExpress.utility.ajax.request("/Supplier/DaYangSupplier", { id: id, isCloses: isCloses, BeginTime: startTime, EndTime: endTime }, function (data) {
            adminjs.closewinbox('.add-openbox');
            if (data) {
                refresh(0);
            }
        })
    })
   
})