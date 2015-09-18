using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data.Core;
using Ets.Model.DataModel.Tag;

namespace Ets.Dao.Tag
{
    /// <summary>
    /// 标签关系类  add by caoheyang 20150917
    /// </summary>
    public class TagRelationDao : DaoBase
    {
        /// <summary>
        /// 增加一条记录
        /// </summary>
        public int Insert(TagRelation tagrelation)
        {
            const string INSERT_SQL = @"
insert into tagrelation(UserId,TagId,UserType,IsEnable,CreateBy,CreateTime,UpdateBy,UpdateTime)
values(@UserId,@TagId,@UserType,@IsEnable,@CreateBy,@CreateTime,@UpdateBy,@UpdateTime)
select @@IDENTITY";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("UserId", tagrelation.UserId);
            dbParameters.AddWithValue("TagId", tagrelation.TagId);
            dbParameters.AddWithValue("UserType", tagrelation.UserType);
            dbParameters.AddWithValue("IsEnable", tagrelation.IsEnable);
            dbParameters.AddWithValue("CreateBy", tagrelation.CreateBy);
            dbParameters.AddWithValue("CreateTime", tagrelation.CreateTime);
            dbParameters.AddWithValue("UpdateBy", tagrelation.UpdateBy);
            dbParameters.AddWithValue("UpdateTime", tagrelation.UpdateTime);
            object result = DbHelper.ExecuteScalar(SuperMan_Write, INSERT_SQL, dbParameters);
            if (result == null)
            {
                return 0;
            }
            return int.Parse(result.ToString());
        }


        /// <summary>
        /// 获取用户 标签关系列表
        /// caoheyang 20150917
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public IList<TagRelation> GetTagRelationRelationList(int userId, int userType)
        {
           const string sql = @"  
select Id,UserId,TagId,UserType,IsEnable,CreateBy,CreateTime,UpdateBy,UpdateTime
from  tagrelation (nolock)
where UserId=@UserId and IsEnable=1 and UserType=@UserType
ORDER BY Id;";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@UserId", userId);
            parm.AddWithValue("@UserType", userType);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            return MapRows<TagRelation>(dt);
        }
        /// <summary>
        /// 编辑商户和快递公司绑定关系
        /// danny-20150706
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Edit(TagRelation model)
        {
            const string updateSql = @"
MERGE INTO TagRelation ber
	USING(values(@UserId,@TagId,@UserType)) AS berNew(UserId,TagId,UserType)
		ON ber.UserId=berNew.UserId AND  ber.TagId=berNew.TagId and
        ber.UserType=berNew.UserType
	WHEN MATCHED 
	THEN UPDATE 
		 SET ber.IsEnable=@IsEnable,
             ber.UpdateBy=@OptName,
             ber.UpdateTime=getdate()
	WHEN NOT MATCHED 
		  THEN INSERT
					(UserId,
					 TagId,
                     UserType,
                     CreateBy,
                     UpdateBy,
                     IsEnable) 
					VALUES 
					(@UserId,
					 @TagId,
                     @UserType,
                     @OptName,
                     @OptName,
                     @IsEnable);";
            var parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@UserId", model.UserId);
            parm.AddWithValue("@TagId", model.TagId);
            parm.AddWithValue("@IsEnable", model.IsEnable);
            parm.AddWithValue("@OptName", model.CreateBy);
            parm.AddWithValue("@UserType", model.UserType);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, parm) > 0;
        }
    }
}
