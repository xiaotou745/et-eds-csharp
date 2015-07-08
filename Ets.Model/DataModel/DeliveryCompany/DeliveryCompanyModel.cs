using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.DeliveryCompany
{
    /// <summary>

    /// 实体类DeliveryCompanyInfo 。(属性说明自动提取数据库字段的描述信息) 
    /// Generate By: tools.etaoshi.com 
    /// Generate Time: 2015-07-06 13:28:46 
    /// </summary>

    public class DeliveryCompanyModel
    {

        public DeliveryCompanyModel() { }

        /// <summary>

        /// Id主键自增

        /// </summary>

        public int Id { get; set; }

        /// <summary>

        /// 物流公司名称

        /// </summary>

        public string DeliveryCompanyName { get; set; }
        public string DeliveryCompanyOldName { get; set; }
        /// <summary>

        /// 物流公司Code（11位，以1开头）

        /// </summary>

        public string DeliveryCompanyCode { get; set; }

        /// <summary>

        /// 是否有效公司(默认1有效,0无效)

        /// </summary>

        public int IsEnable { get; set; }

        /// <summary>

        /// 结算类型（1结算比例、2固定金额）

        /// </summary>

        public int SettleType { get; set; }

        /// <summary>

        /// 骑士固定金额值

        /// </summary>

        public decimal ClienterFixMoney { get; set; }

        /// <summary>

        /// 骑士结算比例值

        /// </summary>

        public decimal ClienterSettleRatio { get; set; }
        /// <summary>
        /// 骑士结算，记录比例或者 固定金额，通过SettleType来区分
        /// </summary>
        public decimal ClienterSettle { get; set; }

        /// <summary>

        /// 物流公司固定金额值

        /// </summary>

        public decimal DeliveryCompanySettleMoney { get; set; }

        /// <summary>

        /// 物流公司结算比例值

        /// </summary>

        public decimal DeliveryCompanyRatio { get; set; }
        /// <summary>
        /// 物流公司结算，记录比例或者 固定金额，通过SettleType来区分
        /// </summary>
        public decimal DeliveryCompanySettle { get; set; }
        /// <summary>

        /// 骑士数量,该物流公司下有多少骑士

        /// </summary>

        public int ClienterQuantity { get; set; }
        /// <summary> 
        /// 商家数量 
        /// </summary> 
        public int BusinessQuantity { get; set; }

        /// <summary>

        /// 创建时间

        /// </summary>

        public DateTime? CreateTime { get; set; }

        /// <summary>

        /// 创建人 
        /// </summary> 
        public string CreateName { get; set; }

        /// <summary> 
        /// 修改人 
        /// </summary>

        public string ModifyName { get; set; }

        /// <summary> 
        /// 修改时间 
        /// </summary> 
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 是否显示骑士金额 0隐藏 1显示
        /// </summary>
        public int IsDisplay { get; set; }
    }

}
