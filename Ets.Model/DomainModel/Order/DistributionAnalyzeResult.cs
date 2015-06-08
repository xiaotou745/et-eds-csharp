using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Order
{
    public class DistributionAnalyzeResult
    {
        /// <summary>
        /// 订单内部ID
        /// </summary>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 任务单号
        /// </summary>
        public string OrderNo
        {
            get;
            set;
        }
        /// <summary>
        /// 城市
        /// </summary>
        public string ReceviceCity
        {
            get;
            set;
        }
        /// <summary>
        /// 商户信息
        /// </summary>
        public string Business
        {
            get;
            set;
        }
        /// <summary>
        /// 商户电话
        /// </summary>
        public string BusinessPhone
        {
            get;
            set;
        }
        /// <summary>
        /// 超人信息
        /// </summary>
        public string clienter
        {
            get;
            set;
        }
        /// <summary>
        /// 超人电话
        /// </summary>
        public string clienterPhone
        {
            get;
            set;
        }
        /// <summary>
        /// 取货地址
        /// </summary>
        public string PickUpAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceviceAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? PubDate
        {
            get;
            set;
        }
        /// <summary>
        /// 接单时间
        /// </summary>
        public DateTime? GrabTime
        {
            get;
            set;
        }
        /// <summary>
        /// 取货时间
        /// </summary>
        public DateTime? TakeTime
        {
            get;
            set;
        }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? ActualDoneDate
        {
            get;
            set;
        }
        public int OrderCount
        {
            get;
            set;
        }
        /// <summary>
        /// 任务金额
        /// </summary>
        public Decimal? TaskMoney
        {
            get;
            set;
        }
    }
}
