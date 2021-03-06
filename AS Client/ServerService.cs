﻿using System;
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
        
        private void EnsureAuthentication()
        {
            if (!IsAuthenticated) throw new Exception();
        }

        public async Task Login(string password)
        {
            //Todo: See about making sure that the password sticks around in memory as short as possible
            var model = new LoginModel
            {
                Username = ClientConfiguration.Username,
                Password = password
            };
            var payload = JsonSerializer.Serialize(model);
            var content = new StringContent(payload, Encoding.UTF8, Json);
            var response = await _client.PostAsync("/authentication/clientLogin", content);
            response.EnsureSuccessStatusCode();

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var login = await JsonSerializer.DeserializeAsync<LoginResult>(responseStream);
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + login.Token);
            AuthenticatedUntil = login.Expiration.Subtract(TimeSpan.FromMinutes(10));
        }

        public async Task<IEnumerable<StatusResultModel>> GetAgentStatuses()
        {
            EnsureAuthentication();
            
            var response = _client.GetAsync("/client/status/get").Result;
            
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var statusData = await JsonSerializer.DeserializeAsync<StatusResultModel[]>(responseStream);
            return statusData;
        }
        
        public async Task<ServerState> GetServerState()
        {
            var response = await _client.GetAsync("/status");
            response.EnsureSuccessStatusCode();
            
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ServerState>(responseStream);
        }
    }
}