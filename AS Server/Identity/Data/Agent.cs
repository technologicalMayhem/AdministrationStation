using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AdministrationStation.Communication.Models.Agent;

namespace AdministrationStation.Server.Identity.Data
{
    public class Agent
    {
        public Agent()
        {
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AgentId { get; set; }

        [ForeignKey("User")] public int UserId { get; set; }
        [JsonIgnore] public User User { get; set; }

        public AgentOptionsHolder Options { get; set; }
    }
}