using AdministrationStation.Communication.Models.Agent;
using AdministrationStation.Server.Filters;
using AdministrationStation.Server.Identity.Data;

namespace AdministrationStation.Server.Data
{
    public class AgentOptionsProvider
    {
        public static AgentOptions GetDefault(User user)
        {
            if(user.Side != Side.Agent) throw new InvalidSideException("Wrong side.");
            return new AgentOptions
            {
                General = new GeneralOptions
                {
                    ShouldSendStatusUpdates = true
                },
                Job = new JobOptions
                {
                    
                }
            };
        }
    }
}