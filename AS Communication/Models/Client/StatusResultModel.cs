using System.Text.Json.Serialization;
using AdministrationStation.Communication.Models.Agent;

namespace AdministrationStation.Communication.Models.Client
{
    public class StatusResultModel
    {
        public string Agent { get; set; }
        public StatusUpdateModel Status { get; set; }
    }
}