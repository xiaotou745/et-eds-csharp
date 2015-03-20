using Ets.Model.DomainModel.Area;
using ETS;
using ETS.Dao;
using ETS.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Dao.Common
{
    public class AreaDao : DaoBase
    {
        /// <summary>
        /// 获取开通城市的省市区
        /// 窦海超
        /// 2015年3月19日 17:09:53
        /// </summary>
        /// <returns></returns>
        public IList<AreaModel> GetOpenCitySql()
        {
            string sql = @"SELECT 
                            p.code as ProvinceCode,
                            p.NAME AS ProvinceName,
                            c.code AS CityCode,
                            c.NAME AS CityName,
                            a.code AS AreaCode,
                            a.NAME AS AreaName
                             FROM PublicProvinceCity(nolock) AS p
                            left JOIN PublicProvinceCity(nolock) AS c ON p.code=c.parentid
                            LEFT JOIN PublicProvinceCity(nolock) AS a ON c.code=a.parentid 
                            WHERE p.code IN (" + Config.OpenCityCode + ")";
            DataSet ds = DbHelper.ExecuteDataset(SuperMan_Read, sql);
            return MapRows<AreaModel>(DataTableHelper.GetTable(ds));
        }
    }
}
