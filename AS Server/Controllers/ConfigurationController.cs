using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AdministrationStation.Communication.Models.Agent;
using AdministrationStation.Communication.Models.Client;
using AdministrationStation.Server.Data;
using AdministrationStation.Server.Filters;
using AdministrationStation.Server.Identity.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using StackExchange.Profiling;

namespace AdministrationStation.Server.Controllers
{
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly AgentOptionsManager _agentOptionsManager;
        private readonly UserManager<User> _userManager;
        private readonly ServerContext _context;
        private readonly ILogger<ConfigurationController> _logger;

        private const string AgentBase = "agent/config/";
        private const string ClientBase = "client/config/";

        public ConfigurationController(ServerContext context, ILogger<ConfigurationController> logger,
            AgentOptionsManager agentOptionsManager, UserManager<User> userManager)
        {
            _context = context;
            _logger = logger;
            _agentOptionsManager = agentOptionsManager;
            _userManager = userManager;
        }

        [HttpPost]
        [Route(AgentBase + "get")]
        public async Task<IActionResult> GetAgentOptions([Optional] [FromBody] DateTime lastUpdate)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var lastUpdateOnRecord = await _agentOptionsManager.GetOptionsLastUpdate(user.Agent);
            if (lastUpdate == DateTime.MinValue || lastUpdate < lastUpdateOnRecord)
            {
                return Ok(await _agentOptionsManager.GetOptions(user.Agent));
            }

            return NoContent();
        }

        [HttpPost]
        [Route(ClientBase + "updateAgent")]
        public async Task<IActionResult> UpdateAgentOptions([FromBody] AgentOptionsUpdateModel model)
        {
            model.AgentOptions.LastUpdate = DateTime.Now;
            var user = await _userManager.FindByNameAsync(model.AgentName);
            if (user == null)
            {
                return BadRequest(OperationResult.Error(OperationError.AgentNotFound));
            }

            var result = await _agentOptionsManager.SetOptions(user.Agent, model.AgentOptions);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }
    }
}