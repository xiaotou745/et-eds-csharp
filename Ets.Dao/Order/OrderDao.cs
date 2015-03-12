using ETS.Dao;
using ETS.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.Order
{
    public class OrderDao : DaoBase
    {
        public PagedList<Model.DataModel.Order.order> GetOrders(Model.ParameterModel.Order.ClientOrderSearchCriteria criteria)
        {
            PagedList<Model.DataModel.Order.order> orderPageList = new PagedList<Model.DataModel.Order.order>();
             
            //列名
            StringBuilder columnStr = new StringBuilder(@"
        o.Id ,
        o.OrderNo ,
        o.PickUpAddress ,
        o.PubDate ,
        o.ReceviceName ,
        o.RecevicePhoneNo ,
        o.ReceviceAddress ,
        o.ActualDoneDate ,
        o.IsPay ,
        o.Amount ,
        o.OrderCommission ,
        o.DistribSubsidy ,
        o.WebsiteSubsidy ,
        o.Remark ,
        o.Status ,
        o.clienterId ,
        o.businessId ,
        o.ReceviceCity ,
        o.ReceviceLongitude ,
        o.ReceviceLatitude ,
        o.OrderFrom ,
        o.OriginalOrderId ,
        o.OriginalOrderNo ,
        o.Quantity ,
        o.Weight ,
        o.ReceiveProvince ,
        o.ReceiveArea ,
        o.ReceiveProvinceCode ,
        o.ReceiveCityCode ,
        o.ReceiveAreaCode ,
        o.OrderType ,
        o.KM ,
        o.GuoJuQty ,
        o.LuJuQty ,
        o.SongCanDate ,
        o.OrderCount ,
        o.CommissionRate ");
            //条件
            StringBuilder whereStr = new StringBuilder(" 1=1 ");
            if (criteria.userId != 0)
            {
                whereStr.Append(" AND o.clienterId =  @clienterId ");
            }
            if (!string.IsNullOrWhiteSpace(criteria.city))
            {
                whereStr.Append(" AND b.City =  @City ");
            }
            if (!string.IsNullOrWhiteSpace(criteria.cityId))
            {
                whereStr.Append(" AND b.CityId =  @CityId ");
            }
            if (criteria.status != -1 && criteria.status != null)
            {
                whereStr.Append(" AND o.[Status] =  @Status ");
            }
            else
            {
                whereStr.Append(" AND o.[Status] = 0 ");  //这里改为枚举值
            }
            //排序
            string orderByStr = " o.Id ";
            //关联表
            StringBuilder tableListStr = new StringBuilder();
            tableListStr.Append(@" dbo.[order] o WITH ( NOLOCK )
        LEFT JOIN dbo.clienter c WITH ( NOLOCK ) ON c.Id = o.clienterId
        LEFT JOIN dbo.business b WITH ( NOLOCK ) ON b.Id = o.businessId");

            





            return orderPageList;
        } 
    }
}
