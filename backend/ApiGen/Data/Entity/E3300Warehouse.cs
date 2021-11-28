using System;
using System.ComponentModel.DataAnnotations;

namespace ApiGen.Data.Entity          
{                                     
    public class E3300Warehouse : EntityBase 
    {                                 
        public long id { get; set; }  
        public string IdShop { get; set; }
        public string Amount { get; set; }
        public string IdCity { get; set; }
        public DateTime ExpiryDate { get; set; }
    }                                 
}                                     
