using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.DbEntities
{
    public abstract class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }
        
        protected BaseEntity()
        {
            Id = 0;
        }

        public bool IsNew()
        {
            return Id == 0;
        }
    }
}
