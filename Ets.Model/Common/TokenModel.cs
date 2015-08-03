using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.Common
{
    /// <summary>
    /// T检查参数model
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// 手机唯一标Ssid
        /// </summary>
        public string Ssid { get; set; }
        /// <summary>
        /// 商家或骑士唯一健值guid
        /// </summary>
        public string Appkey { get; set; }

    }
}
