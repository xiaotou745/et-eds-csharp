using Ets.Model.DomainModel.Subsidy;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.Subsidy
{
    public class SubsidyDao:DaoBase
    {

        public SubsidyResultModel GetCurrentSubsidy(int groupId)
        {
            SubsidyResultModel subsidyResultModel = new SubsidyResultModel();
            StringBuilder selSql = new StringBuilder();

            selSql.Append(@"SELECT 
        TOP 1 
        OrderCommission ,
        DistribSubsidy ,
        WebsiteSubsidy  
FROM    dbo.subsidy sub WITH ( NOLOCK )
WHERE   sub.[Status] = 1 ");

            if (groupId > 0)
            {
                selSql.Append(" AND sub.GroupId = @groupId ");
            }

            selSql.Append(" ORDER BY id DESC ");

            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@groupId", groupId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, selSql.ToString(), parm));
            if (dt != null && dt.Rows.Count!=0)
            {
                subsidyResultModel = DataTableHelper.ConvertDataTableList<SubsidyResultModel>(dt)[0];
            }

            return subsidyResultModel; 
        }
    }
}
