using System;
using System.Collections.Generic;
using ETS.Dao;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DataModel.Order;
using ETS.Data.Core;
using ETS.Util;
using ETS.Const;
using ETS.Data.PageData;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.ParameterModel.Clienter;
using System.Data;
using ETS.Extension;
using ETS.Enums;
using Ets.Model.ParameterModel.WtihdrawRecords;
using Ets.Model.Common;

namespace Ets.Dao.Clienter
{

    /// <summary>
    /// 超人数据层
    /// </summary>
    public class ClienterDao : DaoBase
    {
        //using (var dbEntity = new supermanEntities())
        //  {
        //      var query = dbEntity.order.AsQueryable();
        //      if (!string.IsNullOrWhiteSpace(criteria.city))
        //          query = query.Where(i => i.business.City == criteria.city.Trim());
        //      if (!string.IsNullOrWhiteSpace(criteria.cityId))
        //          query = query.Where(i => i.business.CityId == criteria.cityId.Trim());
        //      query = query.Where(i => i.Status.Value == ConstValues.ORDER_NEW);0
        //      query = query.OrderByDescending(i => i.PubDate);
        //      var result = query.ToList();
        //      return result;
        //  }
        public List<order> GetOrdersNoLoginLatest(ClientOrderSearchCriteria criteria)
        {
            string sql = "";
            return null;
        }


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
                                    b.Latitude,o.OrderCommission";
            return new PageHelper().GetPages<ClientOrderModel>(SuperMan_Read, criteria.PagingRequest.PageIndex, where, "o.Id", columnStr, "[order](NOLOCK) AS o LEFT JOIN business(NOLOCK) AS b ON o.businessId=b.Id", criteria.PagingRequest.PageSize, false);
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
            parm.AddWithValue("@PhoneNo", model.phoneNo);
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
            if (list == null && list.Count <= 0)
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
            parm.AddWithValue("@PhoneNo", PhoneNo);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            IList<ClienterModel> list = MapRows<ClienterModel>(dt);
            if (list == null && list.Count <= 0)
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
                parm.AddWithValue("@PhoneNo", PhoneNo);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm)) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "检查当前骑士是否存在");
                return false;
                throw;
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
                LogHelper.LogWriter(ex, "检查当前骑士是否存在");
                return false;
                throw;
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
                LogHelper.LogWriter(ex, "根据骑士Id判断骑士是否存在");
                return false;
                throw;
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
                LogHelper.LogWriter(ex, "更新骑士照片信息");
                throw;
            }
            return reslut;
        }

        /// <summary>
        /// 检查号码是否存在-平扬 2015.3.30
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public bool CheckExistPhone(string phoneNo)
        {
            try
            {
                string sql = "SELECT 1 FROM dbo.clienter(NOLOCK) WHERE PhoneNo=@PhoneNo ";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.AddWithValue("@PhoneNo", phoneNo);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm)) > 0;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "检查号码是否存在");
                return false;
            }
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
            parm.AddWithValue("@PhoneNo", model.PhoneNo);
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
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool RushOrder(int userId, string orderNo)
        {   
            string sql = @" update [order] set clienterId=@clienterId,Status=@Status where OrderNo=@OrderNo and [Status]=0 ";//未抢订单才更新
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@clienterId", userId);
            parm.AddWithValue("@Status",ConstValues.ORDER_ACCEPT);
            parm.AddWithValue("@OrderNo", orderNo);  
            return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Read, sql, parm)) > 0;

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

    }
}
