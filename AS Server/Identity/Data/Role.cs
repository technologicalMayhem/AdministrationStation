using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdministrationStation.Server.Identity.Data
{
    public class Role
    {
        public Role(string name)
        {
            Name = name;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public virtual ICollection<UserRole> Users { get; set; }
    }
}