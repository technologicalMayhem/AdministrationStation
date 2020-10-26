using System.ComponentModel.DataAnnotations.Schema;

namespace AdministrationStation.Server.Identity.Data
{
    public class Agent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AgentId { get; set; }
        public User User { get; set; }
    }
}