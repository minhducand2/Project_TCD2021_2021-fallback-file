using System;                         
                                      
namespace ApiGen.Data.Entity          
{                                     
    public class E2800OrderShop : EntityBase 
    {                                 
        public long id { get; set; }  
        public string IdProductType { get; set; }
        public string IdUser { get; set; }
        public string IdOrderStatus { get; set; }
        public string IdCity { get; set; }
        public string IdDistrict { get; set; }
        public string IdPaymentStatus { get; set; }
        public string IdPaymentType { get; set; }
        public string TotalPrice { get; set; }
        public string PromotionCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string Point { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }                                 
}                                     
