using Core.Models;
using System.Collections.Generic;

namespace Core.Models.User
{

    public sealed class UserSessionModel
    {
        public bool IsExpirated
        {
            get;
            set;
        }

        public KeyValuePair<int, KeyValuePair<int, KeyValuePair<int, bool>[]>[]>[] SystemAccess
        {
            get;
            set;
        }

        public UserModel UserModel
        {
            get;
            set;
        }
    }
}