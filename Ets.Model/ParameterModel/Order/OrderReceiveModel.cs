﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ets.Model.ParameterModel.Order
{
    public class OrderReceiveModel
    {
        public int userId { get; set; }

        public int businessId { get; set; }

        public string orderNo { get; set; }

        public string version { get; set; }

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        public int orderId { get; set; }


    }
}
