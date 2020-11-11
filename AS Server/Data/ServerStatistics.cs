using System;
using System.Diagnostics;

namespace AdministrationStation.Server.Data
{
    public class ServerStatistics
    {
        public TimeSpan Uptime => _stopwatch.Elapsed;

        private readonly Stopwatch _stopwatch;
        
        public ServerStatistics()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }
    }
}