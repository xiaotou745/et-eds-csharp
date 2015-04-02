﻿using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Order
{
    /// <summary>
    /// 网站补贴，佣金等工厂类 add by caoheyang 20150330         
    /// </summary>
    public class CommissionFactory
    {
        public static OrderPriceProvider GetCommission()
        {
            switch (ConfigSettings.Instance.OrderCommissionType)
            {
                case 0:
                    return new DefaultOrPriceProvider();
                case 1:
                    return new TimeOrPriceProvider();
                default:
                    return new DefaultOrPriceProvider();
            }

        }
    }
}
