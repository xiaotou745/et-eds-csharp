using Ets.Model.Common;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.ParameterModel.Clienter;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
using ETS.Enums;
using ETS.Extension;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.Distribution
{
    public class DistributionDao : DaoBase
    {
        /// <summary>
        /// 骑士列表查询
        /// danny-20150318
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetClienteres<T>(ClienterSearchCriteria criteria)
        {
            string columnList = @"   C.[Id]
                                    ,C.[PhoneNo]
                                    ,C.[LoginName]
                                    ,C.[recommendPhone]
                                    ,C.[Password]
                                    ,C.[TrueName]
                                    ,C.[IDCard]
                                    ,C.[PicWithHandUrl]
                                    ,C.[PicUrl]
                                    ,C.[Status]
                                    ,C.[AccountBalance]
                                    ,C.[InsertTime]
                                    ,C.[InviteCode]
                                    ,C.[City]
                                    ,C.[CityId]
                                    ,C.[GroupId]
                                    ,C.[HealthCardID]
                                    ,C.[InternalDepart]
                                    ,C.[ProvinceCode]
                                    ,C.[AreaCode]
                                    ,C.[CityCode]
                                    ,C.[Province]
                                    ,C.[BussinessID]
                                    ,C.[WorkStatus] 
                                    ,C.[AllowWithdrawPrice] 
                                    ,g.GroupName
                                    ,isnull(cs.ClienterId,0) as CSID  --如果非0就存在跨店
                                    ,ISNULL(DC.DeliveryCompanyName,'') AS CompanyName
";

            var sbSqlWhere = new StringBuilder(" 1=1 ");// AND DC.IsEnable = 1 
            if (!string.IsNullOrEmpty(criteria.clienterPhone))
            {
                sbSqlWhere.AppendFormat(" AND C.PhoneNo='{0}' ", criteria.clienterPhone);
            }
            if (!string.IsNullOrEmpty(criteria.recommonPhone))
            {
                sbSqlWhere.AppendFormat(" AND C.recommendPhone='{0}' ", criteria.recommonPhone.Trim());
            }
            if (criteria.Status != -1)
            {
                sbSqlWhere.AppendFormat(" AND C.Status={0} ", criteria.Status);
            }
            if (criteria.GroupId != null && criteria.GroupId > 0)
            {
                sbSqlWhere.AppendFormat(" AND C.GroupId={0} ", criteria.GroupId);
            }
            if (!string.IsNullOrEmpty(criteria.businessCity))
            {
                sbSqlWhere.AppendFormat(" AND C.City='{0}' ", criteria.businessCity.Trim());
            }
            if (!string.IsNullOrWhiteSpace(criteria.deliveryCompany) && criteria.deliveryCompany != "0")
            {
                sbSqlWhere.AppendFormat(" AND DC.Id={0} ", criteria.deliveryCompany);
            }
            if (!string.IsNullOrEmpty(criteria.AuthorityCityNameListStr) && criteria.UserType != 0)
            {
                sbSqlWhere.AppendFormat(" AND C.City IN ({0}) ", criteria.AuthorityCityNameListStr.Trim());
            }
            if (!string.IsNullOrEmpty(criteria.clienterName))
            {
                sbSqlWhere.AppendFormat(" AND C.TrueName LIKE '%{0}%' ", criteria.clienterName);
            }
            //else
            //{
            //    sbSqlWhere.AppendFormat(" AND C.City IN ({0}) ", criteria.AuthorityCityNameListStr.Trim());
            //}
            //if (!string.IsNullOrEmpty(criteria.txtPubStart))
            //{
            //    sbSqlWhere.AppendFormat(" AND CONVERT(CHAR(10),WR.CreateTime,120)=CONVERT(CHAR(10),'{0}',120) and WtihdrawRecords.Amount < 0", criteria.txtPubStart.Trim());
            //}

            string tableList = @" clienter C WITH (NOLOCK)  
                                  
                                   left JOIN(SELECT UserId,MIN(CreateTime) CreateTime
                                             FROM dbo.WtihdrawRecords  WITH(NOLOCK) 
                                             GROUP BY UserId) WR ON WR.UserId = C.Id 
                                  left Join (SELECT ClienterId 
                                               FROM CrossShopLog csl WITH(NOLOCK) 
                                                GROUP BY csl.ClienterId) cs on c.Id=cs.ClienterId
                                  LEFT JOIN DeliveryCompany DC (NOLOCK) ON c.DeliveryCompanyId=DC.Id
                                  LEFT JOIN dbo.[group] g WITH(NOLOCK) ON g.Id = c.GroupId 
                                  --left JOIN  dbo.WtihdrawRecords ON WtihdrawRecords.UserId = C.Id 
                                  --left join dbo.CrossShopLog cs on c.Id=cs.ClienterId
                                ";
            string orderByColumn = " C.Id DESC";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }
        /// <summary>
        /// 更新审核状态
        /// danny-20150318
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enumStatusType"></param>
        /// <returns></returns>
        public bool UpdateAuditStatus(int id, AuditStatus enumStatusType)
        {
            bool reslut = false;
            try
            {
                string sql = string.Empty;
                if (enumStatusType == AuditStatus.Status1)
                {
                    sql = string.Format(" update clienter set Status={0} where Id=@id ", ClienteStatus.Status1.GetHashCode());
                }
                else if (enumStatusType == AuditStatus.Status0)
                {
                    sql = string.Format(" update clienter set Status={0} where Id=@id ", ClienteStatus.Status0.GetHashCode());
                }
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("id", id);
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
        /// 更新审核状态 
        /// </summary> 
        /// <returns></returns>
        public bool UpdateAuditStatus(ClienterUpdateModel cum)
        {
            bool reslut = false;
            try
            {
                string sql = string.Empty;
                int status = 0;
                if (cum.AuditStatus == AuditStatus.Status1)
                {
                    status = ClienteStatus.Status1.GetHashCode();
                }
                else if (cum.AuditStatus == AuditStatus.Status0)
                {
                    status = ClienteStatus.Status0.GetHashCode();
                }
                sql = string.Format(@"update clienter set Status={0}  
 output Inserted.Id,@optId,@optName,getdate(),3, '骑士状态'+convert(varchar(10),Deleted.[Status])+'修改为'+convert(varchar(10),Inserted.[Status]) 
 into dbo.ClienterOptionLog (ClienterId,OptId,OptName,InsertTime,[Platform],Remark) 
 where Id=@id ", status);
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("id", cum.Id);
                dbParameters.Add("optId", DbType.Int32).Value = cum.OptUserId;
                dbParameters.Add("optName", DbType.String).Value = cum.OptUserName;

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
        /// 清空帐户余额
        /// 加日志
        /// </summary>
        /// <returns></returns>
        public bool ClearSuperManAmount(ClienterUpdateModel cum)
        {
            bool reslut = false;
            try
            {
                string sql = @" update clienter set AccountBalance=0 
output Inserted.Id,@optId,@optName,getdate(),3, '账户余额清零'+convert(varchar(10),Deleted.AccountBalance)+'修改为0'  
 into dbo.ClienterOptionLog (ClienterId,OptId,OptName,InsertTime,[Platform],Remark) 
where Id=@id ";
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("id", cum.Id);
                int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
                if (i > 0) reslut = true;
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriter(ex, "清空帐户余额");
                throw;
            }
            return reslut;
        }

        /// <summary>
        /// 清空帐户余额
        /// danny-20150318
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ClearSuperManAmount(int id)
        {
            bool reslut = false;
            try
            {
                string sql = " update clienter set AccountBalance=0 where Id=@id ";
                IDbParameters dbParameters = DbHelper.CreateDbParameters();
                dbParameters.AddWithValue("id", id);
                int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, dbParameters);
                if (i > 0) reslut = true;
            }
            catch (Exception ex)
            {
                reslut = false;
                LogHelper.LogWriter(ex, "清空帐户余额");
                throw;
            }
            return reslut;
        }


        /// <summary>
        /// 添加骑士
        /// danny-20150318
        /// </summary>
        /// <param name="clienter"></param>
        /// <returns></returns>
        public bool AddClienter(ClienterListModel clienter)
        {
            try
            {
                string sql = @"
                           INSERT INTO clienter
                               ([PhoneNo]
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
                               ,[WorkStatus])
                         VALUES
                               (@PhoneNo
                               ,@LoginName
                               ,@recommendPhone
                               ,@Password
                               ,@TrueName
                               ,@IDCard
                               ,@PicWithHandUrl
                               ,@PicUrl
                               ,@Status
                               ,@AccountBalance
                               ,@InsertTime
                               ,@InviteCode
                               ,@City
                               ,@CityId
                               ,@GroupId
                               ,@HealthCardID
                               ,@InternalDepart
                               ,@ProvinceCode
                               ,@AreaCode
                               ,@CityCode
                               ,@Province
                               ,@BussinessID
                               ,@WorkStatus)SELECT @@IDENTITY
                        ";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.AddWithValue("@PhoneNo", clienter.PhoneNo);
                parm.AddWithValue("@LoginName", clienter.LoginName);
                parm.AddWithValue("@recommendPhone", clienter.recommendPhone);
                parm.AddWithValue("@Password", clienter.Password);
                parm.AddWithValue("@TrueName", clienter.TrueName);
                parm.AddWithValue("@IDCard", clienter.IDCard);
                parm.AddWithValue("@PicWithHandUrl", clienter.PicWithHandUrl);
                parm.AddWithValue("@PicUrl", clienter.PicUrl);
                parm.AddWithValue("@Status", clienter.Status);
                parm.AddWithValue("@AccountBalance", clienter.AccountBalance);
                parm.AddWithValue("@InsertTime", clienter.InsertTime);
                parm.AddWithValue("@InviteCode", clienter.InviteCode);
                parm.AddWithValue("@City", clienter.City);
                parm.AddWithValue("@GroupId", clienter.GroupId);
                parm.AddWithValue("@HealthCardID", clienter.HealthCardID);
                parm.AddWithValue("@InternalDepart", clienter.InternalDepart);
                parm.AddWithValue("@ProvinceCode", clienter.ProvinceCode);
                parm.AddWithValue("@AreaCode", clienter.AreaCode);
                parm.AddWithValue("@CityCode", clienter.CityCode);
                parm.AddWithValue("@Province", clienter.Province);
                parm.AddWithValue("@BussinessID", clienter.BussinessID);
                parm.AddWithValue("@WorkStatus", clienter.WorkStatus);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, sql, parm)) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("添加骑士异常", new { ex = ex, clienter = clienter });
                return false;
                throw;
            }
        }
        /// <summary>
        /// 根据集团id获取超人列表
        /// danny-20150319
        /// </summary>
        /// <param name="groupId">集团id</param>
        /// <returns>IList<ClienterModel></returns>
        public IList<ClienterListModel> GetClienterModelByGroupID(int? groupId)
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
                              FROM clienter WITH (NOLOCK) where 1=1 AND Status = 1";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@GroupId", groupId);
            if (groupId != null && groupId != 0)
            {
                sql += " AND GroupId=@GroupId";
            }
            var dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql, parm));
            var list = ConvertDataTableList<ClienterListModel>(dt);
            return list;
        }

        /// <summary>
        /// 骑士统计
        /// danny-20150326
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetClienteresCount<T>(ClienterSearchCriteria criteria)
        {
            var sbtbl = new StringBuilder(@" (select c.Id,
                                                    COUNT(*) OrderCount,
                                                    c.TrueName Name,
                                                    c.AccountBalance,
                                                    c.PhoneNo
                                                from clienter c with(nolock)
                                                join [order] o on c.Id=o.clienterId
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
            sbtbl.Append(" group  by c.Id,c.TrueName,c.AccountBalance,c.PhoneNo ) tbl ");
            string columnList = @"   tbl.Id
                                    ,tbl.OrderCount
            				        ,tbl.Name
            				        ,tbl.AccountBalance 
                                    ,tbl.PhoneNo ";

            var sbSqlWhere = new StringBuilder(" 1=1 ");

            if (!string.IsNullOrEmpty(criteria.clienterName))
            {
                sbSqlWhere.AppendFormat(" AND Name='{0}' ", criteria.clienterName);
            }
            if (!string.IsNullOrEmpty(criteria.clienterPhone))
            {
                sbSqlWhere.AppendFormat(" AND PhoneNo='{0}' ", criteria.clienterPhone);
            }
            string tableList = sbtbl.ToString();
            string orderByColumn = " tbl.OrderCount DESC ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PagingRequest.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PagingRequest.PageSize, true);
        }
    }
}
