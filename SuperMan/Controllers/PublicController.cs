
﻿using System.Collections.Generic;
﻿using System.Linq;
﻿using System.Text;
﻿using System.Threading.Tasks;
﻿using System.Web.Mvc;
﻿using ETS.Data.PageData;
﻿using Ets.Model.DataModel.Order;
﻿using Ets.Model.DomainModel.Business;
﻿using Ets.Service.IProvider.AuthorityMenu;
﻿using Ets.Service.Provider.Authority;
using Ets.Service.Provider.Distribution;
using Ets.Service.Provider.Order;
using Ets.Service.IProvider.Common;
using Ets.Service.Provider.Common;
﻿using SuperMan.App_Start;
using Ets.Model.ParameterModel.User;
using Ets.Model.ParameterModel.Order;
using ETS.Util;
using Ets.Model.Common;
﻿using Ets.Model.DomainModel.Order;
﻿using Ets.Service.Provider.Clienter;
using ETS.Enums;
using Ets.Service.Provider.Business;
using Ets.Service.IProvider.Business;
﻿using ETS.Const;
﻿using SuperMan.Common;
using Ets.Model.DomainModel.Area;
namespace SuperMan.Controllers
{
    public class PublicController : BaseController
    {
        IAreaProvider iAreaProvider = new AreaProvider();
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取城市列表 仿google下拉列表框
        /// 胡灵波
        /// 2015年8月31日 10:40:51
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public ContentResult GetCity(string cityName)
        {
            int UserType = UserContext.Current.AccountType == 1 ? 0 : UserContext.Current.Id;//如果管理后台的类型是所有权限就传0，否则传管理后台id

            string cityNameZ = Server.UrlDecode(cityName);
            IList<AreaModel> aMoldeList = iAreaProvider.GetOpenCity(ParseHelper.ToInt(UserType)).AreaModels.Where(p => p.Name.Contains(cityNameZ)).ToList();
            string callback = "{\"citylist\":[";
            for (int i = 0; i < aMoldeList.Count; i++)
            {
                if (i == aMoldeList.Count - 1)
                {
                    callback += "{\"id\":" + i + ",\"city\":\"" + aMoldeList[i].Name + "\"}";
                }
                else
                {
                    callback += "{\"id\":" + i + ",\"city\":\"" + aMoldeList[i].Name + "\"},";
                }
            }
            callback += "]}";

            return Content(callback);
        } 
    }
}