using System;                         
                                      
namespace ApiGen.Data.Entity          
{                                     
    public class E2100Promotion : EntityBase 
    {                                 
        public long id { get; set; }  
        public string Name { get; set; }
        public string PromotionCode { get; set; }
        public string PercentCode { get; set; }
        public string MoneyDiscount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Point { get; set; }
    }                                 
}                                     
