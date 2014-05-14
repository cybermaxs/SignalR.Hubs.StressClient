using StressClient.Core.User;
using System;

namespace StressClient.Core
{
    public interface IRun : IDisposable
    {
        void Run();

        bool IsRunning { get; }

        TimeSpan Elapsed { get; }
    }
}
