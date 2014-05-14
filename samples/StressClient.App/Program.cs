using CommandLine;
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
            Options options = new Options();
            if (Parser.Default.ParseArgumentsStrict(args, options))
            {
                using (var stress = StressRun.Setup<EchoUser>(options.UsersLoad, options.Duration, options.Constant))
                {
                    Console.WriteLine("running with {0} virtual users ...", stress.VUsers.Count);
                    stress.Run();

                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("===== Statistics =====");
                    Console.WriteLine("Run done {0} ms", stress.Timer.ElapsedMilliseconds);

                    DumpStatistics("Received messages /user", stress.VUsers, u => u.ReceivedMessages);
                    DumpStatistics("Received bytes /user", stress.VUsers, u => u.ReceivedBytes);
                    DumpStatistics("Connection Duration", stress.VUsers, u => u.ConnectDuration ?? 0);

                    //buggy
                    //foreach (var msgId in Enumerable.Range(1,(int)stress.VUsers.Max(m=>m.ReceivedMessages)))
                    //{
                    //    var alltimings = stress.VUsers.Select(u=>u.MessagesTimings[msgId]);
                    //    Console.WriteLine("msgId {0} : {1}", msgId, alltimings.Max() - alltimings.Min());
                    //}

                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Press any key to close");
                Console.ReadKey();
            }
        }

        private static void DumpStatistics<T>(string title, IEnumerable<T> items, Func<T, double> selector)
        {
            Console.WriteLine(string.Format("{0} Avg. {1}, Min. {2}, Max. {3}", title, items.Average(selector), items.Min(selector), items.Max(selector)));
        }
    }
}
