using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMan.Authority
{
    public interface IAuthenticationService
    {
        void SignIn(string data);
        void SignOut();
    }
}
