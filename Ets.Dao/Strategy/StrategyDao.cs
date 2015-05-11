using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;
using ETS.Dao;
using Ets.Model.DataModel.Clienter;
using ETS.Data.Core;
using System.Data;
using ETS.Util;
using Ets.Model.DataModel.Strategy;


namespace Ets.Dao.Strategy
{
    public class StrategyDao : DaoBase
    {
        /// <summary>
        /// 创建策略
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertDataStrategy(StrategyModel model)
        {
            string sql = @"INSERT INTO [dbo].[Strategy]
                            ([Name],StrategyId                        
                            )
                            VALUES
                            (@Name,@StrategyId
                          )
                            ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Name", model.Name);
            parm.AddWithValue("@StrategyId", model.StrategyId);
            object i = DbHelper.ExecuteScalar(SuperMan_Write, sql, parm);
            if (i != null)
            {
                return int.Parse(i.ToString());
            }
            return 0;
        }

        /// <summary>
        /// 修改策略名称信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateStrategyName(StrategyModel model)
        {
            string sql = " update [Strategy] set Name=@Name where id=@id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@id", model.Id);
            dbParameters.AddWithValue("@Name", model.Name);
            int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
            if (i > 0)
            {
                return true;
            }
            return false;

        }

        /// <summary>
        /// 判断策略是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsExistStrategy(string name)
        {
            string sql = string.Format(@" SELECT COUNT(1) FROM   dbo.[Strategy] WITH ( NOLOCK )  WHERE  Name=@Name");
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@Name", SqlDbType.NVarChar);
            dbParameters.SetValue("@Name", name);
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, sql, dbParameters);

            return ParseHelper.ToInt(executeScalar, 0) > 0;    
        }
        /// <summary>
        /// 判断策略是否已经创建过
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool HasExistsStrategy(StrategyModel model)
        {
            string sql = @" select 1 from [Strategy] with(nolock) where  Name=@Name";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Name", model.Name);
            object i = DbHelper.ExecuteScalar(SuperMan_Read, sql, dbParameters);
            if (i != null)
            {
                return int.Parse(i.ToString()) > 0;
            }
            return false;
        }

        /// <summary>
        /// 获取策略集合列表
        /// </summary>
        /// <returns></returns>
        public IList<StrategyModel> GetStrategyList()
        {
            string sql = string.Format(@" SELECT Id,StrategyId,Name,CreateBy,CreateTime,UpdateBy,UpdateTime FROM   dbo.[Strategy] WITH ( NOLOCK )");
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            return MapRows<StrategyModel>(dt);
        }
       
    }
}
