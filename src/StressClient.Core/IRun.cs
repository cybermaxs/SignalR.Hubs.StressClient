using StressClient.Core.User;
using System;

namespace StressClient.Core
{
    public interface IRun : IDisposable
    {
        void Run();
    }
}
