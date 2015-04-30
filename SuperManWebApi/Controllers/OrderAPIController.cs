using SuperManCore;
using SuperManCore.Common;
using SuperManWebApi.Models;
using SuperManWebApi.Models.Business;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using SuperManDataAccess;
using SuperManBusinessLogic.B_Logic;
using SuperManBusinessLogic.Order_Logic;
using SuperManCommonModel.Models;
using SuperManCore.Paging;
using System.Threading.Tasks;
using SuperManCommonModel;
using System.Text;
using System.Net;
using SuperManCommonModel.Entities;

namespace SuperManWebApi.Controllers
{
    public class OrderAPIController : ApiController
    {
        /// <summary>
        /// 根据时间段获取该段时间内已抢单的需要打印的订单信息
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="isPrint"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionStatus(typeof(GetRushOrderInfoStatus))]        
        public ResultModel<OrderInfoPrint[]> GetRushOrderInfo(DateTime startTime,DateTime endTime,int isPrint=0)
        {
            //单号、人名、电话、门店信息以便打印订单
            if (startTime != null && endTime != null && DateTime.Compare(endTime, startTime) > 0)  
            {
                List<OrderInfoPrint> orderPrintInfo = OrderLogic.orderLogic().GetRushOrderInfo(startTime, endTime, isPrint);

                return ResultModel<OrderInfoPrint[]>.Conclude
                    (GetRushOrderInfoStatus.Success, orderPrintInfo.ToArray());
            }
            else
            {
                return ResultModel<OrderInfoPrint[]>.Conclude
                    (GetRushOrderInfoStatus.ParamError);
            }

        }
        /// <summary>
        /// 更新订单的打印状态为已经打印
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionStatus(typeof(GetRushOrderInfoStatus))]
        public ResultModel<bool> UpdateOrderIsPrint(int orderId)
        {
            if (orderId > 0)
            {
                bool b = OrderLogic.orderLogic().UpdateOrderIsPrint(orderId);
                return ResultModel<bool>.Conclude
                   (GetRushOrderInfoStatus.Success,b);
            }
            else
            {
                return ResultModel<bool>.Conclude
                   (GetRushOrderInfoStatus.ParamError);
            }
        }
    }
}