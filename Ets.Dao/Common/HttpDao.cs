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
            string sql = @"INSERT INTO dbo.HttpLog
        ( Url ,
          HType ,
          RequestBody ,
          ResponseType ,
          Msg ,
          Status ,
          Remark 
        )
VALUES  ( @Url ,
          @HType ,
          @RequestBody ,
          @ResponseType ,
          @Msg ,
          @Status ,
          @Remark 
        )";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@Url", DbType.String).Value=model.Url;
            parm.Add("@HType", DbType.Int32).Value = model.Htype;
            parm.Add("@RequestBody", DbType.String).Value = model.RequestBody;
            parm.Add("@ResponseType", DbType.Int32).Value = model.ResponseType;
            parm.Add("@Msg", DbType.String).Value = model.Msg;
            parm.Add("@Status", DbType.Int32).Value = model.Status;
            parm.Add("@Remark", DbType.String).Value = model.Remark;
            DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm);
        }
        /// <summary>
        /// 记录响应数据
        /// </summary>
        /// <param name="model"></param>
        public void LogResponseInfo(HttpModel model)
        {

        }
        /// <summary>
        /// 记录第三方请求及响应
        /// </summary>
        /// <param name="model"></param>
        public void LogThirdPartyInfo(HttpModel model)
        {

        }
    }
}
