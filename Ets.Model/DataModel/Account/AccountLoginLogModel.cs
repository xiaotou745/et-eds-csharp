using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.DataModel.Account
{
    public class AccountLoginLogModel
    {
        //public int Id { get; set; }
        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; set; }
        public DateTime LoginTime { get; set; }
        public string Mac { get; set; }
        /// <summary>
        /// 登录类型0失败，1登录成功，2退出登录
        /// </summary>
        public int LoginType { get; set; }

        public string Ip { get; set; }
        /// <summary>
        /// 浏览器
        /// </summary>
        public string Browser { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
