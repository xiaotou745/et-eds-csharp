using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using NPOI.HSSF.Record.Formula.Functions;
using Match = NPOI.HSSF.Record.Formula.Functions.Match;

namespace Task.Common
{
    public static class XMLCommon
    {
        public static string objToXml(Type type, Object obj)
        {
            XmlSerializer xml = new XmlSerializer(type);
            String xmldata = "";
            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    xml.Serialize(stream, obj);
                    xmldata = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return xmldata;
        }

        #region 对象序列化XML文件

        /// <summary>  
        /// 序列化XML文件  
        /// </summary>  
        /// <param name="type">类型</param>  
        /// <param name="obj">对象</param>  
        /// <returns></returns>  
        public static string Serializer(Type type, object obj)
        {
            using (MemoryStream Stream = new MemoryStream())
            {
                //创建序列化对象  
                XmlSerializer xml = new XmlSerializer(type);
                try
                {
                    //序列化对象  
                    xml.Serialize(Stream, obj);
                }
                catch (InvalidOperationException ex)
                {
                    throw;
                }
                Stream.Position = 0;
                using (StreamReader sr = new StreamReader(Stream))
                {
                    string str = sr.ReadToEnd();
                    return str;
                }
            }
        }

        #endregion  

        #region 反序列化
        /// <summary>  
        /// 反序列化  
        /// </summary>  
        /// <param name="type">类型</param>  
        /// <param name="xml">XML字符串</param>  
        /// <returns></returns>  
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }
        /// <summary>  
        /// 反序列化  
        /// </summary>  
        /// <param name="type"></param>  
        /// <param name="xml"></param>  
        /// <returns></returns>  
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion 

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
