using Ets.Model.DomainModel.Area;
using ETS;
using ETS.Dao;
using ETS.Extension;
using ETS.Util;
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
        /// <summary>
        /// 获取开通城市
        /// danny-20150327
        /// </summary>
        /// <returns></returns>
        public IList<AreaModel> GetOpenCityInfoSql()
        {

            string sql = string.Format(@" SELECT   a.Code ,
                        a.Name ,
                        a.ParentId ,
                        JiBie = 1
               FROM     dbo.PublicProvinceCity a ( NOLOCK )
               WHERE    a.ParentId = 0
                        AND a.Code IN ( {0} )
            ", ConfigSettings.Instance.OpenCityCode);
            DataSet ds = DbHelper.ExecuteDataset(SuperMan_Read, sql);
            return MapRows<AreaModel>(DataTableHelper.GetTable(ds));
        }


        /// <summary>
        /// 根据开放城市获取 region 表中的信息,主要是国标码 
        /// wc 
        /// </summary>
        /// <returns></returns>
        public IList<AreaModelTranslate> GetRegionInfo()
        {

            string sql = string.Format(@"
 WITH   t AS ( SELECT   a.NationalCode ,
                        a.Name ,
                        a.Code ,
                        a.Id ,
                        JiBie = 1
               FROM     dbo.region a ( NOLOCK )
               WHERE    a.Depth = 2
                        AND a.NationalCode IN ( {0} )
             )
    SELECT  t.NationalCode ,
            t.Name ,
            t.Code ,
            t.JiBie
    FROM    t
    UNION
    SELECT  b.NationalCode ,
            b.Name ,
            b.Code ,
            JiBie = 2
    FROM    t
            LEFT JOIN dbo.region (NOLOCK) AS b ON t.Id = b.Fid
    UNION
    SELECT  c.NationalCode ,
            c.Name ,
            c.Code ,
            JiBie = 3
    FROM    ( SELECT    b.NationalCode ,
                        b.Name ,
                        b.Id ,
                        JiBie = 2
              FROM      t
                        LEFT JOIN dbo.region (NOLOCK) AS b ON t.Id = b.Fid
            ) t1
            LEFT JOIN dbo.region (NOLOCK) AS c ON t1.Id = c.Fid", Config.OpenCityCode);


            DataSet ds = DbHelper.ExecuteDataset(SuperMan_Read, sql);
            return MapRows<AreaModelTranslate>(DataTableHelper.GetTable(ds));
        }
    }
}
