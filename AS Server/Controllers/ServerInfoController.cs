using System.Threading.Tasks;
using AdministrationStation.Communication.Models.Shared;
using AdministrationStation.Server.Data;
using Microsoft.AspNetCore.Mvc;

namespace AdministrationStation.Server.Controllers
{
    [ApiController]
    public class ServerInfoController : ControllerBase
    {

        private readonly ServerStatistics _statistics;

        public ServerInfoController(ServerStatistics statistics)
        {
            _statistics = statistics;
        }

        [Route("status")]
        public async Task<ServerState> GetServerInfo()
        {
            return new ServerState
            {
                Uptime = _statistics.Uptime.TotalSeconds
            };
        }
    }
}