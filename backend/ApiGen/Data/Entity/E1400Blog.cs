using System;                         
                                      
namespace ApiGen.Data.Entity          
{                                     
    public class E1400Blog : EntityBase 
    {                                 
        public long id { get; set; }  
        public string IdBlogCategories { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string NumberView { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }                                 
}                                     
