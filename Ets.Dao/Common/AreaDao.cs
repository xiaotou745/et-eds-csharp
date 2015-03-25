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
//            string sql = @"SELECT 
//                            p.code as ProvinceCode,
//                            p.NAME AS ProvinceName,
//                            c.code AS CityCode,
//                            c.NAME AS CityName,
//                            a.code AS AreaCode,
//                            a.NAME AS AreaName
//                             FROM PublicProvinceCity(nolock) AS p
//                            left JOIN PublicProvinceCity(nolock) AS c ON p.code=c.parentid
//                            LEFT JOIN PublicProvinceCity(nolock) AS a ON c.code=a.parentid 
//                            WHERE p.code IN (" + Config.OpenCityCode + ")";

            string sql = string.Format(@"
WITH    t AS ( SELECT   a.Code ,
                        a.Name ,
                        a.ParentId ,
                        JiBie = 1
               FROM     dbo.PublicProvinceCity a ( NOLOCK )
               WHERE    a.ParentId = 0
                        AND a.Code IN ( {0} )
             )
    SELECT  t.Code ,
            t.Name ,
            t.ParentId ,
            t.JiBie
    FROM    t
    UNION
    SELECT  b.Code ,
            b.Name ,
            b.ParentId ,
            JiBie = 2
    FROM    t
            LEFT JOIN PublicProvinceCity (NOLOCK) AS b ON t.Code = b.ParentId
    UNION
    SELECT  c.Code ,
            c.Name ,
            c.ParentId ,
            JiBie = 3
    FROM    ( SELECT    b.Code ,
                        b.Name ,
                        b.ParentId ,
                        JiBie = 2
              FROM      t
                        LEFT JOIN PublicProvinceCity (NOLOCK) AS b ON t.Code = b.ParentId
            ) t1
            LEFT JOIN PublicProvinceCity (NOLOCK) AS c ON t1.Code = c.ParentId", Config.OpenCityCode);





            DataSet ds = DbHelper.ExecuteDataset(SuperMan_Read, sql);
            return MapRows<AreaModel>(DataTableHelper.GetTable(ds));
        }
    }
}
