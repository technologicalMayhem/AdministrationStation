using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AdministrationStation.Communication;
using AdministrationStation.Communication.Models.Agent;
using AdministrationStation.Communication.Models.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AS_Agent
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly WorkerConfiguration _configuration;
        private readonly ServerService _server;
        private readonly PerformanceCounter _cpuUsage;

        public Worker(ILogger<Worker> logger, WorkerConfiguration configuration, ServerService server)
        {
            _logger = logger;
            _configuration = configuration;
            _server = server;
            _cpuUsage = new PerformanceCounter();
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await ExecuteAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var rng = new Random();
            while (!stoppingToken.IsCancellationRequested)
            {
                var status = new StatusUpdateModel
                {
                    Hostname = Environment.MachineName,
                    IpAddress = GetLocalIpAddress(),
                    CpuUsage = rng.Next(101),
                    MaximumRam = 8192,
                    CurrentRam = rng.Next(8193)
                };
                await _server.ReportStatus(status);

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
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