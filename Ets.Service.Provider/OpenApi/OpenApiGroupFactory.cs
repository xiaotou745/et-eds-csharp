using ETS.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.OpenApi
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
        public static Ets.Service.Provider.OpenApi.IGroupProviderOpenApi Create(int groupId)
        {
            switch (groupId)
            {
                case SystemConst.Group2:  //万达
                    return new WanDaGroup();
                case SystemConst.Group3: //全时
                    return new FulltimeGroup();
                default:
                    return null;
            }

        }
    }
}
