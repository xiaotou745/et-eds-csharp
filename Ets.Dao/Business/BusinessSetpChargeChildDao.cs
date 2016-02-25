using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Const;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
using ETS.Extension;
using Ets.Model.DataModel.Business;
using Ets.Model.DomainModel.Business;
using Ets.Model.ParameterModel.Business;
using ETS.Util;
using Ets.Model.ParameterModel.Order;

namespace Ets.Dao.Business
{
    /// <summary>
    /// 
    /// </summary>
    public class BusinessSetpChargeChildDao : DaoBase
    {
        public BusinessSetpChargeChildDao()
        { }
       

        /// <summary>
        /// 增加一条记录
        /// </summary>
        public long Insert(BusinessSetpChargeChild businessSetpChargeChild)
        {
            const string INSERT_SQL = @"
insert into BusinessSetpChargeChild(SetpChargeId,MinValue,MaxValue,CreateDate,ChargeValue,Enable)
values(@SetpChargeId,@MinValue,@MaxValue,@CreateDate,@ChargeValue,@Enable)

select @@IDENTITY";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("SetpChargeId", businessSetpChargeChild.SetpChargeId);
            dbParameters.AddWithValue("MinValue", businessSetpChargeChild.MinValue);
            dbParameters.AddWithValue("MaxValue", businessSetpChargeChild.MaxValue);
            dbParameters.AddWithValue("CreateDate", businessSetpChargeChild.CreateDate);
            dbParameters.AddWithValue("ChargeValue", businessSetpChargeChild.ChargeValue);
            dbParameters.AddWithValue("Enable", businessSetpChargeChild.Enable);


            object result = DbHelper.ExecuteScalar(SuperMan_Write, INSERT_SQL, dbParameters);
            if (result == null)
            {
                return 0;
            }
            return long.Parse(result.ToString());
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        public void Update(BusinessSetpChargeChild businessSetpChargeChild)
        {
            const string UPDATE_SQL = @"
update  BusinessSetpChargeChild
set  SetpChargeId=@SetpChargeId,MinValue=@MinValue,MaxValue=@MaxValue,CreateDate=@CreateDate,ChargeValue=@ChargeValue,Enable=@Enable
where  Id=@Id ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", businessSetpChargeChild.Id);
            dbParameters.AddWithValue("SetpChargeId", businessSetpChargeChild.SetpChargeId);
            dbParameters.AddWithValue("MinValue", businessSetpChargeChild.MinValue);
            dbParameters.AddWithValue("MaxValue", businessSetpChargeChild.MaxValue);
            dbParameters.AddWithValue("CreateDate", businessSetpChargeChild.CreateDate);
            dbParameters.AddWithValue("ChargeValue", businessSetpChargeChild.ChargeValue);
            dbParameters.AddWithValue("Enable", businessSetpChargeChild.Enable);


            DbHelper.ExecuteNonQuery(SuperMan_Write, UPDATE_SQL, dbParameters);
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        public void Delete(long id)
        {
            const string DELETE_SQL = @"delete from BusinessSetpChargeChild where Id=@Id ";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", id);



            DbHelper.ExecuteNonQuery(SuperMan_Write, DELETE_SQL, dbParameters);
        }

        /// <summary>
        /// 获取BusinessSetpChargeChild
        /// </summary>
        /// <param name="originalOrderNo"></param>
        /// <param name="orderfrom"></param>
        /// <returns></returns>
        public BusinessSetpChargeChild GetDetails(int setpChargeId)
        {
            BusinessSetpChargeChild model = null;

            const string querySql = @"SELECT SetpChargeId,MinValue,MaxValue,CreateDate,ChargeValue,Enable FROM [BusinessSetpChargeChild]   WITH ( NOLOCK )  
            WHERE setpChargeId=@setpChargeId and Enable=1 AND MaxValue=(             
            SELECT MAX(MaxValue) FROM [BusinessSetpChargeChild]   WITH ( NOLOCK )  
            WHERE setpChargeId=@setpChargeId and Enable=1            )";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@setpChargeId", setpChargeId);           
     

            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querySql, dbParameters));
            if (DataTableHelper.CheckDt(dt) && dt.Rows.Count > 0)
            {
                model = MapRows<BusinessSetpChargeChild>(dt)[0];
            }
            return model;
        }


        /// <summary>
        /// 获取商户应付金额
        /// </summary>
        /// <param name="originalOrderNo"></param>
        /// <param name="orderfrom"></param>
        /// <returns></returns>
        public decimal GetChargeValue(int setpChargeId, decimal money)
        {
            const string querySql = @"SELECT top 1  ChargeValue FROM [BusinessSetpChargeChild]   WITH ( NOLOCK )  
            WHERE setpChargeId=@setpChargeId AND @money>MinValue and @money<=MaxValue and Enable=1";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("@setpChargeId", setpChargeId);    
            dbParameters.AddWithValue("@money", money);    
            object executeScalar = DbHelper.ExecuteScalar(SuperMan_Read, querySql, dbParameters);
            return ParseHelper.ToDecimal(executeScalar,0);
        }



    }
}
