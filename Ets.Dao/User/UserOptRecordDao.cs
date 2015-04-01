using Ets.Model.DataModel.User;
using Ets.Model.ParameterModel.User;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.User
{

    /// <summary>
    /// 用户操作历史记录流水 add by caoheyang  20150401
    /// </summary>
    public class UserOptRecordDao : DaoBase
    {
        /// <summary>
        /// 新增用户操作记录 add by caoheyang 
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public int InsertUserOptRecord(UserOptRecordPara model)
        {
            string isnertLog = @"
                insert into dbo.UserOptRecord
                        (
                          UserType ,
                          UserID,       
                          OptType,       
                          OptUserId ,
                          OptUserName ,
                          OptUserType ,
                          Remark
                        )
                values  ( @UserType , 
                          @UserID,  
                          @OptType,       
                          @OptUserId ,
                          @OptUserName ,
                          @OptUserType ,
                          @Remark 
                        )";
            IDbParameters isnertLogdbParas = DbHelper.CreateDbParameters();
            isnertLogdbParas.AddWithValue("@UserType", 1);//被操作用户类型1 B端商户 
            isnertLogdbParas.AddWithValue("@UserID", model.UserID);
            isnertLogdbParas.AddWithValue("@OptType", model.OptType);
            isnertLogdbParas.AddWithValue("@OptUserId", model.OptUserId);
            isnertLogdbParas.AddWithValue("@OptUserName", model.OptUserName);
            isnertLogdbParas.AddWithValue("@OptUserType", model.OptUserType);
            isnertLogdbParas.AddWithValue("@Remark", model.Remark);
            return ParseHelper.ToInt(DbHelper.ExecuteNonQuery(SuperMan_Write, isnertLog, isnertLogdbParas));
        }
    }
}
