using Ets.Model.DataModel.Bussiness;
using ETS.Dao;
using ETS.Data.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.User
{
    public class BusinessRechargeDao : DaoBase
    {

        /// <summary>
        /// 添加一条充值记录
        /// 窦海超
        /// 2015年5月29日 16:08:40
        /// </summary>
        /// <param name="model"></param>
        public void Insert(BusinessRechargeModel model)
        {
            string sql = @"
insert into dbo.BusinessRecharge ( BusinessId , PayType, OrderNo, payAmount, PayStatus,
                                    PayBy, PayTime, OriginalOrderNo )
values  (
          @BusinessId, --int
          @PayType, -- PayType - int
          @OrderNo, -- OrderNo - varchar(100)
          @payAmount, --  - decimal
          @PayStatus , -- PayStatus - int
          @PayBy, -- PayBy - nvarchar(100)
          getdate(), -- PayTime - date
          @OriginalOrderNo  -- OriginalOrderNo - varchar(100)
          )";
            IDbParameters parm = DbHelper.CreateDbParameters();

            parm.Add("BusinessId", DbType.Int32, 4).Value = model.BusinessId;
            parm.Add("PayType", DbType.Int32, 4).Value = model.PayType;
            parm.Add("OrderNo", DbType.String, 100).Value = model.OrderNo;
            parm.Add("payAmount", DbType.Decimal, 18).Value = model.PayAmount;
            parm.Add("PayStatus", DbType.Int32, 4).Value = model.PayStatus;
            parm.Add("PayBy", DbType.String, 200).Value = model.PayBy;
            parm.Add("OriginalOrderNo", DbType.String, 100).Value = model.OriginalOrderNo;
            DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm);
        }
    }
}
