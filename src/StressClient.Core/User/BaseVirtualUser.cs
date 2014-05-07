using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Transports;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace StressClient.Core.User
{
    public abstract class BaseVirtualUser : IVirtualUser
    {
        private HubConnection Connection { get; set; }
        private IHubProxy Proxy { get; set; }
        public async void Setup()
        {
            this.Connection = new HubConnection(this.HubUrl);
            this.Proxy = this.Connection.CreateHubProxy(this.HubName);

            this.SetupHandlers(this.Proxy);

            this.Connection.Received+=Connection_Received;

            //specifiy transport here
            await this.Connection.Start(this.CreateTranport(this.Transport));

            Trace.TraceInformation("New Connection Created :{0}", this.Connection.ConnectionId);

            this.Initialize();
        }

        void Connection_Received(string obj)
        {
            Interlocked.Increment(ref receivedMessages);
            Interlocked.Add(ref this.receivedBytes, obj.Length);
        }

        public abstract void SetupHandlers(IHubProxy proxy);

        public void Initialize()
        {
            this.Initialize(this.Proxy);
        }

        public abstract void Initialize(IHubProxy proxy);

        public void Ping()
        {
            this.Ping(this.Proxy);
        }

        public abstract void Ping(IHubProxy proxy);

        public void Stop()
        {
            if (this.Connection != null)
                this.Connection.Stop();
        }


        #region Private

        public enum TransportType
        {
            Auto,
            Websockets,
            ServerSentEvents,
            ForeverFrame,
            LongPolling,
        }

        private IClientTransport CreateTranport(string transportName)
        {
            TransportType transportType;
            if (!Enum.TryParse<TransportType>(transportName, true, out transportType))
            {
                // default it to Long Polling for transport
                transportType = TransportType.LongPolling;
            }

            var client = new DefaultHttpClient();
            switch (transportType)
            {
                case TransportType.Websockets:
                    return new WebSocketTransport(client);
                case TransportType.ServerSentEvents:
                    return new ServerSentEventsTransport(client);
                case TransportType.ForeverFrame:
                    break;
                case TransportType.LongPolling:
                    return new LongPollingTransport(client);
                default:
                    return new AutoTransport(client);
            }

            throw new NotSupportedException("Transport not supported");

        }
        #endregion

        private long receivedMessages = 0;
        private long receivedBytes = 0;
        protected void NotifyNewMessage(IList<JToken> obj)
        {
            Interlocked.Increment(ref receivedMessages);
        }

        public string ConnectionId { get { return this.Connection.ConnectionId ; } }
        public long ReceivedMessages { get { return receivedMessages; } }
        public long ReceivedBytes { get { return receivedBytes; } }
        public abstract string HubUrl {get;}
        public abstract string HubName { get; }
        public abstract string Transport { get; }
    }
}
