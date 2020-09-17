using AutoMapper;
using CP.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CP.API.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]


    public class CustomerController : ControllerBase
    {
        private readonly ICPRepository _repo;
        private readonly IMapper _mapper;
        public CustomerController(ICPRepository repo,IMapper mapper )
        {
            _mapper = mapper;
            _repo = repo;
        }

        

    }
}