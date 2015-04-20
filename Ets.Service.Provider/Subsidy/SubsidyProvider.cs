﻿using Ets.Dao.Clienter;
using Ets.Dao.Subsidy;
using Ets.Dao.WtihdrawRecords;
using Ets.Model.Common;
using Ets.Model.DataModel.Subsidy;
using Ets.Model.DomainModel.GlobalConfig;
using Ets.Model.DomainModel.Subsidy;
using Ets.Model.ParameterModel.Subsidy;
using Ets.Model.ParameterModel.WtihdrawRecords;
using Ets.Service.IProvider.Subsidy;
using ETS.Data.PageData;
using ETS.Sms;
using ETS.Transaction;
using ETS.Transaction.Common;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Subsidy
{

    public class SubsidyProvider : ISubsidyProvider
    {
        private SubsidyDao subsidyDao = new SubsidyDao();
        /// <summary>
        /// 获取补贴设置  集团可选。
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public SubsidyResultModel GetCurrentSubsidy(int groupId = 0)
        {
            var subsidyResultModel = subsidyDao.GetCurrentSubsidy(groupId);

            return subsidyResultModel;

        }

        /// <summary>
        /// 获取补贴设置信息
        /// danny-20150320
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<subsidy> GetSubsidyList(HomeCountCriteria criteria)
        {

            PageInfo<subsidy> pageinfo = subsidyDao.GetSubsidyList<subsidy>(criteria);
            return pageinfo;
        }
        /// <summary>
        /// 添加补贴配置记录
        /// danny-20150320
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveData(SubsidyModel model)
        {
            return subsidyDao.SaveData(model);
        }

        /// <summary>
        /// 跨店补贴
        /// 徐鹏程
        /// 20150414
        /// </summary>
        public bool CrossShop(List<GlobalConfigSubsidies> SubsidiesList)
        {
            IList<GrabOrderModel> list = subsidyDao.GetBusinessCount();
            WtihdrawRecordsDao withdrawRecordsDao = new WtihdrawRecordsDao();
            ClienterDao clienterDao = new ClienterDao();

            int MaxSubsidiesShop = SubsidiesList.Max(t => ParseHelper.ToInt(t.Value1));//最大数量
            double MaxSubsidiesPrice = ParseHelper.ToDouble(SubsidiesList.Where(t => ParseHelper.ToInt(t.Value1) == MaxSubsidiesShop).ToList()[0].Value2);//最大金额
            
            foreach (GrabOrderModel item in list)
            {
                //Ets.Model.DomainModel.Clienter.ClienterModel cliterModel = new ClienterDao().GetUserInfoByUserId(item.ClienterId);//获取当前用户余额
                WithdrawRecordsModel withdraw = new WithdrawRecordsModel();

                #region 写流水
                withdraw.Platform = 1;
                withdraw.AdminId = 0;
                withdraw.UserId = item.ClienterId;
                int businessCount = item.BusinessCount;
                //var findSubsidie = SubsidiesList.OrderByDescending(t => ParseHelper.ToInt(t.Value1)).ToList().Where(t => ParseHelper.ToInt(t.Value1) >= businessCount).First();
                //double businessPrice = findSubsidie == null ? 0 : ParseHelper.ToDouble(findSubsidie.Value2);

                //ParseHelper.ToDouble(SubsidiesList.Select(t => ParseHelper.ToInt(t.Value1) == businessCount));
                double businessPrice = 0;
                if (businessCount > MaxSubsidiesShop)
                {
                    businessPrice = MaxSubsidiesPrice;
                }
                else
                {
                    var tmpPrice = SubsidiesList.Where(t => ParseHelper.ToInt(t.Value1) == businessCount).ToList();
                    if (tmpPrice == null || tmpPrice.Count <= 0)
                    {
                        continue;
                    }
                    businessPrice = ParseHelper.ToDouble(tmpPrice[0].Value2);//当前金额
                }
                
                withdraw.Amount = ParseHelper.ToDecimal(businessPrice, 0);
                withdraw.Balance = ParseHelper.ToDecimal(item.AccountBalance, 0);
                withdraw.Remark = string.Format("跨店抢单奖励{0}元", withdraw.Amount);

                //记录跨店日志
                CrossShopModel crossShopModel = new CrossShopModel()
                {
                    Amount = withdraw.Amount,
                    BusinessCount = businessCount,
                    ClienterId = withdraw.UserId,
                    Platform = 2,
                    Remark = withdraw.Remark,
                    InsertTime = DateTime.Now
                };
                #endregion

                using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
                {
                    withdrawRecordsDao.AddRecords(withdraw);
                    clienterDao.UpdateClienterAccountBalance(withdraw);//更改用户金额
                    subsidyDao.InsertCrossShopLog(crossShopModel);
                    tran.Complete();
                }
            }
            return true;
        }
        /// <summary>
        /// 跨店补贴短信
        /// 徐鹏程
        /// 20150416
        /// </summary>
        /// <returns></returns>
        public bool ShortMessage()
        {
            string strSendMsg = ETS.Config.SendMessage;
            IList<ShortMessageModel> list = subsidyDao.GetCrossShopInfo();
            foreach (ShortMessageModel item in list)
            {
                string msg = string.Format(strSendMsg, item.InsertTime, item.SumAmount);
                SendSmsStatus result=SendSmsHelper.SendSendSmsSaveLog(item.PhoneNo, msg, Ets.Model.Common.ConstValues.SMSSOURCE);
                ETS.Util.LogHelper.LogWriter(DateTime.Now.ToString() + "短信发送: 骑士ID=" + item.ClienterId + ";金额=" + item.SumAmount + ";电话号码=" + item.PhoneNo + ";发送状态=" + result.ToString());
            }
            return true;
        }
    }
}
