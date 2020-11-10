using System;
using System.Reflection;
using System.Threading.Tasks;
using AdministrationStation.Server.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace AdministrationStation.Server.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SideFilter : ActionFilterAttribute
    {
        private readonly Side _side;

        public SideFilter(Side side)
        {
            _side = side;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userManager = context.HttpContext.RequestServices.GetService<UserManager<User>>();
            var user = userManager.GetUserAsync(context.HttpContext.User).Result;

            if (user.Side == _side)
            {
                await next();
            }
            else
            {
                throw new InvalidSideException($"{user.Side} user tried to access action of side {_side}.");
            }
        }
    }

    public class InvalidSideException : Exception
    {
        public InvalidSideException(string message) : base(message)
        {
        }
    }
}