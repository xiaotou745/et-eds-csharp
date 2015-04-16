using Ets.Model.Common;
using Ets.Model.DomainModel.Area;
using Ets.Model.ParameterModel.Common;
using ETS.Data.PageData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Common
{
    public interface IAreaProvider
    {

        /// <summary>
        /// 获取开通城市的省市区
        /// 窦海超
        /// 2015年3月19日 17:09:53
        /// </summary>
        /// <param name="version">当前版本号</param>
        /// <returns></returns>
        ResultModel<AreaModelList> GetOpenCity(string version);
        /// <summary>
        /// 获取开通城市(只有市)
        /// danny-20150414
        /// </summary>
        /// <returns></returns>
        Model.Common.ResultModel<List<AreaModel>> GetOpenCityOfSingleCity();
        /// <summary>
        /// 根据用户传递的  省、市、区名称、级别（省1，市2，区3）,转换为 国标码
        /// 例如：用户传的是 Name:北京市,Code:1,级别:1，调用该方法返回：Name:北京市,Code:110000,级别:1
        /// 在查询不到的情况下，返回null
        /// wc
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        AreaModelTranslate GetNationalAreaInfo(AreaModelTranslate from);

        /// <summary>
        /// 根据省市区名称获取对应的省市区编码 add by caoheyang 20150407
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        string GetOpenCode(Ets.Model.ParameterModel.Area.ParaAreaNameInfo model);
        /// <summary>
        /// 获取开通城市列表
        /// danny-20150410
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        PageInfo<OpenCityModel> GetOpenCityList(OpenCitySearchCriteria criteria);
        /// <summary>
        ///  获取开放城市列表（非分页）
        /// danny-20150410
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        IList<OpenCityModel> GetOpenCityList(string cityName);
        /// <summary>
        /// 修改开通城市
        /// danny-20150413
        /// </summary>
        /// <param name="openCityCodeList"></param>
        /// <param name="closeCityCodeList"></param>
        /// <returns></returns>
        bool ModifyOpenCityByCode(string openCityCodeList, string closeCityCodeList);
        /// <summary>
        /// 修改开发城市后更新Redis缓存
        /// danny-20150413
        /// </summary>
        void ResetOpenCityListRedis();
    }
}
