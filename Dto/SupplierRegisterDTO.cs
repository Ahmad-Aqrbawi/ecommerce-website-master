using System;
using System.Collections.Generic;
using CP.API.Model;

namespace CP.API.Dto
{
    public class SupplierRegisterDTO
    {
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string facilityOwnerName { get; set; }
        public string IdNumber { get; set; }
        public DateTime DateOfContract { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public SupplierRegisterDTO()
        {
            DateOfContract = DateTime.Now;
        }

    }
}