using ETS.Data.Core;
using Ets.Model.Common;
using ETS;
using ETS.Dao;
using System.Data;
using ETS.Util;

namespace Ets.Dao.Common
{
    /// <summary>
    /// 接口调用统计
    /// </summary>
    public class ApiVersionDao: DaoBase
    { 
            
        /// <summary>
        /// 添加接口调用记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(ApiVersionStatisticModel model)
        {
            string sql = @" INSERT INTO ApiVersionStatistic
                                   (APIName
                                   ,CreateTime
                                   ,Version)
                             VALUES
                                   (@APIName
                                   ,@CreateTime
                                   ,@Version);select @@IDENTITY ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("APIName",SqlDbType.VarChar,150);
            dbParameters.SetValue("APIName",model.APIName);
            dbParameters.Add("CreateTime", SqlDbType.DateTime,8);
            dbParameters.SetValue("CreateTime", model.CreateTime);
            dbParameters.Add("Version", SqlDbType.VarChar, 20);
            dbParameters.SetValue("Version", model.Version);
            object i = DbHelper.ExecuteScalar(Config.SuperMan_Write, sql, dbParameters);
            return ParseHelper.ToInt(i, 0) > 0; 
        }

    }
}
