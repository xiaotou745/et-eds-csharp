using SuperManDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperManCommonModel.Models;
using SuperManCore;
using SuperManCommonModel.Entities;
using SuperManWebApi.Models.Business;
using SuperManCore.Common;
using SuperManCommonModel;
using SuperManBusinessLogic.CommonLogic;

namespace SuperManBusinessLogic.Group_Logic
{
 
    /// <summary>
    /// 集团业务逻辑 add by caoheyang 20150212
    /// </summary>
    public class GroupLogic
    {
        private volatile static GroupLogic _instance = null;
        private static readonly object lockHelper = new object();
        private GroupLogic() { }
        public static GroupLogic groupLogic()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new GroupLogic();
                }
            }
            return _instance;
        }

         /// <summary>
        /// 根据集团id获取集团名称，如果当前集团状态不可用则返回为空  add by caoheyang 20150212
        /// </summary>
        /// <param name="groupId">集团id</param>
        /// <returns></returns>
        public string GetGroupName(int groupId)
        {
            using (var db = new supermanEntities())
            {
                group groupmodel = db.group.Where(p => p.Id == groupId && p.IsValid == ConstValues.GroupIsIsValid).FirstOrDefault();
                if (groupmodel == null)
                    return null;
                else
                    return groupmodel.GroupName;

            }
        }
    }
}
