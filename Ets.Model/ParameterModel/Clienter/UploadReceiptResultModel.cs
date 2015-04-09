using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Clienter
{
    public class UploadReceiptResultModel
    {
        public string OrderNo { get; set; }
        public string[] ImagePath { get; set; }
        public string Status { get; set; }
    }
}
