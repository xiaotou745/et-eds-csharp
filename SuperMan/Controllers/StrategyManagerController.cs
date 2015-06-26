using System;
using System.Web.Mvc;
using Ets.Model.Common;
using Ets.Model.DataModel.Group;
using Ets.Model.ParameterModel.Group;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
using SuperMan.App_Start;
using Ets.Model.DomainModel.Group;
using Ets.Service.IProvider.Strategy;
using Ets.Model.DataModel.Strategy;
namespace SuperMan.Controllers
{
    public class StrategyManagerController : BaseController
    {
        //胡灵波

        readonly IStrategyProvider iStrategyProvider = new StrategyProvider();
    
        readonly IGroupProvider iGroupServices = new GroupProvider();

      
        /// <summary>
        /// 补贴策略管理首页
        /// </summary>
        /// <returns></returns>
        public ActionResult StrategyManager()
        {          
           var pagedList= iStrategyProvider.GetStrategyList();
           return View(pagedList);
        }
        /// <summary>
        /// 添加补贴策略信息
        /// </summary>
        /// <param name="groupname"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddStrategy(string name, string strategyid)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(new ResultModel(false, "策略名称不能为空"));
            }
            var mode = new StrategyModel { Name = name.Trim(), StrategyId = Convert.ToInt32(strategyid) };
            var result = iStrategyProvider.HasExistsStrategy(mode);
            if (result.Result)
            {
                if (result.Data)
                {
                    return Json(new ResultModel(false, "策略名称已经存在"));
                }
            }
            else
            {
                return Json(new ResultModel(false, "服务器异常"));
            }          
            var res = iStrategyProvider.AddStrategy(mode);
            return Json(res.Result ? new ResultModel(true, "成功") : new ResultModel(false, "服务器异常"));
        }

        /// <summary>
        /// 查询补贴策略信息
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostGroupManager(int pageindex = 1)
        {
            GroupParaModel criteria = new GroupParaModel();
            TryUpdateModel(criteria);
            var pagedList = iGroupServices.GetGroupList(criteria);

            return PartialView("_GroupManageList", pagedList);
        }

        
        /// <summary>
        /// 修改策略信息
        /// </summary>
        /// <param name="groupname"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateStrategy(int id, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(new ResultModel(false, "策略名称不能为空"));
            }        
            var mode = new StrategyModel { Id=id, Name = name.Trim() };
            var result = iStrategyProvider.HasExistsStrategy(mode);         
            if (result.Result)
            {
                if (result.Data)
                {
                    return Json(new ResultModel(false, "策略名称已经存在"));
                }
            }
            else
            {
                return Json(new ResultModel(false, "服务器异常"));
            }
            var res = iStrategyProvider.UpdateStrategyName(mode);
            return Json(res.Result ? new ResultModel(true, "成功") : new ResultModel(false, "服务器异常"));
        }          

    }
}