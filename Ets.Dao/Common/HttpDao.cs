using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data.Core;
using Ets.Model.Common;

namespace Ets.Dao.Common
{
    public class HttpDao : DaoBase
    {
        /// <summary>
        /// 记录请求数据
        /// </summary>
        /// <param name="model"></param>
        public void LogRequestInfo(HttpModel model)
        {
            #region===异步记录数据
            Task.Factory.StartNew(() =>
            {
                string sql = @"INSERT INTO dbo.HttpLog
        ( Url ,
          Htype ,
          RequestBody ,
          ResponseBody ,
          ReuqestMethod ,
          ReuqestPlatForm ,
          Status ,
          Remark 
        )
VALUES  ( @Url ,
          @Htype ,
          @RequestBody ,
          @ResponseBody ,
          @ReuqestMethod ,
          @ReuqestPlatForm ,
          @Status ,
          @Remark 
        )";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.Add("@Url", DbType.String).Value = model.Url;
                parm.Add("@Htype", DbType.Int32).Value = model.Htype;
                parm.Add("@RequestBody", DbType.String).Value = model.RequestBody;
                parm.Add("@ResponseBody", DbType.String).Value = model.ResponseBody;
                parm.Add("@ReuqestMethod", DbType.String).Value = model.ReuqestMethod;
                parm.Add("@ReuqestPlatForm", DbType.Int32).Value = model.ReuqestPlatForm;
                parm.Add("@Status", DbType.Int32).Value = model.Status;
                parm.Add("@Remark", DbType.String).Value = model.Remark;
                DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm);
            });
            #endregion
        }

        /// <summary>
        /// 记录第三方请求及响应
        /// </summary>
        /// <param name="model"></param>
        public void LogThirdPartyInfo(HttpModel model)
        {
            Task.Factory.StartNew(() =>
            {
                #region===异步插入数据库
                string sql = @"INSERT INTO dbo.HttpLog
        ( Url ,
          Htype ,
          RequestBody ,
          ResponseBody ,
          ReuqestMethod ,
          ReuqestPlatForm ,
          Status ,
          Remark 
        )
VALUES  ( @Url ,
          @Htype ,
          @RequestBody ,
          @ResponseBody ,
          @ReuqestMethod ,
          @ReuqestPlatForm ,
          @Status ,
          @Remark 
        )";
                IDbParameters parm = DbHelper.CreateDbParameters();
                parm.Add("@Url", DbType.String).Value = model.Url;
                parm.Add("@Htype", DbType.Int32).Value = model.Htype;
                parm.Add("@RequestBody", DbType.String).Value = model.RequestBody;
                parm.Add("@ResponseBody", DbType.String).Value = model.ResponseBody;
                parm.Add("@ReuqestMethod", DbType.String).Value = model.ReuqestMethod;
                parm.Add("@ReuqestPlatForm", DbType.Int32).Value = model.ReuqestPlatForm;
                parm.Add("@Status", DbType.Int32).Value = model.Status;
                parm.Add("@Remark", DbType.String).Value = model.Remark;
                DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm);
                #endregion
            });
        }
    }
}
