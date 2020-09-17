using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CP.API.Model
{
    public class Role : IdentityRole<int>
    {
        public ICollection<SupplierRole> SupplierRoles { get; set; }

    }
}