﻿using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Order;
using Ets.Model.ParameterModel.Order;
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
        public ETS.Page.PagedList<Model.DataModel.Order.order> GetOrders(ClientOrderSearchCriteria criteria)
        {
            ETS.Page.PagedList<Model.DataModel.Order.order> orderPageList = new ETS.Page.PagedList<Model.DataModel.Order.order>();
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
                if (criteria.city.Contains("北京"))   //易淘食注册的商家 city  为北京城区， 通过b端注册是 北京市，不统一 wc 
                {
                    whereStr.AppendFormat(" AND b.City LIKE '北京%'", criteria.city);
                }
                else
                {
                    whereStr.AppendFormat(" AND b.City = '{0}'", criteria.city);
                }
            }
            //if (!string.IsNullOrWhiteSpace(criteria.cityId))
            //{
            //    if (criteria.cityId == "1")  //目前北京市 的 id 在 康珍那里是  1 ， 但是 第三方过来的是code  10201 ，需要统一，康珍那里改
            //    {
            //        whereStr.AppendFormat(" AND ( b.CityId = '{0}' OR b.CityId = '10201' )", criteria.cityId);
            //    }
            //    else
            //    {
            //        whereStr.AppendFormat(" AND b.CityId = '{0}'", criteria.cityId);
            //    }
            //}
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
        /// <param name="groupId">集团id</param>
        /// <returns>订单状态</returns>
        public int GetStatus(string OriginalOrderNo, int groupId)
        {
            const string querySql = @"SELECT top 1  a.Status FROM [order] a  WITH ( NOLOCK )  
            LEFT JOIN  dbo.business AS b ON a.businessId=b.Id
            WHERE OriginalOrderNo=@OriginalOrderNo AND b.GroupId=@GroupId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@OriginalOrderNo", OriginalOrderNo);    //第三方平台订单号
            dbParameters.AddWithValue("@GroupId", groupId);    //集团id
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, querySql, dbParameters);
            return ParseHelper.ToInt(executeScalar, -1);
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
        /// <param name="paramodel">订单号码</param>
        /// <returns>订单号</returns>
        public string CreateToSql(Ets.Model.ParameterModel.Order.CreatePM_OpenApi paramodel)
        {

            #region 操作business表取商户id
            int bussinessId;//商户id
            ///查询该商户是否已存在
            const string queryBussinesssql = @"
                select top 1 id from  dbo.business
                where GroupId=@GroupId and OriginalBusiId=@OriginalBusiId;";
            IDbParameters queryBdbParameters = DbHelper.CreateDbParameters();
            queryBdbParameters.AddWithValue("@GroupId", paramodel.store_info.group); //集团id
            queryBdbParameters.AddWithValue("@OriginalBusiId", paramodel.store_info.store_id);    //对接方店铺ID第三方平台推送过来的商家Id
            bussinessId = ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, queryBussinesssql, queryBdbParameters));
            if (bussinessId == 0)  //如果商户不存在
            {
                ///商户插入sql
                const string insertBussinesssql = @"
                INSERT INTO dbo.business
                (OriginalBusiId,Name,GroupId,IDCard,Password,
                PhoneNo,PhoneNo2,Address,ProvinceCode,CityCode,AreaCode,
                Longitude,Latitude,DistribSubsidy,CommissionTypeId)  
                OUTPUT Inserted.Id   
                values(@OriginalBusiId,@Name,@GroupId,@IDCard,@Password,
                @PhoneNo,@PhoneNo2,@Address,@ProvinceCode,@CityCode,@AreaCode,
                @Longitude,@Latitude,@DistribSubsidy,@CommissionTypeId);";
                IDbParameters insertBdbParameters = DbHelper.CreateDbParameters();
                ///基本参数信息
                insertBdbParameters.AddWithValue("@OriginalBusiId", paramodel.store_info.store_id); //对接方店铺ID第三方平台推送过来的商家Id
                insertBdbParameters.AddWithValue("@Name", paramodel.store_info.store_name);    //店铺名称
                insertBdbParameters.AddWithValue("@GroupId", paramodel.store_info.group);    //集团：3:万达
                insertBdbParameters.AddWithValue("@IDCard", paramodel.store_info.id_card);    //店铺身份证号
                insertBdbParameters.AddWithValue("@Password", MD5Helper.MD5("123456"));    //初始化密码  后期个改为常量
                insertBdbParameters.AddWithValue("@PhoneNo", paramodel.store_info.phone);    //门店联系电话
                insertBdbParameters.AddWithValue("@PhoneNo2", paramodel.store_info.phone2);    //门店第二联系电话
                insertBdbParameters.AddWithValue("@Address", paramodel.store_info.address);    //门店地址
                insertBdbParameters.AddWithValue("@ProvinceCode", paramodel.store_info.city_code);    //门店所在省份code
                insertBdbParameters.AddWithValue("@CityCode", paramodel.store_info.city_code);    //门店所在城市code
                insertBdbParameters.AddWithValue("@AreaCode", paramodel.store_info.area_code);    //门店所在区域code
                insertBdbParameters.AddWithValue("@Longitude", paramodel.store_info.longitude);    //门店所在区域经度
                insertBdbParameters.AddWithValue("@Latitude", paramodel.store_info.latitude);    //门店所在区域纬度
                insertBdbParameters.AddWithValue("@DistribSubsidy", paramodel.store_info.delivery_fee);    //外送费,默认为0
                insertBdbParameters.AddWithValue("@CommissionTypeId", paramodel.store_info.commission_type == null ?
                    1 : paramodel.store_info.commission_type);   //佣金类型，涉及到快递员的佣金计算方式，默认1
                bussinessId = ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, insertBussinesssql, insertBdbParameters));
                if (bussinessId == 0)
                    return null;//添加失败 
            }
            #endregion

            #region 操作插入order表

            const string queryOrderSql = @"SELECT count(a.id) FROM [order] a  WITH ( NOLOCK )  LEFT JOIN  dbo.business AS b ON a.businessId=b.Id
                     WHERE OriginalOrderNo=@OriginalOrderNo AND b.GroupId=@GroupId";
            IDbParameters queryOrderSqldbParameters = DbHelper.CreateDbParameters();
            queryOrderSqldbParameters.AddWithValue("@OriginalOrderNo", paramodel.order_id);    //第三方平台订单号
            queryOrderSqldbParameters.AddWithValue("@GroupId", paramodel.store_info.group);    //集团id
            int orderExists = ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, queryOrderSql, queryOrderSqldbParameters));
            if (orderExists > 0)
                return null;//添加失败 
            ///订单插入sql
            const string insertOrdersql = @" 
                INSERT INTO dbo.[order](OrderNo,
                OriginalOrderNo,PubDate,SongCanDate,IsPay,Amount,
                Remark,Weight,DistribSubsidy,OrderCount,ReceviceName,
                RecevicePhoneNo,ReceiveProvinceCode,ReceiveCityCode,ReceiveAreaCode,ReceviceAddress,
                ReceviceLongitude,ReceviceLatitude,businessId,PickUpAddress,Payment,OrderCommission,
                WebsiteSubsidy,CommissionRate )
                OUTPUT Inserted.OrderNo
                Values(@OrderNo,
                @OriginalOrderNo,@PubDate,@SongCanDate,@IsPay,@Amount,
                @Remark,@Weight,@DistribSubsidy,@OrderCount,@ReceviceName,
                @RecevicePhoneNo,@ReceiveProvinceCode,@ReceiveCityCode,@ReceiveAreaCode,@ReceviceAddress,
                @ReceviceLongitude,@ReceviceLatitude,@BusinessId,@PickUpAddress,@Payment,@OrderCommission,
                @WebsiteSubsidy,@CommissionRate)";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            ///基本参数信息
            dbParameters.AddWithValue("@OrderNo", Helper.generateOrderCode(bussinessId));  //根据商户id生成订单号(15位));
            dbParameters.AddWithValue("@OriginalOrderNo", paramodel.order_id);    //其它平台的来源订单号
            dbParameters.AddWithValue("@PubDate", paramodel.create_time);    //订单下单时间
            dbParameters.AddWithValue("@SongCanDate", paramodel.receive_time);  ///要求送餐时间
            dbParameters.AddWithValue("@IsPay", paramodel.is_pay);    //是否已付款
            dbParameters.AddWithValue("@Amount", paramodel.total_price);    //订单金额
            dbParameters.AddWithValue("@Remark", paramodel.remark);    //备注
            dbParameters.AddWithValue("@Weight", paramodel.weight);    //重量，默认?
            //订单外送费  目前 接收了两个外送费 理论必须一致 ，若不一致，以订单上的为准，方便后续扩展
            dbParameters.AddWithValue("@DistribSubsidy", paramodel.delivery_fee);
            dbParameters.AddWithValue("@OrderCount", paramodel.package_count == null ? 1 : paramodel.package_count);   //订单数量，默认为1
            ///收货地址信息
            dbParameters.AddWithValue("@ReceviceName", paramodel.address.user_name);    //用户姓名 收货人姓名
            dbParameters.AddWithValue("@RecevicePhoneNo", paramodel.address.user_phone);    //用户联系电话  收货人电话
            dbParameters.AddWithValue("@ReceiveProvinceCode", paramodel.address.province_code);    //用户所在省份code
            dbParameters.AddWithValue("@ReceiveCityCode", paramodel.address.city_code);    //用户所在城市code
            dbParameters.AddWithValue("@ReceiveAreaCode", paramodel.address.area_code);    //用户所在区域code
            dbParameters.AddWithValue("@ReceviceAddress", paramodel.address.address);    //用户收货地址
            dbParameters.AddWithValue("@ReceviceLongitude", paramodel.address.longitude);    //用户收货地址所在区域经度
            dbParameters.AddWithValue("@ReceviceLatitude", paramodel.address.latitude);    //用户收货地址所在区域纬度
            dbParameters.AddWithValue("@BusinessId", bussinessId);    //商户id
            dbParameters.AddWithValue("@PickUpAddress", paramodel.store_info.address);    //取货地址即商户地址
            dbParameters.AddWithValue("@Payment", paramodel.payment);    //取货地址即商户地址
            dbParameters.AddWithValue("@OrderCommission", paramodel.ordercommission);    //订单骑士佣金
            dbParameters.AddWithValue("@WebsiteSubsidy", paramodel.websitesubsidy);    //网站补贴
            dbParameters.AddWithValue("@CommissionRate", paramodel.commissionrate);    //订单佣金比例
            string orderNo = ParseHelper.ToString(DbHelper.ExecuteScalar(SuperMan_Read, insertOrdersql, dbParameters));
            if (string.IsNullOrWhiteSpace(orderNo))//添加失败 
                return null;
            #endregion

            #region 操作插入OrderDetail表
            bool addBool = true;  //添加是否成功
            for (int i = 0; i < paramodel.order_details.Length; i++)
            {
                ///订单详情插入sql
                const string insertOrderDetailsql = @" 
                 INSERT INTO dbo.OrderDetail
                 (OrderNo ,ProductName , UnitPrice ,Quantity,FormDetailID,GroupID)
                 VALUES  (@OrderNo ,@ProductName ,@UnitPrice ,@Quantity,@FormDetailID,@GroupID)";
                IDbParameters insertOrderDetaiParas = DbHelper.CreateDbParameters();
                ///基本参数信息
                insertOrderDetaiParas.AddWithValue("@OrderNo", orderNo);    //订单号
                insertOrderDetaiParas.AddWithValue("@ProductName", paramodel.order_details[i].product_name);    //商品名称
                insertOrderDetaiParas.AddWithValue("@UnitPrice", paramodel.order_details[i].unit_price);    //商品单价，精确到两位小数
                insertOrderDetaiParas.AddWithValue("@Quantity", paramodel.order_details[i].quantity);    //商品数量
                insertOrderDetaiParas.AddWithValue("@FormDetailID", paramodel.order_details[i].detail_id);    //第三方平台明细id,与GroupID组成联合唯一约束
                insertOrderDetaiParas.AddWithValue("@GroupID", paramodel.store_info.group);    //集团id,与第三方平台明细id组成联合唯一约束
                int orderdetailId = ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Read, insertOrderDetailsql, insertOrderDetaiParas));
            }
            if (!addBool)
                return null;  //添加失败
            #endregion
            return orderNo;
        }
        #endregion


        /// <summary>
        ///获取当天
        ///订单金额
        ///任务量
        ///订单量 
        /// 窦海超
        /// 2015年3月18日 17:23:14
        /// </summary>
        public HomeCountTitleModel GetCurrentDateCountAndMoney(HomeCountTitleModel model)
        {

            string sql = @"SELECT 
                            SUM(ISNULL(Amount,0)) AS OrderPrice, --订单金额
                            ISNULL(COUNT(Id),0) AS MisstionCount,--任务量
                            SUM(ISNULL(OrderCount,0)) AS OrderCount --订单量
                             FROM dbo.[order](NOLOCK) WHERE CONVERT(CHAR(10),PubDate,120)=CONVERT(CHAR(10),GETDATE(),120) AND [Status]=1";
            DataSet set = DbHelper.ExecuteDataset(SuperMan_Read, sql);
            DataTable dt = DataTableHelper.GetTable(set);
            if (dt == null && dt.Rows.Count <= 0)
            {
                return model;
            }

            //DataRow row = dt.Rows[0];
            //Count = ParseHelper.ToInt(row["acount"], 0);
            //Money = ParseHelper.ToDecimal(row["amount"], 0);
           return MapRows<HomeCountTitleModel>(dt)[0];
        }
        /// <summary>
        /// 根据参数获取订单
        /// danny-20150319
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetOrders<T>(OrderSearchCriteria criteria)
        {
            string columnList = @"   o.[Id]
                                    ,o.[OrderNo]
                                    ,o.[PickUpAddress]
                                    ,o.[PubDate]
                                    ,o.[ReceviceName]
                                    ,o.[RecevicePhoneNo]
                                    ,o.[ReceviceAddress]
                                    ,o.[ActualDoneDate]
                                    ,o.[IsPay]
                                    ,o.[Amount]
                                    ,o.[OrderCommission]
                                    ,o.[DistribSubsidy]
                                    ,o.[WebsiteSubsidy]
                                    ,o.[Remark]
                                    ,o.[Status]
                                    ,o.[clienterId]
                                    ,o.[businessId]
                                    ,o.[ReceviceCity]
                                    ,o.[ReceviceLongitude]
                                    ,o.[ReceviceLatitude]
                                    ,o.[OrderFrom]
                                    ,o.[OriginalOrderId]
                                    ,o.[OriginalOrderNo]
                                    ,o.[Quantity]
                                    ,o.[Weight]
                                    ,o.[ReceiveProvince]
                                    ,o.[ReceiveArea]
                                    ,o.[ReceiveProvinceCode]
                                    ,o.[ReceiveCityCode]
                                    ,o.[ReceiveAreaCode] 
                                    ,o.[SongCanDate]
                                    ,o.[OrderCount]
                                    ,o.[CommissionRate] 
                                    ,c.TrueName ClienterName
                                    ,c.PhoneNo ClienterPhoneNo
                                    ,b.Name BusinessName
                                    ,b.PhoneNo BusinessPhoneNo
                                    ,b.Address BusinessAddress
                                    ,g.GroupName";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.AddWithValue("@businessName", criteria.businessName);
                parm.AddWithValue("@businessPhone", criteria.businessPhone);
                parm.AddWithValue("@orderId", criteria.orderId);
                parm.AddWithValue("@OriginalOrderNo", criteria.OriginalOrderNo);
                parm.AddWithValue("@orderStatus", criteria.orderStatus);
                parm.AddWithValue("@superManName", criteria.superManName);
                parm.AddWithValue("@superManPhone", criteria.superManPhone);
                parm.AddWithValue("@orderPubStart", criteria.orderPubStart);
                parm.AddWithValue("@orderPubEnd", criteria.orderPubEnd);
                parm.AddWithValue("@GroupId", criteria.GroupId);
                var sbSqlWhere = new StringBuilder(" 1=1 ");
                if (!string.IsNullOrWhiteSpace(criteria.businessName))
                {
                    sbSqlWhere.AppendFormat(" AND b.Name='{0}' ", criteria.businessName);
                }
                if (!string.IsNullOrWhiteSpace(criteria.businessPhone))
                {
                    sbSqlWhere.AppendFormat(" AND b.PhoneNo='{0}' ", criteria.businessPhone);
                }
                if (!string.IsNullOrWhiteSpace(criteria.orderId))
                {
                    sbSqlWhere.AppendFormat(" AND o.OrderNo='{0}' ", criteria.orderId);
                }
                if (!string.IsNullOrWhiteSpace(criteria.OriginalOrderNo))
                {
                    sbSqlWhere.AppendFormat(" AND o.OriginalOrderNo='{0}' ", criteria.OriginalOrderNo);
                }
                if (criteria.orderStatus != -1)
                {
                    sbSqlWhere.AppendFormat(" AND o.Status={0} ", criteria.orderStatus);
                }
                if (!string.IsNullOrWhiteSpace(criteria.superManName))
                {
                    sbSqlWhere.AppendFormat(" AND c.TrueName='{0}' ", criteria.superManName);
                }
                if (!string.IsNullOrWhiteSpace(criteria.superManPhone))
                {
                    sbSqlWhere.AppendFormat(" AND c.PhoneNo='{0}' ", criteria.superManPhone);
                }
                if (!string.IsNullOrWhiteSpace(criteria.orderPubStart))
                {
                    sbSqlWhere.AppendFormat(" AND o.PubDate>='{0}' ", criteria.orderPubStart);
                }
                if (!string.IsNullOrWhiteSpace(criteria.orderPubEnd))
                {
                    sbSqlWhere.AppendFormat(" AND o.PubDate<='{0}' ", criteria.orderPubEnd);
                }
                if (criteria.GroupId != null && criteria.GroupId !=0)
                {
                    sbSqlWhere.AppendFormat(" AND o.GroupId={0} ", criteria.GroupId);
                }
                string tableList = @" [order] o WITH ( NOLOCK )
                                LEFT JOIN clienter c WITH ( NOLOCK ) ON c.Id = o.clienterId
                                LEFT JOIN business b WITH ( NOLOCK ) ON b.Id = o.businessId
                                LEFT JOIN [group] g WITH ( NOLOCK ) ON g.Id = b.GroupId ";
            string orderByColumn = " o.Status ASC,o.Id DESC ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PagingRequest.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PagingRequest.PageSize, true);
        }
        /// <summary>
        /// 更新订单佣金
        /// danny-20150320
        /// </summary>
        /// <param name="order"></param>
        public bool UpdateOrderInfo(order order)
        {
            bool reslut = false;
            try
            {
                string sql = @" update [order] set OrderCommission=@OrderCommission where OrderNo=@OrderNo "; 
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("@OrderNo", order.OrderNo);
                dbParameters.AddWithValue("@OrderCommission", order.OrderCommission);
                int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
                if (i > 0) reslut = true;
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriter(ex, "更新审核状态");
                throw;
            }
            return reslut;
        }

        /// <summary>
        /// 根据订单号查订单信息
        /// danny-20150320
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public OrderListModel GetOrderByNo(string orderNo)
        {
            string sql = @"SELECT top 1 o.[Id]
                                        ,o.[OrderNo]
                                        ,o.[PickUpAddress]
                                        ,o.[PubDate]
                                        ,o.[ReceviceName]
                                        ,o.[RecevicePhoneNo]
                                        ,o.[ReceviceAddress]
                                        ,o.[ActualDoneDate]
                                        ,o.[IsPay]
                                        ,o.[Amount]
                                        ,o.[OrderCommission]
                                        ,o.[DistribSubsidy]
                                        ,o.[WebsiteSubsidy]
                                        ,o.[Remark]
                                        ,o.[Status]
                                        ,o.[clienterId]
                                        ,o.[businessId]
                                        ,o.[ReceviceCity]
                                        ,o.[ReceviceLongitude]
                                        ,o.[ReceviceLatitude]
                                        ,o.[OrderFrom]
                                        ,o.[OriginalOrderId]
                                        ,o.[OriginalOrderNo]
                                        ,o.[Quantity]
                                        ,o.[Weight]
                                        ,o.[ReceiveProvince]
                                        ,o.[ReceiveArea]
                                        ,o.[ReceiveProvinceCode]
                                        ,o.[ReceiveCityCode]
                                        ,o.[ReceiveAreaCode]
                                        ,o.[OrderType]
                                        ,o.[KM]
                                        ,o.[GuoJuQty]
                                        ,o.[LuJuQty]
                                        ,o.[SongCanDate]
                                        ,o.[OrderCount]
                                        ,o.[CommissionRate] 
                                        ,b.[City] BusinessCity
                                    FROM [order] o WITH ( NOLOCK )
                                    LEFT JOIN business b WITH ( NOLOCK ) ON b.Id = o.businessId
                                    WHERE 1=1 ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OrderNo", orderNo);
            if (!string.IsNullOrWhiteSpace(orderNo))
            {
                sql += " AND OrderNo=@OrderNo";
            }
            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
            var list = ConvertDataTableList<OrderListModel>(dt);
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 订单指派超人
        /// danny-20150320
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool RushOrder(OrderListModel order)
        {
            bool reslut = false;
            try
            {
                string sql = @" update [order] set clienterId=@clienterId,Status=@Status where OrderNo=@OrderNo ";
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("@clienterId", order.clienterId);
                dbParameters.AddWithValue("@Status", ConstValues.ORDER_ACCEPT);
                dbParameters.AddWithValue("@OrderNo", order.OrderNo);
                int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
                if (i > 0)
                {
                    reslut = true;
                }
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriter(ex, "订单指派超人");
                throw;
            }
            return reslut;
        }

        /// <summary>
        /// 根据订单 号查询 订单是否存在  
        /// wangchao 
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public int GetOrderByOrderNo(string orderNo)
        {
            string selSql = string.Format(@" SELECT COUNT(1)
 FROM   dbo.[order] WITH ( NOLOCK ) 
 WHERE  OrderNo = @orderNo");

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@orderNo", orderNo);    //订单号
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, selSql, dbParameters);

            return ParseHelper.ToInt(executeScalar, -1);

        }

        /// <summary>
        /// 根据订单号 修改订单状态 B端商家取消订单
        /// wc
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        public int CancelOrderStatus(string orderNo, int orderStatus)
        {
            string upSql = string.Format(@" UPDATE dbo.[order]
 SET    [Status] = @status
 WHERE  OrderNo = @orderNo");

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@orderNo", orderNo);    //订单号
            dbParameters.AddWithValue("@status", orderStatus);    //订单号

            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, upSql, dbParameters);

            return ParseHelper.ToInt(executeScalar, -1);
        }
    }
}
