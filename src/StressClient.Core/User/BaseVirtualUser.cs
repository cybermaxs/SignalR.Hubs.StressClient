using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Transports;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace StressClient.Core.User
{
    public abstract class BaseVirtualUser : IVirtualUser
    {
        private HubConnection Connection { get; set; }
        private IHubProxy Proxy { get; set; }

        public async Task Setup(IRun run)
        {
            this.CurrentRun = run;
            this.Connection = new HubConnection(this.HubUrl);
            this.Proxy = this.Connection.CreateHubProxy(this.HubName);

            this.SetupHandlers(this.Proxy);

            this.MessagesTimings = new ConcurrentDictionary<long, double>();
            this.Connection.Received += Connection_Received;

            var watcher = Stopwatch.StartNew();
            await this.Connection.Start(this.CreateTranport(this.Transport));
            watcher.Stop();
            this.ConnectDuration = watcher.ElapsedMilliseconds;

            Trace.TraceInformation("New Connection Created :{0}", this.Connection.ConnectionId);

            await this.Initialize();
        }

        void Connection_Received(string obj)
        {
            var msgId = Interlocked.Increment(ref receivedMessages);
            Interlocked.Add(ref this.receivedBytes, obj.Length);

            //buggy
            //this.MessagesTimings.TryAdd(msgId, this.CurrentRun.Elapsed.TotalMilliseconds);
        }

        public abstract void SetupHandlers(IHubProxy proxy);

        public Task Initialize()
        {
            return this.Initialize(this.Proxy);
        }

        public abstract Task Initialize(IHubProxy proxy);

        public Task Ping()
        {
            return this.Ping(this.Proxy);
        }

        public abstract Task Ping(IHubProxy proxy);

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
                transportType = TransportType.ServerSentEvents;
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

        public string ConnectionId { get { return this.Connection.ConnectionId; } }
        public long? ConnectDuration { get; private set; }
        public long ReceivedMessages { get { return receivedMessages; } }
        public long ReceivedBytes { get { return receivedBytes; } }
        public abstract string HubUrl { get; }
        public abstract string HubName { get; }
        public abstract string Transport { get; }

        public IRun CurrentRun { get; private set; }

        public ConcurrentDictionary<long, double> MessagesTimings { get; private set; }

    }
}
