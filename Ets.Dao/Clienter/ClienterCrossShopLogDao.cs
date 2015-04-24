using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;
using ETS.Dao;
using Ets.Model.DataModel.Clienter;
using ETS.Data.Core;


namespace Ets.Dao.Clienter
{
    public class ClienterCrossShopLogDao : DaoBase
    {
        /// <summary>
        /// 新增统计数据
        /// 胡灵波
        /// 2015年1月24日 15:30:11
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertDataClienterCrossShopLog(ClienterCrossShopLogModel model)
        {
            string sql = @"INSERT INTO [dbo].[ClienterCrossShopLog]
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



    }
}
