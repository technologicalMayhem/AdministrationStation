using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AdministrationStation.Server.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdministrationStation.Server.Controllers
{
    [ApiController]
    [Route("Agent/{controller}/{action}")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }
        
        [HttpGet]
        [Authorize]
        public ForecastContainer Test()
        {
            var rng = new Random();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userManager.GetUserAsync(User).Result;
            //var roles = _userManager.GetRolesAsync(user).Result;

            return new ForecastContainer
            {
                Identity = User.Identity,
                User = user,
                UserId = userId,
                AuthData = User.Claims.Select(claim => new AuthData
                {
                    Issuer = claim.Issuer,
                    Properties = claim.Properties,
                    Type = claim.Type,
                    Value = claim.Value,
                    OriginalIssuer = claim.OriginalIssuer,
                    ValueType = claim.ValueType
                }),
                WeatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = rng.Next(-20, 55),
                        Summary = Summaries[rng.Next(Summaries.Length)]
                    })
                    .ToArray()
            };
        }
        
        public class ForecastContainer
        {
            public IEnumerable<AuthData> AuthData { get; set; }
            public IEnumerable<WeatherForecast> WeatherForecasts { get; set; }
            public IIdentity Identity { get; set; }
            public User User { get; set; }
            public string UserId { get; set; }
        }
        
        public class AuthData
        {
            public string Issuer { get; set; }
            public IDictionary<string,string> Properties { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
            public string OriginalIssuer { get; set; }
            public string ValueType { get; set; }
        }
    }
}