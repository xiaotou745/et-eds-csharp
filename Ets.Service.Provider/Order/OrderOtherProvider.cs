﻿#region
using CalculateCommon;
using Ets.Dao.Order;
using Ets.Model.Common;
using Ets.Model.DataModel.Bussiness;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.DomainModel.Order;
using Ets.Model.ParameterModel.Order;
using Ets.Service.IProvider.Order;
using Ets.Service.IProvider.Subsidy;
using Ets.Service.IProvider.User;
using Ets.Service.Provider.MyPush;
using Ets.Service.Provider.Subsidy;
using Ets.Service.Provider.User;
using ETS.Enums;
using ETS.Data.PageData;
using ETS.Page;
using ETS.Transaction;
using ETS.Transaction.Common;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DomainModel.Subsidy;
using Newtonsoft.Json.Linq;
using Ets.Service.Provider.OpenApi;
using System.Configuration;
using System.Net.Http;
using Ets.Dao.User;
using Ets.Dao.GlobalConfig;
using Ets.Service.Provider.Common;
using ETS.Const;
using Ets.Service.Provider.Clienter;
using Ets.Service.IProvider.OpenApi;
using Ets.Model.ParameterModel.Bussiness;
using Ets.Service.IProvider.Statistics;
using Ets.Model.DataModel.Strategy;
using Ets.Service.Provider.Order;
using Ets.Model.DomainModel.Order;
#endregion
namespace Ets.Service.Provider.Order
{
    public class OrderOtherProvider : IOrderOtherProvider
    {
        private OrderOtherDao orderOtherDao = new OrderOtherDao(); 

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="grabLongitude"></param>
        /// <param name="grabLatitude"></param>
        public void Update(string orderNo, float grabLongitude, float grabLatitude)
        {
             orderOtherDao.UpdateGrab(orderNo, grabLongitude, grabLatitude);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="completeLongitude"></param>
        /// <param name="completeLatitude"></param>
        public void UpdateComplete(string orderNo, float completeLongitude, float completeLatitude)
        {
            orderOtherDao.UpdateComplete(orderNo, completeLongitude, completeLatitude);
        }

        /// <summary>
        /// 更新已提现
        /// </summary>
        public void UpdateIsJoinWithdraw(int orderId)
        {
             orderOtherDao.UpdateIsJoinWithdraw(orderId);
        }
    }
}
