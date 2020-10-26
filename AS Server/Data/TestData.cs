using System;
using System.Collections.Generic;
using System.Linq;
using AdministrationStation.Server.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace AdministrationStation.Server.Data
{
    public static class TestData
    {
        public static void Seed(ServerContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (context.Database.EnsureCreated())
            {
                roleManager.CreateAsync(new Role("Administrator"));
                roleManager.CreateAsync(new Role("Guest"));
                
                var users = new List<(string, string, string[])>
                {
                    ("Tobias", "tobias", new[] {"Administrator"}),
                    ("Calvin", "calvin", new string[] { }),
                    ("JohnSmith", "johnSmith", new[] {"Guest"})
                };
                
                foreach (var (username, password, roles) in users)
                {
                    var user = new User(username);
                    userManager.CreateAsync(user, password);
                    userManager.AddToRolesAsync(user, roles);
                }

                context.SaveChanges();
            }

            Console.WriteLine("--- Users ---");
            foreach (var user in context.Users
                .Include(o => o.Roles)
                .ThenInclude(o => o.Role))
            {
                var roles = userManager.GetRolesAsync(user).Result.Join();
                Console.WriteLine(
                    $"{user.Id} : {user.UserName} : {user.NormalizedUsername} : {(string.IsNullOrEmpty(roles) ? "None" : roles)}");
            }

            Console.WriteLine("--- Roles ---");
            foreach (var role in context.Roles)
            {
                Console.WriteLine($"{role.Id} : {role.Name}");
            }
        }
    }
}