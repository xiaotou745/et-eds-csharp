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
namespace SuperManWebApi.Controllers
{
    /// <summary>
    /// TODO:每个API的日志、异常之类
    /// </summary>
    public class OrderController : ApiController
    {
        IOrderProvider iOrderProvider = new OrderProvider();
        IBusinessProvider iBusinessProvider = new BusinessProvider();
        readonly IClienterProvider iClienterProvider = new ClienterProvider();
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

            #region 验证
            if (!iBusinessProvider.HaveQualification(model.userId))//验证该商户有无发布订单资格 
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.HadCancelQualification);
            }
            ///TODO 之前的任务金额10-5000        
            if (model.Amount < 10m)
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.AmountLessThanTen);
            }
            if (model.Amount > 5000m)
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.AmountMoreThanFiveThousand);
            }
            if (model.OrderCount <= 0 || model.OrderCount > 15) //判断录入订单数量是否符合要求
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.OrderCountError);
            }           
            decimal amount=0;
            for(int i=0;i<model.listOrderChlid.Count;i++)
            {
                if(model.listOrderChlid[i].GoodPrice!=null)
                {
                    amount+=model.listOrderChlid[i].GoodPrice;
                }                
            }
            if(model.Amount!=amount)
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.AmountIsNotEqual);
            }
            if (model.OrderCount != model.listOrderChlid.Count)//主订单与子订单数量
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.CountIsNotEqual);
            }

            order order = iOrderProvider.TranslateOrder(model);
            if (order.CommissionType ==(int)OrderCommissionType.FixedRatio && order.BusinessCommission < 10m) //商户结算比例不能小于10
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.BusiSettlementRatioError);
            }
            PubOrderStatus cuStatus = iOrderProvider.AddOrder(order);

            if (cuStatus == PubOrderStatus.Success)//当前订单执行失败
            {
                BusiOrderResultModel resultModel = new BusiOrderResultModel { userId = model.userId };
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.Success, resultModel);
            }            

            return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.InvalidPubOrder);
            #endregion       
        }

        /// <summary>
        /// 获取订单详情                
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="model">订单参数实体</param>
        /// <returns></returns>        
        [HttpPost]
        public ResultModel<OrderDM> GetDetails(OrderPM model)
        {
            ///TODO static?
            degree.longitude = model.longitude;
            degree.latitude = model.latitude;

            #region 验证

            var version = HttpContext.Current.Request.Form["Version"];
            if (string.IsNullOrWhiteSpace(version)) //版本号 
            {
                return ResultModel<OrderDM>.Conclude(GetOrdersStatus.NoVersion);
            }
            if (model.OrderId < 0)//订单Id不合法
            {
                return ResultModel<OrderDM>.Conclude(GetOrdersStatus.ErrOderNo);
            }
            if (!iOrderProvider.IsExist(model.OrderId)) //订单不存在
            {
                return ResultModel<OrderDM>.Conclude(GetOrdersStatus.ErrOderNo); 
            }

            #endregion

            OrderDM orderDM= iOrderProvider.GetDetails(model.OrderId);
            return ResultModel<OrderDM>.Conclude(GetOrdersStatus.Success, orderDM);          
        }

        /// <summary>
        /// 上传小票信息，子订单
        /// wc
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiVersionStatistic]
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel> TicketUpload(string Version)
        { 
            if (HttpContext.Current.Request.Form.Count == 0)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.NOFormParameter);
            }
            var orderId = ParseHelper.ToInt(HttpContext.Current.Request.Form["OrderId"], 0); //订单号
            var clienterId = ParseHelper.ToInt(HttpContext.Current.Request.Form["ClienterId"], 0); //骑士的id
            var needUploadCount = ParseHelper.ToInt(HttpContext.Current.Request.Form["NeedUploadCount"], 1); //该订单总共需要上传的 小票数量
            var version = HttpContext.Current.Request.Form["Version"]; //版本号  1.0 
            var orderChildId = ParseHelper.ToInt(HttpContext.Current.Request.Form["OrderChildId"], 0); //子单号
            if (clienterId == 0) // 骑士Id
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.ClienterIdInvalid);
            }
            if (orderId == 0)  // 订单id
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidOrderId);
            }
            if (string.IsNullOrWhiteSpace(version))  //版本号
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.NoVersion);
            }
            if (orderChildId == 0) //子订单号
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.NoOrderChildId);
            }
            if (HttpContext.Current.Request.Files.Count == 0)  //图片
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidFileFormat);
            }  
            var file = HttpContext.Current.Request.Files[0]; //照片
            System.Drawing.Image img;
            try
            {
                img = System.Drawing.Image.FromStream(file.InputStream);
            }
            catch (Exception)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidFileFormat);
            }
            var fileName = ETS.Util.ImageTools.GetFileName(Path.GetExtension(file.FileName));
            int fileNameLastDot = fileName.LastIndexOf('.');
            //原图 
            string rFileName = string.Format("{0}{1}{2}", fileName.Substring(0, fileNameLastDot), ImageConst.OriginSize, Path.GetExtension(fileName));

            string saveDbFilePath;

            string fullFileDir = ETS.Util.ImageTools.CreateDirectory(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath, orderId.ToString(), out saveDbFilePath);

            if (fullFileDir == "0")
            {

                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.UpFailed);
            }
            //保存原图
            var fullFilePath = Path.Combine(fullFileDir, rFileName);

            file.SaveAs(fullFilePath);

            //裁图
            var transformer = new SuperManCore.FixedDimensionTransformerAttribute(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.Width, Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.Height, CustomerIconUploader.Instance.MaxBytesLength / 1024);
            //保存到数据库的图片路径
            var destFullFileName = System.IO.Path.Combine(fullFileDir, fileName);
            transformer.Transform(fullFilePath, destFullFileName);

            var picUrl = saveDbFilePath + fileName;
            var uploadReceiptModel = new Ets.Model.ParameterModel.Clienter.UploadReceiptModel
            {
                OrderId = orderId,
                ClienterId = clienterId,
                OrderChildId = orderChildId,
                NeedUploadCount = needUploadCount,
                ReceiptPic = picUrl,
                HadUploadCount = 1
            };
            var orderOther = iClienterProvider.UpdateClientReceiptPicInfo(uploadReceiptModel);
            if (orderOther == null)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.UpFailed, new Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel() { OrderId = orderId });
            }
            else
            {
                List<string> listReceiptPic = ImageCommon.ReceiptPicConvert(uploadReceiptModel.ReceiptPic);
                List<OrderChildImg> listOrderChild = new List<OrderChildImg>();
                listOrderChild.Add(new OrderChildImg() { OrderChildId = orderChildId, TicketUrl = listReceiptPic[0] });
                //上传成功后返回图片全路径
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.Success, new Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel() { OrderId = orderId,  OrderChildList = listOrderChild, HadUploadCount = orderOther.HadUploadCount, NeedUploadCount = orderOther.NeedUploadCount });
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
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel> TicketRemove(string Version)
        { 
            if (HttpContext.Current.Request.Form.Count == 0)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.NOFormParameter);
            }
            var orderId = ParseHelper.ToInt(HttpContext.Current.Request.Form["OrderId"], 0); //订单Id
            var orderChildId = ParseHelper.ToInt(HttpContext.Current.Request.Form["OrderChildId"], 0); //子订单Id
            var receiptPic = HttpContext.Current.Request.Form["ReceiptPicAddress"];  //小票上传地址
            var version = HttpContext.Current.Request.Form["Version"]; //版本号  1.0
            if (orderId == 0)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidOrderId);
            }
            if (orderChildId == 0)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.NoOrderChildId);
            }
            if (string.IsNullOrWhiteSpace(version))  //版本号
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.NoVersion);
            }
            if (!receiptPic.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.ReceiptAddressInvalid);
            }         

            Ets.Model.ParameterModel.Clienter.UploadReceiptModel uploadReceiptModel = new Ets.Model.ParameterModel.Clienter.UploadReceiptModel() { OrderId = orderId, ReceiptPic = receiptPic, HadUploadCount = -1 };
             
            SuperManCore.LogHelper.LogWriter("删除小票参数", new { version = version, receiptPic = receiptPic, orderId = orderId,orderChildId = orderChildId });
            //删除前先判断   订单状态已完成 和已经上传的小票数量 等于需要上传的小票数量相等 时 不允许删除小票
            OrderOther orderOther = iClienterProvider.GetReceipt(orderId);
            //判断订单信息，状态是否为已完成， 已经上传的小票数量是否和 需要上传的一样， 若一样则无法删除
            if (orderOther != null)
            {
                if (orderOther.OrderStatus == ConstValues.ORDER_FINISH && orderOther.NeedUploadCount == orderOther.HadUploadCount)
                {
                    return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.DeleteFailed);
                }
            }
            else
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.CannotFindOrder);
            }
            //判断是否存在小票 
            List<OrderChildForTicket> orderChild = iClienterProvider.GetOrderChildInfo(orderId,orderChildId);
            if (orderChild != null && orderChild.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(orderChild[0].TicketUrl) || orderChild[0].HasUploadTicket)  //没有小票，请先上传
                {
                    return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.FirstUpload);
                }
            }
            else
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.DeleteFailed);
            }
            //删除小票的时候更新orderother表，更新 orderchild表
            OrderOther delOrderOther = iClienterProvider.DeleteReceipt(uploadReceiptModel); 
            if (delOrderOther.Id > 0)
            { 
                //List<string> listReceiptPic = ImageCommon.ReceiptPicConvert(delOrderOther.ReceiptPic); 
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.Success, new Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel() { OrderId = delOrderOther.OrderId, HadUploadCount = delOrderOther.HadUploadCount, NeedUploadCount = delOrderOther.NeedUploadCount });
            }
            else
            { 
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.DeleteFailed);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <param name="bussinessId"></param>
        /// <param name="Version"></param>
        /// <returns></returns>
        [HttpPost]
        [ExecuteTimeLog]
        public ResultModel<RushOrderResultModel> Receive(int userId, string orderNo,int bussinessId, string Version)
        {
            if (string.IsNullOrEmpty(orderNo)) //订单号码非空验证
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.OrderEmpty);
            if (userId <= 0) //用户id验证
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.userIdEmpty);
            if (bussinessId <= 0)
            {
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.userIdEmpty);
            }
            if (string.IsNullOrWhiteSpace(Version))
            {
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.NoVersion);
            } 
            return new ClienterProvider().Receive_C(userId, orderNo, bussinessId); 
        }
    }
}
