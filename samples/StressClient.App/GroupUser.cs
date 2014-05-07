using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;
using StressClient.Core.User;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace StressClient.App
{
    public class GroupUser : BaseVirtualUser
    {
        public long ReceivedMessages;

        public override void SetupHandlers(IHubProxy proxy)
        {
            proxy.Subscribe("send").Received += GroupUser_Received;
        }

        void GroupUser_Received(IList<JToken> obj)
        {
            //this.NotifyNewMessage(obj);
            //Trace.WriteLine(string.Format("message received {0}", obj));
        }

        public override void Initialize(IHubProxy proxy)
        {
            proxy.Invoke("join", "testgroup");
        }

        public override string HubUrl { get { return "http://localhost:34196/"; } }

        public override string HubName { get { return "GroupHub"; } }

        public override string Transport
        {
            get { return string.Empty; }
        }

        public override void Ping(IHubProxy proxy)
        {
            //Nothing
        }
    }
}
