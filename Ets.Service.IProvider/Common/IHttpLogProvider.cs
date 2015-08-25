using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ets.Model.Common;

namespace Ets.Service.IProvider.Common
{
    /// <summary>
    /// 请求记录类
    /// 茹化肖
    /// 2015年8月25日11:18:22
    /// </summary>
   public  interface IHttpLogProvider
    {
        /// <summary>
        /// 记录请求数据
        /// </summary>
        /// <param name="model"></param>
         void LogRequestInfo(HttpRequest request);

        /// <summary>
        /// 记录响应数据
        /// </summary>
        /// <param name="model"></param>
         void LogResponseInfo(HttpRequest request,HttpResponse response);

        /// <summary>
        /// 记录第三方请求及响应
        /// </summary>
        /// <param name="model"></param>
         void LogThirdPartyInfo(HttpWebRequest request, HttpWebResponse response);

    }
}
