using Ets.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Service.IProvider.User
{
    public interface IUser
    {
        List<int> Register(UserModel user);

    }
}


