#region 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
#endregion

namespace Ets.Model.DataModel.Finance
{
    /// <summary>
    /// 骑士金融账号表 实体类ClienterFinanceAccount 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 15:57:21
    /// </summary>
    public class ClienterFinanceAccount
    {
        public ClienterFinanceAccount() { }
        /// <summary>
        /// 自增ID（PK）
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 骑士ID
        /// </summary>
        public int ClienterId { get; set; }
        /// <summary>
        /// 户名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 卡号(DES加密)
        /// </summary>
        public string AccountNo { get; set; }
        /// <summary>
        /// 是否有效(1：有效 0：无效）
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 账号类型：(1网银 2支付宝 3微信 4财付通 5百度钱包）
        /// </summary>
        public int AccountType { get; set; }

        /// <summary>
        /// 账号类别  0 个人账户 1 公司账户  
        /// </summary>
        public int BelongType { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string OpenBank { get; set; }
        /// <summary>
        /// 开户支行
        /// </summary>
        public string OpenSubBank { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 省名称
        /// </summary>
        public string OpenProvince { get; set; }
        /// <summary>
        /// 市区名称
        /// </summary>
        public string OpenCity { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }
    }

}
