using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Clienter
{
    public class ClienterReceivePushModel
    {
        /// <summary>
        /// 其实ID
        /// </summary>
        public int ClienterId { get; set; }
        /// <summary>
        /// 是否接受 1 接受 0 不接受
        /// </summary>
        public int IsReceive { get; set; }
    }
}
