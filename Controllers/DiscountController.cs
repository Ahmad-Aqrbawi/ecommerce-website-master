using System;
using System.Collections.Generic;
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
    public class DiscountController : ControllerBase
    {
        private readonly ICPRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _data;

        public DiscountController(ICPRepository repo, IMapper mapper, DataContext data)
        {
            _data = data;
            _mapper = mapper;
            _repo = repo;
        }


        [HttpGet("{id}", Name = "GetDiscount")]
        public async Task<IActionResult> GetDiscount(int id)
        {

            var discountFroRepo = await _repo.GetDiscount(id);
            var post = _mapper.Map<DiscountReturnDTO>(discountFroRepo);
            if ((discountFroRepo == null))
                return NotFound();
            return Ok(discountFroRepo);
        }


        [HttpGet]
        public async Task<IActionResult> GetDiscounts([FromQuery]DiscountParams discountParams)
        {
            var discounts = await _repo.GetDiscounts(discountParams);
            var discountsToReturn = _mapper.Map<IEnumerable<DiscountReturnDTO>>(discounts);
            Response.AddPagination(discounts.CurrentPage, discounts.PageSize, discounts.TotalCount, discounts.TotalPages);

            return Ok(discountsToReturn);
        }

        [Authorize(Policy = "ModerateSupplierRole")]
        [Authorize(Policy = "DiscountCoupons")]
        [HttpPost]
        public async Task<IActionResult> CreateDiscount(DiscountRegisterDTO discountRegisterDTO)
        {
            var discount = _mapper.Map<Discount>(discountRegisterDTO);
            _repo.Add(discount);
            if (await _repo.SaveAll())
            {
                var discountoReturn = _mapper.Map<DiscountReturnDTO>(discount);
                return CreatedAtRoute("GetDiscount", new { id = discount.DiscountId }, discountoReturn);
            }
            throw new Exception("حدث مشكلة في حفظ كود الخصم الجديده");
        }

        [Authorize(Policy = "ModerateSupplierRole")]
        [Authorize(Policy = "DiscountCoupons")]


        [HttpDelete("{id}", Name = "DeleteDiscount")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {

            var discountFroRepo = await _repo.GetDiscount(id);
            _repo.Delete(discountFroRepo);
            if (await _repo.SaveAll())
                return Ok();
            return BadRequest("لم يتم حذف يرومو كود");
        }




    }
}