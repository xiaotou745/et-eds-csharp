using Ets.Model.DataModel.Group;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.Common
{
    public class GroupApiConfigDao : DaoBase
    {
        /// <summary>
        /// 根据groupId 查询当前集团可用sign信息 add by caoheyang 20150327
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="version"></param>
        /// <returns></returns>
         public GroupApiConfig GetGroupApiConfigByGroupID(int groupId)
        {
            string sql = string.Format(@"SELECT a.* FROM dbo.GroupApiConfig (NOLOCK) AS a 
                    LEFT JOIN dbo.[group](NOLOCK) AS b ON a.GroupId=b.Id  WHERE b.IsValid=1 and GroupId={0}"
                ,groupId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, sql));
            if (dt == null || dt.Rows.Count <= 0)
                return null;
            return MapRows<GroupApiConfig>(dt)[0];
        }
    }
}
