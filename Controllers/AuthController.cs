using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CP.API.Data;
using CP.API.Dto;
using CP.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CP.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }
        
        [HttpPost("Register")]
        public async Task<IActionResult> Register(CustomerRegisterDTO customerRegisterDTO)
        {
            customerRegisterDTO.Email.ToLower();
            if (await _repo.CustomerExists(customerRegisterDTO.Email))
                return BadRequest("UserExisting ");

            var customersToCreate = new Customer
            {
                Email = customerRegisterDTO.Email
            };

            var createUser = await _repo.CustomerRegister(customersToCreate, customerRegisterDTO.Password);
            return StatusCode(202);

        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(CustomerLoginDTO customerLoginDTO)
        {
            //  throw new Exception ("Say Hi NOO!");
            var customerFromRepo = await _repo.CustomerLogin(customerLoginDTO.Email.ToLower(), customerLoginDTO.Password); ;
            if (customerFromRepo == null) return Unauthorized();
            var claims = new[]{
               new Claim(ClaimTypes.NameIdentifier,customerFromRepo.CustomerId.ToString()),
               new Claim(ClaimTypes.Email,customerFromRepo.Email)
           };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var card = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptions = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(24),
                SigningCredentials = card
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptions);
            //var user = _mapper.Map<UserForListDto>(userFromRepo);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                //user
            });
        }

    }
}