using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common.YeePay
{

    /// <summary>
	/// 实体类YeePayUserDTO 。(属性说明自动提取数据库字段的描述信息)
	/// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-07-22 16:17:39   caoheyang
	/// </summary>
	public class YeePayUser
	{
		/// <summary>
		/// Id，自增
		/// </summary>
		public long Id { get; set; }
		/// <summary>
		/// 用户id （骑士id/商户id）
		/// </summary>
		public int UserId { get; set; }
		/// <summary>
		/// 用户类型（0骑士 1商家  默认 0）
		/// </summary>
		public int UserType { get; set; }
		/// <summary>
		/// 注册易宝账户时的请求id  注册请求号 在主帐号下唯一 MAX(50 )
		/// </summary>
		public string RequestId { get; set; }
		/// <summary>
		/// 商户编号 易代送公司主账号 
		/// </summary>
		public string CustomerNumberr { get; set; }
		/// <summary>
		/// 商户密钥
		/// </summary>
		public string HmacKey { get; set; }
		/// <summary>
		/// 绑定手机
		/// </summary>
		public string BindMobile { get; set; }
		/// <summary>
		/// 注册类型  PERSON ：个人 ENTERPRISE：企业 
		/// </summary>
		public string CustomerType { get; set; }
		/// <summary>
		/// 签约名   商户签约名；个人，填写姓名；企业，填写企业名称。
		/// </summary>
		public string SignedName { get; set; }
		/// <summary>
		/// 联系人
		/// </summary>
		public string LinkMan { get; set; }
		/// <summary>
		/// 身份证  customertype为PERSON时，必填
		/// </summary>
		public string IdCard { get; set; }
		/// <summary>
		/// 营业执照号 customertype为ENTERPRISE时，必填
		/// </summary>
		public string BusinessLicence { get; set; }
		/// <summary>
		/// 姓名  PERSON时，idcard对应的姓名； ENTERPRISE时，企业的法人姓名
		/// </summary>
		public string LegalPerson { get; set; }
		/// <summary>
		/// 起结金额
		/// </summary>
		public string MinsettleAmount { get; set; }
		/// <summary>
		/// 结算周期
		/// </summary>
		public string Riskreserveday { get; set; }
		/// <summary>
		/// 银行卡号
		/// </summary>
		public string BankAccountNumber { get; set; }
		/// <summary>
		/// 开户行
		/// </summary>
		public string BankName { get; set; }
		/// <summary>
		/// 开户名
		/// </summary>
		public string AccountName { get; set; }
		/// <summary>
		/// 银行卡类别  PrivateCash：对私 PublicCash： 对公
		/// </summary>
		public string BankAccountType { get; set; }
		/// <summary>
		/// 开户省
		/// </summary>
		public string BankProvince { get; set; }
		/// <summary>
		/// 开户市
		/// </summary>
		public string BankCity { get; set; }
		/// <summary>
		/// 自助结算  N自助结算： N - 隔天自动打款； Y - 不会自动打款，需要通过提现接口或商户后台功能进行结算。  
		/// </summary>
		public string ManualSettle { get; set; }
		/// <summary>
		/// 签名信息
		/// </summary>
		public string Hmac { get; set; }
		/// <summary>
		/// 注册时间
		/// </summary>
		public DateTime Addtime { get; set; }
		/// <summary>
		/// 易宝子账户编码（由易宝返回的）
		/// </summary>
		public string Ledgerno { get; set; }
		/// <summary>
		/// 本系统易宝账户余额
		/// </summary>
		public decimal BalanceRecord { get; set; }
        /// <summary>
        /// 易宝系统账户余额
        /// </summary>
        public decimal YeeBalance { get; set; }

	}
}
