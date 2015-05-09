using Ets.Model.ParameterModel.Finance;
using ETS.Dao;
using ETS.Data.PageData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.Finance
{
    public class BusinessFinanceDao : DaoBase
    {
        /// <summary>
        /// 根据参数获取商家提现申请单列表
        /// danny-20150509
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetBusinessWithdrawList<T>(BusinessWithdrawSearchCriteria criteria)
        {
            string columnList = @"  bwf.Id,
                                    bwf.WithwardNo,
                                    b.[Name] BusinessName,
                                    b.PhoneNo BusinessPhoneNo,
                                    bwf.BalancePrice,
                                    bwf.AllowWithdrawPrice,
                                    bwf.Amount,
                                    bwf.Balance,
                                    bwf.Status,
                                    bwf.WithdrawTime,
                                    bwf.Auditor,
                                    bwf.AuditTime,
                                    bwf.Payer,
                                    bwf.PayTime,
                                    bwf.AuditFailedReason,
                                    bwf.PayFailedReason ";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(criteria.BusinessName))
            {
                sbSqlWhere.AppendFormat(" AND b.[Name]='{0}' ", criteria.BusinessName.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.BusinessPhoneNo))
            {
                sbSqlWhere.AppendFormat(" AND b.PhoneNo='{0}' ", criteria.BusinessPhoneNo.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.BusinessCity))
            {
                sbSqlWhere.AppendFormat(" AND b.City='{0}' ", criteria.BusinessCity.Trim());
            }
            if (criteria.WithdrawStatus != 0)
            {
                sbSqlWhere.AppendFormat(" AND bwf.Status={0} ", criteria.WithdrawStatus);
            }
            if (!string.IsNullOrWhiteSpace(criteria.WithwardNo))
            {
                sbSqlWhere.AppendFormat(" AND bwf.WithwardNo='{0}' ", criteria.WithwardNo.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.WithdrawDateStart))
            {
                sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),bwf.WithdrawTime,120)>=CONVERT(CHAR(10),'{0}',120) ", criteria.WithdrawDateStart.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.WithdrawDateEnd))
            {
                sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),bwf.WithdrawTime,120)<=CONVERT(CHAR(10),'{0}',120) ", criteria.WithdrawDateEnd.Trim());
            }
            string tableList = @" BusinessWithdrawForm bwf with(nolock)
                                  join business b with(nolock) on bwf.BusinessId=b.Id
                                  join BusinessFinanceAccount bfa with(nolock) on bwf.BusinessId = bfa.BusinessId ";
            string orderByColumn = " bwf.Id DESC ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }
    }
}
