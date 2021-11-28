using System;                         
                                      
namespace ApiGen.Data.Entity          
{                                     
    public class E1000ShopComment : EntityBase 
    {                                 
        public long id { get; set; }  
        public string IdShop { get; set; }
        public string IdUser { get; set; }
        public string IdCommentStatus { get; set; }
        public string Content { get; set; }
        public string IdTypeComment { get; set; }
        public string IdStaff { get; set; }        
        public DateTime CreatedAt { get; set; }
    }                                 
}                                     
