using System;
using AdministrationStation.Communication.Models.Agent;
using AdministrationStation.Communication.Models.Shared;

namespace AS_Agent
{
    public class ConfigurationProvider
    {
        public LoginModel Login { get; set; }
        public ServerInfo ServerInfo { get; set; }
        public AgentOptions AgentOptions { get; set; }

        public void Validate()
        {
            if (Login == null) throw new ArgumentNullException(nameof(Login));
            if (ServerInfo == null) throw new ArgumentNullException(nameof(ServerInfo));

            AgentOptions ??= new AgentOptions
            {
                LastUpdate = DateTime.MinValue
            };
        }
    }
}