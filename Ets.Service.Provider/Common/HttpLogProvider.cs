using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ets.Dao.Common;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Service.IProvider.Common;
using ETS.Util;

namespace Ets.Service.Provider.Common
{
    /// <summary>
    /// 茹化肖
    /// 2015年8月25日11:20:49
    /// </summary>
    public class HttpLogProvider : IHttpLogProvider
    {
        private readonly HttpDao dao=new HttpDao();
        /// <summary>
        /// 记录请求信息
        /// </summary>
        /// <param name="model"></param>
        public void LogRequestInfo(HttpModel model)
        {
            dao.LogRequestInfo(model);
        }
        /// <summary>
        /// 记录响应信息
        /// </summary>
        /// <param name="model"></param>
        public void LogResponseInfo(HttpRequest request,HttpResponse response)
        {
            if(response==null)
                return;
            if (request == null)
                return;
            var model = new HttpModel();
            model.Url = request.Url.AbsoluteUri;
            model.Htype = 2;
            //model.ResponseType = 1;
            model.ResponseBody = FormatReponseBody(response);
            model.Status = 1;
            dao.LogRequestInfo(model);
        }
        /// <summary>
        /// 记录第三方请求信息(HTTP调用第三方)
        /// </summary>
        /// <param name="model"></param>
        public void LogThirdPartyInfo(HttpWebRequest request,HttpWebResponse response)
        {
            var model = new HttpModel();
           // model.ReuqestPlatForm = 4;
            dao.LogRequestInfo(model);
        }
        /// <summary>
        /// 记录第三方请求信息(第三方回调)
        /// </summary>
        /// <param name="model"></param>
        public void LogThirdPartyInfo(HttpModel model)
        {
            model.ReuqestPlatForm = 4;
            dao.LogRequestInfo(model);
        }

        /// <summary>
        /// 序列化请求中的数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string FormatRequestBody(System.Web.HttpRequest request)
        {
            if (request == null)
                return "";
            StringBuilder stringBuilder=new StringBuilder();
             stringBuilder.Append("Get参数:");
            foreach (string query in request.QueryString)
            {
                stringBuilder.AppendFormat("{0}={1}&", query, request.QueryString[query]);
            }
            stringBuilder.Append("\r\n");
            stringBuilder.Append("Post参数:");
            foreach (string form in request.Form)
            {
                stringBuilder.AppendFormat("{0}={1}&", form, request.Form[form]);
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 序列化响应中的数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string FormatReponseBody(System.Web.HttpResponse response)
        {
            if (response == null)
                return "";
            Stream myResponseStream = response.OutputStream;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("响应数据:");
            stringBuilder.Append("");
            if (myResponseStream.CanRead)
            {
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("UTF-8"));
                string retString = myStreamReader.ReadToEnd();

                stringBuilder.Append(retString);
            }
            return stringBuilder.ToString();
        }

    }
}
