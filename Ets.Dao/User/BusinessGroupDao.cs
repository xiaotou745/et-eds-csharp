using System.Data;
using ETS;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using ETS.Data.PageData;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Const;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.DomainModel.Clienter;
using Ets.Model.ParameterModel.Bussiness;
using Ets.Model.DataModel.Order;
using Ets.Model.DataModel.Bussiness;
using ETS.Extension;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.DataModel.Group;


namespace Ets.Dao.User
{
    public class BusinessGroupDao : DaoBase
    {
        public bool InsertDataBusinessGroup(BusinessGroupModel model)
        {
            string sql = @"INSERT INTO [dbo].[BusinessGroup]
                            ([Name]
                            ,[StrategyId]
                            ,[CreateBy]
                            ,[CreateTime]
                            ,[UpdateBy]
                            ,[UpdateTime]                            
                            )
                            VALUES
                            (@Name
                            ,@StrategyId
                            ,@CreateBy
                            ,@CreateTime
                            ,@UpdateBy
                            ,@UpdateTime
                            )
                            ";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@Name", model.Name);
            parm.AddWithValue("@StrategyId", model.StrategyId);
            parm.AddWithValue("@CreateBy", model.CreateBy);
            parm.AddWithValue("@CreateTime", model.CreateTime);
            parm.AddWithValue("@UpdateBy", model.UpdateBy);
            parm.AddWithValue("@UpdateTime", model.UpdateTime);         
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }

        public IList<BusinessGroupModel> GetStrategyList()
        {
            string sql = string.Format(@" SELECT * FROM   dbo.[BusinessGroup] WITH ( NOLOCK )");
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            return MapRows<BusinessGroupModel>(dt);
        }

    }
}
