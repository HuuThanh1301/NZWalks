﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository) 
        {        
            _mapper = mapper;
            _walkRepository = walkRepository;
        }

        // Create Walk
        // POST: /api/Walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody]AddWalkRequestDto addWalkRequestDto) 
        {
            // Map DTO to Domain Model
            var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);
           
            await _walkRepository.CreateAsync(walkDomainModel);

            // Map Domain model to DTO
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }

        // Get Walks
        // GET: //api/Walks
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy, [FromQuery] bool isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walkDomainModel = await _walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);

            return Ok(_mapper.Map<List<WalkDto>>(walkDomainModel));
        }

        // Get Walk By Id
        // GET: /api/Walks/{id}
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var walkDomainModel = await _walkRepository.GetByIdAsync(id);
            if(walkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));   
        }

        // Update Walk By Id
        // PUT: api/Walks/{id}
        [HttpPut("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);
            walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);

            if(walkDomainModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }

        // Delete a Walk By Id
        // DELETE: /api/Walks/{id}
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepository.DeleteAsync(id);
            if(walkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }
    }
}
