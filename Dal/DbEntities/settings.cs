using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.DbEntities
{
    public class Settings: BaseEntity
    {
        public string Value { get; set; }

        public string Key { get; set; }
    }
}
