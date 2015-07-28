﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Enums;

namespace Ets.Model.DataModel.Finance
{
    /// <summary>
    /// 商户提现单实体
    /// </summary>
    public class BusinessWithdrawForm
    {
		/// <summary>
		/// 自增ID(PK)
		/// </summary>
		public long Id { get; set; }
		/// <summary>
		/// 提现单号
		/// </summary>
		public string WithwardNo { get; set; }
		/// <summary>
		/// 商家ID(business表)
		/// </summary>
		public int BusinessId { get; set; }
		/// <summary>
		/// 提现前商家余额
		/// </summary>
		public decimal BalancePrice { get; set; }
		/// <summary>
		/// 提现前商家可提现金额
		/// </summary>
		public decimal AllowWithdrawPrice { get; set; }
		/// <summary>
		/// 提现状态(1待审核 2 审核通过 3打款完成 -1审核拒绝 -2 打款失败)
		/// </summary>
		public int Status { get; set; }
		/// <summary>
		/// 提现金额
		/// </summary>
		public decimal Amount { get; set; }
		/// <summary>
		/// 提现后余额
		/// </summary>
		public decimal Balance { get; set; }
		/// <summary>
		/// 提现时间
		/// </summary>
		public DateTime WithdrawTime { get; set; }
		/// <summary>
		/// 审核人
		/// </summary>
		public string Auditor { get; set; }
		/// <summary>
		/// 审核时间
		/// </summary>
		public DateTime? AuditTime { get; set; }
		/// <summary>
		/// 审核拒绝原因
		/// </summary>
		public string AuditFailedReason { get; set; }
		/// <summary>
		/// 打款人
		/// </summary>
		public string Payer { get; set; }
		/// <summary>
		/// 打款时间
		/// </summary>
		public DateTime? PayTime { get; set; }
		/// <summary>
		/// 打款失败原因
		/// </summary>
		public string PayFailedReason { get; set; }
        /// <summary>
        /// 户名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 卡号(DES加密)
        /// </summary>
        public string AccountNo { get; set; }
        /// <summary>
        ///账户类型
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
        /// 省名称
        /// </summary>
        public string OpenProvince { get; set; }
        /// <summary>
        /// 市区名称
        /// </summary>
        public string OpenCity { get; set; } 
        /// <summary>
        /// 省代码
        /// </summary>
        public int OpenProvinceCode { get; set; }
        /// <summary>
        /// 市区代码
        /// </summary>
        public int OpenCityCode { get; set; }
        /// <summary>
        /// 对私身份证号或对公营业执照号 
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 手续费阈值,例如100
        /// </summary>
        public decimal HandChargeThreshold { get; set; }
        /// <summary>
        /// 手续费,例如1元
        /// </summary>
        public decimal HandCharge { get; set; }
        /// <summary>
        /// 手续费支出方:0个人,1易代送
        /// </summary>
        public HandChargeOutlay HandChargeOutlay { get; set; }

        public string PhoneNo { get; set; }
    }
}
