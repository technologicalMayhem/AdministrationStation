using System;
using System.Threading.Tasks;
using AdministrationStation.Communication.Models.Agent;
using AdministrationStation.Server.Data;
using AdministrationStation.Server.Filters;
using AdministrationStation.Server.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdministrationStation.Server.Controllers.AgentSide
{
    [ApiController]
    [Authorize]
    [SideFilter(Side.Agent)]
    [Route("agent/[controller]/[action]")]
    public class StatusController : ControllerBase
    {
        private readonly InfoStore _infoStore;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<StatusController> _logger;

        public StatusController(InfoStore infoStore, UserManager<User> userManager, ILogger<StatusController> logger)
        {
            _infoStore = infoStore;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] StatusUpdateModel model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            _infoStore.StatusModels[user.Id] = model;
            return Ok();
        }
    }
}