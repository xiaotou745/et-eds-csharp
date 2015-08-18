using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.DataModel.Complain;
using Ets.Model.ParameterModel.Complain;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Util;

namespace Ets.Dao.Complain
{
    public class ComplainDao : DaoBase
    {
        public int Insert(ComplainModel complainModel)
        {
            const string insertSql = @"
insert into dbo.Complain
        ( ComplainId ,
          ComplainedId ,
          Reason ,
          OrderId ,
          OrderNo ,
          ComplainType 
        )
values  ( @ComplainClienterId ,
          @ComplainedBusiId ,
          @Reason ,
          @OrderId ,
          @OrderNo ,
          @ComplainType 
        )";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.Add("ComplainId", DbType.Int32).Value = complainModel.ComplainId;
            dbParameters.Add("ComplainedId", DbType.Int32).Value = complainModel.ComplainedId;
            dbParameters.Add("Reason", DbType.String).Value = complainModel.Reason;
            dbParameters.Add("OrderId", DbType.Int32).Value = complainModel.OrderId;
            dbParameters.Add("OrderNo", DbType.String).Value = complainModel.OrderNo;
            dbParameters.Add("ComplainType", DbType.Int32).Value = complainModel.ComplainType; 
            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            return ParseHelper.ToInt(result, -1);
        }
    }
}
