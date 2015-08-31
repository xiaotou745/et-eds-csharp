using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ETS.Util;

namespace SuperMan.Common
{
    public static class ExcelUtility
    {
        /// <summary>
        /// 获取某类型下非私有成员的描述Description,返回string[]
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="t">实例</param>
        /// <returns>string[]</returns>
        public static string[] GetDescription<T>(T t)
        {
            try
            {
                string[] tStr = { "" };
                List<string> listStr = new List<string>();
                if (t == null)
                {
                    return tStr;
                }
                System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                if (properties.Length <= 0)
                {
                    return tStr;
                }
                foreach (System.Reflection.PropertyInfo item in properties)
                {
                    object value = item.GetValue(t, null);
                    string des;
                    try
                    {
                        des = ((DescriptionAttribute)Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute))).Description; // 属性值
                    }
                    catch
                    {
                        des = "";
                    }
                    if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                    {
                        listStr.Add(des);
                    }
                    else
                    {
                        GetDescription(value);
                    }
                }
                return listStr.ToArray();
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(new { t = t }, ex);
                return null;
            }
        }
        /// <summary>
        /// 导出NULLExcel文件内容
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileResult ReturnNullExcel(string fileName)
        {
            try
            {

                byte[] fileContents = Encoding.Default.GetBytes("无数据");
                var result = new FileContentResult(fileContents, "application/ms-excel");
                result.FileDownloadName = fileName + ".xls";
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(new { fileName = fileName }, ex);
                throw;
            }
        }
    }
}