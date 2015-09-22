using System.Data;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using ETS.Util;
using System.Collections.Generic;
using Ets.Model.DataModel.Business;
using Ets.Model.ParameterModel.Business;
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

        /// <summary>
        /// 获取所有集团下拉用
        /// 窦海超
        /// 2015年9月23日 02:13:56
        /// </summary>
        /// <returns></returns>
        public IList<GroupBusinessModel> Get()
        {
            string sql = "SELECT Id,GroupBusiName FROM dbo.GroupBusiness gb(nolock) order by id desc";
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql);
            if (dt == null)
            {
                return null;
            }
            return MapRows<GroupBusinessModel>(dt);
        }
    }
}
