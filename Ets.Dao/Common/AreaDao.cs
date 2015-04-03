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

            string sql = @"
with   t as ( select   a.code ,
                        a.name ,
                        a.parentid ,
                        JiBie = 1
               from     dbo.PublicProvinceCity a ( nolock )
               where    a.parentid = 0
                        --and a.code in ( 110000, 310000, 440000 )
                        and a.IsPublic = 1
             )
    select  t.code ,
            t.name ,
            t.parentid ,
            t.JiBie
    from    t
    union
    select  b.code ,
            b.name ,
            b.parentid ,
            JiBie = 2
    from    t
            left join PublicProvinceCity (nolock) as b on t.code = b.parentid
    where   b.IsPublic = 1
    union
    select  c.code ,
            c.name ,
            c.parentid ,
            JiBie = 3
    from    ( select    b.code ,
                        b.name ,
                        b.parentid ,
                        JiBie = 2
              from      t
                        left join PublicProvinceCity (nolock) as b on t.code = b.parentid
              where     b.IsPublic = 1
            ) t1
            left join PublicProvinceCity (nolock) as c on t1.code = c.parentid
    where   c.IsPublic = 1";





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
