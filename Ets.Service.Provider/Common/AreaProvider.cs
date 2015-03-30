﻿using Ets.Dao.Common;
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

        #region 验证开通城市是否取读绑定
        /// <summary>
        /// config里的版本号
        /// </summary>
        private string OpenCityVersion = Config.OpenCityVersion;

        /// <summary>
        /// 是否是最新版本
        /// </summary>
        private bool CheckSystemCityVersion(string CurrentSystemCityVersion)
        {
            if (OpenCityVersion == CurrentSystemCityVersion)
            {
                return true;
            }
            return false;
        }
        #endregion

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

            if (version.Trim().Equals(OpenCityVersion))//客户端请求
            {
                ///没有最新
                return ResultModel<AreaModelList>.Conclude(ETS.Enums.CityStatus.UnNewest, areaList);
            }

            string key = string.Concat("Ets_Service_Provider_Common_GetOpenCity");
            if (CheckSystemCityVersion(version))
            {
                //根据版本号判断是否读取缓存
                AreaModelList cacheAreaList = CacheFactory.Instance[key] as AreaModelList;
                if (areaList != null)
                {
                    return ResultModel<AreaModelList>.Conclude(ETS.Enums.CityStatus.Newest, cacheAreaList);
                }
            }

            //取数据库
            IList<Model.DomainModel.Area.AreaModel> list = dao.GetOpenCitySql();
            areaList.Version = OpenCityVersion;
            areaList.AreaModels = list;
            CacheFactory.Instance.AddObject(key, areaList);

            return ResultModel<AreaModelList>.Conclude(ETS.Enums.CityStatus.Newest, areaList);
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
        public AreaModel GetNationalAreaInfo(AreaModel from)
        {
            AreaModel areaModel = new AreaModel();
            AreaModel resultAreaModel = new AreaModel();
            List<AreaModel> list = new List<AreaModel>();
            string key = "Ets.Service.Provider.Common_GetNationalAreaInfo";
        
            List<AreaModel> cacheAreaModelList = CacheFactory.Instance[key] as List<AreaModel>;
            if (cacheAreaModelList == null) //为null的时候，取数据库
            {
                list = dao.GetOpenCityInfoSql().ToList();
                areaModel = list.FirstOrDefault(s => s.Name == from.Name.Trim() && s.JiBie == from.JiBie);
                CacheFactory.Instance.AddObject(key, list);
            }
            else
            { 
                areaModel = cacheAreaModelList.FirstOrDefault(s => s.Name == from.Name.Trim() && s.JiBie == from.JiBie);
            }


            if (areaModel != null)
            {
                resultAreaModel.Code = areaModel.Code;
                resultAreaModel.Name = areaModel.Name;
                resultAreaModel.JiBie = from.JiBie;
            }
            else
            {
                resultAreaModel = null;
            }
            return resultAreaModel;
            
            
        }
    }
}
