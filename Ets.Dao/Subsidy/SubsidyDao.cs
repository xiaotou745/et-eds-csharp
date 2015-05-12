using Ets.Model.DataModel.Subsidy;
using Ets.Model.DomainModel.Subsidy;
using Ets.Model.ParameterModel.Subsidy;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
using ETS.Extension;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.Subsidy
{
    public class SubsidyDao : DaoBase
    {
        /// <summary>
        /// 获取最新 的 一条 补贴设置 
        /// wc
        /// </summary> 
        /// <param name="groupId"></param>
        /// <returns></returns>
        public SubsidyResultModel GetCurrentSubsidy(int groupId)
        {
            SubsidyResultModel subsidyResultModel = new SubsidyResultModel();
            StringBuilder selSql = new StringBuilder();

            selSql.Append(@"SELECT 
        TOP 1 
        OrderCommission ,
        DistribSubsidy ,
        WebsiteSubsidy  
FROM    dbo.subsidy sub WITH ( NOLOCK )
WHERE   sub.[Status] = 1 ");

            if (groupId > 0)
            {
                selSql.Append(" AND sub.GroupId = @groupId ");
            }
            else
            {
                selSql.Append(" AND sub.GroupId = 0 ");  //没有集团就 取 系统中默认设置的 补贴记录
            }

            selSql.Append(" ORDER BY id DESC ");

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@groupId", groupId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, selSql.ToString(), parm));
            if (dt != null && dt.Rows.Count != 0)
            {
                subsidyResultModel = DataTableHelper.ConvertDataTableList<SubsidyResultModel>(dt)[0];
            }

            return subsidyResultModel;
        }


        /// <summary>
        /// 获取补贴设置信息
        /// danny-20150320
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetSubsidyList<T>(HomeCountCriteria criteria)
        {
            string columnList = @"   s.[Id]
                                    ,s.[OrderCommission]
                                    ,s.[DistribSubsidy]
                                    ,s.[WebsiteSubsidy]
                                    ,s.[StartDate]
                                    ,s.[EndDate]
                                    ,s.[Status]
                                    ,s.[GroupId]
                                    ,s.[PKMCost]
                                    ,s.[OrderType]
                                    ,g.GroupName";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (criteria.GroupId != null && criteria.GroupId != 0)
            {
                sbSqlWhere.AppendFormat(" AND s.GroupId={0} ", criteria.GroupId);
            }
            string tableList = @" subsidy s WITH (NOLOCK)  LEFT JOIN dbo.[group] g WITH(NOLOCK) ON g.Id = s.GroupId ";
            string orderByColumn = " s.StartDate DESC,s.EndDate DESC ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }


        /// <summary>
        /// 添加补贴配置记录
        /// danny-20150320
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveData(SubsidyModel model)
        {
            try
            {
                string sql = @"
                           INSERT INTO subsidy
                               ( [OrderCommission]
                                ,[DistribSubsidy]
                                ,[WebsiteSubsidy]
                                ,[StartDate]
                                ,[EndDate]
                                ,[Status]
                                ,[GroupId])
                         VALUES
                               (@OrderCommission
                               ,@DistribSubsidy
                               ,@WebsiteSubsidy
                               ,@StartDate
                               ,@EndDate
                               ,@Status
                               ,@GroupId)SELECT @@IDENTITY  ";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.AddWithValue("@OrderCommission", model.OrderCommission != null ? model.OrderCommission.Value / 100 : 0.1m);
                parm.AddWithValue("@DistribSubsidy", model.DistribSubsidy);
                parm.AddWithValue("@WebsiteSubsidy", model.WebsiteSubsidy);
                parm.AddWithValue("@StartDate", model.StartDate);
                parm.AddWithValue("@EndDate", model.EndDate);
                parm.AddWithValue("@Status", 1);
                parm.AddWithValue("@GroupId", model.GroupId);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, sql, parm)) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("添加补贴配置记录", new { ex = ex, model = model });
                return false;
                throw;
            }
        }

        /// <summary>
        /// 查询前一天的抢单量
        /// 窦海超
        /// 2015年4月16日 09:37:07
        /// </summary>
        /// <returns></returns>
        public IList<GrabOrderModel> GetBusinessCount()
        {
            string strSql = @"
 select PubDate, ( select   AccountBalance
                   from     dbo.clienter(nolock)
                   where    Id = t.clienterId
                 ) AccountBalance, ClienterId, BusinessCount
 from   ( select    convert(char(10), o.PubDate, 120) PubDate, clienterId,
                    count(distinct businessId) businessCount
          from      dbo.[order] (nolock) o
          join dbo.GlobalConfig gc ( nolock ) on o.BusinessGroupId = gc.GroupId
          where     o.PubDate between convert(char(10), dateadd(day, -1,
                                                              getdate()), 120)
                              and     convert(char(10), getdate(), 120)
                    and o.[Status] = 1
					and gc.KeyName = 'IsStartOverStoreSubsidies'
					and gc.Value = 1    
                    and not exists ( select ClienterId
                                     from   dbo.CrossShopLog (nolock) cs
                                     where  RewardTime between convert(char(10), dateadd(day,
                                                              -1, getdate()), 120)
                                                       and    convert(char(10), getdate(), 120)
                                            and o.clienterId = cs.ClienterId )
          group by  convert(char(10), o.PubDate, 120), clienterId
        ) t
 where  businessCount > 1";
            DataTable myTable = DbHelper.ExecuteDataTable(SuperMan_Read, strSql);
            return MapRows<GrabOrderModel>(myTable);
        }

        /// <summary>
        /// 写入跨店奖励日志
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertCrossShopLog(CrossShopModel model)
        {
            string sql = @"INSERT INTO CrossShopLog
                        (ClienterId
                        ,Amount
                        ,BusinessCount
                        ,[Platform]
                        ,Remark
                        ,InsertTime)
                        VALUES
                        (@ClienterId
                        ,@Amount
                        ,@BusinessCount
                        ,@Platform
                        ,@Remark
                        ,@InsertTime)";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@ClienterId", model.ClienterId);
            parm.AddWithValue("@Amount", model.Amount);
            parm.AddWithValue("@BusinessCount", model.BusinessCount);
            parm.AddWithValue("@Platform", model.Platform);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@InsertTime", model.InsertTime);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }


        /// <summary>
        /// 写入跨店奖励日志
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IList<ShortMessageModel> GetCrossShopInfo()
        {
            string strSql = @"
                         select CONVERT(char(10),
                            cs.InsertTime,120) InsertTime,
                            SUM(cs.Amount) SumAmount,
                            cs.ClienterId,
                            c.PhoneNo
                        from CrossShopLog(nolock) cs join clienter(nolock) c on cs.ClienterId=c.Id
                        where 
                            CONVERT(char(10),cs.InsertTime,120)=CONVERT(char(10),DATEADD(DAY,-0,GETDATE()),120) 
                        group by CONVERT(char(10),cs.InsertTime,120),clienterId,c.PhoneNo";
            DataTable myTable = DbHelper.ExecuteDataTable(SuperMan_Read, strSql);
            return MapRows<ShortMessageModel>(myTable);
        }

         
        /// <summary>
        /// 跨店奖励日志
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public IList<CrossShopModel> GetCrossShopListByCid(int cid)
        {
            string strSql = @"
                            select cs.Amount,cs.BusinessCount,cs.InsertTime,cs.Remark
                            from CrossShopLog(nolock) cs join clienter(nolock) c on cs.ClienterId=c.Id
                            where c.Id=@ClienterId";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@ClienterId", cid);
            DataTable myTable = DbHelper.ExecuteDataTable(SuperMan_Read, strSql,parm);
            return MapRows<CrossShopModel>(myTable);
        }

        
    }
}
