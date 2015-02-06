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
//using System.Web.Mvc;
using SuperManDataAccess;
using SuperManBusinessLogic.B_Logic;
using SuperManBusinessLogic.Order_Logic;
using SuperManCommonModel.Models;
using SuperManCore.Paging;
using System.Threading.Tasks;
using SuperManCommonModel;
using System.Text;
using System.Net;
namespace SuperManWebApi.Controllers
{
    public class BusinessAPIController : ApiController
    {
        /// <summary>
        ///  B端注册 
        /// </summary>
        /// <param name="model">注册用户基本数据信息</param>
        /// <returns></returns>
        [ActionStatus(typeof(CustomerRegisterStatus))]
        [HttpPost]
        public ResultModel<BusiRegisterResultModel> PostRegisterInfo_B(RegisterInfoModel model)
        {
            if (string.IsNullOrEmpty(model.phoneNo))   //手机号非空验证
                return ResultModel<BusiRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberEmpty);
            else if (BusiLogic.busiLogic().CheckExistPhone(model.phoneNo))  //判断该手机号是否已经注册过
                return ResultModel<BusiRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberRegistered);
            else if (string.IsNullOrEmpty(model.passWord))   //密码非空验证
                return ResultModel<BusiRegisterResultModel>.Conclude(CustomerRegisterStatus.PasswordEmpty);
            else if (model.verifyCode != SupermanApiCaching.Instance.Get(model.phoneNo))  //判断验证法录入是否正确
                return ResultModel<BusiRegisterResultModel>.Conclude(CustomerRegisterStatus.IncorrectCheckCode); //CustomerRegisterStatus用户注册信息枚举
            else if (string.IsNullOrEmpty(model.city) || string.IsNullOrEmpty(model.CityId)) //城市以及城市编码非空验证
                return ResultModel<BusiRegisterResultModel>.Conclude(CustomerRegisterStatus.cityIdEmpty);
            var business = RegisterInfoModelTranslator.Instance.Translate(model);
            bool result = BusiLogic.busiLogic().Add(business);
            var resultModel = new BusiRegisterResultModel
            {
                userId = business.Id
            };
            return ResultModel<BusiRegisterResultModel>.Conclude(CustomerRegisterStatus.Success, resultModel);
        }

        /// <summary>
        /// B端登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(LoginModelStatus))]
        [HttpPost]
        public ResultModel<BusiLoginResultModel> PostLogin_B(LoginModel model)
        {
            var business = BusiLogic.busiLogic().GetBusiness(model.phoneNo, model.passWord);
            if(business==null)
            {
                return ResultModel<BusiLoginResultModel>.Conclude(LoginModelStatus.InvalidCredential);
            }
            var result = new BusiLoginResultModel()
            {
                userId = business.Id,
                status = business.Status,
                city = business.City,
                Address = business.Address,
                districtId = business.districtId,
                district = business.district,
                Landline = business.Landline,
                Name = business.Name,
                cityId = business.CityId,
                phoneNo = business.PhoneNo2 == null ? business.PhoneNo : business.PhoneNo2
            };
            return ResultModel<BusiLoginResultModel>.Conclude(LoginModelStatus.Success, result);
        }

        /// <summary>
        /// 验证图片(审核)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResultModel<UploadIconModel> PostAudit_B()
        {
            var strUserId = HttpContext.Current.Request.Form["UserId"];
            int userId;
            if (!Int32.TryParse(strUserId, out userId))
            {
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.InvalidUserId);
            }

            var business = BusiLogic.busiLogic().GetBusinessById(userId);
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
                img = System.Drawing.Image.FromStream(file.InputStream);
                //if (img.Width < CustomerIconUploader.Instance.Width || img.Height < CustomerIconUploader.Instance.Height)
                //{
                //    return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.InvalidFileFormat);
                //}
                var fileName = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMddhhmmss"), file.FileName);
                if (!System.IO.Directory.Exists(CustomerIconUploader.Instance.PhysicalPath))
                {
                    System.IO.Directory.CreateDirectory(CustomerIconUploader.Instance.PhysicalPath);
                }
                var fullFilePath = System.IO.Path.Combine(CustomerIconUploader.Instance.PhysicalPath, fileName);

                file.SaveAs(fullFilePath);
                var transformer = new FixedDimensionTransformerAttribute(CustomerIconUploader.Instance.Width, CustomerIconUploader.Instance.Height, CustomerIconUploader.Instance.MaxBytesLength / 1024);
                var destFileName = string.Format("{0}_{1}{2}", DateTime.Now.ToString("yyyyMMddhhmmss"), new Random().Next(1000), Path.GetExtension(file.FileName));
                var destFullFileName = System.IO.Path.Combine(CustomerIconUploader.Instance.PhysicalPath, destFileName);
                transformer.Transform(fullFilePath, destFullFileName);


                var picUrl = System.IO.Path.GetFileName(destFullFileName);
                var _status = BusiLogic.busiLogic().UpdateBusi(business, picUrl);

                var relativePath = System.IO.Path.Combine(CustomerIconUploader.Instance.RelativePath, destFileName).ToForwardSlashPath();
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.Success, new UploadIconModel() { Id = 1, ImagePath = relativePath, status = _status });
            }
            catch (Exception)
            {
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.InvalidFileFormat);
            }
        }


        /// <summary>
        /// 发布订单
        /// </summary>
        /// <returns></returns>
        [ActionStatus(typeof(PubOrderStatus))]
        [HttpPost]
        public ResultModel<BusiOrderResultModel> PostPublishOrder_B(BusiOrderInfoModel model)
        { 
            //System.Diagnostics.Debug.WriteLine("getPost" + Guid.NewGuid());
            order dborder = BusiOrderInfoModelTranslator.Instance.Translate(model);  //整合订单信息
            bool result = OrderLogic.orderLogic().AddModel(dborder);    //添加订单记录，并且触发极光推送。          
            if(result)
            {
                BusiOrderResultModel resultModel = new BusiOrderResultModel { userId = model.userId };
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.Success, resultModel);
            }
            else
            {
                return ResultModel<BusiOrderResultModel>.Conclude(PubOrderStatus.InvalidPubOrder);
            }            
        }
        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <returns></returns>
        [ActionStatus(typeof(GetOrdersStatus))]
        [HttpGet]
        public ResultModel<BusiGetOrderModel[]> GetOrderList_B(int userId, int? pagedSize, int? pagedIndex, sbyte? Status)
        {
            var pIndex = pagedIndex.HasValue ? pagedIndex.Value : 0;
           var pSize = pagedSize.HasValue ? pagedSize.Value : int.MaxValue;
            var criteria = new BusiOrderSearchCriteria()
            {
                 PagingRequest = new PagingResult(pIndex, pSize),
                userId = userId,
                Status = Status
            };
            var pagedList = OrderLogic.orderLogic().GetOrders(criteria);
            var list = BusiGetOrderModelTranslator.Instance.Translate(pagedList);
            return ResultModel<BusiGetOrderModel[]>.Conclude(GetOrdersStatus.Success, list.ToArray());
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
            if (string.IsNullOrEmpty(model.phoneNo))
            {
                return ResultModel<BusiAddAddressResultModel>.Conclude(BusiAddAddressStatus.PhoneNumberEmpty);
            }
            if (string.IsNullOrEmpty(model.Address))
            {
                return ResultModel<BusiAddAddressResultModel>.Conclude(BusiAddAddressStatus.AddressEmpty);
            }
            if (string.IsNullOrEmpty(model.businessName))
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
        /// B端订单统计
        /// </summary>
        /// <returns></returns>
        [ActionStatus(typeof(LoginModelStatus))]
        [HttpGet]
        public ResultModel<BusiOrderCountResultModel> OrderCount_B(int userId)
        {
            var resultModel = BusiLogic.busiLogic().GetOrderCountData(userId);
            return ResultModel<BusiOrderCountResultModel>.Conclude(LoginModelStatus.Success, resultModel);
        }

        /// <summary>
        /// 请求动态验证码  (注册)
        /// c</summary>
        [ActionStatus(typeof(SendCheckCodeStatus))]
        [HttpGet]
        public SimpleResultModel CheckCode(string PhoneNumber)
        {
            if (!CommonValidator.IsValidPhoneNumber(PhoneNumber))  //验证电话号码合法性
            {
                return SimpleResultModel.Conclude(SendCheckCodeStatus.InvlidPhoneNumber);
            }
            var randomCode = new Random().Next(100000).ToString("D6");  //生成短信验证码
            var msg = string.Format(SupermanApiConfig.Instance.SmsContentCheckCode, randomCode,ConstValues.MessageBusiness);  //获取提示用语信息
            try
            {
                SupermanApiCaching.Instance.Add(PhoneNumber, randomCode);
                 //更新短信通道 
                Task.Factory.StartNew(() =>
                {
                    SendSmsHelper.SendSendSmsSaveLog(PhoneNumber, msg, ConstValues.SMSSOURCE);
                });
                return SimpleResultModel.Conclude(SendCheckCodeStatus.Sending);  
            }
            catch (Exception)
            {
                return SimpleResultModel.Conclude(SendCheckCodeStatus.SendFailure);
            }  
        }

        /// <summary>
        /// 请求动态验证码  (找回密码)
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        [ActionStatus(typeof(SendCheckCodeStatus))]
        [HttpGet]
        public SimpleResultModel CheckCodeFindPwd(string PhoneNumber)
        {
            if (!CommonValidator.IsValidPhoneNumber(PhoneNumber))  //检查手机号码的合法性
            {
                return SimpleResultModel.Conclude(SendCheckCodeStatus.InvlidPhoneNumber);
            }
            var randomCode = new Random().Next(100000).ToString("D6");
            var msg = string.Format(SupermanApiConfig.Instance.SmsContentFindPassword, randomCode,ConstValues.MessageBusiness);
            try
            {                
                SupermanApiCaching.Instance.Add(PhoneNumber, randomCode);
                // 更新短信通道 
                Task.Factory.StartNew(() =>
                {
                    SendSmsHelper.SendSendSmsSaveLog(PhoneNumber, msg, ConstValues.SMSSOURCE);
                });
                return SimpleResultModel.Conclude(SendCheckCodeStatus.Sending);

            }
            catch (Exception)
            {
                return SimpleResultModel.Conclude(SendCheckCodeStatus.SendFailure);
            }
        }

        /// <summary>
        /// b端修改密码 edit by caoheyang 20150203 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ForgetPwdStatus))]
        [HttpPost]
        public ResultModel<BusiModifyPwdResultModel> PostForgetPwd_B(BusiForgetPwdInfoModel model)
        {
            if (string.IsNullOrEmpty(model.password))  //密码非空验证
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.NewPwdEmpty);
            if (string.IsNullOrEmpty(model.checkCode)) //验证码非空验证
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.checkCodeIsEmpty);
            if (SupermanApiCaching.Instance.Get(model.phoneNumber) != model.checkCode) //验证码正确性验证
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.checkCodeWrong);
            var business = BusiLogic.busiLogic().GetBusinessByPhoneNo(model.phoneNumber);
            if (business == null)  //用户是否存在
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.ClienterIsNotExist);
            if (business.Password == model.password) //您要找回的密码正是当前密码
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.PwdIsSave);
            if (BusiLogic.busiLogic().ModifyPwd(business.Id, model.password))
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.Success);
            else
                return ResultModel<BusiModifyPwdResultModel>.Conclude(ForgetPwdStatus.FailedModifyPwd);
        }
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <returns></returns>
        [ActionStatus(typeof(CancelOrderStatus))]
        [HttpGet]
        public ResultModel<bool> CancelOrder_B(string userId, string OrderId)
        {
            if (OrderId == null)
            {
                return ResultModel<bool>.Conclude(CancelOrderStatus.OrderEmpty);
            }
            var order = OrderLogic.orderLogic().GetOrderById(OrderId);
            if (order == null)
            {
                return ResultModel<bool>.Conclude(CancelOrderStatus.OrderIsNotExist);
            }
            bool b= OrderLogic.orderLogic().UpdateOrder(order, OrderStatus.订单已取消);
            if(b==true)
            {
                return ResultModel<bool>.Conclude(CancelOrderStatus.Success, true);
            }
            else
            {
                return ResultModel<bool>.Conclude(CancelOrderStatus.FailedCancelOrder, true);
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
    }
}