﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Finance
{
    /// <summary>
    /// 骑士提现日志表 实体类ClienterWithdrawLog 。(属性说明自动提取数据库字段的描述信息)
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-05-09 16:02:12
    /// </summary>
    public class ClienterWithdrawLog
    {
        public ClienterWithdrawLog() { }
        /// <summary>
        /// 自增ID(PK)
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 提现单ID
        /// </summary>
        public long WithwardId { get; set; }
        /// <summary>
        /// 操作后状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperatTime { get; set; }
        /// <summary>
        /// 之前状态
        /// </summary>
        public int OldStatus { get; set; }
        /// <summary>
        /// 处理状态（0：待处理 1：已处理）
        /// </summary>
        public int DealStatus { get; set; }
        /// <summary>
        /// 是否为回调（0：否 1：是）
        /// </summary>
        public int IsCallBack { get; set; }
        /// <summary>
        /// 回调请求Id
        /// </summary>
        public string CallBackRequestId { get; set; }
    }

}
