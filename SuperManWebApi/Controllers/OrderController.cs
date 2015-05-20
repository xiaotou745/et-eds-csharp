using ETS.Enums;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Order;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;
using Ets.Service.Provider.User;
using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
using Ets.Service.IProvider.User;
using ETS.Const;
using Ets.Model.Common;
using ETS.Util;
using Ets.Model.ParameterModel.Clienter;
using ETS.Expand;
using Ets.Model.ParameterModel.Bussiness;
using Ets.Model.DomainModel.Order;
using Ets.Service.IProvider.Clienter;
using Ets.Service.Provider.Clienter;
using Ets.Model.DomainModel.Clienter;
using SuperManWebApi.App_Start.Filters;
using System.Text.RegularExpressions;
using SuperManWebApi.Providers;
using Ets.Model.Common.AliPay;
using System.Text;
using System.Runtime.Serialization.Json;

namespace SuperManWebApi.Controllers
{
    [ExecuteTimeLog]
    /// <summary>
    /// TODO:每个API的日志、异常之类
    /// </summary>
    public class OrderController : ApiController
    {
        IOrderProvider iOrderProvider = new OrderProvider();
        IBusinessProvider iBusinessProvider = new BusinessProvider();
        readonly IClienterProvider iClienterProvider = new ClienterProvider();
        IOrderChildProvider iOrderChildProvider = new OrderChildProvider();
        /// <summary>
        /// 商户发布订单   
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="model">订单参数实体</param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.PubOrderStatus))]
        [HttpPost]
        public ResultModel<BusiOrderResultModel> Push(BussinessOrderInfoPM model)
        {
            try
            {           
                //通过传过来的字符串序列化对象                
                //model.listOrderChlid = Deserialize<List<OrderChlidPM>>(model.OrderChlidJson);             
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
                }

                return currResModel;
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("ResultModel<BusiOrderResultModel> Push()方法出错", new { obj = "时间：" + DateTime.Now.ToString() + ex.Message });
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.InvalidPubOrder);
            }
        }

        private T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }
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
            order = null;
            var version = model.Version;
            if (string.IsNullOrWhiteSpace(version)) //版本号 
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.NoVersion);
            }
            if (string.IsNullOrEmpty(model.recevicePhone))//手机号
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.RecevicePhoneIsNULL);
            }           
            Regex dReg = new Regex("^1\\d{10}$");
            if (!dReg.IsMatch(model.recevicePhone))//验证收货人手机号
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.RecevicePhoneErr);
            }
            if (string.IsNullOrEmpty(model.receviceAddress))
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.ReceviceAddressIsNULL);
            }     

            if (!iBusinessProvider.HaveQualification(model.userId))//验证该商户有无发布订单资格 
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.HadCancelQualification);
            }

            decimal amount = 0;
            for (int i = 0; i < model.listOrderChlid.Count; i++)//子订单价格
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

            order = iOrderProvider.TranslateOrder(model);
            if (order.CommissionType == OrderCommissionType.FixedRatio.GetHashCode() && order.BusinessCommission < 10m) //商户结算比例不能小于10
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.BusiSettlementRatioError);
            }

            return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.VerificationSuccess);
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
        [HttpPost]
        [ApiVersionStatistic]
        public ResultModel<UploadReceiptResultModel> TicketUpload()
        {
            if (HttpContext.Current.Request.Form.Count == 0)
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.NOFormParameter);
            }
            var orderId = ParseHelper.ToInt(HttpContext.Current.Request.Form["OrderId"], 0); //订单号
            var clienterId = ParseHelper.ToInt(HttpContext.Current.Request.Form["ClienterId"], 0); //骑士的id
            var needUploadCount = ParseHelper.ToInt(HttpContext.Current.Request.Form["NeedUploadCount"], 1); //该订单总共需要上传的 小票数量
            var receiptPic = HttpContext.Current.Request.Form["ReceiptPicAddress"];  //小票地址更新时
            var version = HttpContext.Current.Request.Form["Version"]; //版本号  1.0 
            var orderChildId = ParseHelper.ToInt(HttpContext.Current.Request.Form["OrderChildId"], 0); //子单号
            if (clienterId == 0) // 骑士Id
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.ClienterIdInvalid);
            }
            if (orderId == 0)  // 订单id
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.InvalidOrderId);
            }
            if (string.IsNullOrWhiteSpace(version))  //版本号
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.NoVersion);
            }
            if (orderChildId == 0) //子订单号
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.NoOrderChildId);
            }
            if (HttpContext.Current.Request.Files.Count == 0)  //图片
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.InvalidFileFormat);
            }
            var file = HttpContext.Current.Request.Files[0]; //照片

            #region 暂时注释

            //System.Drawing.Image img;
            //try
            //{
            //    img = System.Drawing.Image.FromStream(file.InputStream);
            //}
            //catch (Exception)
            //{
            //    return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidFileFormat);
            //}
            //var fileName = ETS.Util.ImageTools.GetFileName(Path.GetExtension(file.FileName));
            //int fileNameLastDot = fileName.LastIndexOf('.');
            ////原图 
            //string rFileName = string.Format("{0}{1}{2}", fileName.Substring(0, fileNameLastDot), ImageConst.OriginSize, Path.GetExtension(fileName));

            //string saveDbFilePath;

            //string fullFileDir = ETS.Util.ImageTools.CreateDirectory(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath, orderId.ToString(), out saveDbFilePath);

            //if (fullFileDir == "0")
            //{ 
            //    return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.UpFailed);
            //}
            ////保存原图
            //var fullFilePath = Path.Combine(fullFileDir, rFileName);

            //file.SaveAs(fullFilePath);

            ////裁图
            //var transformer = new SuperManCore.FixedDimensionTransformerAttribute(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.Width, Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.Height, CustomerIconUploader.Instance.MaxBytesLength / 1024);
            ////保存到数据库的图片路径
            //var destFullFileName = System.IO.Path.Combine(fullFileDir, fileName);
            //transformer.Transform(fullFilePath, destFullFileName); 
            //var picUrl = saveDbFilePath + fileName; 

            #endregion
            //上传图片
            ImgInfo imgInfo = new ImageHelper().UploadImg(file, orderId);
            var uploadReceiptModel = new UploadReceiptModel
            {
                OrderId = orderId,
                ClienterId = clienterId,
                OrderChildId = orderChildId,
                NeedUploadCount = needUploadCount,
                ReceiptPic = imgInfo.PicUrl
            };
            if (string.IsNullOrWhiteSpace(receiptPic))  
            {
                uploadReceiptModel.HadUploadCount = 1;
            }
            else
            {
                uploadReceiptModel.HadUploadCount = 0;  //有地址说明是更新换的小票，数量不变
            }
            var orderOther = iClienterProvider.UpdateClientReceiptPicInfo(uploadReceiptModel);
            if (orderOther == null)
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.UpFailed, new UploadReceiptResultModel() { OrderId = orderId });
            }
            else
            {
                List<string> listReceiptPic = ImageCommon.ReceiptPicConvert(uploadReceiptModel.ReceiptPic);
                List<OrderChildImg> listOrderChild = new List<OrderChildImg>();
                listOrderChild.Add(new OrderChildImg() { OrderChildId = orderChildId, TicketUrl = listReceiptPic[0] });
                if (!string.IsNullOrWhiteSpace(uploadReceiptModel.ReceiptPic))  //当有地址的时候删除
                {
                    ImageHelper imgHelper = new ImageHelper();
                    imgHelper.DeleteTicket(uploadReceiptModel.ReceiptPic);
                }
                //上传成功后返回图片全路径
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.Success, new UploadReceiptResultModel() { OrderId = orderId, OrderChildList = listOrderChild, HadUploadCount = orderOther.HadUploadCount, NeedUploadCount = orderOther.NeedUploadCount });
            }
        }

        /// <summary>
        /// 删除小票信息
        /// wc
        /// </summary>
        /// <param name="Version"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersionStatistic]
        public ResultModel<UploadReceiptResultModel> TicketRemove()
        {
            if (HttpContext.Current.Request.Form.Count == 0)
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.NOFormParameter);
            }
            var orderId = ParseHelper.ToInt(HttpContext.Current.Request.Form["OrderId"], 0); //订单Id
            var orderChildId = ParseHelper.ToInt(HttpContext.Current.Request.Form["OrderChildId"], 0); //子订单Id
            var receiptPic = HttpContext.Current.Request.Form["ReceiptPicAddress"];  //小票上传地址
            var version = HttpContext.Current.Request.Form["Version"]; //版本号  1.0
            if (orderId == 0)
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.InvalidOrderId);
            if (orderChildId == 0)
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.NoOrderChildId);
            if (string.IsNullOrWhiteSpace(version)) //版本号 
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.NoVersion);
            if (!receiptPic.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.ReceiptAddressInvalid);
            UploadReceiptModel uploadReceiptModel = new UploadReceiptModel() { OrderId = orderId, ReceiptPic = receiptPic, HadUploadCount = -1 };
            SuperManCore.LogHelper.LogWriter("删除小票参数", new { version = version, receiptPic = receiptPic, orderId = orderId, orderChildId = orderChildId });
            //删除前先判断   订单状态已完成 和已经上传的小票数量 等于需要上传的小票数量相等 时 不允许删除小票
            OrderOther orderOther = iClienterProvider.GetReceipt(orderId);
            //判断订单信息，状态是否为已完成， 已经上传的小票数量是否和 需要上传的一样， 若一样则无法删除
            if (orderOther != null)
            {
                if (orderOther.OrderStatus == ConstValues.ORDER_FINISH && orderOther.NeedUploadCount == orderOther.HadUploadCount)
                {
                    return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.DeleteFailed);
                }
            }
            else
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.CannotFindOrder);
            }
            //判断是否存在小票 
            List<OrderChildForTicket> orderChild = iClienterProvider.GetOrderChildInfo(orderId, orderChildId);
            if (orderChild != null && orderChild.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(orderChild[0].TicketUrl) || orderChild[0].HasUploadTicket)  //没有小票，请先上传
                {
                    return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.FirstUpload);
                }
            }
            else
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.DeleteFailed);
            }
            //删除小票的时候更新orderother表，更新 orderchild表
            OrderOther delOrderOther = iClienterProvider.DeleteReceipt(uploadReceiptModel);
            if (delOrderOther.Id > 0)
            {
                //List<string> listReceiptPic = ImageCommon.ReceiptPicConvert(delOrderOther.ReceiptPic); 
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.Success, new UploadReceiptResultModel() { OrderId = delOrderOther.OrderId, HadUploadCount = delOrderOther.HadUploadCount, NeedUploadCount = delOrderOther.NeedUploadCount });
            }
            else
            {
                return ResultModel<UploadReceiptResultModel>.Conclude(UploadIconStatus.DeleteFailed);
            }
        }

        /// <summary>
        /// 骑士抢单
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        [ExecuteTimeLog]
        public ResultModel<RushOrderResultModel> Receive()
        {
            var userId = ParseHelper.ToInt(HttpContext.Current.Request.Form["userId"], 0);   //骑士ID
            var orderNo = HttpContext.Current.Request.Form["orderNo"];
            var bussinessId = ParseHelper.ToInt(HttpContext.Current.Request.Form["bussinessId"], 0);
            var version = HttpContext.Current.Request.Form["version"];
            float grabLongitude = 0, grabLatitude = 0;
            if(!string.IsNullOrEmpty(HttpContext.Current.Request.Form["Longitude"]))
                grabLongitude =float.Parse( HttpContext.Current.Request.Form["Longitude"]);
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["Latitude"]))
                grabLatitude = float.Parse( HttpContext.Current.Request.Form["Latitude"]);

            if (string.IsNullOrEmpty(orderNo)) //订单号码非空验证
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.OrderEmpty);
            if (userId <= 0) //用户id验证
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.userIdEmpty);
            if (bussinessId <= 0)
            {
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.userIdEmpty);
            }
            if (string.IsNullOrWhiteSpace(version))
            {
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.NoVersion);
            }
            return new ClienterProvider().Receive_C(userId, orderNo, bussinessId,grabLongitude, grabLatitude);
        }

        /// <summary>
        /// 骑士完成订单
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public ResultModel<FinishOrderResultModel> Complete()
        {
            var userId = ParseHelper.ToInt(HttpContext.Current.Request.Form["userId"], 0);   //骑士ID
            var orderNo = HttpContext.Current.Request.Form["orderNo"];
            var pickupCode = HttpContext.Current.Request.Form["pickupCode"];
            var version = HttpContext.Current.Request.Form["version"];
            float completeLongitude = 0, completeLatitude = 0;
            if(!string.IsNullOrEmpty(HttpContext.Current.Request.Form["Longitude"]))
                completeLongitude = float.Parse(HttpContext.Current.Request.Form["Longitude"]);
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["Latitude"]))
                completeLatitude = float.Parse(HttpContext.Current.Request.Form["Latitude"]);
            if (userId == 0)  //用户id非空验证
                return ResultModel<FinishOrderResultModel>.Conclude(ETS.Enums.FinishOrderStatus.UserIdEmpty);
            if (string.IsNullOrEmpty(orderNo)) //订单号码非空验证
                return ResultModel<FinishOrderResultModel>.Conclude(ETS.Enums.FinishOrderStatus.OrderEmpty);
            if (string.IsNullOrWhiteSpace(version))
            {
                return ResultModel<FinishOrderResultModel>.Conclude(FinishOrderStatus.NoVersion);
            }
            var myorder = new Ets.Dao.Order.OrderDao().IsOrNotFinish(orderNo);
            if (myorder)
            {
                return ResultModel<FinishOrderResultModel>.Conclude(FinishOrderStatus.ExistNotPayChildOrder);
            }
            string finishResult = iClienterProvider.FinishOrder(userId, orderNo, completeLongitude, completeLatitude, pickupCode);
            if (finishResult == "1")  //完成
            {
                var clienter = iClienterProvider.GetUserInfoByUserId(userId);
                var model = new FinishOrderResultModel { userId = userId };
                if (clienter.AccountBalance != null)
                    model.balanceAmount = clienter.AccountBalance.Value;
                else
                    model.balanceAmount = 0.0m;
                return ResultModel<FinishOrderResultModel>.Conclude(ETS.Enums.FinishOrderStatus.Success, model);
            }
            else if (finishResult == "3")
            {
                return ResultModel<FinishOrderResultModel>.Conclude(ETS.Enums.FinishOrderStatus.OrderHadCancel);
            }
            else if (finishResult == ETS.Enums.FinishOrderStatus.PickupCodeError.ToString())
                return ResultModel<FinishOrderResultModel>.Conclude(ETS.Enums.FinishOrderStatus.PickupCodeError);
            else
            {
                return ResultModel<FinishOrderResultModel>.Conclude(ETS.Enums.FinishOrderStatus.Failed);
            }
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
        public ResultModel<PayResultModel> GetChildPayStatus(OrderChildModel model)
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
        public ResultModel<string> ConfirmTake(OrderPM modelPM)
        {

            #region 验证
            if (string.IsNullOrWhiteSpace(modelPM.Version)) //版本号 
            {
                return ResultModel<string>.Conclude(OrdersStatus.NoVersion);
            }
            if (modelPM.OrderId < 0)//订单Id不合法
            {
                return ResultModel<string>.Conclude(OrdersStatus.ErrId);
            }
            //if (!iOrderProvider.IsExist(modelPM.OrderId)) //订单不存在
            //{
            //    return ResultModel<string>.Conclude(OrdersStatus.FailedGet);
            //}            
            int status = iOrderProvider.GetStatus(modelPM.OrderId);
            if (status == OrdersStatus.Status0.GetHashCode())//待接单
            {
                return Ets.Model.Common.ResultModel<string>.Conclude(OrdersStatus.Status0);
            }
            if (status == OrdersStatus.Status1.GetHashCode())//已完成
            {
                return Ets.Model.Common.ResultModel<string>.Conclude(OrdersStatus.Status1);
            }         
            if (status == OrdersStatus.Status3.GetHashCode())//已取消
            {
                return Ets.Model.Common.ResultModel<string>.Conclude(OrdersStatus.Status3);
            }
            if (status == OrdersStatus.Status4.GetHashCode())//送货中
            {
                return Ets.Model.Common.ResultModel<string>.Conclude(OrdersStatus.Status4);
            }
            #endregion

            try
            {
                iOrderProvider.UpdateTake(modelPM);
                return ResultModel<string>.Conclude(OrdersStatus.Success, "");
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter(" ResultModel<string> ConfirmTake", new { obj = "时间：" + DateTime.Now.ToString() + ex.Message });
                return ResultModel<string>.Conclude(GetOrdersStatus.Failed);
            }
        }
        
    }
}
