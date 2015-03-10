using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;

namespace ETS.IO
{
	/// <summary>
	/// Excel数据IO类
	/// </summary>
	public class Excel
	{
		/// <summary>
		/// 从Excel获取表数据
		/// </summary>
		/// <param name="path">Excel file directory</param>
		/// <param name="sql">sql query string</param>
		/// <returns></returns>
		public static DataSet GetDataSetFromExcel(string path, string sql)
		{
			string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";" + "Extended Properties='Excel 8.0;HDR=no;IMEX=1;'";
			DataSet ds = null;
			using (OleDbConnection conn = new OleDbConnection(strConn))
			{
				conn.Open();
				OleDbDataAdapter myCommand = new OleDbDataAdapter(sql, strConn);
				ds = new DataSet();
				myCommand.Fill(ds);
				myCommand.Dispose();
			}
			if (ds.Tables[0].Rows.Count > 1)
			{
				ds.Tables[0].Rows.RemoveAt(0);
			}
			foreach (DataTable dt in ds.Tables)
			{
				foreach (DataRow row in dt.Rows)
				{
					for (int i = 0; i < dt.Columns.Count; i++)
					{
						string col = row[i].ToString().ToLower();
						if (col.IndexOf("e+") > 0)
						{
							string[] arr = col.Split('e');
							double dCol = double.Parse(arr[0]) * (double)Math.Pow(10, int.Parse(arr[1]));
							row[i] = dCol.ToString();
						}
					}
				}
			}
			return ds;
		}

        //// <summary>
        /// 读取Excel文件，将内容存储在DataSet中
        /// </summary>
        /// <param name="opnFileName">带路径的Excel文件名</param>
        /// <returns>DataSet</returns>
        public static DataSet ExcelToDataSet(string opnFileName)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+opnFileName+";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=2\"";
            OleDbConnection conn = new OleDbConnection(strConn);            
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = new DataSet();
            strExcel = "select * from [sheet1$]";
            try

           {
                conn.Open();
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                myCommand.Fill(ds,"dtSource");
                return ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入出错：" + ex, "错误信息");
                return ds;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
		}

		#region Import from Excel 2003/2007
		#region Excel ConnectionStrings

		/// <summary>
		/// "HDR=Yes;" indicates that the first row contains columnnames, not data. "HDR=No;" indicates the opposite.
		/// "IMEX=1;" tells the driver to always read "intermixed" (numbers, dates, strings etc) data columns as text. 
		/// Note that this option might affect excel sheet write access negative.
		/// SQL syntax "SELECT * FROM [sheet1$]". I.e. excel worksheet name followed by a "$" and wrapped in "[" "]" brackets.
		/// </summary>
		private const string FOR_EXCEL =
			"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = {0};Extended Properties =\"Excel 8.0;HDR=Yes;IMEX=1;\"";

		/// <summary>
		/// This one is for connecting to Excel 2007 files with the Xlsx file extension. That is the Office Open XML format with macros disabled.
		/// </summary>
		private const string FOR_EXCEL2007 =
			"Provider=Microsoft.ACE.OLEDB.12.0;Data Source = {0};Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1;\"";
		
		/// <summary>
		/// 连接字符串列表
		/// </summary>
		private static readonly List<string> connectionStrings = new List<string>();

		/// <summary>
		/// excel查询模板sql语句
		/// </summary>
		private const string SQL_TEMPLATE_FOR_EXCEL_QUERY = "SELECT * FROM [{0}]";
		#endregion

		public static DataSet ExcelToDataSetFor03And07(string filePath)
		{
			SetExcelConnectionStrings();

			DataSet result = new DataSet();

			using (OleDbConnection connection = OleDbConnectionOpened(filePath))
			{
                if(connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
				List<string> sheetNames = GetSheetNames(connection);

				OleDbDataAdapter adapter = new OleDbDataAdapter();
				OleDbCommand oleDbCommand = new OleDbCommand(SQL_TEMPLATE_FOR_EXCEL_QUERY, connection);

				foreach (string sheetName in sheetNames)
				{
					oleDbCommand.CommandText = string.Format(SQL_TEMPLATE_FOR_EXCEL_QUERY, sheetName);
					adapter.SelectCommand = oleDbCommand;

					adapter.Fill(result, sheetName);
				}

				oleDbCommand.Dispose();
				adapter.Dispose();
			}

			return result;
		}

		private static OleDbConnection OleDbConnectionOpened(string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException(string.Format("the file:{0} is not exists ", filePath));
			}
			OleDbConnection connection = new OleDbConnection();

			IEnumerator<string> enumerator = connectionStrings.GetEnumerator();
			while (connection.State == ConnectionState.Closed && enumerator.MoveNext())
			{
				connection.ConnectionString = String.Format(enumerator.Current, filePath);
				try
				{
					connection.Open();
				}
				catch
				{
				}
			}

			return connection;
		}
		private static List<string> GetSheetNames(OleDbConnection ocon)
		{
			List<string> result = new List<string>();

			DataTable dtSchema = ocon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
			if (dtSchema != null)
			{
				foreach (DataRow dr in dtSchema.Rows)
				{
					result.Add(dr[2].ToString().Trim());
				}
				dtSchema.Dispose();
			}

			return result;
		}
		private static void SetExcelConnectionStrings() 
		{
			if(connectionStrings.Count == 0)
			{
				connectionStrings.Add(FOR_EXCEL);
				connectionStrings.Add(FOR_EXCEL2007);
			}
		}
		#endregion

		///// <summary>
		///// 从csv获取表数据
		///// </summary>
		///// <param name="path">csv file path</param>
		//public static DataTable GetDataSetFromCSV(string path)
		//{
		//    string strConn = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + path + ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
		//    string tableName = System.IO.Path.GetFileNameWithoutExtension(path);
		//    string sql = string.Format("select * from [{0}]", tableName);
		//    DataSet ds = null;
		//    using (OleDbConnection conn = new OleDbConnection(strConn))
		//    {
		//        conn.Open();
		//        OleDbDataAdapter myCommand = new OleDbDataAdapter(sql, strConn);
		//        ds = new DataSet();
		//        myCommand.Fill(ds);
		//        myCommand.Dispose();
		//    }
		//    return ds.Tables[0];
		//}

		/// <summary>
		/// Excel执行插入、更新、删除操作
		/// </summary>
		/// <param name="path">Excel file directory</param>
		/// <param name="sql">sql query string</param>
		public static void ExcelExecute(string path, string sql)
		{
			string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";" + "Extended Properties=Excel 8.0;";
			using (OleDbConnection conn = new OleDbConnection(strConn))
			{
				conn.Open();
				OleDbCommand myCommand = new OleDbCommand(sql, conn);
				myCommand.ExecuteNonQuery();
				myCommand.Dispose();
			}
		}

		/// <summary>
		/// DataGridView数据导出为CSV
		/// </summary>
		/// <param name="gridForm">DataGridView instance</param>
		/// <param name="saveFileDialog">SaveFileDialog instance</param>
		public static bool OutputCSVFromDataGridView(DataGridView gridForm, SaveFileDialog saveFileDialog)
		{
			return OutputCSVFromDataGridView(gridForm, saveFileDialog, true);
		}

		/// <summary>
		/// DataGridView数据导出为CSV
		/// </summary>
		/// <param name="gridForm">DataGridView instance</param>
		/// <param name="saveFileDialog">SaveFileDialog instance</param>
		public static bool OutputCSVFromDataGridView(DataGridView gridForm, SaveFileDialog saveFileDialog,bool visibleOnly)
		{
			if (gridForm.Rows.Count == 0)
			{
				return false;
			}
			DialogResult rs = saveFileDialog.ShowDialog();
			if (rs != DialogResult.OK)
			{
				return false;
			}
			string header = "";
			foreach (DataGridViewColumn col in gridForm.Columns)
			{
				if (visibleOnly && !col.Visible) continue;
				header += col.HeaderText + ",";
			}
			header = header.TrimEnd(',');
			header += "\r\n";
			StringBuilder txt = new StringBuilder();
			txt.Append(header);
			foreach (DataGridViewRow gr in gridForm.Rows)
			{
				string strRow = string.Empty;
				foreach (DataGridViewCell cell in gr.Cells)
				{
					if (visibleOnly && !cell.Visible) continue;
					string v = "";
					if (cell.Value!=null)
					{
						v = cell.Value.ToString();  
					}
					if (Regex.IsMatch(v, @"^0\d+$|^\d+E\d+$", RegexOptions.IgnoreCase))
					{
						v = v + "\t";
					}
					strRow += v + ",";
				}
				strRow = strRow.TrimEnd(',');
				strRow += "\r\n";
				txt.Append(strRow);
			}
			FileIO.SaveTextFile(saveFileDialog.FileName, txt.ToString(), Encoding.GetEncoding("GB2312"));
			return true;
		}

		/// <summary>
		/// 格式化CSV格式的字符串
		/// </summary>
		/// <param name="s">string</param>
		/// <returns></returns>
		public static string ReplaceBlankForCSV(string s)
		{
			string t = Regex.Replace(s, @"\s{2,}", " ");
			t = t.Replace("\r", " ");
			t = t.Replace("\n", " ");
			t = t.Replace(',', '，');//半角逗号替换为全角
			return t;
		}

		/// <summary>
		/// DataGridView数据导出为CSV
		/// </summary>
		/// <param name="dr">DataReader</param>
		/// <param name="columnName">column name array</param>
		/// <param name="saveFileDialog">SaveFileDialog instance</param>
		public static bool OutputCSVFromDataReader(DbDataReader dr, string[] columnName, SaveFileDialog saveFileDialog)
		{
			if (!dr.HasRows)
			{
				dr.Dispose();
				return false;
			}
			DialogResult rs = saveFileDialog.ShowDialog();
			if (rs != DialogResult.OK)
			{
				return false;
			}
			string header = "";
			if (columnName != null)
			{
				foreach (string name in columnName)
				{
					header += name + ",";
				}
				header = header.TrimEnd(',');
			}
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(header);
			while (dr.Read())
			{
				string strRow = string.Empty;
				for (int i = 0; i < dr.FieldCount; i++)
				{
					string v =  dr[i].ToString();
					if(Regex.IsMatch(v,@"^0\d+$|^\d+E\d+$", RegexOptions.IgnoreCase))
					{
						v = v + "\t";
					}
					strRow += v+ (( i==dr.FieldCount - 1 ) ? "" : ",");
				}
				sb.AppendLine(strRow);
			}
			FileIO.SaveTextFile(saveFileDialog.FileName, sb.ToString(), Encoding.GetEncoding("GB2312"));
			dr.Close();
			dr.Dispose();
			return true;
		}

		/// <summary>
		/// DataTable数据导出为CSV
		/// Modifed:QiaoWenhui
		/// </summary>
		/// <param name="dt">DataTable</param>
		/// <param name="saveFileDialog">SaveFileDialog instance</param>
		public static bool OutputCSVFromDataTable(DataTable dt, SaveFileDialog saveFileDialog)
		{
			if (Common.DataTableIsEmpty(dt))
			{
				return false;
			}
			DialogResult rs = saveFileDialog.ShowDialog();
			if (rs != DialogResult.OK)
			{
				return false;
			}
			string header = "";
			foreach (DataColumn col in dt.Columns)
			{
				header += col.ColumnName + ",";
			}
			header = header.TrimEnd(',');
			header += "\r\n";
			StringBuilder txt = new StringBuilder();
			txt.Append(header);
			string v = "";
			foreach (DataRow dr in dt.Rows)
			{
				string strRow = string.Empty;
				for (int i = 0; i < dt.Columns.Count; i++)
				{
					v = dr[i].ToString();
					if (Regex.IsMatch(v, @"^0\d+$|^\d+E\d+$", RegexOptions.IgnoreCase))
					{
						v = v + "\t";
					}
					strRow += (v + (i == dt.Columns.Count - 1 ? "" : ","));
				}
				strRow = strRow + "\r\n";
				txt.Append(strRow);
			}
			FileIO.SaveTextFile(saveFileDialog.FileName, txt.ToString(), Encoding.GetEncoding("GB2312"));
			return true;
		}

		/// <summary>
		/// DataTable数据导出为CSV(不弹出保存对话框)
		/// </summary>
		/// <param name="dt">DataTable</param>
		/// <param name="path"></param>
		public static bool OutputCSVFromDataTableNoShow(DataTable dt,string path)
		{
			if (Common.DataTableIsEmpty(dt))
			{
				return false;
			}
			string header = "";
			foreach (DataColumn col in dt.Columns)
			{
				header += col.ColumnName + ",";
			}
			header = header.TrimEnd(',');
			header += "\r\n";
			StringBuilder txt = new StringBuilder();
			txt.Append(header);
			string v = "";
			foreach (DataRow dr in dt.Rows)
			{
				string strRow = string.Empty;
				for (int i = 0; i < dt.Columns.Count; i++)
				{
					v = dr[i].ToString();
					if (Regex.IsMatch(v, @"^0\d+$|^\d+E\d+$", RegexOptions.IgnoreCase))
					{
						v = v + "\t";
					}
					strRow += (v + (i == dt.Columns.Count - 1 ? "" : ","));
				}
				strRow = strRow + "\r\n";
				txt.Append(strRow);
			}
			FileIO.SaveTextFile(path, txt.ToString(), Encoding.GetEncoding("GB2312"));
			return true;
		}

		/// <summary>
		/// 导出DataTable为XLS，并打开生成的XLS
		/// </summary>
		/// <param name="dt">表</param>
		/// <param name="saveFileDialog">对话框</param>
		/// <returns></returns>
		public static bool OutputXLSFromDataTable(DataTable dt, SaveFileDialog saveFileDialog)
		{
			return OutputXLSFromDataTable(null, dt, saveFileDialog);
		}

		/// <summary>
		/// 导出DataTable为XLS，并打开生成的XLS
		/// </summary>
		/// <param name="columns">列名</param>
		/// <param name="dt">表</param>
		/// <param name="saveFileDialog">对话框</param>
		/// <returns></returns>
		public static bool OutputXLSFromDataTable(string[] columns ,DataTable dt, SaveFileDialog saveFileDialog)
		{
			if (Common.DataTableIsEmpty(dt))
			{
				return false;
			}
			DialogResult rs = saveFileDialog.ShowDialog();
			if (rs != DialogResult.OK)
			{
				return false;
			}
			try
			{
				ExcelWriter excel = new ExcelWriter(saveFileDialog.FileName);
				excel.BeginWrite();
				short cols = 0;
				if (columns == null || columns.Length == 0)
				{//若没有传列名，则以dt的列名做为Excel的列名
					foreach (DataColumn col in dt.Columns)
					{
						excel.WriteString(0, cols, col.ColumnName);
						cols++;
					}
				}
				else
				{
					foreach (string column in columns)
					{
						excel.WriteString(0, cols, column);
						cols++;
					}
				}
                
				short rows = 1;
				foreach (DataRow dr in dt.Rows)
				{
					cols = 0;
					foreach (DataColumn col in dt.Columns)
					{
						excel.WriteString(rows, cols, dr[col].ToString());
						cols++;
					}
					rows++;
				}
				excel.EndWrite();
				Process.Start(saveFileDialog.FileName);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
        /// 导出DataTable为XLS，并打开生成的XLS
        /// </summary>
        /// <param name="columns">列名</param>
        /// <param name="dt">表</param>
        /// <param name="saveFileDialog">对话框</param>
        /// <returns></returns>
        public static bool OutputXLSFromDataTable(string[] columns, DataTable dt, string strPath)
        {
            if (Common.DataTableIsEmpty(dt))
            {
                return false;
            }
            try
            {
                if(File.Exists(strPath))
                {
                   File.Delete(strPath);
                }
                ExcelWriter excel = new ExcelWriter(strPath);
                excel.BeginWrite();
                short cols = 0;
                if (columns == null || columns.Length == 0)
                {//若没有传列名，则以dt的列名做为Excel的列名
                    foreach (DataColumn col in dt.Columns)
                    {
                        excel.WriteString(0, cols, col.ColumnName);
                        cols++;
                    }
                }
                else
                {
                    foreach (string column in columns)
                    {
                        excel.WriteString(0, cols, column);
                        cols++;
                    }
                }

                short rows = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    cols = 0;
                    foreach (DataColumn col in dt.Columns)
                    {
                        excel.WriteString(rows, cols, dr[col].ToString());
                        cols++;
                    }
                    rows++;
                }
                excel.EndWrite();
                return true;
            }
            catch
            {
                return false;
            }
        }


		/// <summary>
		/// DataTable数据导出为xls(只导出可见列)
		/// </summary>
		/// <param name="grid">DataGridView</param>
		/// <param name="saveFileDialog">SaveFileDialog instance</param>
		public static bool OutputXLSFromDataGridView(DataGridView grid, SaveFileDialog saveFileDialog)
		{
			return OutputXLSFromDataGridView(grid, saveFileDialog, true);
		}

		/// <summary>
		/// DataTable数据导出为xls
		/// </summary>
		/// <param name="grid">DataGridView</param>
		/// <param name="saveFileDialog">SaveFileDialog instance</param>
		/// <param name="visibleOnly">只导出可见的列</param>
		public static bool OutputXLSFromDataGridView(DataGridView  grid, SaveFileDialog saveFileDialog,bool visibleOnly)
		{
			return OutputXLSFromDataGridView(grid, saveFileDialog, visibleOnly, true);
		}
		/// <summary>
		/// DataTable数据导出为xls
		/// </summary>
		/// <param name="grid">DataGridView</param>
		/// <param name="saveFileDialog">SaveFileDialog instance</param>
		/// <param name="visibleOnly">只导出可见的列</param>
		/// <param name="includeNewRow">是否导出新行</param>
		public static bool OutputXLSFromDataGridView(DataGridView grid, SaveFileDialog saveFileDialog, bool visibleOnly, bool includeNewRow)
		{
			if (grid == null || grid.Rows.Count == 0)
			{
				return false;
			}
			DialogResult rs = saveFileDialog.ShowDialog();
			if (rs != DialogResult.OK)
			{
				return false;
			}
			try
			{
				ExcelWriter excel = new ExcelWriter(saveFileDialog.FileName);
				excel.BeginWrite();
				short cols = 0;
				foreach (DataGridViewColumn col in grid.Columns)
				{
					if (visibleOnly && !col.Visible) continue;
					string column = col.HeaderText;
					excel.WriteString(0, cols, column);
					cols++;
				}

				short rows = 1;
				foreach (DataGridViewRow gr in grid.Rows)
				{
					if (!includeNewRow && gr.IsNewRow)
					{
						continue;
					}
					cols = 0;
					foreach (DataGridViewCell cell in gr.Cells)
					{
						if (visibleOnly && !cell.Visible) continue;
						excel.WriteString(rows, cols, cell.Value == null ? string.Empty : cell.Value.ToString());
						cols++;
					}
					rows++;
				}
				excel.EndWrite();
				Process.Start(saveFileDialog.FileName);
				return true;
			}
			catch
			{
				return false;
			}
		}


		/// <summary>
		/// 导出DataReader为XLS，并打开生成的XLS
		/// </summary>
		/// <param name="columns">列名</param>
		/// <param name="dr">DataReader</param>
		/// <param name="saveFileDialog">对话框</param>
		/// <returns></returns>
		public static void OutputXLSFromDataReader(string[] columns, IDataReader  dr, SaveFileDialog saveFileDialog)
		{ 
			DialogResult rs = saveFileDialog.ShowDialog();
			if (rs != DialogResult.OK)
			{
				return ;
			}
      
			ExcelWriter excel = new ExcelWriter(saveFileDialog.FileName);
			excel.BeginWrite();
			short cols = 0; 
			foreach (string column in columns)
			{
				excel.WriteString(0, cols, column);
				cols++;
			} 
			short rows = 1;
			using (dr)
			{
				while (dr.Read())
				{
					cols = 0;
					while(cols < dr.FieldCount)
					{
						excel.WriteString(rows, cols, dr[cols].ToString());
						cols++;
					}
					rows++;
				}
			} 
			excel.EndWrite();
			Process.Start(saveFileDialog.FileName);
          
		}

		/// <summary>
		/// 导出Array为XLS，并打开生成的XLS
		/// </summary>
		/// <param name="array">二维数组</param>
		/// <param name="saveFileDialog">对话框</param>
		/// <returns></returns>
		public static void OutputXLSFromArray(string[,] array, SaveFileDialog saveFileDialog)
		{
			DialogResult rs = saveFileDialog.ShowDialog();
			if (rs != DialogResult.OK)
			{
				return ;
			}
			//try
			//{
			ExcelWriter excel = new ExcelWriter(saveFileDialog.FileName);
			excel.BeginWrite();
			short cols = 0; 
			short rows = 0;
			int rowLength = array.GetUpperBound(0) + 1;
			int columnLength = array.GetUpperBound(1) + 1;
			while(rows <  rowLength)
			{
				cols = 0;
				while(cols < columnLength)
				{
					excel.WriteString(rows, cols, array[rows,cols]);
					cols++;
				}
				rows++;
			}
             
			excel.EndWrite();
			Process.Start(saveFileDialog.FileName);
 
			//}
			//catch
			//{
			//    return false;
			//}
		}

		/// <summary>
		/// 导出Array为XLS，并打开生成的XLS
		/// </summary>
		/// <param name="array">数组</param>
		/// <param name="saveFileDialog">对话框</param>
		/// <returns></returns>
		public static void OutputXLSFromArray(string[] array,  SaveFileDialog saveFileDialog)
		{
			DialogResult rs = saveFileDialog.ShowDialog();
			if (rs != DialogResult.OK)
			{
				return ;
			}
 
			ExcelWriter excel = new ExcelWriter(saveFileDialog.FileName);
			excel.BeginWrite();
			short cols = 0;

			while (cols < array.Length)
			{
				excel.WriteString(0, cols, array[cols]);
				cols++;
			} 

			excel.EndWrite();
			Process.Start(saveFileDialog.FileName);
          
		}

		/// <summary>
		/// 获取Excel文件的第一个表名 
		/// Excel文件中第一个表名的缺省值是Sheet1$, 但有时也会被改变为其他名字. 如果需要在C#中使用OleDb读写Excel文件, 就需要知道这个名字是什么. 以下代码就是实现这个功能的:
		/// </summary>
		/// <param name="excelFileName"></param>
		/// <returns></returns>
		public static string GetFirstTableName(string excelFileName)
		{
			string tableName = null;
			if (File.Exists(excelFileName))
			{
				using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet." +
				                                                  "OLEDB.4.0;Extended Properties=\"Excel 8.0\";Data Source=" + excelFileName))
				{
					conn.Open();
					DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
					tableName = dt.Rows[0][2].ToString().Trim();
				}
				if (!string.IsNullOrEmpty(tableName))
				{
					tableName = tableName.Replace("'", "");
				}
			}
			return tableName;
		}

		/// <summary>
		/// 获取第1张表的数据
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static DataTable GetFirstDataTable(string fileName)
		{
			string table = GetFirstTableName(fileName);
			if (string.IsNullOrEmpty(table)) return null;
			string sql = "select * from [" + table+"]";
			return GetDataSetFromExcel(fileName, sql).Tables[0];
		}
	}
}