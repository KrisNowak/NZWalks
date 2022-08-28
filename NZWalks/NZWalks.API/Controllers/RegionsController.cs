using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
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
        public async Task<IActionResult> GetAllRegions()
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
    }
}
