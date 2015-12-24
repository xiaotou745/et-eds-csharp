﻿using Ets.Dao.Clienter;
using Ets.Dao.DeliveryCompany;
using Ets.Dao.WtihdrawRecords;
using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.DeliveryCompany;
using Ets.Model.DataModel.Finance;
using Ets.Model.DataModel.Order;
using Ets.Model.DataModel.Subsidy;
using Ets.Model.DomainModel.Order;
using Ets.Model.DomainModel.Statistics;
using Ets.Model.DomainModel.Subsidy;
using Ets.Model.ParameterModel.Order;
using Ets.Model.ParameterModel.WtihdrawRecords;
using ETS;
using ETS.Const;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.Generic;
using ETS.Data.PageData;
using ETS.Enums;
using ETS.Extension;
using ETS.NoSql.RedisCache;
using ETS.Page;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.Order
{
    public class OrderDao : DaoBase
    {
        readonly ClienterDao clienterDao = new ClienterDao();
        readonly WtihdrawRecordsDao withDao = new WtihdrawRecordsDao();
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
        ISNULL(o.OriginalOrderNo,'') OriginalOrderNo,
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
        b.PhoneNo2 BusinessPhone2,
        b.City PickUpCity,
        b.Longitude BusiLongitude,
        b.Latitude BusiLatitude,
        b.GroupId,
        ISNULL(oo.HadUploadCount,0) HadUploadCount ");
            //关联表
            StringBuilder tableListStr = new StringBuilder();
            tableListStr.Append(@" dbo.[order] o WITH ( NOLOCK )
        LEFT JOIN dbo.clienter c WITH ( NOLOCK ) ON c.Id = o.clienterId
        LEFT JOIN dbo.business b WITH ( NOLOCK ) ON b.Id = o.businessId 
        left join dbo.OrderOther oo (nolock) on o.Id = oo.OrderId ");
            //条件
            StringBuilder whereStr = new StringBuilder(" 1=1 and o.IsEnable=1 ");
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
            if (criteria.status != -1 && criteria.status != null)
            {
                whereStr.AppendFormat(" AND o.[Status] = {0}", criteria.status);
            }
            else
            {
                whereStr.Append(" AND o.[Status] = 0 ");  //这里改为枚举值
            }

            var pageInfo = new PageHelper().GetPages<Model.DataModel.Order.order>(SuperMan_Read, criteria.PagingRequest.PageIndex, whereStr.ToString(), orderByStr, columnStr.ToString(), tableListStr.ToString(), criteria.PagingRequest.PageSize, true);
            if (pageInfo != null && pageInfo.Records != null && pageInfo.Records.Count > 0)
            {
                orderPageList.ContentList = pageInfo.Records.ToList();
                orderPageList.CurrentPage = pageInfo.Index;  //当前页
                orderPageList.PageCount = pageInfo.PageCount;//总页数
                orderPageList.PageSize = criteria.PagingRequest.PageSize;
            }


            return orderPageList;
        }

        #region   订单状态查询功能  add by caoheyang 20150316
        /// <summary>
        /// 订单状态查询功能  add by caoheyang 20150316
        /// </summary>
        /// <param name="originalOrderNo">第三方平台订单号码</param>
        /// <param name="orderfrom">订单来源</param>
        /// <returns>订单状态</returns>
        public int GetStatus(string originalOrderNo, int orderfrom)
        {
            const string querySql = @"SELECT top 1  a.Status FROM [order] a  WITH ( NOLOCK )  
            WHERE OriginalOrderNo=@OriginalOrderNo AND a.OrderFrom=@OrderFrom";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@OriginalOrderNo", originalOrderNo);    //第三方平台订单号
            dbParameters.AddWithValue("@OrderFrom", orderfrom);    //订单来源
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, querySql, dbParameters);
            return ParseHelper.ToInt(executeScalar, -1);
        }
        #endregion

        /// <summary>
        /// 订单状态查询功能  add by caoheyang 20150316
        /// </summary>
        /// <param name="originalOrderNo">订单号码</param>
        /// <param name="orderfrom">订单来源</param>
        /// <returns>订单状态</returns>
        public OrderListModel GetOpenOrder(string originalOrderNo, int orderfrom)
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
                                        ,b.Name BusinessName
                                        ,c.PhoneNo ClienterPhoneNo
                                        ,c.TrueName ClienterName
                                        ,b.GroupId
                                        ,o.OriginalOrderNo
                                        ,o.SettleMoney
                                    FROM [order] o WITH ( NOLOCK )
                                    LEFT JOIN business b WITH ( NOLOCK ) ON b.Id = o.businessId
                                    LEFT JOIN dbo.clienter c WITH (NOLOCK) ON o.clienterId=c.Id
                                    WHERE 1=1 and o.OriginalOrderNo=@OriginalOrderNo and o.OrderFrom=@OrderFrom";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OriginalOrderNo", SqlDbType.NVarChar);
            parm.SetValue("@OriginalOrderNo", originalOrderNo);
            parm.Add("@OrderFrom", SqlDbType.Int);
            parm.SetValue("@OrderFrom", orderfrom);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count <= 0)
                return null;
            return MapRows<OrderListModel>(dt)[0];
        }

        #region  第三方对接 物流订单接收接口  add by caoheyang 201503167
        /// <summary>
        /// 第三方对接 物流订单接收接口  add by caoheyang 201503167
        /// </summary>
        /// <param name="paramodel">订单号码</param>
        /// <returns>订单号</returns>
        public int CreateToSql(CreatePM_OpenApi paramodel)
        {
            var redis = new RedisCache();
            //订单插入sql 订单不存在时
            const string insertOrdersql = @" 
                INSERT INTO dbo.[order](OrderNo,
                OriginalOrderNo,PubDate,SongCanDate,IsPay,Amount,
                Remark,Weight,DistribSubsidy,OrderCount,ReceviceName,
                RecevicePhoneNo,ReceiveProvinceCode,ReceiveCityCode,ReceiveAreaCode,ReceviceAddress,
                ReceviceLongitude,ReceviceLatitude,businessId,PickUpAddress,Payment,OrderCommission,
                WebsiteSubsidy,CommissionRate,BaseCommission,CommissionFormulaMode,ReceiveProvince,ReceviceCity,ReceiveArea,
                PickupCode,BusinessCommission,SettleMoney,Adjustment,OrderFrom,Status,CommissionType,CommissionFixValue,BusinessGroupId,Invoice,
                MealsSettleMode,BusinessReceivable)
                output Inserted.Id,GETDATE(),@OptName,'新增订单',Inserted.businessId,Inserted.[Status],@Platform
                into dbo.OrderSubsidiesLog(OrderId,InsertTime,OptName,Remark,OptId,OrderStatus,[Platform])
                Values(@OrderNo,
                @OriginalOrderNo,@PubDate,@SongCanDate,@IsPay,@Amount,
                @Remark,@Weight,@DistribSubsidy,@OrderCount,@ReceviceName,
                @RecevicePhoneNo,@ReceiveProvinceCode,@ReceiveCityCode,@ReceiveAreaCode,@ReceviceAddress,
                @ReceviceLongitude,@ReceviceLatitude,@BusinessId,@PickUpAddress,@Payment,@OrderCommission,
                @WebsiteSubsidy,@CommissionRate,@BaseCommission,@CommissionFormulaMode,@ReceiveProvince,@ReceviceCity,@ReceiveArea,
                @PickupCode,@BusinessCommission,@SettleMoney,@Adjustment,@OrderFrom,@Status,@CommissionType,@CommissionFixValue,@BusinessGroupId,@Invoice,
                @MealsSettleMode,@BusinessReceivable);
               select  IDENT_CURRENT('order') ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            //基本参数信息

            dbParameters.Add("@OrderNo", SqlDbType.NVarChar, 50).Value = paramodel.OrderNo; //订单号
            dbParameters.AddWithValue("@OriginalOrderNo", paramodel.order_id);    //其它平台的来源订单号
            dbParameters.AddWithValue("@PubDate", paramodel.create_time);    //订单下单时间
            dbParameters.AddWithValue("@SongCanDate", paramodel.receive_time);  //要求送餐时间
            dbParameters.AddWithValue("@IsPay", paramodel.is_pay);    //是否已付款
            dbParameters.AddWithValue("@Amount", paramodel.total_price);    //订单金额
            dbParameters.AddWithValue("@Remark", paramodel.remark);    //备注
            dbParameters.AddWithValue("@Weight", paramodel.weight);    //重量，默认?
            //订单外送费  目前 接收了两个外送费 理论必须一致 ，若不一致，以订单上的为准，方便后续扩展
            dbParameters.AddWithValue("@DistribSubsidy", paramodel.delivery_fee);
            dbParameters.AddWithValue("@OrderCount", paramodel.package_count ?? 1);   //订单数量，默认为1
            //收货地址信息
            dbParameters.AddWithValue("@ReceviceName", paramodel.address.user_name);    //用户姓名 收货人姓名
            dbParameters.AddWithValue("@RecevicePhoneNo", paramodel.address.user_phone);    //用户联系电话  收货人电话
            dbParameters.AddWithValue("@ReceiveProvinceCode", paramodel.address.province_code);    //用户所在省份code
            dbParameters.AddWithValue("@ReceiveCityCode", paramodel.address.city_code);    //用户所在城市code
            dbParameters.AddWithValue("@ReceiveAreaCode", paramodel.address.area_code);    //用户所在区域code
            dbParameters.AddWithValue("@ReceviceAddress", paramodel.address.address);    //用户收货地址
            dbParameters.AddWithValue("@ReceviceLongitude", paramodel.address.longitude);    //用户收货地址所在区域经度
            dbParameters.AddWithValue("@ReceviceLatitude", paramodel.address.latitude);    //用户收货地址所在区域纬度
            dbParameters.AddWithValue("@BusinessId", paramodel.businessId);    //商户id
            dbParameters.AddWithValue("@PickUpAddress", paramodel.store_info.address);    //取货地址即商户地址
            dbParameters.AddWithValue("@Payment", paramodel.payment);    //取货地址即商户地址
            dbParameters.AddWithValue("@OrderCommission", paramodel.ordercommission);    //订单骑士佣金
            dbParameters.AddWithValue("@WebsiteSubsidy", paramodel.websitesubsidy);    //网站补贴
            dbParameters.AddWithValue("@CommissionRate", paramodel.commissionrate);    //订单佣金比例
            dbParameters.AddWithValue("@BaseCommission", paramodel.basecommission);    //订单基本佣金
            dbParameters.AddWithValue("@CommissionFormulaMode", paramodel.CommissionFormulaMode); //订单佣金计算方式
            dbParameters.AddWithValue("@ReceiveProvince", paramodel.address.province);    //用户省
            dbParameters.AddWithValue("@ReceviceCity", paramodel.address.city); //用户市
            dbParameters.AddWithValue("@ReceiveArea", paramodel.address.area); //用户区
            dbParameters.AddWithValue("@PickupCode", string.IsNullOrWhiteSpace(paramodel.pickupcode) ? "" : paramodel.pickupcode); //用户区
            dbParameters.AddWithValue("@BusinessCommission", paramodel.store_info.businesscommission);//结算比例
            dbParameters.AddWithValue("@SettleMoney", paramodel.settlemoney);//结算比例
            dbParameters.AddWithValue("@Adjustment", paramodel.adjustment);//结算比例
            dbParameters.AddWithValue("@OrderFrom", paramodel.orderfrom);//订单来源
            dbParameters.AddWithValue("@Status", paramodel.status);//订单状态
            //订单操作记录表
            dbParameters.AddWithValue("@OptName", "第三方对接平台");//操作人
            dbParameters.AddWithValue("@Platform", SuperPlatform.ThirdParty.GetHashCode());//操作平台
            dbParameters.AddWithValue("@CommissionType", paramodel.CommissionType);//结算类型
            dbParameters.AddWithValue("@CommissionFixValue", paramodel.CommissionFixValue);//固定金额
            dbParameters.AddWithValue("@BusinessGroupId", paramodel.BusinessGroupId);//分组ID
            dbParameters.AddWithValue("@Invoice", paramodel.invoice_title ?? "");//发票标题
            dbParameters.AddWithValue("@MealsSettleMode", paramodel.MealsSettleMode);
            dbParameters.AddWithValue("@BusinessReceivable", paramodel.BusinessReceivable);

            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertOrdersql, dbParameters);
            return ParseHelper.ToInt(result);

        }

        /// <summary>
        /// CreateToSql 获取商户id 商户不存在则新增商户  caoheyang 20150512  拆方法
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        public int CreateToSqlAddBusiness(CreatePM_OpenApi paramodel)
        {
            var redis = new RedisCache();
            int bussinessId = 0;
            //商户插入sql
            const string insertBussinesssql = @"
                INSERT INTO dbo.business
                (OriginalBusiId,Name,GroupId,IDCard,Password,
                PhoneNo,PhoneNo2,Address,ProvinceCode,CityCode,AreaCode,
                Longitude,Latitude,DistribSubsidy,Province,City,district,CityId,districtId,
                BusinessCommission,CommissionType,CommissionFixValue,BusinessGroupId)  
                OUTPUT Inserted.Id   
                values(@OriginalBusiId,@Name,@GroupId,@IDCard,@Password,
                @PhoneNo,@PhoneNo2,@Address,@ProvinceCode,@CityCode,@AreaCode,
                @Longitude,@Latitude,@DistribSubsidy,@Province,@City,@district,@CityId,@districtId,
                @BusinessCommission,@CommissionType,@CommissionFixValue,@BusinessGroupId);";
            IDbParameters insertBdbParameters = DbHelper.CreateDbParameters();
            //基本参数信息
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
            insertBdbParameters.AddWithValue("@Province", paramodel.store_info.province);    //门店省
            insertBdbParameters.AddWithValue("@City", paramodel.store_info.city);    //门店市编码
            insertBdbParameters.AddWithValue("@district", paramodel.store_info.area);    //门店区编码
            insertBdbParameters.AddWithValue("@CityId", paramodel.store_info.city_code);    //门店市编码
            insertBdbParameters.AddWithValue("@districtId", paramodel.store_info.area_code);    //门店区编码
            insertBdbParameters.AddWithValue("@BusinessCommission", paramodel.store_info.businesscommission);//结算比例
            insertBdbParameters.AddWithValue("@CommissionType", paramodel.CommissionType);//结算类型
            insertBdbParameters.AddWithValue("@CommissionFixValue", paramodel.CommissionFixValue);//固定金额
            insertBdbParameters.AddWithValue("@BusinessGroupId", paramodel.BusinessGroupId);//分组ID
            bussinessId = ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, insertBussinesssql, insertBdbParameters));
            redis.Set(string.Format(ETS.Const.RedissCacheKey.OtherBusinessIdInfo, paramodel.store_info.group.ToString()
                , paramodel.store_info.store_id.ToString()), bussinessId.ToString());//将商户id插入到缓存  key的形式为 OtherBusiness_集团id_第三方平台店铺id
            return bussinessId;
        }

        /// <summary>
        /// CreateToSql  操作插入OrderDetail表  caoheyang 20150512  拆方法
        /// </summary>
        /// <param name="paramodel"></param>
        /// <param name="orderNo"></param>
        public void CreateToSqlAddOrderDetail(CreatePM_OpenApi paramodel, string orderNo)
        {
            for (int i = 0; i < paramodel.order_details.Length; i++)
            {
                //订单详情插入sql
                const string insertOrderDetailsql = @" 
                 INSERT INTO dbo.OrderDetail
                 (OrderNo ,ProductName , UnitPrice ,Quantity,FormDetailID,GroupID)
                 VALUES  (@OrderNo ,@ProductName ,@UnitPrice ,@Quantity,@FormDetailID,@GroupID)";
                IDbParameters insertOrderDetaiParas = DbHelper.CreateDbParameters();
                //基本参数信息
                //订单号
                insertOrderDetaiParas.Add("@OrderNo", SqlDbType.NVarChar);
                insertOrderDetaiParas.SetValue("@OrderNo", orderNo);
                insertOrderDetaiParas.AddWithValue("@ProductName", paramodel.order_details[i].product_name);    //商品名称
                insertOrderDetaiParas.AddWithValue("@UnitPrice", paramodel.order_details[i].unit_price);    //商品单价，精确到两位小数
                insertOrderDetaiParas.AddWithValue("@Quantity", paramodel.order_details[i].quantity);    //商品数量
                insertOrderDetaiParas.AddWithValue("@FormDetailID", paramodel.order_details[i].detail_id);    //第三方平台明细id,与GroupID组成联合唯一约束
                insertOrderDetaiParas.AddWithValue("@GroupID", paramodel.store_info.group);    //集团id,与第三方平台明细id组成联合唯一约束
                DbHelper.ExecuteNonQuery(SuperMan_Write, insertOrderDetailsql, insertOrderDetaiParas);
            }
        }

        /// <summary>
        /// 操作插入OrderChild表
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150604</UpdateTime>
        /// <param name="paramodel"></param>
        /// <param name="orderId"></param>
        public void CreateToSqlAddOrderChild(CreatePM_OpenApi paramodel, int orderId)
        {
            const string insertSql = @"
insert into OrderChild(OrderId,ChildId,TotalPrice,GoodPrice, DeliveryPrice,CreateBy,
        UpdateBy, PayStatus)
values( @OrderId,  @ChildId,@TotalPrice,@GoodPrice,@DeliveryPrice,@CreateBy,
        @UpdateBy, @PayStatus)";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@OrderId", orderId);
            dbParameters.AddWithValue("@ChildId", 1);
            decimal totalPrice = paramodel.total_price + Convert.ToDecimal(paramodel.delivery_fee);
            dbParameters.AddWithValue("@TotalPrice", totalPrice);
            dbParameters.AddWithValue("@GoodPrice", paramodel.total_price);//订单金额
            dbParameters.AddWithValue("@DeliveryPrice", paramodel.delivery_fee);//外送费
            dbParameters.AddWithValue("@CreateBy", "第三方对接平台");
            dbParameters.AddWithValue("@UpdateBy", "第三方对接平台");
            if ((bool)paramodel.is_pay ||
                           (!(bool)paramodel.is_pay && paramodel.MealsSettleMode == MealsSettleMode.LineOff.GetHashCode())
                           )//已付款 未付款线下付款
                dbParameters.AddWithValue("@PayStatus", "1");
            else
                dbParameters.AddWithValue("@PayStatus", "0");

            DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
        }

        #endregion

        /// <summary>
        /// CreateToSql  操作插入 OrderChild 表  caoheyang        
        /// </summary>
        /// <param name="businessId">商家id</param>
        /// <param name="orderId">E代送平台内的订单id</param>
        /// <returns></returns>
        public int CreateToSqlAddOrderOther(int businessId, int orderId)
        {
            const string insertOtherSql = @"
insert into OrderOther(OrderId,NeedUploadCount,HadUploadCount,PubLongitude,PubLatitude,IsAllowCashPay)
select @OrderId,1,0,b.Longitude,b.Latitude,b.IsAllowCashPay 
from dbo.business as b where b.Id=@BusinessId
select @@IDENTITY ";
            IDbParameters dbOtherParameters = DbHelper.CreateDbParameters();
            dbOtherParameters.AddWithValue("@OrderId", orderId); //商户ID
            dbOtherParameters.AddWithValue("@BusinessId", businessId);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, insertOtherSql, dbOtherParameters));
        }


        /// <summary>
        ///获取当天
        ///订单金额
        ///任务量
        ///订单量 
        ///商户结算金额（应收）;
        ///骑士佣金总计（应付）
        /// 窦海超
        /// 2015年3月18日 17:23:14
        /// </summary>
        public IList<HomeCountTitleModel> GetCurrentDateCountAndMoney(string StartTime, string EndTime)
        {
            if (string.IsNullOrEmpty(StartTime) || string.IsNullOrEmpty(EndTime))
            {
                return null;
            }
            string sql = @"SELECT 
                         CONVERT(CHAR(10),PubDate,120) AS PubDate, --
                        --sum(case when o.status=1 then amount else 0 end ) as OrderPrice, --订单金额
                        SUM(ISNULL(Amount,0)) AS OrderPrice, --订单金额
                        ISNULL(COUNT(o.Id),0) AS MisstionCount,--总任务量
                        SUM(ISNULL(OrderCount,0)) AS OrderCount,--总订单量
                         ISNULL(SUM(o.Amount*ISNULL(b.BusinessCommission,0)/100+ ISNULL( b.DistribSubsidy ,0)* o.OrderCount),0) AS YsPrice,  -- 应收金额
                          ISNULL( SUM( OrderCommission),0) AS YfPrice  --应付金额
                        FROM dbo.[order](NOLOCK) AS o
                        LEFT JOIN dbo.business(NOLOCK) AS b ON o.businessId=b.Id
                         WHERE  
                        o.[Status]<>3 AND 
                        CONVERT(CHAR(10),PubDate,120)>=CONVERT(CHAR(10),@StartTime,120) and 
                        CONVERT(CHAR(10),PubDate,120)<=CONVERT(CHAR(10),@EndTime,120)
                        GROUP BY CONVERT(CHAR(10),PubDate,120)
                        ORDER BY PubDate DESC
                        ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@StartTime", StartTime);
            parm.AddWithValue("@EndTime", EndTime);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<HomeCountTitleModel>(dt);
        }

        /// <summary>
        ///按照补贴次数分组获取当天补贴的订单量
        /// danny-20150407
        /// </summary>
        public IList<HomeCountTitleModel> GetCurrentDateSubsidyOrderCount(string StartTime, string EndTime)
        {
            if (string.IsNullOrEmpty(StartTime) || string.IsNullOrEmpty(EndTime))
            {
                return null;
            }
            string sql = @"SELECT 
                        CONVERT(CHAR(10),PubDate,120) AS PubDate, --发布时间
                        --SUM(ISNULL(OrderCount,0)) AS OrderCount,--订单量
                        COUNT(1) AS OrderCount,--订单量
                        DealCount
                        FROM dbo.[order](NOLOCK) AS o
                        WHERE  
                        o.[Status]<>3 AND 
                        CONVERT(CHAR(10),PubDate,120)>='" + StartTime + "' and ";
            sql += "CONVERT(CHAR(10),PubDate,120)<='" + EndTime + "'";
            sql += "  GROUP BY CONVERT(CHAR(10),PubDate,120),DealCount ORDER BY DealCount ASC ";
            //IDbParameters parm = DbHelper.CreateDbParameters();
            //parm.AddWithValue("@StartTime", StartTime);
            //parm.AddWithValue("@EndTime", EndTime);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            return MapRows<HomeCountTitleModel>(dt);
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
                                    ,isnull(o.[PubDate],'') as PubDate
                                    ,o.[ReceviceName]
                                    ,o.[RecevicePhoneNo]
                                    ,o.[ReceviceAddress]
                                    ,o.[ActualDoneDate]
                                    ,isnull(o.[IsPay],0) as IsPay
                                    ,isnull(o.[Amount],0) as Amount
                                    ,isnull(o.[OrderCommission],0) as OrderCommission
                                    ,o.[RealOrderCommission]
                                    ,isnull(o.[DistribSubsidy],0) as DistribSubsidy
                                    ,isnull(o.[WebsiteSubsidy],0) as WebsiteSubsidy
                                    ,o.[Remark]
                                    ,isnull(o.[Status],0) as Status
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
                                    ,isnull(g.GroupName,'') as  GroupName
                                    ,isnull(g.GroupName,'') as OrderFromName  
                                    ,o.[Adjustment]
                                    ,ISNULL(oo.HadUploadCount,0) HadUploadCount
                                    ,CASE ISNULL(oo.GrabLatitude, 0)
                                          WHEN 0 THEN -1
                                          ELSE CASE ISNULL(oo.CompleteLatitude, 0)
                                                 WHEN 0 THEN -1
                                                 ELSE oo.GrabToCompleteDistance
                                               END
                                        END AS GrabToCompleteDistance
                                    ,o.BusinessCommission --商家结算比例
                                    ,o.CommissionFixValue --商家结算固定金额
                                    ,o.CommissionType --结算类型
                                    ,o.SettleMoney
                                    ,o.CommissionFormulaMode
                                    ,o.BaseCommission
                                    ,oo.IsNotRealOrder
                                    ,oo.AuditStatus
                                    ,oo.DeductCommissionReason
                                    ,o.IsEnable
                                    ,o.FinishAll
                                    ,ISNULL(oo.IsJoinWithdraw,0) IsJoinWithdraw
                                    ";
            var sbSqlWhere = new StringBuilder(" 1=1 and o.Platform<=3"); // and o.Platform<=2 老订单列表不显示闪送订单
            if (!string.IsNullOrWhiteSpace(criteria.businessName))
            {
                sbSqlWhere.AppendFormat(" AND b.Name='{0}' ", criteria.businessName);
            }
            if (!string.IsNullOrWhiteSpace(criteria.businessPhone))
            {
                sbSqlWhere.AppendFormat(" AND b.PhoneNo='{0}' ", criteria.businessPhone.Trim());
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
            if (criteria.IsNotRealOrder != -1)
            {
                sbSqlWhere.AppendFormat(" AND oo.IsNotRealOrder={0} ", criteria.IsNotRealOrder);
            }
            if (criteria.AuditStatus != -1)
            {
                if (criteria.AuditStatus == 0)
                {
                    sbSqlWhere.AppendFormat(" AND oo.AuditStatus={0}  and o.FinishAll=1", criteria.AuditStatus);
                }
                else
                {
                    sbSqlWhere.AppendFormat(" AND oo.AuditStatus={0} ", criteria.AuditStatus);
                }
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
                //sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),o.PubDate,120)>=CONVERT(CHAR(10),'{0}',120) ", criteria.orderPubStart.Trim());
                sbSqlWhere.AppendFormat(" AND o.PubDate>='{0}' ", ParseHelper.ToDatetime(criteria.orderPubStart.Trim(), DateTime.Now).ToString());
            }
            if (!string.IsNullOrWhiteSpace(criteria.orderPubEnd))
            {
                //sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),o.PubDate,120)<=CONVERT(CHAR(10),'{0}',120) ", criteria.orderPubEnd.Trim());
                sbSqlWhere.AppendFormat(" AND o.PubDate<='{0}' ", ParseHelper.ToDatetime(criteria.orderPubEnd.Trim(), DateTime.Now).AddDays(1).ToString());
            }
            if (criteria.GroupId != null && criteria.GroupId != 0)
            {
                sbSqlWhere.AppendFormat(" AND o.OrderFrom={0} ", criteria.GroupId);
            }
            if (!string.IsNullOrWhiteSpace(criteria.businessCity))
            {
                sbSqlWhere.AppendFormat(" AND b.City='{0}' ", criteria.businessCity.Trim());
            }
            if (criteria.AuthorityCityNameListStr != null && !string.IsNullOrEmpty(criteria.AuthorityCityNameListStr.Trim()) && criteria.UserType != 0)
            {
                sbSqlWhere.AppendFormat(" AND b.City IN({0}) ", criteria.AuthorityCityNameListStr.Trim());
            }
            if (criteria.CaiWuAudit == 1)
            {
                sbSqlWhere.AppendFormat(" and o.FinishAll = 1 and oo.IsJoinWithdraw =0 and o.IsEnable = 1 and o.[Status] <> {0} ", (byte)OrderStatusCommon.Cancel);
            }
            if (criteria.GroupBusinessId > 0)
            {
                sbSqlWhere.AppendFormat(" and  gbr.GroupId={0}", criteria.GroupBusinessId);
            }

            string tableList = @" [order] o WITH ( NOLOCK )
                                LEFT JOIN clienter c WITH ( NOLOCK ) ON c.Id = o.clienterId
                                JOIN business b WITH ( NOLOCK ) ON b.Id = o.businessId
                                LEFT JOIN [group] g WITH ( NOLOCK ) ON g.id = o.OrderFrom
                                JOIN dbo.OrderOther oo (nolock) ON o.Id = oo.OrderId ";
            if (criteria.GroupBusinessId > 0)
            {
                tableList += " left join GroupBusinessRelation gbr(nolock) on o.BusinessId=b.id";
            }
            if (criteria.TagId != null)
            {
                if (criteria.TagType == TagUserType.Business.GetHashCode())
                {
                    tableList = tableList + string.Format("join dbo.TagRelation tagR on b.Id=tagR.UserId and tagR.IsEnable=1 and tagR.UserType=0 and  tagR.TagId={0}", criteria.TagId);
                }
                else if (criteria.TagType == TagUserType.Clienter.GetHashCode())
                {
                    tableList = tableList + string.Format("join dbo.TagRelation tagR on c.Id=tagR.UserId and tagR.IsEnable=1 and tagR.UserType=1  and  tagR.TagId={0}", criteria.TagId);
                }

            }
            string orderByColumn = " o.Status ASC,o.Id DESC ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
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
                dbParameters.Add("@OrderNo", SqlDbType.NVarChar);
                dbParameters.SetValue("@OrderNo", order.OrderNo);
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
        /// 修改订单数量
        /// </summary>
        /// <param name="order"></param>
        public void UpdateOrderCount(order order)
        {
            const string updateSql = @"
update  [order]
set  OrderCount=@OrderCount
where  Id=@Id ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", order.Id);
            dbParameters.AddWithValue("OrderCount", order.OrderCount);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

        /// <summary>
        /// 根据订单号查订单信息
        /// danny-20150320
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public OrderListModel GetOrderByNo(string orderNo)
        {
            #region 查询脚本
            string sql = @"SELECT top 1 o.[Id]
                                        ,o.[OrderNo]
                                        ,o.[PickUpAddress]
                                        ,o.PubDate
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
                                        ,o.[RealOrderCommission] 
                                        ,b.[City] BusinessCity
                                        ,b.Name BusinessName
                                        ,b.PhoneNo BusinessPhoneNo
                                        ,b.PhoneNo2 BusinessPhoneNo2
                                        ,b.Address BusinessAddress
                                        ,c.PhoneNo ClienterPhoneNo
                                        ,c.TrueName ClienterTrueName
                                        ,c.TrueName ClienterName
                                        ,c.AccountBalance AccountBalance
                                        ,b.GroupId
                                        ,isnull(g.GroupName,'') as  GroupName
                                        ,isnull(g.GroupName,'') as OrderFromName  
                                        ,o.OriginalOrderNo
                                        ,oo.NeedUploadCount
                                        ,oo.HadUploadCount
                                        ,oo.ReceiptPic
                                        ,oo.IsNotRealOrder IsNotRealOrder
                                        ,o.OtherCancelReason
                                        ,o.OriginalOrderNo
                                        ,ISNULL(o.MealsSettleMode,0) MealsSettleMode
                                        ,ISNULL(oo.IsJoinWithdraw,0) IsJoinWithdraw
                                        ,o.BusinessReceivable
                                        ,o.SettleMoney
                                        ,o.FinishAll
                                        ,oo.ExpectedTakeTime 
                                    FROM [order] o WITH ( NOLOCK )
                                    JOIN business b WITH ( NOLOCK ) ON b.Id = o.businessId
                                    left JOIN clienter c WITH (NOLOCK) ON o.clienterId=c.Id
                                    JOIN OrderOther oo WITH (NOLOCK) ON oo.OrderId=o.Id
                                    LEFT JOIN [group] g WITH ( NOLOCK ) ON g.Id = o.orderfrom
                                    WHERE 1=1 ";
            #endregion
            IDbParameters parm = DbHelper.CreateDbParameters();
            if (!string.IsNullOrWhiteSpace(orderNo))
            {
                sql += " AND o.OrderNo=@OrderNo";
                parm.Add("@OrderNo", SqlDbType.NVarChar);
                parm.SetValue("@OrderNo", orderNo);
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

        public OrderListModel GetOrderByIdWithNolock(int id)
        {
            #region 查询脚本
            string sql = @"SELECT top 1 o.[Id]
                                        ,o.[OrderNo]
                                        ,o.[PickUpAddress]
                                        ,o.PubDate
                                        ,o.[ReceviceName]
                                        ,o.[RecevicePhoneNo]
                                        ,o.[ReceviceAddress]
                                        ,o.[ActualDoneDate]
                                        ,o.[IsPay]
                                        ,o.[Amount]                                        
                                        ,isnull(o.[Amount],0)+isnull(o.[TipAmount],0) as AmountAndTip     
                                        ,ISNULL(o.OrderCommission,0) OrderCommission
                                        ,ISNULL(o.DistribSubsidy,0) DistribSubsidy
                                        ,ISNULL(o.WebsiteSubsidy,0) WebsiteSubsidy                                   
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
                                        ,o.[RealOrderCommission] 
                                        ,isnull(o.GroupBusinessId,0) as GroupBusinessId
                                        ,b.[City] BusinessCity
                                        ,b.Name BusinessName
                                        ,b.PhoneNo BusinessPhoneNo
                                        ,b.PhoneNo2 BusinessPhoneNo2
                                        ,b.Address BusinessAddress
                                        ,c.PhoneNo ClienterPhoneNo
                                        ,c.TrueName ClienterTrueName
                                        ,c.TrueName ClienterName
                                        ,c.AccountBalance AccountBalance
                                        ,b.GroupId                                        
                                        ,o.OriginalOrderNo
                                        ,oo.NeedUploadCount
                                        ,oo.HadUploadCount
                                        ,oo.ReceiptPic
                                        ,oo.IsNotRealOrder IsNotRealOrder
                                        ,o.OtherCancelReason
                                        ,o.OriginalOrderNo
                                        ,ISNULL(o.MealsSettleMode,0) MealsSettleMode
                                        ,ISNULL(oo.IsJoinWithdraw,0) IsJoinWithdraw
                                        ,o.BusinessReceivable
                                        ,o.SettleMoney
                                        ,o.FinishAll
                                        ,o.Platform
                                        ,o.Payment
                                    FROM [order] o 
                                    JOIN business b   ON b.Id = o.businessId
                                    left JOIN clienter c  ON o.clienterId=c.Id
                                    JOIN OrderOther oo  ON oo.OrderId=o.Id                                   
                                    WHERE 1=1 ";
            #endregion
            IDbParameters parm = DbHelper.CreateDbParameters();
            if (id > 0)
            {
                sql += " AND o.id=@id";
                parm.Add("@id", SqlDbType.Int);
                parm.SetValue("@id", id);
            }
            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Write, sql, parm));
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
        /// 根据订单号查订单信息
        /// danny-20150320
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public OrderListModel GetOrderByNo(string orderNo, int orderId)
        {
            if (string.IsNullOrWhiteSpace(orderNo))
            {
                return new OrderListModel();
            }
            #region 查询脚本
            //获取订单主单信息以及附带的商户信息、骑士信息、小票信息
            StringBuilder sql = new StringBuilder(@"SELECT top 1 o.[Id]
                                        ,o.[OrderNo]
                                        ,o.[PickUpAddress]
                                        ,o.PubDate
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
                                        ,b.Name BusinessName
                                        ,b.PhoneNo BusinessPhoneNo
                                        ,b.Address BusinessAddress
                                        ,c.PhoneNo ClienterPhoneNo
                                        ,c.TrueName ClienterTrueName
                                        ,c.TrueName ClienterName
                                        ,c.AccountBalance AccountBalance                                      
                                        ,b.GroupId
                                        ,isnull(g.GroupName,'') as  GroupName
                                        ,isnull(g.GroupName,'') as OrderFromName  
                                        ,o.OriginalOrderNo
                                        ,oo.NeedUploadCount
                                        ,oo.HadUploadCount
                                        ,oo.ReceiptPic
                                        ,oo.DeductCommissionReason                                        
                                        ,o.OtherCancelReason
                                        ,o.OriginalOrderNo
                                        ,o.IsEnable
                                        ,o.FinishAll
                                        ,ISNULL(oo.DeductCommissionType,0) DeductCommissionType
                                        ,ISNULL(oo.IsJoinWithdraw,0) IsJoinWithdraw
                                        ,ISNULL(oo.IsOrderChecked,1) IsOrderChecked,o.MealsSettleMode 
                                    FROM [order] o WITH ( NOLOCK )
                                    LEFT JOIN business b WITH ( NOLOCK ) ON b.Id = o.businessId
                                    LEFT JOIN clienter c WITH (NOLOCK) ON o.clienterId=c.Id
                                    LEFT JOIN OrderOther oo WITH (NOLOCK) ON oo.OrderId=o.Id
                                    LEFT JOIN [group] g WITH ( NOLOCK ) ON g.Id = o.orderfrom
                                    WHERE 1=1 AND o.OrderNo=@OrderNo;");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OrderNo", SqlDbType.NVarChar).Value = orderNo;
            parm.Add("@OrderId", SqlDbType.Int).Value = orderId;
            //获取订单详情信息
            sql.Append(@"
select  od.Id ,
        od.OrderNo ,
        od.ProductName ,
        od.UnitPrice ,
        od.Quantity ,
        od.InsertTime ,
        od.FormDetailID ,
        od.GroupID
from    dbo.OrderDetail od ( nolock )
where   od.OrderNo = @OrderNo;");
            //获取子订单信息
            sql.Append(@"
select  oc.Id ,
        oc.OrderId ,
        oc.ChildId ,
        oc.TotalPrice ,
        oc.GoodPrice ,
        oc.DeliveryPrice ,
        oc.PayStyle ,
        oc.PayType ,
        oc.PayStatus ,
        oc.PayBy ,
        oc.PayTime ,
        oc.PayPrice ,
        oc.HasUploadTicket ,
        oc.TicketUrl ,
        oc.CreateBy ,
        oc.CreateTime ,
        oc.UpdateBy ,
        oc.UpdateTime
from    dbo.OrderChild oc ( nolock )
where   oc.OrderId = @OrderId;
");
            #endregion
            var ds = DbHelper.ExecuteDataset(SuperMan_Read, sql.ToString(), parm);

            var order = ConvertDataTableList<OrderListModel>(ds.Tables[0]);
            if (order != null && order.Count > 0)
            {
                var orderDetailList = ConvertDataTableList<Ets.Model.DataModel.Order.OrderDetail>(ds.Tables[1]);
                var orderChildList = ConvertDataTableList<OrderChild>(ds.Tables[2]);

                order[0].OrderDetailList = orderDetailList.ToList();
                order[0].OrderChildList = orderChildList.ToList();
                return order[0];
            }
            else
            {
                return new OrderListModel();
            }
        }

        /// <summary>
        /// 根据任务号判断该任务是否可以完成
        /// wc 获取该任务下的子订单是否全部付款
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool IsOrNotFinish(int orderId)
        {
            StringBuilder sql = new StringBuilder(@" 
select min(convert(int, oc.PayStatus)) IsPay
from   dbo.[order] o ( nolock )
join dbo.OrderChild oc ( nolock ) on o.Id = oc.OrderId
where  o.Id = @orderId");
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("orderId", DbType.Int32, 4).Value = orderId;
            int isPay = ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, sql.ToString(), dbParameters), 0);
            if (isPay == 0)  //最小是0 说明还有未付款的子订单
            {
                return false;  //表示不能完成任务
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 通过订单号获取该订单的详情数据
        /// 窦海超
        /// 2015年5月6日 20:55:54
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        public OrderListModel GetOrderDetailByOrderNo(string orderNo)
        {
            string sql = "SELECT businessId,Status FROM dbo.[order](nolock) o where OrderNo=@OrderNo ";
            IDbParameters parm = DbHelper.CreateDbParameters("OrderNo", DbType.String, 45, orderNo);

            return DbHelper.QueryForObjectDelegate<OrderListModel>(SuperMan_Read, sql, parm,
                dataRow => new OrderListModel
                {
                    businessId = ParseHelper.ToInt(dataRow["businessId"], 0),
                    Status = (byte)ParseHelper.ToInt(dataRow["Status"], 0)
                });
            //DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            //if (!dt.HasData())
            //{
            //    return null;
            //}
            //return MapRows<OrderListModel>(dt)[0];
        }

        /// <summary>
        /// 订单指派超人
        /// danny-20150320
        /// 修改
        /// 窦海超 
        /// 2015年8月13日 13:11:35
        /// 把抢单规则变更，抢单时要把该订单佣金变更，且所属订单也根据该骑士所属物流公司变更
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool RushOrder(OrderListModel order)
        {
            //            string sqlText = @"
            //            update dbo.[order]
            //            set clienterId=@clienterId,Status=@Status
            //            
            //            where OrderNo=@OrderNo and Status=0
            //            if(@@error<>0 or @@ROWCOUNT=0)
            //            begin
            //	            select 1 --更改状态失败
            //	            return
            //            end
            //            insert  into dbo.OrderSubsidiesLog ( OrderId, Price, InsertTime, OptName,
            //                                                 Remark, OptId, OrderStatus, Platform )
            //            select  o.Id, o.OrderCommission, getdate(), @OptName, '任务已被抢', @clienterId, @Status, @Platform
            //            from    dbo.[order] o ( nolock )
            //            where   o.OrderNo = @OrderNo
            //            select 0";


            string sqlText = @"
declare 
@orderamount decimal(18,3),
@ordercommission decimal(18,3),
@ordercount int 

select @orderamount=o.Amount,@ordercommission=o.OrderCommission,@ordercount=o.OrderCount from [order] o(nolock) where o.OrderNo=@OrderNo and Status=0

if(@DeliveryCompanyID>0)
begin
select @ordercommission=case when dc.SettleType=1 then @orderamount*dc.ClienterSettleRatio/100 when dc.SettleType=2 then dc.ClienterFixMoney*@ordercount end
from DeliveryCompany dc(nolock) 
where dc.Id=@DeliveryCompanyID
end
update [order] set Status=@Status,clienterId=@clienterId,OrderCommission=@ordercommission,DeliveryCompanyID=@DeliveryCompanyID where OrderNo=@OrderNo and status=0 and clienterId is null
if(@@error<>0 or @@ROWCOUNT=0)
begin
	select 1 --更改状态失败
	return
end
insert  into dbo.OrderSubsidiesLog ( OrderId, Price, InsertTime, OptName,
                                        Remark, OptId, OrderStatus, Platform )
select  o.Id, o.OrderCommission, getdate(), @OptName, '任务已被抢', @clienterId, @Status, @Platform
from    dbo.[order] o ( nolock )
where   o.OrderNo = @OrderNo
select 0
";
            //*@ordercount
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("clienterId", DbType.Int32, 4).Value = order.clienterId;// userId;
            dbParameters.Add("Status", DbType.Int32, 4).Value = OrderStatus.Status2.GetHashCode();
            dbParameters.Add("OrderNo", DbType.String, 50).Value = order.OrderNo;
            dbParameters.Add("Platform", DbType.Int32, 4).Value = SuperPlatform.FromClienter.GetHashCode();
            dbParameters.Add("OptName", DbType.String, 200).Value = order.ClienterTrueName;  //抢单的骑士姓名
            dbParameters.Add("DeliveryCompanyID", DbType.Int32, 4).Value = order.DeliveryCompanyID;//物流公司ID
            object obj = DbHelper.ExecuteScalar(SuperMan_Write, sqlText, dbParameters);
            return ParseHelper.ToInt(obj, 1) == 0 ? true : false;
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
            //订单号
            dbParameters.Add("@orderNo", SqlDbType.NVarChar);
            dbParameters.SetValue("@orderNo", orderNo);
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, selSql, dbParameters);
            return ParseHelper.ToInt(executeScalar, 0);
        }



        /// <summary>
        /// 根据订单号 修改订单状态 B端商家取消订单
        /// wc
        /// 取消订单的时候增加 日志
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="remark">订单号</param>
        /// <param name="status">原始订单状态</param>
        ///  <param name="price">涉及金额</param>
        /// <returns></returns>
        public int CancelOrderStatus(string orderNo, int orderStatus, string remark, int? status, decimal price = 0)
        {
            string upSql = string.Format(@" UPDATE dbo.[order]
 SET  [Status] = @status,OtherCancelReason=@OtherCancelReason
 output Inserted.Id,@Price,GETDATE(),'{0}',@OtherCancelReason,Inserted.businessId,Inserted.[Status],{1}
 into dbo.OrderSubsidiesLog(OrderId,Price,InsertTime,OptName,Remark,OptId,OrderStatus,[Platform])
 WHERE  OrderNo = @orderNo", SuperPlatform.FromBusiness, (int)SuperPlatform.FromBusiness);

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("orderNo", DbType.String, 45).Value = orderNo;
            dbParameters.Add("status", DbType.Int32, 4).Value = orderStatus;
            dbParameters.Add("Price", DbType.Decimal, 9).Value = price;
            dbParameters.Add("OtherCancelReason", DbType.String, 500).Value = remark;

            if (status != null)
            {
                upSql = upSql + " and Status=" + status;
            }

            object executeScalar = DbHelper.ExecuteNonQuery(SuperMan_Write, upSql.ToString(), dbParameters);
            return ParseHelper.ToInt(executeScalar, -1);
        }

        /// <summary>
        /// 根据订单号 修改订单状态 B端商家取消订单
        /// wc
        /// 取消订单的时候增加 日志
        /// </summary> 
        /// <returns></returns>
        public int CancelOrderStatus(CancelOrderModel com)
        {
            string upSql = string.Format(@" UPDATE dbo.[order]
 SET  [Status] = @status,OtherCancelReason=@OtherCancelReason
 output Inserted.Id,@Price,GETDATE(),'{0}',@OtherCancelReason,Inserted.businessId,Inserted.[Status],{1}
 into dbo.OrderSubsidiesLog(OrderId,Price,InsertTime,OptName,Remark,OptId,OrderStatus,[Platform])
 WHERE  OrderNo = @orderNo", com.OrderCancelName, com.OrderCancelFrom);

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("orderNo", DbType.String, 45).Value = com.OrderNo;
            dbParameters.Add("status", DbType.Int32, 4).Value = com.OrderStatus;
            dbParameters.Add("Price", DbType.Decimal, 9).Value = com.Price;
            dbParameters.Add("OtherCancelReason", DbType.String, 500).Value = com.Remark;

            if (com.Status != null)
            {
                upSql = upSql + " and Status=" + com.Status;
            }

            object executeScalar = DbHelper.ExecuteNonQuery(SuperMan_Write, upSql.ToString(), dbParameters);
            return ParseHelper.ToInt(executeScalar, -1);
        }


        /// <summary>
        ///  第三方更新E代送订单状态   add by caoheyang 20150421  
        /// </summary>
        /// <param name="paramodel">参数</param>
        /// <returns></returns>
        public int UpdateOrderStatus_Other(ChangeStatusPM_OpenApi paramodel)
        {
            string sql = string.Format(@"
update  a
set     a.[Status] =@Status 
output  Inserted.Id ,
        GETDATE() ,
        '{0}' ,
        @Remark ,
        Inserted.businessId ,
        Inserted.[Status] ,
        {1}
        into dbo.OrderSubsidiesLog ( OrderId, InsertTime, OptName, Remark,
                                     OptId, OrderStatus, [Platform] )
from    dbo.[order] as a
where   a.OriginalOrderNo = @OriginalOrderNo
        and a.OrderFrom=@OrderFrom
", SuperPlatform.FromBusiness, (int)SuperPlatform.FromBusiness);
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@Status", SqlDbType.Int);
            dbParameters.SetValue("@Status", paramodel.status);
            dbParameters.Add("@Remark", SqlDbType.NVarChar);
            dbParameters.SetValue("@Remark", paramodel.remark);
            dbParameters.Add("@OriginalOrderNo", SqlDbType.NVarChar);
            dbParameters.SetValue("@OriginalOrderNo", paramodel.order_no);  //第三方平台订单号 
            dbParameters.Add("@OrderFrom", SqlDbType.Int);
            dbParameters.SetValue("@OrderFrom", paramodel.orderfrom);  //集团ID
            object executeScalar = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
            return ParseHelper.ToInt(executeScalar, -1);
        }


        /// <summary>
        /// 完成订单
        /// wc
        /// 窦海超更改
        /// 2015年7月1日 11:59:28
        /// 2015年12月22日15:00:24
        /// 茹化肖修改闪送模式判断
        /// </summary>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        public int FinishOrderStatus(OrderListModel myOrderInfo)
        {
            //更新订单状态
            StringBuilder upSql = new StringBuilder();
            upSql.Append(@"UPDATE dbo.[order]
                           SET [Status] = @status,");
            //if (myOrderInfo.Platform == 3)//闪送模式直接将FinishAll 更新为1
            //{
            //    upSql.Append("FinishAll=1,");
            //}
            upSql.Append(@"ActualDoneDate=getdate()
output Inserted.Id,GETDATE(),'{0}','任务已完成',Inserted.clienterId,Inserted.[Status],{1}
into dbo.OrderSubsidiesLog(OrderId,InsertTime,OptName,Remark,OptId,OrderStatus,[Platform]) 
WHERE  dbo.[order].Id = @orderId AND clienterId =@clienterId and Status = 4;");
            string upsql = string.Format(upSql.ToString(), myOrderInfo.ClienterName, (int)SuperPlatform.FromClienter);
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("orderId", DbType.Int32, 4).Value = myOrderInfo.Id;
            dbParameters.Add("clienterId", DbType.Int32, 4).Value = myOrderInfo.clienterId;
            dbParameters.Add("status", DbType.Int32, 4).Value = OrderStatus.Status1.GetHashCode();

            return ParseHelper.ToInt(
                DbHelper.ExecuteNonQuery(SuperMan_Write, upsql, dbParameters),
                -1
                );
        }

        /// <summary>
        /// 获取总统计数据
        /// 窦海超
        /// 2015年3月25日 15:33:00
        /// </summary>
        /// <returns></returns>
        public HomeCountTitleModel GetHomeCountTitleToAllDataSql()
        {
            string sql = @"
declare 
@incomeTotal decimal(18,2),
@withdrawClienterPrice decimal(18,2),
@businessBalance decimal(18,2),
@withdrawBusinessPrice decimal(18,2),
@rechargeTotal decimal(18,2),
@SystemRecharge decimal(18,2),
@SystemPresented decimal(18,2),
@ClientRecharge decimal(18,2) 
set @incomeTotal =convert(decimal(18,2),(select sum(TotalPrice) from dbo.OrderChild oc(nolock) where ThirdPayStatus =1))
set @withdrawClienterPrice = convert(decimal(18,2),(select isnull( sum(isnull(Amount,0)),0) withPirce FROM dbo.ClienterWithdrawForm(nolock) cwf where Status =3)) 
set @businessBalance=convert(decimal(18,2),(SELECT sum(BalancePrice) FROM dbo.business b(nolock) where Status=1 ))
set @withdrawBusinessPrice=convert( decimal(18,2),(SELECT sum(Amount) as withdrawBusinessPrice FROM dbo.BusinessWithdrawForm(nolock) bwf where Status =3 ))
set @rechargeTotal = (SELECT sum(bbr.payAmount) FROM dbo.BusinessRecharge(nolock) bbr where bbr.PayType in(1,2,3,4)  ) --商户充值总计
set @SystemRecharge = (SELECT sum(isnull(bbr.payAmount,0)) FROM dbo.BusinessRecharge(nolock) bbr where bbr.PayType =3) 
set @SystemPresented = (SELECT sum(isnull(bbr.payAmount,0)) FROM dbo.BusinessRecharge(nolock) bbr where bbr.PayType =4) 
set @ClientRecharge = (SELECT sum(isnull(bbr.payAmount,0)) FROM dbo.BusinessRecharge(nolock) bbr where bbr.PayType in(1,2)) 
select  ( select    sum(AccountBalance)
          from      dbo.clienter(nolock)
          where     AccountBalance >= 1000
        ) as WithdrawPrice,--提现金额
        ( select    convert(decimal(18, 2), sum(Amount))
          from      CrossShopLog(nolock)
        ) as CrossShopPrice,--跨店奖励总金额
        sum(isnull(Amount, 0)) as OrderPrice, --订单金额
        count(1) as MisstionCount,--总任务量
        sum(isnull(OrderCount, 0)) as OrderCount,--总订单量
        sum(isnull(SettleMoney, 0)) as YsPrice, --应收金额
        sum(isnull(OrderCommission, 0)) as YfPrice,  --应付金额
        @incomeTotal incomeTotal, --扫码/代付总计
		isnull(@rechargeTotal,0) rechargeTotal,--商户充值总计
        @SystemRecharge SystemRecharge,
        @SystemPresented SystemPresented,
        @ClientRecharge ClientRecharge,
		(@incomeTotal+@rechargeTotal) allIncomeTotal, --账户收入总计
		-2348288.69-(@withdrawClienterPrice) withdrawClienterPrice, --骑士已提现佣金-实付
		@businessBalance businessBalance,--商家余额总计-应付
		isnull( @withdrawBusinessPrice,0) withdrawBusinessPrice --商家已提款金额-实付
from    dbo.[order] (nolock) as o
        join dbo.business (nolock) as b on o.businessId = b.Id
where   o.[Status] <> 3 

";//不等于未完成的订单 
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            return MapRows<HomeCountTitleModel>(dt)[0];
        }


        /// <summary>
        /// 订单统计
        /// danny-20150326
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetOrderCount<T>(HomeCountCriteria criteria)
        {
            var sbtbl = new StringBuilder(@" (select    b.district
                                    ,sum(OrderCount) orderCount --订单数 
                                    ,SUM(o.WebsiteSubsidy)websiteSubsidy  --网站补贴
                                    ,SUM(o.OrderCommission)orderCommission  --订单佣金 
                                    ,SUM(o.DistribSubsidy)deliverAmount  --配送费
                                    ,SUM(Amount)orderAmount  --订单金额
                                    from business b with(nolock)
                                    join [order] o with(nolock) on b.Id=o.businessId
                                    where o.Status!=3");  //只统计非取消订单
            if (criteria.searchType == 1)//当天
            {
                sbtbl.Append(" AND   o.PubDate between dateadd(day,-1,getdate()) and getdate() ");
            }
            else if (criteria.searchType == 2)//本周
            {
                sbtbl.Append(" AND   o.PubDate between dateadd(day,-7,getdate()) and getdate() ");
            }
            else if (criteria.searchType == 3)//本月
            {
                sbtbl.Append(" and   o.PubDate between dateadd(day,-30,getdate()) and getdate() ");
            }
            sbtbl.Append(" group by b.district ) tbl ");
            string columnList = @"  tbl.district
				                    ,tbl.orderCount
				                    ,tbl.websiteSubsidy
				                    ,tbl.orderCommission
				                    ,tbl.deliverAmount
				                    ,tbl.orderAmount ";

            var sbSqlWhere = new StringBuilder(" 1=1 ");
            string tableList = sbtbl.ToString();
            string orderByColumn = " tbl.orderCount DESC ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PagingRequest.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PagingRequest.PageSize, true);
        }
        /// <summary>
        /// 首页最近数据统计
        /// danny-20150327
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetCurrentDateCountAndMoney<T>(OrderSearchCriteria criteria)
        {

            string columnList = @"  [InsertTime] 
                                  ,[BusinessCount]
                                  ,[RzqsCount]
                                  ,[DdrzqsCount]
                                  ,[OrderPrice]
                                  ,[MisstionCount]
                                  ,[OrderCount]
                                  ,[BusinessAverageOrderCount]
                                  ,[MissionAverageOrderCount]
                                  ,[ClienterAverageOrderCount]
                                  ,[YsPrice]
                                  ,[YfPrice]
                                  ,[YkPrice]
                                  ,[ZeroSubsidyOrderCount]
                                  ,[OneSubsidyOrderCount]
                                  ,[TwoSubsidyOrderCount]
                                  ,[ThreeSubsidyOrderCount]
                                  ,[ActiveBusiness]
                                  ,[ActiveClienter]
                                  ,rechargeTotal
                                  ,SystemRecharge
                                  ,SystemPresented
                                  ,(ZhiFuBaoRecharge+WeiXinRecharge) ClientRecharge 
                                  ,incomeTotal
                                    ";

            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(criteria.orderPubStart))
            {
                sbSqlWhere.AppendFormat(" AND  CONVERT(CHAR(10),InsertTime,120)>=CONVERT(CHAR(10),'{0}',120) ", criteria.orderPubStart);
            }
            if (!string.IsNullOrWhiteSpace(criteria.orderPubEnd))
            {
                sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),InsertTime,120)<=CONVERT(CHAR(10),'{0}',120) ", criteria.orderPubEnd);
            }
            string tableList = " Statistic ";
            string orderByColumn = " id DESC  ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }

        /// <summary>
        /// 验证该平台 商户 订单号 是否存在
        /// 窦海超
        /// 2015年3月30日 13:00:02
        /// </summary>
        /// <param name="orderNO">第三方平台的原订单号</param>
        /// <param name="orderFrom">订单来源</param>
        /// <param name="orderType">订单类型</param>
        /// <returns></returns>
        public order GetOrderByOrderNoAndOrderFrom(string orderNO, int orderFrom, int orderType)
        {
            string where = string.Empty;
            if (orderType > 0)
            {
                where = " and OrderType =@OrderType AND Status <> 3";  //取消的订单还可以推送，加入订单状态判断 王超
            }
            string sql = @"SELECT * FROM [order] WHERE OriginalOrderNo = @OriginalOrderNo AND  OrderFrom = @OrderFrom " + where;
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OriginalOrderNo", orderNO);
            parm.AddWithValue("@OrderFrom", orderFrom);
            parm.AddWithValue("@OrderType", orderType);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<order>(dt)[0];
        }


        /// <summary>
        /// 获取订单金额
        /// 窦海超
        /// 2015年3月30日 16:51:35
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public SubsidyResultModel GetCurrentSubsidy(int groupId = 0, int orderType = 0)
        {
            string where = " (GroupId=0 or GroupId is null) ";


            if (Config.IsGroupPush)
            {
                if (groupId > 0)  // 集团
                {
                    if (orderType > 0)  //订单类型 1送餐订单  2取餐盒订单
                    {
                        where = " and ordertype=@ordertype ";
                    }
                    else
                    {
                        where = " GroupId=@GroupId and ordertype=@ordertype ";
                    }
                }
            }
            string sql = @"
                         SELECT TOP 1 
                         DistribSubsidy,
                         OrderCommission,
                         WebsiteSubsidy,
                         OrderType,
                         PKMCost
                          FROM dbo.subsidy(NOLOCK) WHERE 
                         StartDate<=GETDATE() AND 
                         EndDate>=GETDATE() AND 
                         [Status]=1 AND ";
            sql += where;
            sql += "ORDER BY StartDate DESC";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@ordertype", orderType);
            parm.AddWithValue("@GroupId", groupId);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<SubsidyResultModel>(dt)[0];
        }
        /// <summary>
        /// 根据订单号获取订单信息
        /// 订单id和orderNo传一个就可以
        /// wc
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        public OrderListModel GetOrderInfoByOrderNo(string orderNo, int orderId = 0)
        {
            string sql = @"
select top 1
        o.[Id] ,
        o.[OrderNo] ,
        o.[Status] ,
        c.AccountBalance ,
        c.Id clienterId ,
        o.OrderCommission ,
        o.businessId ,
        b.GroupId ,
        o.PickupCode ,
        o.OrderCount,
        ISNULL(oo.HadUploadCount,0) HadUploadCount,
        Platform 
from    [order] o with ( nolock )
        join dbo.clienter c with ( nolock ) on o.clienterId = c.Id
        join dbo.business b with ( nolock ) on o.businessId = b.Id
        left join dbo.OrderOther oo with(nolock) on o.Id = oo.OrderId
where   1 = 1 
";
            IDbParameters parm = DbHelper.CreateDbParameters();

            if (orderId != 0)
            {
                sql = sql + " and o.Id = @OrderId";
                parm.Add("@OrderId", SqlDbType.Int);
                parm.SetValue("@OrderId", orderId);
            }
            else
            {
                sql = sql + " and o.OrderNo = @OrderNo";
                parm.Add("@OrderNo", SqlDbType.NVarChar);
                parm.SetValue("@OrderNo", orderNo);
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
        /// 根据订单号获取订单信息
        /// 订单OrderNO
        /// wc
        /// </summary>
        /// <returns></returns>
        public OrderListModel GetByOrderNo(string orderNo)
        {
            string sql = @"
select top 1
        o.[Id] ,
        o.[OrderNo] ,
        o.[Status] ,
        c.AccountBalance ,
        c.AllowWithdrawPrice,
        c.Id clienterId ,
        c.GradeType,
        o.OrderCommission ,
        o.businessId ,
        b.GroupId ,
        o.PickupCode ,
        o.OrderCount,
        c.TrueName ClienterName,
        ISNULL(oo.HadUploadCount,0) HadUploadCount,
        ISNULL(oo.NeedUploadCount,0) NeedUploadCount,
        o.MealsSettleMode,
        o.BusinessReceivable,
        o.IsPay,
        b.Name BusinessName,
        o.SettleMoney,
        oo.GrabTime,
        o.Amount,
        o.DeliveryCompanySettleMoney,
        o.DeliveryCompanyID,
        o.Platform,
        ISNULL(oo.IsOrderChecked,1) as IsOrderChecked,
        GroupBusinessId,
       isnull(gb.GroupBusiName,'') as GroupBusiName
from    [order] o with ( nolock )
        left join dbo.clienter c with ( nolock ) on o.clienterId = c.Id
        join dbo.business b with ( nolock ) on o.businessId = b.Id
        join dbo.OrderOther oo with(nolock) on o.Id = oo.OrderId
        LEFT JOIN GroupBusinessRelation gbr WITH(NOLOCK) ON b.Id=gbr.BusinessId  
        LEFT JOIN dbo.GroupBusiness gb WITH(NOLOCK) ON gbr.groupid=gb.Id 
where  o.OrderNo = @OrderNo";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OrderNo", SqlDbType.NVarChar).Value = orderNo;

            var dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
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
        /// 获取订单状态根据
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int GetOrderStatus(int orderId, int businessId)
        {
            string sql = @"
select o.[Status]
from    [order] o with ( nolock ) 
where   o.Id = @Id and businessId=@businessId
";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("Id", DbType.Int32).Value = orderId;
            parm.Add("businessId", DbType.Int32).Value = businessId;
            var oStatus = DbHelper.ExecuteScalar(SuperMan_Read, sql, parm);
            return ParseHelper.ToInt(oStatus, -1);
        }
        /// <summary>
        /// 根据订单号获取订单信息
        /// 订单OrderNO
        /// wc
        /// </summary>
        /// <returns></returns>
        public OrderListModel GetByOrderId(int orderId)
        {
            string sql = @"
select top 1
        o.[Id] ,
        o.[OrderNo] ,
        o.[Status] ,
        c.AccountBalance ,
        c.AllowWithdrawPrice, 
        c.Id clienterId ,
        c.GradeType,
        o.OrderCommission ,
        o.businessId ,
        b.GroupId ,
        o.PickupCode ,
        o.OrderCount,
        c.TrueName ClienterName,
        ISNULL(oo.HadUploadCount,0) HadUploadCount,
        o.SettleMoney,
        o.IsPay,   
        o.ActualDoneDate,
        oo.GrabTime,
        o.Amount,
        isnull(o.[TipAmount],0) TipAmount,
        isnull(o.[Amount],0)+isnull(o.[TipAmount],0) as AmountAndTip,
        o.DeliveryCompanySettleMoney,
        o.DeliveryCompanyID,
        o.MealsSettleMode,
        o.Platform,
        ISNULL(oo.IsOrderChecked,1) AS IsOrderChecked,
		oo.PubLatitude,
		oo.PubLongitude       
from    [order] o with ( nolock )
        left join dbo.clienter c with ( nolock ) on o.clienterId = c.Id
        join dbo.business b with ( nolock ) on o.businessId = b.Id
        left join dbo.OrderOther oo with(nolock) on o.Id = oo.OrderId 
where   1 = 1 and o.Id = @Id
";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@Id", DbType.Int32, 4).Value = orderId;

            var dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
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

        public OrderListModel GetByOrderIdWrite(int orderId)
        {
            string sql = @"
select top 1
        o.[Id] ,
        o.[OrderNo] ,
        o.[Status] ,
        c.AccountBalance ,
        c.AllowWithdrawPrice, 
        c.Id clienterId ,
        c.GradeType,
        o.OrderCommission ,
        o.businessId ,
        b.GroupId ,
        o.PickupCode ,
        o.OrderCount,
        c.TrueName ClienterName,
        ISNULL(oo.HadUploadCount,0) HadUploadCount,
        o.SettleMoney,
        o.IsPay,   
        o.ActualDoneDate,
        oo.GrabTime,
        o.Amount,
        isnull(o.[TipAmount],0) TipAmount,
        isnull(o.[Amount],0)+isnull(o.[TipAmount],0) as AmountAndTip,
        o.DeliveryCompanySettleMoney,
        o.DeliveryCompanyID,
        o.MealsSettleMode,
        o.Platform,
        ISNULL(oo.IsOrderChecked,1) AS IsOrderChecked,
		oo.PubLatitude,
		oo.PubLongitude       
from    [order] o 
        left join dbo.clienter c on o.clienterId = c.Id
        join dbo.business b  on o.businessId = b.Id
        left join dbo.OrderOther oo on o.Id = oo.OrderId 
where   1 = 1 and o.Id = @Id
";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@Id", DbType.Int32, 4).Value = orderId;

            var dt = DbHelper.ExecuteDataTable(SuperMan_Write, sql, parm);
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
        /// 返回订单信息
        /// 窦海超
        /// 2015年5月21日 16:11:35
        /// </summary>
        /// <param name="Id">订单ID</param>
        /// <returns></returns>
        public OrderChildPayModel GetOrderById(int Id)
        {
            string sql = @"
SELECT clienterId,min(PayStatus) as PayStatus FROM dbo.[order] o(nolock)
join dbo.OrderChild oc(nolock) on o.Id= oc.OrderId
 where o.id=@id group by PayStatus,clienterId ";

            IDbParameters parms = DbHelper.CreateDbParameters();
            parms.Add("id", DbType.Int32, 4).Value = Id;
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parms);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<OrderChildPayModel>(dt)[0];
        }

        /// <summary>
        /// 获取超过配置时间未抢单的订单
        /// danny-20150402
        /// </summary>
        /// <param name="IntervalMinute"></param>
        /// <returns></returns>
        public IList<OrderAutoAdjustModel> GetOverTimeOrder(string IntervalMinute)
        {
            string sql = string.Format(@"
SELECT 
distinct(o.Id), --因为本地会有多条数据，所以加了一个distinct，线上应该不会存在这个问题
DealCount,
DateDiff(MINUTE,PubDate, GetDate()) IntervalMinute
 from dbo.[order] o (nolock)
join GlobalConfig gc (nolock) on o.BusinessGroupId = gc.GroupId
where 
gc.KeyName='IsStarTimeSubsidies' and 
gc.Value=1 and 
o.Status=0 and 
DateDiff(MINUTE,PubDate, GetDate()) in ({0})", IntervalMinute);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            return MapRows<OrderAutoAdjustModel>(dt);
        }

        /// <summary>
        /// 修改订单佣金
        /// danny-20150402
        /// </summary>
        /// <param name="config"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool UpdateOrderCommissionById(decimal AdjustAmount, int OrderId)
        {
            string sql =
                @"update [order] 
                      set OrderCommission+=@AdjustAmount,Adjustment+=@AdjustAmount ,
                      DealCount+=1
                      where Id=@OrderId and Status=0;";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OrderId", OrderId);
            parm.AddWithValue("@AdjustAmount", AdjustAmount);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }

        /// <summary>
        ///添加订单佣金日志
        /// danny-20150402
        /// </summary>
        /// <param name="config"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool InsertOrderSubsidiesLog(decimal AdjustAmount, int OrderId, int IntervalMinute)
        {
            string sql =
                @"INSERT INTO OrderSubsidiesLog
                                (Price
                                ,OrderId
                                ,InsertTime
                                ,OptName
                                ,Remark)
                     VALUES
                                (@Price
                                ,@OrderId
                                ,Getdate()
                                ,'服务平台'
                                ,@Remark);";
            string remark = string.Concat("订单超过【", IntervalMinute, "】分钟未被抢单，每单增加补贴【", AdjustAmount, "】元");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Price", AdjustAmount);
            parm.AddWithValue("@OrderId", OrderId);
            parm.AddWithValue("@Remark", remark);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;

        }

        /// <summary>
        /// 修改骑士收入
        /// danny-20150414
        /// </summary>
        /// <returns></returns>
        public bool UpdateAccountBalanceByClienterId(OrderListModel model, OrderOptionModel orderOptionModel)
        {

            string remark = "取消订单【" + model.OrderNo + "】";
            string sql =
                @"UPDATE clienter 
                    SET AccountBalance=AccountBalance-@OrderCommission
                    OUTPUT  @Platform,
                            INSERTED.Id,
                            -@OrderCommission,
                            INSERTED.AccountBalance,
                            getdate(),
                            @AdminId,
                            0,
                            @Remark,
                            @OrderId,
                            1
                      INTO Records
                          ( Platform, 
                            UserId, 
                            Amount, 
                            Balance, 
                            CreateTime, 
                            AdminId, 
                            IsDel,
                            Remark,
                            OrderId,
                            RecordType
                            )
                    WHERE Id=@ClienterId;";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OrderCommission", model.OrderCommission);
            parm.AddWithValue("@Platform", 3);
            parm.AddWithValue("@AdminId", orderOptionModel.OptUserId);
            parm.AddWithValue("@Remark", remark);
            parm.AddWithValue("@ClienterId", model.clienterId);
            parm.AddWithValue("@OrderId", model.Id);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }
        /// <summary>
        /// 取消订单
        /// danny-20150414
        /// </summary>
        /// <param name="model"></param>
        /// <param name="orderOptionModel"></param>
        /// <returns></returns>
        public bool CancelOrder(OrderListModel model, OrderOptionModel orderOptionModel)
        {
            string remark = orderOptionModel.Remark;
            string sql = string.Format(@" UPDATE dbo.[order]
                                             SET    [Status] = @Status,
                                                Othercancelreason=@OptLog
                                            OUTPUT
                                              Inserted.Id,
                                              @Price,
                                              GETDATE(),
                                              @OptId,
                                              @OptName,
                                              Inserted.[Status],
                                              @Platform,
                                              @Remark
                                            INTO dbo.OrderSubsidiesLog
                                              (OrderId,
                                              Price,
                                              InsertTime,
                                              OptId,
                                              OptName,
                                              OrderStatus,
                                              Platform,
                                              Remark)
                                             WHERE  OrderNo = @OrderNo");

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Price", model.OrderCommission);
            parm.AddWithValue("@OptId", orderOptionModel.OptUserId);
            parm.AddWithValue("@OptName", orderOptionModel.OptUserName);
            parm.AddWithValue("@Status", 3);
            parm.AddWithValue("@OrderNo", model.OrderNo);
            parm.AddWithValue("@Platform", orderOptionModel.Platform);
            parm.AddWithValue("@OptLog", orderOptionModel.OptLog);
            parm.AddWithValue("@Remark", remark + "，用户操作描述：【" + orderOptionModel.OptLog + "】");
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }
        /// <summary>
        /// 获取订单操作日志
        /// danny-20150414
        /// </summary>
        /// <param name="IntervalMinute"></param>
        /// <returns></returns>
        public IList<OrderSubsidiesLog> GetOrderOptionLog(int OrderId)
        {
            string sql = @"  SELECT  Id,
                                    OrderId,
                                    OrderStatus,
                                    OptId,
                                    OptName,
                                    InsertTime,
                                    Platform,
                                    Remark
                            FROM OrderSubsidiesLog(nolock)
                            WHERE OrderId=@OrderId
                            ORDER BY Id;";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OrderId", OrderId);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<OrderSubsidiesLog>(dt);
        }


        /// <summary>
        /// 获取订单操作日志
        /// danny-20150414
        /// </summary>
        /// <param name="IntervalMinute"></param>
        /// <returns></returns>
        public IList<OrderRecordsLog> GetOrderRecords(string originalOrderNo, int group)
        {
            #region 脚本

            string sql = @"

select  *
from    ( select    sol.Id ,
                    sol.OrderId ,
                    sol.OrderStatus ,
                    case sol.OrderStatus
                      when 0 then '待抢单'
                      when 1 then '已完成'
                      when 2 then '已接单'
                      when 3 then '已取消'
                      when 4 then '已取货' 
                      else '未知请联系E代送客服'
                    end OrderStatusStr ,
                    sol.InsertTime ,
                    '' OptName ,
                    sol.Remark ,
                    '' PhoneNo
          from      dbo.OrderSubsidiesLog sol ( nolock )
                    join dbo.business c ( nolock ) on c.Id = sol.OptId
          where     sol.OrderId = ( select top 1
                                            o.Id
                                    from    dbo.[order] o ( nolock )
                                    where   o.OriginalOrderNo = @OriginalOrderNo
                                            and o.OrderFrom = @GroupId
                                    order by o.Id desc
                                  )
                    and sol.[Platform] in ( 0 ) and c.[Status] = 1
          union
          select    sol.Id ,
                    sol.OrderId ,
                    sol.OrderStatus ,
                    case sol.OrderStatus
                      when 0 then '待抢单'
                      when 1 then '已完成'
                      when 2 then '已接单'
                      when 3 then '已取消'
                      when 4 then '已取货' 
                      else '未知请联系E代送客服'
                    end OrderStatusStr ,
                    sol.InsertTime ,
                    c.TrueName OptName ,
                    sol.Remark ,
                    ISNULL(c.PhoneNo, '') PhoneNo
          from      dbo.OrderSubsidiesLog sol ( nolock )
                    join dbo.clienter c ( nolock ) on c.Id = sol.OptId
          where     sol.OrderId = ( select top 1
                                            o.Id
                                    from    dbo.[order] o ( nolock )
                                    where   o.OriginalOrderNo = @OriginalOrderNo
                                            and o.OrderFrom = @GroupId
                                    order by o.Id desc
                                  )
                    and sol.[Platform] in ( 1 ) and c.[Status] = 1
          union
          ( select  sol.Id ,
                    sol.OrderId ,
                    sol.OrderStatus ,
                    case sol.OrderStatus
                      when 0 then '代抢单'
                      when 1 then '已完成'
                      when 2 then '已接单'
                      when 3 then '已取消'
                      when 4 then '已取货' 
                      else '未知请联系E代送客服'
                    end OrderStatusStr ,
                    sol.InsertTime ,
                    c.LoginName OptName ,
                    sol.Remark ,
                    '' PhoneNo
            from    dbo.OrderSubsidiesLog sol ( nolock )
                    join dbo.account c ( nolock ) on c.Id = sol.OptId
            where   sol.OrderId = ( select top 1
                                            o.Id
                                    from    dbo.[order] o ( nolock )
                                    where   o.OriginalOrderNo = @OriginalOrderNo
                                            and o.OrderFrom = @GroupId
                                    order by o.Id desc
                                  )
                    and sol.[Platform] = 3 and c.[Status] = 1
          )
        ) bb
order by bb.Id desc;";

            #endregion

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OriginalOrderNo", SqlDbType.NVarChar);
            parm.SetValue("@OriginalOrderNo", originalOrderNo);

            parm.AddWithValue("@GroupId", group);

            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<OrderRecordsLog>(dt);
        }


        /// <summary>
        /// 订单详细
        /// </summary>
        /// <param name="orderNo">订单号码</param>
        /// <returns>订单</returns>
        public IList<OrderDetailModel> GetOrderDetail(string order_no)
        {
            string sql = @"SELECT  o.[OrderNo]
                                        ,o.[ProductName]
                                        ,o.[UnitPrice]
                                        ,o.[Quantity] 
                                    FROM [OrderDetail] o WITH ( NOLOCK )
                                    WHERE o.OrderNo=@OriginalOrderNo";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OriginalOrderNo", SqlDbType.NVarChar);
            parm.SetValue("@OriginalOrderNo", order_no);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count <= 0)
                return null;
            return MapRows<OrderDetailModel>(dt);
        }


        /// <summary>
        /// 新增订单       
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150509</UpdateTime>
        /// <param name="order">订单实体</param>
        /// <returns></returns>
        public int AddOrder(order order)
        {

            #region 写入订单表、订单日志表
            string insertSql = @"
insert  into dbo.[order]
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
          CommissionRate ,
          BaseCommission ,
          CommissionFormulaMode ,
          SongCanDate ,
          [Weight] ,
          Quantity ,
          ReceiveProvince ,
          ReceiveProvinceCode ,
          ReceiveCityCode ,
          ReceiveArea ,
          ReceiveAreaCode ,
          OriginalOrderNo ,
          BusinessCommission ,
          SettleMoney ,
          Adjustment ,
          CommissionType,
          CommissionFixValue,
          BusinessGroupId,
          TimeSpan,
          MealsSettleMode,
          BusinessReceivable,
          GroupBusinessId
        )
values  ( @OrderNo ,
          @PickUpAddress ,
          getdate() ,
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
          @CommissionRate ,
          @BaseCommission,
          @CommissionFormulaMode ,
          @SongCanDate ,
          @Weight1 ,
          @Quantity1 ,
          @ReceiveProvince ,
          @ReceiveProvinceCode ,
          @ReceiveCityCode ,
          @ReceiveArea ,
          @ReceiveAreaCode ,
          @OriginalOrderNo ,
          @BusinessCommission ,
          @SettleMoney ,
          @Adjustment ,
          @CommissionType,
          @CommissionFixValue,
          @BusinessGroupId,
          @TimeSpan,
          @MealsSettleMode,
          @BusinessReceivable,
          @GroupBusinessId
        )
select @@identity";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@OrderNo", order.OrderNo);
            dbParameters.AddWithValue("@PickUpAddress", order.PickUpAddress);
            dbParameters.AddWithValue("@ReceviceName", order.ReceviceName);
            dbParameters.AddWithValue("@RecevicePhoneNo", order.RecevicePhoneNo);
            dbParameters.AddWithValue("@ReceviceAddress", order.ReceviceAddress);
            dbParameters.AddWithValue("@IsPay", order.IsPay);
            dbParameters.AddWithValue("@Amount", order.Amount);
            dbParameters.AddWithValue("@OrderCommission", order.OrderCommission);
            dbParameters.AddWithValue("@DistribSubsidy", order.DistribSubsidy);
            dbParameters.AddWithValue("@WebsiteSubsidy", order.WebsiteSubsidy);
            dbParameters.AddWithValue("@Remark", order.Remark);
            dbParameters.AddWithValue("@OrderFrom", order.OrderFrom);
            dbParameters.AddWithValue("@Status", order.Status);
            dbParameters.AddWithValue("@businessId", order.businessId);
            dbParameters.AddWithValue("@ReceviceCity", order.ReceviceCity);
            dbParameters.AddWithValue("@ReceviceLongitude", order.ReceviceLongitude);
            dbParameters.AddWithValue("@ReceviceLatitude", order.ReceviceLatitude);
            dbParameters.AddWithValue("@OrderCount", order.OrderCount);
            dbParameters.AddWithValue("@CommissionRate", order.CommissionRate);
            dbParameters.AddWithValue("@BaseCommission", order.BaseCommission);
            dbParameters.AddWithValue("@CommissionFormulaMode", order.CommissionFormulaMode);
            dbParameters.AddWithValue("@SongCanDate", order.SongCanDate);
            dbParameters.AddWithValue("@Weight1", order.Weight);
            dbParameters.AddWithValue("@Quantity1", order.Quantity);
            dbParameters.AddWithValue("@ReceiveProvince", order.ReceiveProvince);
            dbParameters.AddWithValue("@ReceiveProvinceCode", order.ReceiveProvinceCode);
            dbParameters.AddWithValue("@ReceiveCityCode", order.ReceiveCityCode);
            dbParameters.AddWithValue("@ReceiveArea", order.ReceiveArea);
            dbParameters.AddWithValue("@ReceiveAreaCode", order.ReceiveAreaCode);
            dbParameters.AddWithValue("@OriginalOrderNo", order.OriginalOrderNo);
            dbParameters.AddWithValue("@BusinessCommission", order.BusinessCommission);
            dbParameters.AddWithValue("@SettleMoney", order.SettleMoney);
            dbParameters.AddWithValue("@Adjustment", order.Adjustment);
            dbParameters.AddWithValue("@CommissionType", order.CommissionType);
            dbParameters.AddWithValue("@CommissionFixValue", order.CommissionFixValue);
            dbParameters.AddWithValue("@BusinessGroupId", order.BusinessGroupId);
            dbParameters.AddWithValue("@TimeSpan", order.TimeSpan);
            dbParameters.AddWithValue("@MealsSettleMode", order.MealsSettleMode);
            dbParameters.AddWithValue("@BusinessReceivable", order.BusinessReceivable);
            dbParameters.AddWithValue("@GroupBusinessId", order.GroupBusinessId);

            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            int orderId = ParseHelper.ToInt(result, 0);
            if (orderId <= 0)//写入订单失败
            {
                return 0;
            }

            string sqlOrderLog = @"
        insert into dbo.OrderSubsidiesLog ( OrderId, InsertTime, OptName, Remark,
                                     OptId, OrderStatus, [Platform] )
    values(@orderid,getdate(),@optname,@remark,@optid,@orderStatus,@platForm)";

            IDbParameters orderLogParameters = DbHelper.CreateDbParameters();
            orderLogParameters.AddWithValue("orderid", orderId);
            orderLogParameters.AddWithValue("optname", order.BusinessName);
            orderLogParameters.AddWithValue("remark", OrderConst.PublishOrder);
            orderLogParameters.AddWithValue("optid", order.businessId);
            orderLogParameters.AddWithValue("orderStatus", OrderStatus.Status0.GetHashCode());
            orderLogParameters.AddWithValue("platForm", SuperPlatform.FromBusiness.GetHashCode());

            DbHelper.ExecuteNonQuery(SuperMan_Write, sqlOrderLog, orderLogParameters);
            #endregion

            #region 写子OrderOther表
            const string insertOtherSql = @"
insert into OrderOther(OrderId,NeedUploadCount,HadUploadCount,PubLongitude,PubLatitude,OneKeyPubOrder,IsOrderChecked,IsAllowCashPay,IsPubDateTimely)
values(@OrderId,@NeedUploadCount,0,@PubLongitude,@PubLatitude,@OneKeyPubOrder,@IsOrderChecked,@IsAllowCashPay,@IsPubDateTimely)";
            IDbParameters dbOtherParameters = DbHelper.CreateDbParameters();
            dbOtherParameters.AddWithValue("@OrderId", orderId); //商户ID
            dbOtherParameters.AddWithValue("@NeedUploadCount", order.OrderCount); //需上传数量
            dbOtherParameters.AddWithValue("@PubLongitude", order.PubLongitude);
            dbOtherParameters.AddWithValue("@PubLatitude", order.PubLatitude);
            dbOtherParameters.AddWithValue("@OneKeyPubOrder", order.OneKeyPubOrder);
            dbOtherParameters.Add("@IsOrderChecked", DbType.Int32).Value = order.IsOrderChecked;
            dbOtherParameters.Add("@IsAllowCashPay", DbType.Int32).Value = order.IsAllowCashPay;
            dbOtherParameters.Add("@IsPubDateTimely", DbType.Int32).Value = order.IsPubDateTimely;

            DbHelper.ExecuteScalar(SuperMan_Write, insertOtherSql, dbOtherParameters);
            #endregion

            if (order.listOrderChild != null && order.listOrderChild.Count > 0)
            {
                AddOrderChild(orderId, order);
            }

            return orderId;
        }


        /// <summary>
        /// 写入订单子表
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150512</UpdateTime>
        /// <returns>订单实体</returns>
        void AddOrderChild(int orderId, order order)
        {
            #region 性能优化
            using (SqlBulkCopy bulk = new SqlBulkCopy(SuperMan_Write))
            {
                try
                {
                    bulk.BatchSize = 1000;
                    bulk.DestinationTableName = "OrderChild";
                    bulk.NotifyAfter = order.listOrderChild.Count;
                    bulk.ColumnMappings.Add("OrderId", "OrderId");
                    bulk.ColumnMappings.Add("ChildId", "ChildId");
                    bulk.ColumnMappings.Add("TotalPrice", "TotalPrice");
                    bulk.ColumnMappings.Add("GoodPrice", "GoodPrice");
                    bulk.ColumnMappings.Add("DeliveryPrice", "DeliveryPrice");
                    bulk.ColumnMappings.Add("PayStatus", "PayStatus");
                    bulk.ColumnMappings.Add("CreateBy", "CreateBy");
                    bulk.ColumnMappings.Add("UpdateBy", "UpdateBy");

                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("OrderId", typeof(int)));
                    dt.Columns.Add(new DataColumn("ChildId", typeof(int)));
                    dt.Columns.Add(new DataColumn("TotalPrice", typeof(decimal)));
                    dt.Columns.Add(new DataColumn("GoodPrice", typeof(decimal)));
                    dt.Columns.Add(new DataColumn("DeliveryPrice", typeof(decimal)));
                    dt.Columns.Add(new DataColumn("PayStatus", typeof(int)));
                    dt.Columns.Add(new DataColumn("CreateBy", typeof(string)));
                    dt.Columns.Add(new DataColumn("UpdateBy", typeof(string)));

                    for (int i = 0; i < order.listOrderChild.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["OrderId"] = orderId;
                        //dr["ChildId"] = order.listOrderChild[i].ChildId;
                        int num = i + 1;
                        dr["ChildId"] = num;
                        decimal totalPrice = order.listOrderChild[i].GoodPrice + Convert.ToDecimal(order.DistribSubsidy);
                        dr["TotalPrice"] = totalPrice;
                        dr["GoodPrice"] = order.listOrderChild[i].GoodPrice;
                        dr["DeliveryPrice"] = order.DistribSubsidy;
                        if ((bool)order.IsPay ||
                            (!(bool)order.IsPay && order.MealsSettleMode == MealsSettleMode.LineOff.GetHashCode())
                            )//已付款 未付款线下付款
                            dr["PayStatus"] = 1;
                        else
                            dr["PayStatus"] = 0;
                        dr["CreateBy"] = order.BusinessName;
                        dr["UpdateBy"] = order.BusinessName;
                        dt.Rows.Add(dr);
                    }
                    bulk.WriteToServer(dt);
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
            #endregion
        }

        /// <summary>
        /// 获取订单实体   
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150512</UpdateTime>
        /// <param name="orderPM">获取订单详情参数实体</param>
        /// <returns></returns>
        public order GetById(OrderPM orderPM)
        {
            order modle = new order();

            const string querySql = @"
declare @clienterLongitude FLOAT;
declare @clienterLatitude FLOAT;
set @clienterLongitude=@Longitude;
set @clienterLatitude=@Latitude;

select  o.Id,o.OrderNo,o.PickUpAddress,o.PubDate,o.ReceviceName,o.RecevicePhoneNo,
case  isnull(o.ReceviceAddress,'')  
		when  '' then '附近3公里左右，由商户指定'
		else o.ReceviceAddress end as ReceviceAddress,
o.ActualDoneDate,o.IsPay,
    o.Amount,o.OrderCommission,o.DistribSubsidy,o.WebsiteSubsidy,o.Remark,o.Status,o.clienterId,o.businessId,o.ReceviceCity,o.ReceviceLongitude,
    o.ReceviceLatitude,o.OrderFrom,o.OriginalOrderId,o.OriginalOrderNo,o.Quantity,o.Weight,o.ReceiveProvince,o.ReceiveArea,o.ReceiveProvinceCode,
    o.ReceiveCityCode,o.ReceiveAreaCode,o.OrderType,o.KM,o.GuoJuQty,o.LuJuQty,o.SongCanDate,o.OrderCount,o.CommissionRate,o.Payment,
    o.CommissionFormulaMode,o.Adjustment,o.BusinessCommission,o.SettleMoney,o.DealCount,o.PickupCode,o.OtherCancelReason,o.CommissionType,
    o.CommissionFixValue,o.BusinessGroupId,o.TimeSpan,o.Invoice,o.DeliveryCompanyID,
    isnull(o.DistribSubsidy,0)*isnull(o.OrderCount,0) as TotalDistribSubsidy,(o.Amount+isnull(o.DistribSubsidy,0)*isnull(o.OrderCount,0)) as TotalAmount,
    o.MealsSettleMode,o.IsComplain,oo.IsAllowCashPay,
    b.[City] BusinessCity,b.Name BusinessName,b.PhoneNo BusinessPhone ,b.PhoneNo2 BusinessPhone2,b.Address BusinessAddress ,b.GroupId, b.Landline,
    b.Longitude, b.Latitude,REPLACE(b.City,'市','') AS pickUpCity,
    oo.NeedUploadCount,oo.HadUploadCount,oo.GrabTime,
    c.TrueName ClienterName,c.PhoneNo ClienterPhoneNo,
   (case 
    when  ISNULL(b.Latitude,0)=0 or ISNULL(b.Longitude,0)=0 or @clienterLongitude=0 or  @clienterLatitude=0  then -1
    else round(geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(geography::Point(@clienterLatitude,@clienterLongitude,4326)),0)
    end)
    as distance,isnull(oo.OneKeyPubOrder,0) as OneKeyPubOrder,oo.ExpectedTakeTime,
    o.[Platform]
from  dbo.[order] o (nolock)
    join business b (nolock) on b.Id=o.businessId
    left join dbo.OrderOther oo (nolock) on o.Id=oo.orderId
    left join clienter c on  o.clienterId=c.id
where  o.Id=@Id ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters("Id", DbType.Int32, 4, orderPM.OrderId);
            dbParameters.AddWithValue("Longitude", orderPM.longitude);
            dbParameters.AddWithValue("Latitude", orderPM.latitude);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, querySql, dbParameters);
            if (dt == null || dt.Rows.Count <= 0)
                return null;

            return MapRows<order>(dt)[0];
        }

        /// <summary>
        /// 判断订单是否存在
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150512</UpdateTime>
        /// <param name="id">订单Id</param>
        /// <returns></returns>
        public bool IsExist(int id)
        {
            bool isExist;

            string querySql = @" select COUNT(1)
 from   dbo.[order] WITH ( NOLOCK ) 
 where  Id = @id";

            IDbParameters dbParameters = DbHelper.CreateDbParameters("Id", DbType.Int32, 4, id);
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, querySql, dbParameters);
            isExist = ParseHelper.ToInt(executeScalar, 0) > 0;

            return isExist;
        }
        /// <summary>
        /// 判断制定状态的订单是否存在
        /// danny-20150908
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        public bool CheckOrderIsExist(int orderId, int orderStatus)
        {
            var querySql = @" select count(1)  from   dbo.[order] with(nolock) where  Id = @OrderId AND Status=@OrderStatus;";
            var dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@OrderId", orderId);
            dbParameters.AddWithValue("@OrderStatus", orderStatus);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, querySql, dbParameters)) > 0;
        }
        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150520</UpdateTime>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetStatus(int id)
        {
            int status;

            string querySql = @" select Status
 from   dbo.[order] WITH ( NOLOCK ) 
 where  Id = @id";

            IDbParameters dbParameters = DbHelper.CreateDbParameters("Id", DbType.Int32, 4, id);
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, querySql, dbParameters);
            status = ParseHelper.ToInt(executeScalar, 0);

            return status;
        }

        /// <summary>
        /// 判断订单是否存在
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150512</UpdateTime>
        /// <param name="order">商户Id,时间戳</param>
        /// <returns></returns>
        public bool IsExist(order order)
        {
            bool isExist;

            const string querysql = @"
select  count(1) 
from  dbo.[order]  
where businessId=@businessId and TimeSpan=@TimeSpan ";
            IDbParameters dbSelectParameters = DbHelper.CreateDbParameters();
            dbSelectParameters.AddWithValue("businessId", order.businessId);
            dbSelectParameters.AddWithValue("TimeSpan", order.TimeSpan);
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Write, querysql, dbSelectParameters);
            isExist = ParseHelper.ToInt(executeScalar, 0) > 0;

            return isExist;
        }
        /// <summary>
        /// 查询X小时内没有加入可提现金额的订单
        /// 窦海超
        /// 2015年5月15日 16:40:05
        /// </summary>
        /// <param name="hour">多少小时内的数据</param>
        public IList<NonJoinWithdrawModel> GetNonJoinWithdraw(double hour)
        {
            string sql = @"
select 
o.id,o.OrderNo, o.amount, 
o.OrderCommission clienterPrice, --给骑士
o.Amount-o.SettleMoney businessPrice,--给商家
o.clienterId, o.businessId
from    dbo.[order] o ( nolock )
        join dbo.OrderOther oo ( nolock ) on o.Id = oo.OrderId
where   oo.IsJoinWithdraw = 0    
        and o.Status = 1 --已完成订单        
        and o.platform=3
        and datediff(hour, o.ActualDoneDate, getdate()) >= @hour";
            IDbParameters parm = DbHelper.CreateDbParameters("@hour", DbType.Int64, 4, hour);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (!dt.HasData())
            {
                return new List<NonJoinWithdrawModel>();
            }
            return MapRows<NonJoinWithdrawModel>(dt);
        }

        /// <summary>
        /// 获取闪送待取消订单 查询超过x小时，未支付的订单
        /// </summary>
        /// 胡灵波
        /// 2015年12月15日 13:37:17
        /// <param name="hour"></param>
        /// <returns></returns>
        public IList<NonJoinWithdrawModel> GetSSCancelOrder(double hour)
        {
            string sql = @"
select 
o.id,o.OrderNo, o.amount, 
o.OrderCommission clienterPrice, --给骑士
o.Amount-o.SettleMoney businessPrice,--给商家
o.clienterId, o.businessId
from    dbo.[order] o ( nolock )
        join dbo.OrderOther oo ( nolock ) on o.Id = oo.OrderId
where    
        o.Status = 50        
        and o.platform=3
        and datediff(hour, o.pubDate, getdate()) >= @hour ";
            IDbParameters parm = DbHelper.CreateDbParameters("@hour", DbType.Int64, 4, hour);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (!dt.HasData())
            {
                return new List<NonJoinWithdrawModel>();
            }
            return MapRows<NonJoinWithdrawModel>(dt);
        }

        /// <summary>
        /// 获取物流公司全部任务
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150708</UpdateTime>
        /// <param name="model"></param>
        /// <returns></returns>
        public IList<GetJobCDM> GetExpressAllJob(GetJobCPM model)
        {
            string querysql = string.Format(@"
declare @cliernterPoint geography ;
select @cliernterPoint=geography::Point(@Latitude,@Longitude,4326) ;
select top {0}
        a.Id, a.OrderCommission, a.OrderCount,a.Platform,a.Weight,a.KM,a.TakeType,a.SongCanDate,
        ( a.Amount + a.OrderCount * a.DistribSubsidy ) as Amount,
        a.Amount CpAmount,
        b.Name as BusinessName, b.City as BusinessCity,b.Id BusinessId,
        b.Address as BusinessAddress, isnull(a.ReceviceCity, '') as UserCity,
        a.Remark,a.OrderNo,a.PubName,'' ExpectedTakeTime,
 case  isnull(a.ReceviceAddress,'')  
		when  '' then '附近3公里左右，由商户指定'
		else a.ReceviceAddress end as  UserAddress,ISNULL(b.Longitude,0) as  Longitude,ISNULL(b.Latitude,0) as Latitude,
        case convert(varchar(100), PubDate, 23)
          when convert(varchar(100), getdate(), 23) then '今日 '
          else substring(convert(varchar(100), PubDate, 23), 6, 5)
        end + '  ' + substring(convert(varchar(100), PubDate, 24), 1, 5) as PubDate,
 case  a.Platform
        when 3 then
			round(geography::Point(ISNULL(a.Pickuplatitude,0),ISNULL(a.Pickuplongitude,0),4326).STDistance(@cliernterPoint),0) 
		else  
		round(geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint),0) 
       end  DistanceToBusiness
      
from    dbo.[order] a ( nolock )
        join dbo.business b ( nolock ) on a.businessId = b.Id
        left join dbo.BusinessExpressRelation ber (nolock) on a.businessId=ber.BusinessId
where a.status = 0 and a.IsEnable=1 and ber.IsEnable=1 and a.ReceviceCity=@ReceviceCity and ber.ExpressId=@ExpressId
and a.Platform!=3 
order by a.Id desc", model.TopNum);

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Latitude", model.Latitude);
            dbParameters.AddWithValue("Longitude", model.Longitude);
            dbParameters.AddWithValue("ReceviceCity", model.City);
            dbParameters.AddWithValue("ExpressId", model.ExpressId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                return TranslateGetJobC(dt, model);
            }
            return new List<GetJobCDM>();
        }

        /// <summary>
        ///获取物流公司附近任务
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150708</UpdateTime>
        /// <param name="model"></param>
        /// <returns></returns>
        public IList<GetJobCDM> GetExpressNearJob(GetJobCPM model)
        {
            string querysql = string.Format(@"
declare @cliernterPoint geography ;
select @cliernterPoint=geography::Point(@Latitude,@Longitude,4326) ;
select top {0} a.Id,a.OrderCommission,a.OrderCount,a.Platform,a.Weight,a.KM,a.TakeType,a.SongCanDate,
        (a.Amount+a.OrderCount*a.DistribSubsidy) as Amount,
        a.Amount CpAmount,a.PubName,'' ExpectedTakeTime,
        b.Name as BusinessName,b.City as BusinessCity,b.Address as BusinessAddress,b.Id BusinessId,
ISNULL(a.ReceviceCity,'') as UserCity,case  isnull(a.ReceviceAddress,'')  
		when  '' then '附近3公里左右，由商户指定'
		else a.ReceviceAddress end as  UserAddress,
        a.Remark,a.OrderNo,
        ISNULL(b.Longitude,0) as  Longitude,ISNULL(b.Latitude,0) as Latitude,
        case convert(varchar(100), PubDate, 23) 
	        when convert(varchar(100), getdate(), 23) then '今日 '
            else substring(convert(varchar(100), PubDate, 23),6,5) 
        end
        +'  '+substring(convert(varchar(100),PubDate,24),1,5)
        as PubDate,
        case  a.Platform
        when 3 then
			round(geography::Point(ISNULL(a.Pickuplatitude,0),ISNULL(a.Pickuplongitude,0),4326).STDistance(@cliernterPoint),0) 
		else  
		round(geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint),0) 
       end  DistanceToBusiness
from dbo.[order] a (nolock)
        join dbo.business b (nolock) on a.businessId=b.Id
        left join dbo.BusinessExpressRelation ber (nolock) on a.businessId=ber.BusinessId
where a.status=0 and a.IsEnable=1 and ber.IsEnable=1 and ber.ExpressId=@ExpressId
and  geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint)<= @PushRadius
and a.Platform!=3
order by geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint) asc
", model.TopNum);

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Latitude", model.Latitude);
            dbParameters.AddWithValue("Longitude", model.Longitude);
            dbParameters.AddWithValue("PushRadius", ParseHelper.ToInt(model.PushRadius) * 1000);
            dbParameters.AddWithValue("ExpressId", model.ExpressId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                return TranslateGetJobC(dt, model);
            }
            return new List<GetJobCDM>();
        }

        /// <summary>
        /// 最新任务列表 即 全部订单  
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IList<GetJobCDM> GetLastedJobC(GetJobCPM model)
        {
            //string whereStr = string.Format(" and a.ReceviceCity = '{0}' ", model.City);
            string whereStr = " and 1=1 ";
            string sql = null;
            if (model.ClienterId == 0 || model.IsBind == (int)IsBindBC.No)  // 查询所有 无雇佣骑士的商家发布的订单，以及有雇佣骑士的商家发布的超过了五分钟无人抢单的订单 
            {
                sql = string.Format(@"
declare @cliernterPoint geography ;
select @cliernterPoint=geography::Point(@Latitude,@Longitude,4326) ;
select top {0}
        a.Id, a.OrderCommission, a.OrderCount,a.PubName,
        ( a.Amount + a.OrderCount * a.DistribSubsidy ) as Amount,a.Platform,a.Weight,a.KM,a.TakeType,
        convert(varchar(16),a.SongCanDate,120) SongCanDate,
        a.Amount CpAmount,convert(varchar(16),oo.ExpectedTakeTime,120) ExpectedTakeTime,
        b.Name as BusinessName, b.City as BusinessCity,b.Id BusinessId,
        isnull(a.ReceviceCity, '') as UserCity,
       a.Remark,a.OrderNo,
 case  isnull(a.ReceviceAddress,'')  
		when  '' then '附近3公里左右，由商户指定'
		else a.ReceviceAddress end as  UserAddress,
	
        case convert(varchar(100), PubDate, 23)
          when convert(varchar(100), getdate(), 23) then '今日 '
          else substring(convert(varchar(100), PubDate, 23), 6, 5)
        end + '  ' + substring(convert(varchar(100), PubDate, 24), 1, 5) as PubDate,
(case when [Platform]=1 then ISNULL(b.Longitude,0) 
when [Platform]=2 then isnull(oo.PubLongitude,0)
when [Platform]=3 then a.pickUpLongitude  else  '' end) as  Longitude,--商户发单经度
(case when [Platform]=1 then ISNULL(b.Latitude,0) 
when [Platform]=2 then isnull(oo.PubLatitude,0) 
when [Platform]=3 then a.pickUpLatitude else '' end) as  Latitude,--商户发单纬度
(case when [Platform]=1 then round(geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint),0) 
when [Platform]=2 then round(geography::Point(ISNULL(oo.PubLatitude,0),ISNULL(oo.PubLongitude,0),4326).STDistance(@cliernterPoint),0) 
when [Platform]=3 then round(geography::Point(ISNULL(a.Pickuplatitude,0),ISNULL(a.Pickuplongitude,0),4326).STDistance(@cliernterPoint),0) 
else '' end)  as DistanceToBusiness,--距离
(case when [Platform]=1 then b.Address when [Platform]=3 then a.PickUpAddress else '' end) as BusinessAddress --发货地址
        
from    dbo.[order] a ( nolock )
		join dbo.OrderOther oo(nolock) on a.Id=oo.OrderId
        join dbo.business b ( nolock ) on a.businessId = b.Id
where   a.status = 0 and a.IsEnable=1  and( b.IsBind=0 or (b.IsBind=1 and DATEDIFF(minute,a.PubDate,GETDATE())>{1}))
        {2}
order by a.Id desc", model.TopNum, model.ExclusiveOrderTime, whereStr);
            }
            else  //查询所有 无雇佣骑士的商家发布的订单，以及 非当前骑士的雇主 里 有雇佣骑士的商家 发布的超过了 五分钟 无人抢单的订单 以及当前骑士所属雇主的所有订单
            {
                sql = string.Format(@"
declare @cliernterPoint geography ;
select @cliernterPoint=geography::Point(@Latitude,@Longitude,4326) ;
select top {0}
        a.Id, a.OrderCommission, a.OrderCount,a.PubName,
        ( a.Amount + a.OrderCount * a.DistribSubsidy ) as Amount,a.Platform,a.Weight,a.KM,a.TakeType,
        convert(varchar(16),a.SongCanDate,120) SongCanDate,
        a.Amount CpAmount,a.OrderNo,
        convert(varchar(16),oo.ExpectedTakeTime,120) ExpectedTakeTime,
        b.Name as BusinessName, b.City as BusinessCity,a.OrderNo,b.Id BusinessId,
         isnull(a.ReceviceCity, '') as UserCity,
        a.Remark,case  isnull(a.ReceviceAddress,'')  
		when  '' then '附近3公里左右，由商户指定'
		else a.ReceviceAddress end as  UserAddress,
        case convert(varchar(100), PubDate, 23)
          when convert(varchar(100), getdate(), 23) then '今日 '
          else substring(convert(varchar(100), PubDate, 23), 6, 5)
        end + '  ' + substring(convert(varchar(100), PubDate, 24), 1, 5) as PubDate,
        
[Platform], --来源（默认1、旧后台，2、智能调度，3新后台）
(case when [Platform]=1 then ISNULL(b.Longitude,0) 
when [Platform]=2 then isnull(oo.PubLongitude,0) 
when [Platform]=3 then a.pickUpLongitude  else '' end) as  Longitude,--商户发单经度
(case when [Platform]=1 then ISNULL(b.Latitude,0) 
when [Platform]=2 then isnull(oo.PubLatitude,0)
when [Platform]=3 then a.pickUpLatitude else '' end) as  Latitude,--商户发单纬度
(case when [Platform]=1 then round(geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint),0) 
when [Platform]=2 then round(geography::Point(ISNULL(oo.PubLatitude,0),ISNULL(oo.PubLongitude,0),4326).STDistance(@cliernterPoint),0)
when [Platform]=3 then round(geography::Point(ISNULL(a.Pickuplatitude,0),ISNULL(a.Pickuplongitude,0),4326).STDistance(@cliernterPoint),0) 
else '' end)  as DistanceToBusiness,--距离
(case when [Platform]=1 then b.Address when [Platform]=3 then a.PickUpAddress else '' end) as BusinessAddress --发货地址

from    dbo.[order] a ( nolock )
join dbo.OrderOther oo(nolock) on a.Id=oo.OrderId 
        join dbo.business b ( nolock ) on a.businessId = b.Id
        left join ( select  distinct
                            ( temp.BusinessId )
                    from    BusinessClienterRelation temp
                    where   temp.IsEnable = 1
                            and temp.IsBind = 1
                            and temp.ClienterId = {1}
                  ) as c on a.BusinessId = c.BusinessId        
where   a.status = 0 and a.IsEnable=1
        and ( b.IsBind = 0
              or ( b.IsBind = 1
                   and DATEDIFF(minute, a.PubDate, GETDATE()) > {2}
                 )
              or c.BusinessId is not null
            )
        {3}
order by a.Id desc", model.TopNum, model.ClienterId, model.ExclusiveOrderTime, whereStr);
            }

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Latitude", model.Latitude);
            dbParameters.AddWithValue("Longitude", model.Longitude);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                return TranslateGetJobC(dt, model);
            }
            return new List<GetJobCDM>();
        }

        /// <summary>
        /// 骑士端获取任务列表最近任务   add by caoheyang 20150519
        /// 窦海超 改 新增闪送模式信息,中间上下有空格位置为新更改字段
        /// </summary>
        /// <param name="model">订单查询实体</param>
        /// <returns></returns>
        public IList<GetJobCDM> GetJobC(GetJobCPM model)
        {
            string sql = null;
            if (model.ClienterId == 0 || model.IsBind == (int)IsBindBC.No) //未登录时以及非雇佣骑士，查询所有 无雇佣骑士的商家发布的订单，以及有雇佣骑士的商家发布的超过了五分钟无人抢单的订单 
            {
                sql = string.Format(@"
declare @cliernterPoint geography ;
select @cliernterPoint=geography::Point(@Latitude,@Longitude,4326) ;
select top {0} a.Id,a.OrderCommission,a.OrderCount,a.PubName,   
(a.Amount+a.OrderCount*a.DistribSubsidy) as Amount,a.Platform,a.Weight,a.KM,a.TakeType, convert(varchar(16),a.SongCanDate,120) SongCanDate,b.Id BusinessId,
a.Amount CpAmount,convert(varchar(16),oo.ExpectedTakeTime,120) ExpectedTakeTime,
a.Remark,a.OrderNo,
b.Name as BusinessName,b.City as BusinessCity,
ISNULL(a.ReceviceCity,'') as UserCity, case  isnull(a.ReceviceAddress,'')  
		when  '' then '附近3公里左右，由商户指定'
		else a.ReceviceAddress end as  UserAddress,
case convert(varchar(100), PubDate, 23) 
	when convert(varchar(100), getdate(), 23) then '今日 '
    else substring(convert(varchar(100), PubDate, 23),6,5) 
end
+'  '+substring(convert(varchar(100),PubDate,24),1,5)
as PubDate,
(case when [Platform]=1 then ISNULL(b.Longitude,0) 
when [Platform]=2 then isnull(oo.PubLongitude,0) 
when [Platform]=3 then a.pickUpLongitude else '' end) as  Longitude,--商户发单经度
(case when [Platform]=1 then ISNULL(b.Latitude,0) 
when [Platform]=2 then isnull(oo.PubLatitude,0) 
when [Platform]=3 then a.pickUpLatitude  else '' end) as  Latitude,--商户发单纬度
(case when [Platform]=1 then round(geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint),0) 
when [Platform]=2 then round(geography::Point(ISNULL(oo.PubLatitude,0),ISNULL(oo.PubLongitude,0),4326).STDistance(@cliernterPoint),0) 
when [Platform]=3 then round(geography::Point(ISNULL(a.Pickuplatitude,0),ISNULL(a.Pickuplongitude,0),4326).STDistance(@cliernterPoint),0) 
	 else '' end)  as DistanceToBusiness,--距离
(case when [Platform]=1 then b.Address when [Platform]=3 then a.PickUpAddress else '' end) as BusinessAddress --发货地址
 
from dbo.[order] a (nolock)
join dbo.OrderOther oo(nolock) on a.Id=oo.OrderId
join dbo.business b (nolock) on a.businessId=b.Id
where a.status=0 and a.IsEnable=1 and( b.IsBind=0 or (b.IsBind=1 and DATEDIFF(minute,a.PubDate,GETDATE())>{1}))
and  geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint)<= @PushRadius
order by geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint) asc
", model.TopNum, model.ExclusiveOrderTime);
            }
            else //查询所有 无雇佣骑士的商家发布的订单，以及 非当前骑士的雇主 里 有雇佣骑士的商家 发布的超过了 五分钟 无人抢单的订单 以及当前骑士所属雇主的所有订单
            {
                sql = string.Format(@"
declare @cliernterPoint geography ;
select @cliernterPoint=geography::Point(@Latitude,@Longitude,4326) ;
select top {0} a.Id,a.OrderCommission,a.OrderCount,   
(a.Amount+a.OrderCount*a.DistribSubsidy) as Amount,a.Platform,a.Weight,a.KM,a.TakeType, convert(varchar(16),a.SongCanDate,120) SongCanDate,
a.Amount CpAmount,convert(varchar(16),oo.ExpectedTakeTime,120) ExpectedTakeTime,a.PubName,
a.Remark,a.OrderNo,b.Id BusinessId,
b.Name as BusinessName,b.City as BusinessCity,
ISNULL(a.ReceviceCity,'') as UserCity, case  isnull(a.ReceviceAddress,'')  
		when  '' then '附近3公里左右，由商户指定'
		else a.ReceviceAddress end as  UserAddress,
case convert(varchar(100), PubDate, 23) 
	when convert(varchar(100), getdate(), 23) then '今日 '
    else substring(convert(varchar(100), PubDate, 23),6,5) 
end
+'  '+substring(convert(varchar(100),PubDate,24),1,5)
as PubDate,

(case when [Platform]=1 then ISNULL(b.Longitude,0) 
when [Platform]=2 then isnull(oo.PubLongitude,0) 
when [Platform]=3 then a.pickUpLongitude else '' end) as  Longitude,--商户发单经度
(case when [Platform]=1 then ISNULL(b.Latitude,0) 
when [Platform]=2 then isnull(oo.PubLatitude,0) 
when [Platform]=3 then a.pickUpLatitude else '' end) as  Latitude,--商户发单纬度
(case when [Platform]=1 then round(geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint),0) 
when [Platform]=2 then round(geography::Point(ISNULL(oo.PubLatitude,0),ISNULL(oo.PubLongitude,0),4326).STDistance(@cliernterPoint),0) 
when [Platform]=3 then round(geography::Point(ISNULL(a.Pickuplatitude,0),ISNULL(a.Pickuplongitude,0),4326).STDistance(@cliernterPoint),0) 
	 else '' end)  as DistanceToBusiness,--距离
(case when [Platform]=1 then b.Address when [Platform]=3 then a.PickUpAddress else '' end) as BusinessAddress --发货地址

from dbo.[order] a (nolock)
join dbo.OrderOther oo(nolock) on a.Id=oo.OrderId
join dbo.business b (nolock) on a.businessId=b.Id
left join ( select  distinct
                            ( temp.BusinessId )
                    from    BusinessClienterRelation temp
                    where   temp.IsEnable = 1
                            and temp.IsBind = 1
                            and temp.ClienterId = {1}
                  ) as c on a.BusinessId = c.BusinessId        
where a.status=0 and a.IsEnable=1
and ( b.IsBind = 0
              or ( b.IsBind = 1
                   and DATEDIFF(minute, a.PubDate, GETDATE()) > {2}
                 )
              or c.BusinessId is not null
            )
and  geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint)<= @PushRadius
order by geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint) asc
", model.TopNum, model.ClienterId, model.ExclusiveOrderTime);
            }
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Latitude", model.Latitude);
            dbParameters.AddWithValue("Longitude", model.Longitude);
            dbParameters.AddWithValue("PushRadius", ParseHelper.ToInt(model.PushRadius) * 1000);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                return TranslateGetJobC(dt, model);
            }
            return new List<GetJobCDM>();
        }

        /// <summary>
        /// 骑士端获取任务列表雇主任务 add by caoheyang 20150519
        /// </summary>
        /// <param name="model">订单查询实体</param>
        /// <returns></returns>
        public IList<GetJobCDM> GetEmployerJobC(GetJobCPM model)
        {
            string sql = string.Format(@"
declare @cliernterPoint geography ;
select @cliernterPoint=geography::Point(@Latitude,@Longitude,4326) ;
select top {0}  a.BusinessId, a.Id,a.OrderCommission,a.OrderCount,   
(a.Amount+a.OrderCount*a.DistribSubsidy) as Amount,a.Platform,a.Weight,a.KM,a.TakeType, convert(varchar(16),a.SongCanDate,120) SongCanDate,
 a.Amount CpAmount,b.Id BusinessId,a.PubName,
a.Remark,a.OrderNo,'' ExpectedTakeTime,
b.Name as BusinessName,b.City as BusinessCity,b.Address as BusinessAddress,
ISNULL(a.ReceviceCity,'') as UserCity, case  isnull(a.ReceviceAddress,'')  
		when  '' then '附近3公里左右，由商户指定'
		else a.ReceviceAddress end as  UserAddress,
ISNULL(b.Longitude,0) as  Longitude,ISNULL(b.Latitude,0) as Latitude,
case convert(varchar(100), PubDate, 23) 
	when convert(varchar(100), getdate(), 23) then '今日 '
    else substring(convert(varchar(100), PubDate, 23),6,5) 
end
+'  '+substring(convert(varchar(100),PubDate,24),1,5)
as PubDate,
 case  a.Platform
        when 3 then
			round(geography::Point(ISNULL(a.Pickuplatitude,0),ISNULL(a.Pickuplongitude,0),4326).STDistance(@cliernterPoint),0) 
		else  
		round(geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint),0) 
       end  DistanceToBusiness
from dbo.[order] a (nolock)
join dbo.business b (nolock) on a.businessId=b.Id
join (select  distinct(temp.BusinessId) from BusinessClienterRelation  temp where temp.IsEnable=1 and  temp.IsBind =1 and temp.ClienterId=@ClienterId ) as c on a.BusinessId=c.BusinessId
where a.status=0 and a.IsEnable=1
and a.Platform!=3 --店内任务不显示闪送订单
order by a.id desc 
", model.TopNum);
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Latitude", model.Latitude);
            dbParameters.AddWithValue("Longitude", model.Longitude);
            dbParameters.AddWithValue("ClienterId", model.ClienterId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                return TranslateGetJobC(dt, model);
            }
            return new List<GetJobCDM>();
        }

        /// <summary>
        /// 骑士端获取所有待抢单任务 add by caoheyang 20150610    TODO 方案搁置
        /// </summary>
        /// <param name="model">订单查询实体</param>
        /// <returns></returns>
        public IList<GetJobCDMRedis> GetJobC()
        {
            return null;
        }

        /// <summary>
        ///  骑士端获取任务列表（最新/最近）任务   add by caoheyang 20150519
        /// </summary>
        /// <param name="dt">datatable</param>
        /// <returns></returns>
        private IList<GetJobCDM> TranslateGetJobC(DataTable dt, GetJobCPM parmModel)
        {
            IList<GetJobCDM> models = new List<GetJobCDM>();
            DeliveryCompanyModel deliveryModel = null;
            if (parmModel.ExpressId > 0)
            {
                deliveryModel = new DeliveryCompanyDao().GetById(parmModel.ExpressId);
            }

            foreach (DataRow dataRow in dt.Rows)
            {
                #region 获取物流公司应付骑士佣金  2015年8月13日 09:34:02 窦海超

                decimal orderCommission = ParseHelper.ToDecimal(dataRow["OrderCommission"]);
                if (deliveryModel != null)
                {
                    //SettleType=结算类型（1结算比例、2固定金额）
                    if (deliveryModel.SettleType == 1)
                    {
                        //订单金额/骑士结算比例值*订单数量
                        orderCommission = deliveryModel.ClienterSettleRatio == 0 ? 0 : ParseHelper.ToDecimal(dataRow["CpAmount"]) * deliveryModel.ClienterSettleRatio / 100;// *ParseHelper.ToInt(dataRow["OrderCount"]);
                    }
                    else if (deliveryModel.SettleType == 2)
                    {
                        //骑士固定金额值*订单数量 
                        orderCommission = deliveryModel.ClienterFixMoney == 0 ? 0 : deliveryModel.ClienterFixMoney * ParseHelper.ToInt(dataRow["OrderCount"]);
                    }
                }
                #endregion


                GetJobCDM temp = new GetJobCDM();
                temp.Id = ParseHelper.ToInt(dataRow["Id"]);
                temp.PubDate = dataRow["PubDate"].ToString();
                temp.OrderCommission = orderCommission;// ParseHelper.ToDecimal(dataRow["OrderCommission"]);
                temp.OrderCount = ParseHelper.ToInt(dataRow["OrderCount"]);
                temp.Amount = ParseHelper.ToDecimal(dataRow["Amount"]);
                temp.BusinessName = dataRow["BusinessName"].ToString();
                temp.BusinessCity = dataRow["BusinessCity"].ToString();
                temp.Longitude = ParseHelper.ToDecimal(dataRow["Longitude"]);
                temp.Latitude = ParseHelper.ToDecimal(dataRow["Latitude"]);
                temp.BusinessAddress = dataRow["BusinessAddress"] == null
                    ? ""
                    : dataRow["BusinessAddress"].ToString();
                temp.Remark = dataRow["Remark"] == null ? "" : dataRow["Remark"].ToString();
                temp.UserCity = dataRow["UserCity"] == null ? "" : dataRow["UserCity"].ToString();
                temp.UserAddress = dataRow["UserAddress"] == null ? "" : dataRow["UserAddress"].ToString();
                int distanceToBusiness = ParseHelper.ToInt(dataRow["DistanceToBusiness"], 0);
                temp.DistanceToBusiness = distanceToBusiness < 1000
                    ? distanceToBusiness + "m"
                    : Math.Round(distanceToBusiness * 0.001, 2) + "km";
                temp.Platform = ParseHelper.ToInt(dataRow["Platform"]);//来源（默认1、旧后台，2新后台）
                temp.Weight = ParseHelper.ToFloat(dataRow["Weight"]);//订单总重量
                temp.KM = ParseHelper.ToFloat(dataRow["KM"]).ToString("f1");//送餐距离
                temp.TakeType = ParseHelper.ToInt(dataRow["TakeType"]);// 取货状态默认0立即，1预约
                temp.SongCanDate = dataRow["SongCanDate"].ToString();
                temp.ExpectedTakeTime = dataRow["ExpectedTakeTime"].ToString();
                temp.OrderNo = dataRow["OrderNo"].ToString();
                temp.BusinessId = ParseHelper.ToInt(dataRow["BusinessId"], 0);
                temp.PubName = dataRow["PubName"].ToString();
                models.Add(temp);
            }
            return models;
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150701</UpdateTime>
        public int UpdateTake(OrderPM modelPM)
        {
            // string clienterTrueName = "";
            //clienter c = clienterDao.GetById(modelPM.ClienterId);
            // if (c != null)
            // {
            //     clienterTrueName = c.TrueName;
            // }
            //            string updateSql = string.Format(@"
            //begin 
            //declare @ClienterTrueName nvarchar(40)
            //SELECT @ClienterTrueName=TrueName FROM dbo.clienter(nolock) where id = @clienterId;
            //update dbo.[Order] 
            //    set Status=4 
            //output Inserted.Id,GETDATE(),@ClienterTrueName,'确认已取货',Inserted.clienterId,Inserted.[Status],{0} 
            //into dbo.OrderSubsidiesLog(OrderId,InsertTime,OptName,Remark,OptId,OrderStatus,[Platform]) 
            //where id=@orderid and Status=2 and clienterId=@clienterId; 
            //update OrderOther 
            //    set TakeTime=GETDATE(),TakeLongitude=@TakeLongitude,TakeLatitude=@TakeLatitude 
            //where orderid=@orderid; 
            //end ", (int)SuperPlatform.FromClienter);
            string updateSql = @"
begin 
declare @ClienterTrueName nvarchar(40)
SELECT @ClienterTrueName=TrueName FROM dbo.clienter(nolock) where id = @clienterId;
update dbo.[Order] 
    set Status=4 where id =@orderId and Status=2 and clienterId=@clienterId; 
 if(@@error<>0 or @@rowcount<>1)--如果到店取货行影响不等于1则返回取货不成功
 begin 
   rollback
   return
 end
update OrderOther 
    set TakeTime=GETDATE(),TakeLongitude=@TakeLongitude,TakeLatitude=@TakeLatitude,IsTakeTimely=@IsTakeTimely 
where orderid=@orderid; 
insert into dbo.OrderSubsidiesLog ( Price, InsertTime, OptName, Remark,
                                     OrderId, OptId, OrderStatus, Platform )
select OrderCommission,getdate(),@ClienterTrueName,'确认已取货',Id,@clienterId,4,@Platform from dbo.[order] o(nolock) where id =@orderId   
end
            ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("orderId", DbType.Int32, 4).Value = modelPM.OrderId;
            dbParameters.AddWithValue("TakeLongitude", modelPM.longitude);
            dbParameters.AddWithValue("TakeLatitude", modelPM.latitude);
            dbParameters.AddWithValue("IsTakeTimely", modelPM.IsTimely);
            dbParameters.AddWithValue("clienterId", modelPM.ClienterId);
            dbParameters.AddWithValue("Platform", SuperPlatform.FromClienter.GetHashCode());
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }
        /// <summary>
        /// 获取任务支付状态（0：未支付 1：部分支付 2：已支付）
        /// danny-20150519
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int GetOrderTaskPayStatus(int orderId)
        {
            string sql = @"  
SELECT CASE SUM(oc.PayStatus) 
			WHEN 0 
			THEN 0 
		ELSE 
			CASE 
				WHEN  SUM(oc.PayStatus)=COUNT(oc.PayStatus) 
				THEN 2 
				ELSE 1 
			END 
		END PayStatus
  FROM OrderChild oc WITH(NOLOCK)
  WHERE OrderId=@OrderId ;";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OrderId", orderId);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm));
        }

        /// <summary>
        /// 根据订单id查询订单主表基本信息  add by caoheyang 20150521
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="businessId">订单id</param>
        /// <param name="status">订单状态</param>
        /// <returns></returns>
        public order GetOrderById(int orderId, int businessId, int? status = null)
        {
            order order = null;
            string sql = @"select  o.Id ,
        o.OrderNo ,o.Status,
        o.SettleMoney ,isnull(o.GroupBusinessId,0) as GroupBusinessId 
from    [order] o ( nolock )
        join dbo.business b ( nolock ) on o.businessId = b.Id
where   o.Id = @OrderId
        and o.businessId = @BusinessId ";
            if (status != null)
            {
                sql = sql + " and o.status=" + status;
            }
            IDbParameters parm = DbHelper.CreateDbParameters("OrderId", DbType.Int32, 4, orderId);
            parm.Add("BusinessId", SqlDbType.Int).Value = businessId;
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
            if (DataTableHelper.CheckDt(dt))
            {
                order = DataTableHelper.ConvertDataTableList<order>(dt)[0];
            }
            return order;
        }

        /// <summary>
        /// 根据订单号查询订单主表基本信息  add by caoheyang 20150521
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="businessId">订单id</param>
        /// <param name="status">订单状态</param>
        /// <returns></returns>
        public order GetOrderMainByNo(string orderNo, int? businessId = null, int? status = null)
        {
            order order = null;
            string sql = @" select * from [order] where orderno=@OrderNo";
            if (businessId != null)
            {
                sql = sql + "  and businessId=@BusinessId" + businessId;
            }
            if (status != null)
            {
                sql = sql + " and status=" + status;
            }
            IDbParameters parm = DbHelper.CreateDbParameters("OrderNo", SqlDbType.NVarChar, 90, orderNo);
            parm.Add("BusinessId", SqlDbType.Int).Value = businessId;
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
            if (DataTableHelper.CheckDt(dt))
            {
                order = DataTableHelper.ConvertDataTableList<order>(dt)[0];
            }
            return order;
        }


        /// <summary>
        /// 订单取消返回商家应收和插入商家余额流水
        /// danny-2015051921
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool OrderCancelReturnBusiness(OrderListModel model)
        {
            string sql = string.Format(@" 
update b
set    b.BalancePrice=ISNULL(b.BalancePrice, 0)+@Amount,
       b.AllowWithdrawPrice=ISNULL(b.AllowWithdrawPrice,0)+@Amount
OUTPUT
  Inserted.Id,
  @Amount,
  @Status,
  Inserted.BalancePrice,
  @RecordType,
  @Operator,
  getdate(),
  @WithwardId,
  @RelationNo,
  @Remark
INTO BusinessBalanceRecord
  ( [BusinessId]
   ,[Amount]
   ,[Status]
   ,[Balance]
   ,[RecordType]
   ,[Operator]
   ,[OperateTime]
   ,[WithwardId]
   ,[RelationNo]
   ,[Remark])
from business b
where b.Id=@BusinessId;");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Amount", model.SettleMoney);
            parm.AddWithValue("@Status", BusinessBalanceRecordStatus.Success);
            parm.AddWithValue("@RecordType", BusinessBalanceRecordRecordType.CancelOrder);
            parm.AddWithValue("@Operator", model.OptUserName);
            parm.AddWithValue("@WithwardId", model.Id);
            parm.AddWithValue("@RelationNo", model.OrderNo);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@BusinessId", model.businessId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 获得配送数据列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRows"></param>
        /// <returns></returns>
        public DataTable DistributionAnalyze(OrderDistributionAnalyze model, int pageIndex, int pageSize, out int totalRows)
        {
            string fields = @"o.Id,
			    OrderNo,
			    businessId,
			    clienterId,
			    PickUpAddress,
			    ReceviceAddress,
			    ReceviceCity ,
			    PubDate,
			    ActualDoneDate,
			    OrderCount,
			    TakeTime,
			    GrabTime,
			    OrderCommission";

            string sql = @"SELECT {0} FROM [order](nolock) o JOIN [OrderOther](nolock) oth ON o.Id = oth.OrderId WHERE o.Status=1 ";

            string dataSql = string.Format(sql, " TOP " + pageSize + "  " + fields);

            string notTopSql = "SELECT MIN(t.Id) FROM (" + string.Format(sql, " TOP " + ((pageIndex - 1) * pageSize).ToString() + " o.Id ");
            string countSql = string.Format(sql, " COUNT(*) ");
            if (pageIndex > 1)
            {
                dataSql += " AND o.Id <({0})";
            }

            IDbParameters parm = DbHelper.CreateDbParameters();

            string[] dataRange = model.GetDataRange();
            if (dataRange.Length > 1)
            {
                if (!string.IsNullOrWhiteSpace(dataRange[0]))
                {
                    dataSql += " and o.FinishDateLength>@FinishDateLength1";
                    notTopSql += " and o.FinishDateLength>@FinishDateLength1";
                    countSql += " and o.FinishDateLength>@FinishDateLength1";
                    parm.Add("FinishDateLength1", DbType.Int32, 4).Value = dataRange[0];
                }
                if (!string.IsNullOrWhiteSpace(dataRange[1]))
                {
                    dataSql += " and o.FinishDateLength<@FinishDateLength2";
                    notTopSql += " and o.FinishDateLength<@FinishDateLength2";
                    countSql += " and o.FinishDateLength<@FinishDateLength2";
                    parm.Add("FinishDateLength2", DbType.Int32, 4).Value = dataRange[1];

                }
            }
            string[] grabToComplete = model.GetGrabToComplete();
            if (grabToComplete.Length > 1)
            {
                if (!string.IsNullOrWhiteSpace(grabToComplete[0]))
                {
                    dataSql += " and oth.GrabToCompleteDistance>@GrabToCompleteDistance1";
                    notTopSql += " and oth.GrabToCompleteDistance>@GrabToCompleteDistance1";
                    countSql += " and oth.GrabToCompleteDistance>@GrabToCompleteDistance1";
                    parm.Add("GrabToCompleteDistance1", DbType.Int32, 4).Value = grabToComplete[0];
                }
                if (!string.IsNullOrWhiteSpace(grabToComplete[1]))
                {
                    dataSql += " and oth.GrabToCompleteDistance<@GrabToCompleteDistance2";
                    notTopSql += " and oth.GrabToCompleteDistance<@GrabToCompleteDistance2";
                    countSql += " and oth.GrabToCompleteDistance<@GrabToCompleteDistance2";
                    parm.Add("GrabToCompleteDistance2", DbType.Int32, 4).Value = grabToComplete[1];
                }
            }
            string[] pubToComplete = model.GetPubToComplete();
            if (pubToComplete.Length > 1)
            {
                if (!string.IsNullOrWhiteSpace(pubToComplete[0]))
                {
                    dataSql += " and oth.PubToCompleteDistance>@PubToCompleteDistance1";
                    notTopSql += " and oth.PubToCompleteDistance>@PubToCompleteDistance1";
                    countSql += " and oth.PubToCompleteDistance>@PubToCompleteDistance1";
                    parm.Add("PubToCompleteDistance1", DbType.Int32, 4).Value = pubToComplete[0];

                }
                if (!string.IsNullOrWhiteSpace(pubToComplete[1]))
                {
                    dataSql += " and oth.PubToCompleteDistance<@PubToCompleteDistance2";
                    notTopSql += " and oth.PubToCompleteDistance<@PubToCompleteDistance2";
                    countSql += " and oth.PubToCompleteDistance<@PubToCompleteDistance2";
                    parm.Add("PubToCompleteDistance2", DbType.Int32, 4).Value = pubToComplete[1];

                }
            }
            string[] pubToGrab = model.GetPubToGrabDistance();
            if (pubToGrab.Length > 1)
            {
                if (!string.IsNullOrWhiteSpace(pubToGrab[0]))
                {
                    dataSql += " and oth.PubToGrabDistance>@PubToGrabDistance1";
                    notTopSql += " and oth.PubToGrabDistance>@PubToGrabDistance1";
                    countSql += " and oth.PubToGrabDistance>@PubToGrabDistance1";
                    parm.Add("PubToGrabDistance1", DbType.Int32, 4).Value = pubToGrab[0];
                }
                if (!string.IsNullOrWhiteSpace(pubToGrab[1]))
                {
                    dataSql += " and oth.PubToGrabDistance<@PubToGrabDistance2";
                    notTopSql += " and oth.PubToGrabDistance<@PubToGrabDistance2";
                    countSql += " and oth.PubToGrabDistance<@PubToGrabDistance2";
                    parm.Add("PubToGrabDistance2", DbType.Int32, 4).Value = pubToGrab[1];

                }
            }
            if (model.StartDate != null)
            {
                dataSql += " and o.ActualDoneDate>=@StartDate";
                notTopSql += " and o.ActualDoneDate>=@StartDate";
                countSql += " and o.ActualDoneDate>=@StartDate";
                parm.Add("StartDate", DbType.DateTime).Value = model.StartDate.Value.Date.ToString();

            }
            if (model.EndDate != null)
            {
                dataSql += " and o.ActualDoneDate<=@EndDate";
                notTopSql += " and o.ActualDoneDate<=@EndDate";
                countSql += " and o.ActualDoneDate<=@EndDate";
                parm.Add("EndDate", DbType.DateTime).Value = model.EndDate.Value.Date.ToString();

            }
            if (!string.IsNullOrWhiteSpace(model.City))
            {
                dataSql += " and o.ReceviceCity=@ReceiveCity";
                notTopSql += " and o.ReceviceCity=@ReceiveCity";
                countSql += " and o.ReceviceCity=@ReceiveCity";
                parm.Add("ReceiveCity", DbType.String, 45).Value = model.City;
            }
            dataSql += " order by oth.id desc";
            notTopSql += " order by oth.id desc ) AS t";

            if (pageIndex > 1)
            {
                dataSql = string.Format(dataSql, notTopSql);
            }

            object count = DbHelper.ExecuteScalar(SuperMan_Read, countSql, parm);
            totalRows = Convert.ToInt32(count);

            return DbHelper.ExecuteDataTable(SuperMan_Read, dataSql, parm);
        }

        /// <summary>
        /// 订单中所有城市去重列表
        /// </summary>
        /// <returns></returns>
        public IList<string> OrderReceviceCity()
        {
            string sql = "select distinct  ReceviceCity from [order](nolock) where ReceviceCity is not null";

            IList<string> cities = new List<string>();
            IDataReader reader = null;
            try
            {
                reader = DbHelper.ExecuteReader(SuperMan_Read, sql);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        string city = reader.GetString(0);
                        cities.Add(city);
                    }
                }
            }
            catch { }
            finally { reader.Close(); }

            return cities;
        }

        /// <summary>
        /// 更新已提现
        /// </summary>
        /// <param name="orderId"></param>
        public bool UpdateFinishAll(int orderId)
        {
            const string updateSql = @"
update  [order]
set FinishAll = 1 
where   Id = @OrderId and FinishAll = 0";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("OrderId", DbType.Int32, 4).Value = orderId;
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters) == 1 ? true : false;
        }

        /// <summary>
        /// 更新是否已付款
        /// </summary>
        /// <param name="orderId"></param>
        public bool UpdateIsPay(int orderId)
        {
            const string updateSql = @"
update  [order]
set IsPay = 1,Status=0
where   Id = @OrderId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("OrderId", DbType.Int32, 4).Value = orderId;
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters) == 1 ? true : false;
        }
        /// <summary>
        /// 更新是否已付款
        /// </summary>
        /// <param name="orderId"></param>
        public bool UpdateTipAmount(int orderId, decimal tipAmount)
        {
            const string updateSql = @"
update  [order]
set  TipAmount=TipAmount+@TipAmount
where   Id = @OrderId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("TipAmount", tipAmount);
            dbParameters.Add("OrderId", DbType.Int32, 4).Value = orderId;
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters) == 1 ? true : false;
        }
        /// <summary>
        /// 付款方式
        /// </summary>
        /// <param name="orderId"></param>
        public bool UpdatePayment(int orderId, int payment)
        {
            const string updateSql = @"
update  [order]
set  Payment=@Payment
where   Id = @OrderId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Payment", payment);
            dbParameters.Add("OrderId", DbType.Int32, 4).Value = orderId;
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters) == 1 ? true : false;
        }
        /// <summary>
        /// 根据orderID获取订单地图数据
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public OrderMapDetail GetOrderMapDetail(long orderID)
        {

            string sql = @" 
                            SELECT  ord.OrderId,
                                    isnull(c.Longitude,0) PubLongitude,
                                    isnull(c.Latitude,0) PubLatitude,           
                                    ISNULL(ab.PubDate, '') AS PubDate,
                                    ISNULL(GrabLongitude, 0) AS GrabLongitude,
                                    ISNULL(GrabLatitude, 0) AS GrabLatitude,
                                    ISNULL(GrabTime, '') AS GrabTime,
                                    ISNULL(TakeLongitude, 0) AS TakeLongitude ,
                                    ISNULL(TakeLatitude, 0) AS TakeLatitude,
                                    ISNULL(TakeTime, '') AS TakeTime,
                                    ISNULL(CompleteLongitude, 0) AS CompleteLongitude,
                                    ISNULL(CompleteLatitude, 0) AS CompleteLatitude,
                                    ISNULL(ab.ActualDoneDate, '') AS ActualDoneDate,
                                    CASE ISNULL(ord.GrabLatitude, 0)
                                        WHEN 0 THEN -1
                                        ELSE CASE ISNULL(ord.CompleteLatitude, 0)
                                                WHEN 0 THEN -1
                                                ELSE ord.GrabToCompleteDistance
                                            END
                                    END AS GrabToCompleteDistance,
                                    ISNULL(ord.IsPubDateTimely, 0 ) as IsPubDateTimely,
                                    ISNULL(ord.IsGrabTimely, 0) as IsGrabTimely,
                                    ISNULL(ord.IsTakeTimely, 0) as IsTakeTimely ,
                                    ISNULL(ord.IsCompleteTimely, 0) as IsCompleteTimely,
                                    ISNULL(ab.clienterId, 0) clienterId
                            FROM  [order] (NOLOCK) ab  
                                    JOIN OrderOther (NOLOCK) ord ON ord.OrderId = ab.Id
                                    JOIN business (NOLOCK) c ON c.id = ab.businessId 
                            WHERE   ab.Id = @orderID
                            ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@orderID", orderID);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, dbParameters);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<OrderMapDetail>(dt)[0];
        }

        /// <summary>
        /// 根据orderID获取订单地图数据
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public OrderMapDetail GetOrderMapDetail(string orderNo)
        {

            string sql = @" 
                            SELECT  ord.OrderId,
                                    isnull(c.Longitude,0) PubLongitude,
                                    isnull(c.Latitude,0) PubLatitude,           
                                    ISNULL(ab.PubDate, '') AS PubDate,
                                    ISNULL(GrabLongitude, 0) AS GrabLongitude,
                                    ISNULL(GrabLatitude, 0) AS GrabLatitude,
                                    ISNULL(GrabTime, '') AS GrabTime,
                                    ISNULL(TakeLongitude, 0) AS TakeLongitude ,
                                    ISNULL(TakeLatitude, 0) AS TakeLatitude,
                                    ISNULL(TakeTime, '') AS TakeTime,
                                    ISNULL(CompleteLongitude, 0) AS CompleteLongitude,
                                    ISNULL(CompleteLatitude, 0) AS CompleteLatitude,
                                    ISNULL(ab.ActualDoneDate, '') AS ActualDoneDate,
                                    CASE ISNULL(ord.GrabLatitude, 0)
                                        WHEN 0 THEN -1
                                        ELSE CASE ISNULL(ord.CompleteLatitude, 0)
                                                WHEN 0 THEN -1
                                                ELSE ord.GrabToCompleteDistance
                                            END
                                    END AS GrabToCompleteDistance,
                                    ISNULL(ord.IsPubDateTimely, 0 ) as IsPubDateTimely,
                                    ISNULL(ord.IsGrabTimely, 0) as IsGrabTimely,
                                    ISNULL(ord.IsTakeTimely, 0) as IsTakeTimely ,
                                    ISNULL(ord.IsCompleteTimely, 0) as IsCompleteTimely,
                                    ISNULL(ab.clienterId, 0) clienterId
                            FROM  [order] (NOLOCK) ab  
                                    JOIN OrderOther (NOLOCK) ord ON ord.OrderId = ab.Id
                                    JOIN business (NOLOCK) c ON c.id = ab.businessId 
                            WHERE   ab.orderNo = @OrderNo
                            ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@OrderNo", orderNo);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, dbParameters);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<OrderMapDetail>(dt)[0];
        }

        /// <summary>
        /// 一键发单修改地址和电话
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="newAddress"></param>
        /// <param name="newPhone"></param>
        /// <returns></returns>
        public int UpdateOrderAddressAndPhone(string orderId, string newAddress, string newPhone)
        {
            string sql = @" select OneKeyPubOrder from  [OrderOther] (nolock) where orderid=@orderId";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@OrderId", orderId);
            object obj = DbHelper.ExecuteScalar(SuperMan_Read, sql, dbParameters);
            if (ParseHelper.ToInt(obj, -1) == 1)
            {
                sql = @" update [order] set ReceviceAddress=@ReceviceAddress,RecevicePhoneNo=@RecevicePhoneNo where id=@orderId";
                dbParameters.AddWithValue("@ReceviceAddress", newAddress);
                dbParameters.AddWithValue("@RecevicePhoneNo", newPhone);
                int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
                return i;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 更新订单是真实佣金
        /// zhaohailong20150706
        /// 修改人：胡灵波
        /// 2015年8月25日 16:28:18
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="realOrderCommission"></param>
        /// <returns></returns>
        public int UpdateOrderRealCommission(OrderOtherPM orderOtherPM)
        {
            string sql = @" update [Order] set RealOrderCommission=@realOrderCommission where id=@orderId";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@realOrderCommission", orderOtherPM.RealOrderCommission);
            dbParameters.AddWithValue("@OrderId", orderOtherPM.OrderId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
        }

        /// <summary>
        /// 骑士目前是否有未完成的订单
        /// add by 彭宜   20150714
        /// </summary>
        /// <param name="clienterId">骑士Id</param>
        /// <returns></returns>
        public bool ClienterHasUnFinishedOrder(int clienterId)
        {
            string sql = "select count(1) from [dbo].[order]  (nolock) where clienterId=@clientId and (Status=2 or Status=4);";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@clientId", DbType.Int32, 4).Value = clienterId;
            return int.Parse(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm).ToString()) > 0;
        }

        /// <summary>
        /// 根据两个点的经纬度计算两点之间的距离
        /// </summary>
        /// <param name="firstLatitude"></param>
        /// <param name="firstLongitude"></param>
        /// <param name="secondLatitude"></param>
        /// <param name="secondLongitude"></param>
        /// <returns></returns>
        public int GetDistanceByPoint(double firstLatitude, double firstLongitude, double secondLatitude, double secondLongitude)
        {

            string sql = @"select (round(
                                    [geography]::Point(@firstLatitude,@firstLongitude,4326)
                                    .STDistance(
                                    [geography]::Point(@secondLatitude,@secondLongitude,4326) )
                                    ,0
                                    )
                              )";

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@firstLatitude", firstLatitude);
            parm.AddWithValue("@firstLongitude", firstLongitude);
            parm.AddWithValue("@secondLatitude", secondLatitude);
            parm.AddWithValue("@secondLongitude", secondLongitude);
            object obj = DbHelper.ExecuteScalar(SuperMan_Read, sql, parm);
            return ParseHelper.ToInt(obj, 0);
        }

        /// <summary>
        /// 更新物流公司的订单的佣金数据
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="orderCommission"></param>
        /// <param name="deliveryCompanySettleMoney"></param>
        /// <param name="deliveryCompanyID"></param>
        /// <returns></returns>
        public int UpdateDeliveryCompanyOrderCommssion(string orderID, decimal orderCommission, decimal deliveryCompanySettleMoney, int deliveryCompanyID)
        {
            string sql = @" update [Order] set OrderCommission=@OrderCommission,
                                                DeliveryCompanySettleMoney=@DeliveryCompanySettleMoney,
                                                DeliveryCompanyID=@DeliveryCompanyID 
                            where id=@orderId";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@OrderCommission", DbType.Decimal).Value = orderCommission;
            dbParameters.Add("@DeliveryCompanySettleMoney", DbType.Decimal).Value = deliveryCompanySettleMoney;
            dbParameters.Add("@DeliveryCompanyID", DbType.Int32).Value = deliveryCompanyID;
            dbParameters.Add("@OrderId", DbType.Int32).Value = ParseHelper.ToInt(orderID, 0);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
        }
        /// <summary>
        /// 获取骑士今天已完成(或完成后又取消了)的非物流公司的订单数量(不是任务数量)
        /// </summary>
        /// <param name="clienterID"></param>
        /// <returns></returns>
        public int GetTotalOrderNumByClienterID(int clienterID, DateTime actualDoneDate)
        {
            string sql = @"SELECT  ISNULL(SUM(OrderCount), 0) AS num
                            FROM    [order] (NOLOCK)
                            WHERE   clienterId = @clienterId
                                    AND ActualDoneDate IS NOT NULL
                                    AND ActualDoneDate >= @beginDate
                                    AND ActualDoneDate < @endDate";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("clienterId", DbType.Int32).Value = clienterID;
            dbParameters.Add("beginDate", DbType.DateTime).Value = actualDoneDate.Date.ToString();
            dbParameters.Add("endDate", DbType.DateTime).Value = actualDoneDate.ToString();

            object obj = DbHelper.ExecuteScalar(SuperMan_Read, sql, dbParameters);
            return ParseHelper.ToInt(obj, 0);
        }
        /// <summary>
        /// 查询分页的活跃用户list
        /// </summary>
        /// <param name="recommendQuery"></param>
        /// <returns></returns>
        public PageInfo<ActiveUserInfo> GetActiveUserList(ActiveUserQuery recommendQuery)
        {
            string tableList = "";
            string whereCondition = "1=1";
            string orderByColumns = " TaskNum DESC ,OrderNum DESC";
            if (recommendQuery.UserType == 0)
            {
                tableList = getClienterTableList(recommendQuery);
            }
            else
            {
                tableList = getBusinessTableList(recommendQuery);
            }

            string columnList = @"   UserID
                                    ,UserName
                                    ,PhoneNo
                                    ,TaskNum
                                    ,OrderNum
                                    ";

            return new PageHelper().GetPages<ActiveUserInfo>(SuperMan_Read, recommendQuery.PageIndex, whereCondition, orderByColumns, columnList, tableList, SystemConst.PageSize, true);
        }
        /// <summary>
        /// 返回活跃商家的查询sql
        /// </summary>
        /// <param name="recommendQuery"></param>
        /// <returns></returns>
        private string getBusinessTableList(ActiveUserQuery recommendQuery)
        {
            string businessSql = @"(SELECT  b.id as UserID,
                                        b.name as UserName,
                                        b.PhoneNo as PhoneNo,
                                        COUNT(a.id) AS TaskNum ,
                                        SUM(a.ordercount) AS OrderNum
                                FROM    dbo.[order] (NOLOCK) a
                                        JOIN dbo.business (NOLOCK) b ON a.businessId = b.Id
                                WHERE   a.PubDate IS NOT NULL {0}
                                GROUP BY b.id ,
                                        b.name ,
                                        b.PhoneNo) mp";
            StringBuilder sb = new StringBuilder();
            if (recommendQuery.StartDate != DateTime.MinValue)
            {
                sb.Append(string.Format(" and a.PubDate>='{0}'", recommendQuery.StartDate.ToString("yyyy-MM-dd")));
            }
            if (recommendQuery.EndDate != DateTime.MinValue)
            {
                sb.Append(string.Format(" and a.PubDate<'{0}'", recommendQuery.EndDate.AddDays(1).ToString("yyyy-MM-dd")));
            }
            if (!string.IsNullOrEmpty(recommendQuery.UserInfo))
            {
                if (recommendQuery.InfoType == 0)//注册手机号
                {
                    sb.Append(string.Format(" and b.PhoneNo='{0}'", recommendQuery.UserInfo));
                }
                else
                {
                    sb.Append(string.Format(" and b.name='{0}'", recommendQuery.UserInfo));
                }
            }
            return string.Format(businessSql, sb.ToString());

        }
        /// <summary>
        /// 返回活跃骑士的查询sql
        /// </summary>
        /// <param name="recommendQuery"></param>
        /// <returns></returns>
        private string getClienterTableList(ActiveUserQuery recommendQuery)
        {
            string clienterSql = @"(SELECT  b.id as UserID,
                                        b.truename as UserName,
                                        b.PhoneNo as PhoneNo,
                                        COUNT(a.id) AS TaskNum ,
                                        SUM(a.ordercount) AS OrderNum
                                FROM    dbo.[order] (NOLOCK) a
                                        JOIN dbo.clienter (NOLOCK) b ON a.clienterId = b.Id
                                WHERE   a.PubDate IS NOT NULL {0}
                                GROUP BY b.id ,
                                        b.truename ,
                                        b.PhoneNo) mp";
            StringBuilder sb = new StringBuilder();
            if (recommendQuery.StartDate != DateTime.MinValue)
            {
                sb.Append(string.Format(" and a.PubDate>='{0}'", recommendQuery.StartDate.ToString("yyyy-MM-dd")));
            }
            if (recommendQuery.EndDate != DateTime.MinValue)
            {
                sb.Append(string.Format(" and a.PubDate<'{0}'", recommendQuery.EndDate.AddDays(1).ToString("yyyy-MM-dd")));
            }
            if (!string.IsNullOrEmpty(recommendQuery.UserInfo))
            {
                if (recommendQuery.InfoType == 0)//注册手机号
                {
                    sb.Append(string.Format(" and b.PhoneNo='{0}'", recommendQuery.UserInfo));
                }
                else
                {
                    sb.Append(string.Format(" and b.truename='{0}'", recommendQuery.UserInfo));
                }
            }
            return string.Format(clienterSql, sb.ToString());
        }
        /// <summary>
        /// 根据订单号查订单信息
        /// danny-20150320
        /// </summary>
        /// <returns></returns>
        public IList<OrderListModel> GetDealOverTimeOrderList()
        {
            #region 查询脚本
            string sql = @"SELECT        o.[Id]
                                        ,o.[Status]
                                        ,o.[OrderCommission]
                                        ,o.[SettleMoney]
                                        ,o.[OrderNo]
                                        ,o.[clienterId]
                                        ,o.[businessId]
                                        ,o.[WebsiteSubsidy]
                                        ,o.[OrderCommission]
                                        ,o.[IsPay]
                                        ,o.FinishAll
                                        ,o.[OrderCount]
                                        ,ISNULL(o.MealsSettleMode,0) MealsSettleMode
                                        ,ISNULL(o.GroupBusinessId,0) GroupBusinessId            
                                        ,ISNULL(oo.IsJoinWithdraw,0) IsJoinWithdraw
                                    FROM [order] o WITH ( NOLOCK )
                                    JOIN OrderOther oo WITH (NOLOCK) ON oo.OrderId=o.Id
                                    JOIN business b with(nolock) on b.Id=o.businessId
                                    WHERE o.PubDate BETWEEN DATEADD(HOUR,-@overTimeHour,Convert(DateTime,Convert(Varchar(10),GetDate(),120))) 
                                    AND Convert(DateTime,Convert(Varchar(10),GetDate(),120))
                                    AND o.FinishAll=0 
                                    AND o.Status<>3 
                                    AND ISNULL(o.DeliveryCompanyID,0)=0 
                                    AND ISNULL(o.OrderFrom,0)=0 
                                    AND ISNULL(b.IsOrderChecked,0)=1";
            #endregion
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@overTimeHour", DbType.Int32).Value = ParseHelper.ToInt(Config.ConfigKey("OverTimeHour"), 24);
            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return ConvertDataTableList<OrderListModel>(dt);
        }
        /// <summary>
        /// 修改订单状态
        /// danny-20150813
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyOrderStatus(OrderListModel model)
        {
            string sql = string.Format(@" UPDATE dbo.[order]
                                             SET    [Status] = @Status
                                            OUTPUT
                                              Inserted.Id,
                                              @Price,
                                              GETDATE(),
                                              @OptId,
                                              @OptName,
                                              Inserted.[Status],
                                              @Platform,
                                              @Remark
                                            INTO dbo.OrderSubsidiesLog
                                              (OrderId,
                                              Price,
                                              InsertTime,
                                              OptId,
                                              OptName,
                                              OrderStatus,
                                              Platform,
                                              Remark)
                                             WHERE  Id = @Id AND Status=@OldStatus ");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Id", model.Id);
            parm.AddWithValue("@Status", model.Status);
            parm.AddWithValue("@OldStatus", model.OldStatus);
            parm.AddWithValue("@Price", model.OrderCommission ?? 0);
            parm.AddWithValue("@OptId", 0);
            parm.AddWithValue("@OptName", "服务");
            parm.AddWithValue("@Platform", 2);
            parm.AddWithValue("@Remark", "服务自动处理超时未完成订单");
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }

        /// <summary>
        /// 自动审核拒绝处理
        /// danny-20150813
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        public bool AutoAuditRefuseDeal(int orderId)
        {
            const string sql = @"
update OrderOther
set  IsJoinWithdraw=@IsJoinWithdraw
    ,AuditStatus = @Auditstatus
    ,DeductCommissionReason=@DeductCommissionReason
    ,DeductCommissionType=@DeductCommissionType
where orderId=@orderId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@orderId", orderId);
            dbParameters.AddWithValue("@IsJoinWithdraw", 1);
            dbParameters.AddWithValue("@Auditstatus", OrderAuditStatusCommon.Refuse.GetHashCode());
            dbParameters.AddWithValue("@DeductCommissionType", 1);
            dbParameters.AddWithValue("@DeductCommissionReason", "未在规定时间段内完成上传小票");
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters) > 0;
        }
        /// <summary>
        /// 订单审核拒绝修改订单
        /// danny-20150813
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool OrderAuditRefuseModifyOrder(OrderListModel model)
        {
            string sql = string.Format(@" UPDATE dbo.[order]
                                             SET    FinishAll = @FinishAll,
                                                    RealOrderCommission=@RealOrderCommission
                                            OUTPUT
                                              Inserted.Id,
                                              @Price,
                                              GETDATE(),
                                              @OptId,
                                              @OptName,
                                              Inserted.[Status],
                                              @Platform,
                                              @Remark
                                            INTO dbo.OrderSubsidiesLog
                                              (OrderId,
                                              Price,
                                              InsertTime,
                                              OptId,
                                              OptName,
                                              OrderStatus,
                                              Platform,
                                              Remark)
                                             WHERE  Id = @Id  ");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Id", model.Id);
            parm.AddWithValue("@FinishAll", 1);
            parm.AddWithValue("@RealOrderCommission", model.RealOrderCommission);
            parm.AddWithValue("@Price", model.RealOrderCommission);
            parm.AddWithValue("@OptId", 0);
            parm.AddWithValue("@OptName", "服务");
            parm.AddWithValue("@Platform", 2);
            parm.AddWithValue("@Remark", "服务自动处理超时未完成订单");
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 查询前一天订单审核数据
        /// danny-20150813
        /// </summary>
        /// <returns></returns>
        public OrderAuditStatisticalModel GetOrderAuditStatistical()
        {
            string sql = @" SELECT  ISNULL(COUNT(CASE when oo.AuditStatus=0 AND  o.FinishAll=1 THEN o.Id END),0) UnAuditTaskQty,
		                            ISNULL(SUM(CASE when oo.AuditStatus=0 AND  o.FinishAll=1 THEN o.OrderCount END),0) UnAuditOrderQty,
		                            ISNULL(COUNT(CASE when oo.AuditStatus=0 AND  o.FinishAll=0 THEN o.Id END),0) UnFinishTaskQty,
		                            ISNULL(SUM(CASE when oo.AuditStatus=0 AND  o.FinishAll=0 THEN o.OrderCount END),0) UnFinishOrderQty,
		                            ISNULL(COUNT(CASE when oo.AuditStatus=1 THEN o.Id END),0) AuditOkTaskQty,
		                            ISNULL(SUM(CASE when oo.AuditStatus=1 THEN o.OrderCount END),0) AuditOkOrderQty,
		                            ISNULL(COUNT(CASE when oo.AuditStatus=2 THEN o.Id END),0) AuditRefuseTaskQty,
		                            ISNULL(SUM(CASE when oo.AuditStatus=2 THEN o.OrderCount END),0) AuditRefuseOrderQty
                            FROM [order] o WITH(NOLOCK)
                            JOIN dbo.OrderOther oo WITH(NOLOCK) ON oo.OrderId=o.Id
                            WHERE o.PubDate BETWEEN DATEADD(HOUR,-24,Convert(DateTime,Convert(Varchar(10),GetDate(),120))) 
								AND Convert(DateTime,Convert(Varchar(10),GetDate(),120))";

            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql));
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<OrderAuditStatisticalModel>(dt)[0];
        }

        /// <summary>
        /// 订单自动审核拒绝增加除网站外的金额
        /// danny-20150813
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool OrderAuditRefuseReturnClienter(OrderListModel model)
        {
            string sql = string.Format(@" 
update c
set    c.AccountBalance=ISNULL(c.AccountBalance, 0)+@Amount
      ,c.AllowWithdrawPrice=ISNULL(c.AllowWithdrawPrice, 0)+@Amount
OUTPUT
  Inserted.Id,
  @Amount,
  @Status,
  Inserted.AccountBalance,
  @RecordType,
  @Operator,
  getdate(),
  @WithwardId,
  @RelationNo,
  @Remark
INTO ClienterBalanceRecord
  (  [ClienterId]
    ,[Amount]
    ,[Status]
    ,[Balance]
    ,[RecordType]
    ,[Operator]
    ,[OperateTime]
    ,[WithwardId]
    ,[RelationNo]
    ,[Remark])
from clienter c
where c.Id=@ClienterId;");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Amount", model.RealOrderCommission);
            parm.AddWithValue("@Status", ClienterBalanceRecordStatus.Success);
            parm.AddWithValue("@RecordType", ClienterBalanceRecordRecordType.OrderCommission);
            parm.AddWithValue("@Operator", "服务");
            parm.AddWithValue("@WithwardId", model.Id);
            parm.AddWithValue("@RelationNo", model.OrderNo);
            parm.AddWithValue("@Remark", "服务自动处理超时未完成订单");
            parm.AddWithValue("@ClienterId", model.clienterId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }

        /// <summary>
        /// 添加骑士可提现余额流水记录
        /// danny-20150813
        /// </summary>
        /// <param name="clienterAllowWithdrawRecord"></param>
        /// <returns></returns>
        public bool InsertClienterAllowWithdrawRecord(ClienterAllowWithdrawRecord clienterAllowWithdrawRecord)
        {
            const string insertSql = @"
insert into ClienterAllowWithdrawRecord
            (ClienterId
            ,Amount
            ,Status
            ,Balance
            ,RecordType
            ,Operator
            ,WithwardId
            ,RelationNo
            ,Remark)
select   @ClienterId
        ,@Amount
        ,@Status
        ,c.AllowWithdrawPrice
        ,@RecordType
        ,@Operator
        ,@WithwardId
        ,@RelationNo
        ,@Remark 
from dbo.clienter as c where Id=@ClienterId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ClienterId", clienterAllowWithdrawRecord.ClienterId);//骑士id
            dbParameters.AddWithValue("Amount", clienterAllowWithdrawRecord.Amount);//流水金额
            dbParameters.AddWithValue("Status", clienterAllowWithdrawRecord.Status); //流水状态(1、交易成功 2、交易中）
            dbParameters.AddWithValue("RecordType", clienterAllowWithdrawRecord.RecordType); //交易类型(1佣金 2奖励 3提现 4取消订单赔偿 5无效订单扣款)
            dbParameters.AddWithValue("Operator", clienterAllowWithdrawRecord.Operator); //操作人 
            dbParameters.AddWithValue("WithwardId", clienterAllowWithdrawRecord.WithwardId); //关联ID
            dbParameters.AddWithValue("RelationNo", clienterAllowWithdrawRecord.RelationNo); //关联单号
            dbParameters.AddWithValue("Remark", clienterAllowWithdrawRecord.Remark); //描述
            return DbHelper.ExecuteNonQuery(SuperMan_Write, insertSql, dbParameters) > 0;
        }
        public List<OrderChild> GetOrderChild(string orderNo)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取FinishAll状态错误的订单
        /// 胡灵波
        /// 2015年8月19日 18:49:05
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IList<order> GetFinallErrByClienterId()
        {
            IList<order> list = new List<order>();

            string querysql = @"
select o.Id,o.OrderCount,ISNULL(oo.HadUploadCount, 0) HadUploadCount        
from    dbo.[order] o ( nolock )
        left join dbo.OrderOther oo ( nolock ) on o.Id = oo.OrderId
where   o.Status=1 and oo.HadUploadCount>=o.OrderCount
and FinishAll=0 ";

            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql));
            if (DataTableHelper.CheckDt(dt))
            {
                list = DataTableHelper.ConvertDataTableList<order>(dt);
            }
            return list;
        }
        /// <summary>
        /// 获取推送订单详情
        /// danny-20150818
        /// </summary>
        /// <param name="lastOrderPushTime">上次的推送时间</param>
        /// <returns></returns>
        public IList<OrderListModel> GetPushOrderList(string lastOrderPushTime)
        {
            string sql = @"
SELECT  o.Id,
		o.OrderNo,
		o.businessId,
		b.Latitude BusinessLatitude,
		b.Longitude BusinessLongitude,
        b.IsBind
FROM dbo.[order] o WITH(NOLOCK)
JOIN dbo.business b WITH(NOLOCK) ON o.businessId=b.Id
WHERE o.[Status]=0 AND o.PubDate>=@LastOrderPushTime;
";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@LastOrderPushTime", SqlDbType.NVarChar).Value = lastOrderPushTime;
            var dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            return ConvertDataTableList<OrderListModel>(dt);

        }
        /// <summary>
        /// 添加订单推送记录
        /// danny-20150819
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertOrderPushRecord(OrderPushRecord model)
        {
            const string insertSql = @"
INSERT INTO [OrderPushRecord]
           ([OrderId]
           ,[ClienterIdList]
           ,[TaskType]
           ,[PushCount]
           ,[ClienterCount])
     VALUES
           (@OrderId
           ,@ClienterIdList
           ,@TaskType
           ,@PushCount
           ,@ClienterCount);";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@OrderId", model.OrderId);
            dbParameters.AddWithValue("@ClienterIdList", model.ClienterIdList);
            dbParameters.AddWithValue("@TaskType", model.TaskType);
            dbParameters.AddWithValue("@ClienterCount", model.ClienterCount);
            dbParameters.AddWithValue("@PushCount", model.PushCount);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, insertSql, dbParameters) > 0;
        }

        /// <summary>
        /// 编辑订单推送记录
        /// danny-20150819
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool EditOrderPushRecord(OrderPushRecord model)
        {
            string updateSql = @"
MERGE INTO OrderPushRecord opr
	USING(values(@OrderId,@TaskType)) AS oprNew(OrderId,TaskType)
		ON opr.OrderId=oprNew.OrderId AND  opr.TaskType=oprNew.TaskType
	WHEN MATCHED 
	THEN UPDATE 
		 SET opr.PushCount=opr.PushCount+1,
             opr.ClienterCount=opr.ClienterCount+@ClienterCount,
             opr.PushTime=getdate(),
             opr.ClienterIdList=opr.ClienterIdList+'/'+@ClienterIdList
	WHEN NOT MATCHED 
		  THEN INSERT ( [OrderId]
                       ,[ClienterIdList]
                       ,[TaskType]
                       ,[PushCount]
                       ,[ClienterCount])
                 VALUES
                       (@OrderId
                       ,@ClienterIdList
                       ,@TaskType
                       ,@PushCount
                       ,@ClienterCount);";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OrderId", model.OrderId);
            parm.AddWithValue("@ClienterIdList", model.ClienterIdList);
            parm.AddWithValue("@TaskType", model.TaskType);
            parm.AddWithValue("@ClienterCount", model.ClienterCount);
            parm.AddWithValue("@PushCount", model.PushCount);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, parm) > 0;
        }
        /// <summary>
        /// 超时订单-获取列表
        /// 茹化肖
        /// 2015年8月28日10:58:28
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetOverTimeOrderList<T>(OverTimeOrderPM model)
        {
            string columnList = @"
                o.OrderNo ,
        o.id AS OrderID,
        b.id as Bid ,
        b.Name ,
        b.PhoneNo ,
        o.PubDate ,
        DATEDIFF(MINUTE, o.PubDate, GETDATE()) AS OverTime ,
        ISNULL(o.ReceviceAddress, '') AS ReceviceAddress ,
        o.OrderCount ,
        o.Amount,
        b.IsEmployerTask
                ";
            var sbSqlWhere = new StringBuilder(" o.Status = 0 ");
            if (model.OverTime == 5)
            {
                sbSqlWhere.Append("AND DATEDIFF(MINUTE, o.PubDate, GETDATE()) >= 5");
            }
            if (model.OverTime == 10)
            {
                sbSqlWhere.Append("AND DATEDIFF(MINUTE, o.PubDate, GETDATE()) >= 10");
            }
            if (model.OverTime == 20)
            {
                sbSqlWhere.Append("AND DATEDIFF(MINUTE, o.PubDate, GETDATE()) >= 20");
            }
            if (!string.IsNullOrWhiteSpace(model.BusName))
            {
                sbSqlWhere.AppendFormat(" AND b.Name LIKE '%{0}%'", model.BusName);
            }
            string tableList = @" dbo.[order] AS o ( NOLOCK )
        JOIN dbo.OrderOther AS oo ( NOLOCK ) ON o.id = oo.OrderId
        JOIN dbo.business AS b ( NOLOCK ) ON b.Id = o.businessId ";
            string orderByColumn = " o.PubDate ASC";
            return new PageHelper().GetPages<T>(SuperMan_Read, model.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, model.PageSize, true);
        }

        /// <summary>
        /// 获取商户未抢单订单数
        /// danny-20150831
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public OrderListModel GetBusinessUnReceiveOrderQty(int orderId, int businessId)
        {
            string sql = @"
SELECT   b.Name BusinessName
        ,b.PhoneNo BusinessPhoneNo
        ,ISNULL(COUNT(1),0) UnReceiveQty
		,b.Latitude BusinessLatitude
		,b.Longitude BusinessLongitude 
FROM dbo.[order] o WITH(NOLOCK)  
JOIN dbo.business b WITH(NOLOCK) ON o.businessId=b.Id
WHERE o.Status=0 AND o.businessId=@BusinessId
GROUP BY b.Name,b.PhoneNo,b.Latitude,b.Longitude;
";
            var parm = DbHelper.CreateDbParameters();
            parm.Add("@BusinessId", DbType.Int32, 4).Value = businessId;
            parm.Add("@OrderId", DbType.Int32, 4).Value = orderId;
            var dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            var list = ConvertDataTableList<OrderListModel>(dt);
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            return list[0];

        }
        /// <summary>
        /// 根据订单Id获取订单信息
        /// danny-20150831
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public OrderListModel GetOrderInfoById(int orderId)
        {
            string sql = @"
select  top 1
        o.[Id] ,
        o.[OrderNo] ,
        o.[Status] ,
        o.businessId ,
		b.Latitude BusinessLatitude,
		b.Longitude BusinessLongitude,
        b.IsBind     
from    [order] o with ( nolock )
        JOIN dbo.business b WITH(NOLOCK) ON o.businessId=b.Id
where    o.Id = @Id
";
            var parm = DbHelper.CreateDbParameters();
            parm.Add("@Id", DbType.Int32, 4).Value = orderId;
            var dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            var list = ConvertDataTableList<OrderListModel>(dt);
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            return list[0];
        }


        /// <summary>
        /// 新增订单       
        /// </summary>
        /// 胡灵波
        /// 2015年11月18日 16:43:36
        /// <param name="order">订单实体</param>
        /// <returns></returns>
        public int Insert(order order)
        {
            string insertSql = @"
insert  into dbo.[order]
        ( OrderNo , PickUpAddress ,PubDate , ReceviceName , RecevicePhoneNo ,
          ReceviceAddress ,IsPay , Amount , OrderCommission ,       DistribSubsidy ,
          WebsiteSubsidy ,Remark ,OrderFrom , Status , businessId ,
          ReceviceCity ,
          ReceviceLongitude ,
          ReceviceLatitude ,
          OrderCount ,
          CommissionRate ,
          BaseCommission ,
          CommissionFormulaMode ,
          SongCanDate ,
          [Weight] ,
          Quantity ,
          ReceiveProvince ,
          ReceiveProvinceCode ,
          ReceiveCityCode ,
          ReceiveArea ,
          ReceiveAreaCode ,
          OriginalOrderNo ,
          BusinessCommission ,
          SettleMoney ,
          Adjustment ,
          CommissionType,
          CommissionFixValue,
          BusinessGroupId,
          TimeSpan,
          MealsSettleMode,
          BusinessReceivable,
          GroupBusinessId,
          ProductName,
          OriginalOrderId
        )
values  ( @OrderNo ,
          @PickUpAddress ,
          getdate() ,
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
          @CommissionRate ,
          @BaseCommission,
          @CommissionFormulaMode ,
          @SongCanDate ,
          @Weight1 ,
          @Quantity1 ,
          @ReceiveProvince ,
          @ReceiveProvinceCode ,
          @ReceiveCityCode ,
          @ReceiveArea ,
          @ReceiveAreaCode ,
          @OriginalOrderNo ,
          @BusinessCommission ,
          @SettleMoney ,
          @Adjustment ,
          @CommissionType,
          @CommissionFixValue,
          @BusinessGroupId,
          @TimeSpan,
          @MealsSettleMode,
          @BusinessReceivable,
          @GroupBusinessId,
          @ProductName,
          @OriginalOrderId
        )
select @@identity";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@OrderNo", order.OrderNo);
            dbParameters.AddWithValue("@PickUpAddress", order.PickUpAddress);
            dbParameters.AddWithValue("@ReceviceName", order.ReceviceName);
            dbParameters.AddWithValue("@RecevicePhoneNo", order.RecevicePhoneNo);
            dbParameters.AddWithValue("@ReceviceAddress", order.ReceviceAddress);
            dbParameters.AddWithValue("@IsPay", order.IsPay);
            dbParameters.AddWithValue("@Amount", order.Amount);
            dbParameters.AddWithValue("@OrderCommission", order.OrderCommission);
            dbParameters.AddWithValue("@DistribSubsidy", order.DistribSubsidy);
            dbParameters.AddWithValue("@WebsiteSubsidy", order.WebsiteSubsidy);
            dbParameters.AddWithValue("@Remark", order.Remark);
            dbParameters.AddWithValue("@OrderFrom", order.OrderFrom);
            dbParameters.AddWithValue("@Status", order.Status);
            dbParameters.AddWithValue("@businessId", order.businessId);
            dbParameters.AddWithValue("@ReceviceCity", order.ReceviceCity);
            dbParameters.AddWithValue("@ReceviceLongitude", order.ReceviceLongitude);
            dbParameters.AddWithValue("@ReceviceLatitude", order.ReceviceLatitude);
            dbParameters.AddWithValue("@OrderCount", order.OrderCount);
            dbParameters.AddWithValue("@CommissionRate", order.CommissionRate);
            dbParameters.AddWithValue("@BaseCommission", order.BaseCommission);
            dbParameters.AddWithValue("@CommissionFormulaMode", order.CommissionFormulaMode);
            dbParameters.AddWithValue("@SongCanDate", order.SongCanDate);
            dbParameters.AddWithValue("@Weight1", order.Weight);
            dbParameters.AddWithValue("@Quantity1", order.Quantity);
            dbParameters.AddWithValue("@ReceiveProvince", order.ReceiveProvince);
            dbParameters.AddWithValue("@ReceiveProvinceCode", order.ReceiveProvinceCode);
            dbParameters.AddWithValue("@ReceiveCityCode", order.ReceiveCityCode);
            dbParameters.AddWithValue("@ReceiveArea", order.ReceiveArea);
            dbParameters.AddWithValue("@ReceiveAreaCode", order.ReceiveAreaCode);
            dbParameters.AddWithValue("@OriginalOrderNo", order.OriginalOrderNo);
            dbParameters.AddWithValue("@BusinessCommission", order.BusinessCommission);
            dbParameters.AddWithValue("@SettleMoney", order.SettleMoney);
            dbParameters.AddWithValue("@Adjustment", order.Adjustment);
            dbParameters.AddWithValue("@CommissionType", order.CommissionType);
            dbParameters.AddWithValue("@CommissionFixValue", order.CommissionFixValue);
            dbParameters.AddWithValue("@BusinessGroupId", order.BusinessGroupId);
            dbParameters.AddWithValue("@TimeSpan", order.TimeSpan);
            dbParameters.AddWithValue("@MealsSettleMode", order.MealsSettleMode);
            dbParameters.AddWithValue("@BusinessReceivable", order.BusinessReceivable);
            dbParameters.AddWithValue("@GroupBusinessId", order.GroupBusinessId);
            dbParameters.AddWithValue("@ProductName", order.ProductName);
            dbParameters.AddWithValue("@OriginalOrderId", order.OriginalOrderId);

            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            return ParseHelper.ToInt(result, 0);
        }


    }
}
