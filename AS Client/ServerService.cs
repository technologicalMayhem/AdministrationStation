using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AdministrationStation.Communication.Models.Client;
using AdministrationStation.Communication.Models.Shared;

namespace AS_Client
{
    public class ServerService
    {
        public static ServerService Instance => _instance ??= new ServerService();
        
        private static ServerService _instance;
        
        private DateTime AuthenticatedUntil { get; set; }
        private bool IsAuthenticated => AuthenticatedUntil > DateTime.Now;
        
        private readonly HttpClient _client;
        
        private const string Json = "application/json";
        
        private ServerService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5001/")
            };
        }
        
        private async Task EnsureAuthentication()
        {
            if (!IsAuthenticated) await Login();
        }

        private async Task Login()
        {
            var model = new LoginModel
            {
                Username = ClientConfiguration.Username,
                Password = ClientConfiguration.Password
            };
            var payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, Json);
            var response = _client.PostAsync("/authentication/clientLogin", content).Result;
            response.EnsureSuccessStatusCode();

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var login = await JsonSerializer.DeserializeAsync<LoginResult>(responseStream);
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + login.Token);
            AuthenticatedUntil = login.Expiration.Subtract(TimeSpan.FromMinutes(10));
        }

        public async Task<IEnumerable<StatusResultModel>> GetAgentStatuses()
        {
            await EnsureAuthentication();
            
            var response = _client.GetAsync("/client/status/get").Result;
            
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var statusData = await JsonSerializer.DeserializeAsync<StatusResultModel[]>(responseStream);
            return statusData;
        }
    }
}