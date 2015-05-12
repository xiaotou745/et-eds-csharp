﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    /// 商户绑定银行卡功能 参数实体 add by caoheyang 20150511
    /// </summary>
    public class CardBindBPM
    {
        /// <summary>
        /// 商家ID
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "商家不能为空")]
        public int BusinessId { get; set; }

        /// <summary>
        /// 户名
        /// </summary>
        [Required(ErrorMessage = "户名不能为空")]
        public string TrueName { get; set; }

        /// <summary>
        /// 卡号(需要DES加密)
        /// </summary>
        [Required(ErrorMessage = "卡号不能为空")]
        public string AccountNo { get; set; }

        /// <summary>
        /// 第二次录入卡号(需要DES加密)
        /// </summary>
        [Required(ErrorMessage = "第二次录入卡号不能为空")]
        public string AccountNo2 { get; set; }

        /// <summary>
        /// 账号类型：(1网银 2支付宝 3微信 4财付通 5百度钱包）
        /// </summary>
        public int AccountType { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        [Required(ErrorMessage = "开户行不能为空")]
        public string OpenBank { get; set; }
        /// <summary>
        /// 开户支行
        /// </summary>
        [Required(ErrorMessage = "开户支行不能为空")]
        public string OpenSubBank { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Required(ErrorMessage = "创建人不能为空")]
        public string CreateBy { get; set; }

    }
}
