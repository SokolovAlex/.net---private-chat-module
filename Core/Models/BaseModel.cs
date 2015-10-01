using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class BaseModel
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsNew() {
            return Id == 0;
        }
    }
}
