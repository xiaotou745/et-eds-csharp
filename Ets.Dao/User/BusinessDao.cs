using System.Data;
using ETS;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using ETS.Data.PageData;
using Ets.Model.ParameterModel.Finance;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Const;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.ParameterModel.Bussiness;
using Ets.Model.DataModel.Order;
using Ets.Model.DataModel.Bussiness;
using ETS.Extension;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.DataModel.Group;
using Ets.Model.ParameterModel.Order;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.DataModel.Finance;
using ETS.Data.Generic;
namespace Ets.Dao.User
{
    public class BusinessDao : DaoBase
    {

        /// <summary>
        /// 商户获取订单   add by caoheyang 20150311
        /// </summary>
        /// <param name="paraModel">查询条件实体</param>
        public virtual PageInfo<T> GetOrdersAppToSql<T>(Ets.Model.ParameterModel.Bussiness.BussOrderParaModelApp paraModel)
        {
            #region where
            string whereStr = "1=1 ";  //where查询条件实体类
            if (paraModel.userId != null)  //订单商户id
                whereStr += " and o.businessId=" + paraModel.userId.ToString();
            //订单状态
            if (paraModel.Status != null)
            {
                if (paraModel.Status == 4)
                {
                    whereStr += " and (o.Status = " + OrderConst.ORDER_NEW + " or o.Status=" + OrderConst.ORDER_ACCEPT + ")";
                }
                else
                {
                    whereStr += " and o.Status=" + paraModel.Status.ToString();
                }
            }
            if (paraModel.OrderFrom > 0)
            {
                whereStr += " and o.OrderFrom=" + paraModel.OrderFrom;
            }

            #endregion

            string orderByColumn = " o.ActualDoneDate DESC ";  //排序条件
            string columnList = @"
                                    CONVERT(VARCHAR(5),o.ActualDoneDate,108) AS ActualDoneDate,
                                    o.Amount,
                                    o.IsPay,
                                    o.OrderNo,
                                    o.PickUpAddress,
                                    CONVERT(VARCHAR(5),o.PubDate,108) AS PubDate,
                                    o.ReceviceAddress,
                                    o.ReceviceName,
                                    o.RecevicePhoneNo,
                                    o.Remark,
                                    o.Status,
                                    o.ReceviceLongitude,
                                    o.ReceviceLatitude,
                                    b.Id as BusinessId,
                                    b.Name as BusinessName,
                                    b.Longitude,
                                    b.Latitude,
                                    b.Name as PickUpName,
                                    c.TrueName as SuperManName,
                                    c.PhoneNo as SuperManPhone,
                                    o.OrderFrom,
                                    isnull(o.OriginalOrderNo,'') as OriginalOrderNo";
            string tableList = @" [order](nolock) as o 
                                   LEFT join business(nolock) as b on o.businessId=b.Id
                                   LEFT join clienter(nolock) as c on o.clienterId=c.Id ";  //表名

            return new PageHelper().GetPages<T>(SuperMan_Read, paraModel.PagingResult.PageIndex, whereStr, orderByColumn, columnList, tableList, paraModel.PagingResult.PageSize, true);
        }

        /// <summary>
        /// 商户获取第三方订单   add by 平扬 20150420
        /// </summary>
        /// <param name="paraModel">查询条件实体</param>
        public virtual PageInfo<T> GetOtherOrdersAppToSql<T>(BussOrderParaModelApp paraModel)
        {
            #region where
            string whereStr = "1=1 ";  //where查询条件实体类
            if (paraModel.userId != null)  //订单商户id
                whereStr += " and o.businessId=" + paraModel.userId.ToString();
            if (paraModel.Status != null)  //状态id
                whereStr += " and o.Status=" + paraModel.Status.ToString();
            #endregion

            string orderByColumn = " o.PubDate DESC ";  //排序条件
            string columnList = @"  o.Amount,
                                    o.OrderNo,
                                    o.PickUpAddress,
                                    CONVERT(VARCHAR(5),o.PubDate,108) AS PubDate,
                                    o.ReceviceAddress,
                                    o.ReceviceName,
                                    o.RecevicePhoneNo,
                                    o.Remark,
                                    o.Status,
                                    o.ReceviceLongitude,
                                    o.ReceviceLatitude,
                                    b.Id as BusinessId,
                                    b.Name as BusinessName,
                                    b.Longitude,
                                    b.Latitude,
                                    b.Name as PickUpName,
                                    o.OrderFrom,
                                    isnull(o.OriginalOrderNo,'') as OriginalOrderNo";
            string tableList = @" [order](nolock) as o 
                                   LEFT join business(nolock) as b on o.businessId=b.Id ";  //表名

            return new PageHelper().GetPages<T>(SuperMan_Read, paraModel.PagingResult.PageIndex, whereStr, orderByColumn, columnList, tableList, paraModel.PagingResult.PageSize, true);
        }


        /// <summary>
        /// 商户结算列表--2015.3.12 平扬
        /// </summary>
        /// <param name="t1">开始计算日期</param>
        /// <param name="t2">结束日期</param>
        /// <param name="name">商户姓名</param>
        /// <returns></returns>
        public IList<BusinessCommissionModel> GetBusinessCommission(DateTime t1, DateTime t2, string name, int groupid, string businessCity)
        {
            IList<BusinessCommissionModel> list = new List<BusinessCommissionModel>();
            try
            {
                string sql = @"  SELECT 
                                        BB.id ,
                                        BB.Name ,
                                        T.Amount ,
                                        T.OrderCount ,
                                        ISNULL(BB.BusinessCommission, 0) BusinessCommission ,
                                        T.TotalAmount,@t1 AS T1,@t2 AS T2 
                                 FROM   business BB WITH ( NOLOCK )
                                        INNER JOIN ( SELECT B.Id AS id ,
                                                            SUM(O.Amount) AS Amount ,
                                                            SUM(ISNULL(O.OrderCount, 0)) AS OrderCount ,
                                                            SUM(ISNULL(O.SettleMoney, 0)) AS TotalAmount
                                                     FROM   dbo.[order] O WITH ( NOLOCK )
                                                            INNER JOIN dbo.business B ON B.Id = o.businessId
                                                     WHERE  O.[Status] = 1 {0}
                                                     GROUP BY B.Id
                                                   ) AS T ON BB.Id = T.Id 
                                  ";
                string where = " and DATEDIFF(s, O.ActualDoneDate,@t2)>1 and DATEDIFF(s, @t1,O.ActualDoneDate)>1 ";


                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("groupid", groupid);
                dbParameters.AddWithValue("t1", t1);
                dbParameters.AddWithValue("t2", t2);
                if (!string.IsNullOrEmpty(name))
                {
                    where += " and Name=@name ";
                    dbParameters.AddWithValue("name", name);
                }
                if (groupid != 0)
                {
                    where += " and groupid=@groupid";
                    dbParameters.AddWithValue("groupid", groupid);
                }
                if (!string.IsNullOrEmpty(businessCity))
                {
                    where += " AND B.City=@City";
                    dbParameters.AddWithValue("City", businessCity);
                }
                sql = string.Format(sql, where);
                DataTable dt = DbHelper.ExecuteDataset(Config.SuperMan_Read, sql, dbParameters).Tables[0];
                list = ConvertDataTableList<BusinessCommissionModel>(dt);
            }
            catch (Exception)
            {
                throw;
            }
            return list;
        }


        /// <summary>
        /// 设置结算比例-平扬 2015.3.12
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="price">结算比例</param>
        /// <param name="waisongfei">外送费</param>
        /// <returns></returns>
        public bool setCommission(int id, decimal price, decimal waisongfei)
        {
            bool reslut = false;
            try
            {
                string sql = " update business set BusinessCommission=@BusinessCommission,DistribSubsidy=@DistribSubsidy where id=@id ";
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("BusinessCommission", price);
                dbParameters.AddWithValue("DistribSubsidy", waisongfei);
                dbParameters.AddWithValue("id", id);
                int i = DbHelper.ExecuteNonQuery(Config.SuperMan_Write, sql, dbParameters);
                if (i > 0) reslut = true;
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriter(ex, "设置结算比例-外送费");
                throw;
            }
            return reslut;

        }

        /// <summary>
        /// 设置结算比例
        /// danny-20150504
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyCommission(BusListResultModel model)
        {
            bool reslut = false;
            try
            {
                string sql = @"update business set  ";
                //if(model.DistribSubsidy>0)
                //{
                sql += "DistribSubsidy=@DistribSubsidy,";
                //}
                if (model.CommissionType > 0)
                {
                    if (model.CommissionType == 1)
                    {
                        if (model.BusinessCommission > 0)
                        {
                            sql += "BusinessCommission=@BusinessCommission,";
                        }
                    }
                    else
                    {
                        if (model.CommissionFixValue > 0)
                        {
                            sql += "CommissionFixValue=@CommissionFixValue,";
                        }
                    }
                    sql += "CommissionType=@CommissionType,";
                }
                if (model.BusinessGroupId > 0)
                {
                    sql += "BusinessGroupId=@BusinessGroupId,";
                }
                sql = sql.TrimEnd(',');
                sql += " where id=@id";
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("BusinessCommission", model.BusinessCommission);
                dbParameters.AddWithValue("DistribSubsidy", model.DistribSubsidy);
                dbParameters.AddWithValue("id", model.Id);
                dbParameters.AddWithValue("CommissionType", model.CommissionType);
                dbParameters.AddWithValue("CommissionFixValue", model.CommissionFixValue);
                dbParameters.AddWithValue("BusinessGroupId", model.BusinessGroupId);
                int i = DbHelper.ExecuteNonQuery(Config.SuperMan_Write, sql, dbParameters);
                if (i > 0) reslut = true;
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriter(ex, "设置结算比例-外送费");
                throw;
            }
            return reslut;
        }

        /// <summary>
        /// 检查当前商户是否存在 
        /// 窦海超
        /// 2015年3月16日 13:56:18
        /// </summary>
        /// <param name="PhoneNo">电话号码</param>
        /// <returns>是否存在,true存在，false不存在</returns>
        public bool CheckBusinessExistPhone(string PhoneNo)
        {
            try
            {
                string sql = "SELECT COUNT(*) FROM dbo.business(NOLOCK) WHERE PhoneNo=@PhoneNo";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.Add("@PhoneNo", SqlDbType.NVarChar);
                parm.SetValue("@PhoneNo", PhoneNo);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm)) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "检查当前商户是否存在");
                return false;
                throw;
            }
        }
        /// <summary>
        /// 获取商户信息
        /// danny-20150316
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetBusinesses<T>(BusinessSearchCriteria criteria)
        {
            string columnList = @"   b.Id
                                    ,b.Name
                                    ,b.City
                                    ,b.district
                                    ,b.PhoneNo
                                    ,b.PhoneNo2
                                    ,b.Password
                                    ,b.CheckPicUrl
                                    ,b.IDCard
                                    ,b.Address
                                    ,b.Landline
                                    ,b.Longitude
                                    ,b.Latitude
                                    ,b.Status
                                    ,b.InsertTime
                                    ,b.districtId
                                    ,b.CityId
                                    ,b.GroupId
                                    ,b.OriginalBusiId
                                    ,b.ProvinceCode
                                    ,b.CityCode
                                    ,b.AreaCode
                                    ,b.Province
                                    ,b.CommissionTypeId
                                    ,b.DistribSubsidy
                                    ,b.BusinessCommission
                                    ,g.GroupName
                                    ,b.CommissionType
                                    ,b.CommissionFixValue
                                    ,b.BusinessGroupId
                                    ,bg.Name BusinessGroupName";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(criteria.businessName))
            {
                sbSqlWhere.AppendFormat(" AND b.Name='{0}' ", criteria.businessName.Trim());
            }
            if (!string.IsNullOrEmpty(criteria.businessPhone))
            {
                sbSqlWhere.AppendFormat(" AND b.PhoneNo='{0}' ", criteria.businessPhone.Trim());
            }
            if (criteria.Status != -1)
            {
                sbSqlWhere.AppendFormat(" AND b.Status={0} ", criteria.Status);
            }
            if (criteria.BusinessCommission > 0)
            {
                sbSqlWhere.AppendFormat(" AND b.BusinessCommission={0} ", criteria.BusinessCommission);
            }
            if (criteria.GroupId != null && criteria.GroupId != 0)
            {
                sbSqlWhere.AppendFormat(" AND b.GroupId={0} ", criteria.GroupId);
            }
            if (criteria.BusinessGroupId != null && criteria.BusinessGroupId > 0)
            {
                sbSqlWhere.AppendFormat(" AND b.BusinessGroupId={0} ", criteria.BusinessGroupId);
            }
            if (criteria.CommissionType != null && criteria.CommissionType > 0)
            {
                sbSqlWhere.AppendFormat(" AND b.CommissionType={0} ", criteria.CommissionType);
            }
            if (!string.IsNullOrEmpty(criteria.businessCity))
            {
                sbSqlWhere.AppendFormat(" AND b.City='{0}' ", criteria.businessCity.Trim());
            }
            string tableList = @" business  b WITH (NOLOCK)  
                                LEFT JOIN dbo.[group] g WITH(NOLOCK) ON g.Id = b.GroupId 
                                JOIN dbo.[BusinessGroup]  bg WITH ( NOLOCK ) ON  b.BusinessGroupId=bg.Id";
            string orderByColumn = " b.Id DESC";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }
        /// <summary>
        ///  新增店铺
        ///  窦海超
        ///  2015年3月16日 15:19:47
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回商铺ID</returns>
        public int InsertBusiness(RegisterInfoModel model)
        {
            string sql = @"
                           INSERT INTO dbo.business
                            ( Name ,
                              City ,
                              district ,
                              PhoneNo ,
                              PhoneNo2 ,
                              Password ,
                              CheckPicUrl ,
                              IDCard ,
                              Address ,
                              Landline ,
                              Longitude ,
                              Latitude ,
                              Status ,
                              InsertTime ,
                              districtId ,
                              CityId ,
                              GroupId ,
                              OriginalBusiId ,
                              ProvinceCode ,
                              CityCode ,
                              AreaCode ,
                              Province ,
                              CommissionTypeId ,
                              DistribSubsidy ,
                              BusinessCommission
                            )
                    VALUES  ( N'' , -- Name - nvarchar(100)
                              @City , -- City - nvarchar(100)
                              N'' , -- district - nvarchar(200)
                              @PhoneNo , -- PhoneNo - nvarchar(20)
                              N'' , -- PhoneNo2 - nvarchar(20)
                              @Password , -- Password - nvarchar(255)
                              N'' , -- CheckPicUrl - nvarchar(255)
                              N'' , -- IDCard - nvarchar(45)
                              N'' , -- Address - nvarchar(255)
                              N'' , -- Landline - nvarchar(15)
                              0.0 , -- Longitude - float
                              0.0 , -- Latitude - float
                              0 , -- Status - tinyint
                              GETDATE() , -- InsertTime - datetime
                              '0' , -- districtId - nvarchar(45)
                              @CityId , -- CityId - nvarchar(45)
                              @GroupId , -- GroupId - int
                              0 , -- OriginalBusiId - int
                              N'' , -- ProvinceCode - nvarchar(20)
                              N'' , -- CityCode - nvarchar(20)
                              N'' , -- AreaCode - nvarchar(20)
                              N'' , -- Province - nvarchar(20)
                              1 , -- CommissionTypeId - int
                              NULL , -- DistribSubsidy - numeric
                              NULL  -- BusinessCommission - decimal
                            )SELECT @@IDENTITY
                        ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@City", model.city);
            parm.AddWithValue("@Password", model.passWord);
            parm.Add("@PhoneNo", SqlDbType.NVarChar);
            parm.SetValue("@PhoneNo", model.phoneNo);
            parm.AddWithValue("@CityId", model.CityId);
            parm.AddWithValue("@GroupId", model.GroupId);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, sql, parm));
        }


        /// <summary>
        /// B端登录
        /// </summary>
        /// <param name="model">用户名称，用户密码</param>
        /// <returns>返回该用户实体</returns>
        public DataTable LoginSql(LoginModel model)
        {
            string sql = @"SELECT  top 1 
                        Id AS userId,
                        status,
                        city ,
                        districtId,
                        district,
                        Address,
                        Landline,
                        Name,
                        cityId,
                        phoneNo,
                        PhoneNo2,
                        DistribSubsidy,ISNULL(OriginalBusiId,0) AS OriginalBusiId
                        FROM business(nolock) where PhoneNo=@PhoneNo AND Password=@Password order by id desc";

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@PhoneNo", SqlDbType.NVarChar);
            parm.SetValue("@PhoneNo", model.phoneNo);
            parm.AddWithValue("@Password", model.passWord);
            return DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
        }


        /// <summary>
        /// 根据商户id获取商户
        /// </summary>
        /// <param name="busiId"></param>
        /// <returns></returns>
        public BusListResultModel GetBusiness(int busiId)
        {
            BusListResultModel busi = new BusListResultModel();
            string selSql = @" select 
                                b.Id ,
                                b.Name ,
                                b.City ,
                                b.district ,
                                b.PhoneNo ,
                                b.PhoneNo2 ,
                                b.IDCard ,
                                b.[Address] ,
                                b.Landline ,
                                b.Longitude ,
                                b.Latitude ,
                                b.[Status] ,
                                b.InsertTime ,
                                b.districtId ,
                                b.CityId ,
                                b.GroupId , 
                                b.ProvinceCode ,
                                b.CityCode ,
                                b.AreaCode ,
                                b.Province ,
                                b.DistribSubsidy,
                                b.BusinessCommission,
                                b.CommissionType,
                                b.CommissionFixValue,
                                b.BusinessGroupId,
                                BusinessGroup.StrategyId
                                FROM dbo.business as b WITH(NOLOCK)
                                left join BusinessGroup on b.BusinessGroupId=BusinessGroup.Id
                                WHERE b.Id = @busiId";
            ///TODO 类型？
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@busiId", busiId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, selSql, parm));
            if (dt != null)
                busi = DataTableHelper.ConvertDataTableList<BusListResultModel>(dt)[0];
            return busi;
        }


        /// <summary>
        /// 根据商户id获取商户
        /// </summary>
        /// <param name="busiId"></param>
        /// <returns></returns>
        public BusListResultModel GetBusiness(int originalBusiId, int groupId)
        {
            BusListResultModel busi = new BusListResultModel();
            string selSql = @" SELECT  
         Id ,
         Name ,
         City ,
         district ,
         PhoneNo ,
         PhoneNo2 ,
         IDCard ,
         [Address] ,
         Landline ,
         Longitude ,
         Latitude ,
         [Status] ,
         InsertTime ,
         districtId ,
         CityId ,
         GroupId , 
         ProvinceCode ,
         CityCode ,
         AreaCode ,
         Province ,
         DistribSubsidy,
         BusinessCommission ,
         OriginalBusiId
         FROM dbo.business WITH(NOLOCK) WHERE OriginalBusiId = @busiId AND GroupId=@groupId";

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@busiId", originalBusiId);

            parm.AddWithValue("@GroupId", groupId);

            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, selSql, parm));
            if (dt != null && dt.Rows.Count > 0)
            {
                busi = DataTableHelper.ConvertDataTableList<BusListResultModel>(dt)[0];
                return busi;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 更新审核状态
        /// danny-20150317
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enumStatusType"></param>
        /// <returns></returns>
        public bool UpdateAuditStatus(int id, EnumStatusType enumStatusType)
        {
            var juWangKeBusiAuditUrl = ConfigSettings.Instance.JuWangKeBusiAuditCallBack;
            bool reslut = false;
            try
            {
                string sql = string.Empty;
                if (enumStatusType == EnumStatusType.审核通过)
                {

                    sql = string.Format(" update business set Status={0} where id=@id;", ConstValues.BUSINESS_AUDITPASS);
                }
                else if (enumStatusType == EnumStatusType.审核取消)
                {
                    sql = string.Format(" update business set Status={0} where id=@id;", ConstValues.BUSINESS_AUDITCANCEL);
                }
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("id", id);
                int i = DbHelper.ExecuteNonQuery(Config.SuperMan_Write, sql, dbParameters);

                if (i > 0)
                {
                    //调用第三方接口 ，聚网客商户审核通过后调用接口
                    //这里不建议使用 1 数字，而是根据 配置文件中的 appkey来获取 groupid
                    var busi = GetBusiness(id);
                    if (busi.GroupId == 1 && busi.OriginalBusiId > 0 && enumStatusType == EnumStatusType.审核通过)
                    {
                        string str = HTTPHelper.HttpPost(juWangKeBusiAuditUrl, "supplier_id=" + busi.OriginalBusiId);
                    }

                    //HTTPHelper.HttpPost()
                    reslut = true;
                }
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
        /// 根据城市信息查询当前城市下该集团的所有商户信息
        ///  danny-20150317
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IList<BusListResultModel> GetBussinessByCityInfo(BusinessSearchCriteria criteria)
        {
            string sql = @"SELECT  Id 
                        Name
                        FROM business(nolock) where 1=1";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@GroupId", criteria.GroupId);
            parm.AddWithValue("@ProvinceCode", criteria.ProvinceCode);
            parm.AddWithValue("@CityCode", criteria.CityCode);
            if (criteria.GroupId != null && criteria.GroupId != 0)
            {
                sql += " AND GroupId=@GroupId";
            }
            if (!string.IsNullOrWhiteSpace(criteria.ProvinceCode))
            {
                sql += " AND ProvinceCode=@ProvinceCode";
            }
            if (!string.IsNullOrWhiteSpace(criteria.CityCode))
            {
                sql += " AND CityCode=@CityCode";
            }
            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
            var list = ConvertDataTableList<BusListResultModel>(dt);
            return list;
        }


        /// <summary>
        /// 根据手机号获取用商家信息
        /// 窦海超
        /// 2015年3月23日 19:00:52
        /// </summary>
        /// <param name="PhoneNo">手机号</param>
        /// <returns>商家信息</returns>
        public BusListResultModel GetBusinessByPhoneNo(string PhoneNo)
        {
            string sql = @"SELECT Id FROM dbo.business(NOLOCK) WHERE PhoneNo=@PhoneNo";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@PhoneNo", SqlDbType.NVarChar);
            parm.SetValue("@PhoneNo", PhoneNo);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            IList<BusListResultModel> list = MapRows<BusListResultModel>(dt);
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            return list[0];
        }

        /// <summary>
        /// 更新商户端密码
        /// 窦海超
        /// 2015年3月23日 19:05:39
        /// </summary>
        /// <param name="BusinessId">商户ID</param>
        /// <param name="BusinessPwd">更新密码</param>
        /// <returns></returns>
        public bool UpdateBusinessPwdSql(int BusinessId, string BusinessPwd)
        {
            if (BusinessId <= 0)
            {
                return false;
            }
            string sql = "UPDATE dbo.business SET [Password]=@Password WHERE id =" + BusinessId;
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Password", BusinessPwd);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }

        /// <summary>
        /// 获取商户端的统计数量
        /// 窦海超
        /// 2015年3月23日 20:19:02
        /// </summary>
        /// <param name="BusinessId">商户ID</param>
        /// <returns></returns>
        public BusiOrderCountResultModel GetOrderCountDataSql(int BusinessId)
        {
            string sql = @"
                    SELECT 
                    ISNULL(SUM(CASE WHEN ([Status]=@order_new OR [Status]=@order_Finish OR [Status]=2) AND CONVERT(CHAR(10),PubDate,120)=CONVERT(CHAR(10),GETDATE(),120) THEN 1 ELSE 0 END ),0) AS todayPublish,
                    ISNULL(SUM(CASE WHEN ([Status]=@order_new  OR [Status]=@order_Finish OR [Status]=2) AND CONVERT(CHAR(10),PubDate,120)=CONVERT(CHAR(10),GETDATE(),120) THEN Amount ELSE 0 END),0) AS todayPublishAmount,

                    ISNULL(SUM(CASE WHEN [Status]=@order_Finish AND CONVERT(CHAR(10),PubDate,120)=CONVERT(CHAR(10),GETDATE(),120) THEN 1 ELSE 0 END),0) AS todayDone,
                    ISNULL(SUM(CASE WHEN [Status]=@order_Finish AND CONVERT(CHAR(10),PubDate,120)=CONVERT(CHAR(10),GETDATE(),120) THEN Amount ELSE 0 END),0) AS todayDoneAmount,

                    ISNULL(SUM(CASE WHEN ([Status]=@order_new  OR [Status]=@order_Finish OR [Status]=2) AND CONVERT(CHAR(7),PubDate,120)=CONVERT(CHAR(7),GETDATE(),120) THEN 1 ELSE 0 END),0) AS monthPublish,
                    ISNULL(SUM(CASE WHEN ([Status]=@order_new  OR [Status]=@order_Finish OR [Status]=2) AND CONVERT(CHAR(7),PubDate,120)=CONVERT(CHAR(7),GETDATE(),120) THEN Amount ELSE 0 END),0) AS monthPublishAmount,

                    ISNULL(SUM(CASE WHEN [Status]=@order_Finish AND CONVERT(CHAR(7),PubDate,120)=CONVERT(CHAR(7),GETDATE(),120) THEN 1 ELSE 0 END),0) AS monthDone,
                    ISNULL(SUM(CASE WHEN [Status]=@order_Finish AND CONVERT(CHAR(7),PubDate,120)=CONVERT(CHAR(7),GETDATE(),120) THEN Amount ELSE 0 END),0) AS monthDoneAmount
                     FROM dbo.[order](NOLOCK)
                    WHERE businessId=@businessId
                ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@businessId", BusinessId);
            parm.AddWithValue("@order_new", ConstValues.ORDER_NEW);
            parm.AddWithValue("@order_Finish", ConstValues.ORDER_FINISH);

            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            IList<BusiOrderCountResultModel> list = MapRows<BusiOrderCountResultModel>(dt);
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            return list[0];
        }

        /// <summary>
        /// 判断该 商户是否有资格 
        /// wc
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public bool HaveQualification(int businessId)
        {
            try
            {
                //状态为1 表示该骑士 已通过审核
                string sql = "SELECT COUNT(1) FROM dbo.business(NOLOCK) WHERE [Status] = 1 AND Id = @businessId ";
                ///TODO 参数更改，加上类型
                //var dbParameters = DbHelper.CreateDbParameters("businessid", DbType.Int32, 4, businessId);
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.AddWithValue("@businessId", businessId);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm)) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "检查当前商户是否存在");
                return false;
                throw;
            }
        }
        /// <summary>
        /// 根据集团id获取集团名称
        /// danny-20150324
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public string GetGroupNameById(int groupId)
        {
            string sql = @"SELECT  Name
                        FROM business WITH(NOLOCK) WHERE  GroupId=@GroupId ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@GroupId", groupId);
            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["Name"].ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取所有可用的集团信息数据
        /// danny-20150324
        /// </summary>
        /// <returns></returns>
        public IList<GroupModel> GetGroups()
        {
            string sql = @"SELECT    [Id]
                                    ,[GroupName]
                          FROM [group] WHERE IsValid=@IsValid";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@IsValid", ConstValues.GroupIsIsValid);
            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
            var list = ConvertDataTableList<GroupModel>(dt);
            return list;
        }

        /// <summary>
        /// 获取当天商家总数
        /// 窦海超
        /// 2015年3月24日 14:11:10
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public HomeCountTitleModel GetCurrentBusinessCount(HomeCountTitleModel model)
        {
            string sql = @"SELECT COUNT(Id) AS BusinessCount FROM dbo.business(NOLOCK) 
                           WHERE [status]=1  --商家总数：";
            model.BusinessCount = ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql));
            return model;
        }

        /// <summary>
        /// 根据商户Id修改外送费
        /// wc
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="waiSongFei"></param>
        /// <returns></returns>
        public int ModifyWaiMaiPrice(int businessId, decimal waiSongFei)
        {
            string upSql = @"UPDATE dbo.business SET DistribSubsidy = @waiSongFei WHERE Id = @businessId";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@waiSongFei", waiSongFei);
            parm.AddWithValue("@businessId", businessId);
            object executeScalar = DbHelper.ExecuteNonQuery(SuperMan_Write, upSql, parm);
            return ParseHelper.ToInt(executeScalar, 0);
        }

        /// <summary>
        /// 原平台id和集团id判断是否存在该商户
        /// 平扬
        /// 2015年3月26日 17:00:52
        /// </summary>
        /// <param name="bid">原平台商户Id</param>
        /// <param name="groupid">集团id</param>
        /// <returns>商家信息</returns>
        public Business CheckExistBusiness(int bid, int groupid)
        {
            string sql = @"select Id,Status from dbo.business where OriginalBusiId=@OriginalBusiId and GroupId=@GroupId";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OriginalBusiId", bid);
            parm.AddWithValue("@GroupId", groupid);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<Business>(dt)[0];
        }

        /// <summary>
        /// 原平台id和订单来源查询订单,是否存在该订单
        /// 平扬
        /// 2015年3月26日 17:00:52
        /// </summary>
        /// <param name="orderNO">原平台商户Id</param>
        /// <param name="orderFrom">订单来源</param>
        /// <param name="orderType">orderType</param>
        /// <returns>商家信息</returns 
        public order GetOrderByOrderNoAndOrderFrom(string orderNO, int orderFrom, int orderType)
        {
            string sql = @" select Id OrderId,OrderNo,OriginalOrderNo,businessId from dbo.[order] with(nolock) where OriginalOrderNo=@OriginalOrderNo and OrderFrom=@OrderFrom ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OriginalOrderNo", orderNO);
            parm.AddWithValue("@OrderFrom", orderFrom);
            if (orderType > 0)
            {
                sql += " and OrderType=@OrderType ";
                parm.AddWithValue("@OrderType", orderType);
            }
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt != null)
            {
                return MapRows<order>(dt)[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 更新订单状态，通过第三方订单号 和 订单 来源更新
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderStatus"></param>
        public bool UpdateOrder(string oriOrderNo, int orderFrom, OrderStatus orderStatus)
        {
            string sql = string.Format(@"UPDATE dbo.[order] SET [Status]=@Status 
                            output Inserted.Id,GETDATE(),'{0}','{1}',Inserted.businessId,Inserted.[Status],{2}
                            into dbo.OrderSubsidiesLog(OrderId,InsertTime,OptName,Remark,OptId,OrderStatus,[Platform])
                            WHERE OriginalOrderNo=@OriginalOrderNo and OrderFrom=@OrderFrom ", SuperPlatform.商家, ConstValues.CancelOrder, (int)SuperPlatform.商家);
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OriginalOrderNo", oriOrderNo);
            parm.AddWithValue("@OrderFrom", orderFrom);
            parm.AddWithValue("@Status", orderStatus);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }


        /// <summary>
        ///  新增第三方店铺
        ///  窦海超
        ///  2015年3月16日 15:19:47
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回商铺ID</returns>
        public int InsertOtherBusiness(Business model)
        {
            string sql = @"
                           INSERT INTO dbo.business
                            ( Name ,
                              City ,
                              district ,
                              PhoneNo ,
                              PhoneNo2 ,
                              Password ,
                              CheckPicUrl ,
                              IDCard ,
                              Address ,
                              Landline ,
                              Longitude ,
                              Latitude ,
                              Status ,
                              InsertTime ,
                              districtId ,
                              CityId ,
                              GroupId ,
                              OriginalBusiId ,
                              ProvinceCode ,
                              CityCode ,
                              AreaCode ,
                              Province ,
                              CommissionTypeId ,
                              DistribSubsidy ,
                              BusinessCommission
                            )
                    VALUES  ( @Name , -- Name - nvarchar(100)
                              @City , -- City - nvarchar(100)
                              @district , -- district - nvarchar(200)
                              @PhoneNo , -- PhoneNo - nvarchar(20)
                              @PhoneNo2 , -- PhoneNo2 - nvarchar(20)
                              @Password , -- Password - nvarchar(255)
                              @CheckPicUrl , -- CheckPicUrl - nvarchar(255)
                              @IDCard , -- IDCard - nvarchar(45)
                              @Address , -- Address - nvarchar(255)
                              N'' , -- Landline - nvarchar(15)
                              @Longitude , -- Longitude - float
                              @Latitude , -- Latitude - float
                              @Status, -- Status - tinyint
                              GETDATE() , -- InsertTime - datetime
                              @districtId , -- districtId - nvarchar(45)
                              @CityId , -- CityId - nvarchar(45)
                              @GroupId , -- GroupId - int
                              @OriginalBusiId , -- OriginalBusiId - int
                              @ProvinceCode , -- ProvinceCode - nvarchar(20)
                              @CityCode , -- CityCode - nvarchar(20)
                              @AreaCode , -- AreaCode - nvarchar(20)
                              @Province , -- Province - nvarchar(20)
                              @CommissionTypeId , -- CommissionTypeId - int
                              @DistribSubsidy , -- DistribSubsidy - numeric
                              NULL  -- BusinessCommission - decimal
                            );select SCOPE_IDENTITY() as id;
                        ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Name", model.Name);
            parm.AddWithValue("@City", model.City);
            parm.AddWithValue("@district", model.district);
            parm.Add("@PhoneNo", SqlDbType.NVarChar);
            parm.SetValue("@PhoneNo", model.PhoneNo);

            parm.Add("@PhoneNo2", SqlDbType.NVarChar);
            parm.SetValue("@PhoneNo2", model.PhoneNo2);

            parm.AddWithValue("@Password", model.Password);
            parm.AddWithValue("@CheckPicUrl", model.CheckPicUrl);


            parm.AddWithValue("@IDCard", model.IDCard);
            parm.AddWithValue("@Address", model.Address);
            parm.AddWithValue("@Longitude", model.Longitude);
            parm.AddWithValue("@Latitude", model.Latitude);
            parm.AddWithValue("@Status", model.Status);
            parm.AddWithValue("@districtId", model.districtId);
            parm.AddWithValue("@CityId", model.CityId);
            parm.AddWithValue("@GroupId", model.GroupId);
            parm.AddWithValue("@OriginalBusiId", model.OriginalBusiId);
            parm.AddWithValue("@CityCode", model.CityCode);
            parm.AddWithValue("@AreaCode", model.AreaCode);
            parm.AddWithValue("@Province", model.Province);
            parm.AddWithValue("@CommissionTypeId", model.CommissionTypeId);
            parm.AddWithValue("@ProvinceCode", model.ProvinceCode);
            parm.AddWithValue("@DistribSubsidy", model.DistribSubsidy);
            object i = DbHelper.ExecuteScalar(SuperMan_Write, sql, parm);
            if (i != null)
            {
                return ParseHelper.ToInt(i.ToString());
            }
            return 0;
        }
        /// <summary>
        /// 商户统计
        /// danny-20150326
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetBusinessesCount<T>(BusinessSearchCriteria criteria)
        {
            var sbtbl = new StringBuilder(@" (select	b.Name,
		                                                b.PhoneNo,
		                                                COUNT(*) OrderCount,
		                                                SUM(o.Amount) OrderAmountCount
                                                from business b with(nolock)
                                                join [order] o on b.Id=o.businessId
                                                where 1=1 ");
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
            sbtbl.Append(" group by b.Id,b.Name,b.PhoneNo ) tbl ");
            string columnList = @"   tbl.Name
                                    ,tbl.PhoneNo
            				        ,tbl.OrderCount
            				        ,tbl.OrderAmountCount ";

            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(criteria.businessName))
            {
                sbSqlWhere.AppendFormat(" AND Name='{0}' ", criteria.businessName);
            }
            if (!string.IsNullOrEmpty(criteria.businessPhone))
            {
                sbSqlWhere.AppendFormat(" AND PhoneNo='{0}' ", criteria.businessPhone);
            }
            string tableList = sbtbl.ToString();
            string orderByColumn = " tbl.OrderCount DESC ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PagingRequest.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PagingRequest.PageSize, true);
        }
        /// <summary>
        /// 修改商户地址西信息
        /// 返回商户修改后的状态
        /// wc
        /// </summary>
        /// <param name="business"></param>
        /// <returns>商户的当前状态</returns>
        public int UpdateBusinessAddressInfo(Business business)
        {
            string upSql = @"UPDATE  dbo.business
                            SET     [Address] = @Address ,
                                    PhoneNo2 = @PhoneNo2 ,
                                    [Name] = @Name ,
                                    Landline = @Landline ,
                                    district = @district ,
                                    districtId = @districtId ,
                                    Longitude = @Longitude ,
                                    Latitude = @Latitude ,
                                    [Status]= @Status
                            OUTPUT  Inserted.[Status]
                            WHERE   Id = @busiID";

            IDbParameters parm = DbHelper.CreateDbParameters();

            parm.AddWithValue("@Address", business.Address);
            parm.Add("@PhoneNo2", SqlDbType.NVarChar);
            parm.SetValue("@PhoneNo2", business.PhoneNo2);
            parm.AddWithValue("@Name", business.Name);
            parm.AddWithValue("@Landline", business.Landline);
            parm.AddWithValue("@district", business.district);
            parm.AddWithValue("@districtId", business.districtId);
            parm.AddWithValue("@Longitude", business.Longitude);
            parm.AddWithValue("@Latitude", business.Latitude);
            parm.AddWithValue("@Status", business.Status);
            parm.AddWithValue("@busiID", business.Id);
            try
            {
                object executeScalar = DbHelper.ExecuteScalar(SuperMan_Write, upSql, parm);
                return ParseHelper.ToInt(executeScalar, -1);
            }
            catch (Exception ex)
            {
                //记日志
                return -1;
            }

        }
        /// <summary>
        /// 更新图片地址信息 
        /// wc
        /// </summary>
        /// <param name="busiId"></param>
        /// <param name="picName"></param>
        /// <returns></returns>
        public int UpdateBusinessPicInfo(int busiId, string picName)
        {
            string upSql = @"UPDATE  dbo.business
                            SET     CheckPicUrl = @CheckPicUrl ,
                                    [Status] = @Status
                            OUTPUT  Inserted.[Status]
                            WHERE   Id = @busiID ";

            IDbParameters parm = DbHelper.CreateDbParameters();

            parm.AddWithValue("@CheckPicUrl", picName);
            parm.AddWithValue("@Status", ConstValues.BUSINESS_AUDITPASSING);
            parm.AddWithValue("@busiID", busiId);

            try
            {
                object executeScalar = DbHelper.ExecuteScalar(SuperMan_Write, upSql, parm);
                return ParseHelper.ToInt(executeScalar, -1);
            }
            catch (Exception ex)
            {
                //记日志
                return -1;
            }
        }

        /// <summary>
        /// 根据原平台商户Id和订单来源获取该商户信息
        /// 窦海超
        /// 2015年3月30日 12:50:26
        /// </summary>
        /// <param name="oriBusiId">商户ID</param>
        /// <param name="orderFrom">集团ID</param>
        /// <returns></returns>
        public Business GetBusiByOriIdAndOrderFrom(int oriBusiId, int orderFrom)
        {
            string sql = @"SELECT Id,[Status] FROM dbo.business(nolock) WHERE OriginalBusiId=@OriginalBusiId AND GroupId=@GroupId";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OriginalBusiId", oriBusiId);
            parm.AddWithValue("@GroupId", orderFrom);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<Business>(dt)[0];
        }
        /// <summary>
        /// 检查号码是否存在
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public bool CheckExistPhone(string phoneNo)
        {
            try
            {
                string sql = "SELECT 1 FROM dbo.business(NOLOCK) WHERE PhoneNo=@PhoneNo ";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.Add("@PhoneNo", SqlDbType.NVarChar);
                parm.SetValue("@PhoneNo", phoneNo);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm)) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "检查号码是否存在");
                return false;
            }
        }

        /// <summary>
        /// 获取用户状态信息
        /// 平扬
        /// 2015年3月31日 10:50:26
        /// </summary>
        /// <param name="userid">商户ID</param>
        /// <returns></returns>
        public BussinessStatusModel GetUserStatus(int userid)
        {
            string sql = @"select  Id as userid,[status] as status from dbo.business with(nolock) WHERE id=@id ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@id", userid);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<BussinessStatusModel>(dt)[0];
        }

        /// <summary>
        /// 商户配送统计
        /// danny-20150408
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetBusinessesDistributionStatisticalInfo<T>(OrderSearchCriteria criteria)
        {
            var sbtbl = new StringBuilder(@" (SELECT PubDate ,
                                                    ISNULL(SUM(CASE when a.ClienterCount=1 THEN BusinessCount END),0) OnceCount,
                                                    ISNULL(SUM(CASE when a.ClienterCount=2 THEN BusinessCount END),0) TwiceCount,
                                                    ISNULL(SUM(CASE when a.ClienterCount=3 THEN BusinessCount END),0) ThreeTimesCount,
                                                    ISNULL(SUM(CASE when a.ClienterCount=4 THEN BusinessCount END),0) FourTimesCount,
                                                    ISNULL(SUM(CASE when a.ClienterCount=5 THEN BusinessCount END),0) FiveTimesCount,
                                                    ISNULL(SUM(CASE when a.ClienterCount=6 THEN BusinessCount END),0) SixTimesCount,
                                                    ISNULL(SUM(CASE when a.ClienterCount=7 THEN BusinessCount END),0) SevenTimesCount,
                                                    ISNULL(SUM(CASE when a.ClienterCount=8 THEN BusinessCount END),0) EightTimesCount,
                                                    ISNULL(SUM(CASE when a.ClienterCount=9 THEN BusinessCount END),0) NineTimesCount,
                                                    ISNULL(SUM(CASE when a.ClienterCount>=10 THEN BusinessCount END),0) ExceedNineTimesCount
                                                FROM (
                                                    select t.PubDate,t.clienterCount ClienterCount,count(t.businessId) as BusinessCount
                                                    FROM (
                                                            select convert(char(10),o.PubDate,120) PubDate,count(DISTINCT clienterId)clienterCount ,businessId
                                                            from dbo.[order] o(nolock)
                                                            where Status=1 ");
            if (!string.IsNullOrWhiteSpace(criteria.orderPubStart))
            {
                sbtbl.AppendFormat(" AND  CONVERT(CHAR(10),PubDate,120)>=CONVERT(CHAR(10),'{0}',120) ", criteria.orderPubStart);
            }
            if (!string.IsNullOrWhiteSpace(criteria.orderPubEnd))
            {
                sbtbl.AppendFormat(" AND CONVERT(CHAR(10),PubDate,120)<=CONVERT(CHAR(10),'{0}',120) ", criteria.orderPubEnd);
            }
            sbtbl.Append(@"         group by convert(char(10),o.PubDate,120), businessId) t
                                group by t.PubDate, t.clienterCount )a 
                          group by PubDate ) tbl ");
            string columnList = @"   tbl.PubDate
                                    ,tbl.OnceCount
            				        ,tbl.TwiceCount
                                    ,tbl.ThreeTimesCount
            				        ,tbl.FourTimesCount
                                    ,tbl.FiveTimesCount
            				        ,tbl.SixTimesCount
                                    ,tbl.SevenTimesCount
            				        ,tbl.EightTimesCount
                                    ,tbl.NineTimesCount
            				        ,tbl.ExceedNineTimesCount ";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            string tableList = sbtbl.ToString();
            string orderByColumn = " tbl.PubDate DESC ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PagingRequest.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PagingRequest.PageSize, true);
        }



        /// <summary>
        ///  后台新增店铺
        ///  平扬
        ///  2015年4月17日 17:19:47
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回商铺ID</returns>
        public int addBusiness(AddBusinessModel model)
        {
            string sql = @"
                           INSERT INTO dbo.business
                            ( Name ,
                              City ,
                              district ,
                              PhoneNo ,
                              PhoneNo2 ,
                              Password ,
                              CheckPicUrl ,
                              IDCard ,
                              Address ,
                              Landline ,
                              Longitude ,
                              Latitude ,
                              Status ,
                              InsertTime ,
                              districtId ,
                              CityId ,
                              GroupId ,
                              OriginalBusiId ,
                              ProvinceCode ,
                              CityCode ,
                              AreaCode ,
                              Province ,
                              CommissionTypeId ,
                              DistribSubsidy ,
                              BusinessCommission
                            )
                    VALUES  ( @Name, -- Name - nvarchar(100)
                              @City , -- City - nvarchar(100)
                              N'' , -- district - nvarchar(200)
                              @PhoneNo , -- PhoneNo - nvarchar(20)
                              N'' , -- PhoneNo2 - nvarchar(20)
                              @Password , -- Password - nvarchar(255)
                              N'' , -- CheckPicUrl - nvarchar(255)
                              N'' , -- IDCard - nvarchar(45)
                              @Address, -- Address - nvarchar(255)
                              N'' , -- Landline - nvarchar(15)
                              0.0 , -- Longitude - float
                              0.0 , -- Latitude - float
                              1 , -- Status - tinyint
                              GETDATE() , -- InsertTime - datetime
                              '0' , -- districtId - nvarchar(45)
                              @CityId , -- CityId - nvarchar(45)
                              @GroupId , -- GroupId - int
                              0 , -- OriginalBusiId - int
                              N'' , -- ProvinceCode - nvarchar(20)
                              N'' , -- CityCode - nvarchar(20)
                              N'' , -- AreaCode - nvarchar(20)
                              N'' , -- Province - nvarchar(20)
                              1 , -- CommissionTypeId - int
                              @DistribSubsidy , -- DistribSubsidy - numeric
                              @BusinessCommission  -- BusinessCommission - decimal
                            )SELECT @@IDENTITY
                        ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@City", model.city);
            parm.AddWithValue("@Password", "A06F6A211CBEDF374FC367FA231865DE");
            parm.Add("@PhoneNo", SqlDbType.NVarChar);
            parm.SetValue("@PhoneNo", model.phoneNo);
            parm.AddWithValue("@CityId", model.CityId);
            parm.AddWithValue("@GroupId", model.GroupId);
            parm.AddWithValue("@Name", model.businessName);
            parm.AddWithValue("@Address", model.businessaddr);
            parm.AddWithValue("@DistribSubsidy", model.businessWaisong);
            parm.AddWithValue("@BusinessCommission", model.businessCommission);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, sql, parm));
        }



        /// <summary>
        /// 修改商户信息
        /// danny-20150417
        /// </summary>
        /// <param name="model"></param>
        /// <param name="orderOptionModel"></param>
        /// <returns></returns>
        public bool ModifyBusinessInfo(Business model, OrderOptionModel orderOptionModel)
        {
            string remark = orderOptionModel.OptUserName + "通过后台管理系统修改商户信息";
            string sql = string.Format(@"UPDATE business 
                                            SET Name=@Name,
                                                GroupId=@GroupId,
                                                OriginalBusiId=@OriginalBusiId,
                                                PhoneNo=@PhoneNo
                                            OUTPUT
                                              Inserted.Id,
                                              @OptId,
                                              @OptName,
                                              GETDATE(),
                                              @Platform,
                                              @Remark
                                            INTO BusinessOptionLog
                                             (BusinessId,
                                              OptId,
                                              OptName,
                                              InsertTime,
                                              Platform,
                                              Remark)
                                             WHERE  Id = @Id");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Name", model.Name);
            parm.AddWithValue("@GroupId", model.GroupId);
            parm.AddWithValue("@OriginalBusiId", model.OriginalBusiId);
            parm.AddWithValue("@Id", model.Id);
            parm.AddWithValue("@PhoneNo", model.PhoneNo);
            parm.AddWithValue("@OptId", orderOptionModel.OptUserId);
            parm.AddWithValue("@OptName", orderOptionModel.OptUserName);
            parm.AddWithValue("@Platform", 3);
            parm.AddWithValue("@Remark", remark);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }


        /// <summary>
        /// 根据ID获取对象
        /// </summary>
        public Business GetById(int id)
        {
            Business model = null;
            const string getbyidSql = @"
select  Id,Name,City,district,PhoneNo,PhoneNo2,Password,CheckPicUrl,IDCard,Address,Landline,Longitude,Latitude,Status,InsertTime,districtId,CityId,GroupId,OriginalBusiId,ProvinceCode,CityCode,AreaCode,Province,CommissionTypeId,DistribSubsidy,BusinessCommission,CommissionType,CommissionFixValue,BusinessGroupId,BalancePrice,AllowWithdrawPrice,HasWithdrawPrice
from  business (nolock)
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, getbyidSql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = MapRows<Business>(dt)[0];
            }
            return model;
        }

        /// <summary>
        ///  超人提现功能 add by caoheyang 20150509
        /// </summary>
        /// <param name="withdrawBpm">超人信息</param>
        /// <returns></returns>
        public void UpdateForWithdrawC(WithdrawBPM withdrawBpm)
        {
            const string updateSql = @"
update  business
set  BalancePrice=BalancePrice-@WithdrawPrice,AllowWithdrawPrice=AllowWithdrawPrice-@WithdrawPrice
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", withdrawBpm.BusinessId);
            dbParameters.AddWithValue("WithdrawPrice", withdrawBpm.WithdrawPrice);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

        /// <summary>
        /// 获取商家详情
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="id">商家Id</param>
        /// <returns></returns>
        public BusinessDM  GetDetails(int id)
        {
            BusinessDM businessDM = new BusinessDM();

            #region 商家表
            string queryBusinessSql = @"
select  Id,Name,City,district,PhoneNo,PhoneNo2,Password,CheckPicUrl,IDCard,
        Address,Landline,Longitude,Latitude,Status,InsertTime,districtId,CityId,GroupId,
        OriginalBusiId,ProvinceCode,CityCode,AreaCode,Province,CommissionTypeId,DistribSubsidy,
        BusinessCommission,CommissionType,CommissionFixValue,BusinessGroupId,BalancePrice,
        AllowWithdrawPrice,HasWithdrawPrice
from  Business (nolock) 
where Id=@Id";

            IDbParameters dbBusinessParameters = DbHelper.CreateDbParameters("Id", DbType.Int32, 4, id);  
            businessDM = DbHelper.QueryForObject(SuperMan_Read, queryBusinessSql, dbBusinessParameters, new BusinessRowMapper());
            #endregion

            #region 商家金融账号表
            const string queryBFAccountSql = @"
select  Id,BusinessId,TrueName,AccountNo,IsEnable,AccountType,BelongType,OpenBank,OpenSubBank,CreateBy,CreateTime,UpdateBy,UpdateTime
from  BusinessFinanceAccount (nolock) 
where BusinessId=@BusinessId and IsEnable=1";
            IDbParameters dbBFAccountParameters = DbHelper.CreateDbParameters();
            dbBFAccountParameters.AddWithValue("BusinessId", id);
            DataTable dtBFAccount = DbHelper.ExecuteDataTable(SuperMan_Read, queryBFAccountSql, dbBFAccountParameters);
            List<BusinessFinanceAccount> listBFAccount = new List<BusinessFinanceAccount>();            
            foreach (DataRow dataRow in dtBFAccount.Rows)
            {
                BusinessFinanceAccount bf = new BusinessFinanceAccount();
                bf.Id = ParseHelper.ToInt(dataRow["Id"]);
                bf.BusinessId =ParseHelper.ToInt(dataRow["BusinessId"]);
                bf.TrueName = dataRow["TrueName"].ToString();
                bf.AccountNo = ETS.Security.DES.Decrypt(dataRow["AccountNo"].ToString()); 
                bf.IsEnable = ParseHelper.ToBool(dataRow["IsEnable"]);
                bf.AccountType = ParseHelper.ToInt(dataRow["AccountType"]);
                bf.BelongType = ParseHelper.ToInt(dataRow["BelongType"]);
                if (dataRow["OpenBank"] != null && dataRow["OpenBank"]!=DBNull.Value)
                { 
                    bf.OpenBank = dataRow["OpenBank"].ToString();
                }
                if (dataRow["OpenSubBank"] != null && dataRow["OpenSubBank"] != DBNull.Value)
                {
                    bf.OpenSubBank = dataRow["OpenSubBank"].ToString();
                }
                bf.CreateBy = dataRow["CreateBy"].ToString();
                bf.CreateTime = ParseHelper.ToDatetime(dataRow["CreateTime"]);
                bf.UpdateBy = dataRow["UpdateBy"].ToString();
                bf.UpdateTime = ParseHelper.ToDatetime(dataRow["UpdateTime"]);
                listBFAccount.Add(bf);
            }            
            businessDM.listBFAcount = listBFAccount;            
            #endregion          
             
            return businessDM;
        }


        /// <summary>
        /// 判断商户是否存在        
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// </summary>
        /// <param name="id">商户Id</param>
        /// <returns></returns>
        public bool IsExist(int id)
        {
            bool isExist;
            string querySql = @" SELECT COUNT(1)
 FROM   dbo.[business] WITH ( NOLOCK ) 
 WHERE  id = @id";

            IDbParameters dbParameters = DbHelper.CreateDbParameters("Id", DbType.Int32, 4, id);              
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, querySql, dbParameters);
            isExist = ParseHelper.ToInt(executeScalar, 0) > 0;

            return isExist;
        }

        #region  Nested type: BusinessRowMapper

        /// <summary>
        /// 绑定对象
        /// </summary>
        private class BusinessRowMapper : IDataTableRowMapper<BusinessDM>
        {
            public BusinessDM MapRow(DataRow dataReader)
            {
                var result = new BusinessDM();
                object obj;
                obj = dataReader["Id"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.Id = int.Parse(obj.ToString());
                }
                result.Name = dataReader["Name"].ToString();
                result.City = dataReader["City"].ToString();
                result.district = dataReader["district"].ToString();
                result.PhoneNo = dataReader["PhoneNo"].ToString();
                result.PhoneNo2 = dataReader["PhoneNo2"].ToString();
                result.Password = dataReader["Password"].ToString();
                result.CheckPicUrl = Ets.Model.Common.ImageCommon.ReceiptPicConvert(dataReader["CheckPicUrl"].ToString())[0];
                result.IDCard = dataReader["IDCard"].ToString();
                result.Address = dataReader["Address"].ToString();
                result.Landline = dataReader["Landline"].ToString();
                obj = dataReader["Longitude"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.Longitude = decimal.Parse(obj.ToString());
                }
                obj = dataReader["Latitude"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.Latitude = decimal.Parse(obj.ToString());
                }
                obj = dataReader["Status"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.Status = int.Parse(obj.ToString());
                }
                obj = dataReader["InsertTime"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.InsertTime = DateTime.Parse(obj.ToString());
                }
                result.districtId = dataReader["districtId"].ToString();
                result.CityId = dataReader["CityId"].ToString();
                obj = dataReader["GroupId"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.GroupId = int.Parse(obj.ToString());
                }
                obj = dataReader["OriginalBusiId"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.OriginalBusiId = int.Parse(obj.ToString());
                }
                result.ProvinceCode = dataReader["ProvinceCode"].ToString();
                result.CityCode = dataReader["CityCode"].ToString();
                result.AreaCode = dataReader["AreaCode"].ToString();
                result.Province = dataReader["Province"].ToString();
                obj = dataReader["CommissionTypeId"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.CommissionTypeId = int.Parse(obj.ToString());
                }
                obj = dataReader["DistribSubsidy"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.DistribSubsidy = decimal.Parse(obj.ToString());
                }
                obj = dataReader["BusinessCommission"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.BusinessCommission = decimal.Parse(obj.ToString());
                }
                obj = dataReader["CommissionType"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.CommissionType = int.Parse(obj.ToString());
                }
                obj = dataReader["CommissionFixValue"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.CommissionFixValue = decimal.Parse(obj.ToString());
                }
                obj = dataReader["BusinessGroupId"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.BusinessGroupId = int.Parse(obj.ToString());
                }
                obj = dataReader["BalancePrice"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.BalancePrice = decimal.Parse(obj.ToString());
                }
                obj = dataReader["AllowWithdrawPrice"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.AllowWithdrawPrice = decimal.Parse(obj.ToString());
                }
                obj = dataReader["HasWithdrawPrice"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.HasWithdrawPrice = decimal.Parse(obj.ToString());
                }

                return result;
            }
        }

        #endregion
        /// <summary>
        /// 获取商户详细信息
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public BusinessDetailModel GetBusinessDetailById(string businessId)
        {
            string selSql = @" 
SELECT   b.Id ,
         b.Name ,
         b.City ,
         b.district ,
         b.PhoneNo ,
         b.PhoneNo2 ,
         b.IDCard ,
         b.[Address] ,
         b.Landline ,
         b.Longitude ,
         b.Latitude ,
         b.[Status] ,
         b.InsertTime ,
         b.districtId ,
         b.CityId ,
         b.GroupId , 
         b.ProvinceCode ,
         b.CityCode ,
         b.AreaCode ,
         b.Province ,
         b.DistribSubsidy,
         b.BusinessCommission ,
         b.OriginalBusiId,
         b.CommissionType,
         b.CommissionFixValue,
         b.BusinessGroupId,
         b.BalancePrice,
         b.AllowWithdrawPrice,
         b.HasWithdrawPrice,
         bfa.TrueName,
         bfa.AccountNo,
         bfa.AccountType,
         bfa.OpenBank,
         bfa.OpenSubBank
FROM business b WITH(NOLOCK) 
	Left join BusinessFinanceAccount bfa WITH(NOLOCK) ON b.Id=bfa.BusinessId AND bfa.IsEnable=1
WHERE b.Id = @BusinessId  ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessId", businessId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, selSql, parm));
            if (dt != null&&dt.Rows.Count>0)
                return  DataTableHelper.ConvertDataTableList<BusinessDetailModel>(dt)[0];
            return null;
        }        

        /// <summary>
        /// 更改可提现金额
        /// 窦海超
        /// 2015年5月15日 16:56:37
        /// </summary>
        /// <param name="price">金额</param>
        /// <param name="clienterId">可提现金额的骑士ID</param>
        public void UpdateAllowWithdrawPrice(decimal price, int businessId)
        {
            string sql = "update dbo.business set AllowWithdrawPrice=AllowWithdrawPrice+@price where Id=@businessId";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("businessId", DbType.Int32, 4).Value = businessId;
            parm.Add("price", DbType.Decimal, 18).Value = price;
            DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm);
        }
    }
}
