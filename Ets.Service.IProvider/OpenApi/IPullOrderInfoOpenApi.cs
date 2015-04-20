using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.OpenApi
{
    /// <summary>
    /// 从第三方抓取订单详情数据 目前仅美团用该方式 20150420
    /// </summary>
    public interface IPullOrderInfoOpenApi
    {
        void PullOrderInfo(string info);
    }
}
