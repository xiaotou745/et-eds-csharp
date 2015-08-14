using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace ETS.Util
{
    public class LogHelper
    {
        public static Logger logger = NLog.LogManager.GetLogger("");

        /// <summary>
        /// 备注日志
        /// </summary>
        /// <param name="dec">需要捕获的参数（必须为属性类）</param>
        /// <param name="rmark">描述操作</param>
        public static void LogWriter(string rmark = "", object dec = null)
        {
            try
            {
                string logstr = "\r\n-----------------start----------------------\r\n";
                logstr = logstr + DateTime.Now.ToString() + "\r\n";
                MethodBase m = new StackTrace().GetFrame(1).GetMethod();
                ParameterInfo[] pm = m.GetParameters();
                string classname = m.DeclaringType.ToString();
                string propertyName = m.Name;
                logstr = logstr + "备注:" + rmark + "\r\n";
                //写类名
                logstr = logstr + "函数类名:" + classname + "\r\n";
                //写函数方法
                logstr = logstr + "函数名称为:" + propertyName + "\r\n";
                for (int i = 0; i < pm.Length; i++)
                {
                    logstr = logstr + "函数的参数有:" + pm[i].Name.ToString() + "\r\n";
                }
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                logstr = logstr + "函数参数值:" + jsonSerializer.Serialize(dec) + "\r\n";
                logstr += "--------------------end---------------------\r\n";
                //写日志
                logger.Info(logstr);
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 备注日志  输出字符串  add by caoheyang 20150423
        /// </summary>
        /// <param name="dec">需要捕获的参数（必须为属性类）</param>
        /// <param name="rmark">描述操作</param>
        public static void LogWriterString(string rmark = "", string dec = null)
        {
            try
            {
                string logstr = "\r\n-----------------start----------------------\r\n";
                logstr = logstr + DateTime.Now.ToString() + "\r\n";
                MethodBase m = new StackTrace().GetFrame(1).GetMethod();
                ParameterInfo[] pm = m.GetParameters();
                string classname = m.DeclaringType.ToString();
                string propertyName = m.Name;
                logstr = logstr + "备注:" + rmark + "\r\n";
                //写类名
                logstr = logstr + "函数类名:" + classname + "\r\n";
                //写函数方法
                logstr = logstr + "函数名称为:" + propertyName + "\r\n";
                for (int i = 0; i < pm.Length; i++)
                {
                    logstr = logstr + "函数的参数有:" + pm[i].Name.ToString() + "\r\n";
                }
                logstr = logstr + "函数参数值:" + dec + "\r\n";
                logstr += "--------------------end---------------------\r\n";
                //写日志
                logger.Info(logstr);
            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// 异常捕获日志
        /// </summary>
        /// <param name="ex">异常对象（必须为属性类）</param>
        /// <param name="rmark">操作简要描述</param>
        public static void LogWriter(Exception ex, string rmark = "")
        {
            try
            {
                string logstr = "\r\n-----------------start----------------------\r\n";
                logstr = logstr + DateTime.Now.ToString() + "\r\n";
                MethodBase m = new StackTrace().GetFrame(1).GetMethod();
                ParameterInfo[] pm = m.GetParameters();
                string classname = m.DeclaringType.ToString();
                string propertyName = m.Name;
                logstr = logstr + "备注:" + rmark + "\r\n";
                //写类名
                logstr = logstr + "函数类名:" + classname + "\r\n";
                //写函数方法
                logstr = logstr + "函数名称为:" + propertyName + "\r\n";
                for (int i = 0; i < pm.Length; i++)
                {
                    logstr = logstr + "函数的参数有:" + pm[i].Name.ToString() + "\r\n";
                }
                logstr = logstr + "函数异常:" + ex.ToString() + "\r\n";
                logstr += "--------------------end---------------------\r\n";
                //写日志
                logger.Info(logstr);
            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// 带参数 异常的日志
        /// </summary>
        /// <param name="dec">异常参数对象（必须为属性类）</param>
        /// <param name="ex">日志异常对象</param>
        public static void LogWriter(object dec, Exception ex)
        {
            try
            {
                string logstr = "\r\n-----------------start----------------------\r\n";
                logstr = logstr + DateTime.Now.ToString() + "\r\n";
                MethodBase m = new StackTrace().GetFrame(1).GetMethod();
                ParameterInfo[] pm = m.GetParameters();
                string classname = m.DeclaringType.ToString();
                string propertyName = m.Name;
                //写类名
                logstr = logstr + "函数类名:" + classname + "\r\n";
                //写函数方法
                logstr = logstr + "函数名称为:" + propertyName + "\r\n";
                for (int i = 0; i < pm.Length; i++)
                {
                    logstr = logstr + "函数的参数有:" + pm[i].Name.ToString() + "\r\n";
                }
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                logstr = logstr + "函数参数值:" + jsonSerializer.Serialize(dec) + "\r\n";
                logstr = logstr + "函数异常:" + ex.ToString() + "\r\n";
                logstr += "--------------------end---------------------\r\n";
                //写日志
                logger.Info(logstr);
            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// 带参数 描述 异常日志捕获
        /// </summary>
        /// <param name="dec">所要捕获的参数（必须为属性类）</param>
        /// <param name="ex">异常对象</param>
        /// <param name="rmark">描述</param>
        public static void LogWriter(object dec, Exception ex, string rmark)
        {
            try
            {
                string logstr = "\r\n-----------------start----------------------\r\n";
                logstr = logstr + DateTime.Now.ToString() + "\r\n";
                MethodBase m = new StackTrace().GetFrame(1).GetMethod();
                ParameterInfo[] pm = m.GetParameters();
                string classname = m.DeclaringType.ToString();
                string propertyName = m.Name;
                //写备注
                logstr = logstr + "备注:" + rmark + "\r\n";
                //写类名
                logstr = logstr + "函数类名:" + classname + "\r\n";
                //写函数方法
                logstr = logstr + "函数名称为:" + propertyName + "\r\n";
                for (int i = 0; i < pm.Length; i++)
                {
                    logstr = logstr + "函数的参数有:" + pm[i].Name.ToString() + "\r\n";
                }
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                logstr = logstr + "函数参数值:" + jsonSerializer.Serialize(dec) + "\r\n";
                logstr = logstr + "函数异常:" + ex.ToString() + "\r\n";
                logstr += "--------------------end---------------------\r\n";
                //写日志
                logger.Info(logstr);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 捕获全局异常
        /// </summary>
        /// <param name="error"></param>
        public static void LogWriterFromFilter(Exception error, int userId = 0, string userName="")
        {
            try
            {
                string logstr = "\r\n-----------------start----------------------\r\n";
                logstr = logstr + DateTime.Now.ToString() + "\r\n";
                logstr += "操作人:" + userName + "(" + userId + ")\r\n";
                //异常发生地址
                logstr = logstr + "异常发生地址:" + HttpContext.Current.Request.Url.AbsoluteUri.ToString() + "\r\n";
                logstr = logstr + "请求类型:" + HttpContext.Current.Request.RequestType.ToString() + "\r\n";
                
                var allKeys = HttpContext.Current.Request.Form.AllKeys;
                StringBuilder parstrBuilder=new StringBuilder("POST参数:");
                for (int i = 0; i < allKeys.Length; i++)
                {
                    var key = allKeys[i];
                    var value = HttpContext.Current.Request.Form[key];
                    parstrBuilder.AppendFormat("{0}={1}&", key, value);
                }
                string postData = parstrBuilder.ToString();
                if (allKeys.Length > 0)
                {
                    postData = postData.Substring(0, postData.Length - 1);
                }
                logstr += (postData + "\r\n");
                logstr += ("异常:" + error.Message + "\r\n");
                logstr += ("堆栈:" + error.StackTrace + "\r\n");
                logstr += "--------------------end---------------------\r\n";
                //发送邮件
                if (ConfigSettings.Instance.IsSendMail == "true")
                {
                    string emailToAddress = ConfigSettings.Instance.EmailToAdress;
                    EmailHelper.SendEmailTo(logstr, emailToAddress);
                }
                //写日志
                logger.Info(logstr);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Trace日志
        /// </summary>
        /// <param name="dec"></param>
        /// <param name="rmark">描述操作</param>
        public static void LogTraceStart(string type,string rmark)
        {
            try
            {
                //写日志
                string logstr = "\r\n-----------------start----------------------\r\n";
                logstr = logstr + DateTime.Now.ToString() + "\r\n";
                logstr = logstr + "推送对象:" + type + "\r\n";
                logstr = logstr + "短信内容:" + rmark + "\r\n";
                logger.Trace(logstr);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Trace日志
        /// </summary>
        /// <param name="dec"></param>
        /// <param name="rmark">描述操作</param>
        public static void LogTraceEnd()
        {
            try
            {
                //写日志
                string logstr = "\r\n--------------------end---------------------\r\n";
                logger.Trace(logstr);
            }
            catch (Exception)
            {

                throw;
            }
        }
        //写入手机号码
        public static void LogTraceWriterPhone(string rmark = "")
        {
            try
            {
                //写日志      
                string logstr = "\r\n";
                logstr = logstr + DateTime.Now.ToString() + "  ";
                logstr = logstr + rmark + "\r\n";
                logger.Trace(logstr);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
