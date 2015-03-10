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
    public class UserProvidermeizhoudongpo : UserProvider, IUserhaidilao
    {
        public override List<int> Register(Ets.Model.UserModel user)
        {
            ////throw new NotImplementedException();
            //UserDao dao = new UserDao();
            //dao.RegisterToSql(new Model.UserModel());
            return new List<int>() { 5, 6 };
        }
    }
}
