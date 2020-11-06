using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AdministrationStation.Server.Identity.Data
{
    public class AgentOptionsHolder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AgentOptionsHolderId { get; set; }
        
        [ForeignKey("Agent")] public int UserId { get; set; }
        [JsonIgnore] public Agent Agent { get; set; }

        public string AgentOptions { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}