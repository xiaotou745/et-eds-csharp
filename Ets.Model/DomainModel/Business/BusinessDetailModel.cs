﻿using Ets.Model.DataModel.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DomainModel.Business
{
    public class BusinessDetailModel : BusinessModel
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
        /// 账号类型：(1网银 2支付宝 3微信 4财付通 5百度钱包 6现金支付）
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
        /// <summary>
        /// 第三方平台名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 是否可修改绑定第三方绑定
        /// </summary>
        public int IsModifyBind { get; set; }
        /// <summary>
        /// 商户绑定骑士数量
        /// </summary>
        public int BindClienterQty { get; set; }
        /// <summary>
        /// 推荐人手机号
        /// </summary>
        public string RecommendPhone { get; set; }
        /// <summary>
        /// 订单是否审核 1是 0 否
        /// </summary>
        public int IsOrderChecked { get; set; } 
    }
}
