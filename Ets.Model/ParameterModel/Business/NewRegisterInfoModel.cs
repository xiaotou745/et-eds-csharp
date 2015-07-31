using System;
using ETS.Expand;
using ETS.Util;
using Ets.Model.DataModel.Business;

namespace Ets.Model.ParameterModel.Business
{
    /// <summary>
    /// B端第三方商家注册类
    /// </summary> 
    public class NewRegisterInfoModel
    {
        /// <summary>
        /// 商户名称
        /// </summary> 
        public string B_Name { get; set; }
        /// <summary>
        /// 原平台商户Id
        /// </summary>
        public int B_OriginalBusiId { get; set; }
        public string B_Password { get; set; }
        /// <summary>
        /// 集团Id
        /// </summary>
        public int B_GroupId { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string B_IdCard { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// 电话号码2
        /// </summary>
        public string PhoneNo2 { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string B_Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string B_City { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string B_Area { get; set; }
        /// <summary>
        /// 街道地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string B_ProvinceCode { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string B_CityCode { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string B_AreaCode { get; set; }
        /// <summary>
        /// 商户所在区域经度
        /// </summary>
        public double B_Longitude { get; set; }
        /// <summary>
        /// 商户所在区域纬度
        /// </summary>
        public double B_Latitude { get; set; }
        /// <summary>
        /// 佣金类型Id
        /// </summary>
        public int CommissionTypeId { get; set; }
        /// <summary>
        /// 外送费
        /// </summary>
        public decimal DistribSubsidy { get; set; }
    }
    public class NewRegisterInfoModelTranslator : TranslatorBase<BusinessModel, NewRegisterInfoModel>
    {
        public static readonly NewRegisterInfoModelTranslator Instance = new NewRegisterInfoModelTranslator();

        public override NewRegisterInfoModel Translate(BusinessModel from)
        {
            throw new NotImplementedException();
        }



        public override BusinessModel Translate(NewRegisterInfoModel from)
        {
            var to = new BusinessModel();
            to.Province = from.B_Province;
            to.ProvinceCode = from.B_ProvinceCode.Trim();


            to.CityCode = from.B_CityCode;
            to.CityId = from.B_CityCode.Trim();
            to.City = from.B_City;

            to.districtId = from.B_AreaCode.Trim();
            to.district = from.B_Area;
            to.AreaCode = from.B_AreaCode.Trim();

            to.Address = from.Address.Trim();
            to.Address = to.Address.Replace((char)13, (char)0);
            to.Address = to.Address.Replace((char)10, (char)0);
            to.GroupId = from.B_GroupId;

            to.IDCard = from.B_IdCard;
            to.Password = from.B_Password;
            to.PhoneNo = from.PhoneNo.Trim();
            to.PhoneNo2 = from.PhoneNo2;
            to.Latitude = from.B_Latitude;
            to.Longitude = from.B_Longitude;
            to.Name = from.B_Name;
            to.DistribSubsidy = from.DistribSubsidy;
            to.OriginalBusiId = from.B_OriginalBusiId;
            to.InsertTime = DateTime.Now;
            //这里 佣金类型  在 接下来整合版本时，需要 调用接口方 传递过来
            //if (ConfigSettings.Instance.IsGroupPush)
            //{
            //    to.CommissionTypeId = 2;
            //}
            //else
            //{
            //    to.CommissionTypeId = 1;
            //}
            to.CommissionTypeId = from.CommissionTypeId != 0 ? from.CommissionTypeId : 1;
            return to;
        }
    }
    public enum CustomerRegisterStatus : int
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
        [DisplayText("您输入的推荐人号码不存在")]
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
        CommissionTypeIdEmpty = 113,
        [DisplayText("添加商铺失败")]
        Faild = 114,
        [DisplayText("注册失败")]
        ClientRegisterFaild = 115,
        [DisplayText("骑士已存在")]
        HasExist = 116,
        [DisplayText("时间戳不能为空")]
        TimespanEmpty = 117,
        [DisplayText("您当前注册的次数大于10，请5分钟后重试")]
        CountError = -10
    }
    //状态:0未审核，1已通过，2未审核且未添加地址，3审核中，4审核被拒绝
    public enum BusiStatus : int
    {
        [DisplayText("商户未注册")]
        BusiNoRegiste = -1,
        [DisplayText("商户未审核")]
        BusiNoPass = 0,
        [DisplayText("审核通过")]
        BusiPass = 1,
        [DisplayText("商户审核中")]
        BusiPassing = 3,
        [DisplayText("审核被拒绝")]
        BusiReject = 4,
        [DisplayText("商户无地址")]
        BusiNoAddress = 2,
        [DisplayText("未知状态")]
        BusiError = 100


    }
}
