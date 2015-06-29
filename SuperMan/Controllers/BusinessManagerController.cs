using System.Text.RegularExpressions;
using Ets.Model.DomainModel.Business;
using Ets.Service.IProvider.User;
using Ets.Service.Provider.Distribution;
using Ets.Service.Provider.User;
using ETS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ets.Model.ParameterModel.Business;
using Ets.Service.Provider.Common;
using Ets.Service.IProvider.Common;
using Ets.Model.ParameterModel.User;
using SuperMan.App_Start;
using Ets.Model.ParameterModel.Order;
using Ets.Model.DataModel.Business;
using Ets.Service.Provider.Finance;
using Ets.Service.IProvider.Finance;
using Ets.Model.ParameterModel.Finance;
using System.Text;
using BusinessSearchCriteria = Ets.Model.ParameterModel.Business.BusinessSearchCriteria;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using Ets.Service.Provider.Clienter;
using Ets.Service.IProvider.Clienter;
using Ets.Service.IProvider.Business;
using Ets.Service.Provider.Business;
using Ets.Model.Common;
using Ets.Service.IProvider.Distribution;
using Ets.Model.DomainModel.Statistics;
using Ets.Service.Provider.Statistics;
using Ets.Service.IProvider.Statistics;
namespace SuperMan.Controllers
{
    [WebHandleError]
    public class BusinessManagerController : BaseController
    {
        /// <summary>
        /// 商户业务类
        /// </summary>
        readonly IBusinessProvider iBusinessProvider = new BusinessProvider();
        readonly IClienterProvider iClienterProvider = new ClienterProvider();
        readonly IAreaProvider iAreaProvider = new AreaProvider();
        readonly IBusinessFinanceProvider iBusinessFinanceProvider = new BusinessFinanceProvider();
        readonly IBusinessClienterRelationProvider iBusinessClienterRelationProvider = new BusinessClienterRelationProvider();
        readonly IDistributionProvider iDistributionProvider = new DistributionProvider();
        private readonly IStatisticsProvider statisticsProvider = new StatisticsProvider();

        // GET: BusinessManager
        [HttpGet]
        public ActionResult BusinessManager()
        {
            ViewBag.txtGroupId = SuperMan.App_Start.UserContext.Current.GroupId;//集团id

            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id

            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(ParseHelper.ToInt(UserType));
            var criteria = new BusinessSearchCriteria()
            {
                Status = -1,
                GroupId = UserContext.Current.GroupId,
                MealsSettleMode = -1,
                AuthorityCityNameListStr = iAreaProvider.GetAuthorityCityNameListStr(UserType)
            };
            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return View();
            }
            var pagedList = iBusinessProvider.GetBusinesses(criteria);
            return View(pagedList);
        }


        [HttpPost]
        public ActionResult PostBusinessManager(int pageindex = 1)
        {
            var criteria = new BusinessSearchCriteria();
            TryUpdateModel(criteria);
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            criteria.AuthorityCityNameListStr =
                iAreaProvider.GetAuthorityCityNameListStr(ParseHelper.ToInt(UserType));
            ViewBag.txtGroupId = UserContext.Current.GroupId;//集团id
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(UserType);
            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return PartialView("_BusinessManageList");
            }
            var pagedList = iBusinessProvider.GetBusinesses(criteria);
            return PartialView("_BusinessManageList", pagedList);
        }

        [HttpPost]
        public JsonResult AuditOK(int id)
        {
            bool b = iBusinessProvider.UpdateAuditStatus(id, ETS.Enums.EnumStatusType.审核通过);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public JsonResult AuditCel(int id)
        {
            iBusinessProvider.UpdateAuditStatus(id, ETS.Enums.EnumStatusType.审核取消);
            return Json(new ResultModel(true, string.Empty), JsonRequestBehavior.DenyGet);
        }


        /// <summary>
        /// 根据城市信息查询当前城市下该集团的所有商户信息  add by caoheyang 20150302
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetBussinessByCityInfo(BusinessSearchCriteria model)
        {
            return Json(iBusinessProvider.GetBussinessByCityInfo(model).ToList(), JsonRequestBehavior.DenyGet);
        }
        [HttpGet]
        public ActionResult BusinessSettlementSet()
        {
            return View("BusinessSettlementSet");
        }

        /// <summary>
        /// 设置商家结算比例-外送费
        /// </summary>
        /// <param name="id">商家id</param>
        /// <param name="commission">结算比例</param>
        /// <param name="waisongfei">外送费</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetCommission(int id, decimal commission, decimal waisongfei, int commissionType, decimal commissionFixValue, int strategyID)
        {
            if (commission < 0)
                return Json(new ResultModel(false, "结算比例不能小于零!"), JsonRequestBehavior.AllowGet);
            if (waisongfei < 0)
                return Json(new ResultModel(false, "外送费不能小于零!"), JsonRequestBehavior.AllowGet);
            string remark = "";
            IBusinessProvider iBus = new BusinessProvider();
            UserOptRecordPara model = new UserOptRecordPara()
            {
                OptUserId = UserContext.Current.Id,//后台用户id
                OptUserName = UserContext.Current.Name, //后台用户
                UserID = id, //商户id 
                UserType = 1, //被操作人类型
                Remark = string.Format(string.Format("将商户id为{0}的商户外送费设置为{1},结算比例设置为{2}", id, waisongfei, commission))
            };
            BusListResultModel busListResultModel = new BusListResultModel()
            {
                Id = id,
                BusinessCommission = commission,
                DistribSubsidy = waisongfei,
                CommissionType = commissionType,
                CommissionFixValue = commissionFixValue,
                BusinessGroupId = strategyID

            };
            return Json(new ResultModel(iBus.ModifyCommission(busListResultModel, model), "成功!"), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加商户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddBusiness(AddBusinessModel model)
        {
            TryUpdateModel(model);

            var result = iBusinessProvider.AddBusiness(model);
            if (result.Status == 0)
            {
                return Json(new ResultModel(true, "成功!"), JsonRequestBehavior.DenyGet);
            }
            return Json(new ResultModel(false, result.Message), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 修改商户信息
        /// </summary>
        /// <param name="id">商户Id</param>
        /// <param name="businessName">商户名称</param>
        /// <param name="businessPhone">商户电话</param>
        /// <param name="businessSourceId">第三方商户id</param>
        /// <param name="groupId">集团Id</param>
        /// <param name="oldBusiSourceId">之前的第三方商户Id</param>
        /// <param name="oldBusGroupId">之前的集团Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifyBusiness(int id, string businessName, string businessPhone, int businessSourceId, int groupId, int oldBusiSourceId, int oldBusGroupId, int mealsSettleMode)
        {
            IBusinessProvider iBus = new BusinessProvider();
            //操作日志
            OrderOptionModel model = new OrderOptionModel()
            {
                OptUserId = UserContext.Current.Id,
                OptUserName = UserContext.Current.Name,
            };
            //商户操作实体
            BusinessModel businessModel = new BusinessModel()
            {
                Name = businessName,
                GroupId = groupId,
                OriginalBusiId = businessSourceId,
                Id = id,
                PhoneNo = businessPhone,
                oldGroupId = oldBusGroupId,
                oldOriginalBusiId = oldBusiSourceId,
                MealsSettleMode = mealsSettleMode
            };
            return Json(new ResultModel(iBus.ModifyBusinessInfo(businessModel, model), "成功!"), JsonRequestBehavior.DenyGet);
        }       

        /// <summary>
        /// 查看商户详细信息
        /// danny-20150512
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public ActionResult BusinessDetail(string businessId)
        {
            var criteria = new BusinessBalanceRecordSerchCriteria()
            {
                BusinessId = Convert.ToInt32(businessId)
            };
            ViewBag.businessBalanceRecord = iBusinessFinanceProvider.GetBusinessBalanceRecordListOfPaging(criteria);

            var queryCriteria = new BussinessBalanceQuery();
            queryCriteria.BusinessId = businessId;
            ViewBag.TotalAmount = statisticsProvider.QueryBusinessTotalAmount(queryCriteria);

            var businessWithdrawFormModel = iBusinessProvider.GetBusinessDetailById(businessId);
            return View(businessWithdrawFormModel);
        }

        /// <summary>
        /// 查看商户余额流水记录
        /// danny-20150512
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public ActionResult BusinessBalanceRecord(BusinessBalanceRecordSerchCriteria criteria)
        {
            ViewBag.businessBalanceRecord = iBusinessFinanceProvider.GetBusinessBalanceRecordList(criteria);
            return PartialView("_BusinessBalanceRecordList");
        }
        /// <summary>
        /// 查看商户余额流水记录分页版
        /// danny-20150604
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostBusinessBalanceRecord(int pageindex = 1)
        {
            var criteria = new BusinessBalanceRecordSerchCriteria();
            TryUpdateModel(criteria);
            ViewBag.businessBalanceRecord = iBusinessFinanceProvider.GetBusinessBalanceRecordListOfPaging(criteria);
            return PartialView("_BusinessBalanceRecordList");
        }
        /// <summary>
        /// 导出商户余额流水记录
        /// danny-20150512
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportBusinessBalanceRecord()
        {
            var criteria = new BusinessBalanceRecordSerchCriteria();
            TryUpdateModel(criteria);
            var dtBusinessBalanceRecord = iBusinessFinanceProvider.GetBusinessBalanceRecordListForExport(criteria);
            if (dtBusinessBalanceRecord != null && dtBusinessBalanceRecord.Count > 0)
            {

                string filname = "商户提款流水记录{0}.xls";
                if (!string.IsNullOrWhiteSpace(criteria.OperateTimeStart))
                {
                    filname = string.Format(filname, criteria.OperateTimeStart + "~" + criteria.OperateTimeEnd);
                }
                byte[] data = Encoding.UTF8.GetBytes(iBusinessFinanceProvider.CreateBusinessBalanceRecordExcel(dtBusinessBalanceRecord.ToList()));
                return File(data, "application/ms-excel", filname);
            }
            var businessWithdrawFormModel = iBusinessProvider.GetBusinessDetailById(criteria.BusinessId.ToString());
            var criteriaNew = new BusinessBalanceRecordSerchCriteria()
            {
                BusinessId = Convert.ToInt32(criteria.BusinessId)
            };
            ViewBag.businessBalanceRecord = iBusinessFinanceProvider.GetBusinessBalanceRecordList(criteriaNew);
            return View("BusinessDetail", businessWithdrawFormModel);
        }
        /// <summary>
        /// 商户充值
        /// danny-20150526
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BusinessRecharge(BusinessOptionLog model)
        {
            model.OptName = UserContext.Current.Name;
            var reg = iBusinessFinanceProvider.BusinessRecharge(model);
            return Json(new ResultModel(reg, reg ? "充值成功！" : "充值失败！"), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 商户提现        
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150626</UpdateTime>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Withdraw(WithdrawBBackPM model)
        {
            int FinanceAccountId= iBusinessFinanceProvider.GetBFinanceAccountId(model.BusinessId);
            if(FinanceAccountId==0)
                return Json(new ResultModel(false, "商户金融账号不存在！"), JsonRequestBehavior.DenyGet);

            model.FinanceAccountId = FinanceAccountId;
            var reg = iBusinessFinanceProvider.WithdrawB(model);            

            return Json(reg, JsonRequestBehavior.DenyGet);            
        }
        /// <summary>
        /// 查询商户综合信息
        /// danny-20150601
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public ActionResult QueryBusinessDetail(string businessId)
        {
            var businessDetailModel = iBusinessProvider.GetBusinessDetailById(businessId);
            var userType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
            var isStarTimeSubsidies = new GlobalConfigProvider().GlobalConfigMethod(businessDetailModel.BusinessGroupId).IsStarTimeSubsidies;
            var isStartOverStoreSubsidies = new GlobalConfigProvider().GlobalConfigMethod(businessDetailModel.BusinessGroupId).IsStartOverStoreSubsidies;
            var subsidyConfig = "";
            if (isStartOverStoreSubsidies == "1")
            {
                subsidyConfig = "全局补贴：跨店抢单奖励";
            }
            if (isStarTimeSubsidies == "1")
            {
                subsidyConfig = string.IsNullOrWhiteSpace(subsidyConfig) ? "全局补贴：动态时间奖励" : "全局补贴：跨店抢单奖励和动态时间奖励";
            }
            ViewBag.subsidyConfig = subsidyConfig;
            ViewBag.openCityList = iAreaProvider.GetOpenCityOfSingleCity(ParseHelper.ToInt(userType));
            ViewBag.openAreaList = iAreaProvider.GetOpenCityDistrict(ParseHelper.ToInt(businessDetailModel.CityId));
            ViewBag.businessThirdRelation = iBusinessProvider.GetBusinessThirdRelation(ParseHelper.ToInt(businessId));
            ViewBag.BusinessOpLog = iBusinessProvider.GetBusinessOpLog(ParseHelper.ToInt(businessId,0));
            return View("BusinessModify", businessDetailModel);
        }
        /// <summary>
        /// 根据城市Id获取区县信息
        /// danny-20150601
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetOpenCityDistrict(int cityId)
        {
            return Json(iAreaProvider.GetOpenCityDistrict(cityId), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 修改商户综合信息
        /// danny-20150601
        /// </summary>
        /// <param name="businessDetailModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifyBusinessDetail(BusinessDetailModel businessDetailModel)
        {
            businessDetailModel.OptUserId = UserContext.Current.Id;
            businessDetailModel.OptUserName = UserContext.Current.Name;
            var reg = iBusinessProvider.ModifyBusinessDetail(businessDetailModel);
            return Json(new Ets.Model.Common.ResultModel(reg.DealFlag, reg.DealMsg), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 根据商户Id获取收支记录
        /// danny-20150604
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public ActionResult GetBusinessBalanceRecordListOfPaging(string businessId)
        {
            var criteria = new BusinessBalanceRecordSerchCriteria()
            {
                BusinessId = Convert.ToInt32(businessId)
            };
            var pagedList = iBusinessFinanceProvider.GetBusinessBalanceRecordListOfPaging(criteria);
            return View(pagedList);
        }
        /// <summary>
        /// 根据商户Id获取收支记录
        /// danny-20150604
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public ActionResult PostGetBusinessBalanceRecordListOfPaging(string businessId)
        {
            var criteria = new BusinessBalanceRecordSerchCriteria()
            {
                BusinessId = Convert.ToInt32(businessId)
            };
            var pagedList = iBusinessFinanceProvider.GetBusinessBalanceRecordListOfPaging(criteria);
            return View(pagedList);
        }


        /// <summary>
        /// 查看商户绑定骑士列表（初始化）
        /// danny-20150608
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public ActionResult ClienterBindManage(string businessId)
        {
            var businessDetailModel = iBusinessProvider.GetBusinessDetailById(businessId);
            businessDetailModel.BindClienterQty =
                iBusinessProvider.GetBusinessBindClienterQty(ParseHelper.ToInt(businessId));
            var criteria = new BusinessSearchCriteria()
            {
                BusinessId = ParseHelper.ToInt(businessId),
                PageSize = 20
            };
            ViewBag.businessClienterRelationList = iBusinessProvider.GetBusinessClienterRelationList(criteria);
            return View(businessDetailModel);
        }
        /// <summary>
        /// 查看商户绑定骑士列表(翻页)
        /// danny-20150608
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostClienterBindManage(int pageindex = 1)
        {
            var criteria = new BusinessSearchCriteria();
            TryUpdateModel(criteria);
            criteria.PageSize = 20;
            ViewBag.businessClienterRelationList = iBusinessProvider.GetBusinessClienterRelationList(criteria);
            return PartialView("_ClienterBindList");
        }

        /// <summary>
        /// 修改骑士绑定
        /// danny-20150608
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifyClienterBind(ClienterBindOptionLogModel model)
        {
            model.OptId = UserContext.Current.Id;
            model.OptName = UserContext.Current.Name;
            model.Remark = "修改绑定";
            var reg = iBusinessProvider.ModifyClienterBind(model);
            return Json(new Ets.Model.Common.ResultModel(reg, reg ? "修改绑定成功！" : "修改绑定失败！"), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 修改骑士绑定
        /// danny-20150608
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RemoveClienterBind(ClienterBindOptionLogModel model)
        {
            model.OptId = UserContext.Current.Id;
            model.OptName = UserContext.Current.Name;
            model.Remark = "删除骑士绑定";
            var reg = iBusinessProvider.RemoveClienterBind(model);
            return Json(new Ets.Model.Common.ResultModel(reg, reg ? "删除骑士绑定成功！" : "删除骑士绑定失败！"), JsonRequestBehavior.DenyGet);
        }
        /// <summary>
        /// 添加骑士绑定查询
        /// danny-20150609
        /// </summary>
        /// <param name="businessId">商户Id</param>
        /// <returns></returns>
        public ActionResult AddClienterBindManage(string businessId)
        {
            var businessDetailModel = iBusinessProvider.GetBusinessDetailById(businessId);

            var criteria = new Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria();

            ViewBag.clienterList = iClienterProvider.GetClienterList(criteria);
            return View(businessDetailModel);
        }
        /// <summary>
        /// 查询骑士列表
        /// danny-20150609
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetClienterList(int pageindex = 1)
        {
            var criteria = new Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria();
            TryUpdateModel(criteria);
            ViewBag.clienterList = iClienterProvider.GetClienterList(criteria);
            return PartialView("_ClienterList");
        }
        /// <summary>
        /// 添加骑士绑定
        /// danny-20150609
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult AddClienterBind(ClienterBindOptionLogModel model)
        {
            if (iBusinessProvider.CheckHaveBind(model))
            {
                return Json(new Ets.Model.Common.ResultModel(false, "此条绑定关系已存在！", JsonRequestBehavior.DenyGet));
            }
            model.OptId = UserContext.Current.Id;
            model.OptName = UserContext.Current.Name;
            model.Remark = "添加绑定";
            var reg = iBusinessProvider.AddClienterBind(model);
            return Json(new Ets.Model.Common.ResultModel(reg, reg ? "添加绑定成功！" : "添加绑定失败！"), JsonRequestBehavior.DenyGet);
        }

        public ActionResult ClienterBatchBind(string businessId)
        {
            var businessDetailModel = iBusinessProvider.GetBusinessDetailById(businessId);               
            return View(businessDetailModel);
        }

        /// <summary>
        /// 批量添加骑士绑定
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150610</UpdateTime>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ClienterImport()
        {
            string businessId = Request.Params["BusinessId"].ToString();
            List<BusinessBindClienterDM> list = new List<BusinessBindClienterDM>();
            Stream fs = null;
            IWorkbook wk = null;

            try
            {
                if (Request.Files["file1"] != null && Request.Files["file1"].FileName != "")
                {
                    HttpPostedFileBase file = Request.Files["file1"];
                    fs = file.InputStream;
                    if (Path.GetExtension(Request.Files["file1"].FileName) == ".xls")
                        wk = new HSSFWorkbook(fs);                                      
                    else
                        wk = new XSSFWorkbook(fs);
                     
                    ISheet st = wk.GetSheetAt(0);
                    int rowCount = st.LastRowNum;
                    if (rowCount > 50)
                    {
                        rowCount = 50;
                        return Json(new Ets.Model.Common.ResultModel(false, "每次最多导入50行数据！", JsonRequestBehavior.DenyGet));
                    }

                    list = GetList(st, rowCount);                   
                }
            }
            catch (Exception ex)
            {
                fs.Close();
            }

            return Json(list);
            
        }
   
        /// <summary>
        /// 批量保存骑士绑定
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150610</UpdateTime>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ClienterBatchSave()
        {                      
            string businessId = Request.Params["BusinessId"].ToString();
            string OverallS = Request.Params["OverallS"].ToString();
            List<BusinessBindClienterDM> list = ParseHelper.JSONStringToList<BusinessBindClienterDM>(Request.Params["OverallS"].ToString());              

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsBind)//前台选中绑定记录
                {
                    string phone = list[i].ClienterPhoneNo;
                    string name = list[i].ClienterName;
                    int clienterId = iClienterProvider.GetId(phone, name);

                    BusinessClienterRelation model = iBusinessClienterRelationProvider.GetDetails(new BusinessClienterRelationPM
                                                    {
                                                        BusinessId = Convert.ToInt32(businessId),
                                                        ClienterId = clienterId
                                                    });
                    if (model == null)//插入
                    {                               
                        iBusinessProvider.AddClienterBind(new ClienterBindOptionLogModel { 
                                                    BusinessId=Convert.ToInt32(businessId),
                                                    ClienterId=clienterId,
                                                    OptId = UserContext.Current.Id,
                                                    OptName = UserContext.Current.Name,
                                                    Remark = "添加绑定"
                                                });
                    }
                    else if(model!=null && model.IsBind==0)//更新                   
                    {                     
                        iBusinessProvider.ModifyClienterBind(new ClienterBindOptionLogModel 
                                            {
                                                BusinessId = Convert.ToInt32(businessId),
                                                ClienterId = clienterId,
                                                OptId = UserContext.Current.Id,
                                                OptName = UserContext.Current.Name,
                                                Remark = "修改绑定",
                                                IsBind=1
                                            });                
                    }
                }             
            }
          
            return Json(new Ets.Model.Common.ResultModel(true, "保存成功！", JsonRequestBehavior.DenyGet));
        }

        #region
        
        /// <summary>
        /// 获取批量导入列表
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150611</UpdateTime>
        /// <param name="st"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        List<BusinessBindClienterDM> GetList(ISheet st,int rowCount)
        {
            List<BusinessBindClienterDM> list = new List<BusinessBindClienterDM>();
            for (int i = 1; i <= rowCount; i++)
            {
                string name = "", phone = "";
                if (st.GetRow(i) != null && st.GetRow(i).GetCell(0) != null)//用户名
                    name = st.GetRow(i).GetCell(0).ToString();
                if (st.GetRow(i) != null && st.GetRow(i).GetCell(1) != null)//手机号
                    phone = st.GetRow(i).GetCell(1).ToString();
                BusinessBindClienterDM model = new BusinessBindClienterDM();
                model.RowCount = i;
                model.ClienterName = name;
                model.ClienterPhoneNo = phone;

                if (string.IsNullOrEmpty(phone))//手机号为空
                {
                    model.ClienterRemarks = "骑士手机错误";
                    model.IsBind = false;
                    model.IsEnable = false;
                    list.Add(model);
                    continue;
                }

                Regex dReg = new Regex("^1\\d{10}$");
                if (!dReg.IsMatch(phone))//验证收货人手机号
                {
                    model.ClienterRemarks = "骑士手机错误";
                    model.IsBind = false;
                    model.IsEnable = false;
                    list.Add(model);
                    continue;
                }

                string trueName = iClienterProvider.GetName(phone);
                if (string.IsNullOrEmpty(trueName))
                {
                    model.ClienterRemarks = "骑士手机不存在";
                    model.IsBind = false;
                    model.IsEnable = false;
                    list.Add(model);
                    continue;
                }

                if (name != trueName)
                {
                    model.ClienterRemarks = "骑士名称错误";
                    model.IsBind = false;
                    model.IsEnable = false;
                    list.Add(model);
                    continue;
                }

                model.ClienterRemarks = "";
                model.IsBind = true;
                model.IsEnable = true;
                list.Add(model);
            }

            return list;
        }
        #endregion

        /// <summary>
        /// 根据单号查询充值详情
        /// </summary>
        /// <UpdateBy>zhaohailong</UpdateBy>
        /// <UpdateTime>20150624</UpdateTime>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ContentResult GetBusinessRechargeDetailByNo(string orderNo)
        {
            BusinessRechargeDetail detailModel = iBusinessFinanceProvider.GetBusinessRechargeDetailByNo(orderNo);
            if (detailModel==null)
            {
                return null;
            }
            return new ContentResult { Content = Newtonsoft.Json.JsonConvert.SerializeObject(detailModel) };
        }

    }
}