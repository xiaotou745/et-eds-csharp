using ETS.Dao;
using ETS.Data.Core;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.Order
{
    /// <summary>
    /// 数据访问类OrderRegionDao。
    /// Generate By: tools.etaoshi.com
    /// Generate Time: 2015-11-04 16:42:11
    /// </summary>

    public class OrderRegionDao : DaoBase
    {
        /// <summary>
        ///获取当前商家下所有区域中是否存在未接单的订单数量
        /// caoheyang 20151104
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public bool GetOrderCountInfoByBusinessId(int businessId) {
            string sql = "SELECT Sum(WaitingCount) FROM dbo.OrderRegion where Status=1 and BusinessId=@BusinessId";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("BusinessId", DbType.Int32, 4).Value = businessId;
            object r = DbHelper.ExecuteScalar(SuperMan_Write, sql, parm);
            int res = ParseHelper.ToInt(r);
            return res > 0;
        }
    }
}
