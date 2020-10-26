using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdministrationStation.Server.Identity.Data
{
    public class User
    {
        public User(string userName)
        {
            UserName = userName;
            NormalizedUsername = userName.ToUpper();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUsername { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string HashedPassword { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; }
    }
}