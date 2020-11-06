using System;
using System.Data;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using AdministrationStation.Communication.Models.Agent;
using Microsoft.Extensions.Logging;

namespace AS_Agent
{
    public class ResponseLine
    {
        //Todo: Look into a way to make this a proper background tasks.
        //Also consider if this should simply notify the main loop or 
        //if this should do the required work itself. Maybe rethink agent
        //design as a whole and how it should go about doing scheduled/requested work.

        public bool IsAvailable { get; set; }
        
        private readonly ILogger<ResponseLine> _logger;
        private readonly ConfigurationProvider _configuration;

        private readonly TcpClient _client;

        public ResponseLine(ILogger<ResponseLine> logger, ConfigurationProvider configuration)
        {
            _logger = logger;
            _configuration = configuration;

            _client = new TcpClient();
        }

        public async Task ConnectAsync()
        {
            try
            {
                await _client.ConnectAsync(_configuration.ServerInfo.ServerAddress, _configuration.ServerInfo.ResponsePort);
                IsAvailable = true;
                return;
            }
            catch (ArgumentOutOfRangeException)
            {
                _logger.LogError($"{_configuration.ServerInfo.ResponsePort} is not a valid port.");
            }
            catch (SocketException)
            {
                _logger.LogError("A response line connection couldn't be established to the host.");
            }

            IsAvailable = false;
        }

        public async Task CheckForUpdates()
        {
            throw new NotImplementedException();
        }
    }
}