using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CP.API.Model
{
    public class SupplierRole : IdentityUserRole<int>
    {
        public Supplier Supplier { get; set; }
        public Role Role { get; set; }
    }
}