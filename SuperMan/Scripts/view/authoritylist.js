$(function () {
    var currentId;
    var currentName;

    var _orderView=false;
    var _supermanView = false;
    var _supermanCheck = false;
    var _supermanclear = false;
    var _businessView = false;
    var _businessCheck = false;
    var _subsidySet = false;
    //设置权限弹出框
    $(".setAuthority").bind("click", function () {        
        currentId = $(this).closest("tr").attr("id");
        var accountId = { "id": currentId };
    
        $.get("/AuthorityManager/AuthorityEdit", accountId, function (data) {
            $("#_AuthorityDiv").html(data);
            adminjs.openwinbox('#AuthorityManagerShow');
        });

    });
    //添加用户 
    //$("#btnAddAccount").click(function () {

    //    $(".AddAccountfrom").validate({
    //        rules: {
    //            accountName: {
    //                required: true
    //            },
    //            loginName: {
    //                required: true
    //            },
    //            password: {
    //                required: true,
    //                minlength: 6
    //            }

    //        },
    //        messages: {
    //            accountName: {
    //                required: "帐号不能为空"
    //            },
    //            loginName: {
    //                required: "登录不能为空"
    //            },
    //            password: {
    //                required: "密码不能为空",
    //                minlength: "密码长度必须大于等于6位"
    //            }
    //        },
    //        submitHandler: function (form) {
    //            var accountName = $('#accountName').val();
    //            var loginName = $('#loginName').val();
    //            var password = $('#password').val();
    //            var groupId = $('#AddGroupId').val();
    //            var paramaters = { "accountName": accountName, "loginName": loginName, "password": password, "GroupId": groupId };
    //            var url = "/AuthorityManager/Add";
    //            $.ajax({
    //                type: 'POST',
    //                url: url,
    //                data: paramaters,
    //                success: function (result) {
    //                    if (result.IsSuccess) {
    //                        window.location.href = "/AuthorityManager/AuthorityManager";
    //                    } else {
    //                        alert(result.Message);
    //                    }
    //                }
    //            });
    //        }
    //    });
    //});
    // close box 
    $(document).on("click",".J_closebox" ,function () {
        adminjs.closewinbox('.add-openbox');
        return false;
    });

    /*配置权限保存功能*/
    $(document).on("click", "#btnSetAuthority",function(){
    // $("#btnSetAuthority").bind("click", function () {
        var id = currentId;
        var orderView = $('#ck_orderView').is(':checked');
        var supermanView = $('#ck_supermanView').is(':checked');
        var supermanCheck = $('#ck_supermanCheck').is(':checked');
        var supermanclear = $('#ck_supermanclear').is(':checked');
        var businessView = $('#businessView').is(':checked');
        var businessCheck = $('#businessCheck').is(':checked');
        var subsidySet = $('#subsidySet').is(':checked');
    
        var auids = new Array();
        $('#AuthorityManagerShow input[type="checkbox"]:checked').each(function() {
            // alert($(this).attr("authorityfuncid"));
            auids.push($(this).attr("authorityfuncid"));
        }); 
        var AuthorityListModel = { "id": id, "auths": auids };
        $.post("/AuthorityManager/saveAuthority", AuthorityListModel, function (data) {
            if (data) {
                window.location.href = window.location.href;
            }
            else {
                alert(data);
            }
        });
    });
    //修改密码弹出框
    $(".modifyPwd").bind("click", function () {
        currentId = $(this).closest("tr").attr("id");
        $("#hddsupplierdishid").val(currentId);
        adminjs.openwinbox('#ModifyPwdShow');
    });
    //修改密码
    $("#btnModifyPwd").click(function () {
        $("#ModifyPwdForm").validate({

            rules: {
                modifypassword: {
                    required: true,
                    minlength: 6
                },
                confirepassword: {
                    required: true,
                    minlength: 6,
                    equalTo: "#modifypassword"
                }

            },
            messages: {
                modifypassword: {
                    required: "密码不能为空",
                    minlength: "密码长度必须大于等于6位"
                },
                confirepassword: {
                    required: "确认密码不能为空",
                    minlength: "密码长度必须大于等于6位",
                    equalTo: "两次密码不一致"
                }
            },
            submitHandler: function (form) {
                var id = $("#hddsupplierdishid").val();
                var modifypassword = $('#modifypassword').val();
                var confirepassword = $('#confirepassword').val();
                var paramaters = { "id": id, "modifypassword": modifypassword };
                var url = "/AuthorityManager/ModifyPassword";
                $.ajax({
                    type: 'POST',
                    url: url,
                    data: paramaters,
                    success: function (result) {
                        if (result.IsSuccess) {
                            alert(result.Message);
                            window.location.href = "/AuthorityManager/AuthorityManager";
                        } else {
                            alert(result.Message);
                        }
                    }
                });
            }
        });
        //查看
        $(".accountView").bind("click", function () {

        });
        //删除
        $(".accountDel").bind("click", function () {
            var id = $(this).closest("tr").attr("id");
            var paramaters = { "id": id };
            var url = "/AuthorityManager/Delete";
            $.ajax({
                type: 'POST',
                url: url,
                data: paramaters,
                success: function (result) {
                    if (result.IsSuccess) {
                        window.location.href = "/AuthorityManager/AuthorityManager";
                    } else {
                        alert(result.Message);
                    }
                }
            });
        });
    })
    //查看
    $(".accountView").bind("click", function () {

    });
    //删除
    $(".accountDel").bind("click", function () {
        
        var id = $(this).closest("tr").attr("id");
        var paramaters = { "id": id };
        var url = "/AuthorityManager/Delete";
        $.ajax({
            type: 'POST',
            url: url,
            data: paramaters,
            success: function (result) {
                if (result.IsSuccess) {
                    window.location.href = "/AuthorityManager/AuthorityManager";
                } else {
                    alert(result.Message);
                }
            }
        });
    });

});