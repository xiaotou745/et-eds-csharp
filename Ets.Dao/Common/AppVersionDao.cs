using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETS.Dao;
using ETS.Data;
using ETS.Data.Core;
using ETS.Extension;
using Ets.Model.Common;

namespace Ets.Dao.Common
{
    public class AppVersionDao : DaoBase
    {
        /// <summary>
        /// 查询升级信息
        /// </summary>
        /// <param name="vcmodel"></param>
        /// <returns></returns>
        public AppVerionModel VersionCheck(VersionCheckModel vcmodel)
        {
            const  string querysql=@"
SELECT TOP 1 [Version],IsMust,UpdateUrl,[Message] FROM dbo.AppVersion 
WHERE [PlatForm]=@PlatForm AND UserType=@UserType
ORDER BY ID DESC";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@PlatForm", vcmodel.PlatForm);
            parm.AddWithValue("@UserType", vcmodel.UserType);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, querysql, parm);
            if (!dt.HasData())
            {
                return null;
            }
            return MapRows<AppVerionModel>(dt)[0]; 
        }
    }
}
