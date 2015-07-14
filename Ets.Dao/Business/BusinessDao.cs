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
                                    isnull(o.OriginalOrderNo,'') as OriginalOrderNo";
            string tableList = @" [order](nolock) as o 
                                   join business(nolock) as b on o.businessId=b.Id
                                   left join clienter(nolock) as c on o.clienterId=c.Id ";  //表名

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
                                    ,ISNULL(b.MealsSettleMode,0) MealsSettleMode
                                    ,ISNULL(b.BalancePrice,0) BalancePrice
                                    ,ISNULL(b.AllowWithdrawPrice,0) AllowWithdrawPrice
                                    ,ISNULL(b.RecommendPhone,'') as RecommendPhone";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(criteria.RecommendPhone))
            {
                sbSqlWhere.AppendFormat(" AND b.RecommendPhone='{0}' ", criteria.RecommendPhone.Trim());
            }
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
            //else
            //{
            //    sbSqlWhere.AppendFormat(" AND b.City IN ({0}) ", criteria.AuthorityCityNameListStr.Trim());
            //}
            if (!string.IsNullOrEmpty(criteria.AuthorityCityNameListStr))
            {
                sbSqlWhere.AppendFormat(" AND b.City IN ({0}) ", criteria.AuthorityCityNameListStr.Trim());
            }
            string tableList = @" business  b WITH (NOLOCK)  
                                LEFT JOIN dbo.[group] g WITH(NOLOCK) ON g.Id = b.GroupId 
                                JOIN dbo.[BusinessGroup]  bg WITH ( NOLOCK ) ON  b.BusinessGroupId=bg.Id";
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
insert into dbo.business (City,PhoneNo,PhoneNo2,Password,CityId,RecommendPhone)
values( @City,@PhoneNo,@PhoneNo2,@Password,@CityId ,@RecommendPhone)
select @@IDENTITY";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@City", model.city);
            dbParameters.AddWithValue("@Password", model.passWord);
            dbParameters.AddWithValue("@PhoneNo", model.phoneNo);
            dbParameters.AddWithValue("@PhoneNo2", model.phoneNo);
            dbParameters.AddWithValue("@CityId", model.CityId);
            dbParameters.AddWithValue("@RecommendPhone", model.RecommendPhone);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters));
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
        ISNULL(a.OriginalBusiId, 0) as OriginalBusiId
from    business (nolock) a
        left join dbo.[group] (nolock) b on a.GroupId = b.Id
where   PhoneNo = @PhoneNo
        and Password = @Password
        and ISNULL(b.IsModifyBind,1) = 1
order by a.id desc
";
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
                                b.MealsSettleMode,
                                b.OriginalBusiId,
                                BusinessGroup.StrategyId,
                                b.BalancePrice,
                                b.OneKeyPubOrder 
                                FROM dbo.business as b WITH(NOLOCK)
                                left join BusinessGroup on b.BusinessGroupId=BusinessGroup.Id
                                WHERE b.Id = @busiId";
            ///TODO 类型？
            IDbParameters parm = DbHelper.CreateDbParameters("busiId", DbType.Int32, 4, busiId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, selSql, parm));
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
        /// <param name="busiAddress"></param>
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
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enumStatusType"></param>
        /// <returns></returns>
        public bool UpdateAuditStatus(int id, int enumStatus, string busiAddress)
        {
            bool reslut = false;
            try
            {
                StringBuilder sql = new StringBuilder("update business set Status=@enumStatus ");

                if (!string.IsNullOrWhiteSpace(busiAddress))  //当商户地址不为空的时候，修改商户地址
                {
                    sql.Append(" ,Address=@Address ");
                }
                sql.Append(" where id=@id");
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.Add("id", DbType.Int32, 4).Value = id;
                parm.Add("@Address", DbType.String).Value = busiAddress; //商户地址
                parm.Add("enumStatus", DbType.Int32, 4).Value = enumStatus;
                return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, sql.ToString(), parm)) > 0 ? true : false;
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
        /// 根据手机号获取用商家信息  B修改商户密码 用到
        /// 窦海超
        /// 2015年3月23日 19:00:52
        /// </summary>
        /// <param name="PhoneNo">手机号</param>
        /// <returns>商家信息</returns>
        public BusListResultModel GetBusinessByPhoneNo(string PhoneNo)
        {
            string sql = @"
SELECT b.Id  FROM dbo.business(NOLOCK) b
LEFT join dbo.[group] g on b.GroupId=g.Id 
where b.PhoneNo=@PhoneNo and isnull(g.IsModifyBind,1)=1";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("PhoneNo", DbType.String, 40).Value = PhoneNo;

            int tmpId = ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm), 0);
            //DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);

            if (tmpId <= 0)
            {
                return null;
            }
            BusListResultModel model = new BusListResultModel()
            {
                Id = tmpId
            };
            return model;
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
            string sql = @"select Id,Status from dbo.business where OriginalBusiId=@OriginalBusiId and GroupId=@GroupId";
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
            string sql = @"select  Id as userid,[status] as status,OneKeyPubOrder,IsAllowOverdraft,BalancePrice from dbo.business with(nolock) WHERE id=@id ";
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
        ///  商户 余额，可提现余额   add by caoheyang 20150509
        /// </summary>
        /// <param name="model">超人信息</param>
        /// <returns></returns>
        public void UpdateForWithdrawC(UpdateForWithdrawPM model)
        {
            const string updateSql = @"
update  business
set  BalancePrice=BalancePrice+@WithdrawPrice,AllowWithdrawPrice=AllowWithdrawPrice+@WithdrawPrice
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", model.Id);
            dbParameters.AddWithValue("WithdrawPrice", model.Money);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
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
                bf.BusinessId = ParseHelper.ToInt(dataRow["BusinessId"]);
                bf.TrueName = dataRow["TrueName"].ToString();
                bf.AccountNo = ETS.Security.DES.Decrypt(dataRow["AccountNo"].ToString());
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
        /// 获取商家外送费
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="id">商家Id</param>
        /// <returns></returns>
        public BusinessInfo GetDistribSubsidy(int id)
        {
            string querSql = @"
select  isnull(DistribSubsidy,0) as DistribSubsidy from  Business (nolock) 
where Id=@Id";

            IDbParameters dbParameters = DbHelper.CreateDbParameters("Id", DbType.Int32, 4, id);

            return DbHelper.QueryForObjectDelegate<BusinessInfo>(SuperMan_Read, querSql, dbParameters,
             dataRow => new BusinessInfo
             {
                 DistribSubsidy = ParseHelper.ToDecimal(dataRow["DistribSubsidy"], 0)

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
         ISNULL(b.RecommendPhone,'') AS RecommendPhone
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


        public bool UpdateBusinessBalancePrice(int businessId, decimal shouldPayBusiMoney)
        {
            bool b = false;
            //更新商户表中的 BalancePrice 余额字段
            StringBuilder upStringBuilder = new StringBuilder(@"
update  dbo.business
set     BalancePrice = BalancePrice + @BalancePrice
where   Id = @Id;");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@BalancePrice", DbType.Decimal).Value = shouldPayBusiMoney;
            parm.Add("@Id", DbType.Int32, 4).Value = businessId;
            int iResult = DbHelper.ExecuteNonQuery(SuperMan_Write, upStringBuilder.ToString(), parm);
            //更新商户流水表
            return iResult > 0 ? true : false;
        }

        /// <summary>
        /// 通过订单ID，用于查询商家信息用
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public BusinessDM GetByOrderId(int orderId)
        {
            string sql = @"
select Name FROM dbo.[order] o(nolock)
join dbo.business b (nolock) on o.businessId = b.Id
 where o.Id = @orderId
            ";
            IDbParameters parm = DbHelper.CreateDbParameters("orderId", DbType.Int32, 4, orderId);
            return DbHelper.QueryForObjectDelegate<BusinessDM>(SuperMan_Read, sql, parm, datarow => new BusinessDM
             {
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

            string remark = model.OptUserName + "通过后台管理系统修改商户信息";
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
                                CommissionType=@CommissionType,
                                OriginalBusiId=@OriginalBusiId,
                                OneKeyPubOrder=@OneKeyPubOrder,
                                IsEmployerTask=@IsEmployerTask,
IsAllowOverdraft=@IsAllowOverdraft,
                                           ";
            if (model.GroupId > 0)
            {
                sql += " GroupId=@GroupId, ";
            }
            if (model.CommissionType == 1)
            {
                sql += " BusinessCommission=@BusinessCommission ";
            }
            else
            {
                sql += " CommissionFixValue=@CommissionFixValue ";
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
            parm.AddWithValue("@CommissionType", model.CommissionType);
            parm.AddWithValue("@BusinessCommission", model.BusinessCommission);
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
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
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
            if (criteria.AuthorityCityNameListStr != null && !string.IsNullOrEmpty(criteria.AuthorityCityNameListStr.Trim()))
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
    }
}
