﻿using System.Data;
using ETS;
using ETS.Dao;
using Ets.Dao.Common;
using ETS.Data;
using ETS.Data.Core;
using ETS.Data.PageData;
using Ets.Model.DomainModel.Statistics;
using Ets.Model.ParameterModel.Finance;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Const;
using Ets.Model.DomainModel.Business;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.ParameterModel.Business;
using Ets.Model.DataModel.Order;
using Ets.Model.DataModel.Business;
using ETS.Extension;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.DataModel.Group;
using Ets.Model.ParameterModel.Order;
using Ets.Model.DataModel.Finance;
using ETS.Data.Generic;
using Ets.Model.ParameterModel.WtihdrawRecords;
namespace Ets.Dao.Business
{
    public class BusinessDao : DaoBase
    {

        /// <summary>
        /// 商户获取订单   add by caoheyang 20150311
        /// </summary>
        /// <param name="paraModel">查询条件实体</param>
        public virtual PageInfo<T> GetOrdersAppToSql<T>(Ets.Model.ParameterModel.Business.BussOrderParaModelApp paraModel)
        {
            #region where
            string whereStr = "1=1 and o.IsEnable=1";  //where查询条件实体类
            if (paraModel.userId != null)  //订单商户id
                whereStr += " and o.businessId=" + paraModel.userId.ToString();

            string orderByColumn = " PubDate desc ";//排序

            //订单状态
            if (paraModel.Status != null)
            {
                if (paraModel.Status == 100)
                {
                    //whereStr += " and (o.Status = " + OrderConst.ORDER_NEW + " or o.Status=" + OrderConst.ORDER_ACCEPT + ")";
                    whereStr += " and (o.Status = " + OrdersStatus.Status0.GetHashCode() + " or o.Status=" + OrdersStatus.Status2.GetHashCode() + " or o.Status=" + OrdersStatus.Status4.GetHashCode() + ")";
                }
                else
                {
                    whereStr += " and o.Status=" + paraModel.Status.ToString();
                }

                if (paraModel.Status == OrderStatus.Status1.GetHashCode())
                {
                    //如果是订单已完成就用完成时间倒序
                    orderByColumn = " o.ActualDoneDate DESC ";  //排序条件
                }
            }
            if (paraModel.OrderFrom > 0)
            {
                whereStr += " and o.OrderFrom=" + paraModel.OrderFrom;
            }

            #endregion

            string columnList = @"
                                    CONVERT(VARCHAR(5),o.ActualDoneDate,108) AS ActualDoneDate,
                                    o.Amount,
                                    o.OrderCount,
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
                                    o.id as OrderId,
                                    o.MealsSettleMode,
                                    (o.Amount+isnull(o.DistribSubsidy,0)*isnull(o.OrderCount,0)) as TotalAmount,
                                    isnull(o.OriginalOrderNo,'') as OriginalOrderNo,
									isnull(g.groupname,'') as OrderFromName";
            string tableList = @" [order](nolock) as o 
                                   join business(nolock) as b on o.businessId=b.Id
                                   left join clienter(nolock) as c on o.clienterId=c.Id 
                                   left join group with(nolock) g on o.OrderFrom=g.id";  //表名

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
        /// <param name="phoneno">商户电话</param>
        /// <returns></returns>
        public IList<BusinessCommissionDM> GetBusinessCommission(DateTime t1, DateTime t2, string name, string phoneno, int groupid, string businessCity, string authorityCityNameListStr)
        {
            IList<BusinessCommissionDM> list = new List<BusinessCommissionDM>();
            try
            {
                string sql = @"  SELECT 
                                        BB.id ,
                                        BB.Name ,
                                        BB.PhoneNo,
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
                string where = string.Format(" and O.ActualDoneDate between '{0}' and '{1}' ", t1, t2);


                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("groupid", groupid);
                dbParameters.AddWithValue("t1", t1);
                dbParameters.AddWithValue("t2", t2);
                if (!string.IsNullOrEmpty(name))
                {
                    where += " and Name=@name ";
                    dbParameters.AddWithValue("name", name);
                }
                if (!string.IsNullOrEmpty(phoneno))
                {
                    where += " and PhoneNo=@PhoneNo ";
                    dbParameters.AddWithValue("PhoneNo", phoneno);
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
                if (authorityCityNameListStr != null && !string.IsNullOrEmpty(authorityCityNameListStr.Trim()))
                {
                    where += string.Format(" AND B.City IN({0}) ", authorityCityNameListStr);
                }
                sql = string.Format(sql, where);
                DataTable dt = DbHelper.ExecuteDataset(Config.SuperMan_Read, sql, dbParameters).Tables[0];
                list = ConvertDataTableList<BusinessCommissionDM>(dt);
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
                        //if (model.CommissionFixValue > 0)
                        //{
                        sql += "CommissionFixValue=@CommissionFixValue,";
                        //}
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
                string sql = @"SELECT COUNT(1)  FROM dbo.business  a
LEFT join dbo.[group] b on a.GroupId=b.Id
where isnull(b.IsModifyBind,1)=1
and a.PhoneNo=@PhoneNo";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.Add("@PhoneNo", DbType.String, 40).Value = PhoneNo;
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
                                    ,ISNULL(b.Latitude,0) Latitude
                                    ,ISNULL(b.Longitude,0) Longitude
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
                                    ,ISNULL(b.DistribSubsidy,0.00) DistribSubsidy
                                    ,b.BusinessCommission
                                    ,g.GroupName
                                    ,b.CommissionType
                                    ,b.CommissionFixValue
                                    ,b.BusinessGroupId
                                    ,bg.Name BusinessGroupName
                                    ,b.IsEnable
                                    ,ISNULL(b.MealsSettleMode,0) MealsSettleMode
                                    ,ISNULL(b.BalancePrice,0) BalancePrice
                                    ,ISNULL(b.AllowWithdrawPrice,0) AllowWithdrawPrice
                                    ,ISNULL(b.RecommendPhone,'') as RecommendPhone";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(criteria.RecommendPhone))
            {
                sbSqlWhere.AppendFormat(" AND b.RecommendPhone='{0}' ", criteria.RecommendPhone.Trim());
            }
            if (!string.IsNullOrEmpty(criteria.businessPhone))
            {
                sbSqlWhere.AppendFormat(" AND b.PhoneNo='{0}' ", criteria.businessPhone.Trim());
            }
            if (criteria.Status != -1)
            {
                sbSqlWhere.AppendFormat(" AND b.Status={0} ", criteria.Status);
            }
            if (criteria.MealsSettleMode != -1)
            {
                sbSqlWhere.AppendFormat(" AND ISNULL(b.MealsSettleMode,0)={0}  ", criteria.MealsSettleMode);
            }
            if (criteria.BusinessCommission > 0)
            {
                sbSqlWhere.AppendFormat(" AND b.BusinessCommission={0} ", criteria.BusinessCommission);
            }
            if (criteria.GroupId != null && criteria.GroupId != 0)
            {
                sbSqlWhere.AppendFormat(" AND b.GroupId={0} ", criteria.GroupId);
            }
            if (ParseHelper.ToInt(criteria.BusinessGroupId, 0) > 0)
            {
                sbSqlWhere.AppendFormat(" AND b.BusinessGroupId={0} ", criteria.BusinessGroupId);
            }
            if (ParseHelper.ToInt(criteria.CommissionType, 0) > 0)
            {
                sbSqlWhere.AppendFormat(" AND b.CommissionType={0} ", criteria.CommissionType);
            }
            if (!string.IsNullOrEmpty(criteria.businessCity))
            {
                sbSqlWhere.AppendFormat(" AND b.City='{0}' ", criteria.businessCity.Trim());
            }
            if (!string.IsNullOrEmpty(criteria.businessName))
            {
                sbSqlWhere.AppendFormat(" AND b.Name LIKE '%{0}%' ", criteria.businessName.Trim());
            }
            if (criteria.TagId != null)
            {
                sbSqlWhere.AppendFormat(" AND TagR.TagId = {0} ", criteria.TagId);
            }
            if (criteria.GroupBusinessId > 0)
            {
                sbSqlWhere.AppendFormat(" and gbr.GroupId={0} ", criteria.GroupBusinessId);
            }

            //else
            //{
            //    sbSqlWhere.AppendFormat(" AND b.City IN ({0}) ", criteria.AuthorityCityNameListStr.Trim());
            //}
            if (!string.IsNullOrEmpty(criteria.AuthorityCityNameListStr) && criteria.UserType != 0)
            {
                sbSqlWhere.AppendFormat(" AND b.City IN ({0}) ", criteria.AuthorityCityNameListStr.Trim());
            }
            string tableList;
            if (criteria.TagId == null)
            {
                tableList = @" business  b WITH (NOLOCK)  
                                LEFT JOIN dbo.[group] g WITH(NOLOCK) ON g.Id = b.GroupId 
                                left JOIN dbo.[BusinessGroup]  bg WITH ( NOLOCK ) ON  b.BusinessGroupId=bg.Id";
            }
            else
            {
                tableList = @"  TagRelation TagR WITH (NOLOCK)  
                                left join business  b WITH (NOLOCK)    on TagR.UserId=b.id and TagR.UserType=0 and IsEnable=1  
                                LEFT JOIN dbo.[group] g WITH(NOLOCK) ON g.Id = b.GroupId 
                                JOIN dbo.[BusinessGroup]  bg WITH ( NOLOCK ) ON  b.BusinessGroupId=bg.Id";

            }
            if (criteria.GroupBusinessId > 0)
            {
                tableList += " left join GroupBusinessRelation gbr(nolock) on b.id=gbr.businessid ";
            }
            string orderByColumn = " b.Id DESC";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }
        /// <summary>
        ///  新增店铺  
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150604</UpdateTime>
        /// <param name="model"></param>
        /// <returns>返回商铺ID</returns>
        public int Insert(RegisterInfoPM model)
        {
            const string insertSql = @"         
insert into dbo.business (City,PhoneNo,PhoneNo2,Password,CityId,RecommendPhone,Timespan,Appkey)
values( @City,@PhoneNo,@PhoneNo2,@Password,@CityId ,@RecommendPhone,@Timespan,@Appkey)
select @@IDENTITY";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@City", model.city);
            dbParameters.AddWithValue("@Password", model.passWord);
            dbParameters.AddWithValue("@PhoneNo", model.phoneNo);
            dbParameters.AddWithValue("@PhoneNo2", model.phoneNo);
            dbParameters.AddWithValue("@CityId", model.CityId);
            dbParameters.AddWithValue("@RecommendPhone", model.RecommendPhone);
            dbParameters.AddWithValue("@Timespan", model.timespan);
            dbParameters.AddWithValue("@Appkey", model.Appkey);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters));
        }
        /// <summary>
        /// 增加一条记录
        /// </summary>
        public int Insert(BusinessModel businessModel)
        {
            const string insertSql = @"
insert into business(Name,City,district,PhoneNo,PhoneNo2,
Password,CheckPicUrl,IDCard,Address,Landline,Longitude,Latitude,
Status,districtId,CityId,GroupId,OriginalBusiId,OriginalBusiUnitId,
ProvinceCode,CityCode,AreaCode,Province,CommissionTypeId,DistribSubsidy,
BusinessCommission,CommissionType,CommissionFixValue,BusinessGroupId,
BalancePrice,AllowWithdrawPrice,HasWithdrawPrice,MealsSettleMode,
BusinessLicensePic,IsBind,OneKeyPubOrder,IsAllowOverdraft,IsEmployerTask,
RecommendPhone,Timespan,Appkey,IsOrderChecked,IsAllowCashPay,LastLoginTime,PushOrderType)
values(@Name,@City,@district,@PhoneNo,@PhoneNo2,@Password,@CheckPicUrl,
@IDCard,@Address,@Landline,@Longitude,@Latitude,@Status,@districtId,
@CityId,@GroupId,@OriginalBusiId,@OriginalBusiUnitId,@ProvinceCode,@CityCode,@AreaCode,@Province,
@CommissionTypeId,@DistribSubsidy,@BusinessCommission,@CommissionType,
@CommissionFixValue,@BusinessGroupId,@BalancePrice,@AllowWithdrawPrice,
@HasWithdrawPrice,@MealsSettleMode,@BusinessLicensePic,
@IsBind,@OneKeyPubOrder,@IsAllowOverdraft,@IsEmployerTask,@RecommendPhone,
@Timespan,@Appkey,@IsOrderChecked,@IsAllowCashPay,@LastLoginTime,@PushOrderType)

select @@IDENTITY";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Name", businessModel.Name);
            dbParameters.AddWithValue("City", businessModel.City);
            dbParameters.AddWithValue("district", businessModel.district);
            dbParameters.AddWithValue("PhoneNo", businessModel.PhoneNo);
            dbParameters.AddWithValue("PhoneNo2", businessModel.PhoneNo2);
            dbParameters.AddWithValue("Password", businessModel.Password);
            dbParameters.AddWithValue("CheckPicUrl", businessModel.CheckPicUrl);
            dbParameters.AddWithValue("IDCard", businessModel.IDCard);
            dbParameters.AddWithValue("Address", businessModel.Address);
            dbParameters.AddWithValue("Landline", businessModel.Landline);
            dbParameters.AddWithValue("Longitude", businessModel.Longitude);
            dbParameters.AddWithValue("Latitude", businessModel.Latitude);
            dbParameters.AddWithValue("Status", businessModel.Status);            
            dbParameters.AddWithValue("districtId", businessModel.districtId);
            dbParameters.AddWithValue("CityId", businessModel.CityId);
            dbParameters.AddWithValue("GroupId", businessModel.GroupId);
            dbParameters.AddWithValue("OriginalBusiId", businessModel.OriginalBusiId);
            dbParameters.AddWithValue("OriginalBusiUnitId", businessModel.OriginalBusiUnitId);
            dbParameters.AddWithValue("ProvinceCode", businessModel.ProvinceCode);
            dbParameters.AddWithValue("CityCode", businessModel.CityCode);
            dbParameters.AddWithValue("AreaCode", businessModel.AreaCode);
            dbParameters.AddWithValue("Province", businessModel.Province);
            dbParameters.AddWithValue("CommissionTypeId", businessModel.CommissionTypeId);
            dbParameters.AddWithValue("DistribSubsidy", businessModel.DistribSubsidy);
            dbParameters.AddWithValue("BusinessCommission", businessModel.BusinessCommission);
            dbParameters.AddWithValue("CommissionType", businessModel.CommissionType);
            dbParameters.AddWithValue("CommissionFixValue", businessModel.CommissionFixValue);
            dbParameters.AddWithValue("BusinessGroupId", businessModel.BusinessGroupId);
            dbParameters.AddWithValue("BalancePrice", businessModel.BalancePrice);
            dbParameters.AddWithValue("AllowWithdrawPrice", businessModel.AllowWithdrawPrice);
            dbParameters.AddWithValue("HasWithdrawPrice", businessModel.HasWithdrawPrice);
            dbParameters.AddWithValue("MealsSettleMode", businessModel.MealsSettleMode);
            //dbParameters.AddWithValue("ContactWay", businessModel.ContactWay);
            dbParameters.AddWithValue("BusinessLicensePic", businessModel.BusinessLicensePic);
            dbParameters.AddWithValue("IsBind", businessModel.IsBind);
            dbParameters.AddWithValue("OneKeyPubOrder", businessModel.OneKeyPubOrder);
            dbParameters.AddWithValue("IsAllowOverdraft", businessModel.IsAllowOverdraft);
            dbParameters.AddWithValue("IsEmployerTask", businessModel.IsEmployerTask);
            dbParameters.AddWithValue("RecommendPhone", businessModel.RecommendPhone);
            dbParameters.AddWithValue("Timespan", businessModel.Timespan);
            dbParameters.AddWithValue("Appkey", businessModel.Appkey);
            dbParameters.AddWithValue("IsOrderChecked", businessModel.IsOrderChecked);
            dbParameters.AddWithValue("IsAllowCashPay", businessModel.IsAllowCashPay);
            dbParameters.AddWithValue("LastLoginTime", businessModel.LastLoginTime);
            dbParameters.AddWithValue("PushOrderType", businessModel.PushOrderType);


            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters));
        }


        /// <summary>
        /// 商户是否已注册
        /// 彭宜  20150716
        /// </summary>
        /// <param name="model"></param>
        /// <returns>是否注册</returns>
        public bool IsExist(RegisterInfoPM model)
        {
            bool isExist;

            const string querysql = @"
select  count(1) 
from  dbo.[business]  
where Timespan=@Timespan and phoneNo=@phoneNo;";
            IDbParameters dbSelectParameters = DbHelper.CreateDbParameters();
            dbSelectParameters.Add("phoneNo", DbType.String, 20).Value = (model.phoneNo ?? "");
            dbSelectParameters.Add("Timespan", DbType.String, 50).Value = model.timespan;
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Write, querysql, dbSelectParameters);
            isExist = ParseHelper.ToInt(executeScalar, 0) > 0;

            return isExist;
        }


        /// <summary>
        /// B端登录
        /// </summary>
        /// <param name="model">用户名称，用户密码</param>
        /// <returns>返回该用户实体</returns>
        public DataTable LoginSql(LoginModel model)
        {
            string sql = @"
select top 1
        a.Id as userId ,
        a.status ,
        a.city ,
        a.districtId ,
        a.district ,
        a.Address ,
        a.Landline ,
        a.Name ,
        a.cityId ,
        a.phoneNo ,
        a.PhoneNo2 ,
        a.DistribSubsidy ,
        ISNULL(a.OriginalBusiId, 0) as OriginalBusiId,
        a.Appkey
from    business (nolock) a
        left join dbo.[group] (nolock) b on a.GroupId = b.Id
where   a.PhoneNo = @PhoneNo
        and a.Password = @Password
        and ISNULL(b.IsModifyBind,1) = 1
order by a.id desc
";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@PhoneNo", SqlDbType.NVarChar);
            parm.SetValue("@PhoneNo", model.phoneNo);
            parm.AddWithValue("@Password", model.passWord);
            return DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
        }


        /// <summary>k
        /// 根据商户id获取商户
        /// </summary>

        /// <param name="busiId"></param>
        /// <returns></returns>
        public BusListResultModel GetBusiness(int busiId)
        {
            BusListResultModel busi = new BusListResultModel();
            string querySql = @"
select   b.Id ,
b.Name, b.City,b.district, b.PhoneNo ,b.PhoneNo2 ,b.IDCard ,b.[Address] ,b.Landline ,
b.Longitude ,b.Latitude ,b.[Status] , b.InsertTime ,b.districtId ,b.CityId ,b.GroupId , 
b.ProvinceCode ,b.CityCode ,b.AreaCode ,b.Province ,b.DistribSubsidy,b.BusinessCommission,
b.CommissionType,b.CommissionFixValue,b.BusinessGroupId,b.MealsSettleMode,b.OriginalBusiId,
b.BalancePrice,b.OneKeyPubOrder,b.IsAllowOverdraft,b.IsEmployerTask,b.IsOrderChecked,b.IsAllowCashPay,  
b.SetpChargeId,b.ReceivableType,  
bg.StrategyId,
CASE WHEN gbr.id >0 THEN 1 ELSE 0 END AS IsBindGroup, 
ISNULL(gb.Id,0) AS BussGroupId,
ISNULL(gb.IsAllowOverdraft,0) AS BussGroupIsAllowOverdraft,
ISNULL(gb.Amount,0) AS BussGroupAmount,
gb.GroupBusiName,gb.CreateName                               
from dbo.business as b WITH(NOLOCK)
left join BusinessGroup bg WITH(nolock) on b.BusinessGroupId=bg.Id
LEFT JOIN GroupBusinessRelation gbr WITH(NOLOCK) ON b.Id=gbr.BusinessId  AND gbr.IsBind=1 AND gbr.IsEnable=1
LEFT JOIN dbo.GroupBusiness gb WITH(NOLOCK) ON gbr.groupid=gb.Id and gb.IsValid=1
WHERE b.Id = @busiId";

            IDbParameters dbParameters = DbHelper.CreateDbParameters("busiId", DbType.Int32, 4, busiId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querySql, dbParameters));
            if (dt != null && dt.Rows.Count > 0)
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
            string querySql = @" SELECT  
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

            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querySql, parm));
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
        /// 根据商户id获取商户
        /// </summary>
        /// <param name="busiId"></param>
        /// <returns></returns>
        public BusinessModel GetBusiness(string originalBusiUnitId, int groupId)
        {
            BusinessModel busi = new BusinessModel();
            string querySql = @" SELECT  
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
         FROM dbo.business WITH(NOLOCK) WHERE OriginalBusiUnitId = @busiId AND GroupId=@groupId";

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@busiId", originalBusiUnitId);
            parm.AddWithValue("@GroupId", groupId);

            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querySql, parm));
            if (dt != null && dt.Rows.Count > 0)
            {
                busi = DataTableHelper.ConvertDataTableList<BusinessModel>(dt)[0];
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
        /// 茹化肖修改
        /// 2015年8月26日11:10:29
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enumStatusType"></param>
        /// <returns></returns>
        public bool UpdateAuditStatus(int id, AuditStatus enumStatusType)
        {
            var juWangKeBusiAuditUrl = ConfigSettings.Instance.JuWangKeBusiAuditCallBack;
            bool reslut = false;
            try
            {
                StringBuilder sql = new StringBuilder();
                if (enumStatusType == AuditStatus.Status1)
                {
                    sql.AppendFormat(" update business set Status={0} ", BusinessStatus.Status1.GetHashCode());
                }
                else if (enumStatusType == AuditStatus.Status0)
                {
                    sql.AppendFormat(" update business set Status={0} ", BusinessStatus.Status4.GetHashCode());
                }

                sql.Append(" where id=@id;");
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("id", id);
                int i = DbHelper.ExecuteNonQuery(Config.SuperMan_Write, sql.ToString(), dbParameters);

                if (i > 0)
                {
                    //调用第三方接口 ，聚网客商户审核通过后调用接口
                    //这里不建议使用 1 数字，而是根据 配置文件中的 appkey来获取 groupid
                    var busi = GetBusiness(id);
                    if (busi.GroupId == 1 && busi.OriginalBusiId > 0 && enumStatusType == AuditStatus.Status1)
                    {
                        string str = HTTPHelper.HttpPost(juWangKeBusiAuditUrl, "supplier_id=" + busi.OriginalBusiId);
                        HttpModel httpModel = new HttpModel()
                        {
                            Url = juWangKeBusiAuditUrl,
                            Htype = HtypeEnum.ReqType.GetHashCode(),
                            RequestBody = "supplier_id=" + busi.OriginalBusiId,
                            ResponseBody = str,
                            ReuqestPlatForm = RequestPlatFormEnum.EdsManagePlat.GetHashCode(),
                            ReuqestMethod = "Ets.Dao.Business.BusinessDao.UpdateAuditStatus",
                            Status = 1,
                            Remark = "更新审核状态:调用聚网客"
                        };
                        new HttpDao().LogThirdPartyInfo(httpModel);
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
        /// 更新审核状态
        /// danny-20150317
        /// 茹化肖修改
        /// 2015年8月26日11:10:29 
        /// wc修改
        /// 2015年9月1日
        /// </summary>
        /// <returns></returns>
        public bool UpdateAuditStatus(BusinessAuditModel bam)
        {
            var juWangKeBusiAuditUrl = ConfigSettings.Instance.JuWangKeBusiAuditCallBack;
            bool reslut = false;
            string remark = "";
            try
            {
                StringBuilder sql = new StringBuilder();
                if (bam.AuditStatus == AuditStatus.Status1)
                {
                    remark = "审核状态修改为审核通过";
                    sql.AppendFormat(@" update business set Status={0} OUTPUT
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
                                              Remark) ", BusinessStatus.Status1.GetHashCode());
                }
                else if (bam.AuditStatus == AuditStatus.Status0)
                {
                    remark = "审核状态修改为审核取消";
                    sql.AppendFormat(@" update business set Status={0} OUTPUT
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
                                              Remark) ", BusinessStatus.Status4.GetHashCode());
                }

                sql.Append(" where id=@id;");
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("id", bam.BusinessId);
                dbParameters.AddWithValue("@OptId", bam.OptionUserId);
                dbParameters.AddWithValue("@OptName", bam.OptionUserName);
                dbParameters.AddWithValue("@Platform", 3);
                dbParameters.AddWithValue("@Remark", remark);
                int i = DbHelper.ExecuteNonQuery(Config.SuperMan_Write, sql.ToString(), dbParameters);

                if (i > 0)
                {
                    //调用第三方接口 ，聚网客商户审核通过后调用接口
                    //这里不建议使用 1 数字，而是根据 配置文件中的 appkey来获取 groupid
                    var busi = GetBusiness(bam.BusinessId);
                    if (busi.GroupId == 1 && busi.OriginalBusiId > 0 && bam.AuditStatus == AuditStatus.Status1)
                    {
                        string str = HTTPHelper.HttpPost(juWangKeBusiAuditUrl, "supplier_id=" + busi.OriginalBusiId);
                        HttpModel httpModel = new HttpModel()
                        {
                            Url = juWangKeBusiAuditUrl,
                            Htype = HtypeEnum.ReqType.GetHashCode(),
                            RequestBody = "supplier_id=" + busi.OriginalBusiId,
                            ResponseBody = str,
                            ReuqestPlatForm = RequestPlatFormEnum.EdsManagePlat.GetHashCode(),
                            ReuqestMethod = "Ets.Dao.Business.BusinessDao.UpdateAuditStatus",
                            Status = 1,
                            Remark = "更新审核状态:调用聚网客"
                        };
                        new HttpDao().LogThirdPartyInfo(httpModel);
                    }
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
        /// 更新审核状态
        /// danny-20150317
        /// </summary>
        /// <param name="id"></param> 
        /// <returns></returns>
        public bool UpdateAuditStatus(BusinessAuditModel bam, string busiAddress)
        {
            bool reslut = false;
            IDbParameters parm = DbHelper.CreateDbParameters();
            try
            {
                string sql = @" update business set Status=@enumStatus {0} ";
                if (!string.IsNullOrWhiteSpace(busiAddress))  //当商户地址不为空的时候，修改商户地址
                {
                    sql = sql.Replace("{0}", " ,Address=@Address ");
                    parm.AddWithValue("@Remark", busiAddress); 
                }
                else { parm.AddWithValue("@Remark", ""); }
                sql += @" where id=@id;
insert into dbo.BusinessOptionLog
         ( BusinessId ,
           OptId ,
           OptName ,
           InsertTime ,
           Platform ,
           Remark
         )
 values  ( @id , -- BusinessId - int
           @OptId , -- OptId - int
           @OptName , -- OptName - varchar(50)
           getdate() , -- InsertTime - datetime
           0 , -- Platform - int
           @Remark  -- Remark - nvarchar(4000)
         )";

                parm.Add("@id", DbType.Int32, 4).Value = bam.BusinessId;
                parm.Add("@Address", DbType.String).Value = busiAddress; //商户地址
                parm.Add("@enumStatus", DbType.Int32, 4).Value = bam.AuditStatus.GetHashCode();
                parm.AddWithValue("@OptId", bam.OptionUserId);
                parm.AddWithValue("@OptName", bam.OptionUserName);
                parm.AddWithValue("@Platform", 0);
                return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm)) > 0;
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriter(ex, "更新审核状态");
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
        /// 根据手机号获取用商家信息  B修改商户密码 用到
        /// 窦海超
        /// 2015年3月23日 19:00:52
        /// </summary>
        /// <param name="PhoneNo">手机号</param>
        /// <returns>商家信息</returns>
        public BusListResultModel GetBusinessByPhoneNo(string PhoneNo)
        {
            string sql = @"
SELECT b.Id,b.[Password]  FROM dbo.business(NOLOCK) b
LEFT join dbo.[group] g on b.GroupId=g.Id 
where b.PhoneNo=@PhoneNo and isnull(g.IsModifyBind,1)=1";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("PhoneNo", DbType.String, 40).Value = PhoneNo;
            // int tmpId = ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm), 0);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            IList<BusListResultModel> list = MapRows<BusListResultModel>(dt);

            if (!dt.HasData())
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
            parm.Add("businessId", DbType.Int32, 4).Value = BusinessId;
            parm.Add("order_new", DbType.Int32, 4).Value = OrderStatus.Status0.GetHashCode();
            parm.Add("order_Finish", DbType.Int32, 4).Value = OrderStatus.Status1.GetHashCode();
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
                                    ,IsModifyBind
                          FROM [group] WHERE IsValid=@IsValid";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@IsValid", 1);
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
        public BusinessModel CheckExistBusiness(int bid, int groupid)
        {
            string sql = @"select Id,Status,Name from dbo.business where OriginalBusiId=@OriginalBusiId and GroupId=@GroupId";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OriginalBusiId", bid);
            parm.AddWithValue("@GroupId", groupid);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            return MapRows<BusinessModel>(dt)[0];
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
                            WHERE OriginalOrderNo=@OriginalOrderNo and OrderFrom=@OrderFrom ", SuperPlatform.FromBusiness, OrderConst.CancelOrder, (int)SuperPlatform.FromBusiness);
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OriginalOrderNo", oriOrderNo);
            parm.AddWithValue("@OrderFrom", orderFrom);
            parm.AddWithValue("@Status", orderStatus);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }


        /// <summary>
        ///  新增第三方店铺   
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150601</UpdateTime>
        /// <param name="model"></param>
        /// <returns>返回商铺ID</returns>
        public int InsertOtherBusiness(BusinessModel model)
        {
            string insertSql = @"
insert into dbo.business(Name,City,district,PhoneNo,PhoneNo2,Password,CheckPicUrl,
            IDCard,Address,Longitude,Latitude,Status,districtId,CityId, 
            ProvinceCode,CityCode,AreaCode,Province,CommissionTypeId,DistribSubsidy, GroupId,OriginalBusiId,IsAllowOverdraft )
            values(@Name,@City,@district,@PhoneNo,@PhoneNo2,@Password,@CheckPicUrl,@IDCard , 
            @Address,@Longitude,@Latitude,@Status,@districtId,@CityId,
            @ProvinceCode,@CityCode,@AreaCode,@Province,@CommissionTypeId ,@DistribSubsidy,@GroupId,@OriginalBusiId,@IsAllowOverdraft );
select SCOPE_IDENTITY() as id;    
";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@Name", model.Name);
            dbParameters.AddWithValue("@City", model.City);
            dbParameters.AddWithValue("@district", model.district);
            dbParameters.AddWithValue("@PhoneNo", model.PhoneNo);
            dbParameters.AddWithValue("@PhoneNo2", model.PhoneNo2);//写PhoneNo2
            dbParameters.AddWithValue("@Password", model.Password);
            dbParameters.AddWithValue("@CheckPicUrl", model.CheckPicUrl);
            dbParameters.AddWithValue("@IDCard", model.IDCard);
            dbParameters.AddWithValue("@Address", model.Address);
            dbParameters.AddWithValue("@Longitude", model.Longitude);
            dbParameters.AddWithValue("@Latitude", model.Latitude);
            dbParameters.AddWithValue("@Status", model.Status);
            dbParameters.AddWithValue("@districtId", model.districtId);
            dbParameters.AddWithValue("@CityId", model.CityId);
            dbParameters.AddWithValue("@CityCode", model.CityCode);
            dbParameters.AddWithValue("@AreaCode", model.AreaCode);
            dbParameters.AddWithValue("@Province", model.Province);
            dbParameters.AddWithValue("@CommissionTypeId", model.CommissionTypeId);
            dbParameters.AddWithValue("@ProvinceCode", model.ProvinceCode);
            dbParameters.AddWithValue("@DistribSubsidy", model.DistribSubsidy);
            dbParameters.AddWithValue("@GroupId", model.GroupId);
            dbParameters.AddWithValue("@OriginalBusiId", model.OriginalBusiId);
            dbParameters.Add("@IsAllowOverdraft", DbType.Int16).Value = model.IsAllowOverdraft; 
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters));
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
        public int UpdateBusinessAddressInfo(BusinessModel business)
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
                            SET     CheckPicUrl = @CheckPicUrl,
                                    [Status] = @Status
                            OUTPUT  Inserted.[Status]
                            WHERE   Id = @busiID ";

            IDbParameters parm = DbHelper.CreateDbParameters();

            parm.AddWithValue("@CheckPicUrl", picName);
            parm.AddWithValue("@Status", BusinessStatus.Status3.GetHashCode());
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
        /// B端修改商户信息 caoheyang
        /// </summary>
        /// <param name="business"></param>
        /// <returns></returns>
        public int UpdateBusinessInfoB(UpdateBusinessInfoBPM business)
        {
            string sqlCheckPicUrl = !string.IsNullOrWhiteSpace(business.CheckPicUrl)
                ? ",CheckPicUrl='" + business.CheckPicUrl + "'"
                : "";
            string sqlBusinessLicensePic = !string.IsNullOrWhiteSpace(business.BusinessLicensePic)
                ? ",BusinessLicensePic='" + business.BusinessLicensePic + "'"
                : "";
            string upSql = string.Format(@"UPDATE  dbo.business
                            SET     [Address] = @Address ,
                                    PhoneNo2 = @PhoneNo2 ,
                                    [Name] = @Name ,
                                    Landline = @Landline ,
                                    district = @district ,
                                    districtId = @districtId ,
                                    Longitude = @Longitude ,
                                    Latitude = @Latitude ,
                                    [Status]= @Status,
                                    AreaCode = @AreaCode ,
                                    ProvinceCode = @ProvinceCode ,
                                    Province = @Province ,
                                    CityId = @CityId ,
                                    CityCode = @CityCode ,
                                    City = @City,
                                    OneKeyPubOrder=@OneKeyPubOrder
                                    {0}{1}
                            OUTPUT  Inserted.[Status]
                            WHERE   Id = @busiID", sqlCheckPicUrl, sqlBusinessLicensePic);

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
            parm.AddWithValue("@AreaCode", business.AreaCode);
            parm.AddWithValue("@ProvinceCode", business.ProvinceCode);
            parm.AddWithValue("@Province", business.Province);
            parm.AddWithValue("@CityId", business.CityId);
            parm.AddWithValue("@CityCode", business.CityCode);
            parm.AddWithValue("@City", business.City);
            parm.AddWithValue("@busiID", business.Id);
            parm.Add("@OneKeyPubOrder", DbType.Int32, 4).Value = business.OneKeyPubOrder;
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
        public BusinessModel GetBusiByOriIdAndOrderFrom(int oriBusiId, int orderFrom)
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
            return MapRows<BusinessModel>(dt)[0];
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
            string sql = @"select  Id as userid,[status] as status,OneKeyPubOrder,IsAllowOverdraft,BalancePrice,IsAllowCashPay from dbo.business with(nolock) WHERE id=@id ";
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
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>2015064</UpdateTime>
        /// <param name="model"></param>
        /// <returns>返回商铺ID</returns>
        public int addBusiness(AddBusinessModel model)
        {
            const string insertSql = @"
insert into dbo.business(City,PhoneNo,PhoneNo2,Password,CityId ,         
                          Name,Address,GroupId ,DistribSubsidy,BusinessCommission)
values  ( @City , @PhoneNo,@PhoneNo2, @Password ,@CityId , 
                          @Name,@Address, @GroupId,@DistribSubsidy, @BusinessCommission)
select @@IDENTITY ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@City", model.city);
            dbParameters.AddWithValue("@Password", "A06F6A211CBEDF374FC367FA231865DE");
            dbParameters.AddWithValue("@PhoneNo", model.phoneNo);
            dbParameters.AddWithValue("@PhoneNo2", model.phoneNo);
            dbParameters.AddWithValue("@CityId", model.CityId);
            dbParameters.AddWithValue("@Name", model.businessName);
            dbParameters.AddWithValue("@Address", model.businessaddr);
            dbParameters.AddWithValue("@GroupId", model.GroupId);
            dbParameters.AddWithValue("@DistribSubsidy", model.businessWaisong);
            dbParameters.AddWithValue("@BusinessCommission", model.businessCommission);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters));
        }



        /// <summary>
        /// 修改商户信息
        /// danny-20150417
        /// </summary>
        /// <param name="model"></param>
        /// <param name="orderOptionModel"></param>
        /// <returns></returns>
        public bool ModifyBusinessInfo(BusinessModel model, OrderOptionModel orderOptionModel)
        {
            string remark = orderOptionModel.OptUserName + "通过后台管理系统修改商户信息";
            string sql = string.Format(@"UPDATE business 
                                            SET Name=@Name,
                                                GroupId=@GroupId,
                                                OriginalBusiId=@OriginalBusiId,
                                                PhoneNo=@PhoneNo,
                                                MealsSettleMode=@MealsSettleMode
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
            parm.AddWithValue("@MealsSettleMode", model.MealsSettleMode);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }


        /// <summary>
        /// 根据ID获取对象
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150626</UpdateTime>
        public BusinessModel GetById(int id)
        {
            BusinessModel model = null;
            const string getbyidSql = @"
select  Id,Name,City,district,PhoneNo,PhoneNo2,Password,CheckPicUrl,IDCard,Address,
Landline,Longitude,Latitude,Status,InsertTime,districtId,CityId,GroupId,OriginalBusiId,
ProvinceCode,CityCode,AreaCode,Province,CommissionTypeId,DistribSubsidy,BusinessCommission,
CommissionType,CommissionFixValue,BusinessGroupId,BalancePrice,AllowWithdrawPrice,HasWithdrawPrice
from  business (nolock)
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, getbyidSql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = MapRows<BusinessModel>(dt)[0];
            }
            return model;
        }

        /// <summary>
        /// 获得商户电话   add by 彭宜    20150717
        /// </summary>
        /// <param name="id">商户id</param>
        /// <returns>电话号码</returns>
        public string GetPhoneNo(int id)
        {
            string querSql = @"
select  PhoneNo
            from Business (nolock) 
where Id=@Id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters("Id", DbType.Int32, 4, id);
            return Convert.ToString(DbHelper.ExecuteScalar(SuperMan_Read, querSql, dbParameters));
        }
        /// <summary>
        /// 根据商品户ID集合查询商户信息
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150626</UpdateTime>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IDictionary<int, BusinessModel> GetByIds(IList<int> ids)
        {
            const string getbyidSql = @"
select  Id,Name,City,district,PhoneNo,PhoneNo2,Password,CheckPicUrl,
IDCard,Address,Landline,Longitude,Latitude,Status,InsertTime,districtId,
CityId,GroupId,OriginalBusiId,ProvinceCode,CityCode,AreaCode,Province,
CommissionTypeId,DistribSubsidy,BusinessCommission,CommissionType,
CommissionFixValue,BusinessGroupId,BalancePrice,AllowWithdrawPrice,HasWithdrawPrice
from  business (nolock)
where  Id IN({0})";

            if (ids == null || ids.Count == 0)
            {
                return new Dictionary<int, BusinessModel>();
            }

            return DbHelper.QueryWithRowMapperDelegate<BusinessModel>(SuperMan_Read, string.Format(getbyidSql, string.Join(",", ids)), datarow => new BusinessModel()
            {
                Id = ParseHelper.ToInt(datarow["Id"]),
                Name = ParseHelper.ToString(datarow["Name"]),
                PhoneNo = ParseHelper.ToString(datarow["PhoneNo"])

            }).ToDictionary(m => m.Id);
        }

        /// <summary>
        /// 获取商家详情
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="id">商家Id</param>
        /// <returns></returns>
        public BusinessDM GetDetails(int id)
        {
            BusinessDM businessDM = new BusinessDM();

            #region 商家表
            string queryBusinessSql = @"
select  Id,Name,City,district,PhoneNo,PhoneNo2,Password,CheckPicUrl,IDCard,
        Address,Landline,Longitude,Latitude,Status,InsertTime,districtId,CityId,GroupId,
        OriginalBusiId,ProvinceCode,CityCode,AreaCode,Province,CommissionTypeId,DistribSubsidy,
        BusinessCommission,CommissionType,CommissionFixValue,BusinessGroupId,BalancePrice,
        AllowWithdrawPrice,HasWithdrawPrice,BusinessLicensePic,OneKeyPubOrder
from  Business (nolock) 
where Id=@Id";

            IDbParameters dbBusinessParameters = DbHelper.CreateDbParameters("Id", DbType.Int32, 4, id);
            businessDM = DbHelper.QueryForObject(SuperMan_Read, queryBusinessSql, dbBusinessParameters, new BusinessRowMapper());
            #endregion

            #region 商家金融账号表
            const string queryBFAccountSql = @"
select  Id,BusinessId,TrueName,AccountNo,IsEnable,AccountType,BelongType,OpenBank,OpenSubBank,
        CreateBy,CreateTime,UpdateBy,UpdateTime,OpenProvince,OpenCity,OpenProvinceCode,OpenCityCode,IDCard 
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
                bf.BusinessId = ParseHelper.ToInt(dataRow["BusinessId"]);
                bf.TrueName = dataRow["TrueName"].ToString();
                bf.AccountNo = ETS.Security.AESApp.AesEncrypt(ETS.Security.DES.Decrypt(dataRow["AccountNo"].ToString()));
                bf.IsEnable = ParseHelper.ToBool(dataRow["IsEnable"]);
                bf.AccountType = ParseHelper.ToInt(dataRow["AccountType"]);
                bf.BelongType = ParseHelper.ToInt(dataRow["BelongType"]);
                if (dataRow["OpenBank"] != null && dataRow["OpenBank"] != DBNull.Value)
                {
                    bf.OpenBank = dataRow["OpenBank"].ToString();
                }
                if (dataRow["OpenSubBank"] != null && dataRow["OpenSubBank"] != DBNull.Value)
                {
                    bf.OpenSubBank = dataRow["OpenSubBank"].ToString();
                }
                if (dataRow["OpenProvince"] != null && dataRow["OpenProvince"] != DBNull.Value)
                {
                    bf.OpenProvince = dataRow["OpenProvince"].ToString();
                }
                if (dataRow["OpenCity"] != null && dataRow["OpenCity"] != DBNull.Value)
                {
                    bf.OpenCity = dataRow["OpenCity"].ToString();
                }
                if (dataRow["OpenProvinceCode"] != null && dataRow["OpenProvinceCode"] != DBNull.Value)
                {
                    bf.OpenProvinceCode = ParseHelper.ToInt(dataRow["OpenProvinceCode"], 0);
                }
                if (dataRow["OpenCityCode"] != null && dataRow["OpenCityCode"] != DBNull.Value)
                {
                    bf.OpenCityCode = ParseHelper.ToInt(dataRow["OpenCityCode"], 0);
                }
                if (dataRow["IDCard"] != null && dataRow["IDCard"] != DBNull.Value)
                {
                    bf.IDCard = dataRow["IDCard"].ToString();
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
        /// 获取门店结算相关
        /// 胡灵波
        /// 2015年9月11日 16:49:36
        /// </summary>
        /// <param name="id">商家Id</param>
        /// <returns></returns>
        public BusinessInfo GetSettlementRelevantById(int id)
        {
            string querSql = @"
SELECT gbr.IsEnable,  isnull(DistribSubsidy,0) as DistribSubsidy, 
            b.BusinessCommission,
            b.CommissionType, 
            b.CommissionFixValue,
            b.BalancePrice ,
            b.ReceivableType,            
            b.SetpChargeId,
            b.TaskDistributionId ,
            CASE WHEN gbr.IsBind=1 THEN gb.Amount
            ELSE 0  END GroupBusinessAmount           
            from Business b (nolock) 
LEFT JOIN   GroupBusinessRelation gbr (NOLOCK)    ON b.Id=gbr.BusinessId  AND gbr.IsEnable=1
LEFT JOIN GroupBusiness gb (NOLOCK) ON gbr.Groupid=gb.Id 
where b.Id=@Id";

            IDbParameters dbParameters = DbHelper.CreateDbParameters("Id", DbType.Int32, 4, id);
            return DbHelper.QueryForObjectDelegate<BusinessInfo>(SuperMan_Read, querSql, dbParameters,
             dataRow => new BusinessInfo
             {
                 DistribSubsidy = ParseHelper.ToDecimal(dataRow["DistribSubsidy"], 0),
                 BusinessCommission = ParseHelper.ToDecimal(dataRow["BusinessCommission"], 0),
                 BalancePrice = ParseHelper.ToDecimal(dataRow["BalancePrice"], 0),
                 CommissionFixValue = ParseHelper.ToDecimal(dataRow["CommissionFixValue"], 0),
                 CommissionType = ParseHelper.ToInt(dataRow["CommissionType"], 0),
                 GroupBusinessAmount = ParseHelper.ToDecimal(dataRow["GroupBusinessAmount"], 0),
                 ReceivableType = ParseHelper.ToInt(dataRow["ReceivableType"], 0),
                 SetpChargeId = ParseHelper.ToInt(dataRow["SetpChargeId"], 0),
                 TaskDistributionId = ParseHelper.ToInt(dataRow["TaskDistributionId"], 0)             

             });
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
                #region 绑定头像信息

                string CheckPicUrl = dataReader["CheckPicUrl"].ToString();
                if (string.IsNullOrEmpty(CheckPicUrl))
                {
                    CheckPicUrl = string.Empty;
                }
                else
                {
                    CheckPicUrl = Ets.Model.Common.ImageCommon.GetUserImage(CheckPicUrl, ImageType.Business);
                }
                #endregion

                result.CheckPicUrl = CheckPicUrl;
                result.BusinessLicensePic = string.IsNullOrEmpty(Convert.ToString(dataReader["BusinessLicensePic"])) ?
                    string.Empty :
                    Ets.Model.Common.ImageCommon.GetUserImage(dataReader["BusinessLicensePic"].ToString(), ImageType.Business);
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
                obj = dataReader["OneKeyPubOrder"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.OneKeyPubOrder = ParseHelper.ToInt(obj.ToString());
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
         b.ProvinceCode ,
         b.CityCode ,
         b.AreaCode ,
         b.Province ,
         ISNULL(b.DistribSubsidy,0.00) DistribSubsidy,
         b.BusinessCommission ,     
         b.CommissionType,
         b.CommissionFixValue,
         b.BusinessGroupId,
         b.BalancePrice,
         b.AllowWithdrawPrice,
         b.HasWithdrawPrice,
         b.CheckPicUrl,
         b.BusinessLicensePic,
         b.MealsSettleMode,
         b.GroupId,
         b.OriginalBusiId,
         bfa.TrueName,
         bfa.AccountNo,
         bfa.AccountType,
         bfa.OpenBank,
         bfa.OpenSubBank,
         g.GroupName,
         ISNULL(g.IsModifyBind,0) IsModifyBind,
         ISNULL(b.OneKeyPubOrder,0) OneKeyPubOrder,
         b.IsAllowOverdraft,
         b.IsEmployerTask,
         ISNULL(b.RecommendPhone,'') AS RecommendPhone,
         b.IsOrderChecked ,b.IsAllowCashPay,b.PushOrderType
FROM business b WITH(NOLOCK) 
	Left join BusinessFinanceAccount bfa WITH(NOLOCK) ON b.Id=bfa.BusinessId AND bfa.IsEnable=1
    Left join [group] g WITH(NOLOCK) on g.Id=b.GroupId 
WHERE b.Id = @BusinessId  ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessId", businessId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, selSql, parm));
            if (dt != null && dt.Rows.Count > 0)
                return DataTableHelper.ConvertDataTableList<BusinessDetailModel>(dt)[0];
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

        /// <summary>
        /// 通过订单ID，用于查询商家信息用
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public BusinessDM GetByOrderId(int orderId)
        {
            string sql = @"
select b.Id, Name FROM dbo.[order] o(nolock)
join dbo.business b (nolock) on o.businessId = b.Id
 where o.Id = @orderId
            ";
            IDbParameters parm = DbHelper.CreateDbParameters("orderId", DbType.Int32, 4, orderId);
            return DbHelper.QueryForObjectDelegate<BusinessDM>(SuperMan_Read, sql, parm, datarow => new BusinessDM
             {
                 Id = ParseHelper.ToInt(datarow["Id"].ToString()),
                 Name = datarow["Name"].ToString()
             });
        }
        /// <summary>
        /// 获取商户第三方绑定关系记录
        /// danny-20150602
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public IList<BusinessThirdRelationModel> GetBusinessThirdRelation(int businessId)
        {
            string sql = @"  
SELECT btr.[Id]
      ,btr.[BusinessId]
      ,btr.[OriginalBusiId]
      ,btr.[GroupId]
      ,btr.[GroupName]
      ,btr.[AuditStatus]
      ,ISNULL(g.IsModifyBind,0) IsModifyBind
FROM BusinessThirdRelation btr with(nolock)
JOIN [group] g with(nolock) on btr.GroupId=g.Id
WHERE btr.BusinessId=@BusinessId
ORDER BY btr.Id;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessId", businessId);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<BusinessThirdRelationModel>(dt);
        }
        /// <summary>
        /// 修改商户详细信息
        /// danny-20150602
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyBusinessDetail(BusinessDetailModel model)
        {
            BusListResultModel brm = GetBusiness(model.Id);
            string remark = GetRemark(brm, model);
            //商铺名称、联系电话、联系座机、配 送 费、城 市、地 址、经 纬 度、结算比例设置、补贴策略设置(应付)、
            //补贴策略、餐费结算方式、一键发单、余额可以透支、使用雇主任务时间限制、第三方ID 
            //结算类型去掉，以后这一列不在有用了
            string sql = @"UPDATE business 
                            SET Name=@Name,
                                Landline=@Landline,
                                PhoneNo2=@PhoneNo2,
                                DistribSubsidy=@DistribSubsidy,
                                CityId=@CityId,
                                districtId=@districtId,
                                City=@City,
                                district=@district,
                                Address=@Address,
                                Latitude=@Latitude,
                                Longitude=@Longitude,
                                BusinessGroupId=@BusinessGroupId,
                                MealsSettleMode=@MealsSettleMode, 
                                OriginalBusiId=@OriginalBusiId,
                                OneKeyPubOrder=@OneKeyPubOrder,
                                IsEmployerTask=@IsEmployerTask,
                                IsAllowOverdraft=@IsAllowOverdraft,
                                IsOrderChecked=@IsOrderChecked,
                                RecommendPhone=@RecommendPhone,
                                BusinessCommission=@BusinessCommission,
                                CommissionFixValue=@CommissionFixValue,IsAllowCashPay=@IsAllowCashPay,
                                PushOrderType=@PushOrderType                                  
                                           ";
            if (model.GroupId > 0)
            {
                sql += " ,GroupId=@GroupId, ";
            }
            sql += @" OUTPUT
                        Inserted.Id,
                        @OptId,
                        @OptName,
                        GETDATE(),
                        3,
                        @Remark
                    INTO BusinessOptionLog
                        (BusinessId,
                        OptId,
                        OptName,
                        InsertTime,
                        Platform,
                        Remark)
                        WHERE  Id = @Id;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Name", model.Name);
            parm.AddWithValue("@Landline", model.Landline);
            parm.AddWithValue("@PhoneNo2", model.PhoneNo2);
            parm.AddWithValue("@DistribSubsidy", model.DistribSubsidy);
            parm.AddWithValue("@CityId", model.CityId);
            parm.AddWithValue("@districtId", model.districtId);
            parm.AddWithValue("@City", model.City);
            parm.AddWithValue("@district", model.district);
            parm.AddWithValue("@Address", model.Address);
            parm.AddWithValue("@Latitude", model.Latitude);
            parm.AddWithValue("@Longitude", model.Longitude);
            //parm.AddWithValue("@CommissionType", model.CommissionType);
            decimal businessCommission = 0;
            if (model.BusinessCommission.HasValue)
            {
                businessCommission = model.BusinessCommission.Value;
            }
            parm.AddWithValue("@BusinessCommission", Math.Round(businessCommission, 1));
            parm.AddWithValue("@CommissionFixValue", model.CommissionFixValue);
            parm.AddWithValue("@BusinessGroupId", model.BusinessGroupId);
            parm.AddWithValue("@MealsSettleMode", model.MealsSettleMode);
            parm.AddWithValue("@GroupId", model.GroupId);
            parm.AddWithValue("@OriginalBusiId", model.OriginalBusiId ?? 0);
            parm.AddWithValue("@Id", model.Id);
            parm.AddWithValue("@OptId", model.OptUserId);
            parm.AddWithValue("@OptName", model.OptUserName);
            parm.AddWithValue("@Remark", remark);
            parm.AddWithValue("@OneKeyPubOrder", model.OneKeyPubOrder);
            parm.AddWithValue("@IsEmployerTask", model.IsEmployerTask);
            parm.Add("@IsAllowOverdraft", DbType.Int16).Value = model.IsAllowOverdraft;
            parm.Add("@RecommendPhone", DbType.String).Value = model.RecommendPhone ?? ""; //推荐人手机号 
            parm.Add("@IsOrderChecked", DbType.Int32).Value = model.IsOrderChecked;
            parm.Add("@IsAllowCashPay", DbType.Int32).Value = model.IsAllowCashPay;
            parm.Add("@PushOrderType", DbType.Int32).Value = model.PushOrderType;
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 修改商户信息，构建备注字段
        /// </summary>
        /// <param name="brm"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetRemark(BusListResultModel brm, BusinessDetailModel model)
        {
            StringBuilder remark = new StringBuilder(model.OptUserName + "通过后台管理系统修改商户信息:");
            if (brm.Id > 0)
            {
                if (brm.Name != model.Name)
                {
                    remark.AppendFormat("商户名原值:{0},修改为{1};", brm.Name, model.Name);
                }
                if (brm.PhoneNo2 != model.PhoneNo2)
                {
                    remark.AppendFormat("联系电话原值:{0},修改为{1};", brm.PhoneNo2, model.PhoneNo2);
                }
                //座机
                if (brm.Landline != model.Landline)
                {
                    remark.AppendFormat("联系座机原值:{0},修改为{1};", brm.Landline, model.Landline);
                }
                if (brm.DistribSubsidy != model.DistribSubsidy)
                {
                    remark.AppendFormat("配送费原值:{0},修改为{1};", brm.DistribSubsidy, model.DistribSubsidy);
                }
                if (brm.City != model.City)
                {
                    remark.AppendFormat("城市原值:{0},修改为{1};", brm.City, model.City);
                }
                if (brm.district != model.district)
                {
                    remark.AppendFormat("区域原值:{0},修改为{1};", brm.district, model.district);
                }
                if (brm.Longitude != model.Longitude)
                {
                    remark.AppendFormat("经度原值:{0},修改为{1};", brm.Longitude, model.Longitude);
                }
                if (brm.Latitude != model.Latitude)
                {
                    remark.AppendFormat("纬度原值:{0},修改为{1};", brm.Latitude, model.Latitude);
                }
                if (brm.CommissionType != model.CommissionType)
                {
                    remark.AppendFormat("结算类型原值:{0},修改为{1};", ((OrderCommissionType)brm.CommissionType).GetDisplayText(), ((OrderCommissionType)model.CommissionType).GetDisplayText());
                    if (model.CommissionType == 1)
                    {
                        remark.AppendFormat("固定比例原值:{0},修改为{1};", brm.BusinessCommission, model.BusinessCommission);
                    }
                    if (model.CommissionType == 2)
                    {
                        remark.AppendFormat("固定金额原值:{0},修改为{1};", brm.CommissionFixValue, model.CommissionFixValue);
                    }
                }
                //补贴策略 BusinessGroupId
                if (brm.BusinessGroupId != model.BusinessGroupId)
                {
                    remark.AppendFormat("补贴策略原值:{0},修改为{1};", brm.BusinessGroupId, model.BusinessGroupId);
                }
                //餐费结算方式
                if (brm.MealsSettleMode != model.MealsSettleMode)
                {
                    remark.AppendFormat("餐费结算方式原值:{0},修改为{1};", ((MealsSettleMode)brm.MealsSettleMode).GetDisplayText(), ((MealsSettleMode)model.MealsSettleMode).GetDisplayText());
                }
                //一键发单
                if (brm.OneKeyPubOrder != model.OneKeyPubOrder)
                {
                    remark.AppendFormat("一键发单原值:{0},修改为{1};", brm.OneKeyPubOrder == 1 ? "是" : "否", model.OneKeyPubOrder == 1 ? "是" : "否");
                }
                //余额可以透支
                if (brm.IsAllowOverdraft != model.IsAllowOverdraft)
                {
                    remark.AppendFormat("余额透支原值:{0},修改为{1};", brm.IsAllowOverdraft == 1 ? "可以透支" : "不可透支", model.IsAllowOverdraft == 1 ? "可以透支" : "不可透支");
                }
                //雇主任务时间限制
                if (brm.IsEmployerTask != model.IsEmployerTask)
                {
                    remark.AppendFormat("是否雇主任务:{0},修改为{1};", brm.IsEmployerTask == 1 ? "是" : "否", model.IsEmployerTask == 1 ? "是" : "否");
                }
                //第三方Id
                if (brm.OriginalBusiId.HasValue)
                {
                    if (brm.OriginalBusiId.Value != model.OriginalBusiId)
                    {
                        remark.AppendFormat("第三方ID原值:{0},修改为{1};", brm.OriginalBusiId, model.OriginalBusiId);
                    }
                }
                if (brm.RecommendPhone != model.RecommendPhone)
                {
                    remark.AppendFormat("推荐人手机号原值:{0},修改为{1};", brm.RecommendPhone, model.RecommendPhone);
                }
                if (brm.IsAllowCashPay != model.IsAllowCashPay)
                {
                    remark.AppendFormat("是否允许现金支付原值:{0},修改为{1};", brm.IsAllowCashPay == 1 ? "是" : "否", model.IsAllowCashPay == 1 ? "是" : "否");
                }
            }
            return remark.ToString();
        }
        /// <summary>
        /// 删除商户第三方绑定关系记录
        /// danny-20150602
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public bool DeleteBusinessThirdRelation(int businessId)
        {
            string sql = @" DELETE FROM [BusinessThirdRelation] WHERE BusinessId=@BusinessId;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessId", businessId);
            return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm)) > 0;
        }
        /// <summary>
        /// 添加商户第三方绑定关系记录
        /// danny-20150602
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddBusinessThirdRelation(BusinessThirdRelationModel model)
        {
            string sql = string.Format(@" 
insert into BusinessThirdRelation
            ([BusinessId]
           ,[OriginalBusiId]
           ,[GroupId]
           ,[GroupName]
           ,[AuditStatus])
VALUES
           (@BusinessId, 
            @OriginalBusiId,
            @GroupId,
            @GroupName,
            @AuditStatus);");
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessId", model.BusinessId);
            parm.AddWithValue("@OriginalBusiId", model.OriginalBusiId);
            parm.AddWithValue("@GroupId", model.GroupId);
            parm.AddWithValue("@GroupName", model.GroupName);
            parm.AddWithValue("@AuditStatus", model.AuditStatus);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 获取商户绑定的骑士列表
        /// danny-20150608
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetBusinessClienterRelationList<T>(BusinessSearchCriteria criteria)
        {
            string columnList = @"  c.TrueName ClienterName,
		                            c.PhoneNo,
		                            bcr.CreateTime,
		                            bcr.IsBind,
		                            bcr.CreateBy,
		                            bcr.UpdateBy,
		                            bcr.UpdateTime,
		                            bcr.BusinessId,
		                            bcr.ClienterId,
                                    bcr.IsEnable";
            var sbSqlWhere = new StringBuilder(" bcr.IsEnable=1 ");
            if (criteria.BusinessId != 0)
            {
                sbSqlWhere.AppendFormat("AND bcr.[BusinessId]={0}", criteria.BusinessId);
            }
            string tableList = @" BusinessClienterRelation bcr WITH(NOLOCK)
  JOIN dbo.clienter c WITH(NOLOCK) ON bcr.ClienterId=c.Id";
            string orderByColumn = " bcr.Id DESC";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }

        /// <summary>
        /// 查询商户绑定骑士数量
        /// danny-20150608
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public int GetBusinessBindClienterQty(int businessId)
        {
            try
            {
                string sql = "SELECT COUNT(1) FROM BusinessClienterRelation bcr WITH(NOLOCK) WHERE bcr.IsBind = 1 AND bcr.IsEnable=1 AND BusinessId=@BusinessId;";
                var parm = DbHelper.CreateDbParameters();
                parm.AddWithValue("@BusinessId", businessId);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, sql, parm));
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "查询商户绑定骑士数量");
                return 0;
            }
        }
        /// <summary>
        /// 查询骑士绑定商户数量
        /// danny-20150722
        /// </summary>
        /// <param name="clienterId">骑士Id</param>
        /// <returns></returns>
        public int GetClienterBindBusinessQty(int clienterId)
        {
            try
            {
                string sql = "SELECT COUNT(1) FROM BusinessClienterRelation bcr WITH(NOLOCK) WHERE bcr.IsBind = 1 AND bcr.IsEnable=1 AND ClienterId=@ClienterId;";
                var parm = DbHelper.CreateDbParameters();
                parm.AddWithValue("@ClienterId", clienterId);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, sql, parm));
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "查询骑士绑定商户数量");
                return 0;
            }
        }
        /// <summary>
        /// 修改骑士绑定关系
        /// danny-20150608
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyClienterBind(ClienterBindOptionLogModel model)
        {
            string sql = string.Format(@" 
update bcr
set    bcr.IsBind=@IsBind
OUTPUT
  Inserted.BusinessId,
  Inserted.ClienterId,
  @OptId,
  @OptName,
  getdate(),
  @Remark
INTO ClienterBindOptionLog
  ( BusinessId
   ,ClienterId
   ,OptId
   ,OptName
   ,InsertTime
   ,Remark)
from BusinessClienterRelation bcr WITH ( ROWLOCK )
where bcr.BusinessId=@BusinessId AND bcr.ClienterId=@ClienterId;");
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OptId", model.OptId);
            parm.AddWithValue("@OptName", model.OptName);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@IsBind", model.IsBind);
            parm.AddWithValue("@BusinessId", model.BusinessId);
            parm.AddWithValue("@ClienterId", model.ClienterId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 删除骑士绑定关系
        /// danny-20150608
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool RemoveClienterBind(ClienterBindOptionLogModel model)
        {
            string sql = string.Format(@" 
update bcr
set    bcr.IsEnable=@IsEnable
OUTPUT
  Inserted.BusinessId,
  Inserted.ClienterId,
  @OptId,
  @OptName,
  getdate(),
  @Remark
INTO ClienterBindOptionLog
  ( BusinessId
   ,ClienterId
   ,OptId
   ,OptName
   ,InsertTime
   ,Remark)
from BusinessClienterRelation bcr WITH ( ROWLOCK )
where bcr.BusinessId=@BusinessId AND bcr.ClienterId=@ClienterId;");
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OptId", model.OptId);
            parm.AddWithValue("@OptName", model.OptName);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@IsEnable", 0);
            parm.AddWithValue("@BusinessId", model.BusinessId);
            parm.AddWithValue("@ClienterId", model.ClienterId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 添加骑士绑定关系
        /// danny-20150609
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddClienterBind(ClienterBindOptionLogModel model)
        {
            string sql = string.Format(@" 
INSERT INTO [BusinessClienterRelation]
           ([BusinessId]
           ,[ClienterId]
           ,[CreateBy]
           ,[UpdateBy])
	OUTPUT
	  Inserted.BusinessId,
	  Inserted.ClienterId,
	  @OptId,
	  @OptName,
	  getdate(),
	  @Remark
	INTO ClienterBindOptionLog
	  ( BusinessId
	   ,ClienterId
	   ,OptId
	   ,OptName
	   ,InsertTime
	   ,Remark)
VALUES
       (@BusinessId
       ,@ClienterId
       ,@OptName
       ,@OptName);");
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@OptId", model.OptId);
            parm.AddWithValue("@OptName", model.OptName);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@BusinessId", model.BusinessId);
            parm.AddWithValue("@ClienterId", model.ClienterId);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 修改商户绑定骑士标志
        /// danny-20150610
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="isBind"></param>
        /// <returns></returns>
        public bool UpdateBusinessIsBind(int businessId, int isBind)
        {
            string updateSql = @"update business set IsBind = @IsBind where Id = @BusinessId;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessId", businessId);
            parm.AddWithValue("@IsBind", isBind);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, parm) > 0;
        }
        /// <summary>
        /// 修改骑士绑定标志
        /// danny-20150610
        /// </summary>
        /// <param name="clienterId"></param>
        /// <param name="isBind"></param>
        /// <returns></returns>
        public bool UpdateClienterIsBind(int clienterId, int isBind)
        {
            string updateSql = @"update clienter set IsBind = @IsBind where Id = @ClienterId;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@ClienterId", clienterId);
            parm.AddWithValue("@IsBind", isBind);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, parm) > 0;
        }
        /// <summary>
        /// 验证是否有绑定关系
        /// danny-20150609
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CheckHaveBind(ClienterBindOptionLogModel model)
        {
            string sql = "SELECT COUNT(1) FROM BusinessClienterRelation bcr WITH(NOLOCK) WHERE  bcr.IsEnable=1 AND BusinessId=@BusinessId AND ClienterId=@ClienterId;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessId", model.BusinessId);
            parm.AddWithValue("@ClienterId", model.ClienterId);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm)) > 0;
        }
        /// <summary>
        /// 查询商户结算列表（分页）
        /// danny-20150609
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetBusinessCommissionOfPaging<T>(Ets.Model.ParameterModel.Business.BusinessCommissionSearchCriteria criteria)
        {

            string columnList = string.Format(@"   BB.id ,
                                        BB.Name ,
                                        BB.PhoneNo,
                                        T.Amount ,
                                        T.OrderCount ,
                                        ISNULL(BB.BusinessCommission, 0) BusinessCommission ,
                                        T.TotalAmount,
                                        '{0}' AS T1,
                                        '{1}' AS T2 ", criteria.T1, criteria.T2);
            string sqlwhere = "";
            if (!string.IsNullOrEmpty(criteria.Name))
            {
                sqlwhere += string.Format(" AND Name='{0}' ", criteria.Name);
            }
            if (!string.IsNullOrEmpty(criteria.PhoneNo))
            {
                sqlwhere += string.Format(" AND PhoneNo='{0}' ", criteria.PhoneNo);
            }
            if (criteria.GroupId != 0)
            {
                sqlwhere += string.Format(" AND groupid={0} ", criteria.GroupId);
            }
            if (!string.IsNullOrEmpty(criteria.BusinessCity))
            {
                sqlwhere += string.Format(" AND B.City='{0}' ", criteria.BusinessCity);
            }
            if (criteria.AuthorityCityNameListStr != null && !string.IsNullOrEmpty(criteria.AuthorityCityNameListStr.Trim()) && criteria.UserType != 0)
            {
                sqlwhere += string.Format("  AND B.City IN({0}) ", criteria.AuthorityCityNameListStr);
            }
            sqlwhere += string.Format(" AND O.ActualDoneDate BETWEEN '{0}' AND '{1}' ", criteria.T1, criteria.T2); ;
            string tableList = string.Format(@" business BB WITH ( NOLOCK )
                                        JOIN ( SELECT B.Id AS id ,
                                                            SUM(O.Amount) AS Amount ,
                                                            SUM(ISNULL(O.OrderCount, 0)) AS OrderCount ,
                                                            SUM(ISNULL(O.SettleMoney, 0)) AS TotalAmount
                                                     FROM   dbo.[order] O WITH ( NOLOCK )
                                                            JOIN dbo.business B ON B.Id = o.businessId
                                                     WHERE  O.[Status] = 1 {0}
                                                     GROUP BY B.Id
                                                   ) AS T ON BB.Id = T.Id ", sqlwhere);
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            string orderByColumn = " BB.Id ASC";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }

        /// <summary>
        /// 获取商家电话号码
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150617</UpdateTime>
        /// <param name="sentStatus"></param>
        /// <returns></returns>
        public DataTable GetPhoneNoList(string pushCity)
        {
            string querysql = @"  
 select id, PhoneNo from dbo.business 
 where   cityid in(" + pushCity + ")";
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, querysql);
            return dt;
        }

        /// <summary>
        /// 查询所有有效商户的总余额
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150623</UpdateTime>
        /// <param name="pushCity"></param>
        /// <returns></returns>
        public decimal QueryAllBusinessTotalBalance()
        {
            string querysql = @"SELECT sum(BalancePrice) as total FROM dbo.business(nolock) b where Status=1";

            object obj = DbHelper.ExecuteScalar(SuperMan_Read, querysql);
            return ParseHelper.ToDecimal(obj, 0);
        }
        /// <summary>
        /// 获取商户操作记录
        /// wc
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns> 
        public List<BusinessOptionLog> GetBusinessOpLog(int businessId)
        {
            string querySql = @"select OptName,InsertTime,Remark from dbo.BusinessOptionLog(nolock) where BusinessId = @BusinessId order by Id desc";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessId", DbType.Int32).Value = businessId;
            DataTable dt = DbHelper.ExecuteDataTable(Config.SuperMan_Read, querySql, parm);
            IList<BusinessOptionLog> list = ConvertDataTableList<BusinessOptionLog>(dt);
            return list.ToList();
        }

        /// <summary>
        /// 判断推荐人手机号是否正确 2015年7月6日14:51:38 茹化肖
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns>-1 :不存在 0:骑士 其他:物流公司的ID</returns>
        public int CheckRecommendPhone(string phoneNum)
        {
            const string sql = @"SELECT ISNULL(MIN(T.PemType),-1) FROM (
SELECT DeliveryCompanyCode AS Phone,id AS PemType FROM DeliveryCompany (NOLOCK) WHERE IsEnable=1
UNION
SELECT PhoneNo AS Phone,0 AS PemType FROM clienter(NOLOCK) WHERE Status=1
) AS T WHERE T.Phone = @PhoneNo";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@PhoneNo", DbType.String).Value = phoneNum;
            object obj = DbHelper.ExecuteScalar(SuperMan_Read, sql, parm);
            return ParseHelper.ToInt(obj, -1);
        }
        /// <summary>
        /// 获取商户和快递公司关系列表
        /// danny-20150706
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public IList<BusinessExpressRelation> GetBusinessExpressRelationList(int businessId)
        {
            string sql = @"  
SELECT [Id]
      ,[BusinessId]
      ,[ExpressId]
      ,[IsEnable]
      ,[CreateBy]
      ,[CreateTime]
      ,[UpdateBy]
      ,[UpdateTime]
FROM BusinessExpressRelation with(nolock)
WHERE BusinessId=@BusinessId
ORDER BY Id;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessId", businessId);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<BusinessExpressRelation>(dt);
        }
        /// <summary>
        /// 编辑商户和快递公司绑定关系
        /// danny-20150706
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool EditBusinessExpressRelation(BusinessExpressRelationModel model)
        {
            string updateSql = @"
MERGE INTO BusinessExpressRelation ber
	USING(values(@BusinessId,@ExpressId)) AS berNew(BusinessId,ExpressId)
		ON ber.BusinessId=berNew.BusinessId AND  ber.ExpressId=berNew.ExpressId
	WHEN MATCHED 
	THEN UPDATE 
		 SET ber.IsEnable=@IsEnable,
             ber.UpdateBy=@OptName,
             ber.UpdateTime=getdate()
	WHEN NOT MATCHED 
		  THEN INSERT
					(BusinessId,
					 ExpressId,
                     CreateBy,
                     UpdateBy,
                     IsEnable) 
					VALUES 
					(@BusinessId,
					 @ExpressId,
                     @OptName,
                     @OptName,
                     @IsEnable);";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessId", model.BusinessId);
            parm.AddWithValue("@ExpressId", model.ExpressId);
            parm.AddWithValue("@IsEnable", model.IsEnable);
            parm.AddWithValue("@OptName", model.OptName);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, parm) > 0;
        }

        /// <summary>
        /// 获取商户图片   add by pengyi 20150709  仅限工具使用
        /// </summary>
        public IList<BusinessPicModel> GetBusinessPics()
        {
            var sql = @"select b.Id,b.CheckPicUrl,b.BusinessLicensePic from [business](nolock) as b where b.CheckPicUrl is not null or b.BusinessLicensePic is not null";
            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql));
            var list = ConvertDataTableList<BusinessPicModel>(dt);
            return list;
        }

        /// <summary>
        /// 插入商家坐标位置    add by 彭宜    20150727
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        public long InsertLocation(int businessId, decimal longitude, decimal latitude, string platform)
        {
            const string insertSql = @"
insert into BusinessLocation(Longitude,Latitude,BusinessId,Platform)
values(@Longitude,@Latitude,@BusinessId,@Platform)
select @@IDENTITY
";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Longitude", longitude);
            dbParameters.AddWithValue("Latitude", latitude);
            dbParameters.AddWithValue("BusinessId", businessId);
            dbParameters.AddWithValue("Platform", platform);
            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            return ParseHelper.ToLong(result);
        }

        /// <summary>
        /// 获得商家app启动热力图
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IList<AppActiveInfo> GetAppActiveInfos(string cityId)
        {
            const string sql = @"
select 1 as UserType, b.Name as TrueName,b.Longitude,b.Latitude,b.PhoneNo as Phone from business b (NOLOCK) where b.CityId=@CityId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("CityId", cityId.Trim());
            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, dbParameters));
            var list = MapRows<AppActiveInfo>(dt);
            return list;
        }

        /// <summary>
        ///  更新商户余额、可提现余额  
        ///  胡灵波
        ///  2015年8月13日 16:19:04
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void UpdateForWithdrawC(UpdateForWithdrawPM model)
        {
            const string updateSql = @"
update  business
set  BalancePrice=BalancePrice+@Money,
AllowWithdrawPrice=AllowWithdrawPrice+@Money
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", model.Id);
            dbParameters.AddWithValue("Money", model.Money);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }
        /// <summary>
        /// 获取商户和骑士关系列表
        /// danny-20150818
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public IList<BusinessClienterRelationModel> GetBusinessClienterRelationList(int businessId)
        {
            string sql = @"  
SELECT bcr.[Id]
      ,bcr.[BusinessId]
      ,bcr.[ClienterId]
      ,bcr.[IsEnable]
      ,bcr.[CreateBy]
      ,bcr.[CreateTime]
      ,bcr.[UpdateBy]
      ,bcr.[UpdateTime]
      ,bcr.[IsBind]
      ,c.PhoneNo
      ,c.TrueName ClienterName
FROM [BusinessClienterRelation] bcr WITH(NOLOCK)
JOIN dbo.clienter c WITH(NOLOCK) ON bcr.ClienterId=c.Id
WHERE bcr.IsEnable=1 
    AND bcr.IsBind=1
    AND c.IsBind=1 
    AND c.[Status]=1
    AND c.IsReceivePush=1 
    AND c.WorkStatus=0
    AND bcr.BusinessId=@BusinessId;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessId", businessId);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<BusinessClienterRelationModel>(dt);
        }
        /// <summary>
        /// 获取快递公司骑士列表
        /// danny-20150819
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IList<BusinessExpressRelationModel> GetExpressClienterList(BusinessExpressRelationModel model)
        {
            string sql = @"  
SELECT ber.[Id]
      ,ber.[BusinessId]
      ,ber.[ExpressId]
      ,ber.[IsEnable]
      ,ber.[CreateBy]
      ,ber.[CreateTime]
      ,ber.[UpdateBy]
      ,ber.[UpdateTime]
      ,c.PhoneNo
      ,c.TrueName 
      ,c.Id ClienterId
FROM BusinessExpressRelation ber with(nolock) 
 JOIN dbo.clienter c WITH(NOLOCK) ON ber.ExpressId=c.DeliveryCompanyId AND ber.IsEnable=1 AND c.[Status]=1 AND c.WorkStatus=0 
    AND c.IsReceivePush=1  AND ber.BusinessId=@BusinessId 
 JOIN( SELECT cl.ClienterId
			 ,cl.CreateTime
			 ,cl.Latitude
			 ,cl.Longitude
	   FROM (
			   SELECT ClienterId,MAX(ID) ID
			   FROM ClienterLocation  WITH(NOLOCK)
			   WHERE CreateTime>DATEADD(MINUTE,-10,GetDate())
			   GROUP BY ClienterId) tbl
	  JOIN ClienterLocation  cl WITH(NOLOCK) ON cl.ID = tbl.ID) tblcl ON tblcl.ClienterId = c.Id
WHERE  geography::Point(ISNULL(@Latitude,0),ISNULL(@Longitude,0),4326).STDistance(geography::Point(tblcl.Latitude,tblcl.Longitude,4326))<=@PushRadius;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessId", model.BusinessId);
            parm.AddWithValue("@Latitude", model.Latitude);
            parm.AddWithValue("@Longitude", model.Longitude);
            parm.AddWithValue("@PushRadius", ParseHelper.ToInt(model.PushRadius) * 1000);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<BusinessExpressRelationModel>(dt);
        }
        /// <summary>
        /// 获取附近店内骑士列表
        /// danny-20150828
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IList<LocalClienterModel> GetBusinessLocalRelClienterList(LocalClienterParameter model)
        {
            string sql = @"  
SELECT   cl.ClienterId
		,cl.Latitude
		,cl.Longitude
INTO #tempActiveClienter
FROM ( SELECT ClienterId,MAX(ID) ID
	   FROM ClienterLocation  WITH(NOLOCK)
	   WHERE CreateTime>DATEADD(MINUTE,-10,GetDate())
	   GROUP BY ClienterId) tbl
JOIN ClienterLocation  cl WITH(NOLOCK) ON cl.ID = tbl.ID;

SELECT c.Id ClienterId
        ,c.TrueName ClienterName
        ,c.PhoneNo
        ,c.WorkStatus
        ,ROUND(geography::Point(ISNULL(@Latitude,0),ISNULL(@Longitude,0),4326).STDistance(geography::Point(tac.Latitude,tac.Longitude,4326)),0) Radius
        ,tac.Latitude
        ,tac.Longitude
INTO #tempLocalClienter
FROM #tempActiveClienter tac
JOIN [BusinessClienterRelation] bcr WITH(NOLOCK) ON tac.ClienterId = bcr.ClienterId
JOIN dbo.clienter c WITH(NOLOCK) ON bcr.ClienterId=c.Id AND bcr.IsEnable=1 AND bcr.IsBind=1 AND c.IsBind=1 AND c.[Status]=1 AND bcr.BusinessId=@BusinessId
WHERE geography::Point(ISNULL(@Latitude,0),ISNULL(@Longitude,0),4326).STDistance(geography::Point(tac.Latitude,tac.Longitude,4326))<=@PushRadius

IF NOT EXISTS(SELECT COUNT(1) FROM #tempLocalClienter)
    BEGIN
        INSERT INTO #tempLocalClienter 
        SELECT c.Id ClienterId
	            ,c.TrueName ClienterName
                ,c.PhoneNo
                ,c.WorkStatus
                ,ROUND(geography::Point(ISNULL(@Latitude,0),ISNULL(@Longitude,0),4326).STDistance(geography::Point(tac.Latitude,tac.Longitude,4326)),0) Radius
                ,tac.Latitude
                ,tac.Longitude
        FROM #tempActiveClienter tac
        JOIN dbo.clienter c WITH(NOLOCK) ON tac.ClienterId=c.Id AND c.Status=1
        JOIN BusinessExpressRelation ber with(nolock) ON ber.ExpressId=c.DeliveryCompanyId AND  ber.BusinessId=@BusinessId AND ber.IsEnable=1
        WHERE  geography::Point(ISNULL(@Latitude,0),ISNULL(@Longitude,0),4326).STDistance(geography::Point(tac.Latitude,tac.Longitude,4326))<=@PushRadius
    END

SELECT  TOP 20 templc.ClienterName,
		templc.PhoneNo,
		templc.Radius,
		templc.WorkStatus,
        templc.Latitude,
        templc.Longitude,
		ISNULL(tbllac.ReceiveQty,0) ReceiveQty,
		ISNULL(tbllac.TransferQty,0) TransferQty,
		ISNULL(tbllac.FinishQty,0) FinishQty,
        1 IsEmployerTask
FROM #tempLocalClienter templc
LEFT JOIN(
SELECT tlc.clienterId,
	   COUNT(CASE WHEN o.Status=2 THEN 1  END) ReceiveQty,
	   COUNT(CASE WHEN o.Status=4 THEN 1  END) TransferQty,
       COUNT(CASE WHEN o.Status=1 THEN 1  END) FinishQty
FROM #tempLocalClienter tlc
JOIN dbo.[order] o WITH(NOLOCK) ON tlc.ClienterId=o.clienterId
GROUP BY tlc.clienterId) tbllac ON templc.ClienterId=tbllac.clienterId
ORDER BY templc.WorkStatus;
";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessId", model.BusinessId);
            parm.AddWithValue("@Latitude", model.Latitude);
            parm.AddWithValue("@Longitude", model.Longitude);
            parm.AddWithValue("@PushRadius", ParseHelper.ToInt(model.PushRadius) * 1000);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<LocalClienterModel>(dt);
        }
        /// <summary>
        /// 获取附近骑士列表
        /// danny-20150831
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IList<LocalClienterModel> GetBusinessLocaClienterList(LocalClienterParameter model)
        {
            string sql = @"  
SELECT cl.ClienterId
	  ,cl.Latitude
	  ,cl.Longitude
INTO #tempActiveClienter
FROM ( SELECT ClienterId,MAX(ID) ID
	   FROM ClienterLocation  WITH(NOLOCK)
	   WHERE CreateTime>DATEADD(MINUTE,-10,GetDate())
	   GROUP BY ClienterId) tbl
JOIN ClienterLocation  cl WITH(NOLOCK) ON cl.ID = tbl.ID;

SELECT c.Id ClienterId
	  ,c.TrueName ClienterName
      ,c.PhoneNo
      ,c.WorkStatus
      ,ROUND(geography::Point(ISNULL(@Latitude,0),ISNULL(@Longitude,0),4326).STDistance(geography::Point(tac.Latitude,tac.Longitude,4326)),0) Radius
      ,tac.Latitude
      ,tac.Longitude
INTO #tempLocalClienter
FROM #tempActiveClienter tac
JOIN dbo.clienter c WITH(NOLOCK) ON tac.ClienterId=c.Id AND c.Status=1
JOIN(   SELECT ExpressId
		FROM BusinessExpressRelation ber with(nolock) 
		WHERE BusinessId=@BusinessId AND ber.IsEnable=1
		UNION SELECT 0 ExpressId) tblbe ON c.DeliveryCompanyId=tblbe.ExpressId
WHERE  geography::Point(ISNULL(@Latitude,0),ISNULL(@Longitude,0),4326).STDistance(geography::Point(tac.Latitude,tac.Longitude,4326))<=@PushRadius;

SELECT  TOP 20 templc.ClienterName,
		templc.PhoneNo,
		templc.Radius,
		templc.WorkStatus,
        templc.Latitude,
        templc.Longitude,
		ISNULL(tbllac.ReceiveQty,0) ReceiveQty,
		ISNULL(tbllac.TransferQty,0) TransferQty,
		ISNULL(tbllac.FinishQty,0) FinishQty,
        0 IsEmployerTask
FROM #tempLocalClienter templc
LEFT JOIN(
SELECT tlc.clienterId,
	   COUNT(CASE WHEN o.Status=2 THEN 1  END) ReceiveQty,
	   COUNT(CASE WHEN o.Status=4 THEN 1  END) TransferQty,
       COUNT(CASE WHEN o.Status=1 THEN 1  END) FinishQty
FROM #tempLocalClienter tlc
JOIN dbo.[order] o WITH(NOLOCK) ON tlc.ClienterId=o.clienterId
GROUP BY tlc.clienterId) tbllac ON templc.ClienterId=tbllac.clienterId
ORDER BY templc.WorkStatus;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@BusinessId", model.BusinessId);
            parm.AddWithValue("@Latitude", model.Latitude);
            parm.AddWithValue("@Longitude", model.Longitude);
            parm.AddWithValue("@PushRadius", ParseHelper.ToInt(model.PushRadius) * 1000);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<LocalClienterModel>(dt);
        }
        
        public bool UpdateBusinessIsEnable(BusinessAuditModel bam)
        {
            bool reslut = false;
            string remark = "";
            try
            {
                StringBuilder sql = new StringBuilder();
                if (bam.IsEnable == 1)
                {
                    remark = "启用状态修改为禁用";
                }
                else
                {
                    remark = "启用状态修改为启用";
                } 
                sql.AppendFormat(@" update business set Status={0},IsEnable ={1} OUTPUT
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
                                              Remark) ", bam.AuditStatus.GetHashCode(),bam.IsEnable);

                sql.Append(" where id=@id;");
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("id", bam.BusinessId);
                dbParameters.AddWithValue("@OptId", bam.OptionUserId);
                dbParameters.AddWithValue("@OptName", bam.OptionUserName);
                dbParameters.AddWithValue("@Platform", 3);
                dbParameters.AddWithValue("@Remark", remark);
                int i = DbHelper.ExecuteNonQuery(Config.SuperMan_Write, sql.ToString(), dbParameters);

                if (i > 0)
                { 
                    reslut = true;
                }
            }
            catch (Exception ex)
            { 
                LogHelper.LogWriter(ex, "更新启用状态");
                throw;
            }
            return reslut;
        }
    }
}
