using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Ets.Model.Common;
using Ets.Model.DataModel.Finance;
using Ets.Model.DomainModel.Finance;
using Ets.Service.IProvider.Finance;
using Ets.Service.Provider.Finance;
using ETS.Util;
using Ets.Model.Common;
using ETS.Enums;
using Ets.Model.DomainModel.Bussiness;
using Ets.Model.ParameterModel.Order;
using Ets.Service.Provider.Finance;
using Ets.Service.IProvider.User;
using Ets.Service.Provider.User;
using SuperManWebApi.App_Start.Filters;
using Ets.Model.ParameterModel.Bussiness;
using Ets.Model.DataModel.Bussiness;
using SuperManWebApi.Providers;

namespace SuperManWebApi.Controllers
{
    /// <summary>
    /// 商户相关接口 add by caoheyang
    /// </summary>
    [ExecuteTimeLog]
    public class BusinessController : ApiController
    {
        private readonly IBusinessFinanceProvider _businessFinanceProvider = new BusinessFinanceProvider();
        private readonly IBusinessProvider _iBusinessProvider = new BusinessProvider();

        /// <summary>
        /// 商户交易流水API caoheyang 20150512
        /// </summary>
        /// <param name="model">查询参数实体</param>
        /// <returns></returns>
        [HttpPost]
        [Validate]
        [ApiVersion]
        public ResultModel<object> Records(BussinessRecordsPM model)
        {
            return _businessFinanceProvider.GetRecords(model.BusinessId);
        }

        /// <summary>
        /// 获取商户详情       
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150511</UpdateTime>
        /// <param name="model">商户参数</param>
        /// <returns></returns>        
        [HttpPost]
        public ResultModel<BusinessDM> Get(BussinessPM model)
        {
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
            if (!_iBusinessProvider.IsExist(model.BussinessId)) //商户不存在
            {
                return ResultModel<BusinessDM>.Conclude(GetBussinessStatus.FailedGet);
            }

            #endregion

            try
            {
                BusinessDM businessDM = _iBusinessProvider.GetDetails(model.BussinessId);
                //if (businessDM.Status == GetBussinessStatus.Audit.GetHashCode())//未审核
                //{
                //    return Ets.Model.Common.ResultModel<BusinessDM>.Conclude(GetBussinessStatus.Audit);
                //}
                //if (businessDM.Status == GetBussinessStatus.AuditAddress.GetHashCode())//未审核且未添加地址
                //{
                //    return Ets.Model.Common.ResultModel<BusinessDM>.Conclude(GetBussinessStatus.AuditAddress);
                //}
                //if (businessDM.Status == GetBussinessStatus.Auditing.GetHashCode())//审核中
                //{
                //    return Ets.Model.Common.ResultModel<BusinessDM>.Conclude(GetBussinessStatus.Auditing);
                //}
                //if (businessDM.Status == GetBussinessStatus.Refuse.GetHashCode())//被拒决
                //{
                //    return Ets.Model.Common.ResultModel<BusinessDM>.Conclude(GetBussinessStatus.Refuse);
                //}
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
        public ResultModel<BusinessInfo> GetDistribSubsidy(BussinessPM model)
        {
            #region 验证
            var version = model.Version;
            if (string.IsNullOrWhiteSpace(version)) //版本号 
            {
                return ResultModel<BusinessInfo>.Conclude(GetBussinessStatus.NoVersion);
            }
            if (model.BussinessId < 0)//商户Id不合法
            {
                return ResultModel<BusinessInfo>.Conclude(GetBussinessStatus.ErrNo);
            }
            if (!_iBusinessProvider.IsExist(model.BussinessId)) //商户不存在
            {
                return ResultModel<BusinessInfo>.Conclude(GetBussinessStatus.FailedGet);
            }

            #endregion

            try
            {
                BusinessInfo busInfo = _iBusinessProvider.GetDistribSubsidy(model.BussinessId);
                return Ets.Model.Common.ResultModel<BusinessInfo>.Conclude(GetBussinessStatus.Success, busInfo);
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("ResultModel<decimal> GetDistribSubsidy", new { obj = "时间：" + DateTime.Now.ToString() + ex.Message });
                return ResultModel<BusinessInfo>.Conclude(GetBussinessStatus.Failed);
            }
        }


        /// <summary>
        /// B端修改商户信息 caoheyang
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultModel<BusiModifyResultModelDM> UpdateBusinessInfoB()//BusiAddAddressInfoModel model
        {
            //model.CheckPicUrl = null;
            //model.BusinessLicensePic = null;  //防止移动端或者不安全情况下给该参数传了值
            //照片有所更新 
            //if (model.IsUpdateCheckPicUrl == 0)
            //{
            BusiAddAddressInfoModel model = new BusiAddAddressInfoModel();
            model.userId = ParseHelper.ToInt(HttpContext.Current.Request.Form["UserId"], 0);
            model.phoneNo = HttpContext.Current.Request.Form["phoneNo"];
            model.Address = HttpContext.Current.Request.Form["Address"];
            model.businessName = HttpContext.Current.Request.Form["businessName"];

            ImageHelper ih = new ImageHelper();
            if (HttpContext.Current.Request.Files["CheckPicUrl"] == null)
            {
                return ResultModel<BusiModifyResultModelDM>.Conclude(UpdateBusinessInfoBReturnEnums.InvalidFileFormat);
            }
            var file = HttpContext.Current.Request.Files["CheckPicUrl"];

            ImgInfo imgInfo = ih.UploadImg(file, 0);
            if (!string.IsNullOrWhiteSpace(imgInfo.FailRemark))
            {
                return ResultModel<BusiModifyResultModelDM>.Conclude(UpdateBusinessInfoBReturnEnums.UpFailed);
            }
            model.CheckPicUrl = imgInfo.PicUrl;
            //}

            //if (model.IsUpdateBusinessLicensePic == 0)
            //{
            if (HttpContext.Current.Request.Files["BusinessLicensePic"] == null)
            {
                return ResultModel<BusiModifyResultModelDM>.Conclude(UpdateBusinessInfoBReturnEnums.InvalidFileFormat);
            }
            var fileLicen = HttpContext.Current.Request.Files["BusinessLicensePic"];
            //ImageHelper ih = new ImageHelper();
            ImgInfo imgInfoLicen = ih.UploadImg(fileLicen, 0);
            if (!string.IsNullOrWhiteSpace(imgInfoLicen.FailRemark))
            {
                return ResultModel<BusiModifyResultModelDM>.Conclude(UpdateBusinessInfoBReturnEnums.UpFailed);
            }
            model.BusinessLicensePic = imgInfo.PicUrl;
            //}
            //修改商户地址信息，返回当前商户的状态
            return _iBusinessProvider.UpdateBusinessInfoB(model);
        }
    }
}
