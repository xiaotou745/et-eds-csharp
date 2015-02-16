using SuperManCore;
using SuperManCore.Common;
using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Business
{
    /// <summary>
    /// B端注册时提供的信息
    /// </summary>
    public class RegisterInfoModel
    {
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        public string CityId { get;set;}
        /// <summary>
        /// 电话号码
        /// </summary>
        public string phoneNo { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string passWord { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string verifyCode { get; set; }
    }
    public class RegisterInfoModelTranslator : TranslatorBase<business, RegisterInfoModel>
    {
        public static readonly RegisterInfoModelTranslator Instance = new RegisterInfoModelTranslator();

        public override RegisterInfoModel Translate(business from)
        {
            throw new NotImplementedException();
        }



        public override business Translate(RegisterInfoModel from)
        {
            var to = new business();
            //to.Id = Helper.generateCode(from.phoneNo,AppType.B端);
            to.City = from.city;
            to.Password = from.passWord;
            to.PhoneNo = from.phoneNo;            
            to.InsertTime = DateTime.Now;
            to.CityId = from.CityId;
            to.districtId = "0";

            //海底捞
            if (ConfigSettings.Instance.IsGroupPush)   //TODO 暂时有效
            {
                to.GroupId = 2;
                to.CommissionTypeId = 2;
            }
            else
            {
                to.CommissionTypeId = 1;
            }      
            
            return to;
        }
    }
    public enum CustomerRegisterStatus : int
    {
        Success = 0,
        [DisplayText("商户名称不能为空")]
        BusiNameEmpty,
        [DisplayText("城市不能为空")]
        cityIdEmpty,
        [DisplayText("手机号不能为空")]
        PhoneNumberEmpty,
        [DisplayText("密码不能为空")]
        PasswordEmpty,
        [DisplayText("验证码不正确")]
        IncorrectCheckCode,
        [DisplayText("昵称已被注册")]
        NickNameAlreadyRegistered,
        [DisplayText("手机号已被注册")]
        PhoneNumberRegistered,
        [DisplayText("您输入的的号码不存在,请检查并修改！")]   
        PhoneNumberNotExist,        
        [DisplayText("原平台商户Id不能为空")]   
        OriginalBusiIdEmpty,        
        [DisplayText("原平台商户Id重复")]
        OriginalBusiIdRepeat,
        [DisplayText("商户地址省市区地址不能为空")]   
        BusiAddressEmpty,
        [DisplayText("集团Id不能为空")]
        GroupIdEmpty,
        [DisplayText("请填写佣金类型")]
        CommissionTypeIdEmpty

    }
}