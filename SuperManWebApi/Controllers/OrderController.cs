using ETS;
using ETS.Enums;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Order; 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Ets.Service.Provider.User;
using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
using Ets.Service.Provider.Common;
using Ets.Service.IProvider.User;
using ETS.Const;
using Ets.Model.Common;
using ETS.Util;
using Ets.Model.ParameterModel.Clienter;
using ETS.Expand;using ETS.Enums;
using Ets.Model.ParameterModel.Bussiness;
using Ets.Model.ParameterModel.Order;
using Ets.Model.DomainModel.Order;
namespace SuperManWebApi.Controllers
{
    public class OrderController : ApiController
    {
        IOrderProvider iOrderProvider = new OrderProvider();
        IBusinessProvider iBusinessProvider = new BusinessProvider();
        readonly Ets.Service.IProvider.Clienter.IClienterProvider iClienterProvider = new Ets.Service.Provider.Clienter.ClienterProvider();
        /// <summary>
        /// 商户发布订单        
        /// </summary>
        /// <param name="model">订单实体</param>
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

        /// <summary>
        /// 订单详情        
        /// </summary>
        /// <param name="model">订单参数</param>
        /// <returns></returns>        
        [HttpPost]
        public ResultModel<OrderDM> GetDetails(OrderPM model)
        {
            //加验证

            OrderDM orderDM= iOrderProvider.GetDetails(model.OrderId);
            return Ets.Model.Common.ResultModel<OrderDM>.Conclude(GetOrdersStatus.Success, orderDM);          
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
            OrderChild orderChild = iClienterProvider.GetOrderChildInfo(orderId,orderChildId);
            if (string.IsNullOrWhiteSpace(orderChild.TicketUrl) || orderChild.HasUploadTicket)  //没有小票，请先上传
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.FirstUpload);
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
    }
}
