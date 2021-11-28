using System;

namespace ApiGen.Data.Entity
{
    public class E500SinhVien : EntityBase
    {
        public long id { get; set; }
        public string IdLop { get; set; } 
        public string Fullname { get; set; }
    }
}
