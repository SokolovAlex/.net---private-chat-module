using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.DbEntities
{
    public partial class User: BaseEntity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [NotMapped]
        public UserRole UserRole {
            get {
                return (UserRole)RoleId;
            }
            set {
                RoleId = (int)value;
            }
        }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
