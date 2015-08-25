using System;
using System.Web;
using System.Web.Http;
using Ets.Model.Common;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Finance;
using ETS.Util;
using ETS.Enums;
using Ets.Model.DomainModel.Business;
using SuperManWebApi.App_Start.Filters;
using Ets.Model.ParameterModel.Business;
using Ets.Model.DataModel.Business;
using SuperManWebApi.Providers;
using Ets.Service.IProvider.Business;
using Ets.Service.Provider.Business;
using Ets.Model.ParameterModel.Common;
using ETS.Security;
namespace SuperManWebApi.Controllers
{
    /// <summary>
    /// 商户相关接口 add by caoheyang
    /// </summary>
    [ExecuteTimeLog]
    public class BusinessController : ApiController
    {
        readonly IBusinessBalanceRecordProvider iBusinessBalanceRecordProvider = new BusinessBalanceRecordProvider();
        readonly IBusinessProvider iBusinessProvider = new BusinessProvider();

        /// <summary>
        /// 商户交易流水API caoheyang 20150512
        /// </summary>
        /// <param name="model">查询参数实体</param>
        /// <returns></returns>
        [HttpPost]
        [Token]
        [Validate]
        [ApiVersion]
        public ResultModel<object> Records(BussinessRecordsPM model)
        {
            return iBusinessBalanceRecordProvider.GetRecords(model.BusinessId);
        }               
 

        /// <summary>
        /// 获取商户详情       
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="model">商户参数</param>
        /// <returns></returns>        
        [HttpPost]
        //[Token]
        public ResultModel<BusinessDM> Get(ParamModel ParModel)
        {
            BussinessPM model = JsonHelper.JsonConvertToObject<BussinessPM>(AESApp.AesDecrypt(ParModel.data));
            #region 验证
            var version = model.Version;
            if (string.IsNullOrWhiteSpace(version)) //版本号 
            {
                return ResultModel<BusinessDM>.Conclude(GetBussinessStatus.NoVersion);
            }
            if (model.BussinessId < 0)//商户Id不合法
            {
                return ResultModel<BusinessDM>.Conclude(GetBussinessStatus.ErrNo);
            }
            if (!iBusinessProvider.IsExist(model.BussinessId)) //商户不存在
            {
                return ResultModel<BusinessDM>.Conclude(GetBussinessStatus.FailedGet);
            }

            #endregion

            try
            {
                BusinessDM businessDM = iBusinessProvider.GetDetails(model.BussinessId);
                return Ets.Model.Common.ResultModel<BusinessDM>.Conclude(GetBussinessStatus.Success, businessDM);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("ResultModel<BusinessDM> Get", new { obj = "时间：" + DateTime.Now.ToString() + ex.Message });
                return ResultModel<BusinessDM>.Conclude(GetBussinessStatus.Failed);
            }
        }

        /// <summary>
        /// 获取商户外送费      
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="model">商户参数</param>
        /// <returns></returns>        
        [HttpPost]
        [Token]
        public ResultModel<BusiDistribSubsidyResultModel> GetDistribSubsidy(BussinessPM model)
        {
            #region 验证
            var version = model.Version;
            if (string.IsNullOrWhiteSpace(version)) //版本号 
            {
                return ResultModel<BusiDistribSubsidyResultModel>.Conclude(GetBussinessStatus.NoVersion);
            }
            if (model.BussinessId < 0)//商户Id不合法
            {
                return ResultModel<BusiDistribSubsidyResultModel>.Conclude(GetBussinessStatus.ErrNo);
            }
            if (!iBusinessProvider.IsExist(model.BussinessId)) //商户不存在
            {
                return ResultModel<BusiDistribSubsidyResultModel>.Conclude(GetBussinessStatus.FailedGet);
            }
            if (model.OrderCount <= 0) //子订单数量不能小于1
            {
                return ResultModel<BusiDistribSubsidyResultModel>.Conclude(GetBussinessStatus.OrderCountError);
            }

            #endregion

            try
            {
                var ret = iBusinessProvider.GetBusinessPushOrderInfo(model.BussinessId, model.OrderCount, model.Amount);
                return Ets.Model.Common.ResultModel<BusiDistribSubsidyResultModel>.Conclude(GetBussinessStatus.Success, ret);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("ResultModel<decimal> GetDistribSubsidy", new { obj = "时间：" + DateTime.Now.ToString() + ex.Message });
                return ResultModel<BusiDistribSubsidyResultModel>.Conclude(GetBussinessStatus.Failed);
            }
        }


        /// <summary>
        /// B端修改商户信息 caoheyang
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Token]
        public ResultModel<BusiModifyResultModelDM> UpdateBusinessInfoB()
        {
            BusiAddAddressInfoModel model = new BusiAddAddressInfoModel();
            model.userId = ParseHelper.ToInt(HttpUtility.UrlDecode(HttpContext.Current.Request.Form["UserId"]), 0); //商户id
            model.businessName = HttpUtility.UrlDecode(HttpContext.Current.Request.Form["businessName"]); //商户名称
            model.Address = HttpUtility.UrlDecode(HttpContext.Current.Request.Form["Address"]);//详细地址
            model.phoneNo = HttpUtility.UrlDecode(HttpContext.Current.Request.Form["phoneNo"]);  // 手机号  2
            model.landLine = HttpUtility.UrlDecode(HttpContext.Current.Request.Form["landLine"]); // 座机
            model.Province = HttpUtility.UrlDecode(HttpContext.Current.Request.Form["Province"]); // 省份
            model.City = HttpUtility.UrlDecode(HttpContext.Current.Request.Form["City"]);// 城市
            model.districtName = HttpUtility.UrlDecode(HttpContext.Current.Request.Form["districtName"]);// 区
            model.longitude = ParseHelper.ToDouble(HttpUtility.UrlDecode(HttpContext.Current.Request.Form["longitude"]));//经度
            model.latitude = ParseHelper.ToDouble(HttpUtility.UrlDecode(HttpContext.Current.Request.Form["latitude"])); //纬度
            model.IsUpdateCheckPicUrl = ParseHelper.ToInt(HttpUtility.UrlDecode(HttpContext.Current.Request.Form["IsUpdateCheckPicUrl"]), 0);
            model.IsUpdateBusinessLicensePic = ParseHelper.ToInt(HttpUtility.UrlDecode(HttpContext.Current.Request.Form["IsUpdateBusinessLicensePic"]), 0);
            //照片有所更新 
            #region 更新商家头像
            ImageHelper ih = new ImageHelper();
            if (model.IsUpdateCheckPicUrl == 0)
            {
                var file = HttpContext.Current.Request.Files["CheckPicUrl"];
                if (file == null)
                {
                    return ResultModel<BusiModifyResultModelDM>.Conclude(UpdateBusinessInfoBReturnEnums.InvalidFileFormat);
                }
                ImgInfo imgInfo = ih.UploadImg(file, model.userId, ImageType.Business);
                if (!string.IsNullOrWhiteSpace(imgInfo.FailRemark))
                {
                    return ResultModel<BusiModifyResultModelDM>.Conclude(UpdateBusinessInfoBReturnEnums.UpFailed);
                }
                model.CheckPicUrl = imgInfo.PicUrl;

            }
            #endregion

            #region 更新商家执照

            if (model.IsUpdateBusinessLicensePic == 0)
            {
                var fileLicen = HttpContext.Current.Request.Files["BusinessLicensePic"];
                if (fileLicen == null)
                {
                    return ResultModel<BusiModifyResultModelDM>.Conclude(UpdateBusinessInfoBReturnEnums.InvalidFileFormat);
                }
                //执照
                ImgInfo imgInfoLicen = ih.UploadImg(fileLicen, model.userId, ImageType.Business);
                if (!string.IsNullOrWhiteSpace(imgInfoLicen.FailRemark))
                {
                    return ResultModel<BusiModifyResultModelDM>.Conclude(UpdateBusinessInfoBReturnEnums.UpFailed);
                }
                model.BusinessLicensePic = imgInfoLicen.PicUrl;
            }
            #endregion

            //修改商户地址信息，返回当前商户的状态
            return iBusinessProvider.UpdateBusinessInfoB(model);
        }

        /// <summary>
        /// 商家坐标上传              
        /// </summary>
        /// <UpdateBy>彭宜</UpdateBy>
        /// <UpdateTime>20150727</UpdateTime>
        /// <returns></returns>     
        [HttpPost]
        [Token]
        [ApiVersion]
        public ResultModel<object> PushLocaltion(BusinessPushLocaltionPM model)
        {
            return iBusinessProvider.InsertLocaltion(model);
        }
    }
}
