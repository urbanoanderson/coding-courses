using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace SignalRApp.Hubs
{
    public class ViewHub : Hub
    {
        private ILogger<ViewHub> logger;

        public ViewHub(ILogger<ViewHub> logger)
        {
            this.logger = logger;
        }

        public static int ViewCount { get; set; } = 0;

        public async override Task OnConnectedAsync()
        {
            ViewCount++;
            await Clients.All.SendAsync("viewCountUpdate", ViewCount);

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            ViewCount--;
            await Clients.All.SendAsync("viewCountUpdate", ViewCount);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SampleVoidMethod()
        {
            this.logger.LogInformation($"Calling {nameof(this.SampleVoidMethod)}");
            await Clients.All.SendAsync("sampleServerEvent");
        }

        public string SampleMethodThatTakesParameters(string par1, int par2)
        {
            return $"Hello {par1} - {par2}";
        }
    }
}