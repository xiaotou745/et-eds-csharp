using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ETS.Extension
{
    public class DataTableHelper
    {
        /// <summary>
        /// 作者：zhuzh
        /// 时间：2010-12-20
        /// 功能：在DataSet 提取 DataTable
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static DataTable GetTable(DataSet ds)
        {
            DataTable dt = new DataTable();
            if (!CheckDs(ds))
                return dt;
            if (ds.Tables.Count <= 0)
                return dt;
            if (ds.Tables[0] == null)
                return dt;
            if (ds.Tables[0].Rows.Count <= 0)
                return dt;
            if (ds.Tables[0].Rows[0] == null)
                return dt;
            return ds.Tables[0];
        }

        /// <summary>
        /// 作者：zhuzh
        /// 时间：2010-12-21
        /// 功能：在DataSet 提取 DataRow
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static DataRow GetRow(DataSet ds)
        {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();
            dt = GetTable(ds);
            if (CheckDt(dt))
                return dr;
            if (dt.Rows.Count <= 0 || dt.Rows[0] == null)
                return dr;
            dr = dt.Rows[0];
            return dr;

        }

        #region 检查数据库返回对象是否有效

        /// <summary>
        /// 检查数据库返回对象是否有效
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static object CheckDbObject(object obj, object defaultValue)
        {
            object temp = defaultValue;

            if (obj != null && obj != DBNull.Value && !string.IsNullOrEmpty(obj.ToString().Trim()))
            {
                temp = obj;
            }

            return temp;
        }

        #endregion

        #region 校验DataSet中是否有数据

        public static bool CheckDs(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取dataset的第一表，第一行，第一列。无效返回空字符串。
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string GetFirstString(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Count > 0)
                return ds.Tables[0].Rows[0][0].ToString();
            return "";
        }
        #endregion

        #region 校验DataTable中是否有数据
        /// <summary>
        /// 功能：dt != null && dt.Rows.Count > 0    return true;
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool CheckDt(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

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
                        catch(Exception err)
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
