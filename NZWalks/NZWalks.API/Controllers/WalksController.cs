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
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDiffRepository;

        public WalksController(IWalkRepository walkRepository, IRegionRepository regionRepository, IWalkDifficultyRepository walkDiffRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDiffRepository = walkDiffRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walks = await walkRepository.GetAllAsync();

            //var regionsDTO = regions.ToList().Select(region => new Models.DTO.Region() { Id = region.Id, Area = region.Area, Code = region.Code, Lat = region.Lat, Long = region.Long, Population = region.Population, Name = region.Name });
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);

            // return DTO Regions
            return Ok(walksDTO);
        }


        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        [Authorize]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walk = await walkRepository.GetAsync(id);
            if (walk == null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);
        }


        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkAsync([FromBody] AddWalkRequest addWalkRequest)
        {
            //Validate The Request
            if (await ValidateAddRegionAsync(addWalkRequest) == false)
            {
                return BadRequest(ModelState);
            }

            // Request(DTO) to Domain model
            var walk = new Models.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };


            // Pass details to Repository
            walk = await walkRepository.AddAsync(walk);


            // Convert back to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            // Get region from database
            var walk = await walkRepository.DeleteAsync(id);

            // If null NotFound
            if (walk == null)
            {
                return NotFound();
            }

            // Convert response back to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);


            // return Ok response
            return Ok(walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequest updateRegionRequest)
        {
            // Validate the incoming request
            if (await ValidateUpdateRegionAsync(updateRegionRequest) == false)
            {
                return BadRequest(ModelState);
            }

            // Convert DTO to Domain model
            var walk = new Models.Domain.Walk()
            {
                Name = updateRegionRequest.Name,
                RegionId = updateRegionRequest.RegionId,
                Length = updateRegionRequest.Length,
                WalkDifficultyId = updateRegionRequest.WalkDifficultyId
            };


            // Update Walk using repository
            walk = await walkRepository.UpdateAsync(id, walk);


            // If Null then NotFound
            if (walk == null)
            {
                return NotFound();
            }

            // Convert Domain back to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            // Return Ok response
            return Ok(walkDTO);
        }


        #region Private mnethoids
        private async Task<bool> ValidateAddRegionAsync(AddWalkRequest request)
        {
            //if (request == null)
            //{
            //    ModelState.AddModelError(nameof(request), "AddWalkData is required");
            //    return false;
            //}

            //if (string.IsNullOrEmpty(request.Name))
            //    ModelState.AddModelError(nameof(request.Name), $"{nameof(request.Name)} cannot be empty");

            //if (request.Length <= 0)
            //    ModelState.AddModelError(nameof(request.Length), $"{nameof(request.Length)} cannot be zero or negative");

            //if (request.RegionId == Guid.Empty)
            //    ModelState.AddModelError(nameof(request.RegionId), $"{nameof(request.RegionId)} cannot be empty");

            var region = await regionRepository.GetAsync(request.RegionId);
            if (region  == null)
                ModelState.AddModelError(nameof(request.RegionId), $"{nameof(request.RegionId)} is invalid");


            if (request.WalkDifficultyId == Guid.Empty)
                ModelState.AddModelError(nameof(request.WalkDifficultyId), $"{nameof(request.WalkDifficultyId)} cannot be empty");

            var wd = await walkDiffRepository.GetAsync(request.WalkDifficultyId);
            if (wd  == null)
                ModelState.AddModelError(nameof(request.WalkDifficultyId), $"{nameof(request.WalkDifficultyId)} is invalid");


            return ModelState.ErrorCount == 0;
        }


        private async Task<bool> ValidateUpdateRegionAsync(UpdateWalkRequest request)
        {
            //if (request == null)
            //{
            //    ModelState.AddModelError(nameof(request), "UpdateWalkData is required");
            //    return false;
            //}

            //if (string.IsNullOrEmpty(request.Name))
            //    ModelState.AddModelError(nameof(request.Name), $"{nameof(request.Name)} cannot be empty");

            //if (request.Length <= 0)
            //    ModelState.AddModelError(nameof(request.Length), $"{nameof(request.Length)} cannot be zero or negative");

            //if (request.RegionId == Guid.Empty)
            //    ModelState.AddModelError(nameof(request.RegionId), $"{nameof(request.RegionId)} cannot be empty");

            var region = await regionRepository.GetAsync(request.RegionId);
            if (region  == null)
                ModelState.AddModelError(nameof(request.RegionId), $"{nameof(request.RegionId)} is invalid");


            if (request.WalkDifficultyId == Guid.Empty)
                ModelState.AddModelError(nameof(request.WalkDifficultyId), $"{nameof(request.WalkDifficultyId)} cannot be empty");

            var wd = await walkDiffRepository.GetAsync(request.WalkDifficultyId);
            if (wd  == null)
                ModelState.AddModelError(nameof(request.WalkDifficultyId), $"{nameof(request.WalkDifficultyId)} is invalid");


            return ModelState.ErrorCount == 0;
        }

        #endregion

    }
}
