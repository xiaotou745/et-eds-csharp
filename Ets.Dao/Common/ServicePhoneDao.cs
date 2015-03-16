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
        /// </summary>
        /// <returns></returns>
        public DataTable GetCustomerServicePhone()
        {
            string sql = "SELECT id,Phone,CityName FROM ServicePhone(NOLOCK)";
            return DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql));
        }
    }
}
