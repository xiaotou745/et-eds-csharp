using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ets.Model.Common;
using Ets.Model.ParameterModel.User;
using Ets.Service.Provider.Common;
using SuperMan.App_Start;

namespace SuperMan.Controllers
{
    public class TestUserManagerController : BaseController
    {
        TestUserProvider provider = new TestUserProvider();

        // GET: TestUserManager
        public ActionResult TestUserManager()
        {
            var list = provider.GetTestUserList();
            return View(list);
        }
       
        /// <summary>
        /// 添加测试账号
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddTestUser(string phoneNo)
        {
            var model = new UserOptRecordPara
            {
                UserID = 0,
                UserType = 0,
                OptUserId = UserContext.Current.Id,
                OptUserName = UserContext.Current.Name,
                Remark = "测试账号管理-添加测试手机号:" + phoneNo,
                OptUserType = 0,
                OptType = 1
            };
            bool b = provider.AddTestUser(phoneNo, model);
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除骑士账户
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteTestClienter(string phoneNo)
        {
            var model = new UserOptRecordPara
            {
                UserID = 0,
                UserType = 0,
                OptUserId = UserContext.Current.Id,
                OptUserName = UserContext.Current.Name,
                Remark = "测试账号管理-删除骑士账户:" + phoneNo,
                OptUserType = 0,
                OptType = 1
            };
            bool b = provider.DeleteTestClienter(phoneNo, model);
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteTestOrder(string phoneNo)
        {
            var model = new UserOptRecordPara
            {
                UserID = 0,
                UserType = 0,
                OptUserId = UserContext.Current.Id,
                OptUserName = UserContext.Current.Name,
                Remark = "测试账号管理-删除订单:" + phoneNo,
                OptUserType = 0,
                OptType = 1
            };
            bool b = provider.DeleteTestOrder(phoneNo, model);
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除商户
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteTestBusiness(string phoneNo)
        {
            var model = new UserOptRecordPara
            {
                UserID = 0,
                UserType = 0,
                OptUserId = UserContext.Current.Id,
                OptUserName = UserContext.Current.Name,
                Remark = "测试账号管理-删除商户:" + phoneNo,
                OptUserType = 0,
                OptType = 1
            };
            bool b = provider.DeleteTestBusiness(phoneNo, model);
            return Json(new ResultModel(b, string.Empty), JsonRequestBehavior.AllowGet);
        }
    }
}