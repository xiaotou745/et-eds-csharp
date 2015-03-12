using System;
using System.Collections.Generic;
using ETS.Dao;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using ETS.Data.Core;
using ETS.Util;


//using ETS;
//using ETS.Dao;
//using ETS.Data;
//using ETS.Data.Core;
//using ETS.Data.PageData;
//using ETS.Util;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

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


        /// <summary>
        /// 商户获取订单   add by caoheyang 20150311
        /// </summary>
        /// <param name="paraModel">参数实体</param>
        public virtual int ChangeWorkStatusToSql(Ets.Model.ParameterModel.Clienter.ChangeWorkStatusPM paraModel)
        {
            const string updateSql = @"UPDATE dbo.clienter SET WorkStatus =@WorkStatus  WHERE id=@id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("id", paraModel.Id);    //超人id
            dbParameters.AddWithValue("WorkStatus", paraModel.WorkStatus);  //目标超人工作状态
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Write, updateSql, dbParameters);
            int a = ParseHelper.ToInt(executeScalar, 0);
            return a;
        }
    }
}
