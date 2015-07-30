using ETS.Expand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Enums
{
    /// <summary>
    /// 登陆状态接口
    /// </summary>
    public enum LoginModelStatus
    {
        Success = 1,
        [DisplayText("用户名或密码错误")]
        InvalidCredential = 0,
        [DisplayText("您当前登录的次数大于10，请5分钟后重试")]
        CountError=-10
    }  

    public enum ForgetPwdStatus
    {
        Success,
        [DisplayText("修改密码失败")]
        FailedModifyPwd,
        [DisplayText("新密码不能为空")]
        NewPwdEmpty,
        [DisplayText("用户不存在")]
        ClienterIsNotExist,
        [DisplayText("验证码不能为空")]
        checkCodeIsEmpty,
        [DisplayText("验证码错误")]
        checkCodeWrong,
        [DisplayText("您要找回的密码正是当前密码")]
        PwdIsSave,
    }
    
    public enum UploadIconStatus
    {
        [DisplayText("成功")]
        Success = 1,
        [DisplayText("未传过来任何变量")]
        NOFormParameter,
        [DisplayText("无效的用户")]
        InvalidUserId,
        [DisplayText("无效的订单号")]
        InvalidOrderId,
        [DisplayText("真实姓名不能为空")]
        TrueNameEmpty,
        /// <summary>
        /// 无效的文件格式
        /// </summary>
        [DisplayText("无效的文件格式")]
        InvalidFileFormat,
        /// <summary>
        /// 图片的尺寸最小为150px*150px
        /// </summary>
        [DisplayText("图片的尺寸最小为150px*150px")]
        InvalidImageSize,
        [DisplayText("上传图片失败")]
        UpFailed,
        [DisplayText("订单状态已完成且小票已全部上传不能删除")]
        DeleteFailed,
        [DisplayText("未找到该订单请联系客服")]
        CannotFindOrder,
        [DisplayText("删除失败请联系客服")]
        DeleteExcepiton,
        [DisplayText("骑士编号无效")]
        ClienterIdInvalid,
        [DisplayText("小票地址无效")]
        ReceiptAddressInvalid,
        [DisplayText("请先上传")]
        FirstUpload,
        [DisplayText("请传递版本号")]
        NoVersion = 100,
        [DisplayText("子订单ID无效")]
        NoOrderChildId = 101
    }    



    public enum UserStatus
    {
        Success,
        [DisplayText("获取用户状态失败")]
        Error
    }      
  

    /// <summary>
    /// 骑士是否绑定了商户（0：否 1：是）  或者 商户是否绑定了骑士（0：否 1：是）
    /// </summary>
    public enum IsBindBC
    {
        /// <summary>
        /// 最新订单
        /// </summary>
        [DisplayText("是")]
        Yes = 1,
        /// <summary>
        /// 附近订单
        /// </summary>
        [DisplayText("否")]
        No = 0,
    }
}
