using ETS;
using ETS.Enums;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Order;
using SuperManCore;
using SuperManWebApi.Models.Business;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using SuperManCommonModel;
using Ets.Service.Provider.User;
using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
using Ets.Service.Provider.Common;
using Ets.Service.IProvider.User;
using ETS.Const;
using Ets.Model.Common;
using ETS.Enums;
using Ets.Model.ParameterModel.Bussiness;
using Ets.Model.ParameterModel.Order;
namespace SuperManWebApi.Controllers
{
    public class OrderController : ApiController
    {
        IOrderProvider iOrderProvider = new OrderProvider();
        IBusinessProvider iBusinessProvider = new BusinessProvider();
        /// <summary>
        /// 商户发布订单接口        
        /// </summary>
        /// <param name="model">订单数据</param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.PubOrderStatus))]
        [HttpPost]
        public ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel> Push(BussinessOrderInfoModel model)
        {          
            //验证该商户有无发布订单资格 
            if (!iBusinessProvider.HaveQualification(model.userId))
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(PubOrderStatus.HadCancelQualification);
            }
            if (model.Amount < 10m)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(PubOrderStatus.AmountLessThanTen);
            }
            if (model.Amount > 5000m)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(PubOrderStatus.AmountMoreThanFiveThousand);
            }           
            if (model.OrderCount <= 0 || model.OrderCount > 15) //判断录入订单数量是否符合要求
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(PubOrderStatus.OrderCountError);

            Ets.Model.DataModel.Order.order order = iOrderProvider.TranslateOrder(model);
            if (order.BusinessCommission < 10m) //商户结算比例不能小于10
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(PubOrderStatus.BusiSettlementRatioError);
            }
            string result = iOrderProvider.AddOrder(order);

            if (result == "0")//当前订单执行失败
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(PubOrderStatus.InvalidPubOrder);
            }
            Ets.Model.ParameterModel.Order.BusiOrderResultModel resultModel = new Ets.Model.ParameterModel.Order.BusiOrderResultModel { userId = model.userId };
            return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(PubOrderStatus.Success, resultModel);

        }


    }
}
