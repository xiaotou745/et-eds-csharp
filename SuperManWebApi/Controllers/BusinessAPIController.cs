using ETS;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Model.DataModel.Order;
using Ets.Model.ParameterModel.Order;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
using Ets.Service.Provider.Common;
using ETS.Const;
using SuperManWebApi.App_Start.Filters;
using SuperManWebApi.Providers;
using Ets.Service.IProvider.Common;
using Ets.Model.DataModel.Business;
using Ets.Model.ParameterModel.Business;
using Ets.Service.IProvider.Business;
using Ets.Service.Provider.Business;
using Ets.Model.DomainModel.Order;
using Ets.Model.DomainModel.Business;
using Ets.Model.ParameterModel.Clienter;
using Ets.Model.DomainModel.Area;
using Ets.Model.ParameterModel.Sms;
using ETS.Util;
namespace SuperManWebApi.Controllers
{
    public class BusinessAPIController : ApiController
    {
        readonly IOrderProvider iOrderProvider = new OrderProvider();
        readonly IBusinessProvider iBusinessProvider = new BusinessProvider();
        readonly IAreaProvider iAreaProvider = new AreaProvider();

        /// <summary>
        ///  B端注册 
        /// </summary>
        /// <param name="model">注册用户基本数据信息</param>
        /// <returns></returns>        
        [HttpPost]
        public ResultModel<BusiRegisterResultModel> PostRegisterInfo_B(RegisterInfoPM model)
        {
            BusinessProvider bprovider = new BusinessProvider();
            return bprovider.PostRegisterInfo_B(model);
        }

        /// <summary>
        /// B端登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>        
        [HttpPost]
        public ResultModel<BusiLoginResultModel> PostLogin_B(LoginModel model)
        {
            return new BusinessProvider().PostLogin_B(model);
        }

        /// <summary>
        /// 验证图片(审核)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Token]
        public ResultModel<UploadIconModel> PostAudit_B()
        {

            var strUserId = HttpContext.Current.Request.Form["UserId"];
            int userId;
            if (!Int32.TryParse(strUserId, out userId))
            {
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.InvalidUserId);
            }
            var business = iBusinessProvider.GetBusiness(userId);  //判断商户是否存在
            if (business == null)
            {
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.InvalidUserId);
            }
            if (HttpContext.Current.Request.Files.Count != 1)
            {
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.InvalidFileFormat);
            }
            var file = HttpContext.Current.Request.Files[0];

            System.Drawing.Image img;
            try
            {
                ImageHelper ih = new ImageHelper();
                ImgInfo imgInfo = ih.UploadImg(file, 0, ImageType.Business);
                if (!string.IsNullOrWhiteSpace(imgInfo.FailRemark))
                {
                    return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.UpFailed);
                }
                //保存图片目录信息到数据库
                var upResult = iBusinessProvider.UpdateBusinessPicInfo(userId, imgInfo.PicUrl);
                imgInfo.PicUrl = CustomerIconUploader.Instance.GetPhysicalPath(ImageType.Business);
                if (upResult == -1)
                {
                    return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.UpFailed, new UploadIconModel() { Id = userId, ImagePath = imgInfo.PicUrl, status = upResult.ToString() });
                }
                else
                {
                    return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.Success, new UploadIconModel() { Id = userId, ImagePath = imgInfo.PicUrl, status = upResult.ToString() });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("上传失败：", new { ex = ex });
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.InvalidFileFormat);
            }
        }


        /// <summary>
        /// b端忘记密码 edit by caoheyang 20150203 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>        
        [HttpPost]
        public ResultModel<BusiModifyPwdResultModel> PostForgetPwd_B(BusiForgetPwdInfoModel model)
        {
            return new BusinessProvider().PostForgetPwd_B(model);
        }

        /// <summary>
        /// b端修改密码  与忘记密码完全一样  只是token的区别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>        
        [HttpPost]
        [Token]
        public ResultModel<BusiModifyPwdResultModel> ModifyPwd_B(BusiForgetPwdInfoModel model)
        {
            return new BusinessProvider().PostForgetPwd_B(model);
        }

        /// <summary>
        /// 请求动态验证码  (注册)
        /// 窦海超
        /// 2015年3月26日 17:46:08
        /// </summary>
        /// <param name="PhoneNumber">手机号码</param>
        /// <returns></returns>        
        [HttpGet]
        public SimpleResultModel CheckCode(string PhoneNumber)
        {

            return new BusinessProvider().CheckCode(PhoneNumber);
        }

        /// <summary>
        /// 请求动态验证码  (找回密码)
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>        
        [HttpGet]
        public SimpleResultModel CheckCodeFindPwd(string PhoneNumber)
        {
            BusinessProvider businessProvider = new BusinessProvider();
            return businessProvider.CheckCodeFindPwd(PhoneNumber);
        }

        /// <summary>
        /// 请求语音动态验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>        
        [HttpPost]
        public SimpleResultModel VoiceCheckCode(SmsParaModel model)
        {
            BusinessProvider businessProvider = new BusinessProvider();
            return businessProvider.VoiceCheckCode(model);

        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <returns></returns>        
        [HttpGet]
        [Token]
        public ResultModel<BusiGetOrderModel[]> GetOrderList_B(int userId, int? pagedSize, int? pagedIndex, sbyte? Status, int? orderfrom,int isCheckStatus=0)
        {
            if (isCheckStatus==1)
            {
                BussinessStatusModel bStatusModel= iBusinessProvider.GetUserStatus(userId);
                if (bStatusModel.status != 1)
                {
                    return ResultModel<BusiGetOrderModel[]>.Conclude(GetOrdersStatus.ErrStatus);
                }
            }

            var pIndex = ParseHelper.ToInt(pagedIndex, 1);
            pIndex = pIndex <= 0 ? 1 : pIndex;
            var pSize = ParseHelper.ToInt(pagedSize, 100);

            BussOrderParaModelApp criteria = new BussOrderParaModelApp()
            {
                PagingResult = new PagingResult(pIndex, pSize),
                userId = userId,
                Status = Status,
                OrderFrom = orderfrom ?? 0
            };
            IList<BusiGetOrderModel> list = new BusinessProvider().GetOrdersApp(criteria);
            return ResultModel<BusiGetOrderModel[]>.Conclude(GetOrdersStatus.Success, list.ToArray());
        }

        /// <summary>
        /// 取消订单 Edit  caoheyang 20150521 
        /// </summary>
        /// <remarks>取消订单时返给商家结算费，优化，大整改</remarks>
        /// <param name="paramodel"></param>
        /// <returns></returns>
        [HttpPost]
        [Token]
        public ResultModel<bool> CancelOrder_B(CancelOrderBPM paramodel)
        {
            return iOrderProvider.CancelOrderB(paramodel);
        }

        /// <summary>
        /// 客服电话获取
        /// 窦海超
        /// 2015年3月16日 11:44:54
        /// </summary>
        /// <param name="CityName">城市名称</param>
        /// <returns></returns>        
        [HttpGet]
        public ResultModel<ResultModelServicePhone> GetCustomerServicePhone(string CityName)
        {
            return ResultModel<ResultModelServicePhone>.Conclude(
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
        [HttpPost]
        [Token]
        public ResultModel<BussinessStatusModel> GetUserStatus(UserStatusModel parModel)
        {
            var model = iBusinessProvider.GetUserStatus(parModel.userId);
            if (model != null)
            {
                return ResultModel<BussinessStatusModel>.Conclude(
                UserStatus.Success,
                model
                );
            }
            return ResultModel<BussinessStatusModel>.Conclude(
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
        [HttpGet]
        [ApiVersionStatistic]
        public ResultModel<AreaModelList> GetOpenCity(string Version)
        {
            AreaProvider area = new AreaProvider();

            return area.GetOpenCity(Version, false);
        }

        #region 第三方调用
        /// <summary>
        /// B端注册，供第三方使用-平扬 2015.3.27修改成 ado方式
        /// </summary>
        /// <param name="model">注册用户基本数据信息</param>
        /// <returns></returns>        
        [HttpPost]
        public ResultModel<NewBusiRegisterResultModel> NewPostRegisterInfo_B(NewRegisterInfoModel model)
        {
            var bprovider = new BusinessProvider();
            return bprovider.NewPostRegisterInfo_B(model);
        }


        /// <summary>
        /// B端取消订单，供第三方使用-2015.3.27-平扬改
        /// </summary>
        /// <param name="model">订单基本数据信息</param>
        /// <returns></returns>        
        [HttpPost]
        public ResultModel<OrderCancelResultModel> NewOrderCancel(OrderCancelModel model)
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
        public ResultModel<NewPostPublishOrderResultModel> NewPostPublishOrder_B(NewPostPublishOrderModel model)
        {
            return new OrderProvider().NewPostPublishOrder_B(model);
        }

        #endregion

        #region 美团等第三方订单处理

        /// <summary>
        /// 获取订单详细
        /// </summary>
        /// <returns></returns>       
        [HttpGet]
        public ResultModel<ListOrderDetailModel> GetOrderDetail(string orderno)
        {
            var model = new OrderProvider().GetOrderDetail(orderno);
            if (model != null)
            {
                return ResultModel<ListOrderDetailModel>.Conclude(GetOrdersStatus.Success, model);
            }
            return ResultModel<ListOrderDetailModel>.Conclude(GetOrdersStatus.FailedGetOrders, model);
        }


        /// <summary>
        /// 商家确认第三方订单接口  平杨
        /// </summary>
        /// <param name="orderlist"></param>
        /// <UpdateBy>确认接入时扣除商家结算费功能  caoheyang 20150526</UpdateBy>
        /// <returns></returns>  
        [HttpGet]
        public ResultModel<List<string>> OtherOrderConfirm_B(string orderlist)
        {
            LogHelper.LogWriterString("参数 ", orderlist);
            if (string.IsNullOrEmpty(orderlist))
                return ResultModel<List<string>>.Conclude(PubOrderStatus.OrderCountError, null);
            var orderProvider = new OrderProvider();
            string[] orders = orderlist.Split(',');
            List<string> errors = new List<string>();
            for (int i = 0; i < orders.Length; i++)
            {
                int res = orderProvider.UpdateOrderStatus(orders[i], OrderStatus.Status0.GetHashCode(), "", OrderStatus.Status30.GetHashCode());
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
        [HttpGet]
        public ResultModel<int> OtherOrderCancel_B(string orderlist, string note)
        {
            if (string.IsNullOrEmpty(orderlist))
            {
                return ResultModel<int>.Conclude(PubOrderStatus.OrderCountError, 0);
            }
            var orderProvider = new OrderProvider();
            string[] orders = orderlist.Split(',');
            int i = orders.Count(s => orderProvider.UpdateOrderStatus(s, OrderStatus.Status3.GetHashCode(), note, OrderStatus.Status30.GetHashCode()) > 0);
            return ResultModel<int>.Conclude(PubOrderStatus.Success, i);
        }

        /// <summary>
        /// 商家拒绝原因接口
        /// </summary>
        /// <param name="orderlist"></param>
        /// <param name="note"></param>
        /// <returns></returns>   
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


        #region 临时 旧接口，现在没有调用
        /// <summary>
        /// 地址管理
        /// 改 ado.net wc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>        
        [HttpPost]
        [Token]
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
                return ResultModel<BusiAddAddressResultModel>.Conclude(BusiAddAddressStatus.BusinessNameEmpty);
            }
            //修改商户地址信息，返回当前商户的状态
            int upResult = iBusinessProvider.UpdateBusinessAddressInfo(model);

            var resultModel = new BusiAddAddressResultModel
            {
                userId = model.userId,
                status = upResult.ToString()
            };
            if (upResult == -1)  //-1表示更新状态失败
            {
                return ResultModel<BusiAddAddressResultModel>.Conclude(BusiAddAddressStatus.UpdateFailed, resultModel);
            }
            else
            {
                return ResultModel<BusiAddAddressResultModel>.Conclude
                    (BusiAddAddressStatus.Success, resultModel);
            }
        }
        /// <summary>
        /// B端订单统计
        /// 改 ado.net
        /// wc
        /// </summary>
        /// <returns></returns>        
        [HttpGet]
        [Token]
        public ResultModel<BusiOrderCountResultModel> OrderCount_B(int userId)
        {
            if (ParseHelper.ToInt(userId, 0) <= 0)
            {
                return ResultModel<BusiOrderCountResultModel>.Conclude(GetOrdersStatus.FailedGetOrders, null);
            }
            var resultModel = new BusinessProvider().GetOrderCountData(userId);
            if (resultModel == null)
            {
                return ResultModel<BusiOrderCountResultModel>.Conclude(GetOrdersStatus.FailedGetOrders, null);
            }
            return ResultModel<BusiOrderCountResultModel>.Conclude(GetOrdersStatus.Success, resultModel);
        }



        /// <summary> 
        /// 商家设置外卖费 平扬 2015.3.5
        /// wangchao  改 ado.net 
        /// </summary>
        /// <returns></returns>        
        [HttpPost]
        [Token]
        public SimpleResultModel PostDistribSubsidy_B(BusiDistribInfoPM mod)
        {
            if (mod.userId <= 0 || mod.price < 0) //判断传入参数是否正常
                return SimpleResultModel.Conclude(DistribSubsidyStatus.Failed);

            var selResult = iBusinessProvider.GetBusiness(mod.userId);
            if (selResult == null) //商户是否存在
                return SimpleResultModel.Conclude(DistribSubsidyStatus.Failed);
            int modResult = iBusinessProvider.ModifyWaiMaiPrice(mod.userId, mod.price);

            if (modResult > 0)
            {
                return SimpleResultModel.Conclude(DistribSubsidyStatus.Success);
            }
            else
            {
                return SimpleResultModel.Conclude(DistribSubsidyStatus.Failed);
            }
        }
        #endregion
    }
}