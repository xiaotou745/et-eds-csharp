using System.Collections.Generic;
using Ets.Dao.Common;
using Ets.Dao.User;
using Ets.Model.Common;
using Ets.Model.ParameterModel.User;

namespace Ets.Service.Provider.Common
{
    public class TestUserProvider
    {
        TestUserDao dao=new TestUserDao();
        UserOptRecordDao logdao = new UserOptRecordDao();
        /// <summary>
        /// 添加测试账号
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public bool AddTestUser(string phoneNo, UserOptRecordPara model)
        {
            logdao.InsertUserOptRecord(model); 
            return dao.AddTestUser(phoneNo);
        }
        /// <summary>
        /// 获取测试账号
        /// </summary>
        /// <returns></returns>
        public IList<TestUserModel> GetTestUserList()
        {
            return dao.GetTestUserList();
        }

        /// <summary>
        /// 删除测试账号
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public bool DelTestUser(string phoneNo)
        {
            return dao.DelTestUser(phoneNo);
        }

        /// <summary>
        /// 删除测试骑士
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public bool DeleteTestClienter(string phoneNo,UserOptRecordPara model)
        { 
            logdao.InsertUserOptRecord(model); 
            return dao.DeleteTestClienter(phoneNo);
        }


        /// <summary>
        /// 删除测试订单
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public bool DeleteTestOrder(string phoneNo, UserOptRecordPara model)
        {
            logdao.InsertUserOptRecord(model); 
            return dao.DeleteTestOrder(phoneNo);
        } 

        /// <summary>
        /// 删除测试商家,先删除订单数据
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public bool DeleteTestBusiness(string phoneNo, UserOptRecordPara model)
        {
            logdao.InsertUserOptRecord(model); 
            return  dao.DeleteTestBusiness(phoneNo);
        }
    }
}
