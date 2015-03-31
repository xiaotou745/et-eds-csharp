using SuperManCore;
using SuperManCore.Common;
using SuperManWebApi.Models.Business;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;
using SuperManDataAccess;
using SuperManBusinessLogic.B_Logic;
using SuperManBusinessLogic.Order_Logic;
using System.Threading.Tasks;
using SuperManCommonModel;
using Ets.Service.Provider.User;
using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
using Ets.Service.Provider.Common;
using Ets.Service.IProvider.User;
using ETS.Cacheing;
namespace SuperManWebApi.Controllers
{
    public class BusinessAPIController : ApiController
    {
        IOrderProvider iOrderProvider = new OrderProvider();
        IBusinessProvider iBusinessProvider = new BusinessProvider();
        /// <summary>
        /// 线程安全
        /// </summary>
        private static object lockHelper = new object();

        /// <summary>
        ///  B端注册 
        /// </summary>
        /// <param name="model">注册用户基本数据信息</param>
        /// <returns></returns>
        [ActionStatus(typeof(CustomerRegisterStatus))]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.DataModel.Bussiness.BusiRegisterResultModel> PostRegisterInfo_B(Ets.Model.ParameterModel.Bussiness.RegisterInfoModel model)
        {
            BusinessProvider bprovider = new BusinessProvider();
            return bprovider.PostRegisterInfo_B(model);
        }

        ///// <summary>
        ///// B端注册，供第三方使用
        ///// </summary>
        ///// <param name="model">注册用户基本数据信息</param>
        ///// <returns></returns>
        //[ActionStatus(typeof(CustomerRegisterStatus))]
        //[HttpPost]
        //public ResultModel<NewBusiRegisterResultModel> NewPostRegisterInfo_B(NewRegisterInfoModel model)
        //{
        //    if (string.IsNullOrWhiteSpace(model.PhoneNo))   //手机号非空验证
        //        return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberEmpty);
        //    //else if (BusiLogic.busiLogic().CheckExistPhone(model.PhoneNo))  //判断该手机号是否已经注册过
        //    //    return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberRegistered);
        //    else if (string.IsNullOrWhiteSpace(model.B_OriginalBusiId.ToString()))  //判断原平台商户Id不能为空
        //        return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.OriginalBusiIdEmpty);
        //    else if (string.IsNullOrWhiteSpace(model.B_GroupId.ToString()))  //集团Id不能为空
        //        return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.GroupIdEmpty);
        //    else if (BusiLogic.busiLogic().CheckExistBusi(model.B_OriginalBusiId, model.B_GroupId))
        //        return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.OriginalBusiIdRepeat);
        //    else if (string.IsNullOrWhiteSpace(model.B_City) || string.IsNullOrWhiteSpace(model.B_CityCode.ToString())) //城市以及城市编码非空验证
        //        return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.cityIdEmpty);
        //    else if (string.IsNullOrEmpty(model.B_Name.Trim())) //商户名称
        //        return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.BusiNameEmpty);
        //    else if (string.IsNullOrWhiteSpace(model.Address) || string.IsNullOrWhiteSpace(model.B_Province) || string.IsNullOrWhiteSpace(model.B_City) || string.IsNullOrWhiteSpace(model.B_Area) || string.IsNullOrWhiteSpace(model.B_AreaCode) || string.IsNullOrWhiteSpace(model.B_CityCode) || string.IsNullOrWhiteSpace(model.B_ProvinceCode))  //商户地址 省市区 不能为空
        //        return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.BusiAddressEmpty);
        //    else if (model.CommissionTypeId == 0)
        //    {
        //        return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.BusiAddressEmpty);
        //    }
        //    if (string.IsNullOrEmpty(model.B_Password))   //密码为空时 设置默认密码
        //        model.B_Password = MD5Helper.MD5("abc123");
        //    else
        //    {
        //        model.B_Password = MD5Helper.MD5(model.B_Password);
        //    }
        //    var business = NewRegisterInfoModelTranslator.Instance.Translate(model);
        //    bool result = BusiLogic.busiLogic().Add(business, true);
        //    var resultModel = new NewBusiRegisterResultModel
        //    {
        //        BusiRegisterId = business.Id
        //    };
        //    LogHelper.LogWriter("第三方调用商户注册接口", new { model = model, Message = CustomerRegisterStatus.Success });
        //    return ResultModel<NewBusiRegisterResultModel>.Conclude(CustomerRegisterStatus.Success, resultModel);
        //}


        /// <summary>
        /// B端注册，供第三方使用-平扬 2015.3.27修改成 ado方式
        /// </summary>
        /// <param name="model">注册用户基本数据信息</param>
        /// <returns></returns>
        [ActionStatus(typeof(Ets.Model.ParameterModel.Bussiness.CustomerRegisterStatus))]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Bussiness.NewBusiRegisterResultModel> NewPostRegisterInfo_B(Ets.Model.ParameterModel.Bussiness.NewRegisterInfoModel model)
        {
            var bprovider = new BusinessProvider();
            return bprovider.NewPostRegisterInfo_B(model);
        }


        /// <summary>
        /// B端取消订单，供第三方使用-2015.3.27-平扬改
        /// </summary>
        /// <param name="model">订单基本数据信息</param>
        /// <returns></returns>
        [ActionStatus(typeof(CancelOrderStatus))]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Bussiness.OrderCancelResultModel> NewOrderCancel(Ets.Model.ParameterModel.Bussiness.OrderCancelModel model)
        {
            //LogHelper.LogWriter("第三方调用取消订单：", new { model = model });
            //if (string.IsNullOrEmpty(model.OriginalOrderNo))   //订单号非空验证
            //    return ResultModel<OrderCancelResultModel>.Conclude(CancelOrderStatus.OrderEmpty);
            //if (string.IsNullOrEmpty(model.OrderFrom.ToString()))   //订单来源非空验证
            //    return ResultModel<OrderCancelResultModel>.Conclude(CancelOrderStatus.OrderFromEmpty);
            //var order = OrderLogic.orderLogic().GetOrderByOrderNoAndOrderFrom(model.OriginalOrderNo, model.OrderFrom, model.OrderType);
            //if (order == null)
            //{
            //    return ResultModel<OrderCancelResultModel>.Conclude(CancelOrderStatus.OrderIsNotExist);
            //}
            //if (order.Status == 3)
            //{
            //    return ResultModel<OrderCancelResultModel>.Conclude(CancelOrderStatus.Success);
            //}
            //bool b = OrderLogic.orderLogic().UpdateOrder(model.OriginalOrderNo, model.OrderFrom, OrderStatus.订单已取消);
            //if (b == true)
            //{
            //    return ResultModel<OrderCancelResultModel>.Conclude(CancelOrderStatus.Success);
            //}
            //else
            //{
            //    return ResultModel<OrderCancelResultModel>.Conclude(CancelOrderStatus.NotCancelOrder, new OrderCancelResultModel { Remark = "取消失败" });
            //}
            var bprovider = new BusinessProvider();
            return bprovider.NewOrderCancel(model);
        }

        /// <summary>
        /// 接收订单，供第三方使用
        /// 窦海超本地连调通过，因为是易淘食要对接，暂时没有内网测试
        /// 2015年3月30日 17:41:50
        /// </summary>
        /// <param name="model">订单基本数据信息</param>
        /// <returns></returns>

        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Order.NewPostPublishOrderResultModel> NewPostPublishOrder_B(Ets.Model.ParameterModel.Order.NewPostPublishOrderModel model)
        {
            #region 老的调用方式
            /*
            LogHelper.LogWriter("订单发布请求实体", new { model = model });
            if (string.IsNullOrWhiteSpace(model.OriginalOrderNo))   //原始订单号非空验证
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.OriginalOrderNoEmpty);
            if (model.OriginalBusinessId == 0)   //原平台商户Id非空验证
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.OriginalBusinessIdEmpty);
            if (string.IsNullOrWhiteSpace(model.OrderFrom.ToString()))   //订单来源
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.OrderFromEmpty);
            if (string.IsNullOrWhiteSpace(model.IsPay.ToString()))   //请确认是否已付款
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.IsPayEmpty);

            if (string.IsNullOrWhiteSpace(model.ReceiveName))    //收货人名称
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.ReceiveNameEmpty);

            if (string.IsNullOrWhiteSpace(model.ReceivePhoneNo)) //手机号
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.ReceivePhoneEmpty);

            if (string.IsNullOrWhiteSpace(model.Receive_Province) || string.IsNullOrWhiteSpace(model.Receive_ProvinceCode))  //所在省
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.ReceiveProvinceEmpty);

            if (string.IsNullOrWhiteSpace(model.Receive_City) || string.IsNullOrWhiteSpace(model.Receive_CityCode))  //所在市
                return ResultModel<NewPostPublishOrderResultModel>.Conclude
                    (OrderPublicshStatus.ReceiveCityEmpty);

            if (string.IsNullOrWhiteSpace(model.Receive_Area) || string.IsNullOrWhiteSpace(model.Receive_AreaCode))  //所在区
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.ReceiveAreaEmpty);

            if (string.IsNullOrWhiteSpace(model.Receive_Address))   //收货地址
                return ResultModel<NewPostPublishOrderResultModel>.Conclude
                    (OrderPublicshStatus.ReceiveAddressEmpty);
            //验证原平台商户是否已经注册
            var busi = BusiLogic.busiLogic().GetBusiByOriIdAndOrderFrom(model.OriginalBusinessId, model.OrderFrom);
            if (busi == null)
            {
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.BusinessNoExist);
            }
            else
            {
                if (busi.Status != ConstValues.BUSINESS_AUDITPASS)
                {
                    return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.BusinessNotAudit);
                }
            }
            //验证该平台 商户 订单号 是否存在
            var order = OrderLogic.orderLogic().GetOrderByOrderNoAndOrderFrom(model.OriginalOrderNo, model.OrderFrom, model.OrderType);
            if (order != null)
            {
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.OrderHadExist);
            }

            order dborder = NewBusiOrderInfoModelTranslator.Instance.Translate(model);  //整合订单信息
            bool result = OrderLogic.orderLogic().AddModel(dborder);    //添加订单记录，并且触发极光推送。          
            if (result)
            {
                NewPostPublishOrderResultModel resultModel = new NewPostPublishOrderResultModel { OriginalOrderNo = model.OriginalOrderNo, OrderNo = dborder.OrderNo };
                LogHelper.LogWriter("订单发布成功", new { model = model, resultModel = resultModel });
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.Success, resultModel);
            }
            else
            {
                NewPostPublishOrderResultModel resultModel = new NewPostPublishOrderResultModel { Remark = "订单发布失败" };
                LogHelper.LogWriter("订单发布失败", new { model = model });
                return ResultModel<NewPostPublishOrderResultModel>.Conclude(OrderPublicshStatus.Failed);
            }
             * 
             * */
            #endregion
            return new OrderProvider().NewPostPublishOrder_B(model);
        }

        /// <summary>
        /// B端登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(LoginModelStatus))]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.DataModel.Bussiness.BusiLoginResultModel> PostLogin_B(Ets.Model.ParameterModel.Bussiness.LoginModel model)
        {
            return new BusinessProvider().PostLogin_B(model);

        }

        /// <summary>
        /// 验证图片(审核)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Ets.Model.Common.ResultModel<UploadIconModel> PostAudit_B()
        {
            var strUserId = HttpContext.Current.Request.Form["UserId"];
            int userId;
            if (!Int32.TryParse(strUserId, out userId))
            {
                return Ets.Model.Common.ResultModel<UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidUserId);
            }

            //var business = BusiLogic.busiLogic().GetBusinessById(userId);
            var business = iBusinessProvider.GetBusiness(userId);  //判断商户是否存在
            if (business == null)
            {
                return Ets.Model.Common.ResultModel<UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidUserId);
            }
            if (HttpContext.Current.Request.Files.Count != 1)
            {
                return Ets.Model.Common.ResultModel<UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidFileFormat);
            }
            var file = HttpContext.Current.Request.Files[0];

            System.Drawing.Image img;
            try
            {
                img = System.Drawing.Image.FromStream(file.InputStream);
                //if (img.Width < CustomerIconUploader.Instance.Width || img.Height < CustomerIconUploader.Instance.Height)
                //{
                //    return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.InvalidFileFormat);
                //}
                string originSize = "_0_0";

                //var fileName = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMddhhmmss"), file.FileName);

                var fileName = string.Format("{0}_{1}_{2}", DateTime.Now.ToString("yyyyMMddhhmmssfff"), new Random().Next(1000), file.FileName);
                int fileNameLastDot = fileName.LastIndexOf('.');

                string rFileName = string.Format("{0}{1}{2}", fileName.Substring(0, fileNameLastDot), originSize, Path.GetExtension(fileName));

                if (!System.IO.Directory.Exists(CustomerIconUploader.Instance.PhysicalPath))
                {
                    System.IO.Directory.CreateDirectory(CustomerIconUploader.Instance.PhysicalPath);
                }
                var fullFilePath = System.IO.Path.Combine(CustomerIconUploader.Instance.PhysicalPath, rFileName);

                file.SaveAs(fullFilePath);

                var transformer = new FixedDimensionTransformerAttribute(CustomerIconUploader.Instance.Width, CustomerIconUploader.Instance.Height, CustomerIconUploader.Instance.MaxBytesLength / 1024);
                //var destFileName = string.Format("{0}_{1}{2}", DateTime.Now.ToString("yyyyMMddhhmmss"), new Random().Next(1000), Path.GetExtension(file.FileName));
                //var destFullFileName = System.IO.Path.Combine(CustomerIconUploader.Instance.PhysicalPath, destFileName);
                var destFullFileName = System.IO.Path.Combine(CustomerIconUploader.Instance.PhysicalPath, fileName);

                transformer.Transform(fullFilePath, destFullFileName);

                var picUrl = System.IO.Path.GetFileName(destFullFileName);

                //var _status = BusiLogic.busiLogic().UpdateBusi(business, picUrl);
                var upResult = iBusinessProvider.UpdateBusinessPicInfo(userId, picUrl);
                var relativePath = System.IO.Path.Combine(CustomerIconUploader.Instance.RelativePath, fileName).ToForwardSlashPath();
                if (upResult == -1)
                {
                    return Ets.Model.Common.ResultModel<UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.UpFailed, new UploadIconModel() { Id = 1, ImagePath = relativePath, status = upResult.ToString() });
                }
                else
                {
                    return Ets.Model.Common.ResultModel<UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.Success, new UploadIconModel() { Id = 1, ImagePath = relativePath, status = upResult.ToString() });
                }
            }
            catch (Exception)
            {
                return Ets.Model.Common.ResultModel<UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidFileFormat);
            }
        }


        /// <summary>
        /// 商户发布订单接口  2015.3.11 平扬 增加订单重复性验证
        /// achao 修改为ado.net
        /// </summary>
        /// <param name="model">订单数据</param>
        /// <returns></returns>
        [ActionStatus(typeof(PubOrderStatus))]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel> PostPublishOrder_B(Ets.Model.ParameterModel.Bussiness.BusiOrderInfoModel model)
        {
            lock (lockHelper)
            {
                //首先验证该 商户有无 资格 发布订单 wc
                if (!iBusinessProvider.HaveQualification(model.userId))
                {
                    return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(ETS.Enums.PubOrderStatus.HadCancelQualification);
                }
                #region 缓存验证
                string cacheKey = model.userId.ToString() + "_" + model.OrderSign;
                var cacheList = ETS.Cacheing.CacheFactory.Instance[cacheKey];
                LogHelper.LogWriter("订单发布~商户时间戳", new { cacheKey = cacheKey, model = model });
                if (cacheList != null)
                {
                    LogHelper.LogWriter("cacheList是否存在同一的商户时间戳：", new { cacheList = cacheList });
                    return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(ETS.Enums.PubOrderStatus.OrderHasExist);//当前时间戳内重复提交,订单已存在 
                }
                LogHelper.LogWriter("如果存在会继续往下执行？cacheList是否存在商户时间戳：", new { cacheList = cacheList, model = model });
                ETS.Cacheing.CacheFactory.Instance.AddObject(cacheKey, "1", DateTime.Now.AddMinutes(10));//添加当前时间戳记录
                LogHelper.LogWriter("在缓存里添加时间戳：", new { cacheKey = cacheKey, obj = 1, guoqishijian = DateTime.Now.AddMinutes(10), model = model });
                #endregion
            }
            if (model.OrderCount <= 0 || model.OrderCount > 15)   //判断录入订单数量是否符合要求
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(ETS.Enums.PubOrderStatus.OrderCountError);

            Ets.Model.DataModel.Order.order order = iOrderProvider.TranslateOrder(model);
            string result = iOrderProvider.AddOrder(order);

            if (result == "0")
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(ETS.Enums.PubOrderStatus.InvalidPubOrder);//当前订单执行失败
            }
            Ets.Model.ParameterModel.Order.BusiOrderResultModel resultModel = new Ets.Model.ParameterModel.Order.BusiOrderResultModel { userId = model.userId };
            return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(ETS.Enums.PubOrderStatus.Success, resultModel);

        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <returns></returns>
        [ActionStatus(typeof(GetOrdersStatus))]
        [HttpGet]
        public Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Bussiness.BusiGetOrderModel[]> GetOrderList_B(int userId, int? pagedSize, int? pagedIndex, sbyte? Status)
        {
            //var pIndex = pagedIndex.HasValue ? pagedIndex.Value : 0;
            //var pSize = pagedSize.HasValue ? pagedSize.Value : int.MaxValue;
            var pIndex = ETS.Util.ParseHelper.ToInt(pagedIndex, 1);
            pIndex = pIndex <= 0 ? 1 : pIndex;
            var pSize = ETS.Util.ParseHelper.ToInt(pagedSize, 100);

            Ets.Model.ParameterModel.Bussiness.BussOrderParaModelApp criteria = new Ets.Model.ParameterModel.Bussiness.BussOrderParaModelApp()
            {
                PagingResult = new Ets.Model.Common.PagingResult(pIndex, pSize),
                userId = userId,
                Status = Status
            };
            //var pagedList = OrderLogic.orderLogic().GetOrders(criteria);
            //var list = BusiGetOrderModelTranslator.Instance.Translate(pagedList);
            IList<Ets.Model.DomainModel.Bussiness.BusiGetOrderModel> list = new BusinessProvider().GetOrdersApp(criteria);
            return Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Bussiness.BusiGetOrderModel[]>.Conclude(ETS.Enums.GetOrdersStatus.Success, list.ToArray());
        }

        /// <summary>
        /// 地址管理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(BusiAddAddressStatus))]
        [HttpPost]
        public ResultModel<BusiAddAddressResultModel> PostManagerAddress_B(BusiAddAddressInfoModel model)
        {
            if (string.IsNullOrWhiteSpace(model.phoneNo))
            {
                return ResultModel<BusiAddAddressResultModel>.Conclude(BusiAddAddressStatus.PhoneNumberEmpty);
            }
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                return ResultModel<BusiAddAddressResultModel>.Conclude(BusiAddAddressStatus.AddressEmpty);
            }
            if (string.IsNullOrWhiteSpace(model.businessName))
            {
                return ResultModel<BusiAddAddressResultModel>.Conclude(BusiAddAddressStatus.businessNameEmpty);
            }
            var business = BusiAddAddressInfoModelTranslator.Instance.Translate(model);
            var result = BusiLogic.busiLogic().Update(business);

            var resultModel = new BusiAddAddressResultModel
            {
                userId = business.Id,
                status = result
            };
            return ResultModel<BusiAddAddressResultModel>.Conclude(BusiAddAddressStatus.Success, resultModel);
        }
        /// <summary>
        /// 地址管理
        /// 改 ado.net wc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.BusiAddAddressStatus))]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Bussiness.BusiAddAddressResultModel> PostManagerAddress_B_WC(Ets.Model.ParameterModel.Bussiness.BusiAddAddressInfoModel model)
        {
            if (string.IsNullOrWhiteSpace(model.phoneNo))
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Bussiness.BusiAddAddressResultModel>.Conclude(ETS.Enums.BusiAddAddressStatus.PhoneNumberEmpty);
            }
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Bussiness.BusiAddAddressResultModel>.Conclude(ETS.Enums.BusiAddAddressStatus.AddressEmpty);
            }
            if (string.IsNullOrWhiteSpace(model.businessName))
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Bussiness.BusiAddAddressResultModel>.Conclude(ETS.Enums.BusiAddAddressStatus.businessNameEmpty);
            }
            //修改商户地址信息，返回当前商户的状态
            int upResult = iBusinessProvider.UpdateBusinessAddressInfo(model);

            var resultModel = new Ets.Model.ParameterModel.Bussiness.BusiAddAddressResultModel
            {
                userId = model.userId,
                status = upResult.ToString()
            };
            if (upResult == -1)  //-1表示更新状态失败
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Bussiness.BusiAddAddressResultModel>.Conclude(ETS.Enums.BusiAddAddressStatus.UpdateFailed, resultModel);
            }
            else
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Bussiness.BusiAddAddressResultModel>.Conclude
                    (ETS.Enums.BusiAddAddressStatus.Success, resultModel);
            }
        }
        /// <summary>
        /// B端订单统计
        /// 改 ado.net
        /// wc
        /// </summary>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.LoginModelStatus))]
        [HttpGet]
        public Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Bussiness.BusiOrderCountResultModel> OrderCount_B(int userId)
        {
            var resultModel = BusiLogic.busiLogic().GetOrderCountData(userId);

            return Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Bussiness.BusiOrderCountResultModel>.Conclude(ETS.Enums.LoginModelStatus.Success, resultModel);
        }

        /// <summary>
        /// 请求动态验证码  (注册)
        /// 窦海超
        /// 2015年3月26日 17:46:08
        /// </summary>
        /// <param name="PhoneNumber">手机号码</param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.SendCheckCodeStatus))]
        [HttpGet]
        public Ets.Model.Common.SimpleResultModel CheckCode(string PhoneNumber)
        {
            /*
            if (!CommonValidator.IsValidPhoneNumber(PhoneNumber))  //验证电话号码合法性
            {
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.InvlidPhoneNumber);
            }
            var randomCode = new Random().Next(100000).ToString("D6");  //生成短信验证码
            var msg = string.Format(SupermanApiConfig.Instance.SmsContentCheckCode, randomCode, Ets.Model.Common.ConstValues.MessageBusiness);  //获取提示用语信息
            try
            {
                if (iBusinessProvider.CheckBusinessExistPhone(PhoneNumber))  //判断该手机号是否已经注册过
                    return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.AlreadyExists);
                else
                {
                    SupermanApiCaching.Instance.Add(PhoneNumber, randomCode);
                    //更新短信通道 
                    Task.Factory.StartNew(() =>
                    {
                        SendSmsHelper.SendSendSmsSaveLog(PhoneNumber, msg, Ets.Model.Common.ConstValues.SMSSOURCE);
                    });
                    return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.Sending);
                }
            }
            catch (Exception)
            {
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.SendFailure);
            }*/
            return new BusinessProvider().CheckCode(PhoneNumber);
        }

        /// <summary>
        /// 请求动态验证码  (找回密码)
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.SendCheckCodeStatus))]
        [HttpGet]
        public Ets.Model.Common.SimpleResultModel CheckCodeFindPwd(string PhoneNumber)
        {
            /* if (!CommonValidator.IsValidPhoneNumber(PhoneNumber))  //检查手机号码的合法性
             {
                 return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.InvlidPhoneNumber);
             }
             var randomCode = new Random().Next(100000).ToString("D6");
             var msg = string.Format(SupermanApiConfig.Instance.SmsContentFindPassword, randomCode, Ets.Model.Common.ConstValues.MessageBusiness);
             try
             {
                 SupermanApiCaching.Instance.Add(PhoneNumber, randomCode);
             
                 // 更新短信通道 
                 Task.Factory.StartNew(() =>
                 {
                     SendSmsHelper.SendSendSmsSaveLog(PhoneNumber, msg, Ets.Model.Common.ConstValues.SMSSOURCE);
                 });
                 return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.Sending);

             }
             catch (Exception)
             {
                 return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.SendFailure);
             }*/
            BusinessProvider businessProvider = new BusinessProvider();
            return businessProvider.CheckCodeFindPwd(PhoneNumber);
        }

        /// <summary>
        /// b端修改密码 edit by caoheyang 20150203 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.ForgetPwdStatus))]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.DataModel.Bussiness.BusiModifyPwdResultModel> PostForgetPwd_B(Ets.Model.DataModel.Bussiness.BusiForgetPwdInfoModel model)
        {
            //if (string.IsNullOrEmpty(model.password))  //密码非空验证
            //    return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.NewPwdEmpty);
            //if (string.IsNullOrEmpty(model.checkCode)) //验证码非空验证
            //    return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.checkCodeIsEmpty);
            //if (SupermanApiCaching.Instance.Get(model.phoneNumber) != model.checkCode) //验证码正确性验证
            //    return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.checkCodeWrong);
            //var business = BusiLogic.busiLogic().GetBusinessByPhoneNo(model.phoneNumber);
            //if (business == null)  //用户是否存在
            //    return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.ClienterIsNotExist);
            //if (business.Password == model.password) //您要找回的密码正是当前密码
            //    return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.PwdIsSave);
            //if (BusiLogic.busiLogic().ModifyPwd(business.Id, model.password))
            //    return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.Success);
            //else
            //    return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.FailedModifyPwd);
            return new BusinessProvider().PostForgetPwd_B(model);
        }

        /// <summary> 
        /// 商家设置外卖费 平扬 2015.3.5
        /// </summary>
        /// <returns></returns>
        //[ActionStatus(typeof(DistribSubsidyStatus))]
        //[HttpPost]
        //public SimpleResultModel PostDistribSubsidy_B(BusiDistribInfoModel mod)
        //{
        //    if (mod.userId <= 0 || mod.price < 0) //判断传入参数是否正常
        //        return SimpleResultModel.Conclude(DistribSubsidyStatus.Failed);
        //    var business = BusiLogic.busiLogic().GetBusinessById(mod.userId);
        //    if (business == null) //商户是否存在
        //        return SimpleResultModel.Conclude(DistribSubsidyStatus.Failed);
        //    bool result = BusiLogic.busiLogic().ModifyWaiMaiPrice(mod.userId, mod.price);
        //    if (result)
        //    {
        //        return SimpleResultModel.Conclude(DistribSubsidyStatus.Success);
        //    }
        //    else
        //    {
        //        return SimpleResultModel.Conclude(DistribSubsidyStatus.Failed);
        //    }
        //}

        /// <summary> 
        /// 商家设置外卖费 平扬 2015.3.5
        /// wangchao  改 ado.net 
        /// </summary>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.DistribSubsidyStatus))]
        [HttpPost]
        public Ets.Model.Common.SimpleResultModel PostDistribSubsidy_B(BusiDistribInfoModel mod)
        {
            if (mod.userId <= 0 || mod.price < 0) //判断传入参数是否正常
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.DistribSubsidyStatus.Failed);

            var selResult = iBusinessProvider.GetBusiness(mod.userId);
            if (selResult == null) //商户是否存在
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.DistribSubsidyStatus.Failed);
            int modResult = iBusinessProvider.ModifyWaiMaiPrice(mod.userId, mod.price);

            if (modResult > 0)
            {
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.DistribSubsidyStatus.Success);
            }
            else
            {
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.DistribSubsidyStatus.Failed);
            }
        }

        ///// <summary>
        ///// 取消订单
        ///// </summary>
        ///// <returns></returns>
        //[ActionStatus(typeof(CancelOrderStatus))]
        //[HttpGet]
        //public ResultModel<bool> CancelOrder_B(string userId, string OrderId)
        //{
        //    if (OrderId == null)
        //    {
        //        return ResultModel<bool>.Conclude(CancelOrderStatus.OrderEmpty);
        //    }
        //    var order = OrderLogic.orderLogic().GetOrderById(OrderId);
        //    if (order == null)
        //    {
        //        return ResultModel<bool>.Conclude(CancelOrderStatus.OrderIsNotExist);
        //    }
        //    bool b = OrderLogic.orderLogic().UpdateOrder(order, OrderStatus.订单已取消);
        //    if (b == true)
        //    {
        //        return ResultModel<bool>.Conclude(CancelOrderStatus.Success, true);
        //    }
        //    else
        //    {
        //        return ResultModel<bool>.Conclude(CancelOrderStatus.FailedCancelOrder, true);
        //    }
        //}

        /// <summary>
        /// 取消订单 ado.net  wangchao
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionStatus(typeof(ETS.Enums.CancelOrderStatus))]
        public Ets.Model.Common.ResultModel<bool> CancelOrder_B(string userId, string OrderId)
        {
            if (string.IsNullOrWhiteSpace(OrderId))
                return Ets.Model.Common.ResultModel<bool>.Conclude(ETS.Enums.CancelOrderStatus.OrderEmpty);
            //查询该订单是否存在
            int selResult = iOrderProvider.GetOrderByOrderNo(OrderId);
            if (selResult > 0)
            {
                //存在的情况下  取消订单  3
                int cacelResult = iOrderProvider.UpdateOrderStatus(OrderId, Ets.Model.Common.ConstValues.ORDER_CANCEL);
                if (cacelResult > 0)
                    return Ets.Model.Common.ResultModel<bool>.Conclude(ETS.Enums.CancelOrderStatus.Success, true);
                else
                    return Ets.Model.Common.ResultModel<bool>.Conclude(ETS.Enums.CancelOrderStatus.FailedCancelOrder, true);
            }
            else
            {
                return Ets.Model.Common.ResultModel<bool>.Conclude(ETS.Enums.CancelOrderStatus.OrderIsNotExist);
            }
        }


        /// <summary>
        /// 流转图片
        /// </summary>
        /// <param name="Bytes"></param>
        /// <returns></returns>
        private Image byteArrayToImage(byte[] Bytes)
        {
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                Image outputImg = Image.FromStream(ms);
                return outputImg;
            }
        }
        /// <summary>
        /// 文件转图片
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <returns></returns>
        private byte[] imageToByteArray(string FilePath)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Image imageIn = Image.FromFile(FilePath))
                {
                    using (Bitmap bmp = new Bitmap(imageIn))
                    {
                        bmp.Save(ms, imageIn.RawFormat);
                    }
                }
                return ms.ToArray();
            }
        }



        /// <summary>
        /// 客服电话获取
        /// 窦海超
        /// 2015年3月16日 11:44:54
        /// </summary>
        /// <param name="CityName">城市名称</param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.ServicePhoneStatus))]
        [HttpGet]
        public Ets.Model.Common.ResultModel<Ets.Model.Common.ResultModelServicePhone> GetCustomerServicePhone(string CityName)
        {
            return Ets.Model.Common.ResultModel<Ets.Model.Common.ResultModelServicePhone>.Conclude(
                GetOrdersStatus.Success,
                new ServicePhone().GetCustomerServicePhone(CityName)
                );
        }
        #region redis测试方法，平扬看完可以删除

        [HttpGet]
        public void test() {
            ETS.NoSql.RedisCache.RedisCache redis = new ETS.NoSql.RedisCache.RedisCache();
            redis.Add("1","123");

           string ss= redis.Get<string>("1");
        }
        #endregion


    }
}