using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Business
{
    public class OrderCancelResultModel
    {
        /// <summary>
        /// 失败原因描述
        /// </summary>
        public string Remark { get; set; }
    }
}