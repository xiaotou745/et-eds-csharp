using System;
using System.Collections.Generic;
using ETS.Dao;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using ETS.Data.Core;
using ETS.Const;
using ETS.Data.PageData;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.ParameterModel.Clienter;
using System.Data;
using ETS.Extension;
using ETS.Enums;
using Ets.Model.ParameterModel.Finance;
using Ets.Model.ParameterModel.WtihdrawRecords;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Order;
using System.Text;
using Ets.Model.DomainModel.Business;
using Ets.Dao.Order;
using System.Text.RegularExpressions;
using ETS.IO;
using System.IO;
using ETS.Util;
using ETS.Data.Generic;
using Ets.Model.DataModel.Finance;
using System.Linq;
namespace Ets.Dao.Clienter
{

    /// <summary>
    /// 超人数据层
    /// </summary>
    public class ClienterDao : DaoBase
    {
        /// <summary>
        /// 骑士上下班功能   add by caoheyang 20150311
        /// </summary>
        /// <param name="paraModel">参数实体</param>
        public virtual int ChangeWorkStatusToSql(Ets.Model.ParameterModel.Clienter.ChangeWorkStatusPM paraModel)
        {
            const string updateSql = @"UPDATE dbo.clienter SET WorkStatus =@WorkStatus  WHERE id=@id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("id", paraModel.Id);    //超人id
            dbParameters.AddWithValue("WorkStatus", paraModel.WorkStatus);  //目标超人工作状态
            object executeScalar = DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
            int a = ParseHelper.ToInt(executeScalar, 0);
            return a;
        }


        /// <summary>
        /// 骑士上下班功能   add by caoheyang 20150311
        /// </summary>
        /// <param name="paraModel">参数实体</param>
        public virtual int QueryOrderount(Ets.Model.ParameterModel.Clienter.ChangeWorkStatusPM paraModel)
        {
            const string querySql = @"select count(1) from dbo.[order](nolock)  WHERE clienterId=@clienterId and (Status=@Status2 or Status=@Status4)";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@clienterId", paraModel.Id);    //超人id
            dbParameters.AddWithValue("@Status2", OrdersStatus.Status2);  //取货中
            dbParameters.AddWithValue("@Status4", OrdersStatus.Status4);  //送货中
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, querySql, dbParameters);
            int a = ParseHelper.ToInt(executeScalar, 0);
            return a;
        }



        /// <summary>
        /// 获取我的任务   根据状态判断是已完成任务还是我的任务
        /// </summary>
        /// <param name="paraModel">查询条件实体</param>
        public virtual PageInfo<ClientOrderModel> GetMyOrders(ClientOrderSearchCriteria criteria)
        {
            #region where语句拼接
            string where = " 1=1 ";
            if (criteria.userId != 0)
            {
                where += " and o.clienterId=" + criteria.userId;
            }
            if (criteria.status != null && criteria.status.Value != -1)
            {
                if (criteria.status.Value == OrderQueryType.Success.GetHashCode())
                {
                    where += " and o.IsEnable=1 and o.[Status]= " + criteria.status.Value;
                }
                if (criteria.status.Value == OrderQueryType.Working.GetHashCode())//此处查询的未进行中的订单（已接单，已取货）
                {
                    where += " and o.IsEnable=1 and o.[Status] in (2,4)";
                }
            }
            else
            {
                where += " and o.[Status] in (2,4) ";
            }
            #endregion

            string columnStr = @"   o.clienterId AS UserId,
                                    o.OrderNo,
                                    o.Id OrderId,o.OrderFrom,
                                    ISNULL(o.OriginalOrderNo,'') OriginalOrderNo,
                                    CONVERT(VARCHAR(5),o.PubDate,108) AS PubDate,
                                    o.PickUpAddress,
                                    o.ReceviceName,
                                    o.ReceviceCity,
                                    o.ReceviceAddress,
                                    o.RecevicePhoneNo,
                                    o.IsPay,
                                    o.Remark,
                                    o.Status,
                                    o.ReceviceLongitude,
                                    o.ReceviceLatitude,
                                    --补贴
                                    o.CommissionRate,
                                    o.OrderCount,
                                    o.DistribSubsidy,
                                    o.WebsiteSubsidy,
                                    o.Amount,
                                    --补贴
                                    o.BusinessId,
                                    b.Name AS BusinessName,
                                    b.PhoneNo AS BusinessPhone,
                                    b.PhoneNo2 AS BusinessPhone2,
                                    REPLACE(b.City,'市','') AS pickUpCity,
                                    b.Longitude,
                                    b.Latitude,o.OrderCommission,
                                    b.GroupId,ISNULL(oo.HadUploadCount,0) HadUploadCount ";
            string sql_from = @" [order](NOLOCK) AS o LEFT JOIN business(NOLOCK) AS b ON o.businessId=b.Id  
                                 join dbo.OrderOther oo (nolock) on o.Id = oo.OrderId ";
            return new PageHelper().GetPages<ClientOrderModel>(SuperMan_Read, criteria.PagingRequest.PageIndex, where, criteria.status == 1 ? "o.ActualDoneDate DESC " : " oo.GrabTime ", columnStr, sql_from, criteria.PagingRequest.PageSize, false);
        }

        /// <summary>
        /// c端用户登录
        /// 窦海超
        /// 2015年3月17日 15:11:46
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ClienterLoginResultModel PostLogin_CSql(LoginCPM loginModel)
        {

            ClienterLoginResultModel model = null;
            const string querysql = @"select 
        c.WorkStatus,
        c.Id as userId ,
        c.phoneNo ,
        c.[status] ,
        c.AccountBalance as Amount ,
        c.city ,
        c.cityId ,
        c.IsBind ,
        ISNULL(d.Id,0) as DeliveryCompanyId,
        isnull(d.DeliveryCompanyName,'') DeliveryCompanyName,
        --isnull(d.IsDisplay,1) IsDisplay,
        c.Appkey,
        (case when d.SettleType=1 and ClienterSettleRatio>0 or d.SettleType=2 and d.ClienterFixMoney>0 then 1 else 0 end) IsDisplay
from    dbo.clienter c(nolock)
 left join dbo.DeliveryCompany d(nolock) on c.DeliveryCompanyId = d.Id
where   c.PhoneNo = @PhoneNo
        and c.[Password] = @Password";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("PhoneNo", loginModel.phoneNo);
            dbParameters.AddWithValue("@Password", loginModel.passWord);

            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = DataTableHelper.ConvertDataTableList<ClienterLoginResultModel>(dt)[0];
            }
            return model;
        }

        /// <summary>
        /// 获取当天
        /// 商家总数：
        /// 认证骑士数量：
        /// 等待认证骑士：
        /// 窦海超
        /// 2015年3月18日 17:23:14
        /// </summary>
        public HomeCountTitleModel GetCountAndMoney(HomeCountTitleModel model)
        {
            string sql = @"SELECT 
                        ISNULL(SUM(CASE WHEN [Status]=1 THEN 1 ELSE 0 END),0) AS RzqsCount, --认证骑士数量
                        ISNULL(SUM(CASE WHEN [Status]=0 THEN 1 ELSE 0 END),0) AS DdrzqsCount --等待认证骑士
                         FROM dbo.clienter(NOLOCK)--认证骑士数量,等待认证骑士";
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            if (dt == null && dt.Rows.Count <= 0)
            {
                return model;
            }
            return MapRows<HomeCountTitleModel>(dt)[0];
        }

        /// <summary>
        /// 获取当前用户的信息（注意此处有事物用到必须用写串）
        /// 窦海超
        /// 2015年3月20日 16:55:11
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public ClienterModel GetUserInfoByUserId(int UserId)
        {
            string sql = "SELECT ID,TrueName,PhoneNo,AccountBalance,AllowWithdrawPrice,Status,IDCard  FROM dbo.clienter(NOLOCK) WHERE Id=" + UserId;
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Write, sql);
            IList<ClienterModel> list = MapRows<ClienterModel>(dt);
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            return list[0];
        }

        /// <summary>
        /// 根据电话获取当前用户的信息
        /// 窦海超
        /// 2015年3月20日 16:55:11
        /// </summary>
        /// <param name="phoneNo">用户手机号</param>
        /// <returns></returns>
        public ClienterModel GetUserInfoByUserPhoneNo(string phoneNo)
        {
            string sql = "SELECT DeliveryCompanyId,Status,Id,TrueName,PhoneNo,AccountBalance,IDCard,[Password],DeliveryCompanyId,AllowWithdrawPrice FROM dbo.clienter(NOLOCK) WHERE PhoneNo=@PhoneNo";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@PhoneNo", SqlDbType.NVarChar);
            parm.SetValue("@PhoneNo", phoneNo);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            IList<ClienterModel> list = MapRows<ClienterModel>(dt);
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            return list[0];
        }

     
        /// <summary>
        /// 根据用户ID更新密码
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <param name="UserPwd">新密码</param>
        /// <returns></returns>
        public bool UpdateClienterPwdSql(int UserId, string UserPwd)
        {
            if (UserId <= 0)
            {
                return false;
            }
            string sql = "UPDATE dbo.clienter SET [Password]=@Password WHERE id=" + UserId;
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Password", UserPwd);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }
        /// <summary>
        /// 检查 骑士 手机号 是否注册过 
        /// wc
        /// </summary>
        /// <param name="PhoneNo"></param>
        /// <returns></returns>
        public bool CheckClienterExistPhone(string PhoneNo)
        {
            try
            {
                string sql = "SELECT COUNT(1) FROM dbo.clienter(NOLOCK) WHERE PhoneNo =@PhoneNo";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.Add("@PhoneNo", SqlDbType.NVarChar);
                parm.SetValue("@PhoneNo", PhoneNo);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm)) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                //LogHelper.LogWriter(ex, "检查当前骑士是否存在");
                return false;
                throw ex;
            }
        }
        /// <summary>
        /// 骑士注册判断推荐人手机号是否正确 2015年7月6日14:51:38 茹化肖
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
        /// 判断该骑士是否有资格 
        /// wc
        /// </summary>
        /// <param name="clienterId"></param>
        /// <returns></returns>
        public bool HaveQualification(int clienterId)
        {
            try
            {
                //状态为1 表示该骑士 已通过审核
                string sql = "SELECT COUNT(1) FROM dbo.clienter(NOLOCK) WHERE [Status] = 1 AND Id = @clienterId ";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.AddWithValue("@clienterId", clienterId);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm)) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                //LogHelper.LogWriter(ex, "检查当前骑士是否存在");
                return false;
                throw ex;
            }
        }
        /// <summary>
        /// 根据骑士Id判断骑士是否存在
        /// danny-20150530
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool CheckClienterExistById(int Id)
        {
            try
            {
                string sql = "SELECT COUNT(1) FROM clienter(NOLOCK) WHERE Id =@Id ";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.AddWithValue("@Id", Id);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm)) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                //LogHelper.LogWriter(ex, "根据骑士Id判断骑士是否存在");
                return false;
                throw ex;
            }
        }

        /// <summary>
        /// 更新骑士照片信息
        /// danny-10150330
        /// </summary>
        /// <param name="clienter"></param>
        public bool UpdateClientPicInfo(ClienterModel clienter)
        {
            bool reslut = false;
            try
            {
                string sql = @" update clienter set PicUrl=@PicUrl,PicWithHandUrl=@PicWithHandUrl,TrueName=@TrueName,IDCard=@IDCard,Status=@Status where Id=@Id ";
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("@PicUrl", clienter.PicUrl);
                dbParameters.AddWithValue("@PicWithHandUrl", clienter.PicWithHandUrl);
                dbParameters.AddWithValue("@TrueName", clienter.TrueName);
                dbParameters.AddWithValue("@IDCard", clienter.IDCard);
                dbParameters.AddWithValue("@Status", ClienteStatus.Status3.GetHashCode());
                dbParameters.AddWithValue("@Id", clienter.Id);
                int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
                if (i > 0)
                {
                    reslut = true;
                }
            }
            catch (Exception ex)
            {
                reslut = false;
                //LogHelper.LogWriter(ex, "更新骑士照片信息");
                throw ex;
            }
            return reslut;
        }

        /// <summary>
        /// 注册超人
        /// </summary>
        /// <param name="model"></param>
        /// <param name="timespan">时间戳</param>
        /// <returns></returns>
        public int AddClienter(clienter model,string timespan)
        {
            string sql = @"INSERT INTO clienter
                           ([PhoneNo]
                           ,[recommendPhone]
                           ,[Password]
                           ,[Status]
                           ,[InsertTime]
                           ,[InviteCode]
                           ,[City]
                           ,[CityId]
                           ,[GroupId]
                           ,[DeliveryCompanyId]
                           ,[Timespan]
                           ,[Appkey])
                     VALUES
                       (@PhoneNo,@recommendPhone,@Password,@Status,@InsertTime,@InviteCode,@City,@CityId,@GroupId,@DeliveryCompanyId,@Timespan,@Appkey );
                    select SCOPE_IDENTITY() as id;";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@PhoneNo", SqlDbType.NVarChar);
            parm.SetValue("@PhoneNo", model.PhoneNo);
            parm.AddWithValue("@recommendPhone", model.recommendPhone);
            parm.AddWithValue("@Password", model.Password);
            parm.AddWithValue("@Status", model.Status);
            parm.AddWithValue("@InsertTime", model.InsertTime);
            parm.AddWithValue("@InviteCode", model.InviteCode);
            parm.AddWithValue("@City", model.City);
            parm.AddWithValue("@CityId", model.CityId);
            parm.AddWithValue("@GroupId", model.GroupId);
            parm.AddWithValue("@DeliveryCompanyId", (object)model.DeliveryCompanyId);
            parm.AddWithValue("@Timespan", timespan);
            parm.AddWithValue("@Appkey", model.Appkey);
            object i = DbHelper.ExecuteScalar(SuperMan_Write, sql, parm);
            if (i != null)
            {
                return ParseHelper.ToInt(i.ToString());
            }
            return 0;

        }

        /// <summary>
        /// 骑士是否已注册
        /// 彭宜  20150716
        /// </summary>
        /// <param name="model"></param>
        /// <returns>是否注册</returns>
        public bool IsExist(ClientRegisterInfoModel model)
        {
            bool isExist;

            const string querysql = @"
select  count(1) 
from  dbo.[clienter]  
where Timespan=@Timespan and phoneNo=@phoneNo;";
            IDbParameters dbSelectParameters = DbHelper.CreateDbParameters();
            dbSelectParameters.Add("phoneNo", DbType.String,20).Value=model.phoneNo;
            dbSelectParameters.Add("Timespan", DbType.String, 50).Value = model.timespan;
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Write, querysql, dbSelectParameters);
            isExist = ParseHelper.ToInt(executeScalar, 0) > 0;

            return isExist;
        }


        /// <summary>
        /// 抢单
        /// wc添加抢单的时增加日志
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool RushOrder(int userId, string orderNo)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(@"update [order] set clienterId=@clienterId,Status=@Status 
output Inserted.Id,GETDATE(),'{0}','{1}',Inserted.clienterId,Inserted.[Status],{2}
into dbo.OrderSubsidiesLog(OrderId,InsertTime,OptName,Remark,OptId,OrderStatus,[Platform])
where OrderNo=@OrderNo and [Status]=0", SuperPlatform.FromClienter, OrderConst.OrderHadRush, (int)SuperPlatform.FromClienter);//未抢订单才更新
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@clienterId", userId);
            parm.AddWithValue("@Status", OrderStatus.Status2.GetHashCode());
            parm.Add("@OrderNo", SqlDbType.NVarChar);
            parm.SetValue("@OrderNo", orderNo);

            return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Read, sql.ToString(), parm)) > 0;

        }

        /// <summary>
        /// 获取附近任务 / 最新
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<order> GetOrdersNoLogin(ClientOrderSearchCriteria criteria)
        {
            string where = " 1=1 ";
            if (criteria.status != null && criteria.status.Value != -1)
            {
                where += " and o.[Status]= " + criteria.status.Value;
            }
            string order = " o.PubDate desc ";
            string columnStr = @"   o.clienterId,
                                    o.OrderNo,
                                    o.Quantity,
                                    o.OriginalOrderNo,
                                    CONVERT(VARCHAR(5),o.PubDate,108) AS PubDate,
                                    o.PickUpAddress,
                                    o.ReceviceName,
                                    o.ReceviceCity,
                                    o.ReceviceAddress,
                                    o.RecevicePhoneNo,
                                    o.IsPay,
                                    o.Remark,
                                    o.Status,
                                    o.ReceviceLongitude,
                                    o.ReceviceLatitude,
                                    o.OrderFrom,
                                    o.OriginalOrderId,
                                    o.Weight,
                                    --补贴
                                    o.CommissionRate,
                                    o.OrderCount,
                                    o.DistribSubsidy,
                                    o.WebsiteSubsidy,
                                    o.Amount,
                                    --补贴
                                    o.businessId,
                                    b.Name AS BusinessName,
                                    b.PhoneNo AS BusinessPhone,
                                    REPLACE(b.City,'市','') AS pickUpCity,
                                    b.Longitude as BusiLongitude,
                                    b.Latitude as BusiLatitude,o.OrderCommission";
            return new PageHelper().GetPages<order>(SuperMan_Read, criteria.PagingRequest.PageIndex, where, order, columnStr, "[order](NOLOCK) AS o LEFT JOIN business(NOLOCK) AS b ON o.businessId=b.Id", criteria.PagingRequest.PageSize, false);

        }

        /// <summary>
        /// 获取用户状态
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ClienterStatusModel GetUserStatus(int userId)
        {
            string sql = @"
 select 
        c.WorkStatus,
        c.Id as userid ,
        c.[status] ,
        c.phoneno ,
        c.AccountBalance as amount ,
        c.AllowWithdrawPrice as AllowWithdrawPrice ,
        c.IsBind,
        ISNULL(d.Id,0) as DeliveryCompanyId,
        isnull(d.DeliveryCompanyName,'') DeliveryCompanyName,
        --isnull(d.IsDisplay,1) IsDisplay,
        (case when c.DeliveryCompanyId=0 or (d.SettleType=1 and ClienterSettleRatio>0 or d.SettleType=2 and d.ClienterFixMoney>0) then 1 else 0 end) IsDisplay
from    dbo.clienter c ( nolock )
 left join dbo.DeliveryCompany d ( nolock ) on c.DeliveryCompanyId = d.Id 
where c.Id=@clienterId ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@clienterId", userId);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            IList<ClienterStatusModel> list = MapRows<ClienterStatusModel>(dt);
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            return list[0];
        }
        /// <summary>
        /// 骑士配送统计
        /// danny-20150408
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetClienterDistributionStatisticalInfo<T>(OrderSearchCriteria criteria)
        {
            var sbtbl = new StringBuilder(@" (SELECT PubDate ,
                                                    ISNULL(SUM(CASE when a.BusinessCount=1 THEN ClienterCount END),0) OnceCount,
                                                    ISNULL(SUM(CASE when a.BusinessCount=2 THEN ClienterCount END),0) TwiceCount,
                                                    ISNULL(SUM(CASE when a.BusinessCount=3 THEN ClienterCount END),0) ThreeTimesCount,
                                                    ISNULL(SUM(CASE when a.BusinessCount=4 THEN ClienterCount END),0) FourTimesCount,
                                                    ISNULL(SUM(CASE when a.BusinessCount=5 THEN ClienterCount END),0) FiveTimesCount,
                                                    ISNULL(SUM(CASE when a.BusinessCount=6 THEN ClienterCount END),0) SixTimesCount,
                                                    ISNULL(SUM(CASE when a.BusinessCount=7 THEN ClienterCount END),0) SevenTimesCount,
                                                    ISNULL(SUM(CASE when a.BusinessCount=8 THEN ClienterCount END),0) EightTimesCount,
                                                    ISNULL(SUM(CASE when a.BusinessCount=9 THEN ClienterCount END),0) NineTimesCount,
                                                    ISNULL(SUM(CASE when a.BusinessCount>=10 THEN ClienterCount END),0) ExceedNineTimesCount
                                                FROM (
                                                    select t.PubDate,t.businessCount BusinessCount,count(t.clienterId) as ClienterCount
                                                    FROM (
                                                            select convert(char(10),o.PubDate,120) PubDate,count(DISTINCT businessId)businessCount , clienterId
                                                            from [order] o(nolock)
                                                            where Status=1 ");
            if (!string.IsNullOrWhiteSpace(criteria.orderPubStart))
            {
                sbtbl.AppendFormat(" AND  CONVERT(CHAR(10),PubDate,120)>=CONVERT(CHAR(10),'{0}',120) ", criteria.orderPubStart);
            }
            if (!string.IsNullOrWhiteSpace(criteria.orderPubEnd))
            {
                sbtbl.AppendFormat(" AND CONVERT(CHAR(10),PubDate,120)<=CONVERT(CHAR(10),'{0}',120) ", criteria.orderPubEnd);
            }
            sbtbl.Append(@"     group by convert(char(10),o.PubDate,120), clienterId) t
                            group by t.PubDate, t.businessCount )a
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
            				        ,tbl.ExceedNineTimesCount  ";

            var sbSqlWhere = new StringBuilder(" 1=1 ");
            string tableList = sbtbl.ToString();
            string orderByColumn = " tbl.PubDate DESC ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PagingRequest.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PagingRequest.PageSize, true);
        }

        /// <summary>
        /// 骑士门店抢单统计
        /// danny-20150408
        /// </summary>
        /// <returns></returns>
        public IList<BusinessesDistributionModel> GetClienteStorerGrabStatisticalInfo()
        {
            string strSql = @"
                        ;with t as(
                        select temp.[date],temp.businessCount,count(temp.clienterId) cCount
                        from 
                        (
	                       select convert(char(10), PubDate, 120) 'date', o.clienterId,count(distinct o.businessId) 'businessCount'
                            from dbo.[order] o(nolock)
                            join dbo.GlobalConfig gc(nolock) on o.BusinessGroupId=gc.GroupId
                            where o.PubDate>getdate()-20 and o.Status =1 and gc.KeyName='IsStartOverStoreSubsidies' and gc.Value=1
                            group by convert(char(10), PubDate, 120), o.clienterId
                        ) as temp
                        group by temp.[date],temp.businessCount
                        )
                        ,t2 as (
                        select convert(char(10), csl.InsertTime-1, 120) date,csl.BusinessCount 'businessCount',
	                        count(distinct csl.ClienterId) clientorCount,sum(csl.Amount) totalAmount
                        from dbo.CrossShopLog csl(nolock)
                        where csl.InsertTime-1 > getdate()-20
                        group by convert(char(10), csl.InsertTime-1, 120), csl.BusinessCount
                        )

                        select temp2.[date],sum(temp2.amount) totalAmount,max(case temp2.businessCount when 1 then temp2.cCount else 0 end) c1,
	                        max(case temp2.businessCount when 1 then temp2.amount else 0 end) a1,
	                        max(case temp2.businessCount when 2 then temp2.cCount else 0 end) c2,
	                        max(case temp2.businessCount when 2 then temp2.amount else 0 end) a2,
	                        max(case temp2.businessCount when 3 then temp2.cCount else 0 end) c3,
	                        max(case temp2.businessCount when 3 then temp2.amount else 0 end) a3,
	                        max(case temp2.businessCount when 4 then temp2.cCount else 0 end) c4,
	                        max(case temp2.businessCount when 4 then temp2.amount else 0 end) a4,
	                        max(case temp2.businessCount when 5 then temp2.cCount else 0 end) c5,
	                        max(case temp2.businessCount when 5 then temp2.amount else 0 end) a5,
	                        max(case temp2.businessCount when 6 then temp2.cCount else 0 end) c6,
	                        max(case temp2.businessCount when 6 then temp2.amount else 0 end) a6,
	                        max(case temp2.businessCount when 7 then temp2.cCount else 0 end) c7,
	                        max(case temp2.businessCount when 7 then temp2.amount else 0 end) a7,
	                        max(case temp2.businessCount when 8 then temp2.cCount else 0 end) c8,
	                        max(case temp2.businessCount when 8 then temp2.amount else 0 end) a8,
	                        max(case temp2.businessCount when 9 then temp2.cCount else 0 end) c9,
	                        max(case temp2.businessCount when 9 then temp2.amount else 0 end) a9,
	                        sum(case when temp2.businessCount>9 then temp2.cCount else 0 end) c10,
	                        sum(case when temp2.businessCount>9 then temp2.amount else 0 end) a10
                        from 
                        (
                        select t.[date],t.businessCount,t.cCount,isnull(t2.totalAmount,0) amount
                        from t t
	                        left join t2 t2 on t.[date] = t2.date and t.businessCount=t2.businessCount
                        ) as temp2
                        group by temp2.[date]
                        order by temp2.[date] desc
                            ";
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, strSql);
            return MapRows<BusinessesDistributionModel>(dt);
        }

        public IList<BusinessesDistributionModel> GetClienteStorerGrabStatisticalInfo(int daysAgo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("with t as(");
            sb.Append(" select temp.[date],temp.businessCount,count(temp.clienterId) cCount");
            sb.Append(" from (");
            sb.Append(" select convert(char(10), PubDate, 120) 'date', o.clienterId,count(distinct o.businessId) 'businessCount'  from dbo.[order] o(nolock)");
            sb.Append(" where o.PubDate>getdate()-" + daysAgo);
            sb.Append("  and o.Status =1 and  convert(char(10), PubDate, 120)!=convert(char(10), getdate(), 120)  group by convert(char(10), PubDate, 120), o.clienterId");
            sb.Append(" ) as temp   group by temp.[date],temp.businessCount  )");
            sb.Append(" ,t2 as (");
            sb.Append("  select convert(char(10), csl.Rewardtime, 120) date,csl.BusinessCount 'businessCount',    count(distinct csl.ClienterId) clientorCount,sum(csl.Amount) totalAmount  from dbo.CrossShopLog csl(nolock)");
            sb.Append(" where csl.Rewardtime > getdate()-" + daysAgo);
            sb.Append("  group by convert(char(10), csl.Rewardtime, 120), csl.BusinessCount   )");
            sb.Append(" select temp2.[date],sum(temp2.amount) totalAmount,max(case temp2.businessCount when 1 then temp2.cCount else 0 end) c1,");
            sb.Append(" max(case temp2.businessCount when 1 then temp2.amount else 0 end) a1,");
            sb.Append(" max(case temp2.businessCount when 2 then temp2.cCount else 0 end) c2,");
            sb.Append(" max(case temp2.businessCount when 2 then temp2.amount else 0 end) a2,");
            sb.Append(" max(case temp2.businessCount when 3 then temp2.cCount else 0 end) c3,");
            sb.Append(" max(case temp2.businessCount when 3 then temp2.amount else 0 end) a3,");
            sb.Append(" max(case temp2.businessCount when 4 then temp2.cCount else 0 end) c4,");
            sb.Append(" max(case temp2.businessCount when 4 then temp2.amount else 0 end) a4,");
            sb.Append(" max(case temp2.businessCount when 5 then temp2.cCount else 0 end) c5,");
            sb.Append(" max(case temp2.businessCount when 5 then temp2.amount else 0 end) a5,");
            sb.Append(" max(case temp2.businessCount when 6 then temp2.cCount else 0 end) c6,");
            sb.Append(" max(case temp2.businessCount when 6 then temp2.amount else 0 end) a6,");
            sb.Append(" max(case temp2.businessCount when 7 then temp2.cCount else 0 end) c7,");
            sb.Append(" max(case temp2.businessCount when 7 then temp2.amount else 0 end) a7,");
            sb.Append(" max(case temp2.businessCount when 8 then temp2.cCount else 0 end) c8,");
            sb.Append(" max(case temp2.businessCount when 8 then temp2.amount else 0 end) a8,");
            sb.Append(" max(case temp2.businessCount when 9 then temp2.cCount else 0 end) c9,");
            sb.Append(" max(case temp2.businessCount when 9 then temp2.amount else 0 end) a9,");
            sb.Append(" sum(case when temp2.businessCount>9 then temp2.cCount else 0 end) c10,");
            sb.Append(" sum(case when temp2.businessCount>9 then temp2.amount else 0 end) a10");
            sb.Append(" from (");
            sb.Append(" select t.[date],t.businessCount,t.cCount,isnull(t2.totalAmount,0) amount   from t t left join t2 t2 on t.[date] = t2.date and t.businessCount=t2.businessCount");
            sb.Append("  ) as temp2");
            sb.Append(" group by temp2.[date]   order by temp2.[date] ");

            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sb.ToString());
            return MapRows<BusinessesDistributionModel>(dt);
        }


        /// <summary>
        /// 骑士门店抢单统计
        /// danny-20150408
        /// </summary>
        /// <returns></returns>
        public IList<BusinessesDistributionModelOld> GetClienteStorerGrabStatisticalInfoOld(int NewCount)
        {
            string sql = @" SELECT  PubDate ,
                                    ISNULL(SUM(CASE when a.BusinessCount=1 THEN ClienterCount END),0) OnceCount,
                                    ISNULL(SUM(CASE when a.BusinessCount=2 THEN ClienterCount END),0) TwiceCount,
                                    ISNULL(SUM(CASE when a.BusinessCount=3 THEN ClienterCount END),0) ThreeTimesCount,
                                    ISNULL(SUM(CASE when a.BusinessCount=4 THEN ClienterCount END),0) FourTimesCount,
                                    ISNULL(SUM(CASE when a.BusinessCount=5 THEN ClienterCount END),0) FiveTimesCount,
                                    ISNULL(SUM(CASE when a.BusinessCount=6 THEN ClienterCount END),0) SixTimesCount,
                                    ISNULL(SUM(CASE when a.BusinessCount=7 THEN ClienterCount END),0) SevenTimesCount,
                                    ISNULL(SUM(CASE when a.BusinessCount=8 THEN ClienterCount END),0) EightTimesCount,
                                    ISNULL(SUM(CASE when a.BusinessCount=9 THEN ClienterCount END),0) NineTimesCount,
                                    ISNULL(SUM(CASE when a.BusinessCount>=10 THEN ClienterCount END),0) ExceedTenTimesCount
                            FROM (select PubDate,businessCount BusinessCount,count(clienterId) as ClienterCount
                                  from (select convert(char(10),o.PubDate,120) PubDate,clienterId,count(distinct businessId) businessCount
                                        from dbo.[order] o(nolock)
                                            where o.PubDate> getdate()-20 and Status =1 group by convert(char(10),o.PubDate,120), clienterId)t group by PubDate, businessCount) a group by PubDate order by PubDate desc ;";
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            return MapRows<BusinessesDistributionModelOld>(dt);
        }
        /// <summary>
        /// 上传小票
        /// </summary>
        /// <param name="uploadReceiptModel"></param>
        /// <returns></returns>
        public OrderOther UpdateClientReceiptPicInfo(UploadReceiptModel uploadReceiptModel)
        {
            var oo = GetReceiptInfo(uploadReceiptModel.OrderId);

            var orderOther = UpdateReceiptInfo(uploadReceiptModel);

            if (orderOther == null)
            {
                return null;
            }
            orderOther.OrderStatus = oo.OrderStatus;
            orderOther.OrderCreateTime = oo.OrderCreateTime;
            return orderOther;
        }

        /// <summary>
        /// 增加一条小票信息
        /// wc
        /// </summary>
        /// <param name="uploadReceiptModel"></param>
        /// <returns></returns>
        public OrderOther InsertReceiptInfo(UploadReceiptModel uploadReceiptModel)
        {
            OrderOther oo = new OrderOther();
            ///TODO output?
            StringBuilder sql = new StringBuilder(@"
insert into dbo.OrderOther
    ( OrderId ,
        NeedUploadCount ,
        ReceiptPic ,
        HadUploadCount
    )
output Inserted.Id,Inserted.OrderId,Inserted.NeedUploadCount,Inserted.ReceiptPic,Inserted.HadUploadCount
values ( @OrderId ,
        @NeedUploadCount , 
        @ReceiptPic ,
        @HadUploadCount 
);");
            sql.Append(@"
update  dbo.OrderChild
set     HasUploadTicket = 1,
        TicketUrl = @ReceiptPic
where   OrderId = @OrderId
        and ChildId = @OrderChildId;
");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OrderId", SqlDbType.Int).Value = uploadReceiptModel.OrderId;
            parm.Add("@NeedUploadCount", SqlDbType.Int).Value = uploadReceiptModel.NeedUploadCount;
            parm.Add("@HadUploadCount", SqlDbType.Int).Value = uploadReceiptModel.HadUploadCount;
            parm.Add("@ReceiptPic", SqlDbType.VarChar).Value = uploadReceiptModel.ReceiptPic;
            parm.Add("@OrderChildId", SqlDbType.Int).Value = uploadReceiptModel.OrderChildId;
            try
            {
                DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Write, sql.ToString(), parm);
                var ooList = MapRows<OrderOther>(dt);
                if (ooList != null && ooList.Count == 1)
                {
                    return ooList[0];
                }
            }
            catch (Exception ex)
            {
                //LogHelper.LogWriter("插入orderOther表异常：", new { ex = ex });
                throw ex;
            }
            return oo;
        }

        /// <summary>
        /// 添加小票信息
        /// wc
        /// </summary>
        /// <param name="uploadReceiptModel"></param>
        /// <returns></returns>
        public OrderOther UpdateReceiptInfo(UploadReceiptModel uploadReceiptModel)
        {
            //更新orderOther
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            StringBuilder updateSql = new StringBuilder();
            if (uploadReceiptModel.HadUploadCount == 1)
            {
                updateSql.Append(@" update dbo.OrderOther
 set HadUploadCount = HadUploadCount + @HadUploadCount
where  OrderId=@OrderId;");
                dbParameters.Add("@HadUploadCount", DbType.Int32, 4).Value = uploadReceiptModel.HadUploadCount;
            }
            updateSql.Append(@"
update  dbo.OrderChild
set     HasUploadTicket = 1, TicketUrl = @ReceiptPic
where    OrderId = @OrderId  and ChildId = @OrderChildId; ");
            dbParameters.Add("@OrderId", DbType.Int32, 4).Value = uploadReceiptModel.OrderId;
            dbParameters.Add("@ReceiptPic", DbType.String, 256).Value = uploadReceiptModel.ReceiptPic;

            dbParameters.Add("@OrderChildId", DbType.Int32, 4).Value = uploadReceiptModel.OrderChildId;
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql.ToString(), dbParameters);

            //返回OrderOther实体
            const string querysql = @"
select Id ,OrderId,NeedUploadCount,ReceiptPic,HadUploadCount
from OrderOther (nolock)
where  OrderId=@OrderId ";
            IDbParameters dbParametersQuerysql = DbHelper.CreateDbParameters();
            dbParametersQuerysql.Add("@OrderId", DbType.Int32, 4).Value = uploadReceiptModel.OrderId;
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Write, querysql, dbParametersQuerysql);
            if (!dt.HasData())
            {
                return null;
            }
            var ooList = MapRows<OrderOther>(dt);
            return ooList[0]; 
        }
        /// <summary>
        /// 删除小票信息OrderChild表
        /// wc
        /// </summary>
        /// <param name="uploadReceiptModel"></param>
        /// <returns></returns>
        public OrderOther DeleteReceiptInfo(UploadReceiptModel uploadReceiptModel)
        {
            OrderOther oo = new OrderOther();

            StringBuilder sql = new StringBuilder(@"
update  dbo.OrderChild
set     HasUploadTicket = 0 ,
        TicketUrl = ''
where   OrderId = @OrderId
        and ChildId = @OrderChildId;
 update dbo.OrderOther
 set    ReceiptPic = @ReceiptPic ,
        HadUploadCount = HadUploadCount + @HadUploadCount
 output Inserted.Id ,
        Inserted.OrderId ,
        Inserted.NeedUploadCount ,
        Inserted.ReceiptPic ,
        Inserted.HadUploadCount
 where  OrderId = @OrderId;
");
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OrderId", SqlDbType.Int).Value = uploadReceiptModel.OrderId;
            parm.Add("@HadUploadCount", SqlDbType.Int).Value = uploadReceiptModel.HadUploadCount;
            parm.Add("@ReceiptPic", SqlDbType.VarChar).Value = uploadReceiptModel.ReceiptPic;
            parm.Add("@OrderChildId", SqlDbType.Int).Value = uploadReceiptModel.OrderChildId;
            try
            {
                DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Write, sql.ToString(), parm);
                var ooList = MapRows<OrderOther>(dt);
                if (ooList != null)
                {
                    return ooList[0];
                }
                else
                {
                    oo.Id = -1;
                }
            }
            catch (Exception ex)
            {
                //LogHelper.LogWriter("删除更新orderOther表异常：", new { ex = ex });
                oo.Id = -1;
                throw ex;
            }
            return oo;
        }

        /// <summary>
        /// 根据订单号获取该订单的小票信息
        /// wc
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>OrderOther</returns>
        public OrderOther GetReceiptInfo(int orderId)
        {
            string sql = @"select  o.Id OrderId ,o.IsPay,
        oo.Id ,
        o.SettleMoney,
        o.[Status] OrderStatus,
        o.OrderCount NeedUploadCount,
        oo.ReceiptPic ,
        o.PubDate OrderCreateTime,
        ISNULL(oo.HadUploadCount, 0) HadUploadCount
from    dbo.[order] o ( nolock )
        join dbo.OrderOther oo ( nolock ) on o.Id = oo.OrderId
where   o.Id = @OrderId";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OrderId", DbType.Int32, 4).Value = orderId;
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Write, sql, parm);
            if (!dt.HasData())
            {
                return null;
            }
            var ooList = MapRows<OrderOther>(dt);
            return ooList[0];
        }
        /// <summary>
        /// 删除小票信息
        /// wc
        /// </summary>
        /// <param name="uploadReceiptModel"></param>
        /// <returns></returns>
        public OrderOther DeleteReceipt(UploadReceiptModel uploadReceiptModel)
        {
            string delPic = uploadReceiptModel.ReceiptPic;
            //更新小票信息
            OrderOther oo = GetReceiptInfo(uploadReceiptModel.OrderId);
            if (oo.Id > 0)
            {
                List<string> listReceiptPic = ImageCommon.GetListImgString(oo.ReceiptPic);
                int delPre = listReceiptPic.Count;
                int delAft = 0;
                Regex regex = new Regex(@"(/\d{4}/\d{2}/\d{2}.*?)\.jpg", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                MatchCollection matchCollection = regex.Matches(delPic);
                string delPicDir = "1.jpg";
                foreach (Match match in matchCollection)
                {
                    delPicDir = match.Value;
                    listReceiptPic.Remove(delPicDir);
                }
                delAft = listReceiptPic.Count;
                if (delPre - delAft == 0)
                {
                    uploadReceiptModel.HadUploadCount = 0;
                }
                string ppath = ConfigSettings.Instance.FileUploadPath + "\\" + ConfigSettings.Instance.FileUploadFolderNameCustomerIcon;
                var delDir = ppath + delPicDir;
                var fileName = Path.GetFileName(delDir);
                int fileNameLastDot = fileName.LastIndexOf('.');
                //原图 
                string orginalFileName = string.Format("{0}{1}{2}", Path.GetDirectoryName(delDir) + "\\" + fileName.Substring(0, fileNameLastDot), SystemConst.OriginSize, Path.GetExtension(fileName));

                uploadReceiptModel.ReceiptPic = String.Join("|", listReceiptPic.ToArray());

                OrderOther ooo = DeleteReceiptInfo(uploadReceiptModel);
                if (oo != null)
                {
                    //删除磁盘中的裁图
                    FileHelper.DeleteFile(delDir);
                    //删除缩略图
                    FileHelper.DeleteFile(orginalFileName);
                    return ooo;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据ID获取对象
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public clienter GetById(int id)
        {
            clienter model = null;
            const string querysql = @"
select  Id,PhoneNo,LoginName,recommendPhone,Password,TrueName,IDCard,PicWithHandUrl,PicUrl,[Status],AccountBalance,InsertTime,InviteCode,City,CityId,GroupId,HealthCardID,InternalDepart,ProvinceCode,AreaCode,CityCode,Province,BussinessID,WorkStatus,AllowWithdrawPrice,HasWithdrawPrice
from  clienter (nolock)
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                model = MapRows<clienter>(dt)[0];
            }
            return model;
        }
        /// <summary>
        /// 获得骑士电话   add by 彭宜    20150717
        /// </summary>
        /// <param name="id">骑士id</param>
        /// <returns>电话号码</returns>
        public string GetPhoneNo(int id)
        {
            string querSql = @"
select  PhoneNo
            from clienter (nolock) 
where Id=@Id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters("Id", DbType.Int32, 4, id);
            return Convert.ToString(DbHelper.ExecuteScalar(SuperMan_Read, querSql, dbParameters));
        }
        public IDictionary<int, clienter> GetByIds(IList<int> ids)
        {
            const string querysql = @"
select  Id,PhoneNo,LoginName,recommendPhone,Password,TrueName,IDCard,PicWithHandUrl,PicUrl,[Status],AccountBalance,InsertTime,InviteCode,City,CityId,GroupId,HealthCardID,InternalDepart,ProvinceCode,AreaCode,CityCode,Province,BussinessID,WorkStatus,AllowWithdrawPrice,HasWithdrawPrice
from  clienter (nolock)
where  Id IN({0}) ";

            if (ids == null || ids.Count == 0)
            {
                return new Dictionary<int, clienter>();
            }
            return DbHelper.QueryWithRowMapperDelegate<clienter>(SuperMan_Read, string.Format(querysql, string.Join(",", ids)), datarow => new clienter()
             {
                 Id = ParseHelper.ToInt(datarow["Id"]),
                 TrueName = ParseHelper.ToString(datarow["TrueName"]),
                 PhoneNo = ParseHelper.ToString(datarow["PhoneNo"])

             }).ToDictionary(m => m.Id);
        }

        /// <summary>
        ///  骑士更新 余额，可提现余额 功能 add by caoheyang 20150509
        /// </summary>
        /// <param name="model">骑士信息</param>
        /// <returns></returns>
        public void UpdateForWithdrawC(UpdateForWithdrawPM model)
        {
            const string updateSql = @"
update  clienter
set  AccountBalance=AccountBalance+@WithdrawPrice,AllowWithdrawPrice=AllowWithdrawPrice+@WithdrawPrice
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", model.Id);
            dbParameters.AddWithValue("WithdrawPrice", model.Money);
            var  num=DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }


        /// <summary>
        /// 获取骑士详情    
        /// 
        /// 修改
        /// 2015年8月13日 10:07:52
        /// 窦海超 
        /// 把获取用户信息里的是否有未读站内信写在获取用户的SQL里了
        /// 另外物流公司如果比例或固定金额大于0返回显示列表物流公司金额 
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150512</UpdateTime>
        /// <param name="id">骑士Id</param>
        /// <returns></returns>
        public ClienterDM GetDetails(int id)
        {
            ClienterDM clienterDM = new ClienterDM();

            #region 骑士表
//            string queryClienterSql = @"
//select  Id,PhoneNo,LoginName,recommendPhone,Password,TrueName,IDCard,PicWithHandUrl,PicUrl,Status,
//AccountBalance,InsertTime,InviteCode,City,CityId,GroupId,HealthCardID,InternalDepart,ProvinceCode
//,AreaCode,CityCode,Province,BussinessID,WorkStatus,AllowWithdrawPrice,HasWithdrawPrice
//from  clienter (nolock) 
//where Id=@Id";
            string queryClienterSql = @"
select  c.Id,PhoneNo,LoginName,recommendPhone,Password,TrueName,IDCard,PicWithHandUrl,PicUrl,Status,
AccountBalance,InsertTime,InviteCode,City,CityId,GroupId,HealthCardID,InternalDepart,ProvinceCode
,AreaCode,CityCode,Province,BussinessID,WorkStatus,AllowWithdrawPrice,HasWithdrawPrice,
(case when (select count(1) from dbo.ClienterMessage cm(nolock) where cm.ClienterId=c.id and cm.IsRead=0)=0 then 0 else 1 end) HasMessage,
--(case when dc.SettleType=1 and ClienterSettleRatio>0 or dc.SettleType=2 and dc.ClienterFixMoney>0 then 1 else 0 end) IsDisplayDeliveryMoney
isnull(dc.IsShowAccount,1) IsShowAccount
from  dbo.clienter c (nolock) 
left join dbo.DeliveryCompany dc(nolock) on c.DeliveryCompanyId=dc.Id
where c.Id=@Id";

            IDbParameters dbClienterParameters = DbHelper.CreateDbParameters("Id", DbType.Int32, 4, id);
            clienterDM = DbHelper.QueryForObject(SuperMan_Read, queryClienterSql, dbClienterParameters, new ClienterRowMapper());
            #endregion

            #region 骑士金融账号表
            const string queryCFAccountSql = @"
select  Id,ClienterId,TrueName,AccountNo,IsEnable,AccountType,BelongType,OpenBank,OpenSubBank,CreateBy,CreateTime,UpdateBy,UpdateTime,OpenProvince,OpenCity,IDCard 
from  ClienterFinanceAccount (nolock) 
where ClienterId=@ClienterId  and IsEnable=1";

            IDbParameters dbCFAccountParameters = DbHelper.CreateDbParameters();
            dbCFAccountParameters.AddWithValue("ClienterId", id);
            DataTable dtBFAccount = DbHelper.ExecuteDataTable(SuperMan_Read, queryCFAccountSql, dbCFAccountParameters);
            List<ClienterFinanceAccount> listCFAccount = new List<ClienterFinanceAccount>();
            foreach (DataRow dataRow in dtBFAccount.Rows)
            {
                ClienterFinanceAccount bf = new ClienterFinanceAccount();
                bf.Id = ParseHelper.ToInt(dataRow["Id"]);
                bf.ClienterId = ParseHelper.ToInt(dataRow["ClienterId"]);
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
                if (dataRow["OpenProvince"] != null && dataRow["OpenProvince"] != DBNull.Value)
                {
                    bf.OpenProvince = dataRow["OpenProvince"].ToString();
                }
                if (dataRow["OpenCity"] != null && dataRow["OpenCity"] != DBNull.Value)
                {
                    bf.OpenCity = dataRow["OpenCity"].ToString();
                }
                if (dataRow["IDCard"] != null && dataRow["IDCard"] != DBNull.Value)
                {
                    bf.IDCard = dataRow["IDCard"].ToString();
                }
                bf.CreateBy = dataRow["CreateBy"].ToString();
                bf.CreateTime = ParseHelper.ToDatetime(dataRow["CreateTime"]);
                bf.UpdateBy = dataRow["UpdateBy"].ToString();
                bf.UpdateTime = ParseHelper.ToDatetime(dataRow["UpdateTime"]);
                listCFAccount.Add(bf);
            }
            clienterDM.listcFAcount = listCFAccount;
            #endregion

            return clienterDM;
        }

        /// <summary>
        /// 判断骑士是否存在       
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="id">骑士Id</param>
        /// <returns></returns>
        public bool IsExist(int id)
        {
            bool isExist;
            string querySql = @"
select count(1)
from   dbo.[clienter] (nolock) 
where  id = @id";

            IDbParameters dbParameters = DbHelper.CreateDbParameters("Id", DbType.Int32, 4, id);
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, querySql, dbParameters);
            isExist = ParseHelper.ToInt(executeScalar, 0) > 0;

            return isExist;
        }
        /// <summary>
        /// 获取骑士详细信息
        /// danny-20150513
        /// </summary>
        /// <param name="clienterId">骑士Id</param>
        /// <returns></returns>
        public ClienterDetailModel GetClienterDetailById(string clienterId)
        {
            string selSql = @" 
SELECT   c.[Id],
         c.[PhoneNo],
         c.[LoginName],
         c.[recommendPhone],
         c.[Password],
         c.[TrueName],
         c.[IDCard],
         c.[PicWithHandUrl],
         c.[PicUrl],
         c.[Status],
         c.[AccountBalance],
         c.[InsertTime],
         c.[InviteCode],
         c.[City],
         c.[CityId],
         c.[GroupId],
         c.[HealthCardID],
         c.[InternalDepart],
         c.[ProvinceCode],
         c.[AreaCode],
         c.[CityCode],
         c.[Province],
         c.[BussinessID],
         c.[WorkStatus], 
         c.DeliveryCompanyId,
         cfa.TrueName AccountName,
         cfa.AccountNo,
         cfa.AccountType,
         cfa.OpenBank,
         cfa.OpenSubBank
FROM clienter c WITH(NOLOCK) 
	Left join ClienterFinanceAccount cfa WITH(NOLOCK) ON c.Id=cfa.ClienterId AND cfa.IsEnable=1
WHERE c.Id = @ClienterId  ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@ClienterId", clienterId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, selSql, parm));
            if (dt != null && dt.Rows.Count > 0)
                return DataTableHelper.ConvertDataTableList<ClienterDetailModel>(dt)[0];
            return null;
        }

        #region  Nested type: ClienterRowMapper

        /// <summary>
        /// 绑定对象
        /// </summary>
        private class ClienterRowMapper : IDataTableRowMapper<ClienterDM>
        {
            public ClienterDM MapRow(DataRow dataReader)
            {
                var result = new ClienterDM();
                object obj;
                obj = dataReader["Id"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.Id = int.Parse(obj.ToString());
                }
                result.PhoneNo = dataReader["PhoneNo"].ToString();
                result.LoginName = dataReader["LoginName"].ToString();
                result.recommendPhone = dataReader["recommendPhone"].ToString();
                result.Password = dataReader["Password"].ToString();
                result.TrueName = dataReader["TrueName"].ToString();
                result.IDCard = dataReader["IDCard"].ToString();
                if (dataReader["PicWithHandUrl"] != null && dataReader["PicWithHandUrl"].ToString() != "")
                    result.PicWithHandUrl = Ets.Model.Common.ImageCommon.GetUserImage(dataReader["PicWithHandUrl"].ToString(), ImageType.Clienter);
                result.PicUrl = dataReader["PicUrl"].ToString();
                obj = dataReader["Status"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.Status = int.Parse(obj.ToString());
                }
                obj = dataReader["AccountBalance"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.AccountBalance = decimal.Parse(obj.ToString());
                }
                obj = dataReader["InsertTime"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.InsertTime = DateTime.Parse(obj.ToString());
                }
                result.InviteCode = dataReader["InviteCode"].ToString();
                result.City = dataReader["City"].ToString();
                result.CityId = dataReader["CityId"].ToString();
                obj = dataReader["GroupId"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.GroupId = int.Parse(obj.ToString());
                }
                result.HealthCardID = dataReader["HealthCardID"].ToString();
                result.InternalDepart = dataReader["InternalDepart"].ToString();
                result.ProvinceCode = dataReader["ProvinceCode"].ToString();
                result.AreaCode = dataReader["AreaCode"].ToString();
                result.CityCode = dataReader["CityCode"].ToString();
                result.Province = dataReader["Province"].ToString();
                //result.IsDisplayDeliveryMoney = ParseHelper.ToInt(dataReader["IsDisplayDeliveryMoney"].ToString());
                obj = dataReader["BussinessID"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.BussinessID = int.Parse(obj.ToString());
                }
                obj = dataReader["WorkStatus"];
                if (obj != null && obj != DBNull.Value)
                {
                    result.WorkStatus = int.Parse(obj.ToString());
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
        /// 获取骑士用户名
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetName(string phoneNo)
        {
            string name = "";

            string querySql = @"
select TrueName
from   dbo.[clienter] (nolock) 
where  phoneNo = @phoneNo ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@phoneNo", phoneNo);

            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, querySql, dbParameters);
            if (executeScalar != null)
                name = executeScalar.ToString();

            return name;
        }

        /// <summary>
        /// 获取骑士Id
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150609</UpdateTime>
        /// <param name="phoneNo"></param>
        /// <param name="trueName"></param>
        /// <returns></returns>
        public int GetId(string phoneNo, string trueName)
        {
            string querySql = @"
select Id
from   dbo.[clienter] (nolock) 
where  phoneNo = @phoneNo and TrueName=@TrueName ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@phoneNo", phoneNo);
            dbParameters.AddWithValue("@TrueName", trueName);

            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, querySql, dbParameters);
            return ParseHelper.ToInt(executeScalar, 0);

        }
        /// <summary>
        /// 获取骑士列表
        /// danny-20150608
        /// </summary>
        /// <param name="model"></param>
        public IList<ClienterListModel> GetClienterList(ClienterListModel model)
        {
            string sql = @"SELECT  [Id]
                                  ,[PhoneNo]
                                  ,[LoginName]
                                  ,[recommendPhone]
                                  ,[Password]
                                  ,[TrueName]
                                  ,[IDCard]
                                  ,[PicWithHandUrl]
                                  ,[PicUrl]
                                  ,[Status]
                                  ,[AccountBalance]
                                  ,[InsertTime]
                                  ,[InviteCode]
                                  ,[City]
                                  ,[CityId]
                                  ,[GroupId]
                                  ,[HealthCardID]
                                  ,[InternalDepart]
                                  ,[ProvinceCode]
                                  ,[AreaCode]
                                  ,[CityCode]
                                  ,[Province]
                                  ,[BussinessID]
                                  ,[WorkStatus]
                              FROM clienter WITH (NOLOCK) 
                              WHERE 1=1 AND Status = 1";
            if (!string.IsNullOrWhiteSpace(model.TrueName))
            {
                sql += " AND TrueName LIKE '%@TrueName%' ";
            }
            if (!string.IsNullOrWhiteSpace(model.TrueName))
            {
                sql += " AND PhoneNo=@TrueName ";
            }
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@TrueName", model.TrueName);
            parm.AddWithValue("@PhoneNo", model.PhoneNo);
            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
            var list = ConvertDataTableList<ClienterListModel>(dt);
            return list;
        }

        /// <summary>
        /// 查询骑士列表
        /// danny-20150609
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetClienterList<T>(ClienterSearchCriteria criteria)
        {
            string columnList = @"   C.[Id]
                                    ,C.[PhoneNo]
                                    ,C.[TrueName]
                                    ,ISNULL(bcr.IsBind,0) IsBind 
                                    ";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(criteria.clienterName))
            {
                sbSqlWhere.AppendFormat(" AND C.TrueName LIKE '%{0}%' ", criteria.clienterName);
            }
            if (!string.IsNullOrEmpty(criteria.clienterPhone))
            {
                sbSqlWhere.AppendFormat(" AND C.PhoneNo='{0}' ", criteria.clienterPhone);
            }
            string tableList = string.Format(@" clienter C WITH (NOLOCK)  
                                  left JOIN BusinessClienterRelation bcr with(nolock) on  C.Id=bcr.ClienterId and bcr.BusinessId={0} and bcr.IsEnable=1 and bcr.IsBind=1
                                ", criteria.businessId);
            string orderByColumn = " C.Id DESC";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }

        /// <summary>
        /// 获取骑士电话号码
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150617</UpdateTime>
        /// <param name="sentStatus"></param>
        /// <returns></returns>
        public DataTable GetPhoneNoList(string pushCity)
        {
            string querysql = @"  
select id, PhoneNo from dbo.clienter(nolock) 
where  cityid in(" + pushCity + ")";

            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, querysql);
            return dt;
        }
        /// <summary>
        /// 查询一个骑士绑定的商家中，是否有任何一个开启了IsEmployerTask
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool IsSettingOnlyShowBussinessTask(int UserId)
        {
            string querysql = @"
                              SELECT    SUM(b.IsEmployerTask) AS IsSettingOnlyShowBussinessTask
                              FROM      BusinessClienterRelation a ( NOLOCK )
                                        JOIN business b ( NOLOCK ) ON a.BusinessId = b.Id
                              WHERE     a.ClienterId = @ClienterId
                                        AND a.IsBind = 1
                                        AND a.IsEnable = 1
                                        AND b.IsBind = 1
                              ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@ClienterId", UserId);
            object obj = DbHelper.ExecuteScalar(SuperMan_Read, querysql, dbParameters);
            return ParseHelper.ToLong(obj) > 0;
        }

        /// <summary>
        /// 判断提款单对应的商家或骑士是否有效
        /// </summary>
        /// <param name="drawType">0是商户提款单，1是骑士提款单</param>
        /// <param name="withDrawID">提款单id</param>
        /// <returns></returns>
        public bool IsBussinessOrClienterValidByID(int drawType, long withDrawID)
        {

            string querysql = @"
                             SELECT  a.BusinessId
                                FROM    BusinessWithdrawForm a
                                        INNER JOIN business b ON a.BusinessId = b.id
                                WHERE   a.id = @id
                                        AND b.Status = 1
                              ";

            if (drawType > 0)
            {
                querysql = @"
                             SELECT  a.ClienterId
                                FROM    ClienterWithdrawForm a
                                        INNER JOIN Clienter b ON a.ClienterId = b.id
                                WHERE   a.id = @id
                                        AND b.Status = 1
                              ";
            }


            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@id", withDrawID);
            object obj = DbHelper.ExecuteScalar(SuperMan_Read, querysql, dbParameters);
            return ParseHelper.ToLong(obj) > 0;
        }

        /// <summary>
        /// 根据骑士id为骑士绑定物流公司  add by caoheyang  20150707
        /// </summary>
        /// <param name="clienterId">骑士id</param>
        /// <param name="deliveryCompanyId">物流公司id</param>
        /// <param name="optId">操作人id</param>
        /// <param name="optName">操作人名</param>
        /// <returns></returns>
        public int BindDeliveryCompany(int clienterId, int deliveryCompanyId, int optId, string optName)
        {
            const string sql = @"
update dbo.clienter set DeliveryCompanyId=@DeliveryCompanyId 
output Inserted.Id,@OptId,@OptName,'配送公司批量导入骑士,更新已有骑士所属物流公司信息'
into dbo.ClienterOptionLog(ClienterId,OptId,OptName,Remark) 
where Id=@Id ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("DeliveryCompanyId", DbType.Int32, 4).Value = deliveryCompanyId;
            dbParameters.Add("Id", DbType.Int32, 4).Value = clienterId;
            dbParameters.Add("OptId", DbType.Int32, 4).Value = optId;
            dbParameters.Add("OptName", DbType.String, 50).Value = optName;
            return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters));
        }


        /// <summary>
        /// 物流公司新增骑士功能  add by caoheyang  20150707
        /// </summary>
        /// <param name="model">骑士实体</param>
        /// <param name="optId">操作人id</param>
        /// <param name="optName">操作人名</param>
        /// <returns></returns>
        public int DeliveryCompanyInsertClienter(clienter model,int optId, string optName)
        {
            const string insertSql = @"
insert into clienter(PhoneNo,Password,TrueName,IDCard,
Status,InsertTime,City,CityId,
CityCode,DeliveryCompanyId)

output Inserted.Id,@OptId,@OptName,'配送公司批量导入骑士'
into dbo.ClienterOptionLog(ClienterId,OptId,OptName,Remark)

values(@PhoneNo,@Password,@TrueName,@IDCard,
@Status,getdate(),@City,@CityId,
@CityCode,@DeliveryCompanyId)

SELECT IDENT_CURRENT('clienter')"
;

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("OptId", optId);
            dbParameters.AddWithValue("OptName", optName);
            dbParameters.AddWithValue("PhoneNo", model.PhoneNo);
            dbParameters.AddWithValue("Password", model.Password);
            dbParameters.AddWithValue("TrueName", model.TrueName);
            dbParameters.AddWithValue("IDCard", model.IDCard);
            dbParameters.AddWithValue("Status", model.Status);
            dbParameters.AddWithValue("City", model.City);
            dbParameters.AddWithValue("CityId", model.CityId);
            dbParameters.AddWithValue("CityCode", model.CityCode);
            dbParameters.AddWithValue("DeliveryCompanyId", model.DeliveryCompanyId);
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters));
        }
		/// <summary>
        /// 修改骑士详细信息
        /// danny-20150707
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyClienterDetail(ClienterDetailModel model)
        {
            var parm = DbHelper.CreateDbParameters();
            //string remark = model.OptUserName + "通过后台管理系统修改骑士信息";
            string sql = @"UPDATE clienter 
                            SET IDCard=@IDCard,
                                TrueName=@TrueName,
                                DeliveryCompanyId=@DeliveryCompanyId,recommendPhone=@recommendPhone ";
		     
            sql += @" OUTPUT
                        Inserted.Id,
                        @OptId,
                        @OptName,
                        GETDATE(),
                        @Platform,
                        '身份证号' + isnull(Deleted.IDCard, '') + '修改为' + isnull(Inserted.IDCard,'') 
                        + ';姓名'  + isnull(Deleted.TrueName, '') + '修改为' + isnull(Inserted.TrueName, '')
                        + ';物流公司' + isnull(convert(nvarchar(10), Deleted.DeliveryCompanyId),'') + '修改为' + convert(nvarchar(100), Inserted.DeliveryCompanyId) 
                        + ';推荐人手机号' + isnull(convert(nvarchar(11), Deleted.recommendPhone), '') + '修改为' + isnull(Inserted.recommendPhone, '') 
                    INTO ClienterOptionLog
                        (ClienterId,
                        OptId,
                        OptName,
                        InsertTime,
                        Platform,
                        Remark)
                        WHERE  Id = @Id;";
           
            parm.AddWithValue("@IDCard", model.IDCard);
            parm.AddWithValue("@TrueName", model.TrueName);
            parm.AddWithValue("@DeliveryCompanyId", model.DeliveryCompanyId);
            parm.AddWithValue("@Id", model.Id);
            parm.AddWithValue("@OptId", model.OptUserId);
            parm.AddWithValue("@OptName", model.OptUserName);
            parm.AddWithValue("@Platform", 3);
            parm.Add("@recommendPhone", DbType.String).Value = model.recommendPhone??"";
            //parm.AddWithValue("@Remark", model.Remark);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }

        /// <summary>
        /// 获取骑士图片   add by pengyi 20150709  仅限工具使用
        /// </summary>
        public IList<ClienterPicModel> GetClienterPics()
        {
            var sql = @"select c.Id,c.PicUrl,c.PicWithHandUrl from [clienter](nolock) as c where c.PicUrl is not null or c.PicWithHandUrl is not null";
            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql));
            var list = ConvertDataTableList<ClienterPicModel>(dt);
            return list;
        }

        #region 更新骑士余额 可提现
        /// <summary>
        /// 更新骑士余额
        /// 胡灵波        
        /// 2015年8月12日 18:10:58
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCAccountBalance(UpdateForWithdrawPM model)
        {
            const string updateSql = @"
update dbo.clienter set AccountBalance=AccountBalance + @Money
where id=@Id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("Id", DbType.Int32, 4).Value = model.Id;
            dbParameters.AddWithValue("@Money", model.Money);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters) > 0 ? true : false;
        }

        /// <summary>
        /// 更新骑士可提现金额
        /// 胡灵波
        /// 2015年8月12日 18:11:26
        /// </summary>
        /// <param name="model"></param>
        public void UpdateCAllowWithdrawPrice(UpdateForWithdrawPM model)
        {
            const string updateSql = @"
update dbo.clienter 
set AllowWithdrawPrice=AllowWithdrawPrice+@Money
where Id=@Id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("Id", DbType.Int32, 4).Value = model.Id;
            dbParameters.AddWithValue("@Money", model.Money);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }

        /// <summary>
        ///  更新骑士余额和可提现金额
        ///  胡灵波
        ///  2015年8月12日 18:12:17
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void UpdateCBalanceAndWithdraw(UpdateForWithdrawPM model)
        {
            const string updateSql = @"
update  clienter
set  AccountBalance=AccountBalance+@Money,
     AllowWithdrawPrice=AllowWithdrawPrice+@Money
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("Id", DbType.Int32, 4).Value = model.Id;
            dbParameters.AddWithValue("@Money", model.Money);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
        }
    
        /// <summary>
        /// 更新用户余额
        /// 窦海超
        /// 2015年3月23日 12:47:54
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <param name="Account">提现金额</param>
        /// <returns></returns>
        public bool UpdateClienterAccountBalance(WithdrawRecordsModel model)
        {
            Ets.Model.DomainModel.Clienter.ClienterModel cliterModel = new ClienterDao().GetUserInfoByUserId(model.UserId);//获取当前用户余额
            decimal balance = ParseHelper.ToDecimal(cliterModel.AccountBalance, 0);
            decimal Money = balance + model.Amount;
            if (Money < 0)//如果提现金额大于当前余额则不能提现
            {
                return false;
            }
            model.Balance = balance;
            string sql = @"UPDATE dbo.clienter SET AccountBalance=@Money WHERE id=" + model.UserId;
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Money", Money);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }        

        /// <summary>
        /// 更新用户余额以及可提现金额
        /// 窦海超
        /// 2015年3月23日 12:47:54
        /// </summary>
        /// <returns></returns>
        public bool UpdateAccountBalanceAndWithdraw(WithdrawRecordsModel model)
        {
            ClienterModel cliterModel = GetUserInfoByUserId(model.UserId);//获取当前用户余额
            decimal balance = ParseHelper.ToDecimal(cliterModel.AccountBalance, 0);
            decimal Money = balance + model.Amount;
            if (Money < 0)//如果提现金额大于当前余额则不能提现
            {
                return false;
            }
            model.Balance = balance;
            string sql = @"update dbo.clienter set AccountBalance = @Money , AllowWithdrawPrice=AllowWithdrawPrice+@AllowWithdrawPrice WHERE id=" + model.UserId;
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Money", Money);
            parm.Add("AllowWithdrawPrice", DbType.Decimal, 18).Value = model.Amount;
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }

        #endregion
    }
}
