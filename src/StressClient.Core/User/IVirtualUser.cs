using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace StressClient.Core.User
{
    public interface IVirtualUser
    {
        string HubUrl {get;}
        string HubName { get; }
        string Transport { get; }

        string ConnectionId { get; }
        long? ConnectDuration { get; }
        long ReceivedMessages { get; }
        long ReceivedBytes { get; }

        ConcurrentDictionary<long, double> MessagesTimings { get; }

        IRun CurrentRun { get; }

        /// <summary>
        /// Setup virtual user : create connection, hub with configured transport (default : LongPolling) 
        /// </summary>
        /// <param name="hubUrl"></param>
        /// <param name="hubName"></param>
        /// <param name="transport"></param>
        Task Setup(IRun run);

        /// <summary>
        /// Initialize virtual user. Allow to run somehting once connected (join group, ping, ...)
        /// </summary>
        Task Initialize();

        /// <summary>
        /// Ping event. Occurs sometimes.
        /// </summary>
        Task Ping();

        /// <summary>
        /// Close connection and mark user as inactive.
        /// </summary>
        void Stop();
    }
}
