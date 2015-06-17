using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Common;
using ETS.Data.PageData;
using ETS.IO;
using Ets.Model.DataModel.Common;
using Ets.Model.ParameterModel.Common;
using Ets.Service.IProvider.Common;
using ETS.Util;

namespace Ets.Service.Provider.Common
{
    /// <summary>
    /// sql定时导出发送数据  add by caoheyang 20150601
    /// </summary>
    public class ExportSqlManageProvider : IExportSqlManageProvider
    {
        private readonly ExportSqlManageDao exportSqlManageDao = new ExportSqlManageDao();
        /// <summary>
        /// 增加一条记录
        /// </summary>
        public int Insert(ExportSqlManage model)
        {
            return exportSqlManageDao.Insert(model);
        }

        /// <summary>
        /// 修改一条记录
        /// </summary>
        public int Update(ExportSqlManage model)
        {
            return exportSqlManageDao.Update(model);
        }


        /// <summary>
        /// 删除一条记录
        /// </summary>
        public int Delete(long id)
        {
            return exportSqlManageDao.Delete(id);
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        public PageInfo<ExportSqlManage> Query(DataManageSearchCriteria search)
        {
            return exportSqlManageDao.Query(search);
        }


        /// <summary>
        /// 根据id查询对象
        /// </summary>
        public ExportSqlManage GetById(long id)
        {
            return exportSqlManageDao.GetById(id) ?? new ExportSqlManage();
        }

        /// <summary>
        /// 查询今日尚未执行的服务 
        /// </summary>
        public IList<ExportSqlManage> QueryForWindows(DataManageSearchCriteria search)
        {
            IList<ExportSqlManage> results = exportSqlManageDao.QueryForWindows(search);
            string urlPath = "E:\\Export\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
            if (!System.IO.Directory.Exists(urlPath))
            {
                System.IO.Directory.CreateDirectory(urlPath);
            }
            foreach (var temp in results)
            {
                //判断今天是否已经导出过该文件
                if (System.IO.File.Exists(urlPath + temp.Name + DateTime.Now.ToString("yyyyMMdd") + ".xls"))
                {
                    continue;
                }
                else
                {
                    DataTable dt = exportSqlManageDao.ExecuteForExport(temp.SqlText);
                    var dics = new Dictionary<string, string>();
                    foreach (DataColumn colucmns in dt.Columns)
                    {
                        dics.Add(colucmns.ColumnName, colucmns.ColumnName);
                    }
                    //ExcelHelper.ExportExcel(urlPath + temp.Name + DateTime.Now.ToString("yyyyMMdd") + ".xls", dt, dics, temp.Name);
                }
            }
            return results;
        }
    }
}
