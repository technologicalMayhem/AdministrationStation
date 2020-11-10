using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using AdministrationStation.Communication.Models.Agent;
using Microsoft.Extensions.Logging;

namespace AS_Agent.Job
{
    public class StatusUpdate : IJob
    {
        public int JobId { get; set; }
        public DateTime RunNext { get; set; }

        private readonly ServerService _server;
        private readonly ILogger<StatusUpdate> _logger;

        public StatusUpdate(ServerService server, ILogger<StatusUpdate> logger)
        {
            _server = server;
            _logger = logger;
        }

        public async Task<JobResult> RunJob()
        {
            var rng = new Random();
            var status = new StatusUpdateModel
            {
                Hostname = Environment.MachineName,
                IpAddress = GetLocalIpAddress(),
                CpuUsage = rng.Next(101),
                MaximumRam = 8192,
                CurrentRam = rng.Next(8193)
            };
            await _server.ReportStatus(status);
            _logger.LogInformation($"Status update ran at: {DateTime.Now:g}");
            
            RunNext = DateTime.Now.AddSeconds(5);
            
            return JobResult.Success;
        }
        
        private static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}