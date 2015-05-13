using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace ETS.WxPay
{
    public class ResponseHandler
    {

        // 密钥 
        private string key;
        // 参与签名的参数列表
        private static string SignField = "appid,appkey,timestamp,openid,noncestr,issubscribe";
        // 微信服务器编码方式
        private string charset = "gb2312";

        //参与签名的参数列表
        protected HttpContext httpContext;
        //protected Hashtable parameters;
        private Hashtable xmlMap;

        //获取页面提交的get和post参数
        public ResponseHandler(HttpContext httpContext)
        {
            //parameters = new Hashtable();
            xmlMap = new Hashtable();

            this.httpContext = httpContext;
            if (this.httpContext.Request.InputStream.Length > 0)
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(this.httpContext.Request.InputStream);
                XmlNode root = xmlDoc.SelectSingleNode("xml");
                XmlNodeList xnl = root.ChildNodes;

                foreach (XmlNode xnf in xnl)
                {
                    xmlMap.Add(xnf.Name, xnf.InnerText);
                }
            }
        }

        #region 参数=======================================
        /// <summary>
        /// 初始化加载
        /// </summary>
        public virtual void init()
        {
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        public string getKey()
        {
            return key;
        }

        /// <summary>
        /// 设置密钥
        /// </summary>
        /// <param name="key"></param>
        public void setKey(string key)
        {
            this.key = key;
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string getParameter(string parameter)
        {
            var s = (string)xmlMap[parameter];
            return (null == s) ? "" : s;
        }

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="parameterValue"></param>
        public void setParameter(string parameter, string parameterValue)
        {
        }
        #endregion
    }
}
