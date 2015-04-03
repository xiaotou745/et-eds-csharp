using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Task.Common
{
    public static class DataTableToList
    {
        /// <summary>
        /// DataTable转换成Model对象。
        /// 胡小兵
        /// 2014年3月4日15:49:00
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> GetModelList<T>(this DataTable table) where T : class, new()
        {
            List<T> list = new List<T>();
            // 获得此模型的类型
            //Type type = typeof (T);
            foreach (DataRow item in table.Rows)
            {
                // 根据泛型创建实例
                T t = Activator.CreateInstance<T>();
                // 获得此模型的公共属性
                PropertyInfo[] propertyInfos = t.GetType().GetProperties();
                //Type type = typeof(T);
                //FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

                foreach (var propertyInfo in propertyInfos)
                {
                    string tempName = propertyInfo.Name;
                    if (table.Columns.Contains(tempName))
                    {
                        if (!propertyInfo.CanWrite) continue;
                        object value = item[tempName];
                        Type declaringType = propertyInfo.GetGetMethod().DeclaringType;
                        //typeof(t.GetType().GetField(tempName).FieldType)
                        if (value != DBNull.Value)
                            propertyInfo.SetPropertyValue(t, value);
                    }
                }
                list.Add(t);
            }
            return list;
        }
        /// <summary>
        ///     高效转换成指定的实体对象集合(以数据字段优先匹配，不区分大小写，请保证数据字段和实体字段之间的数据类型一直，否则异常，可以自动赋值枚举)
        ///     只填充实体对象的公共属性，不能给私有变量赋值
        ///     如果DataTable没有任何行返回一个长度为0的集合不会返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns> 
        public static IList<T> ToEntityList<T>(this DataTable dt) where T : class, new()
        {
            IList<T> l = new List<T>();
            dt.ForEachEntityList<T>(l.Add);
            return l;
        }

        /// <summary>
        ///     自动转成实体后foreach
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="action"></param> 
        public static void ForEachEntityList<T>(this DataTable dt, Action<T> action) where T : class, new()
        {
            if (action == null)
                throw new NullReferenceException("action 不能为空");

            if (dt != null && dt.Rows.Count > 0)
            {
                var cacheInvoke = new Dictionary<string, ExpressionInvoke.SetMethodInvokeHandler<T>>();
                var propertyInfoList = typeof(T).GetProperties().ToDictionary(p => p.Name.ToLower());

                foreach (DataColumn dc in dt.Columns)
                {
                    var columnname = dc.ColumnName.ToLower();
                    if (propertyInfoList.ContainsKey(columnname))
                    {
                        var pi = propertyInfoList[columnname];
                        cacheInvoke.Add(pi.Name, ExpressionInvoke.CreateSetterDelegate<T>(pi.GetSetMethod(), pi.PropertyType));
                    }
                }

                foreach (DataRow dr in dt.Rows)
                {
                    var model = new T();
                    foreach (var key in cacheInvoke)
                    {
                        var obj = dr[key.Key];
                        if (obj != DBNull.Value)
                        { 
                            string dataType = dr[key.Key].GetType().Name;
                            if (dataType == "UInt32")
                            {
                                key.Value(model, Convert.ToInt32(obj));
                                continue;
                            }

                            if (dataType == "MySqlDateTime")
                            {
                                //dr[dc.ColumnName].ToString("yyyy-MM-dd HH:mm:ss")
                                key.Value(model, DateTime.Parse(obj.ToString()));
                                continue;
                            }

                            if (dataType == "Int16")
                            {
                                key.Value(model, Convert.ToInt32(obj));
                                continue;
                            }

                            //SByte
                            if (dataType == "SByte")
                            {
                                key.Value(model, Convert.ToInt32(obj));
                                continue;
                            }

                            key.Value(model, obj);
                        }
                    }
                    action(model);
                }
            }
        }

        /// <summary>
        ///     传统反射赋值，效率低但适用于小数据量绑定，可以给私有属性赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> ToEntityListByInvoke<T>(this DataTable dt) where T : class, new()
        {
            IList<T> l = new List<T>();
            if (dt != null && dt.Rows.Count > 0)
            {
                var propertyInfoList = typeof(T).GetProperties();
                foreach (DataRow dr in dt.Rows)
                {
                    var model = new T();
                    foreach (DataColumn dc in dr.Table.Columns)
                    {
                        if (dr[dc.ColumnName] == DBNull.Value) continue;

                        var pi = Array.Find(propertyInfoList, p => p.Name.Equals(dc.ColumnName, StringComparison.CurrentCultureIgnoreCase));
                        if (pi == null) continue;

                        pi.SetPropertyValue(model, dr[dc.ColumnName]);
                    }
                    l.Add(model);
                }
            }
            return l;
        }
    }
}
