using System.Data;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using ETS.Util;
using System.Collections.Generic;
using Ets.Model.DataModel.Business;

namespace Ets.Dao.Business
{
    public class BusinessGroupDao : DaoBase
    {
        /// <summary>
        /// 获取商家分组集合列表
        /// </summary>
        /// <returns></returns>
        public IList<BusinessGroupModel> GetBusinessGroupList()
        {
            string sql = string.Format(@" Select Bus.ID,Bus.Name,Bus.StrategyId,Bus.CreateBy,Bus.CreateTime,Bus.UpdateBy,Bus.UpdateTime,
                                          St.Name as StrategyName FROM  
                                          dbo.[BusinessGroup] as Bus WITH ( NOLOCK )
                                          left join Strategy as St WITH ( NOLOCK ) on Bus.StrategyId=St.StrategyId");
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            return MapRows<BusinessGroupModel>(dt);
        }     

        /// <summary>
        /// 获取商家分组实体
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public BusinessGroupModel GetCurrenBusinessGroup(int businessId)
        {
            string sql = @"select BusinessGroup.ID,BusinessGroup.Name,BusinessGroup.StrategyId,BusinessGroup.CreateBy,
                           BusinessGroup.CreateTime,BusinessGroup.UpdateBy,BusinessGroup.UpdateTime 
                           from Business WITH ( NOLOCK )
                           left join BusinessGroup WITH ( NOLOCK ) on BusinessGroupId=BusinessGroup.Id                        
                           WHERE  Business.Id=@businessId ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("@businessId", SqlDbType.Int);
            dbParameters.SetValue("@businessId", businessId);

            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, dbParameters);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            return MapRows<BusinessGroupModel>(dt)[0];
        }

        /// <summary>
        /// 添加商家分组
        /// danny-20150506
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddBusinessGroup(BusinessGroupModel model)
        {
            string sql = @"INSERT INTO [BusinessGroup]
                            (
                             [Name]
                            ,[StrategyId]
                            ,[CreateBy]
                            ,[CreateTime]
                            ,[UpdateBy]                           
                            )
                                OUTPUT  Inserted.Name,
                                        Inserted.StrategyId,
                                        getdate(),
                                        Inserted.CreateBy,
                                        @Remark
                                INTO BusinessGroupLog
                                       (Name,
                                        StrategyId,
                                        InsertTime,
                                        OptName,
                                        Remark)
                            VALUES
                            (@Name
                            ,@StrategyId
                            ,@CreateBy
                            ,getdate()
                            ,@CreateBy
                            ); SELECT IDENT_CURRENT('BusinessGroup') --SELECT @@IDENTITY ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Name", model.Name);
            parm.AddWithValue("@StrategyId", model.StrategyId);
            parm.AddWithValue("@CreateBy", model.CreateBy);
            parm.AddWithValue("@Remark", "添加商家分组信息");
            return ParseHelper.ToInt(DbHelper.ExecuteScalar(SuperMan_Write, sql, parm));
        }
        /// <summary>
        /// 修改商家分组
        /// danny-20150506
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateBusinessGroup(BusinessGroupModel model)
        {
            string sql = @"UPDATE [BusinessGroup]
                            SET Name=@Name,
                                StrategyId=@StrategyId,
                                UpdateBy=@UpdateBy,
                                UpdateTime=getdate()
                                OUTPUT  DELETED.Name,
                                        DELETED.StrategyId,
                                        getdate(),
                                        Inserted.UpdateBy,
                                        @Remark
                                INTO BusinessGroupLog
                                       (Name,
                                        StrategyId,
                                        InsertTime,
                                        OptName,
                                        Remark)
                           WHERE Id=@Id; ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Id", model.Id);
            parm.AddWithValue("@Name", model.Name);
            parm.AddWithValue("@StrategyId", model.StrategyId);
            parm.AddWithValue("@UpdateBy", model.UpdateBy);
            parm.AddWithValue("@Remark","修改商家分组信息");
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }
        /// <summary>
        /// 添加全局配置
        /// danny-20150506
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddGlobalConfig(Model.Common.GlobalConfig model)
        {
            string sql = @"INSERT INTO [GlobalConfig]
                            (
                             [KeyName]
                            ,[Value]
                            ,[LastUpdateTime]
                            ,[Remark]
                            ,[GroupId]
                            ,[StrategyId]                            
                            )
                                OUTPUT  Inserted.KeyName,
                                        Inserted.Value,
                                        getdate(),
                                        @OptName,
                                        @LogRemark,
                                        Inserted.GroupId,
                                        Inserted.StrategyId
                                INTO BusinessGroupLog
                                       (KeyName,
                                        Value,
                                        InsertTime,
                                        OptName,
                                        Remark,
                                        GroupId,
                                        StrategyId)
                            VALUES
                            (@KeyName
                            ,@Value
                            ,getdate()
                            ,@Remark
                            ,@GroupId
                            ,@StrategyId
                            ); SELECT @@IDENTITY";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@KeyName", model.KeyName);
            parm.AddWithValue("@Value", model.Value);
            parm.AddWithValue("@Remark", model.Remark);
            parm.AddWithValue("@GroupId", model.GroupId);
            parm.AddWithValue("@StrategyId", model.StrategyId);
            parm.AddWithValue("@LogRemark", model.Remark);
            parm.AddWithValue("@OptName", model.Remark);
            return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm));
        }

        /// <summary>
        /// 复制全局配置默认模板
        /// danny-20150506
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CopyGlobalConfigMode(int groupId, string OptName)
        {
            string sql = @"INSERT	INTO GlobalConfig
                                    (KeyName,
                                    Value,
                                    LastUpdateTime,
                                    Remark,
                                    GroupId,
                                    StrategyId)
			            OUTPUT	INSERTED.KeyName ,
					            INSERTED.Value ,
					            getdate(),
                                @OptName,
                                INSERTED.Remark,
                                INSERTED.GroupId ,
					            INSERTED.StrategyId 
					    INTO GlobalConfigLog(KeyName, Value,InsertTime,OptName,Remark,GroupId,StrategyId)
                        SELECT  KeyName,
                                Value,
                                getdate(),
                                Remark,
                                @GroupId,
                                -1
                        FROM  GlobalConfig 
                        WHERE groupid=0 
                          AND strategyid=-1	; ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@GroupId", groupId);
            parm.AddWithValue("@OptName", OptName);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }
        /// <summary>
        /// 修改全局配置
        /// danny-20150506
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool UpdateGlobalConfig(Model.Common.GlobalConfig model)
        {
            string sql = @" UPDATE GlobalConfig 
                              SET Value=@Value,
                                  LastUpdateTime=getdate(),
                                  StrategyId=@StrategyId
                              OUTPUT    INSERTED.KeyName ,
			                            INSERTED.Value ,
			                            getdate(),
                                        @OptName,
                                        ISNULL(DELETED.Value,''),
                                        INSERTED.GroupId ,
			                            INSERTED.StrategyId 
			                    INTO GlobalConfigLog(KeyName, Value,InsertTime,OptName,Remark,GroupId,StrategyId)
                              WHERE KeyName=@KeyName  and GroupId=@GroupId";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@KeyName", model.KeyName);
            parm.AddWithValue("@GroupId", model.GroupId);
            parm.AddWithValue("@Value", model.Value);
            parm.AddWithValue("@StrategyId", model.StrategyId);
            parm.AddWithValue("@OptName", model.OptName);
            int i = DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm);
            return i > 0;
        }
    }
}
