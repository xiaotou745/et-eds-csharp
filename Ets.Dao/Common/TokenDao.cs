using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS;
using ETS.Dao;
using ETS.Data.Core;

namespace Ets.Dao.Common
{
    /// <summary>
    /// 2015年8月6日20:34:18
    /// 茹化肖
    /// 
    /// </summary>
    public class TokenDao : DaoBase
    { 
        /// <summary>
        /// 查询appkey 在骑士表是否存在
        /// </summary>
        /// <param name="appkey"></param>
        /// <returns></returns>
        public bool CheckAppkey_C(string appkey)
        {
            string sql = @"SELECT COUNT(1) FROM  clienter AS c (NOLOCK) WHERE c.Appkey=@AppKey";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@AppKey", appkey);
            object i = DbHelper.ExecuteScalar(Config.SuperMan_Read, sql, dbParameters);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }
        /// <summary>
        /// 查询appkey在商户表是否存在
        /// </summary>
        /// <param name="appkey"></param>
        /// <returns></returns>
        public bool CheckAppkey_B(string appkey)
        {
            string sql = @"SELECT COUNT(1) FROM  business AS b (NOLOCK) WHERE b.Appkey=@AppKey";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@AppKey", appkey);
            object i = DbHelper.ExecuteScalar(Config.SuperMan_Read, sql, dbParameters);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }
    }
}
