﻿using Ets.Model.Common;
using Ets.Model.DomainModel.Area;
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
        /// 获取开通城市
        /// danny-20150327
        /// </summary>
        /// <returns></returns>
        Model.Common.ResultModel<List<AreaModel>> GetOpenCityInfo();
        /// <summary>
        /// 转换  易淘食 接口 中的  省市区 编码为国标码
        /// wc
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        AreaModel GetNationalAreaInfo(AreaModel from);

    }
}
