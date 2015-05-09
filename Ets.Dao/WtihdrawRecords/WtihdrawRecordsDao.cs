using Ets.Model.DomainModel.Clienter;
﻿using ETS.Data.PageData;
using Ets.Model.ParameterModel.WtihdrawRecords;
using ETS.Dao;
using ETS.Data.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.ParameterModel.Finance;

namespace Ets.Dao.WtihdrawRecords
{
    public class WtihdrawRecordsDao:DaoBase
    {

        /// <summary>
        /// 新增提现记录
        /// 窦海超
        /// 2015年3月23日 11:40:56
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddWtihdrawRecords(WithdrawRecordsModel model)
        {
            string sql = @"
                        INSERT INTO dbo.WtihdrawRecords
                                ( [Platform] ,
                                  UserId ,
                                  Amount ,
                                  Balance ,
                                  CreateTime ,
                                  Status ,
                                  AdminId
                                )
                        VALUES  ( @Platform , -- Platform - int
                                  @UserId , -- UserId - int
                                  @Amount , -- Amount - decimal
                                  @Balance , -- Balance - decimal
                                  GETDATE() , -- CreateTime - datetime
                                  1 , -- Status - int
                                  @AdminId  -- AdminId - int
                                )
                        ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Platform", model.Platform);
            parm.AddWithValue("@UserId", model.UserId);
            parm.AddWithValue("@Amount", model.Amount);
            parm.AddWithValue("@Balance", model.Balance);
            parm.AddWithValue("@AdminId", model.AdminId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }


        /// <summary>
        /// 新增提现流水记录
        /// 窦海超
        /// 2015年3月23日 11:40:56
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddRecords(WithdrawRecordsModel model)
        {
            string sql = @"
                INSERT INTO dbo.Records
                        ( [Platform] ,
                          UserId ,
                          Amount ,
                          Balance ,
                          CreateTime ,
                          AdminId,
                          IsDel,
                          Remark,
                          OrderId,
                          RecordType
                        )
                VALUES  ( @Platform , -- Platform - int
                          @UserId , -- UserId - int
                          @Amount , -- Amount - decimal
                          @Balance , -- Balance - decimal
                          getdate() , -- CreateTime - datetime
                          @AdminId,  -- AdminId - int
                          @IsDel,
                          @Remark,
                          @OrderId,
                          @RecordType
                        )
                ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Platform", model.Platform);
            parm.AddWithValue("@UserId", model.UserId);
            parm.AddWithValue("@Amount", model.Amount);
            parm.AddWithValue("@Balance", model.Balance);
            parm.AddWithValue("@AdminId", model.AdminId);
            parm.AddWithValue("@IsDel", model.IsDel);
            parm.AddWithValue("@Remark", model.Remark == null ? "" : model.Remark);
            parm.AddWithValue("@OrderId", model.OrderId);
            parm.AddWithValue("@RecordType", model.RecordType);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }

        /// <summary>
        /// 获取C端账户流水信息
        /// 窦海超
        /// 2015年3月20日 17:08:05
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public IList<ClienterRecordsModel> GetClienterRecordsByUserId(int UserId)
        {
            string sql = @"SELECT r.Id,Amount,Balance,CreateTime,a.UserName AS AdminName FROM Records(NOLOCK) AS r 
                            LEFT JOIN dbo.account AS a ON r.adminid=a.Id
                             WHERE [platform]=1 AND Amount < 0 AND userid=" + UserId;
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            return MapRows<ClienterRecordsModel>(dt);
        }

        /// <summary>
        /// 获取我的余额
        /// 平扬
        /// 2015年3月23日 11:40:56
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PageInfo<IncomeModel> GetMyIncomeList(MyIncomeSearchCriteria model)
        {
            //string sqlwhere = " Amount > 0 ";
            if (string.IsNullOrEmpty(model.phoneNo))
            {
                return null;
            }
            StringBuilder where = new StringBuilder();
            where.AppendFormat(" C.PhoneNo= '{0}' AND IsDel = 0 ", model.phoneNo.Trim());
            string column = @"Amount as MyInComeAmount,
                            (CASE WHEN Amount>0 THEN '收入' ELSE '支出' END ) AS MyIncome1,
                            CreateTime as InsertTime";
            return new PageHelper().GetPages<IncomeModel>(SuperMan_Read, model.PagingRequest.PageIndex, where.ToString(), "  r.id desc ", column, " Records R (nolock) join clienter C (nolock) on R.UserId=C.Id ", model.PagingRequest.PageSize, true);
        }
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
