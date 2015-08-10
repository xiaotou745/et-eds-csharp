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
        /// <summary>
        ///  
        /// </summary>
        /// <param name="commissionFormulaMode">策略id</param>
        /// <returns></returns>
        public static OrderPriceProvider GetCommission(int commissionFormulaMode)
        {
            switch (commissionFormulaMode)
            {
                case 0:
                    return new DefaultOrPriceProvider();
                case 1:
                    return new TimeOrPriceProvider();
                case 2:
                    return new BreakEvenPointOrPriceProvider();
                case 3:
                    return new AmountOrPriceProvider();
                case 4:
                    return new BaseCommissionOrPriceProvider();
                default:
                    return new DefaultOrPriceProvider();
            }

        }
    }
}
