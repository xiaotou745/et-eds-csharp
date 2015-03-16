using Ets.Model.DataModel.Clienter;
using ETS;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
using ETS.Extension;
using ETS.Page;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.Order
{
    public class OrderDao : DaoBase
    {
        public PagedList<Model.DataModel.Order.order> GetOrders(ClientOrderSearchCriteria criteria)
        {
            PagedList<Model.DataModel.Order.order> orderPageList = new PagedList<Model.DataModel.Order.order>();
            //排序
            string orderByStr = " o.Id ";
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
        o.ReceiveProvince ,
        o.ReceiveArea ,
        o.ReceiveProvinceCode ,
        o.ReceiveCityCode ,
        o.ReceiveAreaCode , 
        o.SongCanDate ,
        o.OrderCount ,
        o.CommissionRate ,
        b.Name BusinessName,
        b.PhoneNo BusinessPhone,
        b.City PickUpCity,
        b.Longitude BusiLongitude,
        b.Latitude BusiLatitude ");
            //关联表
            StringBuilder tableListStr = new StringBuilder();
            tableListStr.Append(@" dbo.[order] o WITH ( NOLOCK )
        LEFT JOIN dbo.clienter c WITH ( NOLOCK ) ON c.Id = o.clienterId
        LEFT JOIN dbo.business b WITH ( NOLOCK ) ON b.Id = o.businessId ");
            //条件
            StringBuilder whereStr = new StringBuilder(" 1=1 ");
            if (criteria.userId != 0)
            {
                whereStr.AppendFormat(" AND o.clienterId = {0}", criteria.userId);
            }
            if (!string.IsNullOrWhiteSpace(criteria.city))
            {
                whereStr.AppendFormat(" AND b.City = '{0}'", criteria.city);
            }
            if (!string.IsNullOrWhiteSpace(criteria.cityId))
            {
                whereStr.AppendFormat(" AND b.CityId = '{0}'", criteria.cityId);
            }
            if (criteria.status != -1 && criteria.status != null)
            {
                whereStr.AppendFormat(" AND o.[Status] = {0}", criteria.status);
            }
            else
            {
                whereStr.Append(" AND o.[Status] = 0 ");  //这里改为枚举值
            }

            var pageInfo = new PageHelper().GetPages<Model.DataModel.Order.order>(SuperMan_Read, criteria.PagingRequest.PageIndex, whereStr.ToString(), orderByStr, columnStr.ToString(), tableListStr.ToString(), criteria.PagingRequest.PageSize, true);

            orderPageList.ContentList = pageInfo.Records.ToList();
            orderPageList.CurrentPage = pageInfo.Index;  //当前页
            orderPageList.PageCount = pageInfo.PageCount;//总页数
            orderPageList.PageSize = criteria.PagingRequest.PageSize;

            return orderPageList;
        }

        #region   订单状态查询功能  add by caoheyang 20150316
        /// <summary>
        /// 订单状态查询功能  add by caoheyang 20150316
        /// </summary>
        /// <param name="orderNo">订单号码</param>
        /// <returns></returns>
        public int? GetStatus(string orderNo)
        {
            const string querySql = @"SELECT top 1  [Status] FROM [order]  WITH ( NOLOCK ) WHERE OrderNo=@OrderNo";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@OrderNo", orderNo);    //订单号
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, querySql, dbParameters);
            return ParseHelper.ToInt(executeScalar);
        }
        #endregion
    }
}
