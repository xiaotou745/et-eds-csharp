using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Ets.Model.Common;
using Ets.Model.DomainModel.Area;
using Ets.Model.DomainModel.DeliveryCompany;
using Ets.Service.IProvider.Common;
using Ets.Service.IProvider.DeliveryCompany;
using Ets.Service.Provider.Common;
using Ets.Service.Provider.DeliveryCompany;
using ETS.Util;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace SuperMan.Controllers
{

    /// <summary>
    /// 物流公司相关业务 
    /// </summary>
    public class DeliveryCompanyController : BaseController
    {

        private readonly IDeliveryCompanyProvider deliveryCompanyProvider = new DeliveryCompanyProvider();
        // GET: DeliveryCompany
        public ActionResult Index()
        {
            return View();
        }


        #region 物流公司批量导入骑士功能 add by caoheyang 20150706

        /// <summary>
        /// 批量导入骑士页面 add by caoheyang 20150706
        /// </summary>
        /// <param name="companyId">公司id</param>
        /// <param name="companyName">公司名称</param>
        /// <returns></returns>
        public ActionResult BatchImportClienter(int companyId, string companyName)
        {
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = companyName;
            return View();
        }

        /// <summary>
        /// 批量导入骑士  excel  处理  add by caoheyang 20150706
        /// </summary>
        /// <param name="companyId">公司id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchImportClienterExcel(int companyId)
        {
            if (Request.Files["file1"] != null && Request.Files["file1"].FileName != "")
            {
                Stream fs = null;
                IWorkbook wk = null;
                HttpPostedFileBase file = Request.Files["file1"];
                fs = file.InputStream;
                if (Path.GetExtension(Request.Files["file1"].FileName) == ".xls")
                {
                    wk = new HSSFWorkbook(fs);
                }
                else
                {
                    ViewBag.Message = "文件格式不正确,支持.xls文件！";
                    return PartialView();
                }
                ISheet st = wk.GetSheetAt(0);
                int rowCount = st.LastRowNum;
                if (rowCount > 100)
                {
                    ViewBag.Message = "每次最多导入100行数据！";
                    return PartialView();
                }
                else
                {
                    IList<BatchImportClienterExcelDM> models = ValatiteBatchImportClienterExcel(st, rowCount);
                    ViewBag.Datas = models;
                    return PartialView();
                }
            }
            ViewBag.Message = "请选择文件！";
            return PartialView();
        }


        /// <summary>
        /// 批量导入骑士验证excel内数据的合法性  add by caoheyang 20150706
        /// </summary>
        /// <param name="st"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        private List<BatchImportClienterExcelDM> ValatiteBatchImportClienterExcel(ISheet st, int rowCount)
        {
            List<BatchImportClienterExcelDM> list = new List<BatchImportClienterExcelDM>();
            for (int i = 1; i <= rowCount; i++)
            {
                string name = "", phone = "", city = "", idCard = "";
                if (st.GetRow(i) != null && st.GetRow(i).GetCell(0) != null) //用户名
                {
                    name = st.GetRow(i).GetCell(0).ToString();
                }
                if (st.GetRow(i) != null && st.GetRow(i).GetCell(1) != null) //手机号
                {
                    phone = st.GetRow(i).GetCell(1).ToString();
                }
                if (st.GetRow(i) != null && st.GetRow(i).GetCell(2) != null) //城市
                {
                    city = st.GetRow(i).GetCell(2).ToString();
                }
                if (st.GetRow(i) != null && st.GetRow(i).GetCell(3) != null) //身份证号
                {
                    idCard = st.GetRow(i).GetCell(3).ToString();
                }
                BatchImportClienterExcelDM model = new BatchImportClienterExcelDM();
                model.Phone = phone;
                model.Name = name;
                model.City = city;
                model.IdCard = idCard;
                Regex phoneReg = new Regex("^1\\d{10}$"); //手机号验证正则表达式
                Regex idcardReg = new Regex(@"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$");
                model.Remark = "";
                if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(city) ||
                    string.IsNullOrWhiteSpace(idCard))
                {
                    model.Remark = model.Remark + "信息不全。";
                    list.Add(model);
                    continue;
                }
                if (!phoneReg.IsMatch(phone))//验证骑士手机号
                {
                    model.Remark = model.Remark + "手机号不合法。";
                }
                if (!idcardReg.IsMatch(idCard))//身份证号验证
                {
                    model.Remark = model.Remark + "身份证号不合法。";
                }
                // 市编码
                IAreaProvider iAreaProvider = new AreaProvider();
                AreaModelTranslate areaModel =
                    iAreaProvider.GetNationalAreaInfo(new AreaModelTranslate()
                    {
                        Name = city.Trim(),
                        JiBie = 2
                    });
                if (areaModel == null)
                {
                    model.Remark = model.Remark + "城市信息不合法。";
                }
                else
                {
                    model.CityCode = areaModel.NationalCode.ToString();
                }

                if (string.IsNullOrWhiteSpace(model.Remark)
                    && (list.Count(item => item.Phone == phone) > 0))//验证xls中手机号是否重复
                {
                    model.Remark = model.Remark + "信息重复。";
                }
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 批量导入骑士  excel  处理  add by caoheyang 20150706
        /// </summary>
        /// <param name="companyId">公司id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DoBatchImportClienter(int companyId)
        {
            string jsondatas = Request.Form["datas"];  //得到页面上可导入的数据
            //序列化得到数据
            var models = JsonHelper.JsonConvertToObject<List<BatchImportClienterExcelDM>>(jsondatas);
            ResultModel<string> res = deliveryCompanyProvider.DoBatchImportClienter(companyId, models);
            return new JsonResult()
            {
                Data = res
            };
        } 
        #endregion
    }
}