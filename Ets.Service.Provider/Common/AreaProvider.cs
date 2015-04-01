using Ets.Dao.Common;
using Ets.Model.Common;
using Ets.Model.DomainModel.Area;
using Ets.Service.IProvider.Common;
using ETS;
using ETS.Cacheing;
using ETS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.Common
{
    public class AreaProvider : IAreaProvider
    {
        AreaDao dao = new AreaDao();

        /// <summary>
        /// 获取开通城市的省市区
        /// 窦海超
        /// 2015年3月19日 17:09:53
        /// </summary>
        /// <param name="version">当前版本号</param>
        /// <returns></returns>
        public Model.Common.ResultModel<Model.DomainModel.Area.AreaModelList> GetOpenCity(string version)
        {
            AreaModelList areaList = new AreaModelList();

            if (version.Trim().Equals(Config.ApiVersion))//客户端请求
            {
                areaList.Version = Config.ApiVersion;
                ///没有最新
                return ResultModel<AreaModelList>.Conclude(ETS.Enums.CityStatus.Newest, areaList);
            }

            string key = string.Concat("Ets_Service_Provider_Common_GetOpenCity_", version);
            AreaModelList cacheAreaList = CacheFactory.Instance[key] as AreaModelList;
            if (cacheAreaList != null)
            {
                return ResultModel<AreaModelList>.Conclude(ETS.Enums.CityStatus.UnNewest, cacheAreaList);
            }

            //取数据库
            IList<Model.DomainModel.Area.AreaModel> list = dao.GetOpenCitySql();
            areaList.Version = Config.ApiVersion;
            areaList.AreaModels = list;
            CacheFactory.Instance.AddObject(key, areaList);

            return ResultModel<AreaModelList>.Conclude(ETS.Enums.CityStatus.UnNewest, areaList);
        }

        /// <summary>
        /// 获取开通城市
        /// danny-20150327
        /// </summary>
        /// <returns></returns>
        public Model.Common.ResultModel<List<AreaModel>> GetOpenCityInfo()
        {
            string key = string.Concat("Ets_Service_Provider_Common_GetOpenCity_New");
            //读取缓存
            List<AreaModel> cacheAreaList = CacheFactory.Instance[key] as List<AreaModel>;
            if (cacheAreaList != null)
            {
                return ResultModel<List<AreaModel>>.Conclude(ETS.Enums.CityStatus.Newest, cacheAreaList);
            }
            //取数据库
            List<AreaModel> list = dao.GetOpenCityInfoSql().ToList();
            CacheFactory.Instance.AddObject(key, list);
            return ResultModel<List<AreaModel>>.Conclude(ETS.Enums.CityStatus.Newest, list);
        }
        /// <summary>
        /// 根据用户传递的  省、市、区名称、级别（省1，市2，区3）,转换为 国标码
        /// 例如：用户传的是 Name:北京市,Code:1,级别:1，调用该方法返回：Name:北京市,Code:110000,级别:1
        /// 在查询不到的情况下，返回null
        /// wc
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public AreaModelTranslate GetNationalAreaInfo(AreaModelTranslate from)
        {
            AreaModelTranslate areaModel = new AreaModelTranslate();
            AreaModelTranslate resultAreaModel = new AreaModelTranslate();
            //List<AreaModel> list = new List<AreaModel>();
            string key = ETS.Const.RedissCacheKey.Common_GetNationalAreaInfo;

            List<AreaModelTranslate> cacheAreaModelList = CacheFactory.Instance[key] as List<AreaModelTranslate>;
            if (cacheAreaModelList == null) //为null的时候，取数据库
            {
                cacheAreaModelList = dao.GetRegionInfo().ToList();

                CacheFactory.Instance.AddObject(key, cacheAreaModelList);
            }
            if (from.JiBie == 2)
            {
                if (from.Name.Contains("北京")) { from.Name = "北京城区"; }
                if (from.Name.Contains("上海")) { from.Name = "上海城区"; }
            }
            areaModel = cacheAreaModelList.FirstOrDefault(s => s.Name == from.Name.Trim() && s.JiBie == from.JiBie);

            if (areaModel != null)
            {
                resultAreaModel.NationalCode = areaModel.NationalCode;
                //resultAreaModel.Name = areaModel.Name;
                //resultAreaModel.JiBie = from.JiBie;
            }
            else
            {
                resultAreaModel = null;
            }
            return resultAreaModel; 
        }
		
    }
}
