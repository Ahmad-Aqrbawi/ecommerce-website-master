using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CP.API.Data;
using CP.API.Dto;
using CP.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CP.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class SuppController : ControllerBase
    {
       
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly SignInManager<Supplier> _signInManager;
        private readonly UserManager<Supplier> _userManager;
        public SuppController( UserManager<Supplier> userManager, SignInManager<Supplier> signInManager, IConfiguration config, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _mapper = mapper;
            
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(SupplierRegisterDTO supplierRegisterDTO)
        {
            var supplierToCreate = _mapper.Map<Supplier>(supplierRegisterDTO);
            
            var reslut = await _userManager.CreateAsync(supplierToCreate, supplierRegisterDTO.Password);
            var supplierToReturn = _mapper.Map<SupplierReturnDTO>(supplierToCreate);

            if (reslut.Succeeded)
            {
                return CreatedAtRoute("GetSupplier", new { controller = "Supplier", id = supplierToCreate.Id },
                             supplierToReturn);
            }
            return BadRequest(reslut.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(SupplierLoginDTO supplierLoginDTO)
        {
            var supplier = await _userManager.FindByEmailAsync(supplierLoginDTO.Email);
            var result = await _signInManager.CheckPasswordSignInAsync(supplier, supplierLoginDTO.Password, false);
            if (result.Succeeded)
            {
                var appSupplier = await _userManager.Users.FirstOrDefaultAsync(
                    s => s.Email == supplierLoginDTO.Email.ToUpper()
                );
                var supplierToReturn = _mapper.Map<SupplierReturnDTO>(appSupplier);
                return Ok(new
                {
                    token = GenerateJwtToken(appSupplier).Result,
                    supplier = supplierToReturn
                });
            }
            return Unauthorized();


        }

        private async Task<string> GenerateJwtToken(Supplier supplier)
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier,supplier.Id.ToString()),
                new Claim(ClaimTypes.Email,supplier.Email)
            };

            var roles = await _userManager.GetRolesAsync(supplier);
            foreach (var role in roles)
            {
                claims.Add(new Claim (ClaimTypes.Role,role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}