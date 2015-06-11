using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Data.PageData;
using Ets.Model.DataModel.Common;
using Ets.Model.ParameterModel.Common;

namespace Ets.Service.IProvider.Common
{
    /// <summary>
    /// sql定时导出发送数据  add by caoheyang 20150601
    /// </summary>
    public interface IExportSqlManageProvider
    {
        /// <summary>
        /// 增加一条记录
        /// </summary>
        int Insert(ExportSqlManage model);

        /// <summary>
        /// 删除一条记录
        /// </summary>
        int Delete(long id);

        /// <summary>
        /// 查询对象
        /// </summary>
        PageInfo<ExportSqlManage> Query(DataManageSearchCriteria search);


        /// <summary>
        /// 根据id查询对象
        /// </summary>
        ExportSqlManage GetById(long id);

        /// <summary>
        /// 修改一条记录
        /// </summary>
        int Update(ExportSqlManage model);
    }
}
