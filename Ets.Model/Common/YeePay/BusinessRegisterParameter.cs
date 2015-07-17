using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Pay.YeePay;
using ETS.Util;

namespace Ets.Model.Common.YeePay
{
    public class BusinessRegisterParameter
    {
        string _requestid = TimeHelper.GetTimeStamp(false);

        string _customertype = CustomertypeEnum.PERSON.ToString(); //注册类型  PERSON ：个人 ENTERPRISE：企业个人 ENTERPRISE：企业

        string _bankaccounttype = BankaccounttypeEnum.PrivateCash.ToString();  //银行卡类别  PrivateCash：对私 PublicCash： 对公


        /// <summary>
        /// 请求Id
        /// </summary>
        public string requestid
        {
            get { return _requestid; }
            set { _requestid = value; }
        }
        /// <summary>
        /// 绑定手机
        /// </summary>
        public string bindmobile { get; set; }
        /// <summary>
        /// 注册类型  PERSON ：个人 ENTERPRISE：企业个人 ENTERPRISE：企业
        /// </summary>
        public string customertype
        {
            get { return _customertype; }
            set { _customertype = value; }
        }
        /// <summary>
        /// 签约名   商户签约名；个人，填写姓名；企业，填写企业名称。
        /// </summary>
        public string signedname { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string linkman { get; set; }
        /// <summary>
        /// 身份证  customertype为PERSON时，必填
        /// </summary>
        public string idcard { get; set; }
        /// <summary>
        /// 营业执照号 customertype为ENTERPRISE时，必填
        /// </summary>
        public string businesslicence { get; set; }
        /// <summary>
        /// 营业执照号 customertype为ENTERPRISE时，必填
        /// </summary>
        public string legalperson { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string bankaccountnumber { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string bankname { get; set; }
        /// <summary>
        /// 开户名
        /// </summary>
        public string accountname { get; set; }
        /// <summary>
        /// 银行卡类别  PrivateCash：对私 PublicCash： 对公
        /// </summary>
        public string bankaccounttype
        {
            get { return _bankaccounttype; }
            set { _bankaccounttype = value; }
        }
        /// <summary>
        /// 银行所在省
        /// </summary>
        public string bankprovince { get; set; }
        /// <summary>
        /// 银行所在市
        /// </summary>
        public string bankcity { get; set; }
        /// <summary>
        /// 金融账号表Id
        /// </summary>
        public string accountid { get; set; }

    }
}
