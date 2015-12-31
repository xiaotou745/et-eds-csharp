using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace PushServiceWCF
{
    [HubName("pushHub")]
    public class PushHubMoblie:Hub
    {
    }
}