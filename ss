[33mcommit c0311a96eeab5c4a8f886d7790a828e267a66021[m
Author: douhaichao <dou631@163.com>
Date:   Wed Mar 11 13:28:43 2015 +0800

    ado分页填加

[1mdiff --git a/ETS.Core/Config.cs b/ETS.Core/Config.cs[m
[1mnew file mode 100644[m
[1mindex 0000000..17057fc[m
[1m--- /dev/null[m
[1m+++ b/ETS.Core/Config.cs[m
[36m@@ -0,0 +1,43 @@[m
[32m+[m[32m﻿using System;[m
[32m+[m[32musing System.Collections.Generic;[m
[32m+[m[32musing System.Linq;[m
[32m+[m[32musing System.Text;[m
[32m+[m
[32m+[m[32mnamespace ETS[m
[32m+[m[32m{[m
[32m+[m[32m    public class Config[m
[32m+[m[32m    {[m
[32m+[m[32m        /// <summary>[m
[32m+[m[32m        /// 读库[m
[32m+[m[32m        /// </summary>[m
[32m+[m[32m        public static string SuperMan_Read { get { return ConfigConnectionStrings("SuperMan_Read"); } }[m
[32m+[m[32m        /// <summary>[m
[32m+[m[32m        /// 写库[m
[32m+[m[32m        /// </summary>[m
[32m+[m[32m        public static string SuperMan_Write { get { return ConfigConnectionStrings("SuperMan_Write"); } }[m
[32m+[m
[32m+[m[32m        #region 取Web.Config值[m
[32m+[m
[32m+[m[32m        /// <summary>[m
[32m+[m[32m        /// 取Web.Config值[m
[32m+[m
[32m+[m[32m        /// </summary>[m
[32m+[m[32m        /// <param name="KeyName"></param>[m
[32m+[m[32m        /// <returns></returns>[m
[32m+[m[32m        public static string ConfigKey(string KeyName)[m
[32m+[m[32m        {[m
[32m+[m[32m            return System.Configuration.ConfigurationManager.AppSettings[KeyName];[m
[32m+[m[32m        }[m
[32m+[m[32m        /// <summary>[m
[32m+[m[32m        /// 取wenconfig数据库连接字符串的值[m
[32m+[m[32m        /// </summary>[m
[32m+[m[32m        /// <param name="name"></param>[m
[32m+[m[32m        /// <returns></returns>[m
[32m+[m[32m        public static string ConfigConnectionStrings(string name)[m
[32m+[m[32m        {[m
[32m+[m[32m            return System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        #endregion[m
[32m+[m[32m    }[m
[32m+[m[32m}[m
[1mdiff --git a/ETS.Core/ETS.Core.csproj b/ETS.Core/ETS.Core.csproj[m
[1mindex 72539a2..860a97b 100644[m
[1m--- a/ETS.Core/ETS.Core.csproj[m
[1m+++ b/ETS.Core/ETS.Core.csproj[m
[36m@@ -48,11 +48,13 @@[m
     <Reference Include="System.Xml" />[m
   </ItemGroup>[m
   <ItemGroup>[m
[32m+[m[32m    <Compile Include="Config.cs" />[m
     <Compile Include="Expand\AttribDescription.cs" />[m
     <Compile Include="Expand\EnumerableExpand.cs" />[m
     <Compile Include="Extension\DataRowExtension.cs" />[m
     <Compile Include="Extension\DataSetExtenstion.cs" />[m
     <Compile Include="Extension\DataTableExtension.cs" />[m
[32m+[m[32m    <Compile Include="Extension\DataTableHelper.cs" />[m
     <Compile Include="Extension\ListExtensions.cs" />[m
     <Compile Include="Extension\StringExtension.cs" />[m
     <Compile Include="IO\Common.cs" />[m
[1mdiff --git a/ETS.Core/Extension/DataTableHelper.cs b/ETS.Core/Extension/DataTableHelper.cs[m
[1mnew file mode 100644[m
[1mindex 0000000..ddec138[m
[1m--- /dev/null[m
[1m+++ b/ETS.Core/Extension/DataTableHelper.cs[m
[36m@@ -0,0 +1,123 @@[m
[32m+[m[32m﻿using System;[m
[32m+[m[32musing System.Collections.Generic;[m
[32m+[m[32musing System.Data;[m
[32m+[m[32musing System.Linq;[m
[32m+[m[32musing System.Text;[m
[32m+[m
[32m+[m[32mnamespace ETS.Extension[m
[32m+[m[32m{[m
[32m+[m[32m    public class DataTableHelper[m
[32m+[m[32m    {[m
[32m+[m[32m        /// <summary>[m
[32m+[m[32m        /// 作者：zhuzh[m
[32m+[m[32m        /// 时间：2010-12-20[m
[32m+[m[32m        /// 功能：在DataSet 提取 DataTable[m
[32m+[m[32m        /// </summary>[m
[32m+[m[32m        /// <param name="ds"></param>[m
[32m+[m[32m        /// <returns></returns>[m
[32m+[m[32m        public static DataTable GetTable(DataSet ds)[m
[32m+[m[32m        {[m
[32m+[m[32m            DataTable dt = new DataTable();[m
[32m+[m[32m            if (!CheckDs(ds))[m
[32m+[m[32m                return dt;[m
[32m+[m[32m            if (ds.Tables.Count <= 0)[m
[32m+[m[32m                return dt;[m
[32m+[m[32m            if (ds.Tables[0] == null)[m
[32m+[m[32m                return dt;[m
[32m+[m[32m            if (ds.Tables[0].Rows.Count <= 0)[m
[32m+[m[32m                return dt;[m
[32m+[m[32m            if (ds.Tables[0].Rows[0] == null)[m
[32m+[m[32m                return dt;[m
[32m+[m[32m            return ds.Tables[0];[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        /// <summary>[m
[32m+[m[32m        /// 作者：zhuzh[m
[32m+[m[32m        /// 时间：2010-12-21[m
[32m+[m[32m        /// 功能：在DataSet 提取 DataRow[m
[32m+[m[32m        /// </summary>[m
[32m+[m[32m        /// <param name="ds"></param>[m
[32m+[m[32m        /// <returns></returns>[m
[32m+[m[32m        public static DataRow GetRow(DataSet ds)[m
[32m+[m[32m        {[m
[32m+[m[32m            DataTable dt = new DataTable();[m
[32m+[m[32m            DataRow dr = dt.NewRow();[m
[32m+[m[32m            dt = GetTable(ds);[m
[32m+[m[32m            if (CheckDt(dt))[m
[32m+[m[32m                return dr;[m
[32m+[m[32m            if (dt.Rows.Count <= 0 || dt.Rows[0] == null)[m
[32m+[m[32m                return dr;[m
[32m+[m[32m            dr = dt.Rows[0];[m
[32m+[m[32m            return dr;[m
[32m+[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        #region 检查数据库返回对象是否有效[m
[32m+[m
[32m+[m[32m        /// <summary>[m
[32m+[m[32m        /// 检查数据库返回对象是否有效[m
[32m+[m[32m        /// </summary>[m
[32m+[m[32m        /// <param name="obj"></param>[m
[32m+[m[32m        /// <param name="defaultValue"></param>[m
[32m+[m[32m        /// <returns></returns>[m
[32m+[m[32m        public static object CheckDbObject(object obj, object defaultValue)[m
[32m+[m[32m        {[m
[32m+[m[32m            object temp = defaultValue;[m
[32m+[m
[32m+[m[32m            if (obj != null && obj != DBNull.Value && !string.IsNullOrEmpty(obj.ToString().Trim()))[m
[32m+[m[32m            {[m
[32m+[m[32m                temp = obj;[m
[32m+[m[32m            }[m
[32m+[m
[32m+[m[32m            return temp;[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        #endregion[m
[32m+[m
[32m+[m[32m        #region 校验DataSet中是否有数据[m
[32m+[m
[32m+[m[32m        public static bool CheckDs(DataSet ds)[m
[32m+[m[32m        {[m
[32m+[m[32m            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)[m
[32m+[m[32m            {[m
[32m+[m[32m                return true;[m
[32m+[m[32m            }[m
[32m+[m[32m            else[m
[32m+[m[32m            {[m
[32m+[m[32m                return false;[m
[32m+[m[32m            }[m
[32m+[m[32m        }[m
[32m+[m[32m        /// <summary>[m
[32m+[m[32m        /// 获取dataset的第一表，第一行，第一列。无效返回空字符串。[m
[32m+[m[32m        /// </summary>[m
[32m+[m[32m        /// <param name="ds"></param>[m
[32m+[m[32m        /// <returns></returns>[m
[32m+[m[32m        public static string GetFirstString(DataSet ds)[m
[32m+[m[32m        {[m
[32m+[m[32m            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Count > 0)[m
[32m+[m[32m                return ds.Tables[0].Rows[0][0].ToString();[m
[32m+[m[32m            return "";[m
[32m+[m[32m        }[m
[32m+[m[32m        #endregion[m
[32m+[m
[32m+[m[32m        #region 校验DataTable中是否有数据[m
[32m+[m[32m        /// <summary>[m
[32m+[m[32m        /// 功能：dt != null && dt.Rows.Count > 0    return true;[m
[32m+[m[32m        /// </summary>[m
[32m+[m[32m        /// <param name="dt"></param>[m
[32m+[m[32m        /// <returns></returns>[m
[32m+[m[32m        public static bool CheckDt(DataTable dt)[m
[32m+[m[32m        {[m
[32m+[m[32m            if (dt != null && dt.Rows.Count > 0)[m
[32m+[m[32m            {[m
[32m+[m[32m                return true;[m
[32m+[m[32m            }[m
[32m+[m[32m            else[m
[32m+[m[32m            {[m
[32m+[m[32m                return false;[m
[32m+[m[32m            }[m
[32m+[m[32m        }[m
[32m+[m[32m        #endregion[m
[32m+[m
[32m+[m[32m    }[m
[32m+[m[32m}[m
[1mdiff --git a/ETS.Data/Data/PageData/PageHelper.cs b/ETS.Data/Data/PageData/PageHelper.cs[m
[1mnew file mode 100644[m
[1mindex 0000000..4dca955[m
[1m--- /dev/null[m
[1m+++ b/ETS.Data/Data/PageData/PageHelper.cs[m
[36m@@ -0,0 +1,145 @@[m
[32m+[m[32m﻿using ETS.Extension;[m
[32m+[m[32musing System;[m
[32m+[m[32musing System.Collections.Generic;[m
[32m+[m[32musing System.Data;[m
[32m+[m[32musing System.Data.SqlClient;[m
[32m+[m[32musing System.Linq;[m
[32m+[m[32musing System.Text;[m
[32m+[m
[32m+[m[32mnamespace ETS.Data.PageData[m
[32m+[m[32m{[m
[32m+[m[32m    public class PageHelper[m
[32m+[m[32m    {[m
[32m+[m[32m        /// <summary>[m
[32m+[m[32m        /// 存储过程分页[m
[32m+[m[32m        /// </summary>[m
[32m+[m[32m        /// <param name="connectionString">连接字符串（DBConfig）</param>[m
[32m+[m[32m        /// <param name="currentPage">当前页</param>[m
[32m+[m[32m        /// <param name="where_">条件</param>[m
[32m+[m[32m        /// <param name="OrderByColumn">排序条件</param>[m
[32m+[m[32m        /// <param name="ColumnList">列名</param>[m
[32m+[m[32m        /// <param name="TableList">表名</param>[m
[32m+[m[32m        /// <param name="PageSize">每页显示条数</param>[m
[32m+[m[32m        /// <param name="IsAccount">是否返回总条数、总页数</param>[m
[32m+[m[32m        /// <returns></returns>[m
[32m+[m[32m        public PageInfo GetPages(string connectionString, int currentPage, string where_, string OrderByColumn, string ColumnList, string TableList, int PageSize, bool IsAccounte)[m
[32m+[m[32m        {[m
[32m+[m[32m            int TotalRecord = 0;[m
[32m+[m[32m            int TotalPage = 0;[m
[32m+[m[32m            var _table = GetPages(connectionString,currentPage, where_, OrderByColumn, ColumnList, TableList, PageSize, IsAccounte, out TotalRecord, out TotalPage);[m
[32m+[m[32m            var pageInfo = new PageInfo(TotalRecord, currentPage, _table, TotalPage);[m
[32m+[m[32m            return pageInfo;[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m
[32m+[m
[32m+[m[32m        #region 存储过程分页[m
[32m+[m
[32m+[m[32m        /// <summary>[m
[32m+[m[32m        /// 获取分页数据[m
[32m+[m[32m        /// </summary>[m
[32m+[m[32m        /// <param name="index"></param>[m
[32m+[m[32m        /// <returns></returns>[m
[32m+[m[32m        private DataTable GetPages(string connectionString, int currentPage, string where_, string OrderByColumn, string ColumnList, string TableList, int PageSize, bool IsAccount, out int TotalRecord, out int TotalPage)[m
[32m+[m[32m        {[m
[32m+[m[32m            DataSet set = new DataSet();[m
[32m+[m[32m            SqlParameter[] parm = {[m[41m [m
[32m+[m[32m                                  new SqlParameter("@OrderByColumn",SqlDbType.NVarChar,500),[m
[32m+[m[32m                                  new SqlParameter("@ColumnList",SqlDbType.NVarChar,8000),[m
[32m+[m[32m                                  new SqlParameter("@TableList",SqlDbType.NVarChar,4000),[m
[32m+[m[32m                                  new SqlParameter("@Condition",SqlDbType.NVarChar,4000),[m
[32m+[m[32m                                  new SqlParameter("@PageSize",SqlDbType.Int,4),[m
[32m+[m[32m                                  new SqlParameter("@CurrentPage",SqlDbType.Int,4),[m
[32m+[m[32m                                  new SqlParameter("@IsAccount",SqlDbType.Bit,1),[m
[32m+[m[32m                                  new SqlParameter("@TotalRecord",SqlDbType.Int),[m
[32m+[m[32m                                  new SqlParameter("@TotalPage",SqlDbType.Int)[m
[32m+[m[32m                                  };[m
[32m+[m[32m            parm[0].Value = OrderByColumn;[m
[32m+[m[32m            parm[1].Value = ColumnList;[m
[32m+[m[32m            parm[2].Value = TableList;[m
[32m+[m[32m            parm[3].Value = where_;[m
[32m+[m[32m            parm[4].Value = PageSize;[m
[32m+[m[32m            parm[5].Value = currentPage;[m
[32m+[m[32m            parm[6].Value = IsAccount;[m
[32m+[m[32m            parm[7].Direction = ParameterDirection.Output;[m
[32m+[m[32m            parm[8].Direction = ParameterDirection.Output;[m
[32m+[m[32m            ExecuteDataset(connectionString, set, CommandType.StoredProcedure, "Sp_CustomPage2015_V1", parm);[m
[32m+[m[32m            TotalRecord = 0;[m
[32m+[m[32m            TotalPage = 0;[m
[32m+[m[32m            if (Convert.ToBoolean(DataTableHelper.CheckDbObject(parm[7].Value, 0)) && Convert.ToBoolean(DataTableHelper.CheckDbObject(parm[8].Value, 1)))[m
[32m+[m[32m            {[m
[32m+[m[32m                TotalRecord = Convert.ToInt32(parm[7].Value);[m
[32m+[m[32m                TotalPage = Convert.ToInt32(parm[8].Value);[m
[32m+[m[32m            }[m
[32m+[m[32m            return DataTableHelper.GetTable(set);[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        private void ExecuteDataset(string connectionString, DataSet ds, CommandType commandType, string commandText, params SqlParameter[] commandParameters)[m
[32m+[m[32m        {[m
[32m+[m[32m            using (SqlConnection cn = new SqlConnection(connectionString))[m
[32m+[m[32m            {[m
[32m+[m[32m                cn.Open();[m
[32m+[m[32m                SqlCommand cmd = new SqlCommand();[m
[32m+[m[32m                PrepareCommand(cmd, cn, (SqlTransaction)null, commandType, commandText, commandParameters);[m
[32m+[m[32m                SqlDataAdapter da = new SqlDataAdapter(cmd);[m
[32m+[m[32m                for (int i = 0; i < ds.Tables.Count; ++i)[m
[32m+[m[32m                {[m
[32m+[m[32m                    if (i == 0)[m
[32m+[m[32m                        da.TableMappings.Add("Table", ds.Tables[i].TableName);[m
[32m+[m[32m                    else[m
[32m+[m[32m                        da.TableMappings.Add(string.Concat("Table", i.ToString()), ds.Tables[i].TableName);[m
[32m+[m[32m                }[m
[32m+[m[32m                da.Fill(ds);[m
[32m+[m[32m                cmd.Parameters.Clear();[m
[32m+[m[32m            }[m
[32m+[m[32m        }[m
[32m+[m
[32m+[m[32m        private void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)[m
[32m+[m[32m        {[m
[32m+[m[32m            //if the provided connection is not open, we will open it[m
[32m+[m[32m            if (connection.State != ConnectionState.Open)[m
[32m+[m[32m            {[m
[32m+[m[32m                connection.Open();[m
[32m+[m[32m            }[m
[32m+[m
[32m+[m[32m            //associate the connection with the command[m
[32m+[m[32m            command.Connection = connection;[m
[32m+[m
[32m+[m[32m            //set the command text (stored procedure name or SQL statement)[m
[32m+[m[32m            command.CommandText = commandText;[m
[32m+[m
[32m+[m[32m            //if we were provided a transaction, assign it.[m
[32m+[m[32m            if (transaction != null)[m
[32m+[m[32m            {[m
[32m+[m[32m                command.Transaction = transaction;[m
[32m+[m[32m            }[m
[32m+[m
[32m+[m[32m            //set the command type[m
[32m+[m