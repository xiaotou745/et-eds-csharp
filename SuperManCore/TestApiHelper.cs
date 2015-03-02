using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCore
{
    public class TestApiHelper
    {
        public string TestApi<T>(string url, T postModel)
        {

            Uri myUri = new Uri(url);
            string postData = ObjToJson(postModel);
            // Uri myUri = new Uri(" http://localhost:9263/BusinessAPI/PostRegisterInfo_B");
            //string signdata = @"{""city"":""北京"",""CityId"":""1"",""phoneNo"":""18612635758"",""passWord"":""7AFBE62485A901909693C3D713034F0F"",""verifyCode"":"""" }";
            return PostDataGetHtml(myUri, postData);
        }
        /// <summary>
        /// 调用Api接口
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string PostDataGetHtml(Uri url, string postData)
        {
            byte[] data = Encoding.GetEncoding("UTF-8").GetBytes(postData);
            var req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "POST";
            req.KeepAlive = true;
            req.ContentType = "application/json";
            req.ContentLength = data.Length;
            req.AllowAutoRedirect = true;
            Stream outStream = req.GetRequestStream();
            outStream.Write(data, 0, data.Length);
            outStream.Close();
            HttpWebResponse res;
            try
            {
                res = req.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }

            var inStream = res.GetResponseStream();
            var sr = new StreamReader(inStream, Encoding.GetEncoding("UTF-8"));
            string htmlResult = sr.ReadToEnd();
            return htmlResult;
        }
        #region 对象和Json之间的转化
        /// <summary>
        /// 对象转化为Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ObjToJson<T>(T data)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(data.GetType());
                using (var ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, data);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Json转化为对象
        /// </summary>
        /// <param name="json"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Object JsonToObj(String json, Type t)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(t);
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {

                    return serializer.ReadObject(ms);
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
