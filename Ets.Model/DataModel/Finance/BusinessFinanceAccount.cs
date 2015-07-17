using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Finance
{
    /// <summary>
    /// 商户金融信息实体
    /// </summary>
    public class BusinessFinanceAccount
    {
        /// <summary>
        /// 自增ID（PK）
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 商家ID
        /// </summary>
        public int BusinessId { get; set; }
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
        /// 身份证号、营业执照 ，对公营业执照，对私身份照
        /// </summary>
        public string IDCard { get; set; }

        /// <summary>
        /// 商户手机号
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// 易宝key
        /// </summary>
        public string YeepayKey { get; set; }
        /// <summary>
        /// 易宝账户状态 0正常 1失败
        /// </summary>
        public int YeepayStatus { get; set; }
        
    }
}
