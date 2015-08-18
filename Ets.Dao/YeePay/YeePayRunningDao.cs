using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;
using Ets.Model.DataModel.DeliveryCompany;
using Ets.Model.DataModel.YeePay;
using Ets.Model.ParameterModel.DeliveryCompany;
using Ets.Model.ParameterModel.YeePay;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
using ETS.Util;

namespace Ets.Dao.YeePay
{
    public class YeePayRunningDao : DaoBase
    {
        /// <summary>
        /// 获取充值记录
        /// </summary>
        /// <returns></returns>
        public IList<YeePayRunningAccountModel> GetYeePayRunningAccountList()
        {
            string sql = @" 
 select 
        y.Id ,
        y.LedgerNo ,
        y.RechargeAmount ,
        y.Operator ,
        y.OperateTime ,
        y.RechargeTime ,
        y.Remark ,
        y.IsEnable
 from   dbo.YeePayRunningAccount y ( nolock )
 where  y.IsEnable = 1
 order by y.Id desc ";
            var dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            return MapRows<YeePayRunningAccountModel>(dt);
        }

        public PageInfo<T> Get<T>(YeePayRunningCriteria yeePayRunningCriteria)
        {
            string columnList = @"
        y.Id ,
        y.LedgerNo ,
        y.RechargeAmount ,
        y.Operator ,
        y.OperateTime ,
        y.RechargeTime ,
        y.Remark ,
        y.IsEnable";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            if (!string.IsNullOrEmpty(yeePayRunningCriteria.RechargeStarteTime) || !string.IsNullOrEmpty(yeePayRunningCriteria.RechargeEndTime))
            {
                sbSqlWhere.AppendFormat(" and y.RechargeTime > '{0}' and y.RechargeTime < '{1}' ", yeePayRunningCriteria.RechargeStarteTime, yeePayRunningCriteria.RechargeEndTime);
            }
            if (!string.IsNullOrEmpty(yeePayRunningCriteria.OptStarteTime) || !string.IsNullOrEmpty(yeePayRunningCriteria.OptEndTime))
            {
                sbSqlWhere.AppendFormat(" and y.OperateTime > '{0}' and y.OperateTime < '{1}' ", yeePayRunningCriteria.OptStarteTime, yeePayRunningCriteria.OptEndTime);
            }  
            string tableList = @" dbo.YeePayRunningAccount y (nolock) ";
            string orderByColumn = " y.Id DESC";
            return new PageHelper().GetPages<T>(SuperMan_Read, yeePayRunningCriteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, yeePayRunningCriteria.PageSize, true);
        }

        public int Add(YeePayRunningAccountModel yeePayRunningAccountModel)
        {
            const string insertSql = @"
 insert into dbo.YeePayRunningAccount
         ( LedgerNo ,
           RechargeAmount ,
           Operator ,
           OperateTime ,
           RechargeTime ,
           Remark  
         )
 values  (  
           @LedgerNo ,
           @RechargeAmount ,
           @Operator ,
           getdate() ,
           @RechargeTime ,
           @Remark  
         )";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("LedgerNo", DbType.String).Value = "";
            dbParameters.Add("RechargeAmount", DbType.Decimal).Value = yeePayRunningAccountModel.RechargeAmount;
            dbParameters.Add("Operator", DbType.String).Value = yeePayRunningAccountModel.Operator;
            dbParameters.Add("RechargeTime", DbType.DateTime).Value = yeePayRunningAccountModel.RechargeTime;
            dbParameters.Add("Remark", DbType.String).Value = yeePayRunningAccountModel.Remark;

            int result = ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, insertSql, dbParameters));

            return result; 
        }
    }
}
