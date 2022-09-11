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
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            //var regions = new List<Region>()
            //{
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Wellington",
            //        Code = "WLG",
            //        Area = 227755,
            //        Lat = -1.8822,
            //        Long = 299.88,
            //        Population = 500000
            //    },
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Auckland",
            //        Code = "AUCK",
            //        Area = 227755,
            //        Lat = -1.8822,
            //        Long = 299.88,
            //        Population = 500000
            //    }
            //};

            var regions = await regionRepository.GetAllAsync();

            //var regionsDTO = regions.ToList().Select(region => new Models.DTO.Region() { Id = region.Id, Area = region.Area, Code = region.Code, Lat = region.Lat, Long = region.Long, Population = region.Population, Name = region.Name });
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            // return DTO Regions


            return Ok(regionsDTO);
        }


        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }


        [HttpPost]
        public async Task<IActionResult> AddRegionAsync( [FromBody] AddRegionRequest addRegionRequest)
        {
            // Validate The Request
            if (!ValidateAddRegionAsync(addRegionRequest))
            {
                return BadRequest(ModelState);
            }

            // Request(DTO) to Domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population
            };


            // Pass details to Repository
            region = await regionRepository.AddAsync(region);


            // Convert back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }


        

        [HttpDelete]
        [Route("{id:guid}")]
        //[Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            // Get region from database
            var region = await regionRepository.DeleteAsync(id);

            // If null NotFound
            if (region == null)
            {
                return NotFound();
            }

            // Convert response back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };


            // return Ok response
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        //[Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] UpdateRegionRequest updateRegionRequest)
        {
            // Validate the incoming request
            if (!ValidateUpdateRegionAsync(updateRegionRequest))
            {
                return BadRequest(ModelState);
            }

            // Convert DTO to Domain model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population
            };


            // Update Region using repository
            region = await regionRepository.UpdateAsync(id, region);


            // If Null then NotFound
            if (region == null)
            {
                return NotFound();
            }

            // Convert Domain back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };


            // Return Ok response
            return Ok(regionDTO);
        }


        #region Private methods
        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest req)
        {
            if (req == null)
            {
                ModelState.AddModelError(nameof(req), "AddRegionData is required");
                return false;
            }

            if (string.IsNullOrEmpty(req.Code))
                ModelState.AddModelError(nameof(req.Code), $"{nameof(req.Code)} cannot be empty");

            if (string.IsNullOrEmpty(req.Name))
                ModelState.AddModelError(nameof(req.Name), $"{nameof(req.Name)} cannot be empty");

            if (req.Area <= 0)
                ModelState.AddModelError(nameof(req.Area), $"{nameof(req.Area)} cannot be zero or negative");

            if (req.Lat <= 0)
                ModelState.AddModelError(nameof(req.Lat), $"{nameof(req.Lat)} cannot be zero or negative");

            if (req.Long <= 0)
                ModelState.AddModelError(nameof(req.Long), $"{nameof(req.Long)} cannot be zero or negative");

            if (req.Population < 0)
                ModelState.AddModelError(nameof(req.Population), $"{nameof(req.Population)} cannot be negative");

            return ModelState.ErrorCount == 0;
        }

        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest req)
        {
            if (req == null)
            {
                ModelState.AddModelError(nameof(req), "UpdateRegionData is required");
                return false;
            }

            if (string.IsNullOrEmpty(req.Code))
                ModelState.AddModelError(nameof(req.Code), $"{nameof(req.Code)} cannot be empty");

            if (string.IsNullOrEmpty(req.Name))
                ModelState.AddModelError(nameof(req.Name), $"{nameof(req.Name)} cannot be empty");

            if (req.Area <= 0)
                ModelState.AddModelError(nameof(req.Area), $"{nameof(req.Area)} cannot be zero or negative");

            if (req.Lat <= 0)
                ModelState.AddModelError(nameof(req.Lat), $"{nameof(req.Lat)} cannot be zero or negative");

            if (req.Long <= 0)
                ModelState.AddModelError(nameof(req.Long), $"{nameof(req.Long)} cannot be zero or negative");

            if (req.Population < 0)
                ModelState.AddModelError(nameof(req.Population), $"{nameof(req.Population)} cannot be negative");

            return ModelState.ErrorCount == 0;
        }
        #endregion
    }
}
