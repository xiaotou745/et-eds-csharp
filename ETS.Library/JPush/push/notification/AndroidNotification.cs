using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ETS.Library.JPush.push.notification
{
   public class AndroidNotification:ETS.Library.JPush.push.notification.PlatformNotification
    {
        public const String NOTIFICATION_ANDROID = "android";
    
        private const String TITLE = "title";
        private const String BUILDER_ID = "builder_id";

        [JsonProperty]
        public String title{get;private set;}
        [JsonProperty]
        public int builder_id { get; private set; }
        public AndroidNotification():base()
        {
            this.title = null;
            this.builder_id = 0;
        }
        public AndroidNotification setTitle(string title)
        {
            this.title = title;
            return this;
        }
        public AndroidNotification setBuilderID(int builder_id)
        {
            this.builder_id = builder_id;
            return this;
        }
        public AndroidNotification setAlert(String alert)
        {
            this.alert = alert;
            return this;
        }
        public AndroidNotification AddExtra(string key, string value)
        {
            if (extras == null)
            {
                extras = new Dictionary<string, object>();
            }
            if (value != null)
            {
                extras.Add(key, value);
            }
            return this;
        }
        public AndroidNotification AddExtra(string key, int value)
        {
            if (extras == null)
            {
                extras = new Dictionary<string, object>();
            }
            extras.Add(key, value);
            return this;
        }
        public AndroidNotification AddExtra(string key, bool value)
        {
            if (extras == null)
            {
                extras = new Dictionary<string, object>();
            }
            extras.Add(key, value);
            return this;

        }
       
        
    }
}
