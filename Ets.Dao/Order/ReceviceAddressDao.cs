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
using Ets.Model.DataModel.Order;
using Ets.Model.DomainModel.Order;
using Ets.Model.ParameterModel.Order;

namespace Ets.Dao.Order
{
    /// <summary>
    /// 收货人地址表  add By  caoheyang   20150702
    /// </summary>
    public class ReceviceAddressDao : DaoBase
    {

       /// <summary>
        ///  B端商户拉取收货人地址缓存到本地 add By  caoheyang   20150702 
       /// </summary>
       /// <param name="model">参数实体</param>
       /// <returns></returns>
        public IList<ConsigneeAddressBDM> ConsigneeAddressB(ConsigneeAddressBPM model)
        {
            IList<ConsigneeAddressBDM> models = new List<ConsigneeAddressBDM>();
            const string querysql = @"
select  Id ,PhoneNo,Address,PubDate
from    dbo.ReceviceAddress
where   Id > @AddressId
        and BusinessId = @BusinessId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("BusinessId", model.BusinessId);
            dbParameters.AddWithValue("AddressId", model.AddressId);
            DataTable dt = DataTableHelper.GetTable(DbHelper.ExecuteDataset(SuperMan_Read, querysql, dbParameters));
            if (DataTableHelper.CheckDt(dt))
            {
                models = DataTableHelper.ConvertDataTableList<ConsigneeAddressBDM>(dt);
            }
            return models;
        }

        /// <summary>
        ///  B端商户拉取收货人地址缓存到本地 add By  caoheyang   20150702 
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public void RemoveAddressB(RemoveAddressBPM model)
        {
            const string deleteSql = @"delete from ReceviceAddress where Id = @AddressId and BusinessId= @BusinessId";
            IDbParameters dbParameters = DbHelper.CreateDbParameters();
            dbParameters.AddWithValue("AddressId", model.AddresssId);
            dbParameters.AddWithValue("BusinessId", model.BusinessId);
            DbHelper.ExecuteNonQuery(SuperMan_Write, deleteSql, dbParameters);
        }
    }
}
