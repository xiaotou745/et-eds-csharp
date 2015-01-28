var adminjs = new adminglass(); //实例化后台类
var _searchCriteria = { PagingRequest: { PageIndex: 0, PageSize: 15 } };
var userId = 0;
//搜索
$(function () {
   
    $("#btnSearch").click(function () {

        if ($("#city").val() == "" && $("#txtSupplierName").val() == "" ) {
            return false
        }
        _searchCriteria = {
            SupplierName: $("#txtSupplierName").val(),
            CityId: $("#city").val(),
            PagingRequest: { PageIndex: 0, PageSize: 15 }
        };

        refresh(0);
    })
    //修改密码
    $(document).on('click', ".EditPassWord", function () {
        adminjs.openwinbox('#PassWord_qxbox');
        userId = $(this).attr("UserId");
        return false;
    });
    $("#btnEdit").click(function () {
        $(".nr_box").validate({

            rules: {
                oldPassWord: {
                    required: true
                },
                newPassWord: {
                    required: true,
                    minlength: 6
                },
                oncePassWord: {
                    required: true,
                    minlength: 6,
                    equalTo: "#newPassWord"
                }
               
            },
            messages: {
                oldPassWord: {
                    required: "旧密码不能为空"                       
                },
                newPassWord:{
                    required: "新密码不能为空",
                    minlength: "密码长度必须大于等于6位"
                },
                oncePassWord:{
                    required: "新密码不能为空",
                    minlength: "密码长度必须大于等于6位",
                    equalTo: "两次密码不一致"
                }
            },
            submitHandler: function (form) {
                var oldPassWord = $("#oldPassWord").val();
                var newPassWord = $("#newPassWord").val();
                var oncePassWord = $("#oncePassWord").val();
                $.get("/Supplier/IsSamePassWord", { UserId: userId, PassWord: oldPassWord }, function (data) {
                    if (data) {
                        adminjs.closewinbox('.add-openbox');
                        $.get("/Supplier/EditPassWord", { UserId: userId, PassWord: newPassWord }, function (data) {
                            if (data) {
                                alert("密码修改成功");
                                refresh(0);
                            }
                        })
                    } else {
                        alert("原密码输入错误");
                    }
                })
            }
    });
})
});
//分页
art.ui.control.Pager.enablePaging(document, refresh);
function refresh(pageIndex) {
    var url = "/Supplier/AccountList";

    if (pageIndex !== undefined) {
        _searchCriteria.PagingRequest.PageIndex = pageIndex;
    }
    webExpress.utility.ajax.request(url, _searchCriteria,
          function (data) {
              $("#dataList").html(data);
          });
}

//关闭弹层
$('.J_closebox').click(function () {
    adminjs.closewinbox('.add-openbox');
    return false;
});