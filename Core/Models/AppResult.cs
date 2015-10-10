using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class AppResult
    {
        public bool isError { get; set; }

        public bool Code { get; set; }

        public string Message { get; set; }

        public bool Success { get {
                return !isError;
            }
        }
    }
}
