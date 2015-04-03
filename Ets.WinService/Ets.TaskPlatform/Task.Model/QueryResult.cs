using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task.Model
{
    public class QueryResult
    {
        private bool _dealFlag = true;
        public bool DealFlag
        {
            get { return _dealFlag; }
            set { _dealFlag = value; }
        }
        public string ExceptionStr { get; set; }
        public int TotalCount { get; set; }
        
    }
}
