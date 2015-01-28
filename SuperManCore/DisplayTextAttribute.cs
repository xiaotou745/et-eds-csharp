using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperManCore
{
    public class DisplayTextAttribute : Attribute
    {
        private string _displayText;

        public DisplayTextAttribute(string displayText)
        {
            _displayText = displayText;
        }

        public string DisplayText
        {
            get
            {
                return _displayText;
            }
        }

    }
}
