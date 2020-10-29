using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdministrationStation.Server.Filters;
using AdministrationStation.Server.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Side = AdministrationStation.Server.Identity.Data.Side;

namespace AdministrationStation.Server.Data
{
    public static class SeedData
    {
        public static void Seed(ServerContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            context.Database.EnsureDeleted();
            if (context.Database.EnsureCreated())
            {
                roleManager.CreateAsync(new Role("Administrator")).Wait();
                roleManager.CreateAsync(new Role("Guest")).Wait();

                var users = new List<(string, string, string[], Side, object)>
                {
                    ("Tobias", "tobias", new[] {"Administrator"}, Side.Client, new Client()),
                    ("Calvin", "calvin", new string[] { }, Side.Client, new Client()),
                    ("JohnSmith", "johnSmith", new[] {"Guest"}, Side.Client, new Client()),
                    ("70eb8784-b06d-401a-b3db-7b629006415f", "password", new string[] { }, Side.Agent, new Agent())
                };


                foreach (var (username, password, roles, side, o) in users)
                {
                    var user = side switch
                    {
                        Side.Agent => new User(username, (Agent) o),
                        Side.Client => new User(username, (Client) o),
                        _ => null
                    };
                    userManager.CreateAsync(user, password).Wait();
                    userManager.AddToRolesAsync(user, roles).Wait();
                }
            }

            Console.WriteLine("--- Users ---");
            foreach (var user in context.Users
                .Include(o => o.Roles)
                .ThenInclude(o => o.Role))
            {
                var roles = string.Join(", ", userManager.GetRolesAsync(user).Result);
                Console.WriteLine(
                    $"{user.Id} : {user.UserName} : {user.NormalizedUsername} : {(string.IsNullOrEmpty(roles) ? "None" : roles)} : {user.Side}");
            }

            Console.WriteLine("--- Roles ---");
            foreach (var role in context.Roles)
            {
                Console.WriteLine($"{role.Id} : {role.Name}");
            }
        }
    }
}