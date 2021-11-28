using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGen.Data.Entity
{
    public class E000Account : EntityBase
    {
        public long id { get; set; }
        public long IdRole { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string img { get; set; }
        public DateTime created_date { get; set; }
        public DateTime created_date1 { get; set; }
    }
}
