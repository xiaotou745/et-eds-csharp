using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
    /// <summary>
    /// 请求记录model
    /// 茹化肖
    /// 2015年8月25日11:06:05
    /// </summary>
    public class HttpModel
    {
        public HttpModel()
        {
            this.Url = "";
            this.Htype = 0;
            this.RequestBody = "";
            this.ResponseBody = "";
            this.ReuqestMethod = "";
            this.Status = 0;
            this.Remark = "";
            this.ReuqestPlatForm = 0;
        }


		/// <summary>
		/// 请求地址URL(站点请求,调用第三方HTTP请求)
		/// </summary>
		public string Url { get; set; }
		/// <summary>
		/// 操作类型 1请求 2响应 3回调 默认0未知
		/// </summary>
		public int Htype { get; set; }

		/// <summary>
		/// 请求体(包括get post参数)
		/// </summary>
		public string RequestBody { get; set; }
		/// <summary>
		/// 响应体(JSON)
		/// </summary>
		public string ResponseBody { get; set; }

		/// <summary>
		/// 请求方法名称
		/// </summary>
		public string ReuqestMethod { get; set; }

		/// <summary>
		/// 请求平台 0:默认未知 1:管理后台 2:WebApi 3:OpenApi 4 :第三方
		/// </summary>
		public int ReuqestPlatForm { get; set; }

		/// <summary>
		/// 返回的状态 1 成功 0 失败 默认0
		/// </summary>
		public int Status { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; }



    }
}
