using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AS_Agent.Job
{
    public class ConfigurationUpdate : IJob
    {
        public int JobId { get; set; }
        public DateTime RunNext { get; set; }

        private readonly ServerService _service;
        private readonly ConfigurationProvider _provider;
        private readonly ILogger<ConfigurationUpdate> _logger;

        public ConfigurationUpdate(ServerService service, ConfigurationProvider provider, ILogger<ConfigurationUpdate> logger)
        {
            _service = service;
            _provider = provider;
            _logger = logger;
        }

        public async Task<JobResult> RunJob()
        {
            _provider.AgentOptions = await _service.UpdateOptions(_provider.AgentOptions);
            _logger.LogInformation($"Configuration update ran at: {DateTime.Now:g}");
            
            RunNext = DateTime.Now.AddMinutes(5);
            
            return JobResult.Success;
        }
    }
}