using SuperManBusinessLogic.B_Logic;
using SuperManBusinessLogic.Subsidy_Logic;
using SuperManCommonModel;
using SuperManCore;
using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Business
{
    /// <summary>
    /// B端发布订单所需数据
    /// </summary>
    public class BusiOrderInfoModel
    {
        /// <summary>
        /// 当前发布者
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string receviceName { get; set; }
        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string recevicePhone { get; set; }
        /// <summary>
        /// 收货人地址
        /// </summary>
        public string receviceAddress { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 是否已付款
        /// </summary>
        public bool IsPay { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double laitude { get; set; }
        /// <summary>
        /// 配送说明
        /// </summary>
        public string Remark { get; set; }
    }
    public class BusiOrderInfoModelTranslator : TranslatorBase<order, BusiOrderInfoModel>
    {
        public static readonly BusiOrderInfoModelTranslator Instance = new BusiOrderInfoModelTranslator();

        public override BusiOrderInfoModel Translate(order from)
        {
            throw new NotImplementedException();
        }



        public override order Translate(BusiOrderInfoModel from)
        {
            order to = new order();
            to.OrderNo = Helper.generateOrderCode(from.userId);  //根据userId生成订单号(15位)
            to.businessId = from.userId; //当前发布者
            business business = BusiLogic.busiLogic().GetBusinessById(from.userId);  //根据发布者id,获取发布者的相关信息实体
            if (business != null)
            {
                to.PickUpAddress = business.Address;  //提取地址
                to.PubDate = DateTime.Now; //提起时间
                to.ReceviceCity = business.City; //城市
            }
            //海底捞
            if (ConfigSettings.Instance.IsGroupPush)
            {
                to.OrderFrom = 2;
            }
            to.Remark = from.Remark;
            to.ReceviceName = from.receviceName;
            to.RecevicePhoneNo = from.recevicePhone;
            to.ReceviceAddress = from.receviceAddress;
            to.IsPay = from.IsPay;
            to.Amount = from.Amount;
            to.ReceviceLongitude = from.longitude;
            to.ReceviceLatitude = from.laitude;
            var subsidy = SubsidyLogic.subsidyLogic().GetCurrentSubsidy();
            to.WebsiteSubsidy = subsidy.WebsiteSubsidy;
            to.DistribSubsidy = subsidy.DistribSubsidy;
            if (subsidy.OrderCommission != null)
            {
                to.OrderCommission = subsidy.OrderCommission.Value * from.Amount;
            }
            else
            {
                to.OrderCommission = subsidy.OrderCommission;
            }                
            to.Status = ConstValues.ORDER_NEW;
            return to;
        }
    }
}