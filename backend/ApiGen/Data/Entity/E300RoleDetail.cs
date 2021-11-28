using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGen.Data.Entity
{
    public class E300RoleDetail : EntityBase
    {
        public long id { get; set; }
        public string IdRole { get; set; }
        public string IdMenu { get; set; }
        public string Status { get; set; }
    }
}
