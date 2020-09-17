using System.Threading.Tasks;
using CP.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CP.API.Model;
using CP.API.Dto;

namespace CP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<Supplier> _userManager;
        public AdminController(DataContext context, UserManager<Supplier> userManager)
        {
            _userManager = userManager;
            _context = context;

        }




        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("SupplierWithRoles")]

        public async Task<IActionResult> GetSupplierWithRole()
        {
            var supplierList = await (from supplier in _context.Users
                                      orderby supplier.UserName
                                      select new
                                      {
                                          Id = supplier.Id,
                                          UserName = supplier.UserName,
                                          Roles = (from userRole in supplier.SupplierRoles
                                                   join role in _context.Roles
                                                   on userRole.RoleId equals role.Id
                                                   select role.Name).ToList()

                                      }).ToListAsync();


            return Ok(supplierList);
        }

        [Authorize(Policy = "VIP")]
        [HttpGet("ModerateSupplierRole")]

        public IActionResult GetModerateSupplierRole()
        {
            return Ok("مصرح بدخول الشرفين ومدير المشروع فقط");
        }



        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("editorroles/{userName}")]

        public async Task<IActionResult> EditRoles(string userName, RoleEditDTO roleEditDTO)
        {
            var supplier = await _userManager.FindByNameAsync(userName);
            var supplierRoles = await _userManager.GetRolesAsync(supplier);
            var selectedRole = roleEditDTO.RoleNames;
            selectedRole = selectedRole ?? new string[] { };
            var result = await _userManager.AddToRolesAsync(supplier, selectedRole.Except(supplierRoles));
            if (!result.Succeeded)
                return BadRequest("حدث خطا");
            result = await _userManager.RemoveFromRolesAsync(supplier, supplierRoles.Except(selectedRole));
            if (!result.Succeeded)
                return BadRequest("حدث خطا");
            return Ok(await _userManager.GetRolesAsync(supplier));

        }


    }
}