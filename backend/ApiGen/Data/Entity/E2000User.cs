using System;                         
                                      
namespace ApiGen.Data.Entity          
{                                     
    public class E2000User : EntityBase 
    {                                 
        public long id { get; set; }  
        public string IdUserStatus { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public string IdRoleUser { get; set; }
        public string authkey { get; set; }
        public string Phone { get; set; }
        public string Sex { get; set; }
        public string IdCity { get; set; }
        public string IdDistrict { get; set; }
        public string Address { get; set; }
        public string Point { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }                                 
}                                     
