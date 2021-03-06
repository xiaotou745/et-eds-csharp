﻿using ETS;
using ETS.Enums;
using Ets.Model.Common;
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
using Ets.Model.DomainModel.Order;
using SuperManCommonModel;
using Ets.Service.Provider.User;
using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
using Ets.Service.Provider.Common;
using Ets.Service.IProvider.User;
using ETS.Const;
using SuperManWebApi.Providers;

namespace SuperManWebApi.Controllers
{
    public class BusinessAPIController : ApiController
    {
        IOrderProvider iOrderProvider = new OrderProvider();
        IBusinessProvider iBusinessProvider = new BusinessProvider();
        readonly Ets.Service.IProvider.Common.IAreaProvider iAreaProvider = new Ets.Service.Provider.Common.AreaProvider();
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
        [ActionStatus(typeof(ETS.Enums.CancelOrderStatus))]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Bussiness.OrderCancelResultModel> NewOrderCancel(Ets.Model.ParameterModel.Bussiness.OrderCancelModel model)
        {

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
            return new OrderProvider().NewPostPublishOrder_B(model);
        }

        /// <summary>
        /// B端登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.LoginModelStatus))]
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
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel> PostAudit_B()
        {

            var strUserId = HttpContext.Current.Request.Form["UserId"];
            int userId;
            if (!Int32.TryParse(strUserId, out userId))
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidUserId);
            }
            var business = iBusinessProvider.GetBusiness(userId);  //判断商户是否存在
            if (business == null)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidUserId);
            }
            if (HttpContext.Current.Request.Files.Count != 1)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidFileFormat);
            }
            var file = HttpContext.Current.Request.Files[0];

            System.Drawing.Image img;
            try
            {
                #region 暂时注释

                //img = System.Drawing.Image.FromStream(file.InputStream);

                ////var fileName = string.Format("{0}_{1}_{2}", DateTime.Now.ToString("yyyyMMddhhmmssfff"), new Random().Next(1000), file.FileName);

                //var fileName = ETS.Util.ImageTools.GetFileName("B", Path.GetExtension(file.FileName));

                //int fileNameLastDot = fileName.LastIndexOf('.');
                ////原图
                //string rFileName = string.Format("{0}{1}{2}", fileName.Substring(0, fileNameLastDot), ImageConst.OriginSize, Path.GetExtension(fileName));
                ////保存到数据库的目录结构，年月日
                //string saveDbPath;
                ////fullDir 保存到 磁盘的 完整路径
                //string fullDir = ETS.Util.ImageTools.CreateDirectory(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath,"", out saveDbPath);
                //if (fullDir == "0")
                //{
                //    LogHelper.LogWriter("上传图片失败：", new { ex = "检查是否有权限创建目录" });
                //    return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.UpFailed);
                //}
                ////保存原图到 磁盘中
                //var fullFilePath = System.IO.Path.Combine(fullDir, rFileName);
                //file.SaveAs(fullFilePath);
                ////裁图
                //var transformer = new FixedDimensionTransformerAttribute(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.Width, Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.Height, Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.MaxBytesLength / 1024);

                //var destFullFileName = System.IO.Path.Combine(fullDir, fileName);
                ////裁图，并保存到磁盘
                //transformer.Transform(fullFilePath, destFullFileName);
                ////保存到数据库的图片路径，包含年月日
                //var picUrl = saveDbPath + fileName;

                #endregion

                ImageHelper ih = new ImageHelper();
                ImgInfo imgInfo = ih.UploadImg(file, 0);
                if (!string.IsNullOrWhiteSpace(imgInfo.FailRemark))
                {
                    return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.UpFailed);
                }
                //保存图片目录信息到数据库
                var upResult = iBusinessProvider.UpdateBusinessPicInfo(userId, imgInfo.PicUrl);
                if (upResult == -1)
                {
                    return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.UpFailed, new Ets.Model.ParameterModel.Clienter.UploadIconModel() { Id = userId, ImagePath = imgInfo.PicUrl, status = upResult.ToString() });
                }
                else
                {
                    return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.Success, new Ets.Model.ParameterModel.Clienter.UploadIconModel() { Id = userId, ImagePath = imgInfo.PicUrl, status = upResult.ToString() });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("上传失败：", new { ex = ex });
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidFileFormat);
            }
        }



        ///// <summary>
        ///// 商户发布订单接口  2015.3.11 平扬 增加订单重复性验证
        ///// achao 修改为ado.net
        ///// </summary>
        ///// <param name="model">订单数据</param>
        ///// <returns></returns>
        //[ActionStatus(typeof(ETS.Enums.PubOrderStatus))]
        //[HttpPost]
        //public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel> PostPublishOrder_B(Ets.Model.ParameterModel.Bussiness.BussinessOrderInfoPM model)
        //{      
        //    //首先验证该 商户有无 资格 发布订单 wc
        //    if (!iBusinessProvider.HaveQualification(model.userId))
        //    {
        //        return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(ETS.Enums.PubOrderStatus.HadCancelQualification);
        //    }
        //    if (model.Amount < 10m)
        //    {
        //        return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(ETS.Enums.PubOrderStatus.AmountLessThanTen);
        //    }
        //    if (model.Amount > 5000m)
        //    {
        //        return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(ETS.Enums.PubOrderStatus.AmountMoreThanFiveThousand);
        //    }
        //    lock (lockHelper)
        //    {  
        //        #region 缓存验证
        //        string cacheKey = "PostPublishOrder_B_" + model.userId + "_" + model.OrderSign;
        //        var redis = new ETS.NoSql.RedisCache.RedisCache(); 
        //        var cacheValue = redis.Get<string>(cacheKey); 
        //        if (cacheValue != null)
        //        {  
        //            return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(ETS.Enums.PubOrderStatus.OrderHasExist);//当前时间戳内重复提交,订单已存在 
        //        } 
        //        redis.Add(cacheKey, "1", DateTime.Now.AddHours(10));//添加当前时间戳记录
        //        #endregion
        //    }
        //    if (model.OrderCount <= 0 || model.OrderCount > 15)   //判断录入订单数量是否符合要求
        //        return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(ETS.Enums.PubOrderStatus.OrderCountError);

        //    Ets.Model.DataModel.Order.order order = iOrderProvider.TranslateOrder(model);            
        //    if (order.CommissionType==1 && order.BusinessCommission < 10m)  //商户结算比例不能小于10
        //    {
        //        return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(ETS.Enums.PubOrderStatus.BusiSettlementRatioError);
        //    }
        //    string result = iOrderProvider.AddOrder(order);

        //    if (result == "0")
        //    {
        //        return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(ETS.Enums.PubOrderStatus.InvalidPubOrder);//当前订单执行失败
        //    }
        //    Ets.Model.ParameterModel.Order.BusiOrderResultModel resultModel = new Ets.Model.ParameterModel.Order.BusiOrderResultModel { userId = model.userId };
        //    return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Order.BusiOrderResultModel>.Conclude(ETS.Enums.PubOrderStatus.Success, resultModel);

        //}    
        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.GetOrdersStatus))]
        [HttpGet]
        public Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Bussiness.BusiGetOrderModel[]> GetOrderList_B(int userId, int? pagedSize, int? pagedIndex, sbyte? Status, int? orderfrom)
        {
            var pIndex = ETS.Util.ParseHelper.ToInt(pagedIndex, 1);
            pIndex = pIndex <= 0 ? 1 : pIndex;
            var pSize = ETS.Util.ParseHelper.ToInt(pagedSize, 100);

            Ets.Model.ParameterModel.Bussiness.BussOrderParaModelApp criteria = new Ets.Model.ParameterModel.Bussiness.BussOrderParaModelApp()
            {
                PagingResult = new Ets.Model.Common.PagingResult(pIndex, pSize),
                userId = userId,
                Status = Status,
                OrderFrom = orderfrom ?? 0
            };
            IList<Ets.Model.DomainModel.Bussiness.BusiGetOrderModel> list = new BusinessProvider().GetOrdersApp(criteria);
            return Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Bussiness.BusiGetOrderModel[]>.Conclude(ETS.Enums.GetOrdersStatus.Success, list.ToArray());
        }




        #region 美团等第三方订单处理

        /// <summary>
        /// 获取订单详细
        /// </summary>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.GetOrdersStatus))]
        [HttpGet]
        public Ets.Model.Common.ResultModel<ListOrderDetailModel> GetOrderDetail(string orderno)
        {
            var model = new OrderProvider().GetOrderDetail(orderno);
            if (model != null)
            {
                return Ets.Model.Common.ResultModel<ListOrderDetailModel>.Conclude(ETS.Enums.GetOrdersStatus.Success, model);
            }
            return Ets.Model.Common.ResultModel<ListOrderDetailModel>.Conclude(ETS.Enums.GetOrdersStatus.FailedGetOrders, model);
        }


        /// <summary>
        /// 商家确认第三方订单接口  平杨
        /// </summary>
        /// <param name="orderlist"></param>
        /// <UpdateBy>确认接入时扣除商家结算费功能  caoheyang 20150526</UpdateBy>
        /// <returns></returns>
        [ActionStatus(typeof(PubOrderStatus))]
        [HttpGet]
        public ResultModel<List<string>> OtherOrderConfirm_B(string orderlist)
        {
            ETS.Util.LogHelper.LogWriterString("参数 ", orderlist);
            if (string.IsNullOrEmpty(orderlist))
                return ResultModel<List<string>>.Conclude(PubOrderStatus.OrderCountError, null);
            var orderProvider = new OrderProvider();
            string[] orders = orderlist.Split(',');
            List<string> errors = new List<string>();
            for (int i = 0; i < orders.Length; i++)
            {
                int res = orderProvider.UpdateOrderStatus(orders[i], OrderConst.ORDER_NEW, "", OrderConst.OrderStatus30);
                if (res <= 0)
                    errors.Add(orders[i]);
            }
            return ResultModel<List<string>>.Conclude(PubOrderStatus.Success, errors);
        }

        /// <summary>
        /// 商家拒绝第三方订单接口
        /// </summary>
        /// <param name="orderlist"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.PubOrderStatus))]
        [HttpGet]
        public ResultModel<int> OtherOrderCancel_B(string orderlist, string note)
        {
            if (string.IsNullOrEmpty(orderlist))
            {
                return ResultModel<int>.Conclude(PubOrderStatus.OrderCountError, 0);
            }
            var orderProvider = new OrderProvider();
            string[] orders = orderlist.Split(',');
            int i = orders.Count(s => orderProvider.UpdateOrderStatus(s, OrderConst.ORDER_CANCEL, note, OrderConst.OrderStatus30) > 0);
            return ResultModel<int>.Conclude(PubOrderStatus.Success, i);
        }

        /// <summary>
        /// 商家拒绝原因接口
        /// </summary>
        /// <param name="orderlist"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.PubOrderStatus))]
        [HttpGet]
        public ResultModel<OrderCancelReasonsModel> OtherOrderCancelReasons(string Version)
        {
            var orderProvider = new OrderProvider();
            string Ressons = orderProvider.OtherOrderCancelReasons();
            var model = new OrderCancelReasonsModel
            {
                Reasons = Ressons.Split(';'),
                GlobalVersion = Config.GlobalVersion
            };
            return ResultModel<OrderCancelReasonsModel>.Conclude(PubOrderStatus.Success, model);
        }


        #endregion
        /// <summary>
        /// 地址管理
        /// 改 ado.net wc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.BusiAddAddressStatus))]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Bussiness.BusiAddAddressResultModel> PostManagerAddress_B(Ets.Model.ParameterModel.Bussiness.BusiAddAddressInfoModel model)
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
            if (userId <= 0)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Bussiness.BusiOrderCountResultModel>.Conclude(ETS.Enums.GetOrdersStatus.FailedGetOrders, null);
            }
            var resultModel = new BusinessProvider().GetOrderCountData(userId);
            if (resultModel == null)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Bussiness.BusiOrderCountResultModel>.Conclude(ETS.Enums.GetOrdersStatus.FailedGetOrders, null);
            }
            return Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Bussiness.BusiOrderCountResultModel>.Conclude(ETS.Enums.GetOrdersStatus.Success, resultModel);
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
            BusinessProvider businessProvider = new BusinessProvider();
            return businessProvider.CheckCodeFindPwd(PhoneNumber);
        }

        /// <summary>
        /// 请求语音动态验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.SendCheckCodeStatus))]
        [HttpPost]
        public Ets.Model.Common.SimpleResultModel VoiceCheckCode(Ets.Model.ParameterModel.Sms.SmsParaModel model)
        {
            BusinessProvider businessProvider = new BusinessProvider();
            return businessProvider.VoiceCheckCode(model);

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
            return new BusinessProvider().PostForgetPwd_B(model);
        }


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



        /// <summary>
        /// 取消订单 Edit  caoheyang 20150521 
        /// </summary>
        /// <remarks>取消订单时返给商家结算费，优化，大整改</remarks>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionStatus(typeof(ETS.Enums.CancelOrderStatus))]
        public ResultModel<bool> CancelOrder_B(CancelOrderBPM paramodel)
        {
            return iOrderProvider.CancelOrderB(paramodel);
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
                ServicePhoneStatus.Success,
                new ServicePhone().GetCustomerServicePhone(CityName)
                );
        }
        /// <summary>
        /// 获取用户状态
        /// 平扬
        /// 2015年3月31日 
        /// </summary>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.UserStatus))]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Bussiness.BussinessStatusModel> GetUserStatus(UserStatusModel parModel)
        {
            var model = iBusinessProvider.GetUserStatus(parModel.userId);
            if (model != null)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Bussiness.BussinessStatusModel>.Conclude(
                UserStatus.Success,
                model
                );
            }
            return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Bussiness.BussinessStatusModel>.Conclude(
                UserStatus.Error,
                null
                );
        }


        /// <summary>
        /// 获取开通城市的省市区
        /// 窦海超
        /// 2015年3月16日 11:44:54
        /// </summary>
        /// <param name="Version">版本号</param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.CityStatus))]
        [HttpGet]
        [ApiVersionStatistic]
        public Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Area.AreaModelList> GetOpenCity(string Version)
        {
            AreaProvider area = new AreaProvider();

            return area.GetOpenCity(Version);
        }

    }
}