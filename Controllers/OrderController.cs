using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CP.API.Data;
using CP.API.Dto;
using CP.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CP.API.Controllers
{
   
    [Route("api/{customerId}/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
         private readonly ICPRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _data;

        public OrderController(ICPRepository repo, IMapper mapper, DataContext data)
        {
            _data = data;
            _mapper = mapper;
            _repo = repo;
        }

        
           [HttpGet("{id}", Name = "GetOrder")]
        public async Task<IActionResult> GetOrder(int customerId, int id)
        {
            if (customerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var ordertFroRepo = await _repo.GetOrder(id);
            var order = _mapper.Map<OrderReturnDTO>(ordertFroRepo);
            if ((ordertFroRepo == null))
                return NotFound();
            return Ok(ordertFroRepo);
        }

         [HttpGet]
        public async Task<IActionResult> GetOrders(int customerId)
        {
             if (customerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var orders = await _repo.GetOrders();
            var ordersToReturn = _mapper.Map<IEnumerable<OrderReturnDTO>>(orders);
            return Ok(ordersToReturn);
        }

         [HttpPost]
        public async Task<IActionResult> CreateOrders(int customerId, OrderRegisterDTO orderRegisterDTO)
        {
            if (customerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var orders = _mapper.Map<Order>(orderRegisterDTO);
           
            _repo.Add(orders);
            if (await _repo.SaveAll())
            {
                var ordersToReturn = _mapper.Map<OrderReturnDTO>(orders);
                return CreatedAtRoute("GetOrder", new { id = orders.OrderId}, ordersToReturn);
            }
            throw new Exception("حدث مشكلة في حفظ الرسالة الجديده");
        }

        [HttpGet("count")]
         public async Task<IActionResult> GetCountOrders()
         {
             var count = await _repo.GetOrderCount();
             return Ok (count);
         }
    }
}