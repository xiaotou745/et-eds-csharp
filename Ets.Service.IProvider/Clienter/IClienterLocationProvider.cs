using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Bussiness;

namespace Ets.Service.IProvider.Clienter
{
    /// <summary>
    /// 骑士轨迹表  
    /// <UpdateBy>caoheyang</UpdateBy>
    /// <UpdateTime>20150519</UpdateTime>
    /// </summary>
    public interface IClienterLocationProvider
    {
        /// <summary>
        /// 插入骑士运行轨迹 
        /// </summary>
        /// <returns></returns>
        ResultModel<object> InsertLocaltion(ClienterPushLocaltionPM model);
    }
}
