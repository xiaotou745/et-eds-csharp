using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    /// 商户/骑士 绑定银行卡功能 参数实体 父类 add by caoheyang 20150511
    /// 父类
    /// </summary>
    public class CardBindPM
    {
        /// <summary>
        /// 户名
        /// </summary>
        [Required(ErrorMessage = "户名不能为空")]
        public string TrueName { get; set; }

        /// <summary>
        /// 卡号(需要DES加密)
        /// </summary>
        [Required(ErrorMessage = "金融账号不能为空")]
        [Compare("AccountNo2", ErrorMessage = "两次录入的金融账号不一致")]
        public string AccountNo { get; set; }

        /// <summary>
        /// 第二次录入卡号(需要DES加密)
        /// </summary>
        [Required(ErrorMessage = "第二次录入金融账号不能为空")]
        public string AccountNo2 { get; set; }

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
        [Required(ErrorMessage = "开户行不能为空")]
        public string OpenBank { get; set; }

        /// <summary>
        /// 开户支行
        /// </summary>
        public string OpenSubBank { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Required(ErrorMessage = "创建人不能为空")]
        public string CreateBy { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [Required(ErrorMessage = "版本号不能为空")]
        public string Version { get; set; }
    }
}
