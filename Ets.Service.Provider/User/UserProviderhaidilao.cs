using Ets.Service.IProvider;
using Ets.Service.IProvider.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.User
{
    public class UserProviderhaidilao : UserProvider, IUserhaidilao
    {
        public override List<int> Register(Ets.Model.UserModel user)
        {
            return new List<int>() { 3,4};
        }
    }
}
