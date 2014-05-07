using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;
using StressClient.Core.User;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace StressClient.App
{
    public class EchoUser : BaseVirtualUser
    {
        public long ReceivedMessages;

        public override void SetupHandlers(IHubProxy proxy)
        {
            proxy.Subscribe("send").Received += EchoUser_Received;
        }

        void EchoUser_Received(IList<JToken> obj)
        {
            //this.NotifyNewMessage(obj);
            //Trace.WriteLine("message received");
        }

        public override void Initialize(IHubProxy proxy)
        {
            proxy.Invoke("send", "fake hello");
        }

        public override string HubUrl { get { return "http://localhost:34196/"; } }

        public override string HubName { get { return "EchoHub"; } }

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
