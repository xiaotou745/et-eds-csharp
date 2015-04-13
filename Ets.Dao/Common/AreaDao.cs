using Ets.Model.Common;
using Ets.Model.DomainModel.Area;
using Ets.Model.ParameterModel.Common;
using ETS;
using ETS.Dao;
using ETS.Data.Core;
using ETS.Data.PageData;
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
        /// 根据省市区名称获取对应的省市区编码 add by caoheyang 20150407
       /// </summary>
       /// <param name="model">参数实体</param>
       /// <returns></returns>
        public DMAreaCodeInfo GetOpenCodeSql(Ets.Model.ParameterModel.Area.ParaAreaNameInfo model)
        {
            string sql = @"
select  p.code as ProvinceCode,
        p.IsPublic as ProvinceIsOpen,
        c.code as CityCode,
        c.IsPublic as CityIsOpen,
        a.code as AreaCode,
        a.IsPublic as AreaIsOpen
from    PublicProvinceCity (nolock) as p
        left join PublicProvinceCity (nolock) as c on p.code = c.parentid
        left join PublicProvinceCity (nolock) as a on c.code = a.parentid
where   p.name =@ProvinceName 
        and c.name =@CityName
        and a.name=@AreaName";
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.Add("@ProvinceName", SqlDbType.NVarChar);
            parm.SetValue("@ProvinceName", model.ProvinceName);
            parm.Add("@CityName", SqlDbType.NVarChar);
            parm.SetValue("@CityName", model.CityName);
            parm.AddWithValue("@AreaName", SqlDbType.NVarChar);
            parm.SetValue("@AreaName",model.AreaName);
            DataTable dt = DbHelper.ExecuteDataTable(SuperMan_Read, sql, parm);
            if (dt == null || dt.Rows.Count <= 0)
                return null;
            return MapRows<DMAreaCodeInfo>(dt)[0];
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
        /// <summary>
        /// 获取开放城市列表（非分页）
        /// danny-20150410
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public IList<OpenCityModel> GetOpenCityList(string cityName)
        {
            StringBuilder sql =new StringBuilder( @"SELECT p.code ProvinceCode
                                    ,p.name ProvinceName
                                    ,c.code CityCode
                                    ,c.name CityName
                                    ,d.code DistrictCode
                                    ,d.name DistrictName
                                    ,d.IsPublic
                            FROM PublicProvinceCity d WITH(NOLOCK)
                                JOIN PublicProvinceCity c WITH(NOLOCK) ON d.parentid=c.code
                                JOIN PublicProvinceCity p WITH(NOLOCK) ON c.parentid=p.code WHERE 1=1 ");
            if (!string.IsNullOrWhiteSpace(cityName))
            {
                sql.AppendFormat(" AND c.name like '%{0}%' ", cityName);
            }
            DataSet ds = DbHelper.ExecuteDataset(SuperMan_Read, sql.ToString());
            return MapRows<OpenCityModel>(DataTableHelper.GetTable(ds));
        }
        /// <summary>
        /// 获取开通城市列表
        /// danny-20150410
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public PageInfo<T> GetOpenCityList<T>(OpenCitySearchCriteria criteria)
        {
            string columnList = @"   p.code ProvinceCode
                                    ,p.name ProvinceName
                                    ,c.code CityCode
                                    ,c.name CityName
                                    ,d.code DistrictCode
                                    ,d.name DistrictName
                                    ,d.IsPublic ";
            var sbSqlWhere = new StringBuilder(" 1=1 AND d.IsPublic=1 ");
            if(!string.IsNullOrWhiteSpace(criteria.CityName))
            {
                sbSqlWhere.AppendFormat(" AND c.name like '%{0}%' ", criteria.CityName);
            }
            string tableList = @" PublicProvinceCity d WITH(NOLOCK)
                                JOIN PublicProvinceCity c WITH(NOLOCK) ON d.parentid=c.code
                                JOIN PublicProvinceCity p WITH(NOLOCK) ON c.parentid=p.code   ";
            string orderByColumn = " d.code ";
            return new PageHelper().GetPages<T>(SuperMan_Read, criteria.PageIndex, sbSqlWhere.ToString(), orderByColumn, columnList, tableList, criteria.PageSize, true);
        }

        /// <summary>
        /// 修改开通城市
        /// danny-20150413
        /// </summary>
        /// <param name="openCityCodeList"></param>
        /// <returns></returns>
        public bool ModifyOpenCityByCode(string openCityCodeList, string closeCityCodeList)
        {
            string sql = "";
            if(!string.IsNullOrWhiteSpace(openCityCodeList))
            {
                int code = Convert.ToInt32(openCityCodeList.Split(',')[0].ToString());
                sql += @"UPDATE PublicProvinceCity SET IsPublic=1 WHERE code IN(@openCityCodeList);";
                sql +=string.Format(@"UPDATE PublicProvinceCity 
                          SET IsPublic=1 
                          WHERE code=(
                                      SELECT parentid 
                                        FROM PublicProvinceCity 
                                        WHERE code={0});
                        UPDATE PublicProvinceCity 
                          SET IsPublic=1 
                          WHERE code=(
                                      SELECT c.parentid  
                                        FROM  PublicProvinceCity d WITH(NOLOCK)
                                          JOIN PublicProvinceCity c ON c.code=d.parentid
                                        WHERE d.code={0});",code);
            }
            if (!string.IsNullOrWhiteSpace(closeCityCodeList))
            {
                sql += @"UPDATE PublicProvinceCity SET IsPublic=0 WHERE code IN(@closeCityCodeList);";
            }
            IDbParameters parm = DbHelper.CreateDbParameters();
            parm.AddWithValue("@openCityCodeList", openCityCodeList);
            parm.AddWithValue("@closeCityCodeList", closeCityCodeList);
            return DbHelper.ExecuteNonQuery(SuperMan_Write, sql, parm) > 0 ? true : false;
        }
    }
}
