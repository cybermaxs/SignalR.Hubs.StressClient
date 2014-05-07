using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace StressClient.DemoServer
{
    public class GroupHub : Hub
    {
        public Task Join(string group)
        {
            return Groups.Add(Context.ConnectionId, group);
        }

        public Task Send(string group, string message)
        {
            return Clients.Group(group).send(message);
        }

        public Task Leave(string group)
        {
            return Groups.Remove(Context.ConnectionId, group);
        }
    }
}