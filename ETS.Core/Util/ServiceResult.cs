
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ETS.Util
{
    public class ServiceResult
    {
        // Methods
        public ServiceResult()
        {
            this.Errors = new List<string>();
        }

        public void AddErrorCode(string errorCode)
        {
            this.Errors.Add(errorCode);
        }

        // Properties
        public IList<string> Errors { get; set; }

        public object Result { get; set; }

        public bool Success
        {
            get
            {
                return (this.Errors.Count == 0);
            }
        }
    } 
}