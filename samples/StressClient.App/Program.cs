using StressClient.Core;
using System;
using System.Linq;

namespace StressClient.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** Simple SignalR Hub Stress Client *** ");

            using (var stress = StressRun.Setup<EchoUser>(20, 10000, 0.1D))
            {
                Console.WriteLine("running with {0} virtual users", stress.VUsers.Count);
                stress.Run();

                Console.WriteLine("Run done {0} ms", stress.Timer.ElapsedMilliseconds);

                Console.WriteLine("Avg. received messages /user : {0}", stress.VUsers.Average(u => u.ReceivedMessages));
                Console.WriteLine("Avg. received bytes /user : {0}", stress.VUsers.Average(u => u.ReceivedBytes));
            }

            Console.WriteLine("Press any key to abort");
            Console.ReadKey();
        }
    }
}
