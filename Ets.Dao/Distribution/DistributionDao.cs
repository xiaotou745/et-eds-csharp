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
            string columnList = @"   [Id]
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
                                    ,[WorkStatus] ";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            if (!string.IsNullOrEmpty(criteria.clienterName))
            {
                sbSqlWhere.AppendFormat(" AND TrueName='{0}' ", criteria.clienterName);
            }
            if (!string.IsNullOrEmpty(criteria.clienterPhone))
            {
                sbSqlWhere.AppendFormat(" AND PhoneNo='{0}' ", criteria.clienterPhone);
            }
            if (criteria.Status != -1)
            {
                sbSqlWhere.AppendFormat(" AND Status={0} ", criteria.Status);
            }
            if (criteria.GroupId != null)
            {
                sbSqlWhere.AppendFormat(" AND GroupId={0} ", criteria.GroupId);
            }
            string tableList = @" clienter  WITH (NOLOCK)   ";
            string orderByColumn = " Id DESC";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PagingRequest.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PagingRequest.PageSize, true);
        }
        /// <summary>
        /// 更新审核状态
        /// danny-20150318
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

                    sql = string.Format(" update clienter set Status={0} where Id=@id ", ConstValues.CLIENTER_AUDITPASS);
                }
                else if (enumStatusType == EnumStatusType.审核取消)
                {
                    sql = string.Format(" update clienter set Status={0} where Id=@id ", ConstValues.CLIENTER_AUDITCANCEL);
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
        /// 检查骑士手机是否存在
        /// danny-20150318
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public bool CheckExistPhone(string phoneNo)
        {
            try
            {
                string sql = "SELECT COUNT(*) FROM clienter WITH (NOLOCK) WHERE PhoneNo=@PhoneNo";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.AddWithValue("@PhoneNo", phoneNo);
                return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Read, sql, parm)) > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(ex, "检查号码是否存在");
                return false;
                throw;
            }
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
    }
}
