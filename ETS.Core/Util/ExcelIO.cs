using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ETS.Enums;
using ICSharpCode.SharpZipLib.Zip;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ETS.Util
{
    public class ExcelIO
    {
        // Fields
        private static readonly long _constMaxRow = 0x10000L;
        private static volatile ExcelIO _excelOperaterFactory;
        private static readonly object _locker = new object();
        private IWorkbook workbookExport;
        private IWorkbook workbookImport;

        // Methods
        public static ExcelIO CreateFactory()
        {
            if (_excelOperaterFactory == null)
            {
                lock (_locker)
                {
                    if (_excelOperaterFactory == null)
                    {
                        _excelOperaterFactory = new ExcelIO();
                    }
                }
            }
            return _excelOperaterFactory;
        }

        public ServiceResult Export(DataSet ds, ExportFileFormat f, IList<string[]> exceltitle = null)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                HttpResponse response = HttpContext.Current.Response;
                long rows = 0L;
                foreach (DataTable table in ds.Tables)
                {
                    if (table.Rows.Count > rows)
                    {
                        rows = table.Rows.Count;
                    }
                }
                string str = (f == ExportFileFormat.excel) ? ((rows >= _constMaxRow) ? ".xlsx" : ".xls") : ".zip";
                string str2 = (f == ExportFileFormat.excel) ? "application/vnd.ms-excel" : "application/zip";
                string str3 = ds.DataSetName + str;
                response.ContentType = str2;
                response.ContentEncoding = Encoding.UTF8;
                response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", HttpUtility.UrlEncode(str3)));
                response.Clear();
                if (rows >= _constMaxRow)
                {
                    this.workbookExport = new XSSFWorkbook();
                }
                else
                {
                    this.workbookExport = new HSSFWorkbook();
                }
                this.GenerateData(ds, exceltitle);
                response.BinaryWrite(this.WriteToStream(f, ds.DataSetName, rows).ToArray());
                response.Flush();
                response.End();
            }
            catch (Exception exception)
            {
                result.AddErrorCode(exception.Message);
            }
            return result;
        }

        public ServiceResult Export(DataTable dt, ExportFileFormat f, string[] exceltitle = null)
        {
            IList<string[]> list = new List<string[]> {
            exceltitle
        };
            if (dt.DataSet == null)
            {
                DataSet set = new DataSet();
                set.Tables.Add(dt);
            }
            dt.DataSet.DataSetName = dt.TableName;
            return this.Export(dt.DataSet, f, list);
        }

        public ServiceResult Export<T>(IList<T> ls, ExportFileFormat f, string exportfilename, string[] exceltitle = null)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                HttpResponse response = HttpContext.Current.Response;
                long rows = (ls != null) ? ((long)ls.Count) : ((long)0);
                string str = (f == ExportFileFormat.excel) ? ((rows >= _constMaxRow) ? ".xlsx" : ".xls") : ".zip";
                string str2 = (f == ExportFileFormat.excel) ? "application/vnd.ms-excel" : "application/zip";
                string str3 = exportfilename + str;
                response.ContentType = str2;
                response.ContentEncoding = Encoding.UTF8;
                response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", HttpUtility.UrlEncode(str3)));
                response.Clear();
                if (rows >= _constMaxRow)
                {
                    this.workbookExport = new XSSFWorkbook();
                }
                else
                {
                    this.workbookExport = new HSSFWorkbook();
                }
                this.GenerateData<T>(ls, exportfilename, exceltitle);
                response.BinaryWrite(this.WriteToStream(f, exportfilename, rows).ToArray());
                response.Flush();
                response.End();
            }
            catch (Exception exception)
            {
                result.AddErrorCode(exception.Message);
            }
            return result;
        }

        private void GenerateData(DataSet ds, IList<string[]> exceltitle)
        {
            int num = 0;
            foreach (DataTable table in ds.Tables)
            {
                if ((exceltitle != null) && (exceltitle.Count > num))
                {
                    this.GenerateData(table, exceltitle[num]);
                }
                else
                {
                    this.GenerateData(table, null);
                }
                num++;
            }
        }

        private void GenerateData(DataTable dt, string[] exceltitle)
        {
            IRow row = null;
            ISheet sheet = this.workbookExport.CreateSheet(dt.TableName);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    row = sheet.CreateRow(i);
                }
                IRow row2 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (i == 0)
                    {
                        if (exceltitle == null)
                        {
                            row.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                        }
                        else if (exceltitle.Count<string>() > j)
                        {
                            row.CreateCell(j).SetCellValue(exceltitle[j]);
                        }
                        else
                        {
                            row.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                        }
                    }
                    row2.CreateCell(j).SetCellValue((dt.Rows[i][j] == null) ? "" : dt.Rows[i][j].ToString());
                }
            }
        }

        private void GenerateData<T>(IList<T> ls, string exportFileName, string[] exceltitle)
        {
            IRow row = null;
            ISheet sheet = this.workbookExport.CreateSheet(exportFileName);
            if ((ls != null) && (ls.Count != 0))
            {
                T local = ls[0];
                PropertyInfo[] properties = local.GetType().GetProperties();
                for (int i = 0; i < ls.Count; i++)
                {
                    if (i == 0)
                    {
                        row = sheet.CreateRow(i);
                    }
                    IRow row2 = sheet.CreateRow(i + 1);
                    for (int j = 0; j < properties.Count<PropertyInfo>(); j++)
                    {
                        if (i == 0)
                        {
                            if (exceltitle == null)
                            {
                                row.CreateCell(j).SetCellValue(properties[j].Name);
                            }
                            else if (exceltitle.Count<string>() > j)
                            {
                                row.CreateCell(j).SetCellValue(exceltitle[j]);
                            }
                            else
                            {
                                row.CreateCell(j).SetCellValue(properties[j].Name);
                            }
                        }
                        object obj2 = properties[j].GetValue(ls[i], null);
                        if (obj2 == null)
                        {
                            obj2 = "";
                        }
                        row2.CreateCell(j).SetCellValue(obj2.ToString());
                    }
                }
            }
        }

        public IList<T> Import<T>(string fileName, bool isFirstRowColumn, out ServiceResult result, Hashtable dic) where T : new()
        {
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            ImportFileFormat xls = ImportFileFormat.xls;
            if (fileName.IndexOf(".xlsx") > 0)
            {
                xls = ImportFileFormat.xlsx;
            }
            else if (fileName.IndexOf(".xls") > 0)
            {
                xls = ImportFileFormat.xls;
            }
            return this.Import<T>(stream, xls, isFirstRowColumn, out result, dic);
        }

        public DataSet Import(string fileName, bool isFirstRowColumn, out ServiceResult result, IList<Hashtable> dicList = null)
        {
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            ImportFileFormat xls = ImportFileFormat.xls;
            if (fileName.IndexOf(".xlsx") > 0)
            {
                xls = ImportFileFormat.xlsx;
            }
            else if (fileName.IndexOf(".xls") > 0)
            {
                xls = ImportFileFormat.xls;
            }
            return this.Import(stream, xls, isFirstRowColumn, out result, dicList);
        }

        public DataSet Import(Stream stream, ImportFileFormat f, bool isFirstRowColumn, out ServiceResult result, IList<Hashtable> dicList = null)
        {
            result = new ServiceResult();
            DataSet set = new DataSet();
            try
            {
                if (f == ImportFileFormat.xls)
                {
                    this.workbookImport = new HSSFWorkbook(stream);
                }
                else
                {
                    this.workbookImport = new XSSFWorkbook(stream);
                }
                for (int i = 0; i < this.workbookImport.NumberOfSheets; i++)
                {
                    ISheet sheetAt = this.workbookImport.GetSheetAt(i);
                    if (dicList != null)
                    {
                        set.Tables.Add(ExcelBase.ExcelToDataTable(sheetAt, isFirstRowColumn, (dicList.Count > i) ? dicList[i] : null));
                    }
                    else
                    {
                        set.Tables.Add(ExcelBase.ExcelToDataTable(sheetAt, isFirstRowColumn, null));
                    }
                }
            }
            catch (Exception exception)
            {
                result.AddErrorCode(exception.Message);
            }
            return set;
        }

        public IList<T> Import<T>(Stream stream, ImportFileFormat f, bool isFirstRowColumn, out ServiceResult result, Hashtable dic = null) where T : new()
        {
            result = new ServiceResult();
            List<T> list = new List<T>();
            try
            {
                if (f == ImportFileFormat.xls)
                {
                    this.workbookImport = new HSSFWorkbook(stream);
                }
                else
                {
                    this.workbookImport = new XSSFWorkbook(stream);
                }
                ISheet sheetAt = this.workbookImport.GetSheetAt(0);
                sheetAt.GetRowEnumerator();
                IRow row = sheetAt.GetRow(0);
                short lastCellNum = row.LastCellNum;
                T local = default(T);
                int num = isFirstRowColumn ? (sheetAt.FirstRowNum + 1) : sheetAt.FirstRowNum;
                for (int i = num; i <= sheetAt.LastRowNum; i++)
                {
                    IRow row2 = sheetAt.GetRow(i);
                    local = Activator.CreateInstance<T>();
                    PropertyInfo[] properties = local.GetType().GetProperties();
                    for (int j = 0; j < properties.Length; j++)
                    {
                        Predicate<ICell> match = null;
                        PropertyInfo column = properties[j];
                        if (match == null)
                        {
                            match = delegate(ICell c)
                            {
                                if ((dic != null) && (dic[c.StringCellValue] != null))
                                {
                                    return dic[c.StringCellValue].ToString().ToLower() == column.Name.ToLower();
                                }
                                return c.StringCellValue.ToLower() == column.Name.ToLower();
                            };
                        }
                        int cellnum = row.Cells.FindIndex(match);
                        if ((cellnum >= 0) && (row2.GetCell(cellnum) != null))
                        {
                            object obj2 = ExcelBase.GetValueType(column.PropertyType, row2.GetCell(cellnum).ToString(), i, cellnum);
                            column.SetValue(local, obj2, null);
                        }
                    }
                    list.Add(local);
                }
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                result.AddErrorCode(message);
            }
            return list;
        }

        private void InitializeWorkbook()
        {
        }

        private MemoryStream WriteToStream(ExportFileFormat f, string contFilename, long rows)
        {
            MemoryStream stream = new MemoryStream();
            this.workbookExport.Write(stream);
            if (f != ExportFileFormat.zip)
            {
                return stream;
            }
            MemoryStream outStream = new MemoryStream();
            using (ZipFile file = ZipFile.Create(outStream))
            {
                file.BeginUpdate();
                StreamDataSource dataSource = new StreamDataSource(stream);
                file.Add(dataSource, contFilename + ((rows >= _constMaxRow) ? ".xlsx" : ".xls"));
                stream.Close();
                stream.Dispose();
                file.CommitUpdate();
            }
            return outStream;
        }

    }
}
