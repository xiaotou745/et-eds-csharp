﻿using Ets.Model.Common;
using Ets.Model.ParameterModel.Order;
using ETS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ets.Model.Common.TaoBao;
namespace Ets.Service.IProvider.OpenApi
{
    /// <summary>
    /// 接口  第三方对接集团的基础业务,各集团都有的业务 add by caoheyang 20150326
    /// </summary>
    public interface IGroupProviderOpenApi
    {
        /// <summary>
        /// 回调第三方接口同步状态  add by caoheyang 20150326
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        OrderApiStatusType AsyncStatus(ParaModel<AsyncStatusPM_OpenApi> paramodel);

        /// <summary>
        /// 新增商铺时根据集团id为店铺设置外送费，结算比例等财务相关信息 add by caoheyang 20150417
        /// </summary>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        CreatePM_OpenApi SetCommissonInfo(CreatePM_OpenApi paramodel);

        /// <summary>
        ///  发布订单
        ///  胡灵波
        ///  2015年11月18日 13:14:19
        /// </summary>
        /// <param name="info"></param>
        void TaoBaoPushOrder(OrderDispatch p);

    }
}
