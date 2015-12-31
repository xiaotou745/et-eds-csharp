using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PushServiceWCF
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IPushSer”。
    [ServiceContract]
    public interface IPushSer
    {
        [OperationContract]
        void DoWork();
      
        /// <summary>
        /// 向手机推送消息
        /// </summary>
        /// <param name="msginfo"></param>
        [OperationContract]
        void PushForMobile(string msginfo);
    }
}
