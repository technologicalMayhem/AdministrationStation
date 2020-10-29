using System;
using System.Text.Json.Serialization;

namespace AdministrationStation.Communication.Models.Shared
{
    public class LoginResult
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}