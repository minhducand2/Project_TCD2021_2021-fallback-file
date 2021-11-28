using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGen.Data.Entity
{
    public class E100Menu : EntityBase
    {
        public long id { get; set; }
        public string IdParentMenu { get; set; }
        public string IsGroup { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Icon { get; set; }
        public string Position { get; set; }  
        public long id1 { get; set; }
        public string IdParentMenu1 { get; set; }
        public string IsGroup1 { get; set; }
        public string Name1 { get; set; }
        public string Slug1 { get; set; }
        public string Icon1 { get; set; }
        public string Position1 { get; set; }
        public string IdRoleDetail { get; set; }
        public string IdRoleDetail1 { get; set; }
        public string Status { get; set; }
        public string Status1 { get; set; }
    }
}
