using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CP.API.Model
{
    public class Supplier: IdentityUser<int>
    {
        public string CompanyName { get; set; }
        public string IdNumber { get; set; }
        public string Phone { get; set; }
        public string CommercialActivities { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public DateTime DateOfContract { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<PhotoForSupplier> PhotoForSuppliers { get; set; }
        public ICollection<SupplierRole> SupplierRoles { get; set; }







    }
}