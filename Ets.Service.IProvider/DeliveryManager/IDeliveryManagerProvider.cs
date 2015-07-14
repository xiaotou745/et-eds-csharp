using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Data.PageData;
using Ets.Model.DataModel.Clienter;
using Ets.Model.ParameterModel.Clienter;

namespace Ets.Service.IProvider.DeliveryManager
{
    public interface IDeliveryManagerProvider
    {
        /// <summary>
        /// 获取骑士信息列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PageInfo<ClienterListModel> GetClienterList(ClienterSearchCriteria criteria);
    }
}
