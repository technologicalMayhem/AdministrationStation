using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdministrationStation.Communication.Models.Agent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AS_Agent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;
                    var options = configuration.GetSection("Agent").Get<ConfigurationProvider>();
                    options.Validate();
                    
                    services.AddSingleton(options);
                    services.AddHttpClient<ServerService>();

                    services.AddHostedService<Worker>();
                });
    }
}
