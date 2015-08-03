using Ets.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.Common
{
    public interface ITokenProvider
    {
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <UpdateBy>hulingbo</UpdateBy>
        /// <UpdateTime>20150731</UpdateTime>
        /// <param name="model"></param>
        /// <returns></returns>
        string GetToken(TokenModel model);
    }
}
