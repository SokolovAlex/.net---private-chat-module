using Core.Enums;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.User
{
    public partial class UserModel : BaseModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public UserRole UserRole
        {
            get
            {
                return (UserRole)RoleId;
            }
            set
            {
                RoleId = (int)value;
            }
        }

        public int RoleId { get; set; }
    }
}
