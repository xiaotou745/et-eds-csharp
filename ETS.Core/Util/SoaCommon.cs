using System.Xml.Schema;

namespace ETS.Util
{

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Script.Serialization;
    using System.Xml.Linq;

    /// <summary>
    /// 类名称：SoaCommon
    /// 命名空间：Ets.SoaService.Common
    /// 类功能：公共类库
    /// </summary>
    /// 创建者：周超
    /// 创建日期：7/2/2014 2:05 AM
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    public static class SoaCommon
    {
        /// <summary>
        /// The method indicates the object serialized using json.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="obj">The TType of obj</param>
        /// <returns>
        /// The json string
        /// </returns>
        /// Creator:zhouchao
        /// Creation Date:11/15/2011 2:50 PM
        /// Modifier:
        /// Last Modified:
        /// ----------------------------------------------------------------------------------------
        public static string Serialize<TType>(this TType obj)
        {
            try
            {
                if (obj.GetType().Name.Contains("<>f__AnonymousType"))
                {
                    var serializer = new JavaScriptSerializer();
                    return serializer.Serialize(obj);
                }

                using (var ms = new MemoryStream())
                {
                    var serializer = new DataContractJsonSerializer(obj.GetType(), new List<Type>(), int.MaxValue, true, null, false);
                    serializer.WriteObject(ms, obj);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch (InvalidDataException exception)
            {
                return null;
            }
            catch (ArgumentNullException exception)
            {
                return null;
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        /// <summary>
        /// The method indicates the json deserialized.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="json">The value of json.</param>
        /// <returns>
        /// The TType
        /// </returns>
        /// Creator:zhouchao
        /// Creation Date:11/15/2011 2:57 PM
        /// Modifier:
        /// Last Modified:
        /// ----------------------------------------------------------------------------------------
        public static TType Deserialize<TType>(this string json)
        {
            var model = Activator.CreateInstance<TType>();
            if (json.IsEmptyOrNull())
            {
                return model;
            }

            try
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    var serializer = new DataContractJsonSerializer(model.GetType(), new List<Type>(), int.MaxValue, true, null, false);
                    return (TType)serializer.ReadObject(ms);
                }
            }
            catch (InvalidCastException exception)
            {
                return model;
            }
            catch (ArgumentNullException exception)
            {
                return model;
            }
        }

        /// <summary>
        /// The method will 
        /// </summary>
        /// <param name="json">The json</param>
        /// <param name="type">The type</param>
        /// <returns>
        /// The Object
        /// </returns>
        /// 创建者：周超
        /// 创建日期：8/20/2014 11:35 PM
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        public static object Deserialize(this string json, Type type)
        {
            var model = Activator.CreateInstance(type);
            if (json.IsEmptyOrNull())
            {
                return model;
            }

            try
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    var serializer = new DataContractJsonSerializer(model.GetType(), new List<Type>(), int.MaxValue, true, null, false);
                    return serializer.ReadObject(ms);
                }
            }
            catch (InvalidCastException exception)
            {
                return model;
            }
            catch (ArgumentNullException exception)
            {
                return model;
            }
        }

        /// <summary>
        /// The method indicates the json deserialized.
        /// </summary>
        /// <param name="json">The value of json.</param>
        /// <returns>
        /// The string
        /// </returns>
        /// Creator:zhouchao
        /// Creation Date:11/15/2011 2:57 PM
        /// Modifier:
        /// Last Modified:
        /// ----------------------------------------------------------------------------------------
        public static string Deserialize(this string json)
        {
            if (json.IsEmptyOrNull())
            {
                return string.Empty;
            }

            try
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    var serializer = new DataContractJsonSerializer(typeof(string));
                    return (string)serializer.ReadObject(ms);
                }
            }
            catch (InvalidCastException exception)
            {
                return string.Empty;
            }
            catch (ArgumentNullException exception)
            {
                return string.Empty;
            }
        }

       
        /// <summary>
        /// 获取当前节点值
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="name">The name</param>
        /// <returns>
        /// 返回当前节点值
        /// </returns>
        /// 创建者：周超
        /// 创建日期：2013/10/15 20:12
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        public static string GetAttribute(this XElement element, string name)
        {
            if (element == null)
            {
                return string.Empty;
            }

            var attribute = element.Attribute(name);
            if (attribute == null)
            {
                return string.Empty;
            }

            return attribute.Value;
        }

        /// <summary>
        /// 判定字符串是否为空或null
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns></returns>
        /// 创建者：周超
        /// 创建日期：2013/10/13 10:47
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        public static bool IsEmptyOrNull(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            return value.Trim().Length == 0;
        }

        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="str">待加密的字串</param>
        /// <returns>
        /// 返回加密字串
        /// </returns>
        /// 创建者：周超
        /// 创建日期：2013/10/17 9:48
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        public static string Md5(this string str)
        {
            var b = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(str));
            var ret = string.Empty;
            for (var i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("X").PadLeft(2, '0');
            }

            return ret;
        }

        /// <summary>
        /// 类型转换为decimal数据类型
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="defaultDecimal">The defaultDecimal</param>
        /// <returns></returns>
        /// 创建者：周超
        /// 创建日期：2013/10/13 10:47
        /// 修改者：
        /// 修改时间：
        /// ----------------------------------------------------------------------------------------
        public static decimal ToDecimal(this string value, decimal defaultDecimal)
        {
            decimal tempDecimal = 0;
            if (decimal.TryParse(value, out tempDecimal))
            {
                return tempDecimal;
            }
            return defaultDecimal;
        }
    }
}
