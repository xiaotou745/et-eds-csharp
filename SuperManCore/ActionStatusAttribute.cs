using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCore
{
    public class ActionStatusAttribute : Attribute
    {
        private Type _type;
        public ActionStatusAttribute(Type status)
        {

        }
    }
}
