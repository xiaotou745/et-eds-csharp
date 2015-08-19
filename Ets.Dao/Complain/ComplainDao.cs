using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Complain;
using Ets.Model.DomainModel.Complain;
using Ets.Model.ParameterModel.Complain;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
using ETS.Util;

namespace Ets.Dao.Complain
{
    public class ComplainDao : DaoBase
    {
        public int Insert(ComplainModel complainModel)
        {
            const string insertSql = @"
insert into dbo.Complain
        ( ComplainId ,
          ComplainedId ,
          Reason ,
          OrderId ,
          OrderNo ,
          ComplainType 
        )
values  ( @ComplainId ,
          @ComplainedId ,
          @Reason ,
          @OrderId ,
          @OrderNo ,
          @ComplainType 
        );update dbo.[order] set IsComplain = 1 where Id = @OrderId;";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("ComplainId", DbType.Int32).Value = complainModel.ComplainId;
            dbParameters.Add("ComplainedId", DbType.Int32).Value = complainModel.ComplainedId;
            dbParameters.Add("Reason", DbType.String).Value = complainModel.Reason;
            dbParameters.Add("OrderId", DbType.Int32).Value = complainModel.OrderId;
            dbParameters.Add("OrderNo", DbType.String).Value = complainModel.OrderNo;
            dbParameters.Add("ComplainType", DbType.Int32).Value = complainModel.ComplainType; 
            object result = DbHelper.ExecuteNonQuery(SuperMan_Write, insertSql, dbParameters);
            return ParseHelper.ToInt(result, -1);
        }

        public PageInfo<T> Get<T>(ComplainCriteria complainCriteria)
        { 
            string columnList = @" 
        cp.Id,cp.OrderNo,cp.Reason ,
        cp.CreateTime ,
        cp.ComplainType ,
        b.Name BussinessName ,
        c.TrueName ClienterName ,
        b.City CityName ";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            if (!string.IsNullOrEmpty(complainCriteria.OrderNo))
            {
                sbSqlWhere.AppendFormat(" and cp.OrderNo = '{0}' ", complainCriteria.OrderNo);
            }
            if (complainCriteria.ComplainType > 0)
            {
                sbSqlWhere.AppendFormat(" and cp.ComplainType = '{0}' ", complainCriteria.ComplainType);
            }
            if (!string.IsNullOrWhiteSpace(complainCriteria.CityId))
            {
                sbSqlWhere.AppendFormat(" and b.CityId = '{0}' ", complainCriteria.CityId);
            } 
            if (!string.IsNullOrEmpty(complainCriteria.ComplainStartTime) && !string.IsNullOrEmpty(complainCriteria.ComplainEndTime))
            {
                sbSqlWhere.AppendFormat(" and cp.CreateTime > '{0}' and cp.CreateTime < '{1}' ", complainCriteria.ComplainStartTime, complainCriteria.ComplainEndTime);
            }
            string tableList = @" dbo.Complain cp ( nolock )
        join dbo.[order] o ( nolock ) on cp.OrderId = o.Id
        join dbo.business b ( nolock ) on o.businessId = b.Id
        join dbo.clienter c ( nolock ) on o.clienterId = c.Id ";
            string orderByColumn = " cp.Id DESC";
            return new PageHelper().GetPages<T>(SuperMan_Read, complainCriteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, complainCriteria.PageSize, true);
        }
        /// <summary>
        /// 该订单是否被投诉过
        /// </summary>
        /// <param name="complainModel"></param>
        /// <returns></returns>
        public int HadComplain(ComplainModel complainModel)
        {
            string sql = @" 
SELECT count(1) FROM dbo.Complain (nolock)
where OrderId = @OrderId and ComplainType = @ComplainType ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("OrderId", DbType.Int32).Value = complainModel.OrderId;
            dbParameters.Add("ComplainType", DbType.Int32).Value = complainModel.ComplainType; 
           return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql),-1);
        }
    }
}
