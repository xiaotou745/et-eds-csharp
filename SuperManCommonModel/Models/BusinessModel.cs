using SuperManCore;
using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCommonModel.Models
{
    /// <summary>
    /// 商户Model
    /// </summary>
    public class BusinessModel
    {
        /// <summary>
        /// 商家编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 所属城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 验证照片
        /// </summary>
        public string CheckPicUrl { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 固话
        /// </summary>
        public string Landline { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double? Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double? Latitude { get; set; }

        public DateTime? InsertTime { get; set; }
        public sbyte? Status { get; set; }
    }

    public class BusinessViewModel
    {
        /// <summary>
        /// 商家编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime? InsertTime { get; set; }
        /// <summary>
        /// 订单数合计
        /// </summary>
        public int? OrderCount { get; set; }
        /// <summary>
        /// 订单金额合计
        /// </summary>
        public decimal? OrderAmountCount { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public sbyte? Status { get; set; }
        /// <summary>
        /// 审核状态显示 
        /// </summary>
        public string statusView { get; set; }
    }
    public class BusinessModelTranslator : TranslatorBase<business, BusinessModel>
    {
        public static readonly BusinessModelTranslator Instance = new BusinessModelTranslator();
        public override BusinessModel Translate(business from)
        {
            var to = new BusinessModel();
            to.Id = from.Id;
            to.Name = from.Name;
            to.City = from.City;
            to.PhoneNo = from.PhoneNo;
            to.Password = from.Password;
            to.CheckPicUrl = from.CheckPicUrl;
            to.IDCard = from.IDCard;
            to.Address = from.Address;
            to.Landline = from.Landline;
            to.Longitude = from.Longitude;
            to.Latitude = from.Latitude;
            to.Status = from.Status;
            return to;
        }
        public override business Translate(BusinessModel from)
        {
            var to = new business();
            to.Id = from.Id;
            to.Name = from.Name;
            to.City = from.City;
            to.PhoneNo = from.PhoneNo;
            to.Password = from.Password;
            to.CheckPicUrl = from.CheckPicUrl;
            to.IDCard = from.IDCard;
            to.Address = from.Address;
            to.Landline = from.Landline;
            to.Longitude = from.Longitude;
            to.Latitude = from.Latitude;
            to.Status = from.Status;
            return to;
        }
    }
}
