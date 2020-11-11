using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AdministrationStation.Communication.Models.Agent;
using AdministrationStation.Communication.Models.Shared;

namespace AS_Agent
{
    public class ServerService
    {
        private DateTime AuthenticatedUntil { get; set; }
        private bool IsAuthenticated => AuthenticatedUntil > DateTime.Now;

        private readonly HttpClient _client;
        private readonly ConfigurationProvider _options;

        private const string Json = "application/json";

        public ServerService(HttpClient client, ConfigurationProvider options)
        {
            client.BaseAddress =
                new Uri("https://" + options.ServerInfo.ServerAddress + ":" + options.ServerInfo.ServerPort + "/");

            _client = client;
            _options = options;
        }

        private async Task EnsureAuthentication()
        {
            if (!IsAuthenticated) await Login();
        }

        public async Task<ServerState> GetServerState()
        {
            var response = await _client.GetAsync("/status");
            response.EnsureSuccessStatusCode();
            
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ServerState>(responseStream);
        }

        private async Task Login()
        {
            var model = new LoginModel
            {
                Username = _options.Login.Username,
                Password = _options.Login.Password
            };
            var payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, Json);
            var response = await _client.PostAsync("/authentication/agentLogin", content);
            response.EnsureSuccessStatusCode();

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var login = await JsonSerializer.DeserializeAsync<LoginResult>(responseStream);
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + login.Token);
            AuthenticatedUntil = login.Expiration.Subtract(TimeSpan.FromMinutes(10));
        }

        public async Task ReportStatus(StatusUpdateModel model)
        {
            await EnsureAuthentication();

            var payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, Json);
            var response = await _client.PostAsync("/agent/status/update", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<AgentOptions> UpdateOptions(AgentOptions options)
        {
            await EnsureAuthentication();

            var payload = JsonSerializer.Serialize(options.LastUpdate);
            var content = new StringContent(payload, Encoding.UTF8, Json);
            var response = await _client.PostAsync("agent/config/get", content);
            response.EnsureSuccessStatusCode();

            if (response.StatusCode == HttpStatusCode.NoContent) return options;
            
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<AgentOptions>(responseStream);
        }
    }
}