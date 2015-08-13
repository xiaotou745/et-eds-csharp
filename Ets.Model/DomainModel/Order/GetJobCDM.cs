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
        /// 骑士距离商户地址的距离
        /// </summary>
        public string DistanceToBusiness { get; set; }

        /// <summary>
        ///  商家经度
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// 商家纬度
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
