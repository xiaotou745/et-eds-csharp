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
        /// <summary>
        /// 骑士Id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 骑士状态 1 审核通过 0审核未通过
        /// </summary>
        public sbyte? status { get; set; }
        /// <summary>
        /// 骑士余额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string phoneNo { get; set; } 
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 城市编码
        /// </summary>
        public string cityId { get; set; }
        /// <summary>
        /// 骑士所属商户Id
        /// </summary>
        public List<int> BusibussId { get; set; }
    }

    public class ClienterModifyPwdResultModel
    {
        public string result { get; set; }
    }
}