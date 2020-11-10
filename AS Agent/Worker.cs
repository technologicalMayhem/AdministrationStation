using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
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
                foreach (var job in _jobs.Where(job => job.RunNext < DateTime.Now))
                {
                    await job.RunJob();
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        private static int PrimaryScreenWidth => GetSystemMetrics(0);
        private static int PrimaryScreenHeight => GetSystemMetrics(1);

        private static byte[] TakeScreenshot()
        {
            using var bitmap = new Bitmap(PrimaryScreenWidth, PrimaryScreenHeight);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(0, 0, 0, 0,
                    bitmap.Size, CopyPixelOperation.SourceCopy);
            }
            using var memory = new MemoryStream();
            bitmap.Save(memory, ImageFormat.Png);

            return memory.ToArray();
        }
    }
}