using Ets.Service.IProvider.User;
using Ets.Service.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.User
{
    public class UserOpeation
    {
        private string currentParentFileName = "UserProvider";
        /// <summary>
        /// 数据分发器
        /// </summary>
        /// <param name="platform">所属子类平台，可为空</param>
        /// <returns>寻找动态类</returns>
        public IUser FunctionName(string platform)
        {
            #region 子类
            dynamic extends = ClassInit(platform);
            if (extends != null)
            {
                return extends;
            }
            #endregion

            #region 父类
            return new UserProvider();
            #endregion
        }

        /// <summary>
        /// 初始化子类数据
        /// </summary>
        /// <param name="platform">所属子类平台，可为空</param>
        /// <returns></returns>
        public IUser ClassInit(string platform)
        {
            string className = string.Concat(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace, ".", currentParentFileName, platform);
            var abl = Assembly.Load("Ets.Service.Provider");//数据集
            return abl.CreateInstance(className) as IUser;//反射
        }
    }
}
