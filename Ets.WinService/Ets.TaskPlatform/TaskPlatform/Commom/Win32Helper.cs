using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TaskPlatform.Commom
{
    public class Win32Helper
    {
        [DllImport("psapi.dll")]
        public static extern int EmptyWorkingSet(IntPtr hwProc);
    }
}
