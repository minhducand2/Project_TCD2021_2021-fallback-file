using System;                         
                                      
namespace ApiGen.Data.Entity          
{                                     
    public class E700Shop : EntityBase 
    {                                 
        public long id { get; set; }  
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public string PriceOrigin { get; set; }
        public string PriceCurrent { get; set; }
        public string PricePromotion { get; set; }
        public string Star { get; set; }
        public string Images { get; set; }
        public string Video { get; set; }
        public string IdShopCategories { get; set; }
        public string Point { get; set; }
    }                                 
}                                     
