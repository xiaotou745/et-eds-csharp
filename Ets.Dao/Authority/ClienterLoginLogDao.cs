#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data;
using Ets.Model.DataModel.Finance;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Extension;
using Ets.Model.DomainModel.Finance;
using ETS.Util;
using Ets.Model.ParameterModel.Finance;
using ETS.Enums;
using Ets.Model.DomainModel.Authority;
#endregion

namespace Ets.Dao.Authority
{
    /// <summary>
    /// 商户余额流水表 数据访问类BusinessBalanceRecordDao。
    /// Generate By: tools.etaoshi.com   caoheyang
    /// Generate Time: 2015-05-09 14:59:36
    /// </summary>
    public class ClienterLoginLogDao : DaoBase
    {
        public ClienterLoginLogDao()
        {

        }

        		/// <summary>
		/// 增加一条记录
		/// </summary>
        public long Insert(ClienterLoginLogDM clienterLoginLogDM)
		{
            const string insertSql = @"
insert into ClienterLoginLog(ClienterId,PhoneNo,SSID,OperSystem,OperSystemModel,PhoneType,AppVersion,Description,IsSuccess)
values(@ClienterId,@PhoneNo,@Ssid,@OperSystem,@OperSystemModel,@PhoneType,@AppVersion,@Description,@IsSuccess)

select @@IDENTITY";

            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("ClienterId", clienterLoginLogDM.ClienterId);
            dbParameters.AddWithValue("PhoneNo", clienterLoginLogDM.PhoneNo);
            dbParameters.AddWithValue("Ssid", clienterLoginLogDM.Ssid);
            dbParameters.AddWithValue("OperSystem", clienterLoginLogDM.OperSystem);
            dbParameters.AddWithValue("OperSystemModel", clienterLoginLogDM.OperSystemModel);
            dbParameters.AddWithValue("PhoneType", clienterLoginLogDM.PhoneType);
            dbParameters.AddWithValue("AppVersion", clienterLoginLogDM.AppVersion);           
            dbParameters.AddWithValue("Description", clienterLoginLogDM.Description);
            dbParameters.AddWithValue("IsSuccess", clienterLoginLogDM.IsSuccess);


            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
            if (result == null)
            {
                return 0;
            }
            return long.Parse(result.ToString());

		}
    }
}

