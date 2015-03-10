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

        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public int OrderFrom { get; set; }

    }
    public class BusiOrderInfoModelTranslator : TranslatorBase<order, BusiOrderInfoModel>
    {
        public static readonly BusiOrderInfoModelTranslator Instance = new BusiOrderInfoModelTranslator();

        public override BusiOrderInfoModel Translate(order from)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// 整合订单信息  Edit by caoheyang 20150305
        /// </summary>
        /// <param name="from">C端订单数据</param>
        /// <returns></returns>
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
                to.DistribSubsidy = business.DistribSubsidy;//设置外送费,从商户中找。
            }
            
            if (ConfigSettings.Instance.IsGroupPush)
            {
                if (from.OrderFrom != 0)
                    to.OrderFrom = from.OrderFrom;
                else
                    to.OrderFrom = 0;
            }
            to.Remark = from.Remark;
            to.ReceviceName = from.receviceName;
            to.RecevicePhoneNo = from.recevicePhone;
            to.ReceviceAddress = from.receviceAddress;
            to.IsPay = from.IsPay;
            to.Amount = from.Amount;
            to.OrderCount = from.OrderCount;  //订单数量
            to.ReceviceLongitude = from.longitude;
            to.ReceviceLatitude = from.laitude;
            var subsidy = SubsidyLogic.subsidyLogic().GetCurrentSubsidy(groupId: business.GroupId == null ? 0 : Convert.ToInt32(business.GroupId));
            to.WebsiteSubsidy = subsidy.WebsiteSubsidy;  //网站补贴
            to.CommissionRate = subsidy.OrderCommission == null ? 0 : subsidy.OrderCommission; //佣金比例 
            decimal distribe = 0;  //默认外送费，网站补贴都为0
            if (to.DistribSubsidy != null)//如果外送费有数据，按照外送费计算骑士佣金
                distribe = Convert.ToDecimal(to.DistribSubsidy);
            else if (to.WebsiteSubsidy != null)//如果外送费没数据，按照网站补贴计算骑士佣金
                distribe = Convert.ToDecimal(to.WebsiteSubsidy);
            to.OrderCommission = from.Amount *to.CommissionRate + distribe*to.OrderCount;//计算佣金
            to.Status = ConstValues.ORDER_NEW;
            return to;
        }
    }
}