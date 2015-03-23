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
            string columnList = @"   [Id]
                                    ,[OrderCommission]
                                    ,[DistribSubsidy]
                                    ,[WebsiteSubsidy]
                                    ,[StartDate]
                                    ,[EndDate]
                                    ,[Status]
                                    ,[GroupId]
                                    ,[PKMCost]
                                    ,[OrderType] ";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (criteria.GroupId != null)
            {
                sbSqlWhere.AppendFormat(" AND GroupId={0} ", criteria.GroupId);
            }
            string tableList = @" subsidy  WITH (NOLOCK)   ";
            string orderByColumn = " StartDate DESC,EndDate DESC ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PagingRequest.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PagingRequest.PageSize, true);
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
    }
}
