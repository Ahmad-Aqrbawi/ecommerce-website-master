using System.Collections.Generic;

namespace CP.API.Dto
{
    public class ShipperReturnDTO
    {
         public int ShipperId { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public ICollection<OrderReturnDTO> Orders { get; set; }
    }
}