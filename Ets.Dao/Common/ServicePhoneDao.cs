using ETS.Dao;
using ETS.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.Common
{
    public class ServicePhoneDao : DaoBase
    {
        /// <summary>
        /// 获取所有客服电话
        /// 窦海超
        /// 2015年3月16日 13:35:29
        /// </summary>
        /// <returns></returns>
        public DataTable Query()
        {
            const string querysql = @"
select  id,Phone,CityName from ServicePhone nolock";
            return DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql));
        }
    }
}
