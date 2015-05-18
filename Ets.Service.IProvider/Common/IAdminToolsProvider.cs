using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Common
{
    /// <summary>
    /// 管理员工具逻辑接口
    /// danny 2015-03-12
    /// </summary>
    public interface IAdminToolsProvider
    {
        /// <summary>
        /// 根据SQL查询数据
        /// danny-20150312
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        DataTable GetDataInfoBySql(string strSql);
        /// <summary>
        ///  执行SQL修改数据
        /// danny-20150312
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        int UpdateDataInfoBySql(string strSql);
    }
}
