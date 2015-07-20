using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using ETS.Data.PageData;
using ETS.Extension;
using Ets.Model.Common;
using Ets.Model.DomainModel.Common;
using Ets.Model.ParameterModel.Clienter;
using Ets.Model.ParameterModel.Common;

namespace Ets.Dao.Common
{
    public class AppVersionDao : DaoBase
    {
        /// <summary>
        /// 查询升级信息
        /// </summary>
        /// <param name="vcmodel"></param>
        /// <returns></returns>
        public AppVerionModel VersionCheck(VersionCheckModel vcmodel)
        {
            const string querysql = @"
SELECT TOP 1 [Version],IsMust,UpdateUrl,[Message] FROM dbo.AppVersion 
WHERE [PlatForm]=@PlatForm AND UserType=@UserType and TimingDate>getdate()
ORDER BY ID DESC";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@PlatForm", vcmodel.PlatForm);
            parm.AddWithValue("@UserType", vcmodel.UserType);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, querysql, parm);
            if (!dt.HasData())
            {
                return null;
            }
            return MapRows<AppVerionModel>(dt)[0];
        }
        /// <summary>
        /// 分页查询App版本信息列表
        /// dannny-20150715
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetAppVersionList<T>(AppVerionSearchCriteria criteria)
        {
            string columnList = @"   
       [ID]
      ,[Version]
      ,[IsMust]
      ,[UpdateUrl]
      ,[Message]
      ,[PlatForm]
      ,[UserType]
      ,[CreateDate]
      ,[CreateBy]
      ,[UpdateDate]
      ,[UpdateBy]
      ,[IsTiming]
      ,[TimingDate]
      ,[PubStatus]";
            var sbSqlWhere = new StringBuilder(" 1=1 ");
            string tableList = @" AppVersion WITH (NOLOCK) ";
            string orderByColumn = " Id DESC";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }

        /// <summary>
        /// 添加App版本信息
        /// danny-20150715
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddAppVersion(AppVersionModel model)
        {
            string sql = string.Format(@" 
INSERT INTO [AppVersion]
           ([Version]
           ,[IsMust]
           ,[UpdateUrl]
           ,[Message]
           ,[PlatForm]
           ,[UserType]
           ,[CreateBy]
           ,[UpdateBy]
           ,[IsTiming]
           ,[TimingDate])
     VALUES
           (@Version
           ,@IsMust
           ,@UpdateUrl
           ,@Message
           ,@PlatForm
           ,@UserType
           ,@CreateBy
           ,@UpdateBy
           ,@IsTiming
           ,@TimingDate);");
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Version", model.Version);
            parm.AddWithValue("@IsMust", model.IsMust);
            parm.AddWithValue("@UpdateUrl", model.UpdateUrl);
            parm.AddWithValue("@Message", model.Message);
            parm.AddWithValue("@PlatForm", model.PlatForm);
            parm.AddWithValue("@UserType", model.UserType);
            parm.AddWithValue("@CreateBy", model.OptUserName);
            parm.AddWithValue("@UpdateBy", model.OptUserName);
            parm.AddWithValue("@IsTiming", model.IsTiming);
            parm.AddWithValue("@TimingDate", model.TimingDate);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 修改App版本信息
        /// danny-20150715
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ModifyAppVersion(AppVersionModel model)
        {
            string sql = @"UPDATE AppVersion 
                            SET PlatForm=@PlatForm,
                                UserType=@UserType,
                                Version=@Version,
                                IsMust=@IsMust,
                                UpdateUrl=@UpdateUrl,
                                Message=@Message,
                                UpdateBy=@UpdateBy,
                                UpdateDate=getdate(),";
            if (model.IsTiming == 1)
            {
                sql += " TimingDate=@TimingDate, ";
            }
            sql += @" IsTiming=@IsTiming
                        WHERE  ID = @Id;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@PlatForm", model.PlatForm);
            parm.AddWithValue("@UserType", model.UserType);
            parm.AddWithValue("@Version", model.Version);
            parm.AddWithValue("@IsMust", model.IsMust);
            parm.AddWithValue("@UpdateUrl", model.UpdateUrl);
            parm.AddWithValue("@Message", model.Message);
            parm.AddWithValue("@TimingDate", model.TimingDate);
            parm.AddWithValue("@IsTiming", model.IsTiming);
            parm.AddWithValue("@UpdateBy", model.OptUserName);
            parm.AddWithValue("@Id", model.ID);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
        /// <summary>
        /// 用Id查询升级信息
        /// danny-20150715
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AppVerionModel GetAppVersionById(int Id)
        {
            const string querysql = @"
SELECT [ID]
      ,[Version]
      ,[IsMust]
      ,[UpdateUrl]
      ,[Message]
      ,[PlatForm]
      ,[UserType]
      ,[CreateDate]
      ,[CreateBy]
      ,[UpdateDate]
      ,[UpdateBy]
      ,[IsTiming]
      ,[TimingDate]
      ,[PubStatus]
 FROM [AppVersion] WITH(NOLOCK)
WHERE ID=@Id;";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Id", Id);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, querysql, parm);
            if (!dt.HasData())
            {
                return null;
            }
            return MapRows<AppVerionModel>(dt)[0];
        }
        /// <summary>
        /// 修改App版本的发布状态为取消
        /// danny-20150715
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CancelAppVersion(AppVersionModel model)
        {
            string sql = @"UPDATE AppVersion 
                            SET PubStatus=@PubStatus,
                                UpdateBy=@UpdateBy,
                                UpdateDate=getdate()
                            WHERE  ID = @Id;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@PubStatus", 2);
            parm.AddWithValue("@UpdateBy", model.OptUserName);
            parm.AddWithValue("@Id", model.ID);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0;
        }
    }
}
