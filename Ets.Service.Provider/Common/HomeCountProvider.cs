using Ets.Dao.Clienter;
using Ets.Dao.Order;
using Ets.Dao.User;
using Ets.Model.Common;
using Ets.Service.IProvider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Common
{
    public class HomeCountProvider : IHomeCountProvider
    {

        /// <summary>
        /// 获取homecount页的数量
        /// 签约商户：xx 家 已申请超人：xx 个，通过超人 xx 个，今日订单： xx ，今日订单金额： xx 元
        /// </summary>
        /// <returns></returns>
        public HomeCountTitleModel GetHomeCountTitle()
        {
            int applyCount = 0;
            int bCount = 0;
            ClienterDao clienterDao = new ClienterDao();


            int orderCount = 0;
            decimal orderMoney = 0;
            OrderDao orderDao = new OrderDao();
            orderDao.GetCurrentDateCountAndMoney(out orderCount, out orderMoney);
            clienterDao.GetCountAndMoney(out applyCount, out bCount);
            HomeCountTitleModel model = new HomeCountTitleModel()
            {
                ApplySuperMan = applyCount,//申请骑士
                AuditPassSuperMan = bCount,//审核通过骑士
                SignBusiness = new BusinessDao().GetBusinessCount(),//签约商户
                TodayOrders = orderCount,
                TodayOrdersAmount = orderMoney
            };
            return model;
        }
    }
}
