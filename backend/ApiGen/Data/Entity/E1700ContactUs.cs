using System;                         
                                      
namespace ApiGen.Data.Entity          
{                                     
    public class E1700ContactUs : EntityBase 
    {                                 
        public long id { get; set; }  
        public string IdContactStatus { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }                                 
}                                     
