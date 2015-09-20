using Dal.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Models
{
    public class PagingParameters<T> where T : BaseEntity
    {
        public int PageIndex = 0;
        public int PageSize = 0;
        public Func<T, object> OrderBy = null;
        public bool Asc = true;
    }
}
