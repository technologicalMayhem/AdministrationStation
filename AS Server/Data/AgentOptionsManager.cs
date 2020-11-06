using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AdministrationStation.Communication.Models.Agent;
using AdministrationStation.Server.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AdministrationStation.Server.Data
{
    public class AgentOptionsManager
    {
        private readonly ServerContext _context;

        public AgentOptionsManager(ServerContext context)
        {
            _context = context;
        }

        public async Task<AgentOptions> GetOptions(Agent agent)
        {
            var configuration = await _context.Users.Where(user => user.Id == agent.UserId)
                .Select(user => user.Agent.Options.AgentOptions).FirstOrDefaultAsync();

            if (configuration == null) return AgentOptionsProvider.GetDefault(agent.User);

            var agentConfiguration = Deserialize(configuration);
            return agentConfiguration;
        }

        public async Task<DateTime> GetOptionsLastUpdate(Agent agent)
        {
            return await _context.Users.Where(user => user.Id == agent.UserId)
                .Select(user => user.Agent.Options.LastUpdate).FirstOrDefaultAsync();
        }

        public async Task<OperationResult> SetOptions(Agent agent, AgentOptions options)
        {
            var lastUpdate = await GetOptionsLastUpdate(agent);

            if (options.LastUpdate >= lastUpdate) return OperationResult.Error(OperationError.NoChangeOrOlder);

            var holder = await _context.Users.Where(user => user.Id == agent.UserId)
                .Select(user => user.Agent.Options).FirstOrDefaultAsync();
            holder.LastUpdate = options.LastUpdate;
            holder.AgentOptions = Serialize(options);
            await _context.SaveChangesAsync();
            return OperationResult.Success;
        }

        public string ComputeChecksum(AgentOptions agentOptions)
        {
            return CreateMd5(Serialize(agentOptions));
        }

        private static string CreateMd5(string input)
        {
            // Use input string to calculate MD5 hash
            using var md5 = MD5.Create();
            var inputBytes = Encoding.Default.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            var sb = new StringBuilder();
            foreach (var t in hashBytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        private static string Serialize(AgentOptions options)
        {
            return JsonConvert.SerializeObject(options);
        }

        private static AgentOptions Deserialize(string configuration)
        {
            return JsonConvert.DeserializeObject<AgentOptions>(configuration);
        }
    }
}