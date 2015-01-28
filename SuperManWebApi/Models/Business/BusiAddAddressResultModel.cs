using SuperManCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuperManDataAccess;
using SuperManCommonModel;
namespace SuperManWebApi.Models.Business
{
    public class BusiAddAddressResultModel
    {
        public int userId { get; set; }
        public string status { get; set; }
    }

    public class BusiAddAddressInfoModel
    {
        public int userId { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string businessName { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string phoneNo { get; set; }
        /// <summary>
        /// 座机
        /// </summary>
        public string landLine { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public string districtName { get; set; }
        /// <summary>
        /// 区Id
        /// </summary>
        public string districtId { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double latitude { get; set; }
        public string cityId { get; set; }
    }

    public class BusiAddAddressInfoModelTranslator : TranslatorBase<business, BusiAddAddressInfoModel>
    {
        public static readonly BusiAddAddressInfoModelTranslator Instance = new BusiAddAddressInfoModelTranslator();

        public override BusiAddAddressInfoModel Translate(business from)
        {
            throw new NotImplementedException();
        }



        public override business Translate(BusiAddAddressInfoModel from)
        {
            var to = new business();
            to.Id = from.userId;
            to.Address = from.Address;
            to.Name = from.businessName;
            to.Landline = from.landLine;
            to.PhoneNo2 = from.phoneNo;
            to.districtId = from.districtId;
            to.district = from.districtName;
            to.Longitude = from.longitude;
            to.Latitude = from.latitude;
            to.Status = ConstValues.BUSINESS_NOAUDIT;
            return to;
        }
    }

    public enum BusiAddAddressStatus : int
    {
        Success = 0,
        [DisplayText("地址不能为空")]
        AddressEmpty,
        [DisplayText("手机号不能为空")]
        PhoneNumberEmpty,
        [DisplayText("商务地址不能为空")]
        businessNameEmpty,
        [DisplayText("验证码不正确")]
        IncorrectCheckCode,
        [DisplayText("昵称已被注册")]
        NickNameAlreadyRegistered,
        [DisplayText("手机号已被注册")]
        PhoneNumberRegistered
    }
}