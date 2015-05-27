using ETS.Const;
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
        /// <returns></returns>
        public static IProvider.OpenApi.IGroupProviderOpenApi Create(int groupId)
        {
            LogHelper.LogWriter("创建订单同步：", "groupId:" + groupId);
            switch (groupId)
            {
                case SystemConst.Group1: //聚网客
                    return new JuWangKeGroup(); 
                case SystemConst.Group2:  //万达
                    return new WanDaGroup();
                case SystemConst.Group3: //全时
                    return new FulltimeGroup();
                case SystemConst.Group4: //美团
                    return new MeiTuanGroup();
                case SystemConst.Group6: //首旅集团
                    return new TourismGroup(); 
                default:
                    return null;
            }

        }

        /// <summary>
        /// 获取集团对应的 回调业务类  add by caoheyang 20150326
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static IProvider.OpenApi.IPullOrderInfoOpenApi GetIPullOrderInfo(int groupId)
        {
            switch (groupId)
            {
                default:
                    return null;
            }

        }
    }
}
