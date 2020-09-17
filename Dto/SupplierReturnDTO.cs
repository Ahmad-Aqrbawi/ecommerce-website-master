using System;
using System.Collections.Generic;

namespace CP.API.Dto
{
    public class SupplierReturnDTO
    {
        public int id { get; set; }
       public string CompanyName { get; set; }
        public string facilityOwnerName { get; set; }
       public string IdNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CommercialActivities { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public DateTime DateOfContract { get; set; }
        public string PhotoURL { get; set; }
        public ICollection<ProductReturnDTO> Products { get; set; }
        public ICollection<PhotoForReturnDto> Photos { get; set; }
    }
}
