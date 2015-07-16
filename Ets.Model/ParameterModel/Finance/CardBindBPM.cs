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
    public class CardBindBPM:CardBindPM
    {
        /// <summary>
        /// 商家ID
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "商家不能为空")]
        public int BusinessId { get; set; }
        //TODO 身份证和营业执照分开
        /// <summary>
        /// 身份证或营业执照号    如果是个人则为身份证,否则为营业执照号 
        /// </summary>
        [Required(ErrorMessage = "身份证或营业执照号不能为空")]
        public string IDCard { get; set; }
    }
}
