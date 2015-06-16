﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace ETS.Compress
{
    public interface IImageFileTransformer
    {
        Size Transform(string srcPath, string destPath);
    }
}
