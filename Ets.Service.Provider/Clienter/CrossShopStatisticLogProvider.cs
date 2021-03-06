﻿using Ets.Dao.Clienter;
using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DomainModel.Bussiness;
using Ets.Service.IProvider.Clienter;
using Ets.Service.IProvider.Statistics;
using Ets.Service.Provider.Clienter;
using ETS.Transaction;
using ETS.Transaction.Common;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ets.Service.Provider.Clienter
{
    public class CrossShopStatisticLogProvider : ICrossShopStatisticLogProvider
    {
        readonly ClienterDao clienterDao = new ClienterDao();
        readonly CrossShopStatisticLogDao _dao = new CrossShopStatisticLogDao();       

        /// <summary>
        /// 获取跨店抢单骑士统计信息
        /// 胡灵波-20150124
        /// </summary>
        /// <param name="daysAgo">几天前</param>
        /// <returns></returns>
        public bool InsertDataCrossShopStatisticLog(int daysAgo)
        {          
            IList<Ets.Model.DomainModel.Bussiness.BusinessesDistributionModel> clienteStorerGrabStatistical = clienterDao.GetClienteStorerGrabStatisticalInfo(daysAgo);
            CrossShopStatisticLogModel model = new CrossShopStatisticLogModel();

            foreach (var item in clienteStorerGrabStatistical)
            {
                bool isExist = _dao.IsExistCrossShopStatisticLog(item.date);
                if (isExist) continue;

                model.TotalAmount = item.totalAmount;
                model.OnceCount = item.c1;
                model.TwiceCount = item.c2;
                model.ThreeTimesCount = item.c3;
                model.FourTimesCount = item.c4;
                model.FiveTimesCount = item.c5;
                model.SixTimesCount = item.c6;
                model.SevenTimesCount = item.c7;
                model.EightTimesCount = item.c8;
                model.NineTimesCount = item.c9;
                model.ExceedNineTimesCount = item.c10;

                model.OncePrice = item.a1;
                model.TwicePrice = item.a1;
                model.ThreeTimesPrice = item.a1;
                model.FourTimesPrice = item.a1;
                model.FiveTimesPrice = item.a1;
                model.SixTimesPrice = item.a1;
                model.SevenTimesPrice = item.a1;
                model.EightTimesPrice = item.a1;
                model.NineTimesPrice = item.a1;
                model.ExceedNineTimesPrice = item.a1;
                model.CreateTime = DateTime.Now;
                model.StatisticalTime = Convert.ToDateTime(item.date);

                //using (IUnitOfWork tran = EdsUtilOfWorkFactory.GetUnitOfWorkOfEDS())
                //{
                _dao.InsertDataCrossShopStatisticLogLog(model);
                //  tran.Complete();
                //}
            }

            return true;
        }

        /// <summary>
        /// 获取几天前跨店抢单骑士统计
        /// 胡灵波-20150124
        /// </summary>
        /// <param name="daysAgo">几天前</param>
        /// <returns></returns>
        public IList<BusinessesDistributionModel> GetCrossShopLogInfo(int daysAgo)
        {
            return _dao.GetClienterCrossShopLogInfo(daysAgo);
        }

        /// <summary>
        /// 判断是否存在指定日期的统计信息
        /// 胡灵波-20150124
        /// </summary>
        /// <param name="statisticalTime">指定日期</param>
        /// <returns></returns>
        bool IsExistClienterCrossShopLog(string statisticalTime)
        {
            bool isExist = false;

            isExist = _dao.IsExistCrossShopStatisticLog(statisticalTime);

            return isExist;
        }
    }
}
