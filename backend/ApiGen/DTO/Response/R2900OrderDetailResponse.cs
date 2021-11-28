using System;

namespace ApiGen.DTO.Response
{
    public class R2900OrderDetailResponse
    { 
        public long id { get; set; }
        public string IdOrderShop { get; set; }
        public string IdShop { get; set; }
        public int Amount { get; set; }
        public int PriceOrigin { get; set; }
        public int PriceCurrent { get; set; }
        public int TotalPrice { get; set; }
    }
}
