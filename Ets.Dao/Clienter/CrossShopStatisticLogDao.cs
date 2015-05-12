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
using Ets.Model.DomainModel.Bussiness;


namespace Ets.Dao.Clienter
{
    public class CrossShopStatisticLogDao : DaoBase
    {
        /// <summary>
        /// 新增跨店抢单骑士统计
        /// 胡灵波
        /// 2015年4月24日 15:30:11
        /// </summary>
        /// <param name="model">跨店抢单骑士实体</param>
        /// <returns></returns>
        public bool InsertDataCrossShopStatisticLogLog(CrossShopStatisticLogModel model)
        {
            string sql = @"INSERT INTO [dbo].[CrossShopStatisticLog]
                            ([TotalAmount]
                            ,[OnceCount]
                            ,[TwiceCount]
                            ,[ThreeTimesCount]
                            ,[FourTimesCount]
                            ,[FiveTimesCount]
                            ,[SixTimesCount]
                            ,[SevenTimesCount]
                            ,[EightTimesCount]
                            ,[NineTimesCount]
                            ,[ExceedNineTimesCount]
                            ,[OncePrice]
                            ,[TwicePrice]
                            ,[ThreeTimesPrice]
                            ,[FourTimesPrice]
                            ,[FiveTimesPrice]
                            ,[SixTimesPrice]
                            ,[SevenTimesPrice]
                            ,[EightTimesPrice]
                            ,[NineTimesPrice]
                            ,[ExceedNineTimesPrice]
                            ,[CreateTime]                          
                            ,[StatisticalTime])
                            VALUES
                            (@TotalAmount
                            ,@OnceCount
                            ,@TwiceCount
                            ,@ThreeTimesCount
                            ,@FourTimesCount
                            ,@FiveTimesCount
                            ,@SixTimesCount
                            ,@SevenTimesCount
                            ,@EightTimesCount
                            ,@NineTimesCount
                            ,@ExceedNineTimesCount
                            ,@OncePrice
                            ,@TwicePrice
                            ,@ThreeTimesPrice
                            ,@FourTimesPrice
                            ,@FiveTimesPrice
                            ,@SixTimesPrice
                            ,@SevenTimesPrice
                            ,@EightTimesPrice
                            ,@NineTimesPrice
                            ,@ExceedNineTimesPrice    
                            ,@CreateTime                        
                            ,@StatisticalTime)
                            ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@TotalAmount", model.TotalAmount);
            parm.AddWithValue("@OnceCount", model.OnceCount);
            parm.AddWithValue("@TwiceCount", model.TwiceCount);
            parm.AddWithValue("@ThreeTimesCount", model.ThreeTimesCount);
            parm.AddWithValue("@FourTimesCount", model.FourTimesCount);
            parm.AddWithValue("@FiveTimesCount", model.FiveTimesCount);
            parm.AddWithValue("@SixTimesCount", model.SixTimesCount);
            parm.AddWithValue("@SevenTimesCount", model.SevenTimesCount);
            parm.AddWithValue("@EightTimesCount", model.EightTimesCount);
            parm.AddWithValue("@NineTimesCount", model.NineTimesCount);
            parm.AddWithValue("@ExceedNineTimesCount", model.ExceedNineTimesCount);
            parm.AddWithValue("@OncePrice", model.OncePrice);
            parm.AddWithValue("@TwicePrice", model.TwicePrice);
            parm.AddWithValue("@ThreeTimesPrice", model.ThreeTimesPrice);
            parm.AddWithValue("@FourTimesPrice", model.FourTimesPrice);
            parm.AddWithValue("@FiveTimesPrice", model.FiveTimesPrice);
            parm.AddWithValue("@SixTimesPrice", model.SixTimesPrice);
            parm.AddWithValue("@SevenTimesPrice", model.SevenTimesPrice);
            parm.AddWithValue("@EightTimesPrice", model.EightTimesPrice);
            parm.AddWithValue("@NineTimesPrice", model.NineTimesPrice);
            parm.AddWithValue("@ExceedNineTimesPrice", model.ExceedNineTimesPrice);

            parm.AddWithValue("@CreateTime", model.CreateTime);
            parm.AddWithValue("@StatisticalTime", model.StatisticalTime);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }

        /// <summary>
        /// 判断是否存在指定日期的统计信息
        /// 胡灵波
        /// 2015年4月24日 17:31:17
        /// SuperMan_Write 临时
        /// </summary>
        /// <param name="statisticalTime">指定日期</param>
        /// <returns></returns>
        public bool IsExistCrossShopStatisticLog(string statisticalTime)
        {
            string selSql = string.Format(@" SELECT COUNT(1) FROM   dbo.[CrossShopStatisticLog] WITH ( NOLOCK )  WHERE  StatisticalTime = @statisticalTime");
            IDbParameters dbParameters = DbHelper.CreateDbParameters();     
            dbParameters.Add("@statisticalTime", SqlDbType.NVarChar);
            dbParameters.SetValue("@statisticalTime", statisticalTime);
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Write, selSql, dbParameters);

            return ParseHelper.ToInt(executeScalar, 0) > 0;    
        }

        /// <summary>
        /// 获取几天前跨店抢单骑士统计
        /// </summary>
        /// <param name="daysAgo">几天前</param>
        /// <returns></returns>
        public IList<BusinessesDistributionModel> GetClienterCrossShopLogInfo(int daysAgo)
        {
            int currdaysAgo = daysAgo + 1;
            StringBuilder sb = new StringBuilder();
            sb.Append("select top " + daysAgo.ToString());
            sb.Append(" * from dbo.CrossShopStatisticLog (nolock) ");       
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sb.ToString());
            return MapRows<BusinessesDistributionModel>(dt);
        }


    }
}
