using Ets.Service.IProvider.Order;
using Ets.Service.Provider.Order;
using Ets.Service.IProvider.Clienter;
using Ets.Service.Provider.Clienter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using Ets.Model.Common;
using Ets.Model.ParameterModel.Business;
using ETS.Enums;
using Ets.Service.Provider.User;
using Ets.Service.IProvider.User;
using ETS.Util;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using ETS.Security;
using Ets.Model.DataModel.Business;
using Ets.Service.IProvider.Business;
using Ets.Service.Provider.Business;
namespace OpenApi.Controllers
{
    public class BusinessController : ApiController
    {
        IBusinessProvider iBusiProvider = new BusinessProvider();
        IAreaProvider iAreaProvider = new AreaProvider();
        /// <summary>
        /// 商户注册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SignOpenApi]
        [OpenApiActionError]
        public ResultModel<object> RegisterBusiness(ParaModel<BusinessRegisterModel> paramodel)
        {
            LogHelper.LogWriter("商户注册：", new { Busi = paramodel });
            if (string.IsNullOrWhiteSpace(paramodel.fields.PhoneNo))   //手机号非空验证
                return ResultModel<object>.Conclude(CustomerRegisterStatus.PhoneNumberEmpty); 
            if (string.IsNullOrWhiteSpace(paramodel.fields.B_OriginalBusiId.ToString()))  //判断原平台商户Id不能为空
                return ResultModel<object>.Conclude(CustomerRegisterStatus.OriginalBusiIdEmpty); 
            if (string.IsNullOrWhiteSpace(paramodel.fields.B_City) || string.IsNullOrWhiteSpace(paramodel.fields.B_CityCode.ToString())) //城市以及城市编码非空验证
                return ResultModel<object>.Conclude(CustomerRegisterStatus.cityIdEmpty);
            if (string.IsNullOrEmpty(paramodel.fields.B_Name.Trim())) //商户名称
                return ResultModel<object>.Conclude(CustomerRegisterStatus.BusiNameEmpty);
            if (string.IsNullOrWhiteSpace(paramodel.fields.Address) || string.IsNullOrWhiteSpace(paramodel.fields.B_Province) || string.IsNullOrWhiteSpace(paramodel.fields.B_City) || string.IsNullOrWhiteSpace(paramodel.fields.B_Area) || string.IsNullOrWhiteSpace(paramodel.fields.B_AreaCode) || string.IsNullOrWhiteSpace(paramodel.fields.B_CityCode) || string.IsNullOrWhiteSpace(paramodel.fields.B_ProvinceCode))  //商户地址 省市区 不能为空
                return ResultModel<object>.Conclude(CustomerRegisterStatus.BusiAddressEmpty);
            
            BusinessModel busi = iBusiProvider.CheckExistBusiness(paramodel.fields.B_OriginalBusiId, paramodel.group);
            if (busi != null)
            {
                //if(busi.Status == ConstValues.BUSINESS_NOADDRESS || busi.Status == ConstValues.BUSINESS_AUDITCANCEL )
                //{
                //   bool upresult = iBusiProvider.UpdateAuditStatus(busi.Id, ConstValues.BUSINESS_NOAUDIT);
                //    if (upresult)
                //    {
                //        return ResultModel<object>.Conclude(CustomerRegisterStatus.Success, busi.Id);
                //    }
                //    else
                //    {
                //        return ResultModel<object>.Conclude(CustomerRegisterStatus.Faild);
                //    }
                //}
                //else
                //{
                //    return ResultModel<object>.Conclude(CustomerRegisterStatus.Success, busi.Id);
                //}
                //if (busi.Status != ConstValues.BUSINESS_NOADDRESS && busi.Status == ConstValues.BUSINESS_AUDITCANCEL)
                //{
                LogHelper.LogWriter("商户注册---：", new { busid = busi.Id,BusiStatus=BusiStatus.BusiNoPass.GetHashCode() });

                bool upresult = iBusiProvider.UpdateAuditStatus(busi.Id, BusiStatus.BusiNoPass.GetHashCode(), paramodel.fields.Address);
                //}
                if (upresult)
                {
                    return ResultModel<object>.Conclude(CustomerRegisterStatus.Success, busi.Id);
                }
                else
                {
                    return ResultModel<object>.Conclude(CustomerRegisterStatus.Faild);
                }
            }
            //if (iBusiProvider.CheckExistBusiness(paramodel.fields.B_OriginalBusiId, paramodel.group))
            //    return ResultModel<object>.Conclude(CustomerRegisterStatus.OriginalBusiIdRepeat);

            paramodel.fields.B_Password = MD5Helper.MD5(string.IsNullOrEmpty(paramodel.fields.B_Password) ? "abc123" : paramodel.fields.B_Password);
            #region 转换省市区
            //转换省
            var _province = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModelTranslate() { Name = paramodel.fields.B_Province, JiBie = 1 });
            if (_province != null)
            {
                paramodel.fields.B_ProvinceCode = _province.NationalCode.ToString();
            }
            //转换市 
            var _city = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModelTranslate() { Name = paramodel.fields.B_City, JiBie = 2 });
            if (_city != null)
            {
                paramodel.fields.B_CityCode = _city.NationalCode.ToString();
            }
            //转换区
            var _area = iAreaProvider.GetNationalAreaInfo(new Ets.Model.DomainModel.Area.AreaModelTranslate() { Name = paramodel.fields.B_Area, JiBie = 3 });
            if (_area != null)
            {
                paramodel.fields.B_AreaCode = _area.NationalCode.ToString();
            }
            #endregion

            int addResult = iBusiProvider.AddThirdBusiness(paramodel);
            if (addResult > 0)
            {
                return ResultModel<object>.Conclude(CustomerRegisterStatus.Success, addResult);
            }
            else
            {
                return ResultModel<object>.Conclude(CustomerRegisterStatus.Faild);
            }
        }


        /// <summary>
        /// 获取商户状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SignOpenApi]
        [OpenApiActionError]
        public ResultModel<object> GetBusinessStatus(ParaModel<BusinessModeltemp> paramodel)
        {
            LogHelper.LogWriter("获取商户状态：", new { Busi = paramodel });
            var busi = iBusiProvider.GetBusiness(paramodel.fields.B_OriginalBusiId, paramodel.group);

            if (busi == null)
            {
                return ResultModel<object>.Conclude(BusiStatus.BusiNoRegiste);
            }
            else if (busi.Status == (byte)BusinessStatus.Status1.GetHashCode()) //1
            {
                return ResultModel<object>.Conclude(BusiStatus.BusiPass);
            }
            else if (busi.Status == BusinessStatus.Status4.GetHashCode())//审核被拒绝
            {
                return ResultModel<object>.Conclude(BusiStatus.BusiReject);
            }
            else
            {
                return ResultModel<object>.Conclude(BusiStatus.BusiPassing);
            }          

        }
    }
}