using ETS.Const;
using ETS.Enums;
using Ets.Model.Common;
using Ets.Service.Provider.WtihdrawRecords;
using Microsoft.Ajax.Utilities;
using SuperManCore.Common;
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
using SuperManBusinessLogic.B_Logic;
using System.ComponentModel;
using ETS.Util;
using Ets.Service.Provider.Clienter;
using Ets.Service.Provider.Common;
using SuperManDataAccess;
using SuperManWebApi.Providers;
using Ets.Model.DataModel.Order;
using System.Text.RegularExpressions;
using ETS.IO;
namespace SuperManWebApi.Controllers
{

    public class ClienterAPIController : ApiController
    {

        private static object lockHelper = new object();
        readonly Ets.Service.IProvider.Clienter.IClienterProvider iClienterProvider = new Ets.Service.Provider.Clienter.ClienterProvider();
        readonly Ets.Service.IProvider.Common.IAreaProvider iAreaProvider = new Ets.Service.Provider.Common.AreaProvider();
        readonly Ets.Service.IProvider.Order.IOrderProvider iOrderProvider = new Ets.Service.Provider.Order.OrderProvider();

        /// <summary>
        /// C端注册 -平扬 2015.3.30
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(Ets.Model.ParameterModel.Bussiness.CustomerRegisterStatus))]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.ClientRegisterResultModel> PostRegisterInfo_C(Ets.Model.ParameterModel.Clienter.ClientRegisterInfoModel model)
        {

            return iClienterProvider.PostRegisterInfo_C(model);

        }

        /// <summary>
        /// C端登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.LoginModelStatus))]
        public Ets.Model.Common.ResultModel<Ets.Model.DataModel.Clienter.ClienterLoginResultModel> PostLogin_C(Ets.Model.ParameterModel.Clienter.LoginModel model)
        {
            return new ClienterProvider().PostLogin_C(model);
        }
        /// <summary>
        /// C端上传图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel> PostAudit_C()
        {
            if (HttpContext.Current.Request.Form.Count == 0)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.NOFormParameter);
            }
            var strUserId = HttpContext.Current.Request.Form["userId"]; //用户Id
            var strIDCard = HttpContext.Current.Request.Form["IDCard"]; //身份证号
            var trueName = HttpContext.Current.Request.Form["trueName"]; //真实姓名
            if (!iClienterProvider.CheckClienterExistById(int.Parse(strUserId)))
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidUserId);
            }
            if (HttpContext.Current.Request.Files.Count == 0)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidFileFormat);
            }
            if (string.IsNullOrEmpty(trueName))
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.TrueNameEmpty);
            }
            var fileHand = HttpContext.Current.Request.Files[0]; //手持照片
            var file = HttpContext.Current.Request.Files[1]; //照片
            System.Drawing.Image imgHand;
            System.Drawing.Image img;
            try
            {
                imgHand = System.Drawing.Image.FromStream(fileHand.InputStream);
                img = System.Drawing.Image.FromStream(file.InputStream);
            }
            catch (Exception)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidFileFormat);
            }

            var fileHandName = ETS.Util.ImageTools.GetFileName("C", Path.GetExtension(fileHand.FileName));

            var fileName = ETS.Util.ImageTools.GetFileName("C", Path.GetExtension(file.FileName));

            int fileHandNameLastDot = fileHandName.LastIndexOf('.');
            int fileNameLastDot = fileName.LastIndexOf('.');
            //原图
            string rFileHandName = string.Format("{0}{1}{2}", fileHandName.Substring(0, fileHandNameLastDot), ImageConst.OriginSize, Path.GetExtension(fileHandName));
            string rFileName = string.Format("{0}{1}{2}", fileName.Substring(0, fileNameLastDot), ImageConst.OriginSize, Path.GetExtension(fileName));

            string saveDbFileHandPath;
            string saveDbFilePath;

            string fullFileHandDir = ETS.Util.ImageTools.CreateDirectory(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath, "", out saveDbFileHandPath);

            string fullFileDir = ETS.Util.ImageTools.CreateDirectory(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.PhysicalPath, "", out saveDbFilePath);

            if (fullFileHandDir == "0" || fullFileDir == "0")
            {
                SuperManCore.LogHelper.LogWriter("上传图片失败：", new { ex = "检查是否有权限创建目录" });
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.UpFailed);
            }
            //保存原图
            var fullFilePath = Path.Combine(fullFileDir, rFileName);
            var fullFileHandPath = Path.Combine(fullFileHandDir, rFileHandName);
            file.SaveAs(fullFilePath);
            fileHand.SaveAs(fullFileHandPath);
            //裁图
            var transformer = new FixedDimensionTransformerAttribute(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.Width, Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.Height, CustomerIconUploader.Instance.MaxBytesLength / 1024);
            //保存到数据库的图片路径
            var destFullFileName = System.IO.Path.Combine(fullFileDir, fileName);
            transformer.Transform(fullFilePath, destFullFileName);

            var destFullFileHandName = System.IO.Path.Combine(fullFileHandDir, fileHandName);
            transformer.Transform(fullFileHandPath, destFullFileHandName);

            var picUrl = saveDbFilePath + fileName;

            var picUrlWithHand = saveDbFileHandPath + fileHandName;

            var upResult = iClienterProvider.UpdateClientPicInfo(new Ets.Model.DomainModel.Clienter.ClienterModel { Id = int.Parse(strUserId), PicUrl = picUrl, PicWithHandUrl = picUrlWithHand, TrueName = trueName, IDCard = strIDCard });
            if (!upResult)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.UpFailed, new Ets.Model.ParameterModel.Clienter.UploadIconModel() { Id = ParseHelper.ToInt(strUserId), ImagePath = picUrl });
            }
            var relativePath = System.IO.Path.Combine(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.RelativePath, fileName).ToForwardSlashPath();
            return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadIconModel>.Conclude(ETS.Enums.UploadIconStatus.Success, new Ets.Model.ParameterModel.Clienter.UploadIconModel() { Id = ParseHelper.ToInt(strUserId), ImagePath = relativePath });
        }
        /// <summary>
        /// 获取我的任务   根据状态判断是已完成任务还是我的任务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.GetOrdersStatus))]
        [ExecuteTimeLog]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Clienter.ClientOrderResultModel[]> GetMyJobList_C(Ets.Model.ParameterModel.Clienter.ClientOrderInfoModel model)
        {
            Ets.Model.DomainModel.Clienter.degree.longitude = model.longitude;
            Ets.Model.DomainModel.Clienter.degree.latitude = model.latitude;
            var pIndex = ParseHelper.ToInt(model.pageIndex, 1);

            var pSize = ParseHelper.ToInt(model.pageSize, ConstValues.App_PageSize);

            var criteria = new Ets.Model.DataModel.Clienter.ClientOrderSearchCriteria()
            {
                PagingRequest = new Ets.Model.Common.PagingResult(pIndex, pSize),
                userId = model.userId,
                status = model.status,
                isLatest = model.isLatest
            };

            IList<Ets.Model.DomainModel.Clienter.ClientOrderResultModel> lists = new ClienterProvider().GetMyOrders(criteria);
            if (model.status != null && model.status != 1)
            {
                // lists = lists.OrderByDescending(i => i.pubDate).ToList();  //按照发布时间倒序排列
                lists = lists.OrderBy(i => i.distance_OrderBy).ToList();
            }
            return Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Clienter.ClientOrderResultModel[]>.Conclude(ETS.Enums.GetOrdersStatus.Success, lists.ToArray());
        }



        /// <summary>
        /// Ado.net  add  王超
        /// C端获取我的任务列表 最近任务 登录未登录根据城市有没有值判断。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.GetOrdersStatus))]
        [ExecuteTimeLog]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Clienter.ClientOrderResultModel[]> GetJobList_C(Ets.Model.ParameterModel.Clienter.ClientOrderInfoModel model)
        {
            Ets.Model.DomainModel.Clienter.degree.longitude = model.longitude;
            Ets.Model.DomainModel.Clienter.degree.latitude = model.latitude;
            var pIndex = ParseHelper.ToInt(model.pageIndex, 1);
            var pSize = ParseHelper.ToInt(model.pageSize, ConstValues.App_PageSize);
            var criteria = new Ets.Model.DataModel.Clienter.ClientOrderSearchCriteria()
            {
                PagingRequest = new Ets.Model.Common.PagingResult(pIndex, pSize),
                userId = model.userId,
                status = model.status,
                isLatest = model.isLatest,
                city = string.IsNullOrWhiteSpace(model.city) ? null : model.city.Trim(),
                cityId = string.IsNullOrWhiteSpace(model.cityId) ? null : model.cityId.Trim()
            };
            //这里转换一下 区域 code ,转换后 修改 为根据 code 作为 查询条件，原来根据name去掉  wc
            //if (!string.IsNullOrWhiteSpace(model.city))
            //{
            //    Ets.Model.DomainModel.Area.AreaModelTranslate areaModel = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModelTranslate() { Name = model.city.Trim(), JiBie = 2 });
            //   if (areaModel != null)
            //   {
            //       criteria.cityId = areaModel.NationalCode.ToString();
            //        
            //   }
            //}
            var pagedList = new Ets.Service.Provider.Order.OrderProvider().GetOrders(criteria);

            // pagedList = pagedList.OrderByDescending(i => i.pubDate).ToList();  //按照发布时间倒序排列
            pagedList = pagedList.OrderBy(i => i.distance_OrderBy).ToList();
            return Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Clienter.ClientOrderResultModel[]>.Conclude(ETS.Enums.GetOrdersStatus.Success, pagedList.ToArray());
        }

        /// <summary>
        /// Ado.net add 王超
        /// 未登录时获取最新任务     登录未登录根据城市有没有值判断。
        /// </summary>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.GetOrdersNoLoginStatus))]
        [ExecuteTimeLog]
        [HttpGet]
        public Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Clienter.ClientOrderNoLoginResultModel[]> GetJobListNoLoginLatest_C()
        {
            Ets.Model.ParameterModel.Clienter.ClientOrderInfoModel model = new Ets.Model.ParameterModel.Clienter.ClientOrderInfoModel();
            model.city = string.IsNullOrWhiteSpace(HttpContext.Current.Request["city"]) ? null : HttpContext.Current.Request["city"].Trim();//城市
            model.cityId = string.IsNullOrWhiteSpace(HttpContext.Current.Request["cityId"]) ? null : HttpContext.Current.Request["cityId"].Trim(); //城市编码
            Ets.Model.DomainModel.Clienter.degree.longitude = ETS.Util.ParseHelper.ToDouble(HttpContext.Current.Request["longitude"]);
            Ets.Model.DomainModel.Clienter.degree.latitude = ETS.Util.ParseHelper.ToDouble(HttpContext.Current.Request["latitude"]);
            var pIndex = ParseHelper.ToInt(model.pageIndex, 1);
            var pSize = ParseHelper.ToInt(model.pageSize, ConstValues.App_PageSize);
            var criteria = new Ets.Model.DataModel.Clienter.ClientOrderSearchCriteria()
            {
                PagingRequest = new Ets.Model.Common.PagingResult(pIndex, pSize),
                city = model.city,
                cityId = model.cityId
            };
            //根据用户传递的  名称，取得 国标编码 wc,这里的 city 是二级 ，已和康珍 确认过
            //新版的 骑士 注册， 城市 非 必填 
            //if (!string.IsNullOrWhiteSpace(model.city))
            //{
            //    Ets.Model.DomainModel.Area.AreaModelTranslate areaModel = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModelTranslate() { Name = model.city.Trim(), JiBie = 2 });
            //    if (areaModel != null)
            //    {
            //        criteria.cityId = areaModel.NationalCode.ToString();
            //         
            //    }
            //}

            var pagedList = new Ets.Service.Provider.Order.OrderProvider().GetOrdersNoLoginLatest(criteria);
            //pagedList = pagedList.OrderByDescending(i => i.pubDate).ToList();

            pagedList = pagedList.OrderBy(i => i.distance_OrderBy).ToList();
            //var pagedList = ClienterLogic.clienterLogic().GetOrdersNoLoginLatest(criteria);
            //var lists = ClientOrderNoLoginResultModelTranslator.Instance.Translate(pagedList);

            return Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Clienter.ClientOrderNoLoginResultModel[]>.Conclude(ETS.Enums.GetOrdersNoLoginStatus.Success, pagedList.ToArray());
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.ModifyPwdStatus))]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.DataModel.Clienter.ClienterModifyPwdResultModel> PostModifyPwd_C(Ets.Model.DataModel.Clienter.ModifyPwdInfoModel model)
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
        [ActionStatus(typeof(ETS.Enums.ForgetPwdStatus))]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.DataModel.Clienter.ClienterModifyPwdResultModel> PostForgetPwd_C(Ets.Model.DataModel.Clienter.ForgetPwdInfoModel model)
        {
            if (string.IsNullOrEmpty(model.password))
            {
                return Ets.Model.Common.ResultModel<Ets.Model.DataModel.Clienter.ClienterModifyPwdResultModel>.Conclude(ETS.Enums.ForgetPwdStatus.NewPwdEmpty);
            }
            if (string.IsNullOrEmpty(model.checkCode))
            {
                return Ets.Model.Common.ResultModel<Ets.Model.DataModel.Clienter.ClienterModifyPwdResultModel>.Conclude(ETS.Enums.ForgetPwdStatus.checkCodeIsEmpty);
            }
            var redis = new ETS.NoSql.RedisCache.RedisCache();
            var code = redis.Get<string>(RedissCacheKey.PostForgetPwd_C + model.phoneNo);
            //start 需要验证 验证码是否正确
            if (string.IsNullOrEmpty(code) || code != model.checkCode)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.DataModel.Clienter.ClienterModifyPwdResultModel>.Conclude(ETS.Enums.ForgetPwdStatus.checkCodeWrong);
            }
            //end
            //var clienter = ClienterLogic.clienterLogic().GetClienter(model.phoneNo);
            var clienter = iClienterProvider.GetUserInfoByUserPhoneNo(model.phoneNo);
            if (clienter == null)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.DataModel.Clienter.ClienterModifyPwdResultModel>.Conclude(ETS.Enums.ForgetPwdStatus.ClienterIsNotExist);
            }
            if (clienter.Password == model.password)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.DataModel.Clienter.ClienterModifyPwdResultModel>.Conclude(ETS.Enums.ForgetPwdStatus.PwdIsSave);
            }
            //bool b = ClienterLogic.clienterLogic().ModifyPwd(clienter, model.password);
            bool b = iClienterProvider.UpdateClienterPwdByUserId(clienter.Id, model.password);
            if (b)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.DataModel.Clienter.ClienterModifyPwdResultModel>.Conclude(ETS.Enums.ForgetPwdStatus.Success);
            }
            else
            {
                return Ets.Model.Common.ResultModel<Ets.Model.DataModel.Clienter.ClienterModifyPwdResultModel>.Conclude(ETS.Enums.ForgetPwdStatus.FailedModifyPwd);
            }
        }

        /// <summary>
        /// 超人抢单-平扬  2015.3.30
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.RushOrderStatus))]
        [HttpGet]
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.RushOrderResultModel> RushOrder_C(int userId, string orderNo)
        {
            if (userId == 0 || new Ets.Dao.Clienter.ClienterDao().GetUserInfoByUserId(userId) == null) //用户id验证
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.RushOrderResultModel>.Conclude(ETS.Enums.RushOrderStatus.userIdEmpty);
            if (!iClienterProvider.HaveQualification(userId))  //判断 该骑士 是否 有资格 抢单 wc
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.RushOrderResultModel>.Conclude(ETS.Enums.RushOrderStatus.HadCancelQualification);
            }
            if (string.IsNullOrEmpty(orderNo)) //订单号码非空验证
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.RushOrderResultModel>.Conclude(ETS.Enums.RushOrderStatus.OrderEmpty);
            var myorder = new Ets.Dao.Order.OrderDao().GetOrderByNo(orderNo);
            if (myorder != null)
            {
                if (myorder.Status == ConstValues.ORDER_CANCEL)   //判断订单状态是否为 已取消
                {
                    return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.RushOrderResultModel>.Conclude(ETS.Enums.RushOrderStatus.OrderHadCancel);  //订单已被取消
                }
                if (myorder.Status == ConstValues.ORDER_ACCEPT || myorder.Status == ConstValues.ORDER_FINISH)  //订单已接单，被抢  或 已完成
                {
                    return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.RushOrderResultModel>.Conclude(ETS.Enums.RushOrderStatus.OrderIsNotAllowRush);
                }
            }
            else
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.RushOrderResultModel>.Conclude(ETS.Enums.RushOrderStatus.OrderIsNotExist);  //订单不存在
            }

            lock (lockHelper)
            {
                bool bResult = iClienterProvider.RushOrder(userId, orderNo);
                if (bResult)
                {
                    Ets.Service.Provider.MyPush.Push.PushMessage(1, "订单提醒", "有订单被抢了！", "有超人抢了订单！", myorder.businessId.ToString(), string.Empty);
                    return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.RushOrderResultModel>.Conclude(ETS.Enums.RushOrderStatus.Success);
                }

                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.RushOrderResultModel>.Conclude(ETS.Enums.RushOrderStatus.Failed);
            }
        }
        /// <summary>
        /// 完成订单 
        /// wc 该 ado
        /// </summary>
        /// <param name="userId">C端用户id</param>
        /// <param name="orderNo">订单号码</param>
        /// <param name="pickupCode">取货码</param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.FinishOrderStatus))]
        [HttpGet]
        public Ets.Model.Common.ResultModel<FinishOrderResultModel> FinishOrder_C(int userId, string orderNo, string pickupCode = null)
        {
            if (userId == 0)  //用户id非空验证
                return Ets.Model.Common.ResultModel<FinishOrderResultModel>.Conclude(ETS.Enums.FinishOrderStatus.userIdEmpty);
            if (string.IsNullOrEmpty(orderNo)) //订单号码非空验证
                return Ets.Model.Common.ResultModel<FinishOrderResultModel>.Conclude(ETS.Enums.FinishOrderStatus.OrderEmpty);
            string finishResult = iClienterProvider.FinishOrder(userId, orderNo, pickupCode);
            if (finishResult == "1")  //完成
            {
                var clienter = iClienterProvider.GetUserInfoByUserId(userId);
                var model = new FinishOrderResultModel();
                model.userId = userId;
                if (clienter.AccountBalance != null)
                    model.balanceAmount = clienter.AccountBalance.Value;
                else
                    model.balanceAmount = 0.0m;
                return Ets.Model.Common.ResultModel<FinishOrderResultModel>.Conclude(ETS.Enums.FinishOrderStatus.Success, model);
            }
            else if (finishResult == ETS.Enums.FinishOrderStatus.PickupCodeError.ToString())
                return Ets.Model.Common.ResultModel<FinishOrderResultModel>.Conclude(ETS.Enums.FinishOrderStatus.PickupCodeError);
            else
            {
                return Ets.Model.Common.ResultModel<FinishOrderResultModel>.Conclude(ETS.Enums.FinishOrderStatus.Failed);
            }
        }

        /// <summary>
        /// 获取我的余额
        /// </summary>
        /// <param name="phoneNo">手机号</param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.RushOrderStatus))]
        [HttpGet]
        public Ets.Model.Common.ResultModel<Ets.Model.DataModel.Clienter.MyBalanceResultModel> GetMyBalance(string phoneNo)
        {
            if (string.IsNullOrEmpty(phoneNo))
            {
                return Ets.Model.Common.ResultModel<Ets.Model.DataModel.Clienter.MyBalanceResultModel>.Conclude(ETS.Enums.GetMyBalanceStatus.PhoneEmpty);
            }
            var item = iClienterProvider.GetUserInfoByUserPhoneNo(phoneNo);
            var result = new Ets.Model.DataModel.Clienter.MyBalanceResultModel()
            {
                MyBalance = item.AccountBalance == null ? 0 : item.AccountBalance
            };
            return Ets.Model.Common.ResultModel<Ets.Model.DataModel.Clienter.MyBalanceResultModel>.Conclude(ETS.Enums.FinishOrderStatus.Success, result);
        }

        /// <summary>
        /// 获取我的余额动态
        /// </summary>
        /// <param name="phoneNo">手机号</param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.RushOrderStatus))]
        [HttpGet]
        public Ets.Model.Common.ResultModel<Ets.Model.DomainModel.MyBalanceListResultModel[]> GetMyBalanceDynamic(string phoneNo, int? pagedSize, int? pagedIndex)
        {
            int pIndex = ParseHelper.ToInt(pagedIndex.HasValue, 1);
            int pSize = ParseHelper.ToInt(pagedSize.HasValue, ConstValues.App_PageSize);
            var criteria = new Ets.Model.ParameterModel.WtihdrawRecords.MyIncomeSearchCriteria()
            {
                PagingRequest = new Ets.Model.Common.PagingResult(pIndex, pSize),
                phoneNo = phoneNo
            };

            //平扬2015.3.23 改为从 Records表查询金额记录,myincome表作废
            // var pagedList = ClienterLogic.clienterLogic().GetMyIncomeList(criteria);
            var withrecord = new WtihdrawRecordsProvider();
            var pagedList = withrecord.GetMyIncomeList(criteria);
            var lists = Ets.Model.DomainModel.MyBalanceListResultModelTranslator.Instance.Translate(pagedList);
            return Ets.Model.Common.ResultModel<Ets.Model.DomainModel.MyBalanceListResultModel[]>.Conclude(ETS.Enums.RushOrderStatus.Success, lists.ToArray());
        }

        /// <summary>
        /// 请求动态验证码
        /// </summary>
        /// <param name="PhoneNumber">手机号</param>
        /// <param name="type">操作类型： 0 注册 1修改密码</param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.SendCheckCodeStatus))]
        [HttpGet]
        public Ets.Model.Common.SimpleResultModel CheckCode(string PhoneNumber, string type)
        {
            if (!CommonValidator.IsValidPhoneNumber(PhoneNumber))
            {
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.InvlidPhoneNumber);
            }
            var randomCode = new Random().Next(100000).ToString("D6");
            string msg = string.Empty;
            string key = "";
            if (type == "0")//注册
            {
                if (iClienterProvider.CheckClienterExistPhone(PhoneNumber))  //判断该手机号是否已经注册过
                    return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.AlreadyExists);
                key = RedissCacheKey.PostRegisterInfo_C + PhoneNumber;
                msg = string.Format(SupermanApiConfig.Instance.SmsContentCheckCode, randomCode, ConstValues.MessageClinenter);
            }
            else //修改密码
            {
                key = RedissCacheKey.PostForgetPwd_C + PhoneNumber;
                msg = string.Format(SupermanApiConfig.Instance.SmsContentFindPassword, randomCode, ConstValues.MessageClinenter);
            }
            try
            {
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                redis.Add(key, randomCode, DateTime.Now.AddHours(1));

                // 更新短信通道 
                Task.Factory.StartNew(() =>
                {
                    ETS.Sms.SendSmsHelper.SendSendSmsSaveLog(PhoneNumber, msg, ConstValues.SMSSOURCE);
                });
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.Sending);

            }
            catch (Exception)
            {
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.SendFailure);
            }
        }

        /// <summary>
        /// 语音请求动态验证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.SendCheckCodeStatus))]
        [HttpPost]
        public Ets.Model.Common.SimpleResultModel VoiceCheckCode(Ets.Model.ParameterModel.Sms.SmsParaModel model)
        {
            if (!CommonValidator.IsValidPhoneNumber(model.PhoneNumber))
            {
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.InvlidPhoneNumber);
            }
            var randomCode = new Random().Next(100000).ToString("D6");
            string msg = string.Empty;
            string key = "";
            string tempcode = randomCode.Aggregate("", (current, c) => current + (c.ToString() + ','));
            if (model.Stype == "0")//注册
            {
                if (iClienterProvider.CheckClienterExistPhone(model.PhoneNumber))  //判断该手机号是否已经注册过
                    return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.AlreadyExists);
                key = RedissCacheKey.PostRegisterInfoSoundCode_C + model.PhoneNumber;
                msg = string.Format(ETS.Util.SupermanApiConfig.Instance.SmsContentCheckCodeVoice, tempcode, ConstValues.MessageClinenter);
            }
            else //修改密码
            {
                key = RedissCacheKey.PostForgetPwdSoundCode_C + model.PhoneNumber;
                msg = string.Format(ETS.Util.SupermanApiConfig.Instance.SmsContentCheckCodeFindPwdVoice, tempcode, ConstValues.MessageClinenter);
            }
            try
            {
                var redis = new ETS.NoSql.RedisCache.RedisCache();
                redis.Add(key, randomCode, DateTime.Now.AddHours(1));

                // 更新短信通道 
                Task.Factory.StartNew(() =>
                {
                    ETS.Sms.SendSmsHelper.SendSmsSaveLogNew(model.PhoneNumber, msg, ConstValues.SMSSOURCE);
                });
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.Sending);

            }
            catch (Exception)
            {
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.SendCheckCodeStatus.SendFailure);
            }
        }


        /// <summary>
        /// 骑士上下班功能 add by caoheyang 20150312
        /// </summary>
        /// <param name="paraModel"></param>
        /// <returns></returns>
        [HttpPost]
        public Ets.Model.Common.SimpleResultModel ChangeWorkStatus(Ets.Model.ParameterModel.Clienter.ChangeWorkStatusPM paraModel)
        {
            if (paraModel.WorkStatus == null) //检查非空
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.ChangeWorkStatusEnum.WorkStatusError);
            if (paraModel.Id == null) //检查非空
                return Ets.Model.Common.SimpleResultModel.Conclude(ETS.Enums.ChangeWorkStatusEnum.ClienterError);
            return Ets.Model.Common.SimpleResultModel.Conclude(iClienterProvider.ChangeWorkStatus(paraModel));
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
                ETS.Enums.GetOrdersStatus.Success,
                new ServicePhone().GetCustomerServicePhone(CityName)
                );

        }

        /// <summary>
        /// 获取用户状态
        /// 平扬
        /// 2015年3月31日 
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        [ActionStatus(typeof(ETS.Enums.UserStatus))]
        [HttpGet]
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.ClienterStatusModel> GetUserStatus(int userId, double version_api)
        {
            var model = new ClienterProvider().GetUserStatus(userId, version_api);
            if (model != null)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.ClienterStatusModel>.Conclude(
                UserStatus.Success,
                model
                );
            }
            return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.ClienterStatusModel>.Conclude(
                UserStatus.Error,
                null
                );
        }
        /// <summary>
        /// 小票上传
        /// wc
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiVersionStatistic]
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel> UploadReceipt(string Version)
        {
            //上传的图片流 大小 判断？
            if (HttpContext.Current.Request.Form.Count == 0)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.NOFormParameter);
            }
            var orderId = ParseHelper.ToInt(HttpContext.Current.Request.Form["OrderId"], 0); //订单号
            var clienterId = ParseHelper.ToInt(HttpContext.Current.Request.Form["ClienterId"], 0); //骑士的id
            if (clienterId == 0)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.ClienterIdInvalid);
            }
            if (orderId == 0)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidOrderId);
            }
            var needUploadCount = ParseHelper.ToInt(HttpContext.Current.Request.Form["NeedUploadCount"], 1); //该订单总共需要上传的 小票数量
            var version = HttpContext.Current.Request.Form["Version"]; //版本号  1.0
            if (HttpContext.Current.Request.Files.Count == 0)
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
            var transformer = new FixedDimensionTransformerAttribute(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.Width, Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.Height, CustomerIconUploader.Instance.MaxBytesLength / 1024);
            //保存到数据库的图片路径
            var destFullFileName = System.IO.Path.Combine(fullFileDir, fileName);
            transformer.Transform(fullFilePath, destFullFileName);

            var picUrl = saveDbFilePath + fileName;
            var uploadReceiptModel = new Ets.Model.ParameterModel.Clienter.UploadReceiptModel
            {
                OrderId = orderId,
                ClienterId = clienterId,
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
                //上传成功后返回图片全路径
                var relativePath = System.IO.Path.Combine(Ets.Model.ParameterModel.Clienter.CustomerIconUploader.Instance.RelativePath, fileName).ToForwardSlashPath();
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.Success, new Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel() { OrderId = orderId, ImagePath = listReceiptPic, HadUploadCount = orderOther.HadUploadCount, NeedUploadCount = orderOther.NeedUploadCount });
            }
        }
        /// <summary>
        /// 删除小票
        /// wc
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiVersionStatistic]
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel> DeleteReceipt(string Version)
        {

            
            if (HttpContext.Current.Request.Form.Count == 0)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.NOFormParameter);
            }
            var orderId = ParseHelper.ToInt(HttpContext.Current.Request.Form["OrderId"], 0); //订单号
            var receiptPic = HttpContext.Current.Request.Form["ReceiptPicAddress"];  //小票上传地址
            if (orderId == 0)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidOrderId);
            }
            if (!receiptPic.StartsWith("http",StringComparison.OrdinalIgnoreCase))
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.ReceiptAddressInvalid);
            }
            var version = HttpContext.Current.Request.Form["Version"]; //版本号  1.0
            Ets.Model.ParameterModel.Clienter.UploadReceiptModel uploadReceiptModel = new Ets.Model.ParameterModel.Clienter.UploadReceiptModel() { OrderId = orderId, ReceiptPic = receiptPic, HadUploadCount = -1 };


            SuperManCore.LogHelper.LogWriter("删除小票参数", new { version = version, receiptPic = receiptPic, orderId = orderId });
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
            if (orderOther.Id == 0)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.FirstUpload);
            }
            else
            {
                //判断小票数量是否为 0
                if (orderOther.HadUploadCount == 0)
                {
                    return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.FirstUpload);
                } 
            }

            OrderOther delOrderOther = iClienterProvider.DeleteReceipt(uploadReceiptModel);

            List<string> listReceiptPic = ImageCommon.ReceiptPicConvert(delOrderOther.ReceiptPic);

            return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.Success, new Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel() { OrderId = delOrderOther.OrderId, ImagePath = listReceiptPic, HadUploadCount = delOrderOther.HadUploadCount, NeedUploadCount = delOrderOther.NeedUploadCount });

        }

        /// <summary>
        /// 根据订单Id获取订单信息
        /// wc
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiVersionStatistic]
        public Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel> GetReceipt(string Version)
        {
            if (HttpContext.Current.Request.Form.Count == 0)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.NOFormParameter);
            }
            var orderId = ParseHelper.ToInt(HttpContext.Current.Request.Form["OrderId"], 0); //订单号

            if (orderId == 0)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.InvalidOrderId);
            }

            var version = HttpContext.Current.Request.Form["Version"]; //版本号  1.0

            OrderOther orderOther = iClienterProvider.GetReceipt(orderId);
            if (orderOther == null)
            {
                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.UpFailed, new Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel() { OrderId = orderId });
            }
            else
            {
                List<string> listReceiptPic = ImageCommon.ReceiptPicConvert(orderOther.ReceiptPic);

                return Ets.Model.Common.ResultModel<Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel>.Conclude(ETS.Enums.UploadIconStatus.Success, new Ets.Model.ParameterModel.Clienter.UploadReceiptResultModel() { OrderId = orderId, ImagePath = listReceiptPic, HadUploadCount = orderOther.HadUploadCount, NeedUploadCount = orderOther.NeedUploadCount });
            }
        }


    }
}