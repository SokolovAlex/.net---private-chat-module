using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivateChat.Web.Models
{
    public class AppUser : IUser<string>
    {
        public string Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string UserName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}