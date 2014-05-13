using StressClient.Core;
using System;
using System.Collections.Generic;
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

                DumpStatistics("Received messages /user", stress.VUsers, u => u.ReceivedMessages);
                DumpStatistics("Received bytes /user", stress.VUsers, u => u.ReceivedBytes);
                DumpStatistics("Connection Duration", stress.VUsers, u => u.ConnectDuration??0);
            }

            Console.WriteLine("Press any key to abort");
            Console.ReadKey();
        }

        private static void DumpStatistics<T>(string title, IEnumerable<T> items, Func<T, double> selector)
        {
            Console.WriteLine(string.Format("{0} Avg. {1}, Min. {2}, Max. {3}", title, items.Average(selector), items.Min(selector), items.Max(selector)));
        }
    }
}
