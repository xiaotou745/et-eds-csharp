using ETS.Const;
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
using ETS.Security;

namespace Ets.Service.Provider.Common
{
    public class AreaProvider : IAreaProvider
    {
        readonly AreaDao dao = new AreaDao();
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

            var redis = new ETS.NoSql.RedisCache.RedisCache();
             
            if (version.Trim().Equals(Config.ApiVersion))//客户端请求
            {
                areaList.Version = Config.ApiVersion;
                ///没有最新
                return ResultModel<AreaModelList>.Conclude(ETS.Enums.CityStatus.Newest, areaList);
            }
            else
            {
                redis.Delete(string.Format(RedissCacheKey.Ets_Service_Provider_Common_GetOpenCity, version));
            } 
            string key = string.Format(RedissCacheKey.Ets_Service_Provider_Common_GetOpenCity, version);
            var cacheValue = redis.Get<string>(key);
            if (!string.IsNullOrEmpty(cacheValue))
            {
                return ResultModel<AreaModelList>.Conclude(ETS.Enums.CityStatus.UnNewest, Letao.Util.JsonHelper.ToObject<AreaModelList>(cacheValue));
            }
            //取数据库
            IList<Model.DomainModel.Area.AreaModel> list = dao.GetOpenCitySql();
            areaList.Version = Config.ApiVersion;
            areaList.AreaModels = list;
            if (list != null)
            {
                redis.Add(key, Letao.Util.JsonHelper.ToJson(areaList));
            }
            return ResultModel<AreaModelList>.Conclude(ETS.Enums.CityStatus.UnNewest, areaList);
        }

        /// <summary>
        /// 获取开通城市
        /// danny-20150327
        /// </summary>
        /// <returns></returns>
        public Model.Common.ResultModel<List<AreaModel>> GetOpenCityInfo()
        {
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            string key = string.Format(RedissCacheKey.Ets_Service_Provider_Common_GetOpenCityInfo, Config.ApiVersion);
            //读取缓存
            var cacheValue = redis.Get<string>(key);
            // redis.Delete(key);
            if (!string.IsNullOrEmpty(cacheValue))
            {
                return ResultModel<List<AreaModel>>.Conclude(ETS.Enums.CityStatus.Newest, Letao.Util.JsonHelper.ToObject<List<AreaModel>>(cacheValue));

            }

            //取数据库
            List<AreaModel> list = dao.GetOpenCitySql().ToList();
            //CacheFactory.Instance.AddObject(key, list);
            if (list != null)
            {
                list = list.Where(i => i.JiBie == 2).ToList();
                redis.Add(key, Letao.Util.JsonHelper.ToJson(list));
            }
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
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            //List<AreaModel> list = new List<AreaModel>();
            string key = ETS.Const.RedissCacheKey.Common_GetNationalAreaInfo;
            var cacheValue = redis.Get<string>(key);
            List<AreaModelTranslate> cacheAreaModelList = null;
            if (!string.IsNullOrEmpty(cacheValue))
            {
                 cacheAreaModelList =Letao.Util.JsonHelper.ToObject<List<AreaModelTranslate>>(cacheValue);

            }
            else
            {
                cacheAreaModelList = dao.GetRegionInfo().ToList();
                redis.Add(key, Letao.Util.JsonHelper.ToJson(cacheAreaModelList));
            }
            //cacheAreaModelList = CacheFactory.Instance[key] as List<AreaModelTranslate>;
            //if (cacheAreaModelList == null) //为null的时候，取数据库
            //{
            //    cacheAreaModelList = dao.GetRegionInfo().ToList();
            //    redis.Add(key, Letao.Util.JsonHelper.ToJson(cacheAreaModelList));
            //    CacheFactory.Instance.AddObject(key, cacheAreaModelList);
            //}
            AreaModelTranslate areaModel = new AreaModelTranslate();
            AreaModelTranslate resultAreaModel = new AreaModelTranslate();
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

        /// <summary>
        /// 根据省市区名称获取对应的省市区编码 add by caoheyang 20150407
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        public string GetOpenCode(Ets.Model.ParameterModel.Area.ParaAreaNameInfo model)
        {
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            string key =MD5.Encrypt(string.Format("{0}_{1}_{2}",model.ProvinceName,model.CityName,model.AreaName).Replace(" ",""));
            string cacheValue = redis.Get<string>(key);
            if (string.IsNullOrWhiteSpace(cacheValue))
            {
                DMAreaCodeInfo tempModel = new AreaDao().GetOpenCodeSql(model);
                if (tempModel == null) 
                    return null;
                else if (tempModel.AreaIsOpen == 0 || tempModel.ProvinceIsOpen == 0 || tempModel.CityIsOpen == 0)
                    redis.Set(key, SystemConst.CityOpenInfo, DateTime.Now.AddDays(30));
                else 
                    redis.Set(key, string.Format("{0}_{1}_{2}",tempModel.ProvinceCode,tempModel.CityCode,tempModel.AreaCode)
                        ,DateTime.Now.AddDays(30));
            }
            return redis.Get<string>(key);
        }
    }
}
