﻿
using System;

namespace Ets.Model.ParameterModel.Clienter
{
    /// <summary>
    /// C端用户当前状态
    /// </summary>
    public class ClienterStatusModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int userid { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string phoneno { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal amount { get; set; }
    }
}
