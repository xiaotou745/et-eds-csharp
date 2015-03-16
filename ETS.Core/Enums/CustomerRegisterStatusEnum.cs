using ETS.Expand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Enums
{
    public enum CustomerRegisterStatusEnum : int
    {
        Success = 0,
        [DisplayText("商户名称不能为空")]
        BusiNameEmpty = 101,
        [DisplayText("城市不能为空")]
        cityIdEmpty = 102,
        [DisplayText("手机号不能为空")]
        PhoneNumberEmpty = 103,
        [DisplayText("密码不能为空")]
        PasswordEmpty = 104,
        [DisplayText("验证码不正确")]
        IncorrectCheckCode = 105,
        [DisplayText("昵称已被注册")]
        NickNameAlreadyRegistered = 106,
        [DisplayText("手机号已被注册")]
        PhoneNumberRegistered = 107,
        [DisplayText("您输入的的号码不存在,请检查并修改")]
        PhoneNumberNotExist = 108,
        [DisplayText("原平台商户Id不能为空")]
        OriginalBusiIdEmpty = 109,
        [DisplayText("原平台商户Id已注册")]
        OriginalBusiIdRepeat = 110,
        [DisplayText("商户地址省市区地址不能为空")]
        BusiAddressEmpty = 111,
        [DisplayText("集团Id不能为空")]
        GroupIdEmpty = 112,
        [DisplayText("请填写佣金类型")]
        CommissionTypeIdEmpty = 113

    }
}
