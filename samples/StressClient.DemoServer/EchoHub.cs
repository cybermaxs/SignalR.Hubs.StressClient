using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace StressClient.DemoServer
{
    public class EchoHub : Hub
    {
        public Task Echo(string message)
        {
            return Clients.Caller.echo(message);
        }

        public Task Send(string message)
        {
            return Clients.All.send(message);
        }
    }
}