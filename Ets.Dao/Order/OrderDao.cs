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
           CommissionRate,
           CommissionFormulaMode,
           SongCanDate,
           [Weight],
          Quantity,
          ReceiveProvince ,
          ReceiveProvinceCode , 
          ReceiveCityCode ,
          ReceiveArea ,
          ReceiveAreaCode,  
          OriginalOrderNo,
          BusinessCommission,
          SettleMoney
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
           @CommissionRate,
           @CommissionFormulaMode,
           @SongCanDate,
           @Weight1,
          @Quantity1,
          @ReceiveProvince ,
          @ReceiveProvinceCode , 
          @ReceiveCityCode ,
          @ReceiveArea ,
          @ReceiveAreaCode,  
          @OriginalOrderNo,
          @BusinessCommission,
          @SettleMoney
         )");

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OrderNo", SqlDbType.NVarChar);
            parm.SetValue("@OrderNo", order.OrderNo);
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
            parm.AddWithValue("@CommissionFormulaMode", order.CommissionFormulaMode);

            parm.AddWithValue("@SongCanDate", order.SongCanDate);
            parm.AddWithValue("@Weight1", order.Weight);

            parm.AddWithValue("@Quantity1", order.Quantity);
            parm.AddWithValue("@ReceiveProvince", order.ReceiveProvince);
            parm.AddWithValue("@ReceiveProvinceCode", order.ReceiveProvinceCode);
            parm.AddWithValue("@ReceiveCityCode", order.ReceiveCityCode);
            parm.AddWithValue("@ReceiveArea", order.ReceiveArea);
            parm.AddWithValue("@ReceiveAreaCode", order.ReceiveAreaCode);
            parm.AddWithValue("@OriginalOrderNo", order.OriginalOrderNo);
            parm.AddWithValue("@BusinessCommission", order.BusinessCommission);
            parm.AddWithValue("@SettleMoney", order.SettleMoney);
              
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
                Longitude,Latitude,DistribSubsidy,Province,City,district,CityId,districtId)  
                OUTPUT Inserted.Id   
                values(@OriginalBusiId,@Name,@GroupId,@IDCard,@Password,
                @PhoneNo,@PhoneNo2,@Address,@ProvinceCode,@CityCode,@AreaCode,
                @Longitude,@Latitude,@DistribSubsidy,@Province,@City,@district,@CityId,@districtId);";
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
                insertBdbParameters.AddWithValue("@Province", paramodel.store_info.province);    //门店省
                insertBdbParameters.AddWithValue("@City", paramodel.store_info.city);    //门店市编码
                insertBdbParameters.AddWithValue("@district", paramodel.store_info.area);    //门店区编码
                insertBdbParameters.AddWithValue("@CityId", paramodel.store_info.city_code);    //门店市编码
                insertBdbParameters.AddWithValue("@districtId", paramodel.store_info.area_code);    //门店区编码
                //insertBdbParameters.AddWithValue("@CommissionTypeId", paramodel.store_info.commission_type == null ?
                //    1 : paramodel.store_info.commission_type);   //佣金类型，涉及到快递员的佣金计算方式，默认1  业务改变已经无效  
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
                WebsiteSubsidy,CommissionRate,CommissionFormulaMode,ReceiveProvince,ReceviceCity,ReceiveArea)
                OUTPUT Inserted.OrderNo
                Values(@OrderNo,
                @OriginalOrderNo,@PubDate,@SongCanDate,@IsPay,@Amount,
                @Remark,@Weight,@DistribSubsidy,@OrderCount,@ReceviceName,
                @RecevicePhoneNo,@ReceiveProvinceCode,@ReceiveCityCode,@ReceiveAreaCode,@ReceviceAddress,
                @ReceviceLongitude,@ReceviceLatitude,@BusinessId,@PickUpAddress,@Payment,@OrderCommission,
                @WebsiteSubsidy,@CommissionRate,@CommissionFormulaMode,@ReceiveProvince,@ReceviceCity,@ReceiveArea)";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            ///基本参数信息

            dbParameters.Add("@OrderNo", SqlDbType.NVarChar);
            dbParameters.SetValue("@OrderNo", Helper.generateOrderCode(bussinessId)); //根据商户id生成订单号(15位));

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
            dbParameters.AddWithValue("@CommissionFormulaMode", paramodel.CommissionFormulaMode); //订单佣金计算方式
            dbParameters.AddWithValue("@ReceiveProvince", paramodel.address.province);    //用户省
            dbParameters.AddWithValue("@ReceviceCity", paramodel.address.city); //用户市
            dbParameters.AddWithValue("@ReceiveArea", paramodel.address.area); //用户区
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
                //订单号
                insertOrderDetaiParas.Add("@OrderNo", SqlDbType.NVarChar);
                insertOrderDetaiParas.SetValue("@OrderNo", orderNo);
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
                         CONVERT(CHAR(10),PubDate,120) AS PubDate, --发布时间
                        SUM(ISNULL(Amount,0)) AS OrderPrice, --订单金额
                        ISNULL(COUNT(o.Id),0) AS MisstionCount,--任务量
                        SUM(ISNULL(OrderCount,0)) AS OrderCount,--订单量
                         ISNULL(SUM(o.Amount*ISNULL(b.BusinessCommission,0)/100+ ISNULL( b.DistribSubsidy ,0)* o.OrderCount),0) AS YsPrice,  -- 应收金额
                          ISNULL( SUM( OrderCommission),0) AS YfPrice  --应付金额
                        FROM dbo.[order](NOLOCK) AS o
                        LEFT JOIN dbo.business(NOLOCK) AS b ON o.businessId=b.Id
                         WHERE  
                        o.[Status]=1 AND 
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
                        SUM(ISNULL(OrderCount,0)) AS OrderCount,--订单量
                        DealCount
                        FROM dbo.[order](NOLOCK) AS o
                         WHERE  
                        o.[Status]=1 AND 
                        DealCount>0 AND
                        CONVERT(CHAR(10),PubDate,120)>=CONVERT(CHAR(10),@StartTime,120) and 
                        CONVERT(CHAR(10),PubDate,120)<=CONVERT(CHAR(10),@EndTime,120)
                        GROUP BY CONVERT(CHAR(10),PubDate,120),DealCount
                        ORDER BY DealCount ASC
                        ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@StartTime", StartTime);
            parm.AddWithValue("@EndTime", EndTime);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
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
                                    ,g.GroupName
                                    ,o.[Adjustment]
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
                sbSqlWhere.AppendFormat(" AND o.PubDate>='{0}' ", criteria.orderPubStart);
            }
            if (!string.IsNullOrWhiteSpace(criteria.orderPubEnd))
            {
                sbSqlWhere.AppendFormat(" AND o.PubDate<='{0}' ", criteria.orderPubEnd);
            }
            if (criteria.GroupId != null && criteria.GroupId != 0)
            {
                sbSqlWhere.AppendFormat(" AND o.GroupId={0} ", criteria.GroupId);
            }
            if (!string.IsNullOrWhiteSpace(criteria.businessCity))
            {
                sbSqlWhere.AppendFormat(" AND b.City='{0}' ", criteria.businessCity.Trim());
            }
            string tableList = @" [order] o WITH ( NOLOCK )
                                LEFT JOIN clienter c WITH ( NOLOCK ) ON c.Id = o.clienterId
                                LEFT JOIN business b WITH ( NOLOCK ) ON b.Id = o.businessId
                                LEFT JOIN [group] g WITH ( NOLOCK ) ON g.Id = b.GroupId ";
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
                                        ,c.TrueName ClienterTrueName
                                        ,b.GroupId
                                        ,o.OriginalOrderNo
                                    FROM [order] o WITH ( NOLOCK )
                                    LEFT JOIN business b WITH ( NOLOCK ) ON b.Id = o.businessId
                                     LEFT JOIN dbo.clienter c WITH (NOLOCK) ON o.clienterId=c.Id
                                    WHERE 1=1 ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OrderNo", SqlDbType.NVarChar);
            parm.SetValue("@OrderNo", orderNo);
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
                dbParameters.Add("@OrderNo", SqlDbType.NVarChar);
                dbParameters.SetValue("@OrderNo", order.OrderNo);

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
            dbParameters.Add("@orderNo", SqlDbType.NVarChar);
            dbParameters.SetValue("@orderNo", orderNo);  //订单号  
            dbParameters.AddWithValue("@status", orderStatus);

            object executeScalar = DbHelper.ExecuteNonQuery(SuperMan_Read, upSql, dbParameters);

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
            StringBuilder upSql = new StringBuilder(@" UPDATE dbo.[order]
 SET [Status] = @status WHERE  OrderNo = @orderNo AND clienterId IS NOT NULL;");

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@orderNo", SqlDbType.NVarChar);
            dbParameters.SetValue("@orderNo", orderNo);  //订单号  
            dbParameters.AddWithValue("@status", ConstValues.ORDER_FINISH);


            object executeScalar = DbHelper.ExecuteNonQuery(SuperMan_Read, upSql.ToString(), dbParameters);


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
            string sql = @"SELECT 
                        (SELECT SUM (AccountBalance) FROM dbo.clienter(NOLOCK)  WHERE AccountBalance>=1000) AS  WithdrawPrice,--提现金额
                        SUM(ISNULL(Amount,0)) AS OrderPrice, --订单金额
                        COUNT(1) AS MisstionCount,--任务量
                        SUM(ISNULL(OrderCount,0)) AS OrderCount,--订单量
                        SUM(o.Amount*ISNULL(b.BusinessCommission,0)/100+ ISNULL(b.DistribSubsidy ,0) * o.OrderCount) AS YsPrice,  -- 应收金额
                        SUM(ISNULL( OrderCommission,0)) AS YfPrice  --应付金额
                        FROM dbo.[order](NOLOCK) AS o
                        JOIN dbo.business(NOLOCK) AS b ON o.businessId=b.Id
                         WHERE  
                        o.[Status]=1 ";
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
            var sbtbl = new StringBuilder(@" (select   b.district
				                    ,COUNT(*) orderCount
				                    ,SUM(o.DistribSubsidy)distribSubsidy
				                    ,SUM(o.WebsiteSubsidy)websiteSubsidy
				                    ,SUM(o.OrderCommission)orderCommission
				                    ,SUM(o.DistribSubsidy+o.WebsiteSubsidy+o.OrderCommission)deliverAmount
				                    ,SUM(Amount)orderAmount
                                    from business b with(nolock)
                                    join [order] o with(nolock) on b.Id=o.businessId
                                    where 1=1");
            if (criteria.searchType == 1)//当天
            {
                sbtbl.Append(" AND DateDiff(DAY, GetDate(),o.PubDate)=0 ");
            }
            else if (criteria.searchType == 2)//本周
            {
                sbtbl.Append(" AND DateDiff(WEEK, GetDate(),DATEADD (DAY, -1,o.PubDate))=0 ");
            }
            else if (criteria.searchType == 3)//本月
            {
                sbtbl.Append(" AND DateDiff(MONTH, GetDate(),o.PubDate)=0 ");
            }
            sbtbl.Append(" group by b.district ) tbl ");
            string columnList = @"  tbl.district
				                    ,tbl.orderCount
				                    ,tbl.distribSubsidy
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
                                  ,[OneSubsidyOrderCount]
                                  ,[TwoSubsidyOrderCount]
                                  ,[ThreeSubsidyOrderCount]";

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
                where = " and OrderType =@OrderType";
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
        /// wc
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public OrderListModel GetOrderInfoByOrderNo(string orderNo)
        {
            string sql = @" SELECT TOP 1 
        o.[Id] ,
        o.[OrderNo] , 
        o.[Status] ,
        c.AccountBalance,
        c.Id clienterId,
        o.OrderCommission,
        o.businessId
 FROM   [order] o WITH ( NOLOCK ) 
        LEFT JOIN dbo.clienter c WITH ( NOLOCK ) ON o.clienterId = c.Id
 WHERE  1 = 1 AND o.OrderNo = @orderNo";

            if (!string.IsNullOrWhiteSpace(orderNo))
            {
                sql += " AND OrderNo=@OrderNo";
            }
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OrderNo", SqlDbType.NVarChar);
            parm.SetValue("@OrderNo", orderNo);
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
        /// 获取超过配置时间未抢单的订单
        /// danny-20150402
       /// </summary>
       /// <param name="IntervalMinute"></param>
       /// <returns></returns>
        public IList<OrderAutoAdjustModel> GetOverTimeOrder(string IntervalMinute)
        {
            string sql = string.Format(@"select 
                            Id,
                            DealCount,
                            DateDiff(MINUTE,PubDate, GetDate()) IntervalMinute
                            from [order] with(nolock)
                            where Status=0 AND DateDiff(MINUTE,PubDate, GetDate()) in ({0})", IntervalMinute);
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
    }
}
