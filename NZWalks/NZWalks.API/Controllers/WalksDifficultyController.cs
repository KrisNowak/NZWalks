using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {
            var walks = await walkDifficultyRepository.GetAllAsync();

            //var regionsDTO = regions.ToList().Select(region => new Models.DTO.Region() { Id = region.Id, Area = region.Area, Code = region.Code, Lat = region.Lat, Long = region.Long, Population = region.Population, Name = region.Name });
            var walksDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walks);

            // return DTO Regions
            return Ok(walksDTO);
        }


        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            var walk = await walkDifficultyRepository.GetAsync(id);
            if (walk == null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Models.DTO.WalkDifficulty>(walk);

            return Ok(walkDTO);
        }


        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody] AddWalkDifficultyRequest addWalkRequest)
        {
            // Validate The Request
            //if (!ValidateAddRegionAsync(addRegionRequest))
            //{
            //    return BadRequest(ModelState);
            //}

            // Request(DTO) to Domain model
            var walk = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkRequest.Code
            };


            // Pass details to Repository
            walk = await walkDifficultyRepository.AddAsync(walk);


            // Convert back to DTO
            var walkDTO = mapper.Map<Models.DTO.WalkDifficulty>(walk);

            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        //[Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            // Get region from database
            var walk = await walkDifficultyRepository.DeleteAsync(id);

            // If null NotFound
            if (walk == null)
            {
                return NotFound();
            }

            // Convert response back to DTO
            var walkDTO = mapper.Map<Models.DTO.WalkDifficulty>(walk);


            // return Ok response
            return Ok(walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        //[Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] UpdateWalkDifficultyRequest updateRegionRequest)
        {
            // Validate the incoming request
            //if (!ValidateUpdateRegionAsync(updateRegionRequest))
            //{
            //    return BadRequest(ModelState);
            //}

            // Convert DTO to Domain model
            var walk = new Models.Domain.WalkDifficulty()
            {
                Code = updateRegionRequest.Code
            };


            // Update Walk using repository
            walk = await walkDifficultyRepository.UpdateAsync(id, walk);


            // If Null then NotFound
            if (walk == null)
            {
                return NotFound();
            }

            // Convert Domain back to DTO
            var walkDTO = mapper.Map<Models.DTO.WalkDifficulty>(walk);

            // Return Ok response
            return Ok(walkDTO);
        }
    }
}
