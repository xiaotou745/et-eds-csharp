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
using Ets.Model.ParameterModel.Bussiness;
using ETS.Enums;
using Ets.Service.Provider.User;
using Ets.Service.IProvider.User;
using ETS.Util; 
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;

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
            //是否存在该商户
            if (iBusiProvider.CheckExistBusiness(paramodel.fields.B_OriginalBusiId, paramodel.group))
                return ResultModel<object>.Conclude(CustomerRegisterStatus.OriginalBusiIdRepeat);

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

            string addResult= iBusiProvider.AddThirdBusiness(paramodel);
            if (addResult == "1")
            {
                return ResultModel<object>.Conclude(CustomerRegisterStatus.Success );
            }
            else
            {
                return ResultModel<object>.Conclude(CustomerRegisterStatus.Faild);
            }
             
        }
    }
}