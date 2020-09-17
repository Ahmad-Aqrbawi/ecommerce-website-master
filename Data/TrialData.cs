using System.Collections.Generic;
using System.Linq;
using CP.API.Model;
using Microsoft.AspNetCore.Identity;

using Newtonsoft.Json;


namespace Converge.API.Data
{
    public class TrialData
    {
        private readonly UserManager<Supplier> _userManager;
        private readonly RoleManager<Role> _roleManager;


        public TrialData(UserManager<Supplier> userManager, RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;


        }
        public void TrialSuppliers()
        {
            if (!_userManager.Users.Any())
            {

               
                var roles = new List<Role>{
                 new Role{Name ="Admin"},
                 new Role{Name ="EstablishAStore"},
                 new Role{Name ="Products"},
                 new Role{Name ="Deals"},
                 new Role{Name ="DiscountCoupons"},
                 new Role{Name ="NewOrders"},
                 new Role{Name ="Reports"},
                 new Role{Name ="TechnicalSupport"},
                 new Role{Name ="Moderatore"},
                 new Role{Name ="Member"},
                 new Role{Name ="VIP"}
                 };
                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }


              

               var adminUser = new Supplier
            {
                Email = "Admin@Admin.com",
                UserName = "mohammad"
            };

            IdentityResult result = _userManager.CreateAsync(adminUser, "adminadmin").Result;

            var admin = _userManager.FindByEmailAsync("Admin@Admin.com").Result;
            _userManager.AddToRolesAsync(
                admin, new[] { "Admin", "Moderatore" }
            ).Wait();

            }






            

        }

    }

}
