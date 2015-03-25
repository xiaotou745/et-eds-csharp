/* 
 * 平扬
 * 2015.3.16新增  
 * 集团api配置数据层
 */
using ETS;
using System;
using ETS.Data.PageData;
using Ets.Model.Common;
using ETS.Dao;
using ETS.Data.Core;
using Ets.Model.DataModel.Group;
using Ets.Model.DomainModel.Group;
using Ets.Model.ParameterModel.Group;

namespace Ets.Dao.Common
{

    /// <summary>
    /// 集团api配置数据层
    /// </summary>
    public class GroupDao : DaoBase
    {
        /// <summary>
        /// 创建集团
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public int AddGroup(GroupModel model)
        {
            string sql = @" INSERT INTO [group](GroupName,CreateName,CreateTime,ModifyName,ModifyTime,IsValid)
                             VALUES (@GroupName,@CreateName,@CreateTime,@ModifyName,@ModifyTime,@IsValid);select @@IDENTITY ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("GroupName", model.GroupName);
            dbParameters.AddWithValue("CreateName", model.CreateName);
            dbParameters.AddWithValue("CreateTime", model.CreateTime);
            dbParameters.AddWithValue("ModifyName", model.ModifyName);
            dbParameters.AddWithValue("ModifyTime", model.ModifyTime);
            dbParameters.AddWithValue("IsValid", model.IsValid);
            object i = DbHelper.ExecuteScalar(Config.SuperMan_Write, sql, dbParameters);
            if (i != null)
            {
                return int.Parse(i.ToString());
            }
            return 0;
        }

        /// <summary>
        /// 判断集团是否已经创建过
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool HasExistsGroup(GroupModel model)
        {
            string sql = @" select 1 from [group] with(nolock) where  GroupName=@GroupName";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("GroupName", model.GroupName);
            object i = DbHelper.ExecuteScalar(Config.SuperMan_Read, sql, dbParameters);
            if (i != null)
            {
                return int.Parse(i.ToString())>0;
            }
            return false;
        }

        /// <summary>
        /// 创建集团Api配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool CreateGroupApiConfig(GroupApiConfigModel model)
        {
            string sql = @" if not exists (select 1 from GroupApiConfig with(nolock) where AppKey=@AppKey and AppVersion=@AppVersion) begin INSERT INTO GroupApiConfig
                                   (AppKey
                                   ,AppSecret
                                   ,AppVersion
                                   ,GroupId)
                             VALUES
                                   (@AppKey
                                   ,@AppSecret
                                   ,@AppVersion
                                   ,@GroupId);select @@IDENTITY end select 0 ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("AppKey", model.AppKey);
            dbParameters.AddWithValue("AppSecret", model.AppSecret);
            dbParameters.AddWithValue("AppVersion", model.AppVersion);
            dbParameters.AddWithValue("GroupId", model.GroupId);
            object i = DbHelper.ExecuteScalar(Config.SuperMan_Write, sql, dbParameters);
            if (i!=null)
            {
                return int.Parse(i.ToString())>0;
            }
            return false;
        }
        /// <summary>
        /// 修改集团api配置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateGroupApiConfig(GroupApiConfigModel model)
        {
            string sql = " update GroupApiConfig set AppKey=@AppKey,AppSecret=@AppSecret,AppVersion=@AppVersion where GroupId=@id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("GroupId", model.GroupId);
            dbParameters.AddWithValue("AppKey", model.AppKey);
            dbParameters.AddWithValue("AppSecret", model.AppSecret);
            dbParameters.AddWithValue("AppVersion", model.AppVersion);  
            int i = DbHelper.ExecuteNonQuery(Config.SuperMan_Write, sql, dbParameters);
            if (i>0)
            {
                return true;
            }
            return false;

        }

        /// <summary>
        /// 修改集团名称信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateGroupName(GroupModel model)
        {
            string sql = " update [group] set GroupName=@GroupName where id=@id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("id", model.Id);
            dbParameters.AddWithValue("GroupName", model.GroupName);
            int i = DbHelper.ExecuteNonQuery(Config.SuperMan_Write, sql, dbParameters);
            if (i > 0)
            {
                return true;
            }
            return false;

        }
        /// <summary>
        /// 修改集团api状态信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateGroupStatus(GroupModel model)
        {
            string sql = " update [group] set IsValid=@IsValid where id=@id";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("id", model.Id);
            dbParameters.AddWithValue("IsValid", model.IsValid);
            int i = DbHelper.ExecuteNonQuery(Config.SuperMan_Write, sql, dbParameters);
            if (i > 0)
            {
                return true;
            }
            return false;

        }
        /// <summary>
        /// 根据appkey和版本查询配置信息
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public GroupApiConfigModel GetGroupApiConfigByAppKey(string appkey,string version)
        {
            string sql = @" select AppSecret,GroupId,IsValid from GroupApiConfig with(nolock) inner join [group] with(nolock) 
                             on GroupApiConfig.groupid=[group].id
                             where AppKey=@AppKey and AppVersion=@AppVersion ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("AppKey", appkey);
            dbParameters.AddWithValue("AppVersion", version);
            var dr = DbHelper.ExecuteReader(Config.SuperMan_Read, sql, dbParameters);
            GroupApiConfigModel config = null;
            if (dr.Read())
            {
                config=new GroupApiConfigModel {AppKey = appkey, AppVersion = version};
                if (dr["AppSecret"] != null)
                { 
                    config.AppSecret = dr["AppSecret"].ToString();
                }
                if (dr["GroupId"] != null)
                {
                    config.GroupId = int.Parse(dr["GroupId"].ToString());
                }
                if (dr["IsValid"] != null)
                {
                    config.IsValid = (byte) int.Parse(dr["IsValid"].ToString());
                }
            }
            return config;
        }


        public PageInfo<T> GetGroupList<T>(GroupParaModel model)
        {
            string sqlcolomn = @" g.id as GroupId,g.GroupName,g.CreateName,g.CreateTime,g.IsValid,isnull(gc.AppKey,'') AppKey,isnull(gc.AppSecret,'') AppSecret,isnull(gc.AppVersion,'') AppVersion  ";
            string where = " 1=1 ";
            if (!string.IsNullOrEmpty(model.GroupName) && model.GroupName!="")
            {
                where += string.Format(" and GroupName= '{0}'  ",model.GroupName.Trim()); 
            }
            if (!string.IsNullOrEmpty(model.AppKey) && model.AppKey != "")
            {
                where += string.Format(" and AppKey= '{0}' ", model.AppKey); 
            }
            return new PageHelper().GetPages<T>(SuperMan_Read, model.PageIndex, where, "g.Id", sqlcolomn, " [group] g with(nolock) left join GroupApiConfig gc with(nolock) on g.Id = gc.GroupId ", model.PageSize, true);
        }
    }
}
