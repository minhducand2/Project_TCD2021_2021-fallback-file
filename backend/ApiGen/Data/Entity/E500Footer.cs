using System;                         
                                      
namespace ApiGen.Data.Entity          
{                                     
    public class E500Footer : EntityBase 
    {                                 
        public long id { get; set; }  
        public string Content1 { get; set; }
        public string Content2 { get; set; }
        public string Content3 { get; set; }
        public string AndroidLink { get; set; }
        public string IosLink { get; set; }
    }                                 
}                                     
