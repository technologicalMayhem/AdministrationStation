using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using AdministrationStation.Communication.Models.Agent;
using AS_Agent.Job;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AS_Agent
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ConfigurationProvider _options;
        private readonly ServerService _server;
        private readonly PerformanceCounter _cpuUsage;
        private readonly IServiceProvider _provider;

        private readonly List<IJob> _jobs;

        public Worker(ILogger<Worker> logger, ConfigurationProvider options, ServerService server, IServiceProvider provider)
        {
            _logger = logger;
            _options = options;
            _server = server;
            _provider = provider;
            _cpuUsage = new PerformanceCounter();
            _jobs = new List<IJob>();
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _jobs.AddRange(new IJob[]
            {
                ActivatorUtilities.CreateInstance<StatusUpdate>(_provider),
                ActivatorUtilities.CreateInstance<ConfigurationUpdate>(_provider)
            });
            await ExecuteAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var rng = new Random();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    foreach (var job in _jobs.Where(job => job.RunNext < DateTime.Now))
                    {
                        await job.RunJob();
                    }
                }
                catch (HttpRequestException e)
                {
                    _logger.LogError(e.Message);
                    DiagnoseNetworkProblems();
                    await HandleServerOutage(stoppingToken);
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

        private void DiagnoseNetworkProblems()
        {
            
        }

        private async Task HandleServerOutage(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(30) ,cancellationToken);
                try
                {
                    await _server.GetServerState();
                }
                catch (HttpRequestException e)
                {
                    _logger.LogInformation($"Connection attempt to the server failed. {e.Message}");
                    continue;
                }
                _logger.LogInformation("Connection to the server established.");
                return;
            }
        }
    }
}