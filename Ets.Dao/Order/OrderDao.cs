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
        public int GetStatus(string orderNo)
        {
            const string querySql = @"SELECT top 1  [Status] FROM [order]  WITH ( NOLOCK ) WHERE OrderNo=@OrderNo";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@OrderNo", orderNo);    //订单号
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, querySql, dbParameters);
            return ParseHelper.ToInt(executeScalar);
        }
        #endregion

        public int AddOrder(Model.DataModel.Order.order order)
        {
            StringBuilder insertOrder = new StringBuilder();

            insertOrder.Append(@"INSERT INTO dbo.[order]
         ( OrderNo ,
           PickUpAddress ,
           PubDate ,
           ReceviceName ,
           RecevicePhoneNo ,
           ReceviceAddress ,
           IsPay ,
           Amount ,
           OrderCommission ,
           DistribSubsidy ,
           WebsiteSubsidy ,
           Remark ,
           OrderFrom ,
           Status ,
           businessId ,
           ReceviceCity ,
           ReceviceLongitude ,
           ReceviceLatitude ,
           OrderCount ,
           CommissionRate
         )
 VALUES  ( @OrderNo ,
           @PickUpAddress ,
           @PubDate ,
           @ReceviceName ,
           @RecevicePhoneNo ,
           @ReceviceAddress ,
           @IsPay ,
           @Amount ,
           @OrderCommission ,
           @DistribSubsidy ,
           @WebsiteSubsidy ,
           @Remark ,
           @OrderFrom ,
           @Status ,
           @businessId ,
           @ReceviceCity ,
           @ReceviceLongitude ,
           @ReceviceLatitude ,
           @OrderCount ,
           @CommissionRate
         )");

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OrderNo", order.OrderNo);
            parm.AddWithValue("@PickUpAddress", order.PickUpAddress);
            parm.AddWithValue("@PubDate", order.PubDate);
            parm.AddWithValue("@ReceviceName", order.ReceviceName);
            parm.AddWithValue("@RecevicePhoneNo", order.RecevicePhoneNo);

            parm.AddWithValue("@ReceviceAddress", order.ReceviceAddress);
            parm.AddWithValue("@IsPay", order.IsPay);
            parm.AddWithValue("@Amount", order.Amount);
            parm.AddWithValue("@OrderCommission", order.OrderCommission);
            parm.AddWithValue("@DistribSubsidy", order.DistribSubsidy);

            parm.AddWithValue("@WebsiteSubsidy", order.WebsiteSubsidy);
            parm.AddWithValue("@Remark", order.Remark);
            parm.AddWithValue("@OrderFrom", order.OrderFrom);
            parm.AddWithValue("@Status", order.Status);
            parm.AddWithValue("@businessId", order.businessId);

            parm.AddWithValue("@ReceviceCity", order.ReceviceCity);
            parm.AddWithValue("@ReceviceLongitude", order.ReceviceLongitude);
            parm.AddWithValue("@ReceviceLatitude", order.ReceviceLatitude);
            parm.AddWithValue("@OrderCount", order.OrderCount);
            parm.AddWithValue("@CommissionRate", order.CommissionRate);
            return DbHelper.ExecuteNonQuery(SuperMan_Read, insertOrder.ToString(), parm);

        }

        #region  第三方对接 物流订单接收接口  add by caoheyang 201503167
        /// <summary>
        /// 第三方对接 物流订单接收接口  add by caoheyang 201503167
        /// </summary>
        /// <param name="paramodel">参数实体</param>
        /// <returns></returns>
        public int CreateToSql(Ets.Model.ParameterModel.Order.CreatePM_OpenApi paramodel)
        {
            #region 操作插入business表
            ///商户插入sql
            const string insertBussinesssql = @"
                INSERT INTO dbo.business
                (OriginalBusiId,Name,GroupId,IDCard,
                PhoneNo,PhoneNo2,Address,ProvinceCode,CityCode,AreaCode,
                Longitude,Latitude,DistribSubsidy,CommissionTypeId)
                values(@OriginalBusiId,@Name,@GroupId,@IDCard,
                @PhoneNo,@PhoneNo2,@Address,@ProvinceCode,@CityCode,@AreaCode,
                @Longitude,@Latitude,@DistribSubsidy,@CommissionTypeId)";
            IDbParameters insertBdbParameters = DbHelper.CreateDbParameters();
            ///基本参数信息
            insertBdbParameters.AddWithValue("@OriginalBusiId", paramodel.store_info.store_id); //对接方店铺ID第三方平台推送过来的商家Id
            insertBdbParameters.AddWithValue("@Name", paramodel.store_info.store_name);    //店铺名称
            insertBdbParameters.AddWithValue("@GroupId", paramodel.store_info.group);    //集团：3:万达
            insertBdbParameters.AddWithValue("@IDCard", paramodel.store_info.id_card);    //店铺身份证号
            insertBdbParameters.AddWithValue("@PhoneNo", paramodel.store_info.phone);    //门店联系电话
            insertBdbParameters.AddWithValue("@PhoneNo2", paramodel.store_info.phone2);    //门店第二联系电话
            insertBdbParameters.AddWithValue("@Address", paramodel.store_info.address);    //门店地址
            insertBdbParameters.AddWithValue("@ProvinceCode", paramodel.store_info.city_code);    //门店所在省份code
            insertBdbParameters.AddWithValue("@CityCode", paramodel.store_info.city_code);    //门店所在城市code
            insertBdbParameters.AddWithValue("@AreaCode", paramodel.store_info.area_code);    //门店所在区域code
            insertBdbParameters.AddWithValue("@Longitude", paramodel.store_info.longitude);    //门店所在区域经度
            insertBdbParameters.AddWithValue("@Latitude", paramodel.store_info.latitude);    //门店所在区域纬度
            insertBdbParameters.AddWithValue("@DistribSubsidy", paramodel.store_info.delivery_fee);    //外送费,默认为0
            insertBdbParameters.AddWithValue("@CommissionTypeId", paramodel.store_info.commission_type);    //佣金类型，涉及到快递员的佣金计算方式，默认1
            #endregion

            #region 操作插入order表
            ///订单插入sql
            const string insertOrdersql = @" 
                INSERT INTO dbo.[order](
                OriginalOrderNo,PubDate,IsPay,Amount,
                Remark,Weight,DistribSubsidy,OrderCount,ReceviceName,
                RecevicePhoneNo,ReceiveProvinceCode,ReceiveCityCode,ReceiveAreaCode,ReceviceAddress,
                ReceviceLongitude,ReceviceLatitude)
                Values(
                @OriginalOrderNo,@PubDate，@IsPay，@Amount,
                @Remark,@Weight,@DistribSubsidy,@OrderCount,@ReceviceName,
                @RecevicePhoneNo,@ReceiveProvinceCode,@ReceiveCityCode,@ReceiveAreaCode,@ReceviceAddress,
                @ReceviceLongitude,@ReceviceLatitude)";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            ///基本参数信息
            dbParameters.AddWithValue("@OriginalOrderNo", paramodel.order_id);    //其它平台的来源订单号
            dbParameters.AddWithValue("@PubDate", paramodel.create_time);    //订单下单时间
            //要求送餐时间
            dbParameters.AddWithValue("@IsPay", paramodel.is_pay);    //是否已付款
            dbParameters.AddWithValue("@Amount", paramodel.total_price);    //订单金额
            dbParameters.AddWithValue("@Remark", paramodel.remark);    //备注
            dbParameters.AddWithValue("@Weight", paramodel.weight);    //重量，默认?
            dbParameters.AddWithValue("@DistribSubsidy", paramodel.delivery_fee);    //外送费,默认？
            dbParameters.AddWithValue("@OrderCount", paramodel.package_count);    //订单数量，默认为1
            ///收货地址信息
            dbParameters.AddWithValue("@ReceviceName", paramodel.address.user_name);    //用户姓名 收货人姓名
            dbParameters.AddWithValue("@RecevicePhoneNo", paramodel.address.user_phone);    //用户联系电话  收货人电话
            dbParameters.AddWithValue("@ReceiveProvinceCode", paramodel.address.province_code);    //用户所在省份code
            dbParameters.AddWithValue("@ReceiveCityCode", paramodel.address.city_code);    //用户所在城市code
            dbParameters.AddWithValue("@ReceiveAreaCode", paramodel.address.area_code);    //用户所在区域code
            dbParameters.AddWithValue("@ReceviceAddress", paramodel.address.address);    //用户收货地址
            dbParameters.AddWithValue("@ReceviceLongitude", paramodel.address.longitude);    //用户收货地址所在区域经度
            dbParameters.AddWithValue("@ReceviceLatitude", paramodel.address.latitude);    //用户收货地址所在区域纬度
            object executeScalar = DbHelper.ExecuteNonQuery(SuperMan_Read,insertOrdersql, dbParameters);
            #endregion

            return ParseHelper.ToInt(executeScalar);
        }
        #endregion

    }
}
