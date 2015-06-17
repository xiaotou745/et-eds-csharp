using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Business
{
    public class BusiDistribInfoModel1
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 外送费价格
        /// </summary>
        public decimal price { get; set; }
    }
}