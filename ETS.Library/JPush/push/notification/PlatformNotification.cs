using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ETS.Library.JPush.push.notification
{
    public abstract class PlatformNotification
    {
        
        public const String ALERT = "alert";
        private const String EXTRAS = "extras";
        [JsonProperty]
        public String alert{get;protected set;}
        [JsonProperty]
        public Dictionary<String, object> extras { get; protected set; }

        public PlatformNotification()
        {
            this.alert = null;
            this.extras = null;
        }
      

    }
}
