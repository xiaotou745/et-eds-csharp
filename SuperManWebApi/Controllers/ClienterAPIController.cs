﻿using SuperManCore.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using SuperManWebApi.Models.Clienter;
using SuperManCore;
using SuperManWebApi.Models.Business;
using SuperManBusinessLogic.C_Logic;
using System.IO;
using SuperManCore.Paging;
using SuperManCommonModel.Entities;
using SuperManBusinessLogic.CommonLogic;
using System.Threading.Tasks;
using SuperManDataAccess;
using SuperManCommonModel;
namespace SuperManWebApi.Controllers
{
    public class ClienterAPIController : ApiController
    {
        /// <summary>
        /// C端注册 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(CustomerRegisterStatus))]
        [HttpPost]
        public ResultModel<ClientRegisterResultModel> PostRegisterInfo_C(ClientRegisterInfoModel model)
        {
            if (string.IsNullOrEmpty(model.phoneNo))
            {
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberEmpty);
            }
            if (ClienterLogic.clienterLogic().CheckExistPhone(model.phoneNo))
            {
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberRegistered);
            }
            if (string.IsNullOrEmpty(model.passWord))
            {
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PasswordEmpty);
            }
            //验证码
            //if (model.verifyCode != SupermanApiCaching.Instance.Get(model.phoneNo))
            //{
            //    return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.IncorrectCheckCode);
            //}
            var clienter = ClientRegisterInfoModelTranslator.Instance.Translate(model);
            bool result = ClienterLogic.clienterLogic().Add(clienter);
            
            var resultModel = new ClientRegisterResultModel
            {
                userId = clienter.Id
            };
            return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.Success, resultModel);
        }

        /// <summary>
        /// C端登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(LoginModelStatus))]
        public ResultModel<ClienterLoginResultModel> PostLogin_C(LoginModel model)
        {
            var business = ClienterLogic.clienterLogic().GetClienter(model.phoneNo, model.passWord);
            if (business == null)
            {
                return ResultModel<ClienterLoginResultModel>.Conclude(LoginModelStatus.InvalidCredential);
            }
            var result = new ClienterLoginResultModel()
            {
                userId = business.Id,
                phoneNo=business.PhoneNo,
                status = business.Status,
                Amount = business.AccountBalance
            };
            return ResultModel<ClienterLoginResultModel>.Conclude(LoginModelStatus.Success, result);
        }
        /// <summary>
        /// C端上传图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResultModel<UploadIconModel> PostAudit_C()
        {
            if (HttpContext.Current.Request.Form.Count == 0)
            {
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.NOFormParameter);
            }
            var strUserId = HttpContext.Current.Request.Form["userId"]; //用户Id
            var strIDCard = HttpContext.Current.Request.Form["IDCard"]; //身份证号
            var trueName = HttpContext.Current.Request.Form["trueName"]; //真实姓名

            var customer = ClienterLogic.clienterLogic().GetClienterById(int.Parse(strUserId));
            if (customer == null)
            {
               return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.InvalidUserId);
            }
            if (HttpContext.Current.Request.Files.Count == 0)
            {
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.InvalidFileFormat);
            }
            if(string.IsNullOrEmpty(trueName))
            {
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.TrueNameEmpty);
            }
            var fileHand = HttpContext.Current.Request.Files[0]; //手持照片
            var file = HttpContext.Current.Request.Files[1]; //照片
            System.Drawing.Image imgHand;
            System.Drawing.Image img;
            try
            {
                imgHand = System.Drawing.Image.FromStream(fileHand.InputStream);
                img = System.Drawing.Image.FromStream(file.InputStream);
                //if (img.Width < CustomerIconUploader.Instance.Width || img.Height < CustomerIconUploader.Instance.Height)
                //{
                //    return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.InvalidFileFormat);
                //}
            }
            catch (Exception)
            {
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.InvalidFileFormat);
            }

            var fileName = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMddhhmmss"), file.FileName);
            var fileHandName = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMddhhmmss"), fileHand.FileName);
            if (!System.IO.Directory.Exists(CustomerIconUploader.Instance.PhysicalPath))
            {
                System.IO.Directory.CreateDirectory(CustomerIconUploader.Instance.PhysicalPath);
            }
            var fullFilePath = Path.Combine(CustomerIconUploader.Instance.PhysicalPath, fileName);
            var fullFileHandPath = Path.Combine(CustomerIconUploader.Instance.PhysicalPath, fileHandName);
            file.SaveAs(fullFilePath);
            fileHand.SaveAs(fullFileHandPath);
            var transformer = new FixedDimensionTransformerAttribute(CustomerIconUploader.Instance.Width, CustomerIconUploader.Instance.Height, CustomerIconUploader.Instance.MaxBytesLength / 1024);

            var destFileName = string.Format("{0}_{1}{2}", DateTime.Now.ToString("yyyyMMddhhmmss"), new Random().Next(1000), Path.GetExtension(file.FileName));
            var destFullFileName = System.IO.Path.Combine(CustomerIconUploader.Instance.PhysicalPath, destFileName);
            transformer.Transform(fullFilePath, destFullFileName);

            var destFileHandName = string.Format("{0}_{1}{2}", DateTime.Now.ToString("yyyyMMddhhmmss"), new Random().Next(1000), Path.GetExtension(fileHand.FileName));
            var destFullFileHandName = System.IO.Path.Combine(CustomerIconUploader.Instance.PhysicalPath, destFileHandName);
            transformer.Transform(fullFileHandPath, destFullFileHandName);

            var picUrl = System.IO.Path.GetFileName(destFullFileName);
            var picUrlWithHand = System.IO.Path.GetFileName(destFullFileHandName);
            ClienterLogic.clienterLogic().UpdateClient(customer, picUrl, picUrlWithHand, trueName, strIDCard);

            var relativePath = System.IO.Path.Combine(CustomerIconUploader.Instance.RelativePath, destFileName).ToForwardSlashPath();
            return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.Success, new UploadIconModel() { Id = 1, ImagePath = relativePath });
        }

        /// <summary>
        /// C端获取我的任务列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(GetOrdersStatus))]
        [HttpPost]
        public ResultModel<ClientOrderResultModel[]> GetJobList_C(ClientOrderInfoModel model)
        {
            degree.longitude = model.longitude;
            degree.latitude = model.latitude;
            var pIndex = model.pageIndex.HasValue ? model.pageIndex.Value : 0;
            var pSize = model.pageSize.HasValue ? model.pageIndex.Value : int.MaxValue;
            var criteria = new ClientOrderSearchCriteria()
            {
                PagingRequest = new PagingResult(pIndex, pSize),
                userId = model.userId,
                status = model.status,
                isLatest=model.isLatest
            };
            var pagedList = ClienterLogic.clienterLogic().GetOrders(criteria);
            var lists = ClientOrderResultModelTranslator.Instance.Translate(pagedList);
            //if (!model.isLatest) //不是最新任务的话就按距离排序,否则按发布时间排序
            //{
            lists = lists.OrderBy(i => i.distance).ToList();
            //}
            return ResultModel<ClientOrderResultModel[]>.Conclude(GetOrdersStatus.Success, lists.ToArray());
        }

        [ActionStatus(typeof(GetOrdersStatus))]
        [HttpPost]
        public ResultModel<ClientOrderResultModel[]> GetMyJobList_C(ClientOrderInfoModel model)
        {
            degree.longitude = model.longitude;
            degree.latitude = model.latitude;
            var pIndex = model.pageIndex.HasValue ? model.pageIndex.Value : 0;
            var pSize = model.pageSize.HasValue ? model.pageIndex.Value : int.MaxValue;
            var criteria = new ClientOrderSearchCriteria()
            {
                PagingRequest = new PagingResult(pIndex, pSize),
                userId = model.userId,
                status = model.status,
                isLatest = model.isLatest
            };
            var pagedList = ClienterLogic.clienterLogic().GetMyOrders(criteria);
            var lists = ClientOrderResultModelTranslator.Instance.Translate(pagedList);
            if (!model.isLatest) //不是最新任务的话就按距离排序,否则按发布时间排序
            {
                lists = lists.OrderBy(i => i.distance).ToList();
            }
            return ResultModel<ClientOrderResultModel[]>.Conclude(GetOrdersStatus.Success, lists.ToArray());
        }

        /// <summary>
        /// C端未登录时首页获取任务列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(GetOrdersNoLoginStatus))]
        [HttpPost]
        public ResultModel<ClientOrderNoLoginResultModel[]> GetJobListNoLogin_C(ClientOrderInfoModel model)
        {
            degree.longitude = model.longitude;
            degree.latitude = model.latitude;
            var pIndex = model.pageIndex.HasValue ? model.pageIndex.Value : 0;
            var pSize = model.pageSize.HasValue ? model.pageIndex.Value : int.MaxValue;
            var criteria = new ClientOrderSearchCriteria()
            {
                PagingRequest = new PagingResult(pIndex, pSize),
                userId = model.userId,
                status = model.status,
                isLatest = model.isLatest
            };
            var pagedList = ClienterLogic.clienterLogic().GetOrdersNoLogin(criteria);
            var lists = ClientOrderNoLoginResultModelTranslator.Instance.Translate(pagedList);
            if(!model.isLatest) //不是最新任务的话就按距离排序,否则按发布时间排序
            {
                lists = lists.OrderBy(i => i.distance).ToList();
            }
            return ResultModel<ClientOrderNoLoginResultModel[]>.Conclude(GetOrdersNoLoginStatus.Success, lists.ToArray());
        }

        /// <summary>
        /// 未登录时获取最新任务
        /// </summary>
        /// <returns></returns>
        [ActionStatus(typeof(GetOrdersNoLoginStatus))]
        [HttpGet]
        public ResultModel<ClientOrderNoLoginResultModel[]> GetJobListNoLoginLatest_C()
        {
            degree.longitude = 0;
            degree.latitude = 0;
            var pagedList = ClienterLogic.clienterLogic().GetOrdersNoLoginLatest();
            var lists = ClientOrderNoLoginResultModelTranslator.Instance.Translate(pagedList);
            return ResultModel<ClientOrderNoLoginResultModel[]>.Conclude(GetOrdersNoLoginStatus.Success, lists.ToArray());
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ModifyPwdStatus))]
        [HttpPost]
        public ResultModel<ClienterModifyPwdResultModel> PostModifyPwd_C(ModifyPwdInfoModel model)
        {
            if (string.IsNullOrEmpty(model.newPassword))
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ModifyPwdStatus.NewPwdEmpty);
            }
            var clienter = ClienterLogic.clienterLogic().GetClienter(model.phoneNo);
            if (clienter == null)
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ModifyPwdStatus.ClienterIsNotExist);
            }
            if (clienter.Password == model.newPassword)
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ModifyPwdStatus.PwdIsSame);
            }
            bool b = ClienterLogic.clienterLogic().ModifyPwd(clienter, model.newPassword);
            if (b)
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ModifyPwdStatus.Success);
            }
            else
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ModifyPwdStatus.FailedModifyPwd);
            }
        }
        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ForgetPwdStatus))]
        [HttpPost]
        public ResultModel<ClienterModifyPwdResultModel> PostForgetPwd_C(ForgetPwdInfoModel model)
        {
            if (string.IsNullOrEmpty(model.password))
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ForgetPwdStatus.NewPwdEmpty);
            }
            if (string.IsNullOrEmpty(model.checkCode))
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ForgetPwdStatus.checkCodeIsEmpty);
            }
            //start 需要验证 验证码是否正确
            //if (SupermanApiCaching.Instance.Get(model.phoneNo) != model.checkCode)
            //{
            //    return ResultModel<ClienterModifyPwdResultModel>.Conclude(ForgetPwdStatus.checkCodeWrong);
            //}
            //end
            var clienter = ClienterLogic.clienterLogic().GetClienter(model.phoneNo);
            if (clienter == null)
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ForgetPwdStatus.ClienterIsNotExist);
            }
            if (clienter.Password == model.password)
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ForgetPwdStatus.PwdIsSave);
            }
            bool b = ClienterLogic.clienterLogic().ModifyPwd(clienter, model.password);
            if (b)
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ForgetPwdStatus.Success);
            }
            else
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ForgetPwdStatus.FailedModifyPwd);
            }
        }
        /// <summary>
        /// 超人抢单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [ActionStatus(typeof(RushOrderStatus))]
        [HttpGet]
        public ResultModel<RushOrderResultModel> RushOrder_C(int userId, string orderNo)
        {
            if (userId==0)
            {
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.userIdEmpty);
            }
            if (string.IsNullOrEmpty(orderNo))
            {
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.OrderEmpty);
            }
            if (ClienterLogic.clienterLogic().GetOrderByNo(orderNo) == null)
            {
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.OrderIsNotExist);
            }
            if (!ClienterLogic.clienterLogic().CheckOrderIsAllowRush(orderNo))
            {
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.OrderIsNotAllowRush);
            }
            bool bResult = ClienterLogic.clienterLogic().RushOrder(userId, orderNo);
            if (bResult)
            {
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.Success);
            }
            else
            {
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.Failed);
            }
        }
        /// <summary>
        /// 完成订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [ActionStatus(typeof(FinishOrderStatus))]
        [HttpGet]
        public ResultModel<FinishOrderResultModel> FinishOrder_C(int userId, string orderNo)
        {
            if (userId == 0)
            {
                return ResultModel<FinishOrderResultModel>.Conclude(FinishOrderStatus.userIdEmpty);
            }
            if (string.IsNullOrEmpty(orderNo))
            {
                return ResultModel<FinishOrderResultModel>.Conclude(FinishOrderStatus.OrderEmpty);
            }
            if (ClienterLogic.clienterLogic().GetOrderByNo(orderNo) == null)
            {
                return ResultModel<FinishOrderResultModel>.Conclude(FinishOrderStatus.OrderIsNotExist);
            }
            int bResult = ClienterLogic.clienterLogic().FinishOrder(userId, orderNo);

            if (bResult == 2)
            {
                var clienter = ClienterLogic.clienterLogic().GetClienterById(userId);
                var model = new FinishOrderResultModel();
                model.userId = userId;
                if (clienter.AccountBalance != null)
                    model.balanceAmount = clienter.AccountBalance.Value;
                else
                    model.balanceAmount = 0.0m;
                return ResultModel<FinishOrderResultModel>.Conclude(FinishOrderStatus.Success, model);
            }
            else if (bResult == 1)
            {
                return ResultModel<FinishOrderResultModel>.Conclude(FinishOrderStatus.OrderIsNotAllowRush);
            }
            else
            {
                return ResultModel<FinishOrderResultModel>.Conclude(FinishOrderStatus.Failed);
            }
        }
        /// <summary>
        /// 获取我的余额
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        [ActionStatus(typeof(RushOrderStatus))]
        [HttpGet]
        public ResultModel<MyBalanceResultModel> GetMyBalance(string phoneNo)
        {
            if(string.IsNullOrEmpty(phoneNo))
            {
                return ResultModel<MyBalanceResultModel>.Conclude(GetMyBalanceStatus.PhoneEmpty);
            }
            var item = ClienterLogic.clienterLogic().GetMyBalanceByPhoneNo(phoneNo);
            var result = new MyBalanceResultModel()
            {
                MyBalance = item
            };
            return ResultModel<MyBalanceResultModel>.Conclude(FinishOrderStatus.Success,result);
        }

        /// <summary>
        /// 获取我的余额动态
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        [ActionStatus(typeof(RushOrderStatus))]
        [HttpGet]
        public ResultModel<MyBalanceListResultModel[]> GetMyBalanceDynamic(string phoneNo, int? pagedSize, int? pagedIndex)
        {
            var pIndex = pagedIndex.HasValue ? pagedIndex.Value : 0;
            var pSize = pagedSize.HasValue ? pagedSize.Value : int.MaxValue;
            var criteria = new MyIncomeSearchCriteria()
            {
                PagingRequest = new PagingResult(pIndex, pSize),
                phoneNo = phoneNo
            };
            var pagedList = ClienterLogic.clienterLogic().GetMyIncomeList(criteria);
            var lists = MyBalanceListResultModelTranslator.Instance.Translate(pagedList);
            return ResultModel<MyBalanceListResultModel[]>.Conclude(RushOrderStatus.Success, lists.ToArray());
        }

        /// <summary>
        /// 请求动态验证码 
        /// c</summary>
        [ActionStatus(typeof(SendCheckCodeStatus))]
        [HttpGet]
        public SimpleResultModel CheckCode(string PhoneNumber,string type)
        {
            if (!CommonValidator.IsValidPhoneNumber(PhoneNumber))
            {
                return SimpleResultModel.Conclude(SendCheckCodeStatus.InvlidPhoneNumber);
            }

            var randomCode = new Random().Next(100000).ToString("D6");
            string msg = string.Empty;
            if (type == "0")//注册
            {
                msg = string.Format(SupermanApiConfig.Instance.SmsContentCheckCode, randomCode,ConstValues.MessageBusiness);  
            }
            else //修改密码
            {
                msg = string.Format(SupermanApiConfig.Instance.SmsContentFindPassword, randomCode,ConstValues.MessageBusiness);
            }
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

        [HttpGet]
        public SimpleResultModel testPush()
        {
            //Push.PushMessage(0, "有新订单了！", "有新的订单可以抢了！", "有新的订单可以抢了！", string.Empty);
            Push.PushMessage(1, "订单提醒", "有订单被抢了！", "有超人抢了订单！", "14");
            return SimpleResultModel.Conclude(SendCheckCodeStatus.Sending);
        }
    }
}