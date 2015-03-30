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
            //根据版本号判断是否读取缓存
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

        public AreaModel GetNationalAreaInfo(AreaModel from)
        {
            AreaModel areaModel = new AreaModel();

            if (from != null)
            {
                string key = string.Format("Ets.Service.Provider.Common_GetNationalAreaInfo_{0}{1}", from.Name, from.Code);

            }



            return areaModel;
            
            
        }
    }
}
