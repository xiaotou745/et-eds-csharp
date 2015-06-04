using Ets.Model.DataModel.Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Bussiness
{
    public class BusinessDetailModel : Business
    {
        /// <summary>
        /// 户名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 卡号(DES加密)
        /// </summary>
        public string AccountNo { get; set; }
        /// <summary>
        /// 账号类型：(1网银 2支付宝 3微信 4财付通 5百度钱包）
        /// </summary>
        public int AccountType { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string OpenBank { get; set; }
        /// <summary>
        /// 开户支行
        /// </summary>
        public string OpenSubBank { get; set; }
        /// <summary>
        /// 操作人id
        /// </summary>
        public int OptUserId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OptUserName { get; set; }
        /// <summary>
        /// 操作描述
        /// </summary>
        public string OptLog { get; set; }
        /// <summary>
        /// 第三方绑定集合
        /// </summary>
        public string ThirdBindListStr { get; set; }
    }
}
