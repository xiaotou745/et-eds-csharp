using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Dao.DeliveryManager;
using ETS.Data.PageData;
using Ets.Model.DataModel.Clienter;
using Ets.Model.ParameterModel.Clienter;
using Ets.Service.IProvider.DeliveryManager;

namespace Ets.Service.Provider.DeliveryManager
{
    public class DeliveryManagerProvider:IDeliveryManagerProvider
    {
        readonly DeliveryManagerDao deliveryManagerDao=new DeliveryManagerDao();
        /// <summary>
        /// 获取骑士信息列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<ClienterListModel> GetClienterList(ClienterSearchCriteria criteria)
        {
            PageInfo<ClienterListModel> pageinfo = deliveryManagerDao.GetClienterList<ClienterListModel>(criteria);
            return pageinfo;
        }
    }
}
