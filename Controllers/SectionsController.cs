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
    public class SectionsController : ControllerBase
    {
        private readonly ICPRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _data;

        public SectionsController(ICPRepository repo, IMapper mapper, DataContext data)
        {
            _data = data;
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet("{id}", Name = "GetSection")]
        public async Task<IActionResult> GetSection(int id)
        {
            var sectionFroRepo = await _repo.GetSection(id);
            var sections = _mapper.Map<SectionReturnDTO>(sectionFroRepo);
            if ((sectionFroRepo == null))
                return NotFound();
            return Ok(sectionFroRepo);
        }

        [HttpGet("{CatgeoryId}", Name = "GetSectionWhereCatgeoryId")]
        public async Task<IActionResult> GetSectionWhereCatgeoryId(int CatgeoryId)
        {
            var sectionFroRepo = await _repo.GetSectionWhereCatgeoryId(CatgeoryId);
            var sections = _mapper.Map<SectionReturnDTO>(sectionFroRepo);
            if ((sectionFroRepo == null))
                return NotFound();
            return Ok(sectionFroRepo);
        }


        [HttpGet]
        public async Task<IActionResult> GetSections()
        {
            var sections = await _repo.GetSections();
            var sectionsToReturn = _mapper.Map<IEnumerable<SectionReturnDTO>>(sections);
            return Ok(sectionsToReturn);
        }

        [Authorize(Policy = "EstablishAStore")]
        [HttpPost("{categoryId}")]
        public async Task<IActionResult> CreateSection(int categoryId, SectionRegisterDTO sectionRegisterDTO)
        {
            var getCategory = await _repo.GetCategory(categoryId);
            sectionRegisterDTO.CategoryId = getCategory.CategoryId;
            var section = _mapper.Map<Section>(sectionRegisterDTO);
            _repo.Add(section);
            if (await _repo.SaveAll())
            {
                var sectionToReturn = _mapper.Map<SectionReturnDTO>(section);
                return CreatedAtRoute("GetSection", new { id = section.SectionId }, sectionToReturn);
            }
            throw new Exception("حدث مشكلة في حفظ الفئة الجديده");
        }
    }
}