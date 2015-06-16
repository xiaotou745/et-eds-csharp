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
using Ets.Model.Common;
using Ets.Model.DataModel.Finance;
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
        public ResultModel<BankModel[]> Get(BankCriteria bankCriteria)
        {
            List<BankModel> listBank = new List<BankModel>(); 
            if (string.IsNullOrWhiteSpace(bankCriteria.Version))
            {
                return ResultModel<BankModel[]>.Conclude(BankStatus.NoVersion, listBank.ToArray());
            } 
            try
            {
                XElement root = XElement.Load(HttpContext.Current.Server.MapPath("~/bank.xml"));
                IEnumerable<XElement> listElement = root.Descendants("bank");
                foreach (XElement x in listElement)
                {
                    BankModel abank = new BankModel();
                    var id = x.Element("Id").Value;
                    var name = x.Element("Name").Value;
                    abank.Id = ParseHelper.ToInt(id, -1);
                    abank.Name = name;
                    listBank.Add(abank);
                }
                return ResultModel<BankModel[]>.Conclude(BankStatus.Success, listBank.ToArray());
            }
            catch (Exception ex)
            {
                LogHelper.LogWriter("载入银行出错：", new { obj = "bank.xml配置文件缺失"+ex.Message });
                return ResultModel<BankModel[]>.Conclude(BankStatus.NoXmlConfig, listBank.ToArray());
            }
        }
    }
}