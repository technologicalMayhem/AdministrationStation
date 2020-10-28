using System.Collections.Generic;
using AdministrationStation.Server.Controllers.Agent;

namespace AdministrationStation.Server.Data
{
    public class InfoStore
    {
        public Dictionary<int, StatusModel> StatusModels { get; set; }

        public InfoStore()
        {
            StatusModels = new Dictionary<int, StatusModel>();
        }
    }
}