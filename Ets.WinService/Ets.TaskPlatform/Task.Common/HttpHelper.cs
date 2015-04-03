using System;
using System.IO;
using System.Net;
using System.Text;

namespace Task.Common
{
    public class HttpHelper
    {
        /// <summary>
        ///     向指定的路径发送http请求并接收返回值。
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static string Post(string url, string postdata)
        {
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                if (request != null)
                {
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    byte[] data = Encoding.UTF8.GetBytes(postdata);
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    var response = request.GetResponse() as HttpWebResponse;
                    if (response != null)
                    {
                        var sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                        string outMessage = sr.ReadToEnd();
                        sr.Close();
                        return outMessage;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}