using System.Collections.Generic;
using System.Linq;
using AdministrationStation.Communication.Models.Client;
using AdministrationStation.Server.Data;
using AdministrationStation.Server.Filters;
using AdministrationStation.Server.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdministrationStation.Server.Controllers.ClientSide
{
    [ApiController]
    [SideFilter(Side.Client)]
    [Route("client/[controller]/[action]")]
    public class StatusController : ControllerBase
    {
        private readonly InfoStore _infoStore;
        private readonly UserManager<User> _userManager;

        public StatusController(InfoStore infoStore, UserManager<User> userManager)
        {
            _infoStore = infoStore;
            _userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<StatusResultModel> Get()
        {
            var status = _infoStore.StatusModels.Select(pair => new StatusResultModel
            {
                Agent = _userManager.FindByIdAsync(pair.Key.ToString()).Result.UserName,
                Status = pair.Value
            }).ToArray();
            return status;
        }
    }
}