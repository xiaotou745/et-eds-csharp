using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using SuperManWebApi.Models;
using SuperManWebApi.Providers;
using SuperManWebApi.Results;
using ETS.Util;
using System.Xml;
using System.Xml.Linq;
using ETS.Enums;
using Ets.Model.ParameterModel.Finance;

namespace SuperManWebApi.Controllers
{
    public class BankController : ApiController
    {
        /// <summary>
        /// 获取银行列表
        /// wc
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Ets.Model.Common.ResultModel<Ets.Model.DataModel.Finance.BankModel[]> Get(BankCriteria bankCriteria)
        {
            List<Ets.Model.DataModel.Finance.BankModel> listBank = new List<Ets.Model.DataModel.Finance.BankModel>(); 
            if (string.IsNullOrWhiteSpace(bankCriteria.Version))
            {
                return Ets.Model.Common.ResultModel<Ets.Model.DataModel.Finance.BankModel[]>.Conclude(BankStatus.NoVersion, listBank.ToArray());
            } 
            try
            {
                XElement root = XElement.Load(HttpContext.Current.Server.MapPath("~/bank.xml"));
                IEnumerable<XElement> listElement = root.Descendants("bank");
                foreach (XElement x in listElement)
                {
                    Ets.Model.DataModel.Finance.BankModel abank = new Ets.Model.DataModel.Finance.BankModel();
                    var id = x.Element("Id").Value;
                    var name = x.Element("Name").Value;
                    abank.Id = ParseHelper.ToInt(id, -1);
                    abank.Name = name;
                    listBank.Add(abank);
                }
                return Ets.Model.Common.ResultModel<Ets.Model.DataModel.Finance.BankModel[]>.Conclude(BankStatus.Success, listBank.ToArray());
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("载入银行出错：", new { obj = "bank.xml配置文件缺失"+ex.Message });
                return Ets.Model.Common.ResultModel<Ets.Model.DataModel.Finance.BankModel[]>.Conclude(BankStatus.NoXmlConfig, listBank.ToArray());
            }
        }
    }
}