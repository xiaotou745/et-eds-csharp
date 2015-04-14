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
                          RType
                        )
                VALUES  ( @Platform , -- Platform - int
                          @UserId , -- UserId - int
                          @Amount , -- Amount - decimal
                          @Balance , -- Balance - decimal
                          getdate() , -- CreateTime - datetime
                          @AdminId,  -- AdminId - int
                          @IsDel,
                          @Remark,
                          @RType
                        )
                ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Platform", model.Platform);
            parm.AddWithValue("@UserId", model.UserId);
            parm.AddWithValue("@Amount", model.Amount);
            parm.AddWithValue("@Balance", model.Balance);
            parm.AddWithValue("@AdminId", model.AdminId);
            parm.AddWithValue("@IsDel",model.IsDel);
            parm.AddWithValue("@Remark",model.Remark);
            parm.AddWithValue("@RType", model.RType);
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
            string column = @"Amount as MyInComeAmount,
                            (CASE WHEN Amount>0 THEN '收入' ELSE '支出' END ) AS MyIncome1,
                            CreateTime as InsertTime";
            return new PageHelper().GetPages<IncomeModel>(SuperMan_Read, model.PagingRequest.PageIndex, "C.PhoneNo='" + model.phoneNo.Trim() + "' ", "  r.id desc ", column, " Records R (nolock) join clienter C (nolock) on R.UserId=C.Id ", model.PagingRequest.PageSize, true);
        }


    }
}
