﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Finance
{
    /// <summary>
    ///  商户修改绑定银行卡功能 参数实体 add by caoheyang 20150511
    /// </summary>
    public class CardModifyBPM:CardModifyPM
    {
        /// <summary>
        /// 商家ID(business表)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "商家不能为空")]
        public int BusinessId { get; set; }
        /// <summary>
        /// 身份证号 或营业执照
        /// </summary>
        public string IDCard { get; set; }
    }
}
