using ETS.Enums;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Order;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
using Ets.Model.Common;
using Ets.Model.DataModel.Business;
using ETS.Util;
using Ets.Model.ParameterModel.Clienter;
using Ets.Model.ParameterModel.Business;
using Ets.Model.DomainModel.Order;
using Ets.Service.IProvider.Clienter;
using Ets.Service.Provider.Clienter;
using SuperManWebApi.App_Start.Filters;
using SuperManWebApi.Providers;
using Ets.Service.IProvider.Business;
using Ets.Service.Provider.Business;
using Letao.Util;

namespace SuperManWebApi.Controllers
{
    [ExecuteTimeLog]// TODO:每个API的日志、异常之类
    public class OrderController : ApiController
    {
        readonly IOrderProvider iOrderProvider = new OrderProvider();
        readonly IBusinessProvider iBusinessProvider = new BusinessProvider();
        readonly IClienterProvider iClienterProvider = new ClienterProvider();
        readonly IOrderChildProvider iOrderChildProvider = new OrderChildProvider();
        readonly IReceviceAddressProvider receviceAddressProvider = new ReceviceAddressProvider();
        /// <summary>
        /// 商户发布订单   
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="model">订单参数实体</param>
        /// <returns></returns> 
        [Token]
        [HttpPost]
        public ResultModel<BusiOrderResultModel> Push(BussinessOrderInfoPM model)
        {
            try
            {
                //通过传过来的字符串序列化对象                
                model.listOrderChlid = ParseHelper.Deserialize<List<OrderChlidPM>>(model.OrderChlidJson);

                order order;
                ResultModel<BusiOrderResultModel> currResModel = Verification(model, out order);
                if (currResModel.Status == PubOrderStatus.VerificationSuccess.GetHashCode())
                {
                    PubOrderStatus cuStatus = iOrderProvider.AddOrder(order);
                    if (cuStatus == PubOrderStatus.Success)//当前订单执行成功
                    {
                        BusiOrderResultModel resultModel = new BusiOrderResultModel { userId = model.userId };
                        return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.Success, resultModel);
                    }
                    return ResultModel<BusiOrderResultModel>.Conclude(cuStatus);
                }
                return currResModel;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("ResultModel<BusiOrderResultModel> Push()方法出错", new { obj = "时间：" + DateTime.Now.ToString() + ex.Message });
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.InvalidPubOrder);
            }
        }

        /// <summary>
        /// 获取订单详情                
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="modelPM">订单参数实体</param>
        /// <returns></returns>        
        [HttpPost]
        public ResultModel<OrderDM> GetDetails(OrderPM modelPM)
        {
            #region 验证
            if (string.IsNullOrWhiteSpace(modelPM.Version)) //版本号 
            {
                return ResultModel<OrderDM>.Conclude(GetOrdersStatus.NoVersion);
            }
            if (modelPM.OrderId < 0)//订单Id不合法
            {
                return ResultModel<OrderDM>.Conclude(GetOrdersStatus.ErrOderNo);
            }
            if (!iOrderProvider.IsExist(modelPM.OrderId)) //订单不存在
            {
                return ResultModel<OrderDM>.Conclude(GetOrdersStatus.FailedGetOrders);
            }

            #endregion

            try
            {
                OrderDM orderDM = iOrderProvider.GetDetails(modelPM);
                return ResultModel<OrderDM>.Conclude(GetOrdersStatus.Success, orderDM);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(" ResultModel<OrderDM> GetDetails", new { obj = "时间：" + DateTime.Now.ToString() + ex.Message });
                return ResultModel<OrderDM>.Conclude(GetOrdersStatus.Failed);
            }
        }

        /// <summary>
        /// 上传小票信息，子订单
        /// wc
        /// </summary>
        /// <returns></returns>
        [Token]
        [HttpPost]
        [ApiVersionStatistic]
        public ResultModel<UploadReceiptResultModel> TicketUpload()
        {    
            #region 参数验证
            if (HttpContext.Current.Request.Form.Count == 0)
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.NOFormParameter);
            }     

            var orderId = ParseHelper.ToInt(HttpContext.Current.Request.Form["OrderId"], 0); //订单号
            if (orderId == 0) // 订单id
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.InvalidOrderId);
            }
            var clienterId = ParseHelper.ToInt(HttpContext.Current.Request.Form["ClienterId"], 0); //骑士的id
            if (clienterId == 0) // 骑士Id
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.ClienterIdInvalid);
            }
            var needUploadCount = ParseHelper.ToInt(HttpContext.Current.Request.Form["NeedUploadCount"], 1);
                //该订单总共需要上传的 小票数量
            var receiptPic = HttpContext.Current.Request.Form["ReceiptPicAddress"]; //小票地址更新时           
            var orderChildId = ParseHelper.ToInt(HttpContext.Current.Request.Form["OrderChildId"], 0); //子单号 
            if (orderChildId == 0) //子订单号
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.NoOrderChildId);
            }

            if (HttpContext.Current.Request.Files.Count == 0) //图片
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.InvalidFileFormat);
            }
            var file = HttpContext.Current.Request.Files[0]; //照片

            #endregion
           
            //上传图片
            ImgInfo imgInfo = new ImageHelper().UploadImg(file, orderId, ImageType.Receipt);
            var uploadReceiptModel = new UploadReceiptModel
            {
                OrderId = orderId,
                ClienterId = clienterId,
                OrderChildId = orderChildId,
                NeedUploadCount = needUploadCount,
                ReceiptPic = imgInfo.PicUrl
            };
            LogHelper.LogWriter("上传小票", new { orderId = orderId, clienterId = clienterId, orderChildId = orderChildId, ReceiptPic = imgInfo.PicUrl });
            var orderOther = iClienterProvider.UpdateClientReceiptPicInfo(uploadReceiptModel);
            if (orderOther == null)
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.UpFailed, new UploadReceiptResultModel() { OrderId = orderId });
            }
            else
            {
                List<OrderChildImg> listOrderChild = new List<OrderChildImg>();
                listOrderChild.Add(new OrderChildImg()
                {
                    OrderChildId = orderChildId, 
                    TicketUrl = ImageCommon.ReceiptPicConvert(uploadReceiptModel.ReceiptPic)
                });
                if (!string.IsNullOrWhiteSpace(receiptPic))  //当有地址的时候删除
                {
                    ImageHelper imgHelper = new ImageHelper();
                    imgHelper.DeleteTicket(receiptPic);
                }
                //上传成功后返回图片全路径
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.Success, new UploadReceiptResultModel() { OrderId = orderId, OrderChildList = listOrderChild, HadUploadCount = orderOther.HadUploadCount, NeedUploadCount = orderOther.NeedUploadCount });
            }
        }

        /// <summary>
        /// 骑士抢单
        /// </summary> 
        /// <returns></returns>
        [Token]
        [HttpPost]
        [ExecuteTimeLog]
        public ResultModel<RushOrderResultModel> Receive(OrderReceiveModel model)
        {
            #region 验证
            if (model.orderId <= 0) //订单号码非空验证
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.OrderEmpty);
            if (model.userId <= 0) //用户id验证
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.userIdEmpty);
            if (model.businessId <= 0)  //商户Id
            {
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.BussinessEmpty);
            }
            if (string.IsNullOrWhiteSpace(model.orderNo))
            {
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.OrderNoEmpty);
            }
            if (string.IsNullOrWhiteSpace(model.version))
            {
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.NoVersion);
            }
            #endregion

            return new ClienterProvider().Receive_C(model.userId, model.orderNo, model.businessId, model.Longitude, model.Latitude);
        }

        /// <summary>
        /// 骑士完成订单
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        [Token]
        public ResultModel<FinishOrderResultModel> Complete(OrderCompleteModel parModel)
        {
            if (parModel.userId <= 0)  //用户id非空验证 骑士Id
                return ResultModel<FinishOrderResultModel>.Conclude(FinishOrderStatus.UserIdEmpty);
            if (string.IsNullOrEmpty(parModel.orderNo)) //订单号码非空验证
                return ResultModel<FinishOrderResultModel>.Conclude(FinishOrderStatus.OrderEmpty);
            if (parModel.orderId <= 0) //订单Id
            {
                return ResultModel<FinishOrderResultModel>.Conclude(FinishOrderStatus.OrderIdEmpty);
            }
            if (string.IsNullOrWhiteSpace(parModel.version))
            {
                return ResultModel<FinishOrderResultModel>.Conclude(FinishOrderStatus.NoVersion);
            }

            FinishOrderResultModel finishModel = iClienterProvider.FinishOrder(parModel);
            return ResultModel<FinishOrderResultModel>.Conclude(finishModel.FinishOrderStatus, finishModel);
          
        }

        /// <summary>
        /// 查询子订单是否支付
        /// 窦海超
        /// 2015年5月17日 15:51:21
        /// </summary>
        /// <param name="orderId">主订单ID</param>
        /// <param name="childId">子订单ID</param>
        /// <returns>成功返回1，支付中未支付返回0</returns>
        [HttpPost]
        [Token]
        public ResultModel<PayStatusModel> GetChildPayStatus(OrderChildModel model)
        {
            return iOrderChildProvider.GetPayStatus(model.orderId, model.childId);
        }


        /// <summary>
        /// 骑士端获取任务列表（最新/最近）任务   add by caoheyang 20150519
        /// </summary>
        /// <param name="model">参数实体</param>
        /// <returns></returns>
        [HttpPost]
        public ResultModel<object> GetJobC(GetJobCPM model)
        {
            return iOrderProvider.GetJobC(model);
        }

        /// <summary>
        /// 确认取货
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150520</UpdateTime>
        /// <param name="modelPM"></param>
        /// <returns></returns>
        [Token]
        public ResultModel<string> ConfirmTake(OrderPM modelPM)
        {
            #region 验证
            if (string.IsNullOrWhiteSpace(modelPM.Version)) //版本号 
            {
                return ResultModel<string>.Conclude(ConfirmTakeStatus.NoVersion);
            }
            if (modelPM.OrderId <= 0)//订单Id不合法
            {
                return ResultModel<string>.Conclude(ConfirmTakeStatus.ErrId);
            }
            if (modelPM.ClienterId <= 0)  //骑士Id不合法
            {
                return ResultModel<string>.Conclude(ConfirmTakeStatus.ClienterIdEmpty);
            }
            #endregion

            try
            {
                iOrderProvider.UpdateTake(modelPM);
                return ResultModel<string>.Conclude(ConfirmTakeStatus.Success, "");
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(" ResultModel<string> ConfirmTake", new { obj = "时间：" + DateTime.Now.ToString() + ex.Message });
                return ResultModel<string>.Conclude(ConfirmTakeStatus.Failed);
            }
        }
        /// <summary>
        /// 一键发单修改地址和电话
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Token]
        [HttpPost]
        public ResultModel<string> UpdateOrderAddressAndPhone(NewAddressPM model)
        {
            if (string.IsNullOrEmpty(model.OrderId) ||
               string.IsNullOrEmpty(model.NewAddress) ||
               string.IsNullOrEmpty(model.NewPhone))
            {
                return ResultModel<string>.Conclude(OneKeyPubOrderUpdateStatus.ParamEmpty);
            }
            int result = iOrderProvider.UpdateOrderAddressAndPhone(model.OrderId, model.NewAddress, model.NewPhone);
            switch (result)
            {
                case -1:
                    return ResultModel<string>.Conclude(OneKeyPubOrderUpdateStatus.OnlyOneKeyPubOrder);
                case 0:
                    return ResultModel<string>.Conclude(OneKeyPubOrderUpdateStatus.Failed);
                case 1:
                default:
                    return ResultModel<string>.Conclude(OneKeyPubOrderUpdateStatus.Success);
            }
        }
        /// <summary>
        ///  B端商户拉取收货人地址缓存到本地 add By  caoheyang   20150702 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Token]
        [Validate]
        [ApiVersion]
        public ResultModel<object> ConsigneeAddressB([FromBody]ConsigneeAddressBPM model)
        {
            return receviceAddressProvider.ConsigneeAddressB(model);
        }

        /// <summary>
        ///  B端商户删除收货人地址 add By  caoheyang   20150702 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Token]
        [Validate]
        [ApiVersion]
        public ResultModel<object> RemoveAddressB([FromBody]RemoveAddressBPM model)
        {
            return receviceAddressProvider.RemoveAddressB(model);
        }

        #region 用户自定义方法
        /// <summary>
        /// 订单合法性验证
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150515</UpdateTime>
        /// <param name="model"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        ResultModel<BusiOrderResultModel> Verification(BussinessOrderInfoPM model, out  order order)
        {
            bool isOneKeyPubOrder = false;
            BussinessStatusModel buStatus = iBusinessProvider.GetUserStatus(model.userId);
            if (buStatus != null && buStatus.OneKeyPubOrder == 1)
                isOneKeyPubOrder = true;

            order = null;
            var version = model.Version;
            if (string.IsNullOrWhiteSpace(version)) //版本号 
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.NoVersion);
            }
            //if (!isOneKeyPubOrder && !StringHelper.CheckPhone(model.recevicePhone))
            //{
            //    return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.RecevicePhoneErr);
            //}
            if (!isOneKeyPubOrder && string.IsNullOrEmpty(model.recevicePhone))//手机号
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.RecevicePhoneIsNULL);
            }
            if (!isOneKeyPubOrder && string.IsNullOrEmpty(model.receviceAddress))
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.ReceviceAddressIsNULL);
            }

            if (!iBusinessProvider.HaveQualification(model.userId))//验证该商户有无发布订单资格 
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.HadCancelQualification);
            }
            int orderChileCount = model.listOrderChlid.Count;
            if (orderChileCount >= 16 || orderChileCount <= 0)
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.OrderCountError);
            }
            decimal amount = 0;
            for (int i = 0; i < orderChileCount; i++)//子订单价格
            {
                if (model.listOrderChlid[i].GoodPrice < 5m)
                {
                    return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.AmountLessThanTen);
                }
                if (model.listOrderChlid[i].GoodPrice > 1000m)
                {
                    return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.AmountMoreThanFiveThousand);
                }
                amount += model.listOrderChlid[i].GoodPrice;

            }
            if (model.Amount != amount)
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.AmountIsNotEqual);
            }

            if (model.OrderCount <= 0 || model.OrderCount > 15) //判断录入订单数量是否符合要求
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.OrderCountError);
            }
            if (model.OrderCount != model.listOrderChlid.Count)//主订单与子订单数量
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.CountIsNotEqual);
            }
            BusListResultModel business = null;
            //= iBusinessProvider.GetBusiness(model.userId)

            order = iOrderProvider.TranslateOrder(model, out business);
            if (order.CommissionType == OrderCommissionType.FixedRatio.GetHashCode() && order.BusinessCommission < 10m) //商户结算比例不能小于10
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.BusiSettlementRatioError);
            }

            if (business == null) //如果商户不允许可透支发单，验证余额是否满足结算费用，如果不满足，提示：“您的余额不足，请及时充值!”
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.BusinessEmpty);  //未取到商户信息
            }
            if (buStatus.IsAllowOverdraft == 0) //0不允许透支
            {
                if (business.BalancePrice < order.SettleMoney)
                {
                    return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.BusiBalancePriceLack);
                }
            }
            return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.VerificationSuccess);
        }
        #endregion
    }
}
