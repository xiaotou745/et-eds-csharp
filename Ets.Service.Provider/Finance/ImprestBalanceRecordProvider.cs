using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Const;
using Ets.Dao.Clienter;
using Ets.Dao.Finance;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Model.ParameterModel.Finance;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Order;
using ETS.Transaction;
using ETS.Transaction.Common;
using ETS.Util;


namespace Ets.Service.Provider.Finance
{
    /// <summary>
    /// 备用金操作记录  add by caoheyang 20150812
    /// </summary>
    public class ImprestBalanceRecordProvider : IImprestBalanceRecordProvider
    {

        private readonly ImprestBalanceRecordDao _imprestBalanceRecordDao = new ImprestBalanceRecordDao();
        private readonly ClienterWithdrawFormDao _clienterWithdrawFormDao = new ClienterWithdrawFormDao();
        private readonly ClienterDao _clienterDao=new ClienterDao();
        private readonly ClienterFinanceDao _clienterFinanceDao=new ClienterFinanceDao();
        private readonly ImprestRechargeDao _imprestRechargeDao=new ImprestRechargeDao();
        private readonly ClienterBalanceRecordDao _clienterBalanceRecordDao= new ClienterBalanceRecordDao();
        private readonly ClienterAllowWithdrawRecordDao _clienterAllowWithdrawRecordDao= new ClienterAllowWithdrawRecordDao();
        /// <summary>
        /// 验证手机号是否存在
        /// 2015年8月12日17:53:24
        /// 茹化肖
        /// </summary>
        /// <param name="phonenum"></param>
        /// <returns></returns>
        public ImprestClienterModel ClienterPhoneCheck(string phonenum)
        {
            ImprestClienterModel mode = new ImprestClienterModel();
            //获取骑士信息
            var clienter = _clienterDao.GetUserInfoByUserPhoneNo(phonenum);
            if (clienter == null)
            {
                mode.Status = 0;
                return mode;
            }
            //获取骑士提现中金额
            var amount = _clienterFinanceDao.GetClienterWithdrawingAmount(clienter.Id);
            //获取备用金可用余额
            var imprestPrice = _imprestRechargeDao.GetRemainingAmountNoLock();
            mode.Id = clienter.Id;
            mode.TrueName = clienter.TrueName;
            mode.ImprestPrice = imprestPrice;
            mode.PhoneNo = clienter.PhoneNo;
            mode.Status = 1;
            mode.WithdrawingPrice = amount;
            mode.AccountBalance = clienter.AccountBalance;
            mode.AllowWithdrawPrice = clienter.AllowWithdrawPrice;
            return mode;
        }

        /// <summary>
        /// 查询备用金流水列表  add by 彭宜  20150812
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public ETS.Data.PageData.PageInfo<ImprestBalanceRecordModel> GetImprestBalanceRecordList(ImprestBalanceRecordSearchCriteria criteria)
        {
            return _imprestBalanceRecordDao.GetImprestBalanceRecordList(criteria);
        }
        /// <summary>
        /// 备用金提现时间锁
        /// </summary>
        private static object mylock = new object();
        /// <summary>
        /// 骑士备用金提现
        /// 2015年8月12日18:18:40
        /// 茹化肖
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public ImprestPayoutModel ClienterWithdrawOk(ImprestWithdrawModel parmodel)
        {
            ImprestPayoutModel model = new ImprestPayoutModel();
            #region 时间锁
            lock (mylock)
            {
                string key = string.Format(RedissCacheKey.Ets_ImprestWithdraw_Lock_C, parmodel.ClienterId);
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                if (redis.Get<int>(key) == 1)
                {
                    model.Status = 0;
                    model.Message = "备用金提现正在执行中，请勿重新提交，请一分钟后重试";
                    return model;
                }
                redis.Set(key, 1, new TimeSpan(0, 1, 0));
            }
            #endregion

            bool flag = false;
            #region===验证提现
            using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
            {
                //1.验证 查询可提现金额>骑士提现金额
                var climodel = _clienterDao.GetUserInfoByUserId(parmodel.ClienterId);
                if (parmodel.WithdrawPrice > climodel.AllowWithdrawPrice)
                {
                    return new ImprestPayoutModel(){Status = 0,Message = "提现金额大于可提现金额!"};
                }
                //2.验证 查询备用金账户余额>骑士提现金额
                var imprsetAmount = _imprestRechargeDao.GetRemainingAmountLock();
                if (parmodel.WithdrawPrice > imprsetAmount.RemainingAmount)
                {
                    return new ImprestPayoutModel() { Status = 0, Message = "提现金额大于备用金可用余额!" };
                }

                #region ===3.创建骑士提现单,状态打款成功
                string withwardNo = Helper.generateOrderCode(parmodel.ClienterId);
                long withwardId = _clienterWithdrawFormDao.Insert(new ClienterWithdrawForm()
                {
                    WithwardNo = withwardNo,//单号
                    ClienterId = climodel.Id,//骑士Id
                    BalancePrice = (decimal) climodel.AccountBalance,//提现前骑士余额
                    AllowWithdrawPrice = climodel.AllowWithdrawPrice,//提现前骑士可提现金额
                    Status = (int)ClienterWithdrawFormStatus.Success,//打款完成
                    Amount = parmodel.WithdrawPrice,//提现金额
                    Balance = (decimal) (climodel.AccountBalance - parmodel.WithdrawPrice), //提现后余额
                    TrueName = climodel.TrueName,//骑士收款户名
                    AccountNo = "", //卡号(DES加密)
                    AccountType = 1, //账号类型：
                    BelongType = 0,//账号类别  0 个人账户 1 公司账户  
                    OpenBank = "",//开户行
                    OpenSubBank = "", //开户支行
                    IDCard = climodel.IDCard,//申请提款身份证号
                    OpenCity = "",//城市
                    OpenCityCode = 0,//城市代码
                    OpenProvince = "",//省份
                    OpenProvinceCode = 0,//省份代码
                    HandCharge =0,//手续费
                    HandChargeOutlay = HandChargeOutlay.Private,//手续费支出方
                    PhoneNo = climodel.PhoneNo, //手机号
                    HandChargeThreshold = 0//手续费阈值
                });
                #endregion
                
                #region===4.扣除骑士余额,提现余额,写流水
                //更新骑士表的余额，可提现余额
                _clienterDao.UpdateForWithdrawC(new UpdateForWithdrawPM
                {
                    Id = climodel.Id,
                    Money = -parmodel.WithdrawPrice
                });

                _clienterBalanceRecordDao.Insert(new ClienterBalanceRecord()
                {
                    ClienterId = climodel.Id,//骑士Id
                    Amount = -parmodel.WithdrawPrice,//流水金额
                    Status = (int)ClienterBalanceRecordStatus.Success, //流水状态(1、交易成功 2、交易中）
                    RecordType = (int)ClienterBalanceRecordRecordType.WithdrawApply,
                    Operator = parmodel.OprName,
                    WithwardId = withwardId,
                    RelationNo = withwardNo,
                    Remark = "骑士提现(备用金提现)"
                });

                _clienterAllowWithdrawRecordDao.Insert(new ClienterAllowWithdrawRecord()
                {
                    ClienterId = climodel.Id,//骑士Id
                    Amount = -parmodel.WithdrawPrice,//流水金额
                    Status = (int)ClienterAllowWithdrawRecordStatus.Success, //流水状态(1、交易成功 2、交易中）
                    RecordType = (int)ClienterAllowWithdrawRecordType.WithdrawApply,
                    Operator = parmodel.OprName,
                    WithwardId = withwardId,
                    RelationNo = withwardNo,
                    Remark = "骑士提现(备用金提现)"
                });
                #endregion

                #region===5.修改骑士表累计提现金额
                flag = _clienterFinanceDao.ModifyClienterTotalAmount(withwardId.ToString());
                #endregion

                #region===6.扣除备用金账户总额,写备用金支出流水
                flag = _imprestRechargeDao.ImprestRechargePayOut(parmodel.WithdrawPrice, 1);
                flag = _imprestBalanceRecordDao.InsertRecord(new ImprestBalanceRecord()
                {
                     Amount=parmodel.WithdrawPrice,
                     BeforeAmount = imprsetAmount.RemainingAmount,
                     AfterAmount = imprsetAmount.RemainingAmount - parmodel.WithdrawPrice,
                     OptName = parmodel.OprName,
                     Remark = parmodel.Remark,
                     ClienterName = climodel.TrueName,
                     ClienterPhoneNo = climodel.PhoneNo,
                     OptType = 2
                });
                #endregion
                if (flag)
                {
                    tran.Complete();
                    model.Status = 0;
                    model.Message = "备用金提现成功";
                    return model;
                }
            }
            #endregion
            return model;
        }
    }
}
