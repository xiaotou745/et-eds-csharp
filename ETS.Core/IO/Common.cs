using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ETS.IO
{
	/// <summary>
	/// ���÷���
	/// </summary>
	public class Common
	{
		/// <summary>
		/// �ж�DataSet��������
		/// </summary>
		/// <param name="ds">DataSet</param>
		/// <returns></returns>
		public static bool DataSetIsEmpty(DataSet ds)
		{
			return (ds == null) || (ds.Tables.Count < 1) || DataTableIsEmpty(ds.Tables[0]);
		}

		/// <summary>
		/// �ж�DataTable��������
		/// </summary>
		/// <param name="dt">DataTable</param>
		/// <returns></returns>
		public static bool DataTableIsEmpty(DataTable dt)
		{
			return (dt == null) || (dt.Rows.Count < 1);
		}

		/// <summary>
		/// �ֶ�ֵ����ת��Ϊ�ַ���
		/// </summary>
		/// <param name="arr">�ֶ�ֵ����</param>
		/// <returns>�Զ���ƴ�ӵĲ�ѯ���</returns>
		public static string ValueArrayToQueryString(ArrayList arr)
		{
			if (arr == null || arr.Count == 0)
			{
				return null;
			}
			StringBuilder sb = new StringBuilder();
			int i = 0;
			foreach (string obj in arr)
			{
				sb.AppendFormat("'{0}'{1}", obj, (i < arr.Count - 1) ? "," : "");
				i++;
			}
			return sb.ToString();
		}

		/// <summary>
		/// �ֶ�ֵ����ת��Ϊ�ַ���
		/// </summary>
		/// <param name="arr">�ֶ�ֵ����</param>
		/// <param name="whatever">���ݾ�API����ֵ��������</param>
		/// <returns>�Զ���ƴ�ӵĲ�ѯ���</returns>
		public static string ValueArrayToQueryString(IList<string> arr, bool whatever)
		{
			return ValueArrayToQueryString(arr);
		}

		/// <summary>
		/// �ֶ�ֵ����ת��Ϊ�ַ���
		/// </summary>
		/// <param name="arr">�ֶ�ֵ����</param>
		/// <returns>�Զ���ƴ�ӵĲ�ѯ���</returns>
		public static string ValueArrayToQueryString(IList<string> arr)
		{
			if (arr == null || arr.Count == 0)
			{
				return null;
			}
			StringBuilder sb = new StringBuilder();
			int i = 0;
			foreach (string obj in arr)
			{
				sb.AppendFormat("'{0}'{1}", obj,(i<arr.Count-1) ? "," : "");
				i++;
			}
			return sb.ToString();
		}

		/// <summary>
		/// DataTable��һ�в���Ĭ�����ƺ�ֵ(��"��ѡ��...","0")
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="headerValue"></param>
		/// <param name="headerText"></param>
		public static void DataTableAddHeader(DataTable dt, string headerValue, string headerText)
		{
			if (dt != null && dt.Columns.Count > 1)
			{
				DataRow dr = dt.NewRow();
				dr[0] = headerValue;
				dr[1] = headerText;
				dt.Rows.InsertAt(dr, 0);
			}
			else
			{
				throw new Exception("DataTable must be 2 columns");
			}
		}
		/// <summary>
		/// DataTable������һ�м�¼
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="value"></param>
		/// <param name="text"></param>
		public static void DataTableAddRow(DataTable dt, string value, string text)
		{
			if (dt != null && dt.Columns.Count > 1)
			{
				DataRow dr = dt.NewRow();
				dr[0] = value;
				dr[1] = text;
				dt.Rows.Add(dr);
			}
			else
			{
				throw new Exception("DataTable Add rows error.");
			}

		}
		/// <summary>
		/// �ϲ�DataSet�еĶ��DataTable�����ݵ���0��DataTable��Ҫ��DataTable�Ľṹ��ȫ��ͬ
		/// </summary>
		/// <param name="ds"></param>
		public static DataTable MegerDataTableInDataSet(DataSet ds)
		{
			for (int i = 1; i < ds.Tables.Count; i++)
			{
				foreach (DataRow dr in ds.Tables[i].Rows)
				{
					ds.Tables[0].ImportRow(dr);
				}
			}
			for (int i = 1; i < ds.Tables.Count; i++)
			{
				ds.Tables.RemoveAt(i);
			}
			return ds.Tables.Count > 0 ? ds.Tables[0] : null;
		}

		private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
		{
			if (command == null) throw new ArgumentNullException("command");
			if (commandParameters != null)
			{
				foreach (SqlParameter p in commandParameters)
				{
					if (p != null)
					{
						// Check for derived output value with no value assigned
						if ((p.Direction == ParameterDirection.InputOutput ||
							p.Direction == ParameterDirection.Input) &&
							(p.Value == null))
						{
							p.Value = DBNull.Value;
						}
						command.Parameters.Add(p);
					}
				}
			}
		}

		public static DataSet LongTimeExecuteDataSet(string connectionString,string commandText,CommandType commandType,
			SqlParameter[] parameters,int timeout)
		{
			DataSet ds = new DataSet();
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				using (SqlDataAdapter da = new SqlDataAdapter(commandText, conn))
				{
					da.SelectCommand.CommandTimeout = timeout;
					da.SelectCommand.CommandType = commandType;
					AttachParameters(da.SelectCommand, parameters);
					da.Fill(ds);
				}
			}
			return ds;
		}

		public static int LongTimeExecuteNonQuery(string connectionString, string commandText, CommandType commandType,
			SqlParameter[] parameters, int timeout)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(commandText, conn);
				cmd.CommandTimeout = timeout;
				cmd.CommandType = commandType;
				AttachParameters(cmd, parameters);
				return cmd.ExecuteNonQuery();
			}
		}

		public static string DataTableToSql(DataTable table, string insertSql)
		{
			var sb = new StringBuilder();
			foreach (DataRow row in table.Rows)
			{
				var values = new StringBuilder();
				for (int j = 0; j < table.Columns.Count; j++)
				{
					if (j > 0) values.Append(", ");
					if (row.IsNull(j) || table.Columns[j].DataType.Name=="Byte[]") 
						values.Append("NULL");
					else if (table.Columns[j].DataType == typeof(int) ||
							 table.Columns[j].DataType == typeof(decimal) ||
							 table.Columns[j].DataType == typeof(long) ||
							 table.Columns[j].DataType == typeof(double) ||
							 table.Columns[j].DataType == typeof(float) ||
							 table.Columns[j].DataType == typeof(byte))
						values.Append(row[j].ToString());
					else
						values.AppendFormat("'{0}'",
							row[j].ToString().Replace("\\", "\\\\").Replace("'", "''"));
				}
				sb.AppendFormat(insertSql, row[0], values);
				sb.AppendLine();
			}
			return sb.ToString();
		}
	}
}