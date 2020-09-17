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
    
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
         private readonly ICPRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _data;

        public PaymentController(ICPRepository repo, IMapper mapper, DataContext data)
        {
            _data = data;
            _mapper = mapper;
            _repo = repo;
        }

         [HttpGet("{id}", Name = "GetPayment")]
        public async Task<IActionResult> GetPayment( int id)
        {
          

            var paymentFroRepo = await _repo.GetPayment(id);
            var payment = _mapper.Map<PaymentReturnDTO>(paymentFroRepo);
            if ((paymentFroRepo == null))
                return NotFound();
            return Ok(paymentFroRepo);
        }
       
        
       
        [HttpGet]
        public async Task<IActionResult> GetPayments(int customerId)
        {
             if (customerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var payments = await _repo.GetPayments();
            var paymentsToReturn = _mapper.Map<IEnumerable<PaymentReturnDTO>>(payments);
            return Ok(paymentsToReturn);
        }

         [Authorize(Policy = "EstablishAStore")]
         [HttpPost]
        public async Task<IActionResult> CreatePayments(int customerId, PaymentRegisterDTO paymentRegisterDTO)
        {
            if (customerId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var payments = _mapper.Map<Payment>(paymentRegisterDTO);
            _repo.Add(payments);
            if (await _repo.SaveAll())
            {
                var paymentsToReturn = _mapper.Map<PaymentReturnDTO>(payments);
                return CreatedAtRoute("GetCategory", new { id = paymentsToReturn.PaymentId }, paymentsToReturn);
            }
            throw new Exception("حدث مشكلة في حفظ  طريقة الدفع");
        }
    }
}