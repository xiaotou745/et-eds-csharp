using Ets.Dao.Clienter;
using Ets.Dao.WtihdrawRecords;
using ETS.Data.PageData;
using Ets.Model.Common;
using Ets.Model.ParameterModel.WtihdrawRecords;
using Ets.Service.IProvider.WtihdrawRecords;
using ETS.Transaction;
using ETS.Transaction.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.Finance;
using Ets.Model.DataModel.Finance;
using ETS.Enums;
using ETS.Util;

namespace Ets.Service.Provider.WtihdrawRecords
{
    public class WtihdrawRecordsProvider : IWtihdrawRecordsProvider
    {
        readonly ClienterBalanceRecordDao clienterBalanceRecordDao = new ClienterBalanceRecordDao();
        /// <summary>
        /// 提现
        /// 窦海超
        /// 2015年3月23日 12:54:43
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddWtihdrawRecords(Model.ParameterModel.WtihdrawRecords.WithdrawRecordsModel model)
        {

            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                WtihdrawRecordsDao withDao = new WtihdrawRecordsDao();
                ClienterDao clienterDao = new ClienterDao();
                bool checkBalance = clienterDao.UpdateClienterAccountBalance(model);
                if (!checkBalance)//如果余额不正确，返回错误
                {
                    return false;
                }
                var cliterModel = clienterDao.GetUserInfoByUserId(model.UserId);//获取当前用户余额
                decimal balance = ParseHelper.ToDecimal(cliterModel.AccountBalance, 0);
                model.Balance = balance;//最新余额
                bool checkAddwith = withDao.AddWtihdrawRecords(model);//新增提现记录 
                //流水改到
                ClienterBalanceRecord cbrm = new ClienterBalanceRecord()
                {
                    ClienterId = model.UserId,
                    Amount = Convert.ToDecimal(model.Amount),
                    Status = ClienterBalanceRecordStatus.Success.GetHashCode(),
                    Balance = balance - Convert.ToDecimal(model.Amount), //最新余额 - 提现金额
                    RecordType = ClienterBalanceRecordRecordType.Withdraw.GetHashCode(),
                    Operator = cliterModel.TrueName,
                    RelationNo = "", //提现的时候没有关联单号吧
                    Remark = "骑士提现"
                };

                long iResult = clienterBalanceRecordDao.Insert(cbrm);
                // bool checkAddrecords = withDao.AddRecords(model);//新增提现流水记录 
                if (!checkAddwith || iResult <= 0)
                {
                    return false;
                }
                tran.Complete();
            }
            return true;
        }

        /// <summary>
        /// 获取我的余额 平扬 2015.3.23
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Model.Common.NewPagedList<IncomeModel> GetMyIncomeList(MyIncomeSearchCriteria criteria)
        {
            try
            {
                var withDao = new WtihdrawRecordsDao();
                var reslut = withDao.GetMyIncomeList(criteria);
                var list = new NewPagedList<IncomeModel>(reslut.Records);
                var pageresult = new NewPagingResult(criteria.PagingRequest.PageIndex, criteria.PagingRequest.PageSize)
                {
                    TotalCount = reslut.All,
                    RecordCount = reslut.Records.Count
                };
                list.PagingResult = pageresult;
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriterFromFilter(ex);
                return null;
            }
            return null;

        }

        ////<summary>
        ////增加一条流水记录
        ////平扬
        ////2015年3月23日
        ////</summary>
        ////<param name="model"></param>
        ////<returns></returns>
        //public bool AddRecords(WithdrawRecordsModel model)
        //{
        //    try
        //    {
        //        WtihdrawRecordsDao withDao = new WtihdrawRecordsDao();
        //        return withDao.AddRecords(model);//新增提现流水记录
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.LogWriterFromFilter(ex);
        //        return false;
        //    }
        //    return false;
        //}
    }
}
