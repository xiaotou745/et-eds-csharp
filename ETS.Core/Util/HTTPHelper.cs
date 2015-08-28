using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;


namespace ETS.Util
{

    /// <summary>
    /// HTTP请求帮助类 add by caoheyang 20150331
    /// </summary>
    public class HTTPHelper
    {
        /// <summary>
        /// 模拟post请求 add by caoheyang 20150331
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        ///  <param name="accept">Accept HTTP 标头的值</param>  
        ///   <param name="method">请求方法</param>  
        /// <returns></returns>
        public static string HttpPost(string Url, string postDataStr,
            string accept = "Accept:text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
            string method = "POST")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.CookieContainer = new CookieContainer();
            CookieContainer cookie = request.CookieContainer;//如果用不到Cookie，删去即可  
            request.Accept = accept;
            request.Headers["Accept-Language"] = "zh-CN,zh;q=0.";
            request.Headers["Accept-Charset"] = "GBK,utf-8;q=0.7,*;q=0.3";
            request.UserAgent = "User-Agent:Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.835.202 Safari/535.1";
            request.KeepAlive = true;
            //上面的http头看情况而定，但是下面俩必须加  
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = method;
            Encoding encoding = Encoding.UTF8;//根据网站的编码自定义  
            byte[] postData = encoding.GetBytes(postDataStr);//postDataStr即为发送的数据，格式还是和上次说的一样  
            request.ContentLength = postData.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postData, 0, postData.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            //如果http头中接受gzip的话，这里就要判断是否为有压缩，有的话，直接解压缩即可  
            if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
            {
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            }

            StreamReader streamReader = new StreamReader(responseStream, encoding);
            string retString = streamReader.ReadToEnd();

            streamReader.Close();
            responseStream.Close();

            return retString;
        }

           /// <summary>
        /// 
        /// </summary>
        /// <param name="fromModel">paraModel</param>
        public static T BindeModel<T>(System.Web.HttpRequest httpRequest)
        {
            ////实体类赋值
            T paramodel =Activator.CreateInstance<T>();
            System.Reflection.PropertyInfo[] props = paramodel.GetType().GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                if (httpRequest.Form[props[i].Name] != null)
                {
                    object valtemp = System.Web.HttpUtility.UrlDecode(httpRequest.Form[props[i].Name]);
                    valtemp = ParseHelper.ToType(valtemp, props[i].PropertyType);
                    props[i].SetValue(paramodel, valtemp);
                }
            }
            return paramodel;
        }
        ///// <summary>
        ///// 模拟post请求(带OUT参数重载)
        ///// 2015年8月26日08:51:51
        ///// 茹化肖
        ///// </summary>
        ///// <param name="Url">URL</param>
        ///// <param name="postDataStr">POST数据</param>
        /////  <param name="accept">Accept HTTP 标头的值</param>  
        /////   <param name="method">请求方法</param>  
        ///// <returns>响应体</returns>
        //public static string HttpPost(string Url, string postDataStr, out HttpWebRequest request,out  HttpWebResponse response,
        //    string accept = "Accept:text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
        //    string method = "POST")
        //{
        //    request = (HttpWebRequest)WebRequest.Create(Url);
        //    request.CookieContainer = new CookieContainer();
        //    CookieContainer cookie = request.CookieContainer;//如果用不到Cookie，删去即可  
        //    request.Accept = accept;
        //    request.Headers["Accept-Language"] = "zh-CN,zh;q=0.";
        //    request.Headers["Accept-Charset"] = "GBK,utf-8;q=0.7,*;q=0.3";
        //    request.UserAgent = "User-Agent:Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.835.202 Safari/535.1";
        //    request.KeepAlive = true;
        //    //上面的http头看情况而定，但是下面俩必须加  
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.Method = method;
        //    Encoding encoding = Encoding.UTF8;//根据网站的编码自定义  
        //    byte[] postData = encoding.GetBytes(postDataStr);//postDataStr即为发送的数据，格式还是和上次说的一样  
        //    request.ContentLength = postData.Length;
        //    Stream requestStream = request.GetRequestStream();
        //    requestStream.Write(postData, 0, postData.Length);
        //    response = (HttpWebResponse)request.GetResponse();
        //    Stream responseStream = response.GetResponseStream();
        //    //如果http头中接受gzip的话，这里就要判断是否为有压缩，有的话，直接解压缩即可  
        //    if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
        //    {
        //        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
        //    }

        //    StreamReader streamReader = new StreamReader(responseStream, encoding);
        //    string retString = streamReader.ReadToEnd();

        //    streamReader.Close();
        //    responseStream.Close();

        //    return retString;
        //}
    }
}
