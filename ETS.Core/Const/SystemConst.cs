using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS.Const
{
    /// <summary>
    /// 系统级别常量 add by caoheyang  20150325
    /// </summary>
    public class SystemConst
    {      
        #region 分页相关常量 

        /// <summary>
        /// 默认页码
        /// </summary>
        public const int PageIndex = 1;

        /// <summary>
        /// 默认页容量
        /// </summary>
        public const int PageSize =15;

        #endregion

        public const string MessageBusiness = "E代送商家版";

        /// <summary>
        /// 代送外送员版
        /// </summary>
        public const string MessageClinenter = "E代送骑士版";



        public const string cookieName = "userinfo_edaisong";


        public const string menuListCookieName = "menulist";


        /// <summary>
        /// 城市尚未开放的信息 缓存用到 add by caoheyang 20150407
        /// </summary>
        public const string CityOpenInfo = "NotOpen";          

        public const string SMSSOURCE = "superManCheckCode";

        public const string OriginSize = "_0_0";

        public const string NoExportData = "<script>alert('您需要导出的数据为空，请重新选择筛选条件');window.history.go(-1);</script>";
    }
}
