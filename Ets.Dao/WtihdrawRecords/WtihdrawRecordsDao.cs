using ETS.Data.PageData;
using Ets.Model.ParameterModel.WtihdrawRecords;
using ETS.Dao;
using ETS.Data.Core;
using System;
using System.Collections.Generic;
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
                          AdminId
                        )
                VALUES  ( @Platform , -- Platform - int
                          @UserId , -- UserId - int
                          @Amount , -- Amount - decimal
                          @Balance , -- Balance - decimal
                          getdate() , -- CreateTime - datetime
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
        /// 获取我的余额
        /// 平扬
        /// 2015年3月23日 11:40:56
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PageInfo<IncomeModel> GetMyIncomeList(MyIncomeSearchCriteria model)
        {
            string sqlwhere = " Amount > 0 ";
            if (!string.IsNullOrEmpty(model.phoneNo))
            {
                sqlwhere += " and C.PhoneNo='" + model.phoneNo+"'";
            }
            return new PageHelper().GetPages<IncomeModel>(SuperMan_Read, model.PagingRequest.PageIndex, sqlwhere, " R.CreateTime ", " C.PhoneNo,'收入' as MyIncome1,Amount as MyInComeAmount,CreateTime as InsertTime ", " Records R (nolock) join clienter C (nolock) on R.UserId=C.Id ", model.PagingRequest.PageSize, true);
        }


    }
}
