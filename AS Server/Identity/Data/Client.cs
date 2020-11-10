using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AdministrationStation.Server.Identity.Data
{
    public class Client
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }

        [ForeignKey("User")] public int UserId { get; set; }
        [JsonIgnore] public User User { get; set; }
    }
}