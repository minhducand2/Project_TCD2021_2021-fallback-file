using System;                         
                                      
namespace ApiGen.Data.Entity          
{                                     
    public class E2900OrderDetail : EntityBase 
    {                                 
        public long id { get; set; }  
        public string IdOrderShop { get; set; }
        public string IdShop { get; set; }
        public string Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }                                 
}                                     
