using Ets.Dao.Common;
using Ets.Service.IProvider.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Common
{
    /// <summary>
    /// 管理员工具逻辑接口实现类 
    /// 王旭丹 2015-03-12
    /// </summary>
    public class AdminToolsProvider:IAdminToolsProvider
    {
        /// <summary>
        /// 根据SQL查询数据
        /// 王旭丹-20150312
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public DataTable GetDataInfoBySql(string strSql)
        {
            return new AdminToolsDao().GetDataInfoBySql(strSql);
        }
        /// <summary>
        /// 执行SQL修改数据
        /// 王旭丹-20150312
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public int UpdateDataInfoBySql(string strSql)
        {
            return new AdminToolsDao().UpdateDataInfoBySql(strSql);
        }
    }
}
