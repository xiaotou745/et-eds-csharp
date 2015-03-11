using Ets.Dao.User;
using Ets.Service.IProvider;
using Ets.Service.IProvider.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.Provider.User
{
    public class UserProvider : IUser
    {
        public virtual List<int> Register(Model.UserModel user)
        {
            new UserDao().RegisterToSql(new Model.UserModel());
            return new List<int>() { 
                1,2
            };
        }
    }
}