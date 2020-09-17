using System;
using System.Collections.Generic;
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
    public class SocialCommunicationsController : ControllerBase
    {
         private readonly ICPRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _data;

        public SocialCommunicationsController(ICPRepository repo, IMapper mapper, DataContext data)
        {
            _data = data;
            _mapper = mapper;
            _repo = repo;
        }


        [HttpGet("{id}", Name = "GetSocialCommunications")]
        public async Task<IActionResult> GetSocialCommunication(int id)
        {

            var categoryFroRepo = await _repo.GetSocialCommunication(id);
            var post = _mapper.Map<CategoryReturnDTO>(categoryFroRepo);
            if ((categoryFroRepo == null))
                return NotFound();
            return Ok(categoryFroRepo);
        }

         [HttpGet]
        public async Task<IActionResult> GetSocialCommunications()
        {
            var socialCommunications = await _repo.GetCategorys();
            var socialCommunicationsToReturn = _mapper.Map<IEnumerable<CategoryReturnDTO>>(socialCommunications);
            return Ok(socialCommunicationsToReturn);
        }

       
        [HttpPost]
        public async Task<IActionResult> CreateSocialCommunications(SocialCommunicationRegisterDTO socialCommunicationRegisterDTO)
        {
            var socialCommunications = _mapper.Map<SocialCommunication>(socialCommunicationRegisterDTO);
            _repo.Add(socialCommunications);
            if (await _repo.SaveAll())
            {
                var socialCommunicationsToReturn = _mapper.Map<SocialCommunicationReturnDTO>(socialCommunications);
                return CreatedAtRoute("GetSocialCommunications", new { id = socialCommunications.SocialCommunicationId }, socialCommunicationsToReturn);
            }
            throw new Exception("حدث مشكلة في اضافة روابط مواقع التواصل الاجماعي");
        }
    }
}