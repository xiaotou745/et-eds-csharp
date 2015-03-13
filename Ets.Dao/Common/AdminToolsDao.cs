using ETS;
using ETS.Dao;
using ETS.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.Common
{
    /// <summary>
    /// 管理员工具方法类
    /// 王旭丹 2015-03-12
    /// </summary>
    public class AdminToolsDao : DaoBase
    {
        DbHelper dbHelper = new DbHelper();
        /// <summary>
        /// 根据SQL查询数据
        /// 王旭丹-20150312
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public DataTable GetDataInfoBySql(string strSql)
        {
            return dbHelper.ExecuteDataset(Config.SuperMan_Read, strSql).Tables[0]; 
        }
        /// <summary>
        /// 执行SQL修改数据
        /// 王旭丹-20150312
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public int UpdateDataInfoBySql(string strSql)
        {
            return dbHelper.ExecuteNonQuery(Config.SuperMan_Write, strSql);
        }
    }

}
