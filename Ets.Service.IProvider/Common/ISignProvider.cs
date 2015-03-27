using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Common
{

    /// <summary>
    /// sign签名信息处理类  add by caoheyang 20150327
    /// </summary>

    public interface ISignProvider
    {  /// <summary>
        /// 获取该集团的sign签名信息 add by caoheyang 20150327
        /// </summary>
        /// <param name="groupId">集团id</param>
        /// <returns></returns>
         string GetSign(int groupId);
    }
}
