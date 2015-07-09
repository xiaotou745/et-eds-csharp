﻿using ETS.Const;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Service.Provider.Order;
using Ets.Service.Provider.WtihdrawRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using ETS.Util;
using Ets.Service.Provider.Clienter;
using Ets.Service.Provider.Common;
using SuperManWebApi.Providers;
using Ets.Model.DataModel.Order;
using ETS.Extension;
using Ets.Service.IProvider.Clienter;
using Ets.Service.IProvider.Common;
using Ets.Service.IProvider.Order;
using Ets.Model.ParameterModel.Clienter;
using Ets.Model.DomainModel.Clienter;
using System.IO;
using ETS.Sms;
using Ets.Model.DataModel.Clienter;
using Ets.Model.DomainModel;
using ETS.NoSql.RedisCache;
using Ets.Model.ParameterModel.WtihdrawRecords;
using ETS.Validator;
using Ets.Model.ParameterModel.Sms;
using Ets.Model.ParameterModel.Order;
namespace SuperManWebApi.Controllers
{
    public class ClienterAPIController : ApiController
    {
        readonly IClienterProvider iClienterProvider = new ClienterProvider();
        readonly IAreaProvider iAreaProvider = new AreaProvider();
        readonly IOrderProvider iOrderProvider = new OrderProvider();

        /// <summary>
        /// C端注册 -平扬 2015.3.30
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>        
        [HttpPost]
        public ResultModel<ClientRegisterResultModel> PostRegisterInfo_C(ClientRegisterInfoModel model)
        {
            return iClienterProvider.PostRegisterInfo_C(model);
        }

        /// <summary>
        /// C端登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>        
        public ResultModel<ClienterLoginResultModel> PostLogin_C(LoginCPM model)
        {
            return new ClienterProvider().PostLogin_C(model);
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
            var strIdCard = HttpContext.Current.Request.Form["IDCard"]; //身份证号
            var trueName = HttpContext.Current.Request.Form["trueName"]; //真实姓名
            if (!iClienterProvider.CheckClienterExistById(int.Parse(strUserId)))
            {
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.InvalidUserId);
            }
            if (HttpContext.Current.Request.Files.Count == 0)
            {
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.InvalidFileFormat);
            }
            if (string.IsNullOrEmpty(trueName))
            {
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.TrueNameEmpty);
            }
            var fileHand = HttpContext.Current.Request.Files[0]; //手持照片
            var file = HttpContext.Current.Request.Files[1]; //照片

            ImageHelper ih = new ImageHelper();
            //手持照片
            ImgInfo handImg = ih.UploadImg(fileHand, ParseHelper.ToInt(strUserId, 0));
            //身份证照片
            ImgInfo sfhImg = ih.UploadImg(file, ParseHelper.ToInt(strUserId, 0));

            var upResult = iClienterProvider.UpdateClientPicInfo(new ClienterModel { Id = int.Parse(strUserId), PicUrl = sfhImg.PicUrl, PicWithHandUrl = handImg.PicUrl, TrueName = trueName, IDCard = strIdCard });
            if (!upResult)
            {
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.UpFailed, new UploadIconModel() { Id = ParseHelper.ToInt(strUserId), ImagePath = "" });
            }
            var relativePath = Path.Combine(CustomerIconUploader.Instance.RelativePath, sfhImg.FileName).ToForwardSlashPath();
            return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.Success, new UploadIconModel() { Id = ParseHelper.ToInt(strUserId), ImagePath = relativePath });
        }
        /// <summary>
        /// 获取我的任务   根据状态判断是已完成任务还是我的任务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>        
        [ExecuteTimeLog]
        [HttpPost]
        public ResultModel<ClientOrderResultModel[]> GetMyJobList_C(ClientOrderInfoModel model)
        {
            degree.longitude = model.longitude;
            degree.latitude = model.latitude;
            var pIndex = ParseHelper.ToInt(model.pageIndex, 1);

            var pSize = ParseHelper.ToInt(model.pageSize, PageSizeType.App_PageSize.GetHashCode());

            var criteria = new ClientOrderSearchCriteria()
            {
                PagingRequest = new PagingResult(pIndex, pSize),
                userId = model.userId,
                status = model.status,
                isLatest = model.isLatest
            };
            IList<ClientOrderResultModel> lists = new ClienterProvider().GetMyOrders(criteria);
            if (model.status != null && model.status != 1)
            {
                lists = lists.OrderBy(i => i.distance_OrderBy).ToList();
            }
            return ResultModel<ClientOrderResultModel[]>.Conclude(GetOrdersStatus.Success, lists.ToArray());
        }



        /// <summary>
        /// Ado.net  add  王超
        /// C端获取我的任务列表 最近任务 登录未登录根据城市有没有值判断。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>      
        [ExecuteTimeLog]
        [HttpPost]
        public ResultModel<ClientOrderResultModel[]> GetJobList_C(ClientOrderInfoModel model)
        {
            degree.longitude = model.longitude;
            degree.latitude = model.latitude;
            var pIndex = ParseHelper.ToInt(model.pageIndex, 1);
            var pSize = ParseHelper.ToInt(model.pageSize, PageSizeType.App_PageSize.GetHashCode());
            var criteria = new ClientOrderSearchCriteria()
            {
                PagingRequest = new PagingResult(pIndex, pSize),
                userId = model.userId,
                status = model.status,
                isLatest = model.isLatest,
                city = string.IsNullOrWhiteSpace(model.city) ? null : model.city.Trim(),
                cityId = string.IsNullOrWhiteSpace(model.cityId) ? null : model.cityId.Trim()
            };

            var pagedList = new OrderProvider().GetOrders(criteria);
            pagedList = pagedList.OrderBy(i => i.distance_OrderBy).ToList();
            return ResultModel<ClientOrderResultModel[]>.Conclude(GetOrdersStatus.Success, pagedList.ToArray());
        }

        /// <summary>
        /// Ado.net add 王超
        /// 未登录时获取最新任务     登录未登录根据城市有没有值判断。
        /// </summary>
        /// <returns></returns>        
        [ExecuteTimeLog]
        [HttpGet]
        public ResultModel<ClientOrderNoLoginResultModel[]> GetJobListNoLoginLatest_C()
        {
            ClientOrderInfoModel model = new ClientOrderInfoModel();
            model.city = string.IsNullOrWhiteSpace(HttpContext.Current.Request["city"]) ? null : HttpContext.Current.Request["city"].Trim();//城市
            model.cityId = string.IsNullOrWhiteSpace(HttpContext.Current.Request["cityId"]) ? null : HttpContext.Current.Request["cityId"].Trim(); //城市编码
            degree.longitude = ParseHelper.ToDouble(HttpContext.Current.Request["longitude"]);
            degree.latitude = ParseHelper.ToDouble(HttpContext.Current.Request["latitude"]);
            var pIndex = ParseHelper.ToInt(model.pageIndex, 1);
            var pSize = ParseHelper.ToInt(model.pageSize, PageSizeType.App_PageSize.GetHashCode());
            var criteria = new ClientOrderSearchCriteria()
            {
                PagingRequest = new PagingResult(pIndex, pSize),
                city = model.city,
                cityId = model.cityId
            };

            var pagedList = new OrderProvider().GetOrdersNoLoginLatest(criteria);
            pagedList = pagedList.OrderBy(i => i.distance_OrderBy).ToList();

            return ResultModel<ClientOrderNoLoginResultModel[]>.Conclude(GetOrdersNoLoginStatus.Success, pagedList.ToArray());
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>        
        [HttpPost]
        public ResultModel<ClienterModifyPwdResultModel> PostModifyPwd_C(ModifyPwdInfoModel model)
        {

            ClienterProvider cliProvider = new ClienterProvider();
            return cliProvider.PostForgetPwd_C(model);
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>        
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
            var redis = new RedisCache();
            var code = redis.Get<string>(RedissCacheKey.PostForgetPwd_C + model.phoneNo);
            //start 需要验证 验证码是否正确
            if (string.IsNullOrEmpty(code) || code != model.checkCode)
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ForgetPwdStatus.checkCodeWrong);
            }
            //end

            var clienter = iClienterProvider.GetUserInfoByUserPhoneNo(model.phoneNo);
            if (clienter == null)
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ForgetPwdStatus.ClienterIsNotExist);
            }
            if (clienter.Password == model.password)
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ForgetPwdStatus.PwdIsSave);
            }

            bool b = iClienterProvider.UpdateClienterPwdByUserId(clienter.Id, model.password);
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
        /// 平扬  2015.3.30
        /// 窦海超 更新 2015年5月6日 21:00:23
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [ExecuteTimeLog]
        [HttpGet]
        public ResultModel<RushOrderResultModel> RushOrder_C(int userId, string orderNo)
        {
            return new ClienterProvider().RushOrder_C(userId, orderNo);
        }

        /// <summary>
        /// 获取我的余额
        /// </summary>
        /// <param name="phoneNo">手机号</param>
        /// <returns></returns>     
        [HttpGet]
        public ResultModel<Ets.Model.DataModel.Clienter.MyBalanceResultModel> GetMyBalance(string phoneNo)
        {
            if (string.IsNullOrEmpty(phoneNo))
            {
                return ResultModel<Ets.Model.DataModel.Clienter.MyBalanceResultModel>.Conclude(ETS.Enums.GetMyBalanceStatus.PhoneEmpty);
            }
            var item = iClienterProvider.GetUserInfoByUserPhoneNo(phoneNo);
            var result = new Ets.Model.DataModel.Clienter.MyBalanceResultModel()
            {
                MyBalance = item.AccountBalance ?? 0
            };
            return ResultModel<Ets.Model.DataModel.Clienter.MyBalanceResultModel>.Conclude(ETS.Enums.FinishOrderStatus.Success, result);
        }

        /// <summary>
        /// 获取我的余额动态
        /// </summary>
        /// <param name="phoneNo">手机号</param>
        /// <returns></returns>        
        [HttpGet]
        public ResultModel<MyBalanceListResultModel[]> GetMyBalanceDynamic(string phoneNo, int? pagedSize, int? pagedIndex)
        {
            int pIndex = ParseHelper.ToInt(pagedIndex.HasValue, 1);
            int pSize = ParseHelper.ToInt(pagedSize.HasValue, PageSizeType.App_PageSize.GetHashCode());
            var criteria = new MyIncomeSearchCriteria()
            {
                PagingRequest = new PagingResult(pIndex, pSize),
                phoneNo = phoneNo
            };

            var withrecord = new WtihdrawRecordsProvider();
            var pagedList = withrecord.GetMyIncomeList(criteria);
            var lists = MyBalanceListResultModelTranslator.Instance.Translate(pagedList);
            return ResultModel<MyBalanceListResultModel[]>.Conclude(RushOrderStatus.Success, lists.ToArray());
        }

        /// <summary>
        /// 请求动态验证码
        /// </summary>
        /// <param name="PhoneNumber">手机号</param>
        /// <param name="type">操作类型： 0 注册 1修改密码</param>
        /// <returns></returns>      
        [HttpGet]
        public SimpleResultModel CheckCode(string PhoneNumber, string type)
        {
            if (!CommonValidator.IsValidPhoneNumber(PhoneNumber))
            {
                return SimpleResultModel.Conclude(SendCheckCodeStatus.InvlidPhoneNumber);
            }
            var randomCode = new Random().Next(1000).ToString("D4");
            string msg = string.Empty;
            string key = "";
            bool checkUser = iClienterProvider.CheckClienterExistPhone(PhoneNumber);

            if (type == "0")//注册
            {
                if (checkUser)  //判断该手机号是否已经注册过
                {
                    return SimpleResultModel.Conclude(SendCheckCodeStatus.AlreadyExists);
                }
                key = RedissCacheKey.PostRegisterInfo_C + PhoneNumber;
                msg = string.Format(SupermanApiConfig.Instance.SmsContentCheckCode, randomCode, SystemConst.MessageClinenter);
            }
            else //修改密码
            {
                if (!checkUser)
                {
                    //如果骑士不存在 
                    return SimpleResultModel.Conclude(SendCheckCodeStatus.NotExists);
                }
                key = RedissCacheKey.PostForgetPwd_C + PhoneNumber;
                msg = string.Format(SupermanApiConfig.Instance.SmsContentFindPassword, randomCode, SystemConst.MessageClinenter);
            }
            try
            {
                var redis = new RedisCache();
                redis.Add(key, randomCode, DateTime.Now.AddHours(1));

                // 更新短信通道 
                Task.Factory.StartNew(() =>
                {
                    SendSmsHelper.SendSendSmsSaveLog(PhoneNumber, msg, SystemConst.SMSSOURCE);
                });
                return SimpleResultModel.Conclude(SendCheckCodeStatus.Sending);

            }
            catch (Exception)
            {
                return SimpleResultModel.Conclude(SendCheckCodeStatus.SendFailure);
            }
        }

        /// <summary>
        /// 语音请求动态验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>   
        [HttpPost]
        public SimpleResultModel VoiceCheckCode(SmsParaModel model)
        {
            if (!CommonValidator.IsValidPhoneNumber(model.PhoneNumber))
            {
                return SimpleResultModel.Conclude(SendCheckCodeStatus.InvlidPhoneNumber);
            }
            var redis = new RedisCache();
            string msg = string.Empty;
            string key = model.Stype == "0" ? RedissCacheKey.PostRegisterInfo_C + model.PhoneNumber : RedissCacheKey.PostForgetPwd_C + model.PhoneNumber;


            object obj = redis.Get<object>(key);
            if (obj == null)
            {
                return SimpleResultModel.Conclude(SendCheckCodeStatus.CodeNotExists);
            }

            #region 判断该语音验证码是否存在
            string keycheck = key + "_voice";
            if (ParseHelper.ToInt(redis.Get<int>(keycheck)) == 1)
            {
                //如果该验证码存在直接提示成功
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.Sending);
            }
            redis.Add(keycheck, 1, new TimeSpan(0, 1, 0));
            #endregion

            string tempcode = obj.ToString().Aggregate("", (current, c) => current + (c.ToString() + ','));

            bool userStatus = iClienterProvider.CheckClienterExistPhone(model.PhoneNumber);

            if (model.Stype == "0")//注册
            {
                if (userStatus)  //判断该手机号是否已经注册过
                    return SimpleResultModel.Conclude(SendCheckCodeStatus.AlreadyExists);
                msg = string.Format(SupermanApiConfig.Instance.SmsContentCheckCodeVoice, tempcode, SystemConst.MessageClinenter);
            }
            else //修改密码
            {
                if (!userStatus)
                    return SimpleResultModel.Conclude(SendCheckCodeStatus.NotExists);
                msg = string.Format(SupermanApiConfig.Instance.SmsContentCheckCodeFindPwdVoice, tempcode, SystemConst.MessageClinenter);
            }
            try
            {

                // 更新短信通道 
                Task.Factory.StartNew(() =>
                {
                    SendSmsHelper.SendSmsSaveLogNew(model.PhoneNumber, msg, SystemConst.SMSSOURCE);
                });
                return SimpleResultModel.Conclude(SendCheckCodeStatus.Sending);

            }
            catch (Exception)
            {
                return SimpleResultModel.Conclude(SendCheckCodeStatus.SendFailure);
            }
        }


        /// <summary>
        /// 骑士上下班功能 add by caoheyang 20150312
        /// </summary>
        /// <param name="paraModel"></param>
        /// <returns></returns>
        [HttpPost]
        public SimpleResultModel ChangeWorkStatus(ChangeWorkStatusPM paraModel)
        {
            if (paraModel.WorkStatus == null) //检查非空
            {
                return SimpleResultModel.Conclude(ChangeWorkStatusEnum.WorkStatusError);
            }
            if (paraModel.Id == null) //检查非空
            {
                return SimpleResultModel.Conclude(ChangeWorkStatusEnum.ClienterError);
            }
            return SimpleResultModel.Conclude(iClienterProvider.ChangeWorkStatus(paraModel));
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
        /// 窦海超
        /// 2015年3月31日 
        /// </summary>
        /// <param name="parModel">userId</param>
        /// <returns></returns>    
        [HttpPost]
        public ResultModel<ClienterStatusModel> GetUserStatus(UserStatusModel parModel)
        {
            var model = new ClienterProvider().GetUserStatus(parModel.userId);
            if (model != null)
            {
                return ResultModel<ClienterStatusModel>.Conclude(
                UserStatus.Success,
                model
                );
            }
            return ResultModel<ClienterStatusModel>.Conclude(
                UserStatus.Error,
                null
                );
        }

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

    }
}