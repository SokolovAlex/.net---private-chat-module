﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.DbEntities
{
    public class Role: BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }

    }
}
