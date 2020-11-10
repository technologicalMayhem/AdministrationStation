using System.ComponentModel.DataAnnotations.Schema;

namespace AdministrationStation.Server.Identity.Data
{
    public class UserRole
    {
        public UserRole(User user, Role role)
        {
            User = user;
            Role = role;
        }

        public UserRole()
        {
        }

        public int UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}