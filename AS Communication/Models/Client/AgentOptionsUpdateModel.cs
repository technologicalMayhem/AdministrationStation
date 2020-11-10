using AdministrationStation.Communication.Models.Agent;

namespace AdministrationStation.Communication.Models.Client
{
    public class AgentOptionsUpdateModel
    {
        public string AgentName { get; set; }
        public AgentOptions AgentOptions { get; set; }
    }
}