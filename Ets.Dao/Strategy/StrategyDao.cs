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
        public bool InsertDataStrategy(StrategyModel model)
        {
            string sql = @"INSERT INTO [dbo].[Strategy]
                            ([Name]                         
                            )
                            VALUES
                            (@Name
                          )
                            ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Name", model.Name);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }

        public bool IsExistStrategy(string name)
        {
            string sql = string.Format(@" SELECT COUNT(1) FROM   dbo.[Strategy] WITH ( NOLOCK )  WHERE  Name=@Name");
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@Name", SqlDbType.NVarChar);
            dbParameters.SetValue("@Name", name);
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, sql, dbParameters);

            return ParseHelper.ToInt(executeScalar, 0) > 0;    
        }

        public IList<StrategyModel> GetStrategyList()
        {
            string sql = string.Format(@" SELECT * FROM   dbo.[Strategy] WITH ( NOLOCK )");
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            return MapRows<StrategyModel>(dt);
        }
    }
}
