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
    public class BusinessLoginLogDao : DaoBase
    {
        public BusinessLoginLogDao()
        {

        }

        		/// <summary>
		/// 增加一条记录
		/// </summary>
        public long Insert(BusinessLoginLogDM BusinessLoginLogDM)
		{
            const string insertSql = @"
insert into BusinessLoginLog(BusinessId,PhoneNo,SSID,OperSystem,OperSystemModel,PhoneType,AppVersion,Description,IsSuccess)
values(@BusinessId,@PhoneNo,@SSID,@OperSystem,@OperSystemModel,@PhoneType,@AppVersion,@Description,@IsSuccess)

select @@IDENTITY";

			IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("BusinessId", BusinessLoginLogDM.BusinessId);
            dbParameters.AddWithValue("PhoneNo", BusinessLoginLogDM.PhoneNo);
            dbParameters.AddWithValue("SSID", BusinessLoginLogDM.Ssid);
            dbParameters.AddWithValue("OperSystem", BusinessLoginLogDM.OperSystem);
            dbParameters.AddWithValue("OperSystemModel", BusinessLoginLogDM.OperSystemModel);
            dbParameters.AddWithValue("PhoneType", BusinessLoginLogDM.PhoneType);
            dbParameters.AddWithValue("AppVersion", BusinessLoginLogDM.AppVersion);       
            dbParameters.AddWithValue("Description", BusinessLoginLogDM.Description);
            dbParameters.AddWithValue("IsSuccess", BusinessLoginLogDM.IsSuccess);

            object result = DbHelper.ExecuteScalar(SuperMan_Write, insertSql, dbParameters);
			if (result == null)
			{
				return 0;
			}
			return long.Parse(result.ToString());
		}
    }
}

