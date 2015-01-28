using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Clienter
{
    public class ClientOrderInfoModel
    {
        /// <summary>
        /// userId
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double latitude { get; set; }
        /// <summary>
        /// 是否最新任务
        /// </summary>
        public bool isLatest { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int? pageSize { get; set; }
        /// <summary>
        /// 第几页
        /// </summary>
        public int? pageIndex { get; set; }
        public sbyte? status { get; set; }
    }
}