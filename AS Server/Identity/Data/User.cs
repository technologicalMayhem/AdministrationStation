using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdministrationStation.Server.Identity.Data
{
    public class User
    {
        public User(string userName, Client client)
        {
            UserName = userName;
            Client = client;
            Side = Side.Client;
        }

        public User(string userName, Agent agent)
        {
            UserName = userName;
            Agent = agent;
            Side = Side.Agent;
        }

        public User()
        {
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserName { get; set; }
        public string NormalizedUsername { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string HashedPassword { get; set; }
        
        public Side Side { get; set; }
        public Agent Agent { get; set; }
        public Client Client { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; }
    }
}