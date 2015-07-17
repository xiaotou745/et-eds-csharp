using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Pay.YeePay;
using ETS.Util;

namespace ETS.Pay.YeePay
{
    /// <summary>
    /// 易宝注册子账户参数实体类
    /// </summary>
    public class YeeRegisterParameter
    {
        private string _minsettleAmount="0.1";
        private string _riskReserveday="1";
        private string _manualSettle = "Y";

        /// <summary>
        /// 请求Id
        /// </summary>
        public string RequestId
        {
            get { return TimeHelper.GetTimeStamp(false); }
        }

        /// <summary>
        /// 绑定手机
        /// </summary>
        public string BindMobile { get; set; }

        /// <summary>
        /// 注册类型  PERSON ：个人 ENTERPRISE：企业个人 ENTERPRISE：企业
        /// </summary>
        public CustomertypeEnum CustomerType{ get;   set; }

        /// <summary>
        /// 签约名     商户签约名；个人，填写姓名；企业，填写企业名称。
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
        public string BankAccountType
        {
            get
            {
                return CustomerType == CustomertypeEnum.PERSON
                    ? BankaccounttypeEnum.PrivateCash.ToString()
                    : BankaccounttypeEnum.PublicCash.ToString();
            }
        }

        /// <summary>
        /// 银行所在省
        /// </summary>
        public string BankProvince { get; set; }

        /// <summary>
        /// 银行所在市
        /// </summary>
        public string BankCity { get; set; }

        /// <summary>
        /// 结金额  默认 0.1
        /// </summary>
        public string MinsettleAmount
        {
            get { return _minsettleAmount; }
            set { _minsettleAmount = value; }
        }

        /// <summary>
        /// 结算周期 默认 1
        /// </summary>
        public string RiskReserveday
        {
            get { return _riskReserveday; }
            set { _riskReserveday = value; }
        }

        /// <summary>
        ///自助结算  N自助结算： N - 隔天自动打款； Y - 不会自动打款，需要通过提现接口或商户后台功能进行结算。   默认Y
        /// </summary>
        public string ManualSettle
        {
            get { return _manualSettle; }
            set { _manualSettle = value; }
        }
        /// <summary>
        /// 金融账号表Id
        /// </summary>
        public string AccountId { get; set; }
    }
}
