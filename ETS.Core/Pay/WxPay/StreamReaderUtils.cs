using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Pay.WxPay
{
    public sealed class StreamReaderUtils
    {
        #region StreamReader
        public static ReturnValue StreamReader(string RequestURI, Encoding Encode)
        {
            return StreamReader(RequestURI, Encode, false, true);
        }

        public static ReturnValue StreamReader(string RequestURI, Encoding Encode, bool Post)
        {
            return StreamReader(RequestURI, Encode, Post, true);
        }

        public static ReturnValue StreamReader(string RequestURI, Encoding Encode, bool Post, bool Expect100Continue)
        {
            System.Net.ServicePointManager.Expect100Continue = Expect100Continue;

            ReturnValue retValue = new ReturnValue();
            if (Encode == null)
            {
                Encode = System.Text.Encoding.GetEncoding("utf-8");
            }
            string strReturnCode;
            HttpWebRequest myHttpWebRequest = null;
            string[] arrRequestURI = RequestURI.Split('?');

            //如果是发送HTTPS请求  
            if (RequestURI.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(Post ? arrRequestURI[0] : RequestURI);
                myHttpWebRequest.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(Post ? arrRequestURI[0] : RequestURI);
            }

            try
            {
                byte[] bs;
                if (Post)
                {
                    myHttpWebRequest.Method = "POST";
                    myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    bs = Encoding.ASCII.GetBytes(arrRequestURI.Length >= 2 ? arrRequestURI[1] : "");

                    myHttpWebRequest.ContentLength = bs.Length;

                    using (Stream reqStream = myHttpWebRequest.GetRequestStream())
                    {
                        reqStream.Write(bs, 0, bs.Length);
                    }
                }
                else
                {
                    myHttpWebRequest.Method = "GET";
                }
                using (WebResponse myWebResponse = myHttpWebRequest.GetResponse())
                {
                    StreamReader readStream = new StreamReader(myWebResponse.GetResponseStream(), Encode);
                    strReturnCode = readStream.ReadToEnd();
                }

                retValue.HasError = false;
                retValue.Message = strReturnCode;
                retValue.ReturnObject = null;
            }
            catch (Exception ex)
            {
                retValue.HasError = true;
                retValue.Message = "请求接口信息出错";
                retValue.ReturnObject = ex;
            }
            return retValue;
        }

        public static ReturnValue StreamReader(string RequestURI, byte[] RequestData, Encoding Encode, bool Post)
        {
            System.Net.ServicePointManager.Expect100Continue = true;

            ReturnValue retValue = new ReturnValue();
            if (Encode == null)
            {
                Encode = System.Text.Encoding.GetEncoding("utf-8");
            }
            string strReturnCode;
            HttpWebRequest myHttpWebRequest = null;

            //如果是发送HTTPS请求  
            if (RequestURI.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(RequestURI);
                myHttpWebRequest.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(RequestURI);
            }

            try
            {
                byte[] bs;
                if (Post)
                {
                    myHttpWebRequest.Method = "POST";
                    myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    bs = RequestData;

                    myHttpWebRequest.ContentLength = bs.Length;

                    using (Stream reqStream = myHttpWebRequest.GetRequestStream())
                    {
                        reqStream.Write(bs, 0, bs.Length);
                    }
                }
                else
                {
                    myHttpWebRequest.Method = "GET";
                }
                using (WebResponse myWebResponse = myHttpWebRequest.GetResponse())
                {
                    StreamReader readStream = new StreamReader(myWebResponse.GetResponseStream(), Encode);
                    strReturnCode = readStream.ReadToEnd();
                }

                retValue.HasError = false;
                retValue.Message = strReturnCode;
                retValue.ReturnObject = null;
            }
            catch (Exception ex)
            {
                retValue.HasError = true;
                retValue.Message = "请求接口信息出错";
                retValue.ReturnObject = ex;
            }
            return retValue;
        }

        public static ReturnValue StreamReaderLoop(string RequestURI, Encoding Encode)
        {
            HttpWebRequest req = WebRequest.Create(RequestURI) as HttpWebRequest;
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encode);

            ReturnValue retValue = new ReturnValue();

            string strReturn = "";
            int length = 0x400;

            char[] buf = new char[length];
            try
            {
                int i = 0;
                do
                {
                    i = reader.Read(buf, 0, length);
                    strReturn += new string(buf, 0, i);
                    Array.Clear(buf, 0, length);
                } while (i > 0);
            }
            catch
            {
                int index = Array.IndexOf<char>(buf, '\0');
                if (index >= 0)
                {
                    strReturn += new string(buf, 0, index);
                }
            }
            finally
            {
                reader.Close();
                stream.Close();
                resp.Close();
            }

            retValue.Message = strReturn;

            return retValue;
        }

        private static bool CheckValidationResult(object Sender, System.Security.Cryptography.X509Certificates.X509Certificate Certificate, System.Security.Cryptography.X509Certificates.X509Chain Chain, System.Net.Security.SslPolicyErrors Errors)
        {
            return true;
        }
        #endregion
    }
}
