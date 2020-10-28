using System.Threading.Tasks;
using AdministrationStation.Server.Data;
using AdministrationStation.Server.Filters;
using AdministrationStation.Server.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdministrationStation.Server.Controllers.Agent
{
    [ApiController]
    [SideFilter(Side.Agent)]
    [Route("agent/[controller]/[action]")]
    public class StatusController : ControllerBase
    {
        private readonly InfoStore _infoStore;
        private readonly UserManager<User> _userManager;

        public StatusController(InfoStore infoStore, UserManager<User> userManager)
        {
            _infoStore = infoStore;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] StatusModel model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            _infoStore.StatusModels[user.Id] = model;
            return Ok();
        }
    }

    public class StatusModel
    {
        public string Hostname { get; set; }
        public string IpAddress { get; set; }
        public int CpuUsage { get; set; }
        public int MaximumRam { get; set; }
        public int CurrentRam { get; set; }
    }
}