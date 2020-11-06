using System;

namespace AdministrationStation.Communication.Models.Agent
{
    public class AgentOptions
    {
        public DateTime LastUpdate { get; set; }
        
        public GeneralOptions General { get; set; } = new GeneralOptions();
        public JobOptions Job { get; set; } = new JobOptions();
    }
}