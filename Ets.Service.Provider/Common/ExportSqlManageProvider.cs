using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Common;
using ETS.Data.PageData;
using Ets.Model.DataModel.Common;
using Ets.Model.ParameterModel.Common;
using Ets.Service.IProvider.Common;

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
    }
}
