using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Cryptography.Xml;
using WalksAPI.Data;
using WalksAPI.Models.Domain;
using WalksAPI.Models.DTO;

namespace WalksAPI.Controllers
{
    //https//localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public RegionsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain  = await _dbContext.Regions.ToListAsync();

            var regionsDto = new List<RegionDto>();

            foreach (var region in regionsDomain) 
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl,
                });
            }
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {

            var region = await _dbContext.Regions.FirstOrDefaultAsync(u => u.Id == id);

            if (region == null)
            {
                return NotFound();
            }

            var regionsDto = new RegionDto
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl,
            };

            return Ok(regionsDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {

            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl,
            };

            await _dbContext.Regions.AddAsync(regionDomainModel);
            _dbContext.SaveChangesAsync();

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
                Code = regionDomainModel.Code,
            };

            return CreatedAtAction(nameof(GetById), new {id = regionDto.Id}, regionDto);
        }

        //update Region
        //put : //https//localhost:1234/api/regions/{id}

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomain = await _dbContext.Regions.FirstOrDefaultAsync(u => u.Id == id);

            if (regionDomain == null) {
                return NotFound();
            }

            regionDomain.Code = updateRegionRequestDto.Code;
            regionDomain.Name = updateRegionRequestDto.Name;
            regionDomain.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            _dbContext.SaveChangesAsync();

            var regionDto = new Region
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
                Code = regionDomain.Code,
            };

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) { 
            var regionDomain = await _dbContext.Regions.FirstOrDefaultAsync(u => u.Id == id);

            if (regionDomain == null) {
                return NotFound();            
            }

            //delete 
            _dbContext.Regions.Remove(regionDomain);
            _dbContext.SaveChangesAsync();

            //return deleted region back


            return Ok();

        }
    }
}
