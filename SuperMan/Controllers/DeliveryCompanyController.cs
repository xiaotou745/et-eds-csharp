using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Ets.Model.Common;
using Ets.Model.DomainModel.DeliveryCompany;
using ETS.Util;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SuperMan.App_Start;

namespace SuperMan.Controllers
{

    /// <summary>
    /// 物流公司相关业务 
    /// </summary>
    [WebHandleError]
    public class DeliveryCompanyController : BaseController
    {

        public ActionResult DeliveryCompany()
        { 
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id

             
            ViewBag.deliveryCompanyList = new DeliveryCompanyProvider().Get();//获取物流公司
            var criteria = new Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria()
            {
                Status = -1,
                GroupId = SuperMan.App_Start.UserContext.Current.GroupId 

            };
            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return View();
            }
            return View();
            //ViewBag.openCityList.Result.AreaModels;
            //var pagedList = iDistributionProvider.GetClienteres(criteria);
            //return System.Web.UI.WebControls.View();
        }

        [HttpPost]
        public ActionResult PostDeliveryCompany(int pageindex = 1)
        {
            var criteria = new Ets.Model.ParameterModel.Clienter.ClienterSearchCriteria();
            TryUpdateModel(criteria);

            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id
             
            if (UserType > 0 && string.IsNullOrWhiteSpace(criteria.AuthorityCityNameListStr))
            {
                return PartialView("_DeliveryCompanyList");
            } 
            return PartialView("_DeliveryCompanyList", null);
        }


        /// <summary>
        /// 批量导入骑士页面 add by caoheyang 20150706
        /// </summary>
        /// <param name="companyId">公司id</param>
        /// <returns></returns>
        public ActionResult BatchImportClienter(int companyId)
        {
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
                    IList<BatchImportClienterExcelDM> models=ValatiteBatchImportClienterExcel(st, rowCount);
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
                if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(city) ||
                    string.IsNullOrEmpty(idCard))
                {
                    model.Remark = model.Remark + "信息不全。";
                    list.Add(model);
                    continue;
                }
                if (string.IsNullOrEmpty(phone)||!phoneReg.IsMatch(phone))//验证骑士手机号
                {
                    model.Remark = model.Remark + "手机号不合法。";
                }
                if (string.IsNullOrEmpty(idCard) || !idcardReg.IsMatch(idCard))//验证
                {
                    model.Remark = model.Remark + "身份证号不合法。";
                }
                if ((!string.IsNullOrWhiteSpace(model.Remark))
                    &&(list.Count(item => item.Phone == phone || item.IdCard == idCard) > 0))//验证xls中身份证号或者手机号是否重复
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
           return new JsonResult (){  Data="成功"};
       }
    }
}