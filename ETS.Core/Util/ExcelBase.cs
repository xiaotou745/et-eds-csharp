using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;

namespace ETS.Util
{
    internal class ExcelBase
    {
        // Methods
        public ExcelBase()
        {
        }

        internal static T ConvertType<T>(object val)
        {
            if (val != null)
            {
                Type type = typeof(T);
                if (type.IsGenericType)
                {
                    type = type.GetGenericArguments()[0];
                }
                if (type.Name.ToLower() == "string")
                {
                    return (T)val;
                }
                ParameterModifier[] modifiers = new ParameterModifier[] { new ParameterModifier(2) };
                MethodInfo info = type.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder, new Type[] { typeof(string), type.MakeByRefType() }, modifiers);
                object[] parameters = new object[] { val, Activator.CreateInstance(type) };
                if ((bool)info.Invoke(null, parameters))
                {
                    return (T)parameters[1];
                }
            }
            return default(T);

        }

        internal static DataTable ExcelToDataTable(ISheet sheet, bool isFirstRowColumn, Hashtable dic)
        {
            DataTable table = new DataTable
            {
                TableName = sheet.SheetName
            };
            int firstRowNum = 0;
            try
            {
                if (sheet != null)
                {
                    IRow row = sheet.GetRow(0);
                    if (row == null)
                    {
                        return table;
                    }
                    int lastCellNum = row.LastCellNum;
                    if (isFirstRowColumn)
                    {
                        for (int j = row.FirstCellNum; j < lastCellNum; j++)
                        {
                            DataColumn column = null;
                            string name = row.GetCell(j).ToString();
                            if (dic != null)
                            {
                                name = (dic[row.GetCell(j).ToString()] == null) ? row.GetCell(j).ToString() : dic[row.GetCell(j).ToString()].ToString();
                            }
                            if (table.Columns.Contains(name))
                            {
                                column = new DataColumn(name + "_" + j);
                            }
                            else
                            {
                                column = new DataColumn(name);
                            }
                            table.Columns.Add(column);
                        }
                        firstRowNum = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        for (int k = row.FirstCellNum; k < lastCellNum; k++)
                        {
                            DataColumn column2 = new DataColumn("F" + k.ToString());
                            table.Columns.Add(column2);
                        }
                        firstRowNum = sheet.FirstRowNum;
                    }
                    int lastRowNum = sheet.LastRowNum;
                    for (int i = firstRowNum; i <= lastRowNum; i++)
                    {
                        IRow row2 = sheet.GetRow(i);
                        if (row2 != null)
                        {
                            DataRow row3 = table.NewRow();
                            for (int m = row2.FirstCellNum; m < lastCellNum; m++)
                            {
                                if (row2.GetCell(m) != null)
                                {
                                    row3[m] = row2.GetCell(m).ToString();
                                }
                            }
                            table.Rows.Add(row3);
                        }
                    }
                }
                return table;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: " + exception.Message);
                return null;
            }
        }
        internal static object GetValueType(Type t, string value, int row, int col)
        {
            object obj2;
            try
            {
                string name = t.Name;
                if (!t.IsGenericType)
                {
                    return Convert.ChangeType(value, t);
                }
                if (t.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return (string.IsNullOrEmpty(value) ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(t)));
                }
                obj2 = value;
            }
            catch (Exception)
            {
                throw new Exception(string.Format("第{0}行第{1}列格式转换错误:{2}==>{3}", new object[] { row + 1, col + 1, value, t.Name }));
            }
            return obj2;

        }
    }
}
