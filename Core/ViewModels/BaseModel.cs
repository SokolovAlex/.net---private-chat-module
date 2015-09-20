using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class BaseVm
    {
        public int Id { get; set; }

        protected BaseVm()
        {
            Id = 0;
        }

        public bool IsNew()
        {
            return Id == 0;
        }
    }
}
