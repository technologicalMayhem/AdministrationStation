using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        private readonly WorkerConfiguration _configuration;

        private const string Json = "application/json";

        public ServerService(HttpClient client, WorkerConfiguration configuration)
        {
            client.BaseAddress =
                new Uri("https://" + configuration.ServerAddress + ":" + configuration.ServerPort + "/");

            _client = client;
            _configuration = configuration;
        }

        private async Task EnsureAuthentication()
        {
            if (!IsAuthenticated) await Login();
        }

        private async Task Login()
        {
            var model = new LoginModel
            {
                UserName = _configuration.Username,
                Password = _configuration.Password
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
    }
}