using System.Data;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using ETS.Util;
using System.Collections.Generic;
using Ets.Model.DataModel.Business;
using   Ets.Model.ParameterModel.Business;
using Ets.Model.ParameterModel.Finance;
namespace Ets.Dao.Business
{
    public class GroupBusinessDao : DaoBase
    {
        public void UpdateAmount(UpdateForWithdrawPM model)
        {
            const string updateSql = @"
update  GroupBusiness
set  Amount=Amount+@Amount
where  Id=@Id ";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("Id", model.Id);
            dbParameters.AddWithValue("Amount", model.Money);
            DbHelper.ExecuteNonQuery(SuperMan_Write, updateSql, dbParameters);
         }
    }
}
