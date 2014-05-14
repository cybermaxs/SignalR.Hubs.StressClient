using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;
using StressClient.Core.User;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace StressClient.App
{
    public class GroupUser : BaseVirtualUser
    {
        public override void SetupHandlers(IHubProxy proxy)
        {
            proxy.Subscribe("send").Received += GroupUser_Received;
        }

        void GroupUser_Received(IList<JToken> obj)
        {
            //do somehting here
        }

        public override Task Initialize(IHubProxy proxy)
        {
            return proxy.Invoke("join", "testgroup");
        }

        public override string HubUrl { get { return "http://localhost:34196/"; } }

        public override string HubName { get { return "GroupHub"; } }

        public override string Transport
        {
            get { return string.Empty; }
        }

        public override Task Ping(IHubProxy proxy)
        {
            return Task.FromResult(true);
        }
    }
}
