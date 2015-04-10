using Ets.Dao.GlobalConfig;
using ETS.Util;
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
            switch (GlobalConfigDao.GlobalConfigGet.CommissionFormulaMode)
            {
                case 0:
                    return new DefaultOrPriceProvider();
                case 1:
                    return new TimeOrPriceProvider();
                case 2:
                    return new BreakEvenPointOrPriceProvider();
                case 3:
                    return new AmountOrPriceProvider();
                default:
                    return new DefaultOrPriceProvider();
            }

        }
    }
}
