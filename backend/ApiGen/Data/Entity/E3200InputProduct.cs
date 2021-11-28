using System;                         
                                      
namespace ApiGen.Data.Entity          
{                                     
    public class E3200InputProduct : EntityBase 
    {                                 
        public long id { get; set; }  
        public string IdShop { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Amount { get; set; }
        public string IdCity { get; set; }
    }                                 
}                                     
