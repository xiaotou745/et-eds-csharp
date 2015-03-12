using System;
using System.Collections.Generic;
using ETS.Dao;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;

namespace Ets.Dao.Clienter
{

    /// <summary>
    /// 超人数据层
    /// </summary>
    public class ClienterDao : DaoBase
    {
          //using (var dbEntity = new supermanEntities())
          //  {
          //      var query = dbEntity.order.AsQueryable();
          //      if (!string.IsNullOrWhiteSpace(criteria.city))
          //          query = query.Where(i => i.business.City == criteria.city.Trim());
          //      if (!string.IsNullOrWhiteSpace(criteria.cityId))
          //          query = query.Where(i => i.business.CityId == criteria.cityId.Trim());
          //      query = query.Where(i => i.Status.Value == ConstValues.ORDER_NEW);0
          //      query = query.OrderByDescending(i => i.PubDate);
          //      var result = query.ToList();
          //      return result;
          //  }
        public List<order> GetOrdersNoLoginLatest(ClientOrderSearchCriteria criteria)
        {
            string sql = "";
            return null;
        }

    }
}
