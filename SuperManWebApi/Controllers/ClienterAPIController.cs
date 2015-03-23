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
using SuperManDataAccess;
using SuperManCommonModel;
using SuperManBusinessLogic.B_Logic;
using System.ComponentModel;
using ETS.Util;
using Ets.Service.Provider.Clienter;
using Ets.Service.Provider.Common;


namespace SuperManWebApi.Controllers
{

    public class ClienterAPIController : ApiController
    {

        private static object lockHelper = new object();
        readonly Ets.Service.IProvider.Clienter.IClienterProvider iClienterProvider = new Ets.Service.Provider.Clienter.ClienterProvider();
        /// <summary>
        /// C端注册 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(CustomerRegisterStatus))]
        [HttpPost]
        public ResultModel<ClientRegisterResultModel> PostRegisterInfo_C(ClientRegisterInfoModel model)
        {
            if (string.IsNullOrEmpty(model.phoneNo))  //手机号非空验证
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberEmpty);
            else if (ClienterLogic.clienterLogic().CheckExistPhone(model.phoneNo))  //判断该手机号是否已经注册过
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberRegistered);
            else if (string.IsNullOrEmpty(model.passWord)) //密码非空验证
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PasswordEmpty);
            else if (string.IsNullOrEmpty(model.City) || string.IsNullOrEmpty(model.CityId)) //城市以及城市编码非空验证
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.cityIdEmpty);
            else if (model.verifyCode != SupermanApiCaching.Instance.Get(model.phoneNo)) //判断验码法录入是否正确
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.IncorrectCheckCode);
            else if (model.recommendPhone != null && (!ClienterLogic.clienterLogic().CheckExistPhone(model.recommendPhone))
                && (!BusiLogic.busiLogic().CheckExistPhone(model.recommendPhone))) //如果推荐人手机号在B端C端都不存在提示信息
                return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.PhoneNumberNotExist);
            var clienter = ClientRegisterInfoModelTranslator.Instance.Translate(model);
            bool result = ClienterLogic.clienterLogic().Add(clienter);
            var resultModel = new ClientRegisterResultModel
            {
                userId = clienter.Id,
                city = string.IsNullOrWhiteSpace(clienter.City) ? null : clienter.City.Trim(),  //城市
                cityId = string.IsNullOrWhiteSpace(clienter.CityId) ? null : clienter.CityId.Trim()  //城市编码
            };
            return ResultModel<ClientRegisterResultModel>.Conclude(CustomerRegisterStatus.Success, resultModel);
        }

        /// <summary>
        /// C端登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(LoginModelStatus))]
        public Ets.Model.Common.ResultModel<Ets.Model.DataModel.Clienter.ClienterLoginResultModel> PostLogin_C(Ets.Model.ParameterModel.Clienter.LoginModel model)
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
            if (string.IsNullOrEmpty(trueName))
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

            string originSize = "_0_0";

            var fileHandName = string.Format("{0}_{1}_{2}", DateTime.Now.ToString("yyyyMMddhhmmssfff"), new Random().Next(1000), fileHand.FileName);
            var fileName = string.Format("{0}_{1}_{2}", DateTime.Now.ToString("yyyyMMddhhmmssfff"), new Random().Next(1000), file.FileName);


            int fileHandNameLastDot = fileHandName.LastIndexOf('.');

            int fileNameLastDot = fileName.LastIndexOf('.');

            //增加 原图 尺寸标记 _0_0
            string rFileHandName = string.Format("{0}{1}{2}", fileHandName.Substring(0, fileHandNameLastDot), originSize, Path.GetExtension(fileHandName));
            string rFileName = string.Format("{0}{1}{2}", fileName.Substring(0, fileNameLastDot), originSize, Path.GetExtension(fileName));


            if (!System.IO.Directory.Exists(CustomerIconUploader.Instance.PhysicalPath))
            {
                System.IO.Directory.CreateDirectory(CustomerIconUploader.Instance.PhysicalPath);
            }
            var fullFilePath = Path.Combine(CustomerIconUploader.Instance.PhysicalPath, rFileName);
            var fullFileHandPath = Path.Combine(CustomerIconUploader.Instance.PhysicalPath, rFileHandName);
            //LogHelper.LogWriter("原图手持名称："+ rFileHandName + "   路径："+fullFileHandPath);
            //LogHelper.LogWriter("tupian名称：" + rFileName + "   路径：" + fullFilePath);

            //保存原图
            file.SaveAs(fullFilePath);
            fileHand.SaveAs(fullFileHandPath);

            //裁图
            var transformer = new FixedDimensionTransformerAttribute(CustomerIconUploader.Instance.Width, CustomerIconUploader.Instance.Height, CustomerIconUploader.Instance.MaxBytesLength / 1024);

            //var destFileName = string.Format("{0}_{1}{2}", DateTime.Now.ToString("yyyyMMddhhmmss"), new Random().Next(1000), Path.GetExtension(file.FileName));
            //var destFullFileName = System.IO.Path.Combine(CustomerIconUploader.Instance.PhysicalPath, destFileName);

            var destFullFileName = System.IO.Path.Combine(CustomerIconUploader.Instance.PhysicalPath, fileName);
            transformer.Transform(fullFilePath, destFullFileName);
            //LogHelper.LogWriter("裁剪后图片名称：" + destFullFileName);

            //var destFileHandName = string.Format("{0}_{1}{2}", DateTime.Now.ToString("yyyyMMddhhmmss"), new Random().Next(1000), Path.GetExtension(fileHand.FileName));
            //var destFullFileHandName = System.IO.Path.Combine(CustomerIconUploader.Instance.PhysicalPath, destFileHandName);

            var destFullFileHandName = System.IO.Path.Combine(CustomerIconUploader.Instance.PhysicalPath, fileHandName);
            transformer.Transform(fullFileHandPath, destFullFileHandName);
            //LogHelper.LogWriter("裁剪后手持图片名称：" + destFullFileHandName);

            var picUrl = System.IO.Path.GetFileName(destFullFileName);
            var picUrlWithHand = System.IO.Path.GetFileName(destFullFileHandName);

            //LogHelper.LogWriter("picUrl：" + picUrl);
            //LogHelper.LogWriter("picUrlWithHand：" + picUrlWithHand);

            ClienterLogic.clienterLogic().UpdateClient(customer, picUrl, picUrlWithHand, trueName, strIDCard);


            var relativePath = System.IO.Path.Combine(CustomerIconUploader.Instance.RelativePath, fileName).ToForwardSlashPath();
            return ResultModel<UploadIconModel>.Conclude(UploadIconStatus.Success, new UploadIconModel() { Id = 1, ImagePath = relativePath });
        }

        /// <summary>
        /// C端获取我的任务列表 最近任务    登录未登录根据城市有没有值判断。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[ActionStatus(typeof(GetOrdersStatus))]
        //[HttpPost]
        //public ResultModel<ClientOrderResultModel[]> GetJobList_C(ClientOrderInfoModel model)
        //{
        //    degree.longitude = model.longitude;
        //    degree.latitude = model.latitude;
        //    var pIndex = model.pageIndex.HasValue ? model.pageIndex.Value : 0;
        //    var pSize = model.pageSize.HasValue ? model.pageSize.Value : 20;
        //    var criteria = new ClientOrderSearchCriteria()
        //    {
        //        PagingRequest = new SuperManCore.Paging.PagingResult(pIndex, pSize),
        //        userId = model.userId,
        //        status = model.status,
        //        isLatest = model.isLatest,
        //        city = string.IsNullOrWhiteSpace(model.city) ? null : model.city.Trim(),
        //        cityId = string.IsNullOrWhiteSpace(model.cityId) ? null : model.cityId.Trim()
        //    };

        //    var pagedList = ClienterLogic.clienterLogic().GetOrders(criteria);
        //    var lists = ClientOrderResultModelTranslator.Instance.Translate(pagedList);


        //    //if (!model.isLatest) //不是最新任务的话就按距离排序,否则按发布时间排序
        //    //{
        //    lists = lists.OrderBy(i => i.distance).ToList();
        //    //}

        //    return ResultModel<ClientOrderResultModel[]>.Conclude(GetOrdersStatus.Success, lists.ToArray());
        //}

        /// <summary>
        /// 获取我的任务   根据状态判断是已完成任务还是我的任务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(GetOrdersStatus))]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Clienter.ClientOrderResultModel[]> GetMyJobList_C(ClientOrderInfoModel model)
        {
            Ets.Model.DomainModel.Clienter.degree.longitude = model.longitude;
            Ets.Model.DomainModel.Clienter.degree.latitude = model.latitude;
            var pIndex = ParseHelper.ToInt(model.pageIndex + 1, 1);

            var pSize = ParseHelper.ToInt(model.pageSize, 100);

            var criteria = new Ets.Model.DataModel.Clienter.ClientOrderSearchCriteria()
            {
                PagingRequest = new Ets.Model.Common.PagingResult(pIndex, pSize),
                userId = model.userId,
                status = model.status,
                isLatest = model.isLatest
            };
            //var pagedList = ClienterLogic.clienterLogic().GetMyOrders(criteria);
            //var lists = ClientOrderResultModelTranslator.Instance.Translate(pagedList);
            IList<Ets.Model.DomainModel.Clienter.ClientOrderResultModel> lists = new ClienterProvider().GetMyOrders(criteria);
            if (!model.isLatest) //不是最新任务的话就按距离排序,否则按发布时间排序
            {
                lists = lists.OrderBy(i => i.distance).ToList();
            }
            return Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Clienter.ClientOrderResultModel[]>.Conclude(GetOrdersStatus.Success, lists.ToArray());
        }

        /// <summary>
        /// 未登录时获取最新任务     登录未登录根据城市有没有值判断。
        /// </summary>
        /// <returns></returns>
        //[ActionStatus(typeof(GetOrdersNoLoginStatus))]
        //[HttpGet]
        //public ResultModel<ClientOrderNoLoginResultModel[]> GetJobListNoLoginLatest_C()
        //{
        //    ClientOrderInfoModel model = new ClientOrderInfoModel();
        //    model.city = string.IsNullOrWhiteSpace(HttpContext.Current.Request["city"]) ? null : HttpContext.Current.Request["city"].Trim();//城市
        //    model.cityId = string.IsNullOrWhiteSpace(HttpContext.Current.Request["cityId"]) ? null : HttpContext.Current.Request["cityId"].Trim(); //城市编码
        //    degree.longitude = ETS.Util.ParseHelper.ToDouble(HttpContext.Current.Request["longitude"]);
        //    degree.latitude = ETS.Util.ParseHelper.ToDouble(HttpContext.Current.Request["latitude"]);
        //    var pIndex = model.pageIndex.HasValue ? model.pageIndex.Value : 0;
        //    var pSize = model.pageSize.HasValue ? model.pageSize.Value : 20;
        //    ClientOrderSearchCriteria criteria = new ClientOrderSearchCriteria()
        //    {
        //        PagingRequest = new SuperManCore.Paging.PagingResult(pIndex, pSize),
        //        city = model.city,
        //        cityId = model.cityId
        //    };
        //    var pagedList = ClienterLogic.clienterLogic().GetOrdersNoLoginLatest(criteria);
        //    var lists = ClientOrderNoLoginResultModelTranslator.Instance.Translate(pagedList);
        //    return ResultModel<ClientOrderNoLoginResultModel[]>.Conclude(GetOrdersNoLoginStatus.Success, lists.ToArray());
        //}


        /// <summary>
        /// Ado.net  add  王超
        /// C端获取我的任务列表 最近任务 登录未登录根据城市有没有值判断。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ActionStatus(typeof(GetOrdersStatus))]
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Clienter.ClientOrderResultModel[]> GetJobList_C(Ets.Model.ParameterModel.Clienter.ClientOrderInfoModel model)
        {
            Ets.Model.DomainModel.Clienter.degree.longitude = model.longitude;
            Ets.Model.DomainModel.Clienter.degree.latitude = model.latitude;
            var pIndex = ParseHelper.ToInt(model.pageIndex + 1, 1);
            var pSize = ParseHelper.ToInt(model.pageSize, 20);
            var criteria = new Ets.Model.DataModel.Clienter.ClientOrderSearchCriteria()
            {
                PagingRequest = new Ets.Model.Common.PagingResult(pIndex, pSize),
                userId = model.userId,
                status = model.status,
                isLatest = model.isLatest,
                city = string.IsNullOrWhiteSpace(model.city) ? null : model.city.Trim(),
                cityId = string.IsNullOrWhiteSpace(model.cityId) ? null : model.cityId.Trim()
            };

            var pagedList = new Ets.Service.Provider.Order.OrderProvider().GetOrders(criteria);

            if (!model.isLatest) //不是最新任务的话就按距离排序,否则按发布时间排序
            {
                pagedList = pagedList.OrderBy(i => i.distance).ToList();
            }
            else
            {
                pagedList = pagedList.OrderByDescending(i => i.pubDate).ToList();
            }

            return Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Clienter.ClientOrderResultModel[]>.Conclude(GetOrdersStatus.Success, pagedList.ToArray());
        }



        /// <summary>
        /// Ado.net add 王超
        /// 未登录时获取最新任务     登录未登录根据城市有没有值判断。
        /// </summary>
        /// <returns></returns>
        [ActionStatus(typeof(GetOrdersNoLoginStatus))]
        [HttpGet]
        public Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Clienter.ClientOrderNoLoginResultModel[]> GetJobListNoLoginLatest_C()
        {
            Ets.Model.ParameterModel.Clienter.ClientOrderInfoModel model = new Ets.Model.ParameterModel.Clienter.ClientOrderInfoModel();
            model.city = string.IsNullOrWhiteSpace(HttpContext.Current.Request["city"]) ? null : HttpContext.Current.Request["city"].Trim();//城市
            model.cityId = string.IsNullOrWhiteSpace(HttpContext.Current.Request["cityId"]) ? null : HttpContext.Current.Request["cityId"].Trim(); //城市编码
            Ets.Model.DomainModel.Clienter.degree.longitude = ETS.Util.ParseHelper.ToDouble(HttpContext.Current.Request["longitude"]);
            Ets.Model.DomainModel.Clienter.degree.latitude = ETS.Util.ParseHelper.ToDouble(HttpContext.Current.Request["latitude"]);
            var pIndex = ParseHelper.ToInt(model.pageIndex + 1, 1);
            var pSize = ParseHelper.ToInt(model.pageSize, 20);
            var criteria = new Ets.Model.DataModel.Clienter.ClientOrderSearchCriteria()
            {
                PagingRequest = new Ets.Model.Common.PagingResult(pIndex, pSize),
                city = model.city,
                cityId = model.cityId
            };
            var pagedList = new Ets.Service.Provider.Order.OrderProvider().GetOrdersNoLoginLatest(criteria);
            //var pagedList = ClienterLogic.clienterLogic().GetOrdersNoLoginLatest(criteria);
            //var lists = ClientOrderNoLoginResultModelTranslator.Instance.Translate(pagedList);

            return Ets.Model.Common.ResultModel<Ets.Model.DomainModel.Clienter.ClientOrderNoLoginResultModel[]>.Conclude(GetOrdersNoLoginStatus.Success, pagedList.ToArray());
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
            var pSize = model.pageSize.HasValue ? model.pageSize.Value : 20;
            var criteria = new ClientOrderSearchCriteria()
            {
                PagingRequest = new SuperManCore.Paging.PagingResult(pIndex, pSize),
                userId = model.userId,
                status = model.status,
                isLatest = model.isLatest
            };
            var pagedList = ClienterLogic.clienterLogic().GetOrdersNoLogin(criteria);
            var lists = ClientOrderNoLoginResultModelTranslator.Instance.Translate(pagedList);
            if (!model.isLatest) //不是最新任务的话就按距离排序,否则按发布时间排序
            {
                lists = lists.OrderBy(i => i.distance).ToList();
            }
            return ResultModel<ClientOrderNoLoginResultModel[]>.Conclude(GetOrdersNoLoginStatus.Success, lists.ToArray());
        }



        #region 获取海底捞 送餐任务 和 取餐盒任务

        /// <summary>
        /// 获取送餐任务
        /// </summary>
        /// <returns></returns>
        [ActionStatus(typeof(GetOrdersNoLoginStatus))]
        [HttpPost]
        public ResultModel<ClientOrderNoLoginResultModel[]> GetJobListSongCanTask_C(ClientOrderInfoModel model)
        {
            //degree.longitude = model.longitude;
            //degree.latitude = model.latitude;
            var pIndex = model.pageIndex.HasValue ? model.pageIndex.Value : 0;
            var pSize = model.pageSize.HasValue ? model.pageSize.Value : int.MaxValue;
            var criteria = new ClientOrderSearchCriteria()
            {
                PagingRequest = new SuperManCore.Paging.PagingResult(pIndex, pSize),
                userId = model.userId,
                status = model.status,
                isLatest = model.isLatest,
                city = string.IsNullOrWhiteSpace(model.city) ? null : model.city.Trim(),
                cityId = string.IsNullOrWhiteSpace(model.cityId) ? null : model.cityId.Trim(),
                OrderType = 1 //送餐任务1，取餐盒任务2
            };

            if (!string.IsNullOrWhiteSpace(model.city))
            {
                if (model.city.Contains("北京"))
                {
                    criteria.cityId = "10201";
                }
                if (model.city.Contains("上海"))
                {
                    criteria.cityId = "11101";
                }
            }
            var pagedList = ClienterLogic.clienterLogic().GetOrdersForSongCanOrQuCan(criteria);
            var lists = ClientOrderNoLoginResultModelTranslator.Instance.Translate(pagedList);
            if (!model.isLatest) //不是最新任务的话就按距离排序,否则按发布时间排序
            {
                lists = lists.OrderBy(i => i.distance).ToList();
            }
            return ResultModel<ClientOrderNoLoginResultModel[]>.Conclude(GetOrdersNoLoginStatus.Success, lists.ToArray());
        }

        /// <summary>
        /// 获取取餐盒任务
        /// </summary>
        /// <returns></returns>
        [ActionStatus(typeof(GetOrdersNoLoginStatus))]
        [HttpPost]
        public ResultModel<ClientOrderNoLoginResultModel[]> GetJobListCanHeTask_C(ClientOrderInfoModel model)
        {
            //degree.longitude = model.longitude;
            //degree.latitude = model.latitude;
            var pIndex = model.pageIndex.HasValue ? model.pageIndex.Value : 0;
            var pSize = model.pageSize.HasValue ? model.pageSize.Value : int.MaxValue;
            var criteria = new ClientOrderSearchCriteria()
            {
                PagingRequest = new SuperManCore.Paging.PagingResult(pIndex, pSize),
                userId = model.userId,
                status = model.status,
                isLatest = model.isLatest,
                city = string.IsNullOrWhiteSpace(model.city) ? null : model.city.Trim(),
                cityId = string.IsNullOrWhiteSpace(model.cityId) ? null : model.cityId.Trim(),
                OrderType = 2 //送餐任务1，取餐盒任务2
            };

            if (!string.IsNullOrWhiteSpace(model.city))
            {
                if (model.city == "北京市")
                {
                    criteria.cityId = "10201";
                }
                if (model.city == "上海市")
                {
                    criteria.cityId = "11101";
                }
            }
            var pagedList = ClienterLogic.clienterLogic().GetOrdersForSongCanOrQuCan(criteria);
            var lists = ClientOrderNoLoginResultModelTranslator.Instance.Translate(pagedList);
            if (!model.isLatest) //不是最新任务的话就按距离排序,否则按发布时间排序
            {
                lists = lists.OrderBy(i => i.distance).ToList();
            }
            return ResultModel<ClientOrderNoLoginResultModel[]>.Conclude(GetOrdersNoLoginStatus.Success, lists.ToArray());
        }


        #endregion

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
            if (userId == 0) //用户id验证
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.userIdEmpty);
            if (string.IsNullOrEmpty(orderNo)) //订单号码非空验证
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.OrderEmpty);
            if (ClienterLogic.clienterLogic().GetOrderByNo(orderNo) == null) //查询订单是否存在
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.OrderIsNotExist);
            if (!ClienterLogic.clienterLogic().CheckOrderIsAllowRush(orderNo))  //查询订单是否被抢
                return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.OrderIsNotAllowRush);
            lock (lockHelper)
            {
                bool bResult = ClienterLogic.clienterLogic().RushOrder(userId, orderNo);
                if (bResult)
                    return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.Success);
                else
                    return ResultModel<RushOrderResultModel>.Conclude(RushOrderStatus.Failed);
            }
        }
        /// <summary>
        /// 完成订单 edit by caoheyang 20150204
        /// </summary>
        /// <param name="userId">C端用户id</param>
        /// <param name="orderNo">订单号码</param>
        /// <returns></returns>
        [ActionStatus(typeof(FinishOrderStatus))]
        [HttpGet]
        public ResultModel<FinishOrderResultModel> FinishOrder_C(int userId, string orderNo)
        {
            if (userId == 0)  //用户id非空验证
                return ResultModel<FinishOrderResultModel>.Conclude(FinishOrderStatus.userIdEmpty);
            if (string.IsNullOrEmpty(orderNo)) //订单号码非空验证
                return ResultModel<FinishOrderResultModel>.Conclude(FinishOrderStatus.OrderEmpty);
            if (ClienterLogic.clienterLogic().GetOrderByNo(orderNo) == null) //订单是否存在验证
                return ResultModel<FinishOrderResultModel>.Conclude(FinishOrderStatus.OrderIsNotExist);
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
        /// <param name="phoneNo">手机号</param>
        /// <returns></returns>
        [ActionStatus(typeof(RushOrderStatus))]
        [HttpGet]
        public ResultModel<MyBalanceResultModel> GetMyBalance(string phoneNo)
        {
            if (string.IsNullOrEmpty(phoneNo))
            {
                return ResultModel<MyBalanceResultModel>.Conclude(GetMyBalanceStatus.PhoneEmpty);
            }
            var item = ClienterLogic.clienterLogic().GetMyBalanceByPhoneNo(phoneNo);
            var result = new MyBalanceResultModel()
            {
                MyBalance = item
            };
            return ResultModel<MyBalanceResultModel>.Conclude(FinishOrderStatus.Success, result);
        }

        /// <summary>
        /// 获取我的余额动态
        /// </summary>
        /// <param name="phoneNo">手机号</param>
        /// <returns></returns>
        [ActionStatus(typeof(RushOrderStatus))]
        [HttpGet]
        public ResultModel<MyBalanceListResultModel[]> GetMyBalanceDynamic(string phoneNo, int? pagedSize, int? pagedIndex)
        {
            var pIndex = pagedIndex.HasValue ? pagedIndex.Value : 0;
            var pSize = pagedSize.HasValue ? pagedSize.Value : int.MaxValue;
            var criteria = new MyIncomeSearchCriteria()
            {
                PagingRequest = new SuperManCore.Paging.PagingResult(pIndex, pSize),
                phoneNo = phoneNo
            };
            var pagedList = ClienterLogic.clienterLogic().GetMyIncomeList(criteria);
            var lists = MyBalanceListResultModelTranslator.Instance.Translate(pagedList);
            return ResultModel<MyBalanceListResultModel[]>.Conclude(RushOrderStatus.Success, lists.ToArray());
        }

        /// <summary>
        /// 请求动态验证码
        /// </summary>
        /// <param name="PhoneNumber">手机号</param>
        /// <param name="type">操作类型： 0 注册 1修改密码</param>
        /// <returns></returns>
        [ActionStatus(typeof(SendCheckCodeStatus))]
        [HttpGet]
        public SimpleResultModel CheckCode(string PhoneNumber, string type)
        {
            if (!CommonValidator.IsValidPhoneNumber(PhoneNumber))
            {
                return SimpleResultModel.Conclude(SendCheckCodeStatus.InvlidPhoneNumber);
            }
            var randomCode = new Random().Next(100000).ToString("D6");
            string msg = string.Empty;
            if (type == "0")//注册
            {
                if (ClienterLogic.clienterLogic().CheckExistPhone(PhoneNumber))  //判断该手机号是否已经注册过
                {
                    return SimpleResultModel.Conclude(SendCheckCodeStatus.AlreadyExists);
                }
                msg = string.Format(SupermanApiConfig.Instance.SmsContentCheckCode, randomCode, ConstValues.MessageBusiness);
            }
            else //修改密码
            {
                msg = string.Format(SupermanApiConfig.Instance.SmsContentFindPassword, randomCode, ConstValues.MessageClinenter);
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
                GetOrdersStatus.Success,
                new ServicePhone().GetCustomerServicePhone(CityName)
                );

        }

    }
}