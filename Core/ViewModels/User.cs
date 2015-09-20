using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class UserVm
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

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
