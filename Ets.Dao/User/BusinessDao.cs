﻿using System.Data;
using ETS;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using ETS.Data.PageData;
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

            #endregion

            string orderByColumn = "o.id ";  //排序条件
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
                                    c.PhoneNo as SuperManPhone ";
            string tableList = @" [order](nolock) as o 
                                   LEFT join business(nolock) as b on o.businessId=b.Id
                                   LEFT join clienter(nolock) as c on o.clienterId=c.Id ";  //表名

            return new PageHelper().GetPages<T>(SuperMan_Read, paraModel.PagingResult.PageIndex, whereStr, orderByColumn, columnList, tableList, paraModel.PagingResult.PageSize, true);
        }


        /// <summary>
        /// 商户结算列表--2015.3.12 平扬
        /// </summary>
        /// <param name="t1">开始计算日期</param>
        /// <param name="t2">结束日期</param>
        /// <param name="name">商户姓名</param>
        /// <returns></returns>
        public IList<BusinessCommissionModel> GetBusinessCommission(DateTime t1, DateTime t2, string name, int groupid)
        {
            IList<BusinessCommissionModel> list = new List<BusinessCommissionModel>();
            try
            {
                string sql = " select BB.id,BB.Name,T.Amount,T.OrderCount,isnull(BB.BusinessCommission,0) BusinessCommission, CAST(isnull(BB.BusinessCommission,0) * T.Amount*0.01 as decimal(5,2)) as TotalAmount,@t1 as T1,@t2 as T2 " +
                         " from business BB with(nolock) inner join " +
                         " (select B.id,B.Name,sum(O.Amount) as Amount,sum(ISNULL(O.OrderCount,1)) as OrderCount " +
                         " from dbo.[order] O with(nolock) inner join dbo.business B with(nolock) on O.businessId=B.Id " +
                        " where O.[Status]=1  {0}  group by B.Id,B.Name) as T on BB.Id=T.Id";
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
        /// 商户结算列表--2015.3.12 平扬
        /// </summary>
        /// <param name="t1">开始计算日期</param>
        /// <param name="t2">结束日期</param>
        /// <param name="name">商户姓名</param>
        /// <returns></returns>
        public IList<BusinessCommissionModel> GetBusinessCommissionById(DateTime t1, DateTime t2, string name, int groupid)
        {
            IList<BusinessCommissionModel> list = new List<BusinessCommissionModel>();
            try
            {
                string sql = " select BB.id,BB.Name,T.Amount,T.OrderCount,isnull(BB.BusinessCommission,0) BusinessCommission, CAST(isnull(BB.BusinessCommission,0) * T.Amount*0.01 as decimal(5,2)) as TotalAmount,@t1 as T1,@t2 as T2 " +
                         " from business BB with(nolock) inner join " +
                         " (select B.id,B.Name,sum(O.Amount) as Amount,sum(ISNULL(O.OrderCount,1)) as OrderCount " +
                         " from dbo.[order] O with(nolock) inner join dbo.business B with(nolock) on O.businessId=B.Id " +
                        " where O.[Status]=1  {0}  group by B.Id,B.Name) as T on BB.Id=T.Id";
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
        /// <param name="id"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public bool setCommission(int id, decimal price)
        {
            bool reslut = false;
            try
            {
                string sql = " update business set BusinessCommission=@BusinessCommission where id=@id ";
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("BusinessCommission", price);
                dbParameters.AddWithValue("id", id);
                int i = DbHelper.ExecuteNonQuery(Config.SuperMan_Write, sql, dbParameters);
                if (i > 0) reslut = true;
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriter(ex, "设置结算比例");
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
                parm.AddWithValue("@PhoneNo", PhoneNo);
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
                                    ,g.GroupName";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(criteria.businessName))
            {
                sbSqlWhere.AppendFormat(" AND b.Name='{0}' ", criteria.businessName);
            }
            if (!string.IsNullOrEmpty(criteria.businessPhone))
            {
                sbSqlWhere.AppendFormat(" AND b.PhoneNo='{0}' ", criteria.businessPhone);
            }
            if (criteria.Status != -1)
            {
                sbSqlWhere.AppendFormat(" AND b.Status={0} ", criteria.Status);
            }
            if (criteria.BusinessCommission > 0)
            {
                sbSqlWhere.AppendFormat(" AND b.BusinessCommission={0} ", criteria.BusinessCommission);
            }
            if (criteria.GroupId != null)
            {
                sbSqlWhere.AppendFormat(" AND b.GroupId={0} ", criteria.GroupId);
            }
            string tableList = @" business  b WITH (NOLOCK)  LEFT JOIN dbo.[group] g WITH(NOLOCK) ON g.Id = b.GroupId ";
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
            parm.AddWithValue("@PhoneNo", model.phoneNo);
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
                        DistribSubsidy
                        FROM business(nolock) where PhoneNo=@PhoneNo AND Password=@Password order by id desc";

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@PhoneNo", model.phoneNo);
            parm.AddWithValue("@Password", model.passWord);
            return DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
        }

        public BusListResultModel GetBusiness(int busiId)
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
         DistribSubsidy FROM dbo.business WITH(NOLOCK) WHERE Id = @busiId";

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@busiId", busiId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, selSql, parm));
            if (dt != null)
            {
                busi = DataTableHelper.ConvertDataTableList<BusListResultModel>(dt)[0];
            }
            return busi;
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
            bool reslut = false;
            try
            {
                string sql = string.Empty;
                if (enumStatusType == EnumStatusType.审核通过)
                {

                    sql = string.Format(" update business set Status={0} where id=@id ", ConstValues.BUSINESS_AUDITPASS);
                }
                else if (enumStatusType == EnumStatusType.审核取消)
                {
                    sql = string.Format(" update business set Status={0} where id=@id ", ConstValues.BUSINESS_AUDITCANCEL);
                }
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("id", id);
                int i = DbHelper.ExecuteNonQuery(Config.SuperMan_Write, sql, dbParameters);
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
            parm.AddWithValue("@PhoneNo", PhoneNo);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            IList<BusListResultModel> list = MapRows<BusListResultModel>(dt);
            if (list == null && list.Count <= 0)
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
                    ISNULL(SUM(CASE WHEN ([Status]=@order_new OR [Status]=@order_Finish) AND CONVERT(CHAR(10),PubDate,120)=CONVERT(CHAR(10),GETDATE(),120) THEN 1 ELSE 0 END ),0) AS todayPublish,
                    ISNULL(SUM(CASE WHEN ([Status]=@order_new  OR [Status]=@order_Finish) AND CONVERT(CHAR(10),PubDate,120)=CONVERT(CHAR(10),GETDATE(),120) THEN Amount ELSE 0 END),0) AS todayPublishAmount,

                    ISNULL(SUM(CASE WHEN [Status]=@order_Finish AND CONVERT(CHAR(10),PubDate,120)=CONVERT(CHAR(10),GETDATE(),120) THEN 1 ELSE 0 END),0) AS todayDone,
                    ISNULL(SUM(CASE WHEN [Status]=@order_Finish AND CONVERT(CHAR(10),PubDate,120)=CONVERT(CHAR(10),GETDATE(),120) THEN Amount ELSE 0 END),0) AS todayDoneAmount,

                    ISNULL(SUM(CASE WHEN ([Status]=@order_new  OR [Status]=@order_Finish) AND CONVERT(CHAR(7),PubDate,120)=CONVERT(CHAR(7),GETDATE(),120) THEN 1 ELSE 0 END),0) AS monthPublish,
                    ISNULL(SUM(CASE WHEN ([Status]=@order_new  OR [Status]=@order_Finish) AND CONVERT(CHAR(7),PubDate,120)=CONVERT(CHAR(7),GETDATE(),120) THEN Amount ELSE 0 END),0) AS monthPublishAmount,

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
                           WHERE CONVERT(CHAR(10),InsertTime,120)=CONVERT(CHAR(10),GETDATE(),120) and [status]=1  --商家总数：";
            model.BusinessCount = ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql));
            return model;
        }

        /// <summary>
        /// 获取当天 商户结算金额（应收）
        /// 窦海超
        /// 2015年3月24日 14:15:00
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public HomeCountTitleModel GetCurrentBusinessYSPrice(HomeCountTitleModel model)
        {
            string sql = @"
                            SELECT 
                            ISNULL(SUM(o.Amount*ISNULL(b.BusinessCommission,0)+ ISNULL( b.DistribSubsidy ,0)* o.OrderCount),0) AS YsPrice
                             FROM dbo.[order](NOLOCK) AS o
                             LEFT JOIN dbo.business(NOLOCK) AS b ON o.businessId=b.Id
                              where CONVERT(CHAR(10),PubDate,120)=CONVERT(CHAR(10),GETDATE(),120) 
                            ";
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return model;
            }
            return MapRows<HomeCountTitleModel>(dt)[0];
        }
    }
}
