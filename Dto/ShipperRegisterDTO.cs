using System.Collections.Generic;
using CP.API.Model;

namespace CP.API.Dto
{
    public class ShipperRegisterDTO
    {
        public int ShipperId { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
    }
}