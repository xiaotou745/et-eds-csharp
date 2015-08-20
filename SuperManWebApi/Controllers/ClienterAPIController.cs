using ETS.Const;
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
using SuperManWebApi.App_Start.Filters;
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
using Ets.Model.ParameterModel.Common;
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
        //public ResultModel<string> PostLogin_C(ParamModel model)//LoginCPM  ClienterLoginResultModel
        //{
        //    return new ClienterProvider().PostLogin_C(model);
        //}
        public ResultModel<ClienterLoginResultModel> PostLogin_C(LoginCPM model)//LoginCPM  ClienterLoginResultModel
        {
            return new ClienterProvider().PostLogin_C(model);
        }
        /// <summary>
        /// C端上传图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Token]
        public ResultModel<UploadIconModel> PostAudit_C()
        {
            if (HttpContext.Current.Request.Form.Count == 0)
            {
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.NOFormParameter);
            }
            int UserId = ParseHelper.ToInt(HttpContext.Current.Request.Form["userId"], 0); //用户Id
            var strIdCard = HttpContext.Current.Request.Form["IDCard"]; //身份证号
            var trueName = HttpContext.Current.Request.Form["trueName"]; //真实姓名
            if (UserId == 0 || !iClienterProvider.CheckClienterExistById(UserId))
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
            ImgInfo handImg = ih.UploadImg(fileHand, UserId, ImageType.Clienter);
            //身份证照片
            ImgInfo sfhImg = ih.UploadImg(file, UserId, ImageType.Clienter);

            var upResult = iClienterProvider.UpdateClientPicInfo(new ClienterModel { Id = UserId, PicUrl = sfhImg.PicUrl, PicWithHandUrl = handImg.PicUrl, TrueName = trueName, IDCard = strIdCard });
            if (!upResult)
            {
                return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.UpFailed, new UploadIconModel() { Id = UserId, ImagePath = "" });
            }
            var relativePath = Path.Combine(CustomerIconUploader.Instance.GetPhysicalPath(ImageType.Clienter), sfhImg.FileName).ToForwardSlashPath();
            return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.Success, new UploadIconModel() { Id = UserId, ImagePath = relativePath });
        }
        /// <summary>
        /// 获取我的任务   根据状态判断是已完成任务还是我的任务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>        
        [ExecuteTimeLog]
        [HttpPost]
        [Token]
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
        /// 修改密码
        /// </summary>
        /// <returns></returns>        
        [HttpPost]
        [Token]
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

            string key = string.Concat(RedissCacheKey.PostForgetPwdCount_C, model.phoneNo);
            int excuteCount = redis.Get<int>(key);
            if (excuteCount >= 10)
            {
                return ResultModel<ClienterModifyPwdResultModel>.Conclude(ForgetPwdStatus.CountError);
            }
            redis.Set(key, excuteCount + 1, new TimeSpan(0, 5, 0));

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
        [HttpGet]
        [Token]
        [ExecuteTimeLog]
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
        [Token]
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
        [Token]
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
            else if(type=="1") //忘记密码
            {
                if (!checkUser)
                {
                    //如果骑士不存在 
                    return SimpleResultModel.Conclude(SendCheckCodeStatus.NotExists);
                }
                key = RedissCacheKey.PostForgetPwd_C + PhoneNumber;
                msg = string.Format(SupermanApiConfig.Instance.SmsContentFindPassword, randomCode, SystemConst.MessageClinenter);
            }
            else if(type=="2")//修改密码
            {
                if (!checkUser)
                {
                    //如果骑士不存在 
                    return SimpleResultModel.Conclude(SendCheckCodeStatus.NotExists);
                }
                key = RedissCacheKey.ChangePasswordCheckCode_C + PhoneNumber;
                msg = string.Format(SupermanApiConfig.Instance.SmsContentFindPassword, randomCode, SystemConst.MessageClinenter);
            }
            try
            {
                var redis = new RedisCache();
                redis.Add(key, randomCode, new TimeSpan(0, 5, 0));
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
            string key = "";//model.Stype == "0" ? RedissCacheKey.PostRegisterInfo_C + model.PhoneNumber : RedissCacheKey.PostForgetPwd_C + model.PhoneNumber;
            switch (model.Stype)
            {
                case "0":key = RedissCacheKey.PostRegisterInfo_C + model.PhoneNumber;break;
                case "1": key = RedissCacheKey.PostForgetPwd_C + model.PhoneNumber; break;
                case "2": key = RedissCacheKey.ChangePasswordCheckCode_C + model.PhoneNumber; break;
            }

            string obj = redis.Get<string>(key);
            if (string.IsNullOrEmpty(obj))
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
            else if(model.Stype=="1")//忘记密码
            {
                if (!userStatus)
                    return SimpleResultModel.Conclude(SendCheckCodeStatus.NotExists);
                msg = string.Format(SupermanApiConfig.Instance.SmsContentCheckCodeFindPwdVoice, tempcode, SystemConst.MessageClinenter);
            }
            else if (model.Stype == "2")//修改密码
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
        [Token]
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
        [Token]
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

        /// <summary>
        /// 设置是否开启接受推送
        /// 茹化肖
        /// 2015年8月19日18:05:41
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Token]
        public ResultModel<object> SetReceivePush(ClienterReceivePushModel model)
        {
            if (model == null||model.ClienterId<1)
            {
                return ResultModel<object>.Conclude(SetReceivePushStatus.ParError);
            }
            if (iClienterProvider.SetReceivePush(model))
            {
                return ResultModel<object>.Conclude(SetReceivePushStatus.Success);
            }
            return ResultModel<object>.Conclude(SetReceivePushStatus.Failed);
        }

    }
}