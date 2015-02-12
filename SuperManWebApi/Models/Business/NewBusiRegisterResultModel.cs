using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Business
{
    public class NewBusiRegisterResultModel
    {
        /// <summary>
        /// 注册成功后的商户Id
        /// </summary>
        public int BusiRegisterId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}