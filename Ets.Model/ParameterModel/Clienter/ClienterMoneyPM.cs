﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    /// <summary>
    /// 骑士 余额 可提现余额 
    /// </summary>
    public class ClienterMoneyPM
    {
        /// <summary>
        /// 骑士Id(Clienter表）
        /// </summary>
        public int ClienterId { get; set; }
        /// <summary>
        /// 流水金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 流水状态(1、交易成功 2、交易中）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 交易后余额
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public int RecordType { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }
        /// <summary>
        /// 提现单ID
        /// </summary>
        public long WithwardId { get; set; }
        /// <summary>
        /// 关联单号
        /// </summary>
        public string RelationNo { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 骑士手续费金额
        /// </summary>
        public decimal WithdrawCommission { get; set; }
        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal PaidAmount { get; set; }
        
    }
}
