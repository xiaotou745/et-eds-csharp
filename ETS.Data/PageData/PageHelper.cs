using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ETS.Extension;
namespace ETS.Data.PageData
{
    public class PageHelper
    {
        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="connectionString">连接字符串（DBConfig）</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="where_">条件</param>
        /// <param name="OrderByColumn">排序条件</param>
        /// <param name="ColumnList">列名</param>
        /// <param name="TableList">表名</param>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="IsAccount">是否返回总条数、总页数</param>
        /// <returns></returns>
        public PageInfo<T> GetPages<T>(string connectionString, int currentPage, string where_, string OrderByColumn, string ColumnList, string TableList, int PageSize, bool IsAccounte)
        {
            try
            {
                int TotalRecord = 0;
                int TotalPage = 0;
                var _table = GetPages(connectionString, currentPage, where_, OrderByColumn, ColumnList, TableList, PageSize, IsAccounte, out TotalRecord, out TotalPage);
                var pageInfo = new PageInfo<T>(TotalRecord, currentPage, DataTableHelper.ConvertDataTableList<T>(_table), TotalPage);
                return pageInfo;
            }
            catch(Exception ex)
            {
                return null;
            }
        }



        #region 存储过程分页

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private DataTable GetPages(string connectionString, int currentPage, string where_, string OrderByColumn, string ColumnList, string TableList, int PageSize, bool IsAccount, out int TotalRecord, out int TotalPage)
        {
            DataSet set = new DataSet();
            SqlParameter[] parm = { 
                                  new SqlParameter("@OrderByColumn",SqlDbType.NVarChar,500),
                                  new SqlParameter("@ColumnList",SqlDbType.NVarChar,8000),
                                  new SqlParameter("@TableList",SqlDbType.NVarChar,4000),
                                  new SqlParameter("@Condition",SqlDbType.NVarChar,4000),
                                  new SqlParameter("@PageSize",SqlDbType.Int,4),
                                  new SqlParameter("@CurrentPage",SqlDbType.Int,4),
                                  new SqlParameter("@IsAccount",SqlDbType.Bit,1),
                                  new SqlParameter("@TotalRecord",SqlDbType.Int),
                                  new SqlParameter("@TotalPage",SqlDbType.Int)
                                  };
            parm[0].Value = OrderByColumn;
            parm[1].Value = ColumnList;
            parm[2].Value = TableList;
            parm[3].Value = where_;
            parm[4].Value = PageSize;
            parm[5].Value = currentPage;
            parm[6].Value = IsAccount;
            parm[7].Direction = ParameterDirection.Output;
            parm[8].Direction = ParameterDirection.Output;
            ExecuteDataset(connectionString, set, CommandType.StoredProcedure, "Sp_CustomPage2015_V1", parm);
            TotalRecord = 0;
            TotalPage = 0;
            if (Convert.ToBoolean(DataTableHelper.CheckDbObject(parm[7].Value, 0)) && Convert.ToBoolean(DataTableHelper.CheckDbObject(parm[8].Value, 1)))
            {
                TotalRecord = Convert.ToInt32(parm[7].Value);
                TotalPage = Convert.ToInt32(parm[8].Value);
            }
            return DataTableHelper.GetTable(set);
        }

        private void ExecuteDataset(string connectionString, DataSet ds, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, cn, (SqlTransaction)null, commandType, commandText, commandParameters);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                for (int i = 0; i < ds.Tables.Count; ++i)
                {
                    if (i == 0)
                        da.TableMappings.Add("Table", ds.Tables[i].TableName);
                    else
                        da.TableMappings.Add(string.Concat("Table", i.ToString()), ds.Tables[i].TableName);
                }
                da.Fill(ds);
                cmd.Parameters.Clear();
            }
        }

        private void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            //if the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            //associate the connection with the command
            command.Connection = connection;

            //set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            //if we were provided a transaction, assign it.
            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            //set the command type
            command.CommandType = commandType;

            //attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }

            return;
        }

        private void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            foreach (SqlParameter p in commandParameters)
            {
                //check for derived output value with no value assigned
                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }

                command.Parameters.Add(p);
            }
        }

        #endregion
    }
}
