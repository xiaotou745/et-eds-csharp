﻿using System;
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
        public const string cookieName = "userinfo_edaisong";

        #region 分页相关常量 add by caoheyang  20150325

        /// <summary>
        /// 默认页码
        /// </summary>
        public const int PageIndex = 1;

        /// <summary>
        /// 默认页容量
        /// </summary>
        public const int PageSize =15;

        #endregion

        /// <summary>
        /// 城市尚未开放的信息 缓存用到 add by caoheyang 20150407
        /// </summary>
        public const string CityOpenInfo = "NotOpen";

    }
}
