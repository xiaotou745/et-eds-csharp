using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using ETS.Data;
using ETS.Extension;
using System.Data.Common;

namespace ETS.Dao
{
    public class DaoBase : AbstractDaoBase
    {
        /// <summary>
        /// ETS数据库连接字符串
        /// </summary>
        protected string SuperMan_Write
        {
            get { return GetConnString("SuperMan_Write"); }
        }

        /// <summary>
        /// 主读
        /// </summary>
        protected string SuperMan_Read
        {
            get { return GetConnString("SuperMan_Read"); }
        }

        /// <summary>
        /// 将Table数据转化为对象列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="table">数据表</param>
        /// <returns></returns>
        protected IList<T> MapRows<T>(DataTable table)
        {
            IList<T> lstT = new List<T>();

            if (!table.HasData())
            {
                return lstT;
            }

            Type type = typeof(T);
            PropertyInfo[] typeProperties = type.GetProperties();
            foreach (DataRow dataRow in table.Rows)
            {
                T t = (T)type.Assembly.CreateInstance(type.FullName);
                lstT.Add(t);

                foreach (DataColumn dataColumn in table.Columns)
                {
                    var properties = typeProperties.Where(p => p.Name.ToLower() == dataColumn.ColumnName.ToLower());
                    if (!properties.Any()) //没找到与字段名相同的属性
                    {
                        continue;
                    }

                    PropertyInfo propertyInfo = properties.First();
                    object obj = (dataRow[dataColumn.ColumnName] == DBNull.Value) ? "" : dataRow[dataColumn.ColumnName];
                    if (!string.IsNullOrEmpty(obj.ToString()))
                    {
                        if (dataColumn.DataType == typeof(DateTime) && propertyInfo.PropertyType == typeof(String))
                        {
                            obj = ((DateTime)dataRow[dataColumn.ColumnName]).ToString();
                        }

                        if (dataColumn.DataType == typeof(String) && propertyInfo.PropertyType == typeof(DateTime))
                        {
                            DateTime temp;
                            obj = DateTime.TryParse(obj.ToString(), out temp);
                        }
                    }
                    //时间类型或者整形,则跳过赋值
                    if (string.IsNullOrEmpty(obj.ToString())
                        && (propertyInfo.PropertyType == typeof(DateTime)
                            || propertyInfo.PropertyType == typeof(int)
                            || propertyInfo.PropertyType == typeof(Int16)
                            || propertyInfo.PropertyType == typeof(Int32)
                            || propertyInfo.PropertyType == typeof(Int64)
                           )
                        )
                    {
                        continue;
                    }
                    try
                    {
                        propertyInfo.SetValue(t, obj, null);
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                    }
                }
            }

            return lstT;
        }

        /// <summary>
        /// DataTable转为List类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static IList<T> ConvertDataTableList<T>(DataTable dataTable)
        {
            if (!dataTable.HasData())
            {
                return new List<T>();
            }

            IList<T> lstT = new List<T>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow == null)
                {
                    continue;
                }
                try
                {
                    T type = default(T);
                    Type tbType = typeof(T);
                    type = (T)tbType.Assembly.CreateInstance(tbType.FullName);
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        var column = dataTable.Columns[j];
                        if (string.IsNullOrWhiteSpace(column.ColumnName))
                        {
                            continue;
                        }
                        try
                        {
                            PropertyInfo pi = tbType.GetProperty(column.ColumnName);
                            if (pi == null || dataRow[j] == null || DBNull.Value == dataRow[j])
                            {
                                continue;
                            }
                            pi.SetValue(type, dataRow[j], null);
                        }
                        catch
                        {
                            continue;
                        }

                    }
                    lstT.Add(type);
                }
                catch
                {
                    continue;
                }
            }
            return lstT;
        }

    }
}