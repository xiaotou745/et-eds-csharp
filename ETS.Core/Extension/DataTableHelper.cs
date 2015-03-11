using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

    }
}
