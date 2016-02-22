using ETS.Const;
using Ets.Service.IProvider.OpenApi;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.OpenApi
{
    /// <summary>
    /// 第三方对接集团的回调业务工厂 add by caoheyang 20150326
    /// </summary>
    public class OpenApiGroupFactory
    {
        /// <summary>
        /// 获取集团对应的 回调业务类  add by caoheyang 20150326
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public static IGroupProviderOpenApi Create(int groupId,string returnUrl)
        {
            LogHelper.LogWriter("创建订单同步：", "groupId:" + groupId);
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return new DefaultGroup();
            }
            switch (groupId)
            {
                case GroupConst.Group1: //聚网客
                    return new JuWangKeGroup();
                case GroupConst.Group2:  //万达
                    return new WanDaGroup();
                case GroupConst.Group3: //全时
                    return new FulltimeGroup();
                case GroupConst.Group4: //美团
                    return new MeiTuanGroup();
                case GroupConst.Group6: //首旅集团
                    return new TourismGroup();
                case GroupConst.Group100: //淘点点
                    return new TaoDianDianGroup(); 
                default:
                    return null;
            }

        }

        /// <summary>
        /// 获取集团对应的 佣金计算类  add by caoheyang 20150326
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public static IGroupSetCommissonOpenApi CreateSetCommission(int groupId)
        {
            LogHelper.LogWriter("创建订单同步：", "groupId:" + groupId);
            switch (groupId)
            {
                case GroupConst.Group1: //聚网客
                    return new JuWangKeGroup();
                case GroupConst.Group2:  //万达
                    return new WanDaGroup();
                case GroupConst.Group3: //全时
                    return new FulltimeGroup();
                case GroupConst.Group4: //美团
                    return new MeiTuanGroup();
                case GroupConst.Group6: //首旅集团
                    return new TourismGroup();
                case GroupConst.Group100: //淘点点
                    return new TaoDianDianGroup();
                default:
                    return null;
            }

        }
    }
}
