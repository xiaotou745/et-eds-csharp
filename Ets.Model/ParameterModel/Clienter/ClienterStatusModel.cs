
using System;

namespace Ets.Model.ParameterModel.Clienter
{
    /// <summary>
    /// C端用户当前状态
    /// </summary>
    public class ClienterStatusModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int userid { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string phoneno { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal amount { get; set; }

        /// <summary>
        /// 是否绑定了商户（0：否 1：是）
        /// </summary>
        public int IsBind { get; set; }

        /// <summary>
        /// 是否只显示雇主任务
        /// </summary>
        public int IsOnlyShowBussinessTask { get; set; }
        /// <summary>
        /// 配送公司Id
        /// </summary>
        public int DeliveryCompanyId { get; set; }
        /// <summary>
        /// 配送公司名称
        /// </summary>
        public string DeliveryCompanyName { get; set; }
        /// <summary>
        /// 是否显示 金额 0隐藏 1 显示
        /// </summary>
        public int IsDisplay { get; set; }

        /// <summary>
        /// 超人状态 0上班  1下班 默认为0
        /// </summary>
        public int WorkStatus { get; set; }
    }
}
