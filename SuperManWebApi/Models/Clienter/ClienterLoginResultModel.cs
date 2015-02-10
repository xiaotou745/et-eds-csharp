using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Clienter
{
    /// <summary>
    /// 登录成功返回的数据
    /// </summary>
    public class ClienterLoginResultModel
    {
        public int userId { get; set; }
        public sbyte? status { get; set; }
        public decimal? Amount { get; set; }
        public string phoneNo { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 城市编码
        /// </summary>
        public string cityId { get; set; }
    }

    public class ClienterModifyPwdResultModel
    {
        public string result { get; set; }
    }
}