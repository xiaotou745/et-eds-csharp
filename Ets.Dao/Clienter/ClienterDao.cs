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
using Ets.Model.ParameterModel.WtihdrawRecords;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Order;
using System.Text;
using Ets.Model.DomainModel.Bussiness;
using Ets.Dao.Order;
using System.Text.RegularExpressions;
using ETS.IO;
using System.IO;
using ETS.Util;

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
            const string querySql = @"select count(id) from dbo.[order](nolock)  WHERE clienterId=@clienterId and Status=@Status";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("clienterId", paraModel.Id);    //超人id
            dbParameters.AddWithValue("Status", paraModel.OrderStatus);  //目标超人工作状态
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Write, querySql, dbParameters);
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
                where += " and o.[Status]= " + criteria.status.Value;
            }
            else
            {
                where += " and o.[Status]= " + OrderConst.ORDER_ACCEPT;
            }
            #endregion

            string columnStr = @"   o.clienterId AS UserId,
                                    o.OrderNo,
                                    o.Id OrderId,
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
                                    REPLACE(b.City,'市','') AS pickUpCity,
                                    b.Longitude,
                                    b.Latitude,o.OrderCommission,
                                    b.GroupId,ISNULL(oo.HadUploadCount,0) HadUploadCount ";
            string sql_from = @" [order](NOLOCK) AS o LEFT JOIN business(NOLOCK) AS b ON o.businessId=b.Id  
                                 left join dbo.OrderOther oo (nolock) on o.Id = oo.OrderId ";
            return new PageHelper().GetPages<ClientOrderModel>(SuperMan_Read, criteria.PagingRequest.PageIndex, where, criteria.status == 1 ? "o.ActualDoneDate DESC " : " o.Id ", columnStr, sql_from, criteria.PagingRequest.PageSize, false);
        }

        /// <summary>
        /// c端用户登录
        /// 窦海超
        /// 2015年3月17日 15:11:46
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ClienterLoginResultModel PostLogin_CSql(LoginModel model)
        {
            string sql = @"SELECT 
                        Id AS userId ,
                        PhoneNo,
                        status,
                        AccountBalance AS Amount,
                        city,
                        cityId 
                        FROM dbo.clienter(NOLOCK) WHERE PhoneNo=@PhoneNo AND [Password]=@Password";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@PhoneNo", SqlDbType.NVarChar);
            parm.SetValue("@PhoneNo", model.phoneNo);
            parm.AddWithValue("@Password", model.passWord);
            DataSet set = DbHelper.ExecuteDataset(SuperMan_Read, sql, parm);

            DataTable dt = DataTableHelper.GetTable(set);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            IList<ClienterLoginResultModel> list = MapRows<ClienterLoginResultModel>(dt);
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            return list[0];
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
        /// 获取当前用户的信息
        /// 窦海超
        /// 2015年3月20日 16:55:11
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public ClienterModel GetUserInfoByUserId(int UserId)
        {
            string sql = "SELECT TrueName,PhoneNo,AccountBalance FROM dbo.clienter(NOLOCK) WHERE Id=" + UserId;
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
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
        /// <param name="PhoneNo">用户手机号</param>
        /// <returns></returns>
        public ClienterModel GetUserInfoByUserPhoneNo(string PhoneNo)
        {
            string sql = "SELECT Id,TrueName,PhoneNo,AccountBalance FROM dbo.clienter(NOLOCK) WHERE PhoneNo=@PhoneNo";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@PhoneNo", SqlDbType.NVarChar);
            parm.SetValue("@PhoneNo", PhoneNo);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            IList<ClienterModel> list = MapRows<ClienterModel>(dt);
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            return list[0];
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
                dbParameters.AddWithValue("@Status", ConstValues.CLIENTER_AUDITPASSING);
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
        /// <returns></returns>
        public int AddClienter(clienter model)
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
                           ,[GroupId])
                     VALUES
                       (@PhoneNo,@recommendPhone,@Password,@Status,@InsertTime,@InviteCode,@City,@CityId,@GroupId );select SCOPE_IDENTITY() as id;";
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
            object i = DbHelper.ExecuteScalar(SuperMan_Write, sql, parm);
            if (i != null)
            {
                return ParseHelper.ToInt(i.ToString());
            }
            return 0;

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
output Inserted.Id,GETDATE(),'{0}','',Inserted.clienterId,Inserted.[Status],{1}
into dbo.OrderSubsidiesLog(OrderId,InsertTime,OptName,Remark,OptId,OrderStatus,[Platform])
where OrderNo=@OrderNo and [Status]=0", SuperPlatform.骑士, (int)SuperPlatform.骑士);//未抢订单才更新
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@clienterId", userId);
            parm.AddWithValue("@Status", ConstValues.ORDER_ACCEPT);
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
            string sql = @" select id as userid,status,phoneno,AccountBalance as amount from dbo.clienter with(nolock) where Id=@clienterId ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@clienterId", userId);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            IList<ClienterStatusModel> list = MapRows<ClienterStatusModel>(dt);
            if (list == null && list.Count <= 0)
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
                                        where o.PubDate> getdate()-20
                                            and Status in (1,2)
                                        group by convert(char(10),o.PubDate,120), clienterId)t
                                  group by PubDate, businessCount) a
                            group by PubDate
                            order by PubDate desc ;";
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            return MapRows<BusinessesDistributionModel>(dt);
        }

        /// <summary>
        /// 上传小票
        /// </summary>
        /// <param name="uploadReceiptModel"></param>
        /// <returns></returns>
        public OrderOther UpdateClientReceiptPicInfo(UploadReceiptModel uploadReceiptModel)
        {
            OrderOther orderOther = new OrderOther();
            int orderStatus = 0;
            var oo = GetReceiptInfo(uploadReceiptModel.OrderId, out orderStatus);
            if (oo.Id == 0)
            {
                orderOther = InsertReceiptInfo(uploadReceiptModel);
            }
            else
            {
                orderOther = UpdateReceiptInfo(uploadReceiptModel);
            }
            orderOther.OrderStatus = orderStatus;
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
            string sql = @"
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
                            );";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OrderId", SqlDbType.Int);
            parm.SetValue("@OrderId", uploadReceiptModel.OrderId);
            parm.AddWithValue("@NeedUploadCount", uploadReceiptModel.NeedUploadCount);
            parm.AddWithValue("@ReceiptPic", uploadReceiptModel.ReceiptPic);
            parm.AddWithValue("@HadUploadCount", uploadReceiptModel.HadUploadCount);

            try
            {
                DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Write, sql, parm);
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
            OrderOther oo = new OrderOther();

            string sql = @"
  update dbo.OrderOther
 set    ReceiptPic = ReceiptPic + '|' + @ReceiptPic ,
        HadUploadCount = HadUploadCount + @HadUploadCount,
        NeedUploadCount = @NeedUploadCount
 output Inserted.Id ,
        Inserted.OrderId ,
        Inserted.NeedUploadCount ,
        Inserted.ReceiptPic ,
        Inserted.HadUploadCount
 where  OrderId = @OrderId;";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OrderId", SqlDbType.Int);
            parm.SetValue("@OrderId", uploadReceiptModel.OrderId);
            parm.AddWithValue("@NeedUploadCount", uploadReceiptModel.NeedUploadCount);
            parm.AddWithValue("@HadUploadCount", uploadReceiptModel.HadUploadCount);
            parm.AddWithValue("@ReceiptPic", uploadReceiptModel.ReceiptPic);
            try
            {
                DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Write, sql, parm);
                var ooList = MapRows<OrderOther>(dt);
                if (ooList != null && ooList.Count == 1)
                {
                    return ooList[0];
                }
            }
            catch (Exception ex)
            {
                //LogHelper.LogWriter("更新orderOther表异常：", new { ex = ex });
                throw ex;
            }
            return oo;
        }
        /// <summary>
        /// 删除小票信息
        /// wc
        /// </summary>
        /// <param name="uploadReceiptModel"></param>
        /// <returns></returns>
        public OrderOther DeleteReceiptInfo(UploadReceiptModel uploadReceiptModel)
        {
            OrderOther oo = new OrderOther();

            string sql = @"
  update dbo.OrderOther
 set    ReceiptPic = @ReceiptPic ,
        HadUploadCount = HadUploadCount + @HadUploadCount
 output Inserted.Id ,
        Inserted.OrderId ,
        Inserted.NeedUploadCount ,
        Inserted.ReceiptPic ,
        Inserted.HadUploadCount
 where  OrderId = @OrderId;";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OrderId", SqlDbType.Int);
            parm.SetValue("@OrderId", uploadReceiptModel.OrderId);
            parm.AddWithValue("@HadUploadCount", uploadReceiptModel.HadUploadCount);
            parm.AddWithValue("@ReceiptPic", uploadReceiptModel.ReceiptPic);
            try
            {
                DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Write, sql, parm);
                var ooList = MapRows<OrderOther>(dt);
                if (ooList != null && ooList.Count == 1)
                {
                    return ooList[0];
                }
            }
            catch (Exception ex)
            {
                //LogHelper.LogWriter("删除更新orderOther表异常：", new { ex = ex });
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
        public OrderOther GetReceiptInfo(int orderId, out int OrderStatus)
        {
            OrderStatus = 0;
            int orderCount = 0;
            string sql = @"select oo.Id ,
        oo.OrderId ,
        oo.NeedUploadCount ,
        oo.ReceiptPic ,
        oo.HadUploadCount
from    dbo.OrderOther oo ( nolock )
where   oo.OrderId = @OrderId;select o.[Status],o.OrderCount FROM dbo.[order] o (nolock)
 where o.Id = @OrderId";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@OrderId", SqlDbType.Int);
            parm.SetValue("@OrderId", orderId);

            DataSet dt = DbHelper.ExecuteDataset(SuperMan_Read, sql, parm);
            var ooList = MapRows<OrderOther>(dt.Tables[0]);
            if (dt.Tables[1] != null && dt.Tables[1].Rows.Count > 0)
            { 
                OrderStatus = ParseHelper.ToInt(dt.Tables[1].Rows[0][0], 0);
                orderCount = ParseHelper.ToInt(dt.Tables[1].Rows[0][1], 0);
            }
            if (ooList != null && ooList.Count> 0)
            {
                ooList[0].NeedUploadCount = orderCount;
                return ooList[0];
            }
            else
            {
                return new OrderOther() { OrderId = orderId, OrderStatus = OrderStatus, NeedUploadCount = orderCount, HadUploadCount = 0, ReceiptPic = "" };
            }
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
            int orderStatus = 0;
            //更新小票信息
            OrderOther oo = GetReceiptInfo(uploadReceiptModel.OrderId, out orderStatus);
            if (oo.Id>0)
            {
                List<string> listReceiptPic = ImageCommon.GetListImgString(oo.ReceiptPic);

                Regex regex = new Regex(@"(/\d{4}/\d{2}/\d{2}.*?)\.jpg", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.Singleline);
                MatchCollection matchCollection = regex.Matches(delPic);
                string delPicDir = "1.jpg";
                foreach (Match match in matchCollection)
                {
                    delPicDir = match.Value;
                    listReceiptPic.Remove(delPicDir);
                }
                string ppath = ConfigSettings.Instance.FileUploadPath + "\\" + ConfigSettings.Instance.FileUploadFolderNameCustomerIcon;
                var delDir = ppath + delPicDir; 
              
                var fileName = Path.GetFileName(delDir);

                int fileNameLastDot = fileName.LastIndexOf('.'); 
                 
                //原图 
                string orginalFileName = string.Format("{0}{1}{2}", Path.GetDirectoryName(delDir) + "\\" + fileName.Substring(0, fileNameLastDot), ImageConst.OriginSize, Path.GetExtension(fileName));

                //删除磁盘中的裁图
                FileHelper.DeleteFile(delDir);
                //删除缩略图
                FileHelper.DeleteFile(orginalFileName);
                uploadReceiptModel.ReceiptPic = String.Join("|", listReceiptPic.ToArray());

                OrderOther ooo = DeleteReceiptInfo(uploadReceiptModel);
                if (oo != null)
                {
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
    }
}
