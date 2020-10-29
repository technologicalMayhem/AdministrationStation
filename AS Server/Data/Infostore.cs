using System.Collections.Generic;
using AdministrationStation.Communication.Models.Agent;

namespace AdministrationStation.Server.Data
{
    public class InfoStore
    {
        public Dictionary<int, StatusUpdateModel> StatusModels { get; set; }

        public InfoStore()
        {
            StatusModels = new Dictionary<int, StatusUpdateModel>();
        }
    }
}