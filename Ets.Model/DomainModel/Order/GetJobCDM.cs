using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Order
{
    /// <summary>
    ///  骑士端获取任务列表（最新/最近）任务返回数据实体  add by caoheyang 20150519
    /// </summary>
    public class GetJobCDM
    {
        /// <summary>
        /// 订单id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 下单时间   格式  今日 11:38
        /// </summary>
        public string PubDate { get; set; }
        /// <summary>
        /// 配送费，骑士收入
        /// </summary>
        public decimal OrderCommission { get; set; }
        /// <summary>
        /// 订单数
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 任务总金额（订单金额总和）  会加上外送费显示
        /// </summary>
        public decimal Amount { get; set; }
        public string OrderNo { get; set; }
        /// <summary>
        /// 菜品金额
        /// </summary>
        public decimal CpAmount { get; set; }
        /// <summary>
        ///取货地址（商户名称）
        /// </summary>
        public string BusinessName { get; set; }
        /// <summary>
        /// 取货地址（商户地址）
        /// </summary>
        public string BusinessCity { get; set; }
        /// <summary>
        /// 取货地址（商户城市）
        /// </summary>
        public string BusinessAddress { get; set; }
        /// <summary>
        /// 送货地址(城市)
        /// </summary>
        public string UserCity { get; set; }
        /// <summary>
        ///送货地址
        /// </summary>
        public string UserAddress { get; set; }
        /// <summary>
        /// 骑士距离商户/发单地址的距离
        /// </summary>
        public string DistanceToBusiness { get; set; }

        /// <summary>
        /// 当前坐标到取货坐标
        /// </summary>
        public string Pubtocurrentdistance { get; set; }

        /// <summary>
        ///  商家经度/发单经度
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// 商家纬度/发单纬度
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 来源（默认1、旧后台，2新后台）
        /// </summary>
        public int Platform { get; set; }

        /// <summary>
        /// 订单总重量
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// 送餐距离
        /// </summary>
        public string KM { get; set; }

        /// <summary>
        /// 取货状态默认0立即，1预约
        /// </summary>
        public int TakeType { get; set; }

        private string songCanDate;
        /// <summary>
        /// 送餐时间,客户要求送餐时间
        /// </summary>
        public string SongCanDate
        {
            get { return songCanDate == null ? "" : songCanDate; }
            set { songCanDate = value; }
        }

        private string expectedTakeTime;
        public string ExpectedTakeTime
        {
            get { return expectedTakeTime == null ? "" : expectedTakeTime; }
            set { expectedTakeTime = value; }
        } //商户期望骑士取货时间
        public int BusinessId { get; set; }

        public string PubName { get; set; }
    }
}
