using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NPOI.HSSF.Record.Formula.Functions;
using NPOI.HSSF.UserModel;

namespace SuperManCore
{
    public static class ExcelHelper
    {
        #region Common Method

        /// <summary>
        /// 创建Workbook
        /// </summary>
        /// <returns></returns>
        private static HSSFWorkbook CreateExcelFile()
        {
            var hssfworkbook = new HSSFWorkbook();
            return hssfworkbook;
        }

        /// <summary>
        /// 保存excel到Stream
        /// </summary>
        /// <param name="workBook"></param>
        /// <param name="stream"></param>
        private static void SaveExcelFile(this HSSFWorkbook workBook, Stream stream)
        {
            try
            {
                workBook.Write(stream);
                stream.Flush();
                stream.Position = 0;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }


        private static void CreateHeader(this HSSFSheet sheet, Dictionary<string, string> columns, HSSFCellStyle cellStyle)
        {
            HSSFRow row = sheet.CreateRow(0);
            int index = 0;

            foreach (KeyValuePair<string, string> de in columns)
            {
                HSSFCell cell = row.CreateCell(index);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(de.Value);
                index++;
            }
        }

        /// <summary>
        /// 设置行首样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private static HSSFCellStyle SetHeaderStyle(this HSSFWorkbook workbook)
        {
            HSSFCellStyle style = workbook.CreateCellStyle();
            style.Alignment = HSSFCellStyle.ALIGN_CENTER;
            style.BorderBottom = HSSFCellStyle.BORDER_THIN;
            style.BorderLeft = HSSFCellStyle.BORDER_THIN;
            style.BorderRight = HSSFCellStyle.BORDER_THIN;
            style.BorderTop = HSSFCellStyle.BORDER_THIN;
            style.Alignment = HSSFCellStyle.ALIGN_CENTER;
            HSSFFont font = workbook.CreateFont();
            font.FontHeightInPoints = 10;
            font.Boldweight = 700;
            style.SetFont(font);
            return style;
        }

        /// <summary>
        /// 设置单元格样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private static HSSFCellStyle SetCellStyle(this HSSFWorkbook workbook)
        {
            HSSFCellStyle style = workbook.CreateCellStyle();
            style.Alignment = HSSFCellStyle.ALIGN_CENTER;
            style.BorderBottom = HSSFCellStyle.BORDER_THIN;
            style.BorderLeft = HSSFCellStyle.BORDER_THIN;
            style.BorderRight = HSSFCellStyle.BORDER_THIN;
            style.BorderTop = HSSFCellStyle.BORDER_THIN;
            HSSFFont font = workbook.CreateFont();
            font.FontHeightInPoints = 10;
            font.Boldweight = HSSFFont.BOLDWEIGHT_NORMAL;
            style.SetFont(font);
            return style;
        }

        /// <summary>
        /// 设置日期格式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private static short SetDateFormat(this HSSFWorkbook workbook)
        {
            HSSFDataFormat format = workbook.CreateDataFormat();
            return format.GetFormat("yyyy-mm-dd");
        }

        #endregion

        #region DataTable To Stream

        /// <summary>
        /// DataTable 到Excel
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="filePath">文件保存路径</param>
        /// <param name="columns">DataTable 中的列及Excel中的标题组成的键值对</param>
        /// <param name="sheetName"></param>
        public static void ExportExcel(string filePath, DataTable dt, Dictionary<string, string> columns, string sheetName = "Sheet")
        {
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                ExportExcel(fs, dt, columns, sheetName);
            }
        }

        /// <summary>
        /// DataTable写入数据流
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="stream">数据流</param>
        /// <param name="columns">DataTable 中的列及Excel中的标题组成的键值对</param>
        /// <param name="sheetName">sheet名称</param>
        public static void ExportExcel(Stream stream, DataTable dt, Dictionary<string, string> columns, string sheetName = "Sheet")
        {
            if (columns == null || columns.Count == 0)
            {
                throw (new ArgumentNullException("columns", "请设置要导出的列名"));
            }
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new ArgumentNullException("dt", "请设置导出的数据");
            }

            HSSFWorkbook workbook = CreateExcelFile();

            workbook.CreateRows(dt.Rows, columns);

            workbook.SaveExcelFile(stream);
        }


        private static void CreateRows(this HSSFWorkbook workbook, DataRowCollection rows, Dictionary<string, string> columns, string sheetName = "Sheet")
        {
            HSSFCellStyle headStyle = workbook.SetHeaderStyle();
            HSSFCellStyle cellStyle = workbook.SetCellStyle();
            short dateFormat = workbook.SetDateFormat();

            //行首
            HSSFSheet sheet = workbook.CreateSheet(sheetName);
            sheet.CreateHeader(columns, headStyle);

            int rowCount = 1;
            int sheetCount = 1;
            foreach (DataRow dr in rows)
            {
                //超出10000条数据 创建新的工作簿                
                if (rowCount == 65536)
                {
                    rowCount = 1;
                    sheetCount++;
                    sheet = workbook.CreateSheet(sheetName + sheetCount);
                    sheet.CreateHeader(columns, headStyle);
                }
                HSSFRow row = sheet.CreateRow(rowCount);
                row.CreateCells(dr, columns, cellStyle, dateFormat);
                rowCount++;
            }
        }

        private static void CreateCells(this HSSFRow row, DataRow dtRow, Dictionary<string, string> columns, HSSFCellStyle cellStyle, short dateFormat)
        {
            int index = 0;
            foreach (KeyValuePair<string, string> keyValuePair in columns)
            {
                //列名称                
                string columnsName = keyValuePair.Key;

                Type rowType = dtRow[columnsName].GetType();
                string obj = dtRow[columnsName].ToString().Trim();

                HSSFCell cell = row.CreateCell(index);

                cell.SetCellValue(rowType, obj, cellStyle, dateFormat);
                index++;
            }
        }

        private static void SetCellValue(this HSSFCell cell, Type rowType, string cellValue, HSSFCellStyle cellStyle, short dateStyle)
        {
            if (cell == null)
            {
                throw new ArgumentNullException("cell");
            }

            switch (rowType.ToString())
            {
                case "System.String": //字符串类型                        
                    cellValue = cellValue.Replace("&", "&").Replace(">", ">").Replace("<", "<");
                    cell.SetCellValue(cellValue);
                    break;
                case "System.DateTime": //日期类型                        
                    DateTime dateTime;
                    DateTime.TryParse(cellValue, out dateTime);
                    cell.SetCellValue(dateTime);
                    //格式化显示
                    cellStyle.DataFormat = dateStyle;
                    break;
                case "System.Boolean": //布尔型                        
                    bool boolV;
                    bool.TryParse(cellValue, out boolV);
                    cell.SetCellValue(boolV);
                    break;
                case "System.Int16": //整型                    
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    int intV;
                    int.TryParse(cellValue, out intV);
                    cell.SetCellValue(intV.ToString(CultureInfo.InvariantCulture));
                    break;
                case "System.Decimal": //浮点型                    
                case "System.Double":
                    double doubV;
                    double.TryParse(cellValue, out doubV);
                    cell.SetCellValue(doubV);
                    break;
                case "System.DBNull": //空值处理                      
                    cell.SetCellValue("");
                    break;
                default:
                    throw new Exception(rowType + "：类型数据无法处理!");
            }
            cell.CellStyle = cellStyle;
        }

        #endregion

        #region List To Stream

        public static void ExportExcel<T>(string fileName, IList<T> list, Dictionary<string, string> columns, string sheetName = "Sheet")
        {
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                ExportExcel(fs, list, columns, sheetName);
            }
        }

        /// <summary>
        /// 将List输出到excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="list"></param>
        /// <param name="columns"></param>
        /// <param name="sheetName"></param>
        public static void ExportExcel<T>(Stream stream, IList<T> list, Dictionary<string, string> columns, string sheetName = "Sheet")
        {
            //创建Excel
            HSSFWorkbook workbook = CreateExcelFile();
            //创建sheet
            HSSFSheet sheet = workbook.CreateSheet(sheetName);

            // 数据
            workbook.CreateRows(sheet, columns, list, sheetName);

            SaveExcelFile(workbook, stream);
        }


        private static void CreateRows<T>(this HSSFWorkbook workbook, HSSFSheet sheet, Dictionary<string, string> columns, IEnumerable<T> list, string sheetName = "Sheet")
        {
            HSSFCellStyle headStyle = workbook.SetHeaderStyle();
            HSSFCellStyle cellStyle = workbook.SetCellStyle();

            // 表头
            sheet.CreateHeader(columns, headStyle);
            int rowCount = 1;
            int sheetCount = 0;
            foreach (T item in list)
            {
                //超出65536条数据 创建新的工作簿                
                if (rowCount == 65536)
                {
                    sheetCount++;
                    rowCount = 1;
                    sheet = workbook.CreateSheet(sheetName + sheetCount);
                    sheet.CreateHeader(columns, headStyle);
                }
                //excel从第2行开始
                HSSFRow row = sheet.CreateRow(rowCount);
                workbook.CreateCells(row, columns, item, cellStyle);
                rowCount++;
            }
        }

        private static void CreateCells<T>(this HSSFWorkbook workbook, HSSFRow row, Dictionary<string, string> columns, T item, HSSFCellStyle cellStyle)
        {
            Type type = typeof(T);
            PropertyInfo[] propertyInfos = type.GetProperties();
            int index = 0;
            foreach (KeyValuePair<string, string> entry in columns)
            {
                PropertyInfo info = propertyInfos.First(p => String.Equals(p.Name, entry.Key, StringComparison.CurrentCultureIgnoreCase));
                if (info == null)
                {
                    continue;
                }

                var cell = row.CreateCell(index);
                var obj = info.GetValue(item, null);
                short dateFormat = workbook.SetDateFormat();
                cell.SetCellValue(obj.GetType(), obj.ToString(), cellStyle, dateFormat);

                index++;
            }
        }

        #endregion

        #region Read DataTable from Stream

        public static DataTable ReadExcelToDataTable(string filePath)
        {
            HSSFWorkbook workbook;
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new HSSFWorkbook(file);
            }
            //获取excel的第一个sheet
            HSSFSheet sheet = workbook.GetSheetAt(0);
            var table = new DataTable();
            //获取sheet的首行
            HSSFRow headerRow = sheet.GetRow(0);
            //一行最后一个方格的编号 即总的列数
            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                var column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            //最后一列的标号  即总的行数
            int rowCount = sheet.LastRowNum + 1;
            for (int i = (sheet.FirstRowNum + 1); i < rowCount; i++)
            {
                HSSFRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                for (int j = row.FirstCellNum; j < 3; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }

        /// <summary>
        /// 读取Excel文件流到Datatable(只获取第一个sheet,跳过空行)
        /// </summary>
        /// <param name="sm"></param>
        /// <returns></returns>
        public static DataTable GetExcelDataToDataTable(Stream sm)
        {
            var input = new byte[sm.Length];
            sm.Read(input, 0, input.Length);
            sm.Dispose();
            var ms = new MemoryStream(input);
            var hssfworkbook = new HSSFWorkbook(ms);
            var sheet = hssfworkbook.GetSheetAt(0);
            return GetSheetDataToDataTable(sheet, hssfworkbook);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sm"></param>
        /// <returns></returns>
        public static DataTable[] GetExcelDataToMultiDataTable(Stream sm)
        {
            var input = new byte[sm.Length];
            sm.Read(input, 0, input.Length);
            sm.Dispose();
            var ms = new MemoryStream(input);
            var hssfworkbook = new HSSFWorkbook(ms);

            var tables = new DataTable[hssfworkbook.NumberOfSheets];

            int index = 0;
            while (index < hssfworkbook.NumberOfSheets)
            {
                var sheet = hssfworkbook.GetSheetAt(index);
                tables[index] = GetSheetDataToDataTable(sheet, hssfworkbook);
                index++;
            }
            return tables;
        }


        private static DataTable GetSheetDataToDataTable(HSSFSheet sheet, HSSFWorkbook hssfworkbook)
        {
            var dt = new DataTable();
            sheet.GetRowEnumerator();
            var headerRow = sheet.GetRow(0);
            if (headerRow == null)
            {
                throw new Exception("Excel工作表没有数据内容。");
            }
            int cellCount = headerRow.LastCellNum;
            for (int i = 0; i < cellCount; i++)
            {
                if (headerRow.GetCell(i) != null && !string.IsNullOrWhiteSpace(headerRow.GetCell(i).StringCellValue))
                {
                    var column = new DataColumn(headerRow.GetCell(i).StringCellValue.Trim());
                    dt.Columns.Add(column);
                }
                else
                {
                    var column = new DataColumn("A" + i);
                    dt.Columns.Add(column);
                }
            }
            var e = new HSSFFormulaEvaluator(hssfworkbook);

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null)
                {
                    continue;
                }
                DataRow dataRow = dt.NewRow();
                int blankCellCount = 0;
                for (int j = 0; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null && !string.IsNullOrWhiteSpace(row.GetCell(j).ToString()))
                    {
                        var cell = row.GetCell(j);
                        if (!string.IsNullOrWhiteSpace(cell.CellFormula))
                        {
                            cell = e.EvaluateInCell(cell);
                        }

                        #region

                        switch (cell.CellType)
                        {
                            //数字
                            case 0:
                                if (HSSFDateUtil.IsCellDateFormatted(cell))
                                {
                                    dataRow[j] = cell.DateCellValue;
                                }
                                else
                                {
                                    dataRow[j] = cell.NumericCellValue;
                                }
                                break;
                            //字符串
                            case 1:
                                dataRow[j] = cell.StringCellValue;
                                break;
                            //公式
                            case 2:
                                dataRow[j] = cell.ToString();
                                break;
                            //布尔
                            case 4:
                                dataRow[j] = cell.BooleanCellValue;
                                break;
                            //空白
                            //错误
                            //unknown
                            default:
                                dataRow[j] = "";
                                break;
                        }

                        #endregion
                    }
                    else
                    {
                        dataRow[j] = "";
                        blankCellCount++;
                    }
                }
                if (blankCellCount == cellCount) continue; //跳过空行
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        #endregion
    }
}