using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CP.API.Data;
using CP.API.Dto;
using CP.API.Helpers;
using CP.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAMMAPP.API.Helpers;

namespace CP.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ShipperController : ControllerBase
    {
        private readonly ICPRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _data;

        public ShipperController(ICPRepository repo, IMapper mapper, DataContext data)
        {
            _data = data;
            _mapper = mapper;
            _repo = repo;
        }
        
         [HttpGet("{id}", Name = "GetShipper")]
        public async Task<IActionResult> GetShipper( int id)
        {
            var shipperFroRepo = await _repo.GetShipper(id);
            var shipper = _mapper.Map<ShipperReturnDTO>(shipperFroRepo);
            if ((shipperFroRepo == null))
                return NotFound();
            return Ok(shipper);
        }


      [HttpGet]
        public async Task<IActionResult> GetShippers([FromQuery] ShipperParams shipperParams)
        {
            var shippers = await _repo.GetShippers(shipperParams);
            var shippersToReturn = _mapper.Map<IEnumerable<ShipperReturnDTO>>(shippers);
            Response.AddPagination(shippers.CurrentPage, shippers.PageSize, shippers.TotalCount, shippers.TotalPages);
            return Ok(shippersToReturn);
        }

        [Authorize(Policy = "EstablishAStore")]
        [HttpPost]
        public async Task<IActionResult> CreateShippers( ShipperRegisterDTO shipperRegisterDTO)
        {
            var shipper = _mapper.Map<Shipper>(shipperRegisterDTO);
            _repo.Add(shipper);
            if (await _repo.SaveAll())
            {
                var shipperToReturn = _mapper.Map<ShipperReturnDTO>(shipper);
                return CreatedAtRoute("GetShipper", new { id = shipper.ShipperId }, shipperToReturn);
            }
            throw new Exception("حدث مشكلة في اضافة شركة شحن");
        }

         [Authorize(Policy = "EstablishAStore")]
         [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipper(int id)
        {
            var shipperFromRepo = await _repo.GetShipper(id);
            _repo.Delete(shipperFromRepo);
            if (await _repo.SaveAll())
                return Ok();
            return BadRequest("فشل في حذف");
        }
    }
}