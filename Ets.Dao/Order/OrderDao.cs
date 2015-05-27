using Ets.Dao.Clienter;
using Ets.Dao.WtihdrawRecords;
using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using Ets.Model.DataModel.Subsidy;
using Ets.Model.DomainModel.Order;
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
                WebsiteSubsidy,CommissionRate,CommissionFormulaMode,ReceiveProvince,ReceviceCity,ReceiveArea,
                PickupCode,BusinessCommission,SettleMoney,Adjustment,OrderFrom,Status,CommissionType,CommissionFixValue,BusinessGroupId,Invoice)
                output Inserted.Id,GETDATE(),@OptName,'新增订单',Inserted.businessId,Inserted.[Status],@Platform
                into dbo.OrderSubsidiesLog(OrderId,InsertTime,OptName,Remark,OptId,OrderStatus,[Platform])
                Values(@OrderNo,
                @OriginalOrderNo,@PubDate,@SongCanDate,@IsPay,@Amount,
                @Remark,@Weight,@DistribSubsidy,@OrderCount,@ReceviceName,
                @RecevicePhoneNo,@ReceiveProvinceCode,@ReceiveCityCode,@ReceiveAreaCode,@ReceviceAddress,
                @ReceviceLongitude,@ReceviceLatitude,@BusinessId,@PickUpAddress,@Payment,@OrderCommission,
                @WebsiteSubsidy,@CommissionRate,@CommissionFormulaMode,@ReceiveProvince,@ReceviceCity,@ReceiveArea,
                @PickupCode,@BusinessCommission,@SettleMoney,@Adjustment,@OrderFrom,@Status,@CommissionType,@CommissionFixValue,@BusinessGroupId,@Invoice);select  IDENT_CURRENT('order') ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            //基本参数信息

            dbParameters.Add("@OrderNo", SqlDbType.NVarChar);
            dbParameters.SetValue("@OrderNo", paramodel.OrderNo); //订单号
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
            dbParameters.AddWithValue("@OptName", (SuperPlatform.第三方对接平台.ToString()));//操作人
            dbParameters.AddWithValue("@Platform", (int)SuperPlatform.第三方对接平台);//操作平台
            dbParameters.AddWithValue("@CommissionType", paramodel.CommissionType);//结算类型
            dbParameters.AddWithValue("@CommissionFixValue", paramodel.CommissionFixValue);//固定金额
            dbParameters.AddWithValue("@BusinessGroupId", paramodel.BusinessGroupId);//分组ID
            dbParameters.AddWithValue("@Invoice", paramodel.invoice_title);//发票标题

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
        /// CreateToSql  操作插入OrderChild表  hulingbo 
        /// </summary>
        /// <param name="paramodel"></param>
        /// <param name="orderId"></param>
        public void CreateToSqlAddOrderChild(CreatePM_OpenApi paramodel, int orderId)
        {
            const string insertOrderChildSql = @"
insert into OrderChild
        (OrderId,
        ChildId,
        TotalPrice,
        GoodPrice,
        DeliveryPrice,
        CreateBy,
        UpdateBy)
values( @OrderId,
        @ChildId,
        @TotalPrice,
        @GoodPrice,
        @DeliveryPrice,
        @CreateBy,
        @UpdateBy)";
            IDbParameters dbOrderChildParameters = DbHelper.CreateDbParameters();
            dbOrderChildParameters.AddWithValue("@OrderId", orderId);
            dbOrderChildParameters.AddWithValue("@ChildId", 1);
            decimal totalPrice = paramodel.total_price + Convert.ToDecimal(paramodel.delivery_fee);
            dbOrderChildParameters.AddWithValue("@TotalPrice", totalPrice);
            dbOrderChildParameters.AddWithValue("@GoodPrice", paramodel.total_price);//订单金额
            dbOrderChildParameters.AddWithValue("@DeliveryPrice", paramodel.delivery_fee);//外送费
            dbOrderChildParameters.AddWithValue("@CreateBy", SuperPlatform.第三方对接平台.ToString());
            dbOrderChildParameters.AddWithValue("@UpdateBy", SuperPlatform.第三方对接平台.ToString());

            DbHelper.ExecuteScalar(SuperMan_Write, insertOrderChildSql, dbOrderChildParameters);
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
insert into OrderOther(OrderId,NeedUploadCount,HadUploadCount,PubLongitude,PubLatitude)
select @OrderId,1,0,b.Longitude,b.Latitude 
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
                                    ,case when o.OrderFrom=0 then '客户端' else g.GroupName end GroupName
                                    ,o.[Adjustment]
                                    ,ISNULL(oo.HadUploadCount,0) HadUploadCount
                                    ,o.BusinessCommission --商家结算比例
                                    ";
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
                sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),o.PubDate,120)>=CONVERT(CHAR(10),'{0}',120) ", criteria.orderPubStart.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.orderPubEnd))
            {
                sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),o.PubDate,120)<=CONVERT(CHAR(10),'{0}',120) ", criteria.orderPubEnd.Trim());
            }
            if (criteria.GroupId != null && criteria.GroupId != 0)
            {
                sbSqlWhere.AppendFormat(" AND o.OrderFrom={0} ", criteria.GroupId);
            }
            if (!string.IsNullOrWhiteSpace(criteria.businessCity))
            {
                sbSqlWhere.AppendFormat(" AND b.City='{0}' ", criteria.businessCity.Trim());
            }
            if (criteria.AuthorityCityNameListStr!=null && !string.IsNullOrEmpty(criteria.AuthorityCityNameListStr.Trim()))
            {
                sbSqlWhere.AppendFormat(" AND b.City IN({0}) ", criteria.AuthorityCityNameListStr.Trim());
            }
            string tableList = @" [order] o WITH ( NOLOCK )
                                LEFT JOIN clienter c WITH ( NOLOCK ) ON c.Id = o.clienterId
                                LEFT JOIN business b WITH ( NOLOCK ) ON b.Id = o.businessId
                                LEFT JOIN [group] g WITH ( NOLOCK ) ON g.id = o.OrderFrom
                                LEFT JOIN dbo.OrderOther oo (nolock) ON o.Id = oo.OrderId ";
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
                                        ,b.[City] BusinessCity
                                        ,b.Name BusinessName
                                        ,b.PhoneNo BusinessPhoneNo
                                        ,b.Address BusinessAddress
                                        ,c.PhoneNo ClienterPhoneNo
                                        ,c.TrueName ClienterTrueName
                                        ,c.TrueName ClienterName
                                        ,c.AccountBalance AccountBalance
                                        ,b.GroupId
                                        ,case when o.orderfrom=0 then '客户端' else g.GroupName end GroupName
                                        ,o.OriginalOrderNo
                                        ,oo.NeedUploadCount
                                        ,oo.HadUploadCount
                                        ,oo.ReceiptPic
                                        ,o.OtherCancelReason
                                        ,o.OriginalOrderNo
                                        ,ISNULL(o.MealsSettleMode,0) MealsSettleMode
                                        ,ISNULL(oo.IsJoinWithdraw,0) IsJoinWithdraw
                                        ,o.BusinessReceivable
                                        ,o.SettleMoney
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
                                        ,case when o.orderfrom=0 then '客户端' else g.GroupName end GroupName
                                        ,o.OriginalOrderNo
                                        ,oo.NeedUploadCount
                                        ,oo.HadUploadCount
                                        ,oo.ReceiptPic
                                        ,o.OtherCancelReason
                                        ,o.OriginalOrderNo
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

            var orderDetailList = ConvertDataTableList<Ets.Model.DataModel.Order.OrderDetail>(ds.Tables[1]);
            var orderChildList = ConvertDataTableList<OrderChild>(ds.Tables[2]);

            order[0].OrderDetailList = orderDetailList.ToList();
            order[0].OrderChildList = orderChildList.ToList();
            if (order != null && order.Count > 0)
            {
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
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool IsOrNotFinish(string orderNo)
        {
            StringBuilder sql = new StringBuilder(@" 
select min(convert(int, oc.PayStatus)) IsPay
from   dbo.[order] o ( nolock )
join dbo.OrderChild oc ( nolock ) on o.Id = oc.OrderId
where  o.OrderNo = @OrderNo");
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("OrderNo", DbType.String).Value = orderNo;
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
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool RushOrder(OrderListModel order)
        {
            //bool reslut = false;
            //try
            //{
            //    string sql = @" update [order] set clienterId=@clienterId,Status=@Status where OrderNo=@OrderNo and Status=0 and Status!=3 ";
            //    IDbParameters dbParameters = DbHelper.CreateDbParameters();
            //    dbParameters.AddWithValue("@clienterId", order.clienterId);
            //    dbParameters.AddWithValue("@Status", ConstValues.ORDER_ACCEPT);
            //    dbParameters.Add("@OrderNo", SqlDbType.NVarChar);
            //    dbParameters.SetValue("@OrderNo", order.OrderNo);

            //    int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
            //    if (i > 0)
            //    {
            //        reslut = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    reslut = false;
            //    LogHelper.LogWriter(ex, "订单指派超人");
            //    throw;
            //}
            //return reslut;
            StringBuilder sql = new StringBuilder();

            string sqlText = @"
            update dbo.[order]
            set clienterId=@clienterId,Status=@Status
            where OrderNo=@OrderNo and Status=0
            if(@@error<>0 or @@ROWCOUNT=0)
            begin
	            select 1 --更改状态失败
	            return
            end
            insert  into dbo.OrderSubsidiesLog ( OrderId, Price, InsertTime, OptName,
                                                 Remark, OptId, OrderStatus, Platform )
            select  o.Id, o.OrderCommission, getdate(), '骑士', '任务已被抢', @clienterId, @Status, @Platform
            from    dbo.[order] o ( nolock )
            where   o.OrderNo = @OrderNo
            select 0";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("clienterId", DbType.Int32, 4).Value = order.clienterId;// userId;
            dbParameters.Add("Status", DbType.Int32, 4).Value = ConstValues.ORDER_ACCEPT;
            dbParameters.Add("OrderNo", DbType.String, 50).Value = order.OrderNo;
            dbParameters.Add("Platform", DbType.Int32, 4).Value = SuperPlatform.骑士.GetHashCode();
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
        /// 订单是否被抢
        /// 平扬 
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool CheckOrderIsAllowRush(string orderNo)
        {
            string selSql = string.Format(@" SELECT 1 FROM  [order] WITH(NOLOCK)  WHERE  OrderNo = @orderNo and [Status]=0 ");
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            //订单号
            dbParameters.Add("@orderNo", SqlDbType.NVarChar);
            dbParameters.SetValue("@orderNo", orderNo);
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, selSql, dbParameters);
            return ParseHelper.ToInt(executeScalar, 0) > 0;
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
            string upSql =string.Format(@" UPDATE dbo.[order]
 SET  [Status] = @status,OtherCancelReason=@OtherCancelReason
 output Inserted.Id,@Price,GETDATE(),'{0}',@OtherCancelReason,Inserted.businessId,Inserted.[Status],{1}
 into dbo.OrderSubsidiesLog(OrderId,Price,InsertTime,OptName,Remark,OptId,OrderStatus,[Platform])
 WHERE  OrderNo = @orderNo", SuperPlatform.商家, (int)SuperPlatform.商家);

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("orderNo", DbType.String, 45).Value = orderNo;
            dbParameters.Add("status", DbType.Int32, 4).Value = orderStatus;
            dbParameters.Add("Price", DbType.Decimal, 9).Value = price;
            dbParameters.Add("OtherCancelReason", DbType.String, 500).Value = remark;

            if (status != null)
            {
                upSql = upSql+" and Status=" + status;
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
", SuperPlatform.商家, (int)SuperPlatform.商家);
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
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        public int FinishOrderStatus(string orderNo, int clientId, OrderListModel myOrderInfo)
        {
            //更新订单状态
            StringBuilder upSql = new StringBuilder();

            upSql.AppendFormat(@" UPDATE dbo.[order]
 SET [Status] = @status,ActualDoneDate=getdate()
output Inserted.Id,GETDATE(),'{0}','任务已完成',Inserted.clienterId,Inserted.[Status],{1}
into dbo.OrderSubsidiesLog(OrderId,InsertTime,OptName,Remark,OptId,OrderStatus,[Platform]) 
WHERE  OrderNo = @orderNo AND clienterId IS NOT NULL and Status = 4;", SuperPlatform.骑士, (int)SuperPlatform.骑士);

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@orderNo", SqlDbType.NVarChar).Value = orderNo;
            dbParameters.AddWithValue("@status", ConstValues.ORDER_FINISH);
            object executeScalar = DbHelper.ExecuteNonQuery(SuperMan_Write, upSql.ToString(), dbParameters);
            return ParseHelper.ToInt(executeScalar, -1);
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
@rechargeTotal decimal(18,2)
set @incomeTotal =convert(decimal(18,2),(select sum(TotalPrice) from dbo.OrderChild oc(nolock) where ThirdPayStatus =1))
set @withdrawClienterPrice = convert(decimal(18,2),(select isnull( sum(isnull(Amount,0)),0) withPirce FROM dbo.ClienterWithdrawForm(nolock) cwf where Status =3)) 
set @businessBalance=convert(decimal(18,2),(SELECT sum(BalancePrice) FROM dbo.business b(nolock) where Status=1 ))
set @withdrawBusinessPrice=convert( decimal(18,2),(SELECT sum(Amount) as withdrawBusinessPrice FROM dbo.BusinessWithdrawForm(nolock) bwf where Status =3 ))
set @rechargeTotal = (select sum(Amount) from BusinessBalanceRecord(nolock) where RecordType = 4 ) --商户充值总计
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
		(@incomeTotal+0) allIncomeTotal, --账户收入总计
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
            //            string columnList = @"  b.district
            //				                    ,COUNT(*) orderCount
            //				                    ,SUM(o.DistribSubsidy)distribSubsidy
            //				                    ,SUM(o.WebsiteSubsidy)websiteSubsidy
            //				                    ,SUM(o.OrderCommission)orderCommission
            //				                    ,SUM(o.DistribSubsidy+o.WebsiteSubsidy+o.OrderCommission)deliverAmount
            //				                    ,SUM(Amount)orderAmount ";
            //            var sbSqlWhere = new StringBuilder(" 1=1 ");
            //            if (criteria.searchType == 1)//当天
            //            {
            //                sbSqlWhere.Append(" AND DateDiff(DAY, GetDate(),o.PubDate)=0 ");
            //            }
            //            else if (criteria.searchType == 2)//本周
            //            {
            //                sbSqlWhere.Append(" AND DateDiff(WEEK, GetDate(),DATEADD (DAY, -1,o.PubDate))=0 ");
            //            }
            //            else if (criteria.searchType == 3)//本月
            //            {
            //                sbSqlWhere.Append(" AND DateDiff(MONTH, GetDate(),o.PubDate)=0 ");
            //            }
            //            sbSqlWhere.Append(" group by b.district ");
            //            string tableList = @" business b with(nolock)
            //                                  join [order] o with(nolock) on b.Id=o.businessId ";
            //            string orderByColumn = " COUNT(*) DESC ";
            //            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
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
                sbtbl.Append("   o.PubDate between dateadd(day,-30,getdate()) and getdate() ");
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
                                  ,[ActiveClienter]";

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
        ISNULL(oo.HadUploadCount,0) HadUploadCount
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
        c.Id clienterId ,
        o.OrderCommission ,
        o.businessId ,
        b.GroupId ,
        o.PickupCode ,
        o.OrderCount,
        c.TrueName ClienterName,
        ISNULL(oo.HadUploadCount,0) HadUploadCount,
        o.MealsSettleMode,
        o.BusinessReceivable,
        o.IsPay,
        b.Name BusinessName,
        o.SettleMoney
from    [order] o with ( nolock )
        join dbo.clienter c with ( nolock ) on o.clienterId = c.Id
        join dbo.business b with ( nolock ) on o.businessId = b.Id
        join dbo.OrderOther oo with(nolock) on o.Id = oo.OrderId
where  o.OrderNo = @OrderNo
";
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
        c.Id clienterId ,
        o.OrderCommission ,
        o.businessId ,
        b.GroupId ,
        o.PickupCode ,
        o.OrderCount,
        c.TrueName ClienterName,
        ISNULL(oo.HadUploadCount,0) HadUploadCount,
        o.SettleMoney,
        o.IsPay,   
        o.ActualDoneDate
from    [order] o with ( nolock )
        join dbo.clienter c with ( nolock ) on o.clienterId = c.Id
        join dbo.business b with ( nolock ) on o.businessId = b.Id
        left join dbo.OrderOther oo with(nolock) on o.Id = oo.OrderId 
where   1 = 1 and o.Id = @Id
";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@Id", SqlDbType.Int).Value = orderId;

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
        /// 返回订单信息
        /// 窦海超
        /// 2015年5月21日 16:11:35
        /// </summary>
        /// <param name="Id">订单ID</param>
        /// <returns></returns>
        public OrderChildPayModel GetOrderById(int Id)
        {
            //string sql = "SELECT clienterId FROM dbo.[order] o where id = @id";
            string sql = @"
SELECT clienterId,min(PayStatus) as PayStatus FROM dbo.[order] o(nolock)
join dbo.OrderChild oc(nolock) on o.Id= oc.OrderId
 where o.id=@id group by PayStatus,clienterId
 ";
            //IDbParameters parms = DbHelper.CreateDbParameters("id", DbType.Int32, 4, Id);
            IDbParameters parms = DbHelper.CreateDbParameters();
            parms.Add("id", DbType.Int32, 4).Value = Id;
            //return DbHelper.QueryForObjectDelegate<OrderListModel>(SuperMan_Read, sql, row => new OrderListModel()
            //{
            //    clienterId = ParseHelper.ToInt(row["clienterId"]),
            //    IsPay = ParseHelper.ToBool(row["IsPay"], false)//是否允许点击完成，1=允许，0=不允许 
            //});
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
            //            string sql = string.Format(@"select 
            //                            Id,
            //                            DealCount,
            //                            DateDiff(MINUTE,PubDate, GetDate()) IntervalMinute
            //                            from [order] with(nolock)
            //                            where Status=0 AND DateDiff(MINUTE,PubDate, GetDate()) in ({0})", IntervalMinute);
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
        public bool addOrderSubsidiesLog(decimal AdjustAmount, int OrderId, string Remark)
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
                                ,'发布订单'
                                ,@Remark);";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Price", AdjustAmount);
            parm.AddWithValue("@OrderId", OrderId);
            parm.AddWithValue("@Remark", Remark);
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
        /// 根据订单id获取订单信息 和 小票相关
        /// wc
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public order GetOrderInfoByOrderId(int orderId)
        {
            string sql = @"
select  o.Id ,
        o.[Status] ,
        ISNULL(oo.HadUploadCount, 0) HadUploadCount,
        o.OrderCount
from    dbo.[order] o ( nolock )
        left join dbo.OrderOther oo ( nolock ) on o.Id = oo.OrderId
where   o.Id = @orderId;
";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@orderId", SqlDbType.NVarChar);
            parm.SetValue("@orderId", orderId);
            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
            var list = ConvertDataTableList<order>(dt);
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
            string remark = orderOptionModel.OptUserName + "通过后台管理系统取消订单";
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
                                             WHERE  OrderNo = @OrderNo");

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Price", model.OrderCommission);
            parm.AddWithValue("@OptId", orderOptionModel.OptUserId);
            parm.AddWithValue("@OptName", orderOptionModel.OptUserName);
            parm.AddWithValue("@Status", 3);
            parm.AddWithValue("@OrderNo", model.OrderNo);
            parm.AddWithValue("@Platform", 3);
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
                      else '未知请联系e代送客服'
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
                      else '未知请联系e代送客服'
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
                      else '未知请联系e代送客服'
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
          BusinessReceivable
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
          @BusinessReceivable
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
            orderLogParameters.AddWithValue("remark", ConstValues.PublishOrder);
            orderLogParameters.AddWithValue("optid", order.businessId);
            orderLogParameters.AddWithValue("orderStatus", OrderStatus.订单待抢单.GetHashCode());
            orderLogParameters.AddWithValue("platForm", SuperPlatform.商家.GetHashCode());

            DbHelper.ExecuteNonQuery(SuperMan_Write, sqlOrderLog, orderLogParameters);
            #endregion

            #region 写子OrderOther表
            const string insertOtherSql = @"
insert into OrderOther(OrderId,NeedUploadCount,HadUploadCount,PubLongitude,PubLatitude)
values(@OrderId,@NeedUploadCount,0,@PubLongitude,@PubLatitude)";
            IDbParameters dbOtherParameters = DbHelper.CreateDbParameters();
            dbOtherParameters.AddWithValue("@OrderId", orderId); //商户ID
            dbOtherParameters.AddWithValue("@NeedUploadCount", order.OrderCount); //需上传数量
            dbOtherParameters.AddWithValue("@PubLongitude", order.PubLongitude);
            dbOtherParameters.AddWithValue("@PubLatitude", order.PubLatitude);
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
                        dr["ChildId"] = order.listOrderChild[i].ChildId;
                        decimal totalPrice = order.listOrderChild[i].GoodPrice + Convert.ToDecimal(order.DistribSubsidy);
                        dr["TotalPrice"] = totalPrice;
                        dr["GoodPrice"] = order.listOrderChild[i].GoodPrice;
                        dr["DeliveryPrice"] = order.DistribSubsidy;
                        if ((bool)order.IsPay ||
                            (!(bool)order.IsPay && order.MealsSettleMode == MealsSettleMode.Status0.GetHashCode())
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

select  o.Id,o.OrderNo,o.PickUpAddress,o.PubDate,o.ReceviceName,o.RecevicePhoneNo,o.ReceviceAddress,o.ActualDoneDate,o.IsPay,
    o.Amount,o.OrderCommission,o.DistribSubsidy,o.WebsiteSubsidy,o.Remark,o.Status,o.clienterId,o.businessId,o.ReceviceCity,o.ReceviceLongitude,
    o.ReceviceLatitude,o.OrderFrom,o.OriginalOrderId,o.OriginalOrderNo,o.Quantity,o.Weight,o.ReceiveProvince,o.ReceiveArea,o.ReceiveProvinceCode,
    o.ReceiveCityCode,o.ReceiveAreaCode,o.OrderType,o.KM,o.GuoJuQty,o.LuJuQty,o.SongCanDate,o.OrderCount,o.CommissionRate,o.Payment,
    o.CommissionFormulaMode,o.Adjustment,o.BusinessCommission,o.SettleMoney,o.DealCount,o.PickupCode,o.OtherCancelReason,o.CommissionType,
    o.CommissionFixValue,o.BusinessGroupId,o.TimeSpan,o.Invoice,
    isnull(o.DistribSubsidy,0)*isnull(o.OrderCount,0) as TotalDistribSubsidy,(o.Amount+isnull(o.DistribSubsidy,0)*isnull(o.OrderCount,0)) as TotalAmount,
    o.MealsSettleMode,
    b.[City] BusinessCity,b.Name BusinessName,b.PhoneNo BusinessPhone ,b.Address BusinessAddress ,b.GroupId, 
    b.Longitude, b.Latitude,REPLACE(b.City,'市','') AS pickUpCity,
    oo.NeedUploadCount,oo.HadUploadCount,oo.GrabTime,
    c.TrueName ClienterName,c.PhoneNo ClienterPhoneNo,
   (case 
    when  ISNULL(b.Latitude,0)=0 or ISNULL(b.Longitude,0)=0 or @clienterLongitude=0 or  @clienterLatitude=0  then -1
    else round(geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(geography::Point(@clienterLatitude,@clienterLongitude,4326)),0)
    end)
    as distance
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
o.id,o.amount, 
o.orderCommission clienterPrice, --给骑士
o.Amount-o.SettleMoney businessPrice,--给商家
o.clienterId, o.businessId
from    dbo.[order] o ( nolock )
        join dbo.OrderOther oo ( nolock ) on o.Id = oo.OrderId
where   oo.IsJoinWithdraw = 0
        and oo.HadUploadCount = o.OrderCount --订单量=已上传
        and o.Status = 1 --已完成订单
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
        /// 更新订单是否加入已提现
        /// 窦海超
        /// 2015年5月15日 17:08:27
        /// </summary>
        /// <param name="orderId"></param>
        public void UpdateJoinWithdraw(int orderId)
        {
            string sql = @"update dbo.OrderOther set IsJoinWithdraw = 1 where OrderId=@orderId";
            IDbParameters parm = DbHelper.CreateDbParameters("orderId", DbType.Int32, 4, orderId);
            DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm);
        }

        public IList<GetJobCDM> GetLastedJobC(GetJobCPM model)
        {
            StringBuilder whereStr = new StringBuilder();

            if (model.city.Contains("北京"))
            {
                whereStr.AppendFormat(" AND a.ReceviceCity LIKE '北京%'", model.city);
            }
            else
            {
                whereStr.AppendFormat(" AND a.ReceviceCity = '{0}'", model.city);
            }

            string sql = string.Format(@"
declare @cliernterPoint geography ;
select @cliernterPoint=geography::Point(@Latitude,@Longitude,4326) ;
select top {0}
        a.Id, a.OrderCommission, a.OrderCount,
        ( a.Amount + a.OrderCount * a.DistribSubsidy ) as Amount,
        b.Name as BusinessName, b.City as BusinessCity,
        b.Address as BusinessAddress, isnull(a.ReceviceCity, '') as UserCity,
        isnull(a.ReceviceAddress, '') as UserAddress,
        case convert(varchar(100), PubDate, 23)
          when convert(varchar(100), getdate(), 23) then '今日 '
          else substring(convert(varchar(100), PubDate, 23), 6, 5)
        end + '  ' + substring(convert(varchar(100), PubDate, 24), 1, 5) as PubDate,
		round(geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint),0) as DistanceToBusiness 
from    dbo.[order] a ( nolock )
        join dbo.business b ( nolock ) on a.businessId = b.Id
where   a.status = 0
        {1}
order by a.Id desc", model.TopNum, whereStr.ToString());
            
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Latitude", model.Latitude);
            dbParameters.AddWithValue("Longitude", model.Longitude);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                return TranslateGetJobC(dt);
            }
            return new List<GetJobCDM>();
        }

        /// <summary>
        /// 骑士端获取任务列表（最新/最近）任务   add by caoheyang 20150519
        /// </summary>
        /// <param name="model">订单查询实体</param>
        /// <returns></returns>
        public IList<GetJobCDM> GetJobC(GetJobCPM model)
        {
            string sql = string.Format(@"
declare @cliernterPoint geography ;
select @cliernterPoint=geography::Point(@Latitude,@Longitude,4326) ;
select top {0} a.Id,a.OrderCommission,a.OrderCount,   
(a.Amount+a.OrderCount*a.DistribSubsidy) as Amount,
b.Name as BusinessName,b.City as BusinessCity,b.Address as BusinessAddress,
ISNULL(a.ReceviceCity,'') as UserCity,ISNULL(a.ReceviceAddress,'') as UserAddress,
case convert(varchar(100), PubDate, 23) 
	when convert(varchar(100), getdate(), 23) then '今日 '
    else substring(convert(varchar(100), PubDate, 23),6,5) 
end
+'  '+substring(convert(varchar(100),PubDate,24),1,5)
as PubDate,
round(geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint),0) as DistanceToBusiness 
from dbo.[order] a (nolock)
join dbo.business b (nolock) on a.businessId=b.Id
where a.status=0 and  geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint)<= @PushRadius
order by {1}
", model.TopNum, model.SearchType == 0 ? "a.Id desc" : " geography::Point(ISNULL(b.Latitude,0),ISNULL(b.Longitude,0),4326).STDistance(@cliernterPoint) asc");
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Latitude", model.Latitude);
            dbParameters.AddWithValue("Longitude", model.Longitude);
            dbParameters.AddWithValue("PushRadius", ParseHelper.ToInt(model.PushRadius) * 1000);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                return TranslateGetJobC(dt);
            }
            return new List<GetJobCDM>();
        }

        /// <summary>
        ///  骑士端获取任务列表（最新/最近）任务   add by caoheyang 20150519
        /// </summary>
        /// <param name="dt">datatable</param>
        /// <returns></returns>
        private IList<GetJobCDM> TranslateGetJobC(DataTable dt)
        {
            IList<GetJobCDM> models = new List<GetJobCDM>();
            foreach (DataRow dataRow in dt.Rows)
            {
                GetJobCDM temp = new GetJobCDM();
                temp.Id = ParseHelper.ToInt(dataRow["Id"]);
                temp.PubDate = dataRow["PubDate"].ToString();
                temp.OrderCommission = ParseHelper.ToDecimal(dataRow["OrderCommission"]);
                temp.OrderCount = ParseHelper.ToInt(dataRow["OrderCount"]);
                temp.Amount = ParseHelper.ToDecimal(dataRow["Amount"]);
                temp.BusinessName = dataRow["BusinessName"].ToString();
                temp.BusinessCity = dataRow["BusinessCity"].ToString();
                temp.BusinessAddress = dataRow["BusinessAddress"] == null
                    ? ""
                    : dataRow["BusinessAddress"].ToString();
                temp.UserCity = dataRow["UserCity"] == null ? "" : dataRow["UserCity"].ToString();
                temp.UserAddress = dataRow["UserAddress"] == null ? "" : dataRow["UserAddress"].ToString();
                int distanceToBusiness = ParseHelper.ToInt(dataRow["DistanceToBusiness"], 0);
                temp.DistanceToBusiness = distanceToBusiness < 1000
                    ? distanceToBusiness + "m"
                    : Math.Round(distanceToBusiness * 0.001, 2) + "Km";
                models.Add(temp);
            }
            return models;
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        public void UpdateTake(int orderId, int clienterId, float takeLongitude, float takeLatitude)
        {
            const string UPDATE_SQL = @"
update dbo.[Order] 
    set Status=4
where id=@orderid and Status=2 and clienterId=@clienterId;
update OrderOther 
    set TakeTime=GETDATE(),TakeLongitude=@TakeLongitude,TakeLatitude=@TakeLatitude 
where orderid=@orderid  
";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("TakeLongitude", takeLongitude);
            dbParameters.AddWithValue("TakeLatitude", takeLatitude);
            dbParameters.Add("orderId", DbType.Int32, 4).Value = orderId;
            dbParameters.AddWithValue("clienterId", clienterId);
            DbHelper.ExecuteNonQuery(SuperMan_Write, UPDATE_SQL, dbParameters);
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
            string sql = @" select * from [order] where Id=@OrderId and businessId=@BusinessId";
            if (status != null)
            {
                sql = sql + " and status=" + status;
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
        public order GetOrderMainByNo(string orderNo, int? businessId=null, int? status = null)
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
        /// 订单取消扣除骑士佣金和插入骑士余额流水
        /// danny-2015051921
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool OrderCancelReturnClienter(OrderListModel model)
        {
            string sql = string.Format(@" 
update c
set    c.AccountBalance=ISNULL(c.AccountBalance, 0)-@Amount
OUTPUT
  Inserted.Id,
  -@Amount,
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
            parm.AddWithValue("@Amount", model.OrderCommission);
            parm.AddWithValue("@Status", ClienterBalanceRecordStatus.Success);
            parm.AddWithValue("@RecordType", ClienterBalanceRecordRecordType.CancelOrder);
            parm.AddWithValue("@Operator", model.OptUserName);
            parm.AddWithValue("@WithwardId", model.Id);
            parm.AddWithValue("@RelationNo", model.OrderNo);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@ClienterId", model.clienterId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }

    }
}
