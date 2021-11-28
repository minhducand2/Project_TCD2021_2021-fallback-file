using System;                         
                                      
namespace ApiGen.Data.Entity          
{                                     
    public class E1500ContactInfo : EntityBase 
    {                                 
        public long id { get; set; }  
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Working { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
        public string Twitter { get; set; }
        public string Map { get; set; }
    }                                 
}                                     
