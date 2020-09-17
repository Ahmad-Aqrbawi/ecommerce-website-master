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
    public class SupplierController : ControllerBase
    {
        private readonly ICPRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _data;

        public SupplierController(ICPRepository repo, IMapper mapper, DataContext data)
        {
            _data = data;
            _mapper = mapper;
            _repo = repo;
        }
       

        [HttpGet("{id}", Name = "GetSupplier")]
        public async Task<IActionResult> GetSupplier(int id)
        {
            var supplierFroRepo = await _repo.GetSupplier(id);
            var supplier = _mapper.Map<SupplierReturnDTO>(supplierFroRepo);
            return Ok(supplier);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet]
        public async Task<IActionResult> GetSuppliers([FromQuery] SupplierParams supplierParams)
        { 
            var suppliers = await _repo.GetSuppliers(supplierParams);
            var suppliersToReturn = _mapper.Map<IEnumerable<SupplierReturnDTO>>(suppliers);
            Response.AddPagination(suppliers.CurrentPage, suppliers.PageSize, suppliers.TotalCount, suppliers.TotalPages);
            return Ok(suppliersToReturn);
        }



        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)

        {
            var supplierFromRepo = await _repo.GetSupplier(id);
            _repo.Delete(supplierFromRepo);
            if (await _repo.SaveAll())
                return Ok();
            return BadRequest("فشل في حذف");
        }



      [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateSupplier(SupplierRegisterDTO supplierRegisterDTO)
        {
            var supplier = _mapper.Map<Supplier>(supplierRegisterDTO);
            _repo.Add(supplier);
            if (await _repo.SaveAll())
            {
                var supplierToReturn = _mapper.Map<SupplierReturnDTO>(supplier);
                return CreatedAtRoute("GetSupplier", new { id = supplier.Id }, supplierToReturn);
            }
            throw new Exception("حدث مشكلة في حفظ الرسالة الجديده");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, SupplierForUpdateDTO supplierForUpdateDTO)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var userFromRepo = await _repo.GetSupplier(id);
            _mapper.Map(supplierForUpdateDTO, userFromRepo);
            if (await _repo.SaveAll())
                return Ok();

            throw new Exception($"حدثت مشكلة في تعديل بيانات التاجر رقم {id}");

        }

    }
}